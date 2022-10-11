// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpContent
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>A base class representing an HTTP entity body and content headers.</summary>
  public abstract class HttpContent : IDisposable
  {

    #nullable disable
    private HttpContentHeaders _headers;
    private MemoryStream _bufferedContent;
    private object _contentReadStream;
    private bool _disposed;
    private bool _canCalculateLength;
    internal static readonly Encoding DefaultStringEncoding = Encoding.UTF8;


    #nullable enable
    /// <summary>Gets the HTTP content headers as defined in RFC 2616.</summary>
    /// <returns>The content headers as defined in RFC 2616.</returns>
    public HttpContentHeaders Headers
    {
      get
      {
        if (this._headers == null)
          this._headers = new HttpContentHeaders(this);
        return this._headers;
      }
    }

    private bool IsBuffered => this._bufferedContent != null;


    #nullable disable
    internal bool TryGetBuffer(out ArraySegment<byte> buffer)
    {
      if (this._bufferedContent != null)
        return this._bufferedContent.TryGetBuffer(out buffer);
      buffer = new ArraySegment<byte>();
      return false;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpContent" /> class.</summary>
    protected HttpContent()
    {
      if (NetEventSource.Log.IsEnabled())
        NetEventSource.Info((object) this, (FormattableString) null, ".ctor");
      this._canCalculateLength = true;
    }


    #nullable enable
    /// <summary>Serialize the HTTP content to a string as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<string> ReadAsStringAsync() => this.ReadAsStringAsync(CancellationToken.None);

    /// <summary>Serialize the HTTP content to a string as an asynchronous operation.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<string> ReadAsStringAsync(CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      return HttpContent.WaitAndReturnAsync<HttpContent, string>(this.LoadIntoBufferAsync(cancellationToken), this, (Func<HttpContent, string>) (s => s.ReadBufferedContentAsString()));
    }


    #nullable disable
    private string ReadBufferedContentAsString()
    {
      if (this._bufferedContent.Length == 0L)
        return string.Empty;
      ArraySegment<byte> buffer;
      if (!this.TryGetBuffer(out buffer))
        buffer = new ArraySegment<byte>(this._bufferedContent.ToArray());
      return HttpContent.ReadBufferAsString(buffer, this.Headers);
    }

    internal static string ReadBufferAsString(ArraySegment<byte> buffer, HttpContentHeaders headers)
    {
      Encoding encoding = (Encoding) null;
      int preambleLength = -1;
      string charSet = headers.ContentType?.CharSet;
      if (charSet != null)
      {
        try
        {
          encoding = charSet.Length <= 2 || charSet[0] != '"' || charSet[charSet.Length - 1] != '"' ? Encoding.GetEncoding(charSet) : Encoding.GetEncoding(charSet.Substring(1, charSet.Length - 2));
          preambleLength = HttpContent.GetPreambleLength(buffer, encoding);
        }
        catch (ArgumentException ex)
        {
          throw new InvalidOperationException(SR.net_http_content_invalid_charset, (Exception) ex);
        }
      }
      if (encoding == null && !HttpContent.TryDetectEncoding(buffer, out encoding, out preambleLength))
      {
        encoding = HttpContent.DefaultStringEncoding;
        preambleLength = 0;
      }
      return encoding.GetString(buffer.Array, buffer.Offset + preambleLength, buffer.Count - preambleLength);
    }


    #nullable enable
    /// <summary>Serialize the HTTP content to a byte array as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<byte[]> ReadAsByteArrayAsync() => this.ReadAsByteArrayAsync(CancellationToken.None);

    /// <summary>Serialize the HTTP content to a byte array as an asynchronous operation.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      return HttpContent.WaitAndReturnAsync<HttpContent, byte[]>(this.LoadIntoBufferAsync(cancellationToken), this, (Func<HttpContent, byte[]>) (s => s.ReadBufferedContentAsByteArray()));
    }


    #nullable disable
    internal byte[] ReadBufferedContentAsByteArray() => this._bufferedContent.ToArray();


    #nullable enable
    /// <summary>Serializes the HTTP content and returns a stream that represents the content.</summary>
    /// <returns>The stream that represents the HTTP content.</returns>
    public Stream ReadAsStream() => this.ReadAsStream(CancellationToken.None);

    /// <summary>Serializes the HTTP content and returns a stream that represents the content.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The stream that represents the HTTP content.</returns>
    public Stream ReadAsStream(CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      if (this._contentReadStream == null)
      {
        ArraySegment<byte> buffer;
        Stream stream = this.TryGetBuffer(out buffer) ? (Stream) new MemoryStream(buffer.Array, buffer.Offset, buffer.Count, false) : this.CreateContentReadStream(cancellationToken);
        this._contentReadStream = (object) stream;
        return stream;
      }
      return this._contentReadStream is Stream contentReadStream ? contentReadStream : throw new HttpRequestException(SR.net_http_content_read_as_stream_has_task);
    }

    /// <summary>Serialize the HTTP content and return a stream that represents the content as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<Stream> ReadAsStreamAsync() => this.ReadAsStreamAsync(CancellationToken.None);

    /// <summary>Serialize the HTTP content and return a stream that represents the content as an asynchronous operation.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      if (this._contentReadStream == null)
      {
        ArraySegment<byte> buffer;
        Task<Stream> task = this.TryGetBuffer(out buffer) ? Task.FromResult<Stream>((Stream) new MemoryStream(buffer.Array, buffer.Offset, buffer.Count, false)) : this.CreateContentReadStreamAsync(cancellationToken);
        this._contentReadStream = (object) task;
        return task;
      }
      if (this._contentReadStream is Task<Stream> contentReadStream)
        return contentReadStream;
      Task<Stream> task1 = Task.FromResult<Stream>((Stream) this._contentReadStream);
      this._contentReadStream = (object) task1;
      return task1;
    }


    #nullable disable
    internal Stream TryReadAsStream()
    {
      this.CheckDisposed();
      if (this._contentReadStream == null)
      {
        ArraySegment<byte> buffer;
        Stream stream = this.TryGetBuffer(out buffer) ? (Stream) new MemoryStream(buffer.Array, buffer.Offset, buffer.Count, false) : this.TryCreateContentReadStream();
        this._contentReadStream = (object) stream;
        return stream;
      }
      if (this._contentReadStream is Stream contentReadStream1)
        return contentReadStream1;
      Task<Stream> contentReadStream2 = (Task<Stream>) this._contentReadStream;
      return contentReadStream2.Status != TaskStatus.RanToCompletion ? (Stream) null : contentReadStream2.Result;
    }


    #nullable enable
    /// <summary>Serialize the HTTP content to a stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected abstract Task SerializeToStreamAsync(Stream stream, TransportContext? context);

    /// <summary>When overridden in a derived class, serializes the HTTP content to a stream. Otherwise, throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (for example, the channel binding token). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.NotSupportedException">The method is not overridden in the derived class.</exception>
    protected virtual void SerializeToStream(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      throw new NotSupportedException(SR.Format(SR.net_http_missing_sync_implementation, (object) this.GetType(), (object) nameof (HttpContent), (object) nameof (SerializeToStream)));
    }

    /// <summary>Serialize the HTTP content to a stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected virtual Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return this.SerializeToStreamAsync(stream, context);
    }

    internal virtual bool AllowDuplex => true;

    /// <summary>Serializes the HTTP content into a stream of bytes and copies it to <paramref name="stream" />.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (for example, the channel binding token). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="stream" /> was <see langword="null" />.</exception>
    public void CopyTo(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      try
      {
        ArraySegment<byte> buffer;
        if (this.TryGetBuffer(out buffer))
          stream.Write(buffer.Array, buffer.Offset, buffer.Count);
        else
          this.SerializeToStream(stream, context, cancellationToken);
      }
      catch (Exception ex) when (HttpContent.StreamCopyExceptionNeedsWrapping(ex))
      {
        throw HttpContent.GetStreamCopyException(ex);
      }
    }

    /// <summary>Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the <paramref name="stream" /> parameter.</summary>
    /// <param name="stream">The target stream.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task CopyToAsync(Stream stream) => this.CopyToAsync(stream, CancellationToken.None);

    /// <summary>Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the <paramref name="stream" /> parameter.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task CopyToAsync(Stream stream, CancellationToken cancellationToken) => this.CopyToAsync(stream, (TransportContext) null, cancellationToken);

    /// <summary>Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the <paramref name="stream" /> parameter.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task CopyToAsync(Stream stream, TransportContext? context) => this.CopyToAsync(stream, context, CancellationToken.None);

    /// <summary>Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the <paramref name="stream" /> parameter.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task CopyToAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      try
      {
        return WaitAsync(this.InternalCopyToAsync(stream, context, cancellationToken));
      }
      catch (Exception ex) when (HttpContent.StreamCopyExceptionNeedsWrapping(ex))
      {
        return Task.FromException(HttpContent.GetStreamCopyException(ex));
      }


      #nullable disable
      static async Task WaitAsync(ValueTask copyTask)
      {
        try
        {
          await copyTask.ConfigureAwait(false);
        }
        catch (Exception ex) when (HttpContent.StreamCopyExceptionNeedsWrapping(ex))
        {
          throw HttpContent.WrapStreamCopyException(ex);
        }
      }
    }

    internal ValueTask InternalCopyToAsync(
      Stream stream,
      TransportContext context,
      CancellationToken cancellationToken)
    {
      ArraySegment<byte> buffer;
      if (this.TryGetBuffer(out buffer))
        return stream.WriteAsync((ReadOnlyMemory<byte>) buffer, cancellationToken);
      Task streamAsync = this.SerializeToStreamAsync(stream, context, cancellationToken);
      this.CheckTaskNotNull(streamAsync);
      return new ValueTask(streamAsync);
    }

    internal void LoadIntoBuffer(long maxBufferSize, CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      MemoryStream tempBuffer;
      Exception error;
      if (!this.CreateTemporaryBuffer(maxBufferSize, out tempBuffer, out error))
        return;
      if (tempBuffer == null)
        throw error;
      using (cancellationToken.Register((Action<object>) (s => ((HttpContent) s).Dispose()), (object) this))
      {
        try
        {
          this.SerializeToStream((Stream) tempBuffer, (TransportContext) null, cancellationToken);
          tempBuffer.Seek(0L, SeekOrigin.Begin);
          this._bufferedContent = tempBuffer;
        }
        catch (Exception ex)
        {
          if (NetEventSource.Log.IsEnabled())
            NetEventSource.Error((object) this, (object) ex, nameof (LoadIntoBuffer));
          if (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
            throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
          if (HttpContent.StreamCopyExceptionNeedsWrapping(ex))
            throw HttpContent.GetStreamCopyException(ex);
          throw;
        }
      }
    }


    #nullable enable
    /// <summary>Serialize the HTTP content to a memory buffer as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task LoadIntoBufferAsync() => this.LoadIntoBufferAsync((long) int.MaxValue);

    /// <summary>Serialize the HTTP content to a memory buffer as an asynchronous operation.</summary>
    /// <param name="maxBufferSize">The maximum size, in bytes, of the buffer to use.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task LoadIntoBufferAsync(long maxBufferSize) => this.LoadIntoBufferAsync(maxBufferSize, CancellationToken.None);


    #nullable disable
    internal Task LoadIntoBufferAsync(CancellationToken cancellationToken) => this.LoadIntoBufferAsync((long) int.MaxValue, cancellationToken);

    internal Task LoadIntoBufferAsync(long maxBufferSize, CancellationToken cancellationToken)
    {
      this.CheckDisposed();
      MemoryStream tempBuffer;
      Exception error;
      if (!this.CreateTemporaryBuffer(maxBufferSize, out tempBuffer, out error))
        return Task.CompletedTask;
      if (tempBuffer == null)
        return Task.FromException(error);
      try
      {
        Task streamAsync = this.SerializeToStreamAsync((Stream) tempBuffer, (TransportContext) null, cancellationToken);
        this.CheckTaskNotNull(streamAsync);
        return this.LoadIntoBufferAsyncCore(streamAsync, tempBuffer);
      }
      catch (Exception ex) when (HttpContent.StreamCopyExceptionNeedsWrapping(ex))
      {
        return Task.FromException(HttpContent.GetStreamCopyException(ex));
      }
    }

    private async Task LoadIntoBufferAsyncCore(
      Task serializeToStreamTask,
      MemoryStream tempBuffer)
    {
      HttpContent thisOrContextObject = this;
      try
      {
        await serializeToStreamTask.ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        tempBuffer.Dispose();
        Exception streamCopyException = HttpContent.GetStreamCopyException(ex);
        if (streamCopyException != ex)
          throw streamCopyException;
        throw;
      }
      try
      {
        tempBuffer.Seek(0L, SeekOrigin.Begin);
        thisOrContextObject._bufferedContent = tempBuffer;
      }
      catch (Exception ex)
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) thisOrContextObject, (object) ex, nameof (LoadIntoBufferAsyncCore));
        throw;
      }
    }


    #nullable enable
    /// <summary>Serializes the HTTP content to a memory stream.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The output memory stream which contains the serialized HTTP content.</returns>
    protected virtual Stream CreateContentReadStream(CancellationToken cancellationToken)
    {
      this.LoadIntoBuffer((long) int.MaxValue, cancellationToken);
      return (Stream) this._bufferedContent;
    }

    /// <summary>Serialize the HTTP content to a memory stream as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected virtual Task<Stream> CreateContentReadStreamAsync() => HttpContent.WaitAndReturnAsync<HttpContent, Stream>(this.LoadIntoBufferAsync(), this, (Func<HttpContent, Stream>) (s => (Stream) s._bufferedContent));

    /// <summary>Serializes the HTTP content to a memory stream as an asynchronous operation.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected virtual Task<Stream> CreateContentReadStreamAsync(
      CancellationToken cancellationToken)
    {
      return this.CreateContentReadStreamAsync();
    }


    #nullable disable
    internal virtual Stream TryCreateContentReadStream() => (Stream) null;


    #nullable enable
    /// <summary>Determines whether the HTTP content has a valid length in bytes.</summary>
    /// <param name="length">The length in bytes of the HTTP content.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="length" /> is a valid length; otherwise, <see langword="false" />.</returns>
    protected internal abstract bool TryComputeLength(out long length);


    #nullable disable
    internal long? GetComputedOrBufferLength()
    {
      this.CheckDisposed();
      if (this.IsBuffered)
        return new long?(this._bufferedContent.Length);
      if (this._canCalculateLength)
      {
        long length = 0;
        if (this.TryComputeLength(out length))
          return new long?(length);
        this._canCalculateLength = false;
      }
      return new long?();
    }

    private bool CreateTemporaryBuffer(
      long maxBufferSize,
      out MemoryStream tempBuffer,
      out Exception error)
    {
      if (maxBufferSize > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (maxBufferSize), (object) maxBufferSize, SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_content_buffersize_limit, (object) int.MaxValue));
      if (this.IsBuffered)
      {
        tempBuffer = (MemoryStream) null;
        error = (Exception) null;
        return false;
      }
      tempBuffer = this.CreateMemoryStream(maxBufferSize, out error);
      return true;
    }

    private MemoryStream CreateMemoryStream(long maxBufferSize, out Exception error)
    {
      error = (Exception) null;
      long? contentLength = this.Headers.ContentLength;
      if (!contentLength.HasValue)
        return (MemoryStream) new HttpContent.LimitMemoryStream((int) maxBufferSize, 0);
      long? nullable = contentLength;
      long num = maxBufferSize;
      if (!(nullable.GetValueOrDefault() > num & nullable.HasValue))
        return (MemoryStream) new HttpContent.LimitMemoryStream((int) maxBufferSize, (int) contentLength.Value);
      error = (Exception) new HttpRequestException(SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_content_buffersize_exceeded, (object) maxBufferSize));
      return (MemoryStream) null;
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpContent" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._disposed)
        return;
      this._disposed = true;
      if (this._contentReadStream != null)
      {
        if (!(this._contentReadStream is Stream stream))
          stream = !(this._contentReadStream is Task<Stream> contentReadStream) || contentReadStream.Status != TaskStatus.RanToCompletion ? (Stream) null : contentReadStream.Result;
        stream?.Dispose();
        this._contentReadStream = (object) null;
      }
      if (!this.IsBuffered)
        return;
      this._bufferedContent.Dispose();
    }

    /// <summary>Releases the unmanaged resources and disposes of the managed resources used by the <see cref="T:System.Net.Http.HttpContent" />.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().ToString());
    }

    private void CheckTaskNotNull(Task task)
    {
      if (task == null)
      {
        InvalidOperationException message = new InvalidOperationException(SR.net_http_content_no_task_returned);
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) this, (object) message, nameof (CheckTaskNotNull));
        throw message;
      }
    }

    internal static bool StreamCopyExceptionNeedsWrapping(Exception e) => e is IOException || e is ObjectDisposedException;

    private static Exception GetStreamCopyException(Exception originalException) => !HttpContent.StreamCopyExceptionNeedsWrapping(originalException) ? originalException : HttpContent.WrapStreamCopyException(originalException);

    internal static Exception WrapStreamCopyException(Exception e) => (Exception) new HttpRequestException(SR.net_http_content_stream_copy_error, e);

    private static int GetPreambleLength(ArraySegment<byte> buffer, Encoding encoding)
    {
      byte[] array = buffer.Array;
      int offset = buffer.Offset;
      int count = buffer.Count;
      switch (encoding.CodePage)
      {
        case 1200:
          return count < 2 || array[offset] != byte.MaxValue || array[offset + 1] != (byte) 254 ? 0 : 2;
        case 1201:
          return count < 2 || array[offset] != (byte) 254 || array[offset + 1] != byte.MaxValue ? 0 : 2;
        case 12000:
          return count < 4 || array[offset] != byte.MaxValue || array[offset + 1] != (byte) 254 || array[offset + 2] != (byte) 0 || array[offset + 3] != (byte) 0 ? 0 : 4;
        case 65001:
          return count < 3 || array[offset] != (byte) 239 || array[offset + 1] != (byte) 187 || array[offset + 2] != (byte) 191 ? 0 : 3;
        default:
          byte[] preamble = encoding.GetPreamble();
          return !HttpContent.BufferHasPrefix(buffer, preamble) ? 0 : preamble.Length;
      }
    }

    private static bool TryDetectEncoding(
      ArraySegment<byte> buffer,
      [NotNullWhen(true)] out Encoding encoding,
      out int preambleLength)
    {
      byte[] array = buffer.Array;
      int offset = buffer.Offset;
      int count = buffer.Count;
      if (count >= 2)
      {
        switch ((int) array[offset] << 8 | (int) array[offset + 1])
        {
          case 61371:
            if (count >= 3 && array[offset + 2] == (byte) 191)
            {
              encoding = Encoding.UTF8;
              preambleLength = 3;
              return true;
            }
            break;
          case 65279:
            encoding = Encoding.BigEndianUnicode;
            preambleLength = 2;
            return true;
          case 65534:
            if (count >= 4 && array[offset + 2] == (byte) 0 && array[offset + 3] == (byte) 0)
            {
              encoding = Encoding.UTF32;
              preambleLength = 4;
            }
            else
            {
              encoding = Encoding.Unicode;
              preambleLength = 2;
            }
            return true;
        }
      }
      encoding = (Encoding) null;
      preambleLength = 0;
      return false;
    }

    private static bool BufferHasPrefix(ArraySegment<byte> buffer, byte[] prefix)
    {
      byte[] array = buffer.Array;
      if (prefix == null || array == null || prefix.Length > buffer.Count || prefix.Length == 0)
        return false;
      int index = 0;
      int offset = buffer.Offset;
      while (index < prefix.Length)
      {
        if ((int) prefix[index] != (int) array[offset])
          return false;
        ++index;
        ++offset;
      }
      return true;
    }

    private static async Task<TResult> WaitAndReturnAsync<TState, TResult>(
      Task waitTask,
      TState state,
      Func<TState, TResult> returnFunc)
    {
      await waitTask.ConfigureAwait(false);
      return returnFunc(state);
    }

    private static Exception CreateOverCapacityException(int maxBufferSize) => (Exception) new HttpRequestException(SR.Format(SR.net_http_content_buffersize_exceeded, (object) maxBufferSize));

    internal sealed class LimitMemoryStream : MemoryStream
    {
      private readonly int _maxSize;

      public LimitMemoryStream(int maxSize, int capacity)
        : base(capacity)
      {
        this._maxSize = maxSize;
      }

      public byte[] GetSizedBuffer()
      {
        ArraySegment<byte> buffer;
        return !this.TryGetBuffer(out buffer) || buffer.Offset != 0 || buffer.Count != buffer.Array.Length ? this.ToArray() : buffer.Array;
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
        this.CheckSize(count);
        base.Write(buffer, offset, count);
      }

      public override void WriteByte(byte value)
      {
        this.CheckSize(1);
        base.WriteByte(value);
      }

      public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        this.CheckSize(count);
        return base.WriteAsync(buffer, offset, count, cancellationToken);
      }

      public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken)
      {
        this.CheckSize(buffer.Length);
        return base.WriteAsync(buffer, cancellationToken);
      }

      public override IAsyncResult BeginWrite(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback,
        object state)
      {
        this.CheckSize(count);
        return base.BeginWrite(buffer, offset, count, callback, state);
      }

      public override void EndWrite(IAsyncResult asyncResult) => base.EndWrite(asyncResult);

      public override Task CopyToAsync(
        Stream destination,
        int bufferSize,
        CancellationToken cancellationToken)
      {
        ArraySegment<byte> buffer;
        if (!this.TryGetBuffer(out buffer))
          return base.CopyToAsync(destination, bufferSize, cancellationToken);
        Stream.ValidateCopyToArguments(destination, bufferSize);
        long position = this.Position;
        long length = this.Length;
        this.Position = length;
        long count = length - position;
        return destination.WriteAsync(buffer.Array, (int) ((long) buffer.Offset + position), (int) count, cancellationToken);
      }

      private void CheckSize(int countToAdd)
      {
        if ((long) this._maxSize - this.Length < (long) countToAdd)
          throw HttpContent.CreateOverCapacityException(this._maxSize);
      }
    }

    internal sealed class LimitArrayPoolWriteStream : Stream
    {
      private readonly int _maxBufferSize;
      private byte[] _buffer;
      private int _length;

      public LimitArrayPoolWriteStream(int maxBufferSize)
        : this(maxBufferSize, 256L)
      {
      }

      public LimitArrayPoolWriteStream(int maxBufferSize, long capacity)
      {
        if (capacity < 256L)
          capacity = 256L;
        else if (capacity > (long) maxBufferSize)
          throw HttpContent.CreateOverCapacityException(maxBufferSize);
        this._maxBufferSize = maxBufferSize;
        this._buffer = ArrayPool<byte>.Shared.Rent((int) capacity);
      }

      protected override void Dispose(bool disposing)
      {
        ArrayPool<byte>.Shared.Return(this._buffer);
        this._buffer = (byte[]) null;
        base.Dispose(disposing);
      }

      public ArraySegment<byte> GetBuffer() => new ArraySegment<byte>(this._buffer, 0, this._length);

      public byte[] ToArray()
      {
        byte[] dst = new byte[this._length];
        Buffer.BlockCopy((Array) this._buffer, 0, (Array) dst, 0, this._length);
        return dst;
      }

      private void EnsureCapacity(int value)
      {
        if ((uint) value > (uint) this._maxBufferSize)
          throw HttpContent.CreateOverCapacityException(this._maxBufferSize);
        if (value <= this._buffer.Length)
          return;
        this.Grow(value);
      }

      private void Grow(int value)
      {
        byte[] buffer = this._buffer;
        this._buffer = (byte[]) null;
        uint val2 = (uint) (2 * buffer.Length);
        byte[] dst = ArrayPool<byte>.Shared.Rent((long) val2 > (long) Array.MaxLength ? Math.Max(value, Array.MaxLength) : Math.Max(value, (int) val2));
        Buffer.BlockCopy((Array) buffer, 0, (Array) dst, 0, this._length);
        ArrayPool<byte>.Shared.Return(buffer);
        this._buffer = dst;
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
        this.EnsureCapacity(this._length + count);
        Buffer.BlockCopy((Array) buffer, offset, (Array) this._buffer, this._length, count);
        this._length += count;
      }

      public override void Write(ReadOnlySpan<byte> buffer)
      {
        this.EnsureCapacity(this._length + buffer.Length);
        buffer.CopyTo(new Span<byte>(this._buffer, this._length, buffer.Length));
        this._length += buffer.Length;
      }

      public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        this.Write(buffer, offset, count);
        return Task.CompletedTask;
      }

      public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default (CancellationToken))
      {
        this.Write(buffer.Span);
        return new ValueTask();
      }

      public override IAsyncResult BeginWrite(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback asyncCallback,
        object asyncState)
      {
        return TaskToApm.Begin(this.WriteAsync(buffer, offset, count, CancellationToken.None), asyncCallback, asyncState);
      }

      public override void EndWrite(IAsyncResult asyncResult) => TaskToApm.End(asyncResult);

      public override void WriteByte(byte value)
      {
        int num = this._length + 1;
        this.EnsureCapacity(num);
        this._buffer[this._length] = value;
        this._length = num;
      }

      public override void Flush()
      {
      }

      public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;

      public override long Length => (long) this._length;

      public override bool CanWrite => true;

      public override bool CanRead => false;

      public override bool CanSeek => false;

      public override long Position
      {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
      }

      public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

      public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

      public override void SetLength(long value) => throw new NotSupportedException();
    }
  }
}
