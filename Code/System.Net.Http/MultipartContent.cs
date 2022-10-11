// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartContent
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Provides a collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized using the multipart/* content type specification.</summary>
  public class MultipartContent : HttpContent, IEnumerable<HttpContent>, IEnumerable
  {

    #nullable disable
    private readonly List<HttpContent> _nestedContent;
    private readonly string _boundary;

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartContent" /> class.</summary>
    public MultipartContent()
      : this("mixed", MultipartContent.GetDefaultBoundary())
    {
    }


    #nullable enable
    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartContent" /> class.</summary>
    /// <param name="subtype">The subtype of the multipart content.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="subtype" /> was <see langword="null" /> or contains only white space characters.</exception>
    public MultipartContent(string subtype)
      : this(subtype, MultipartContent.GetDefaultBoundary())
    {
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartContent" /> class.</summary>
    /// <param name="subtype">The subtype of the multipart content.</param>
    /// <param name="boundary">The boundary string for the multipart content.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="subtype" /> was <see langword="null" /> or an empty string.
    /// 
    /// The <paramref name="boundary" /> was <see langword="null" /> or contains only white space characters.
    /// 
    /// -or-
    /// 
    /// The <paramref name="boundary" /> ends with a space character.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The length of the <paramref name="boundary" /> was greater than 70.</exception>
    public MultipartContent(string subtype, string boundary)
    {
      if (string.IsNullOrWhiteSpace(subtype))
        throw new ArgumentException(SR.net_http_argument_empty_string, nameof (subtype));
      MultipartContent.ValidateBoundary(boundary);
      this._boundary = boundary;
      string str = boundary;
      if (!str.StartsWith('"'))
        str = "\"" + str + "\"";
      this.Headers.ContentType = new MediaTypeHeaderValue("multipart/" + subtype)
      {
        Parameters = {
          new NameValueHeaderValue(nameof (boundary), str)
        }
      };
      this._nestedContent = new List<HttpContent>();
    }


    #nullable disable
    private static void ValidateBoundary(string boundary)
    {
      if (string.IsNullOrWhiteSpace(boundary))
        throw new ArgumentException(SR.net_http_argument_empty_string, nameof (boundary));
      if (boundary.Length > 70)
        throw new ArgumentOutOfRangeException(nameof (boundary), (object) boundary, SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_content_field_too_long, (object) 70));
      if (boundary.EndsWith(' '))
        throw new ArgumentException(SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_headers_invalid_value, (object) boundary), nameof (boundary));
      foreach (char ch in boundary)
      {
        if (('0' > ch || ch > '9') && ('a' > ch || ch > 'z') && ('A' > ch || ch > 'Z') && !"'()+_,-./:=? ".Contains(ch))
          throw new ArgumentException(SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_headers_invalid_value, (object) boundary), nameof (boundary));
      }
    }

    private static string GetDefaultBoundary() => Guid.NewGuid().ToString();


    #nullable enable
    /// <summary>Add multipart HTTP content to a collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized using the multipart/* content type specification.</summary>
    /// <param name="content">The HTTP content to add to the collection.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> was <see langword="null" />.</exception>
    public virtual void Add(HttpContent content)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      this._nestedContent.Add(content);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.MultipartContent" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        foreach (HttpContent httpContent in this._nestedContent)
          httpContent.Dispose();
        this._nestedContent.Clear();
      }
      base.Dispose(disposing);
    }

    /// <summary>Returns an enumerator that iterates through the collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized using the multipart/* content type specification.</summary>
    /// <returns>An object that can be used to iterate through the collection.</returns>
    public IEnumerator<HttpContent> GetEnumerator() => (IEnumerator<HttpContent>) this._nestedContent.GetEnumerator();


    #nullable disable
    /// <summary>The explicit implementation of the <see cref="M:System.Net.Http.MultipartContent.GetEnumerator" /> method.</summary>
    /// <returns>An object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._nestedContent.GetEnumerator();


    #nullable enable
    /// <summary>Gets or sets a callback that decode response header values.</summary>
    /// <returns>The header encoding selector callback to decode the value for the specified response header name, or <see langword="null" /> to indicate the default behavior.</returns>
    public System.Net.Http.HeaderEncodingSelector<HttpContent>? HeaderEncodingSelector { get; set; }

    /// <summary>Serializes the multipart HTTP content to a stream.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (for example, the channel binding token). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    protected override void SerializeToStream(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      try
      {
        MultipartContent.WriteToStream(stream, "--" + this._boundary + "\r\n");
        for (int index = 0; index < this._nestedContent.Count; ++index)
        {
          HttpContent content = this._nestedContent[index];
          this.SerializeHeadersToStream(stream, content, index != 0);
          content.CopyTo(stream, context, cancellationToken);
        }
        MultipartContent.WriteToStream(stream, "\r\n--" + this._boundary + "--\r\n");
      }
      catch (Exception ex)
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) this, (object) ex, nameof (SerializeToStream));
        throw;
      }
    }

    /// <summary>Serialize the multipart HTTP content to a stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) => this.SerializeToStreamAsyncCore(stream, context, new CancellationToken());

    /// <summary>Serialize the multipart HTTP content to a stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return !(this.GetType() == typeof (MultipartContent)) ? base.SerializeToStreamAsync(stream, context, cancellationToken) : this.SerializeToStreamAsyncCore(stream, context, cancellationToken);
    }


    #nullable disable
    private protected async Task SerializeToStreamAsyncCore(
      Stream stream,
      TransportContext context,
      CancellationToken cancellationToken)
    {
      MultipartContent thisOrContextObject = this;
      try
      {
        ConfiguredValueTaskAwaitable valueTaskAwaitable = MultipartContent.EncodeStringToStreamAsync(stream, "--" + thisOrContextObject._boundary + "\r\n", cancellationToken).ConfigureAwait(false);
        await valueTaskAwaitable;
        MemoryStream output = new MemoryStream();
        for (int contentIndex = 0; contentIndex < thisOrContextObject._nestedContent.Count; ++contentIndex)
        {
          HttpContent content = thisOrContextObject._nestedContent[contentIndex];
          output.SetLength(0L);
          thisOrContextObject.SerializeHeadersToStream((Stream) output, content, contentIndex != 0);
          output.Position = 0L;
          ConfiguredTaskAwaitable configuredTaskAwaitable = output.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
          await configuredTaskAwaitable;
          configuredTaskAwaitable = content.CopyToAsync(stream, context, cancellationToken).ConfigureAwait(false);
          await configuredTaskAwaitable;
          content = (HttpContent) null;
        }
        valueTaskAwaitable = MultipartContent.EncodeStringToStreamAsync(stream, "\r\n--" + thisOrContextObject._boundary + "--\r\n", cancellationToken).ConfigureAwait(false);
        await valueTaskAwaitable;
        output = (MemoryStream) null;
      }
      catch (Exception ex)
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) thisOrContextObject, (object) ex, nameof (SerializeToStreamAsyncCore));
        throw;
      }
    }


    #nullable enable
    /// <summary>Serializes the HTTP content to a stream using the multipart/* encoding.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The HTTP content stream that represents the multipart/* encoded HTTP content.</returns>
    protected override Stream CreateContentReadStream(CancellationToken cancellationToken) => this.CreateContentReadStreamAsyncCore(false, cancellationToken).GetAwaiter().GetResult();

    /// <summary>Serializes the HTTP content to a stream using the multipart/* encoding as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task<Stream> CreateContentReadStreamAsync() => this.CreateContentReadStreamAsyncCore(true, CancellationToken.None).AsTask();

    /// <summary>Serializes the HTTP content to a stream using the multipart/* encoding as an asynchronous operation.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task<Stream> CreateContentReadStreamAsync(
      CancellationToken cancellationToken)
    {
      return !(this.GetType() == typeof (MultipartContent)) ? base.CreateContentReadStreamAsync(cancellationToken) : this.CreateContentReadStreamAsyncCore(true, cancellationToken).AsTask();
    }


    #nullable disable
    private async ValueTask<Stream> CreateContentReadStreamAsyncCore(
      bool async,
      CancellationToken cancellationToken)
    {
      MultipartContent thisOrContextObject = this;
      try
      {
        Stream[] streams = new Stream[2 + thisOrContextObject._nestedContent.Count * 2];
        int streamIndex = 0;
        streams[streamIndex++] = MultipartContent.EncodeStringToNewStream("--" + thisOrContextObject._boundary + "\r\n");
        for (int contentIndex = 0; contentIndex < thisOrContextObject._nestedContent.Count; ++contentIndex)
        {
          cancellationToken.ThrowIfCancellationRequested();
          HttpContent content = thisOrContextObject._nestedContent[contentIndex];
          streams[streamIndex++] = thisOrContextObject.EncodeHeadersToNewStream(content, contentIndex != 0);
          Stream stream1;
          if (async)
          {
            Stream stream2 = content.TryReadAsStream();
            if (stream2 == null)
              stream2 = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            stream1 = stream2;
          }
          else
            stream1 = content.ReadAsStream(cancellationToken);
          if (stream1 == null)
            stream1 = (Stream) new MemoryStream();
          if (!stream1.CanSeek)
          {
            Stream readStreamAsyncCore;
            if (async)
            {
              // ISSUE: reference to a compiler-generated method
              readStreamAsyncCore = await thisOrContextObject.\u003C\u003En__0().ConfigureAwait(false);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              readStreamAsyncCore = thisOrContextObject.\u003C\u003En__1(cancellationToken);
            }
            return readStreamAsyncCore;
          }
          streams[streamIndex++] = stream1;
        }
        streams[streamIndex] = MultipartContent.EncodeStringToNewStream("\r\n--" + thisOrContextObject._boundary + "--\r\n");
        return (Stream) new MultipartContent.ContentReadStream(streams);
      }
      catch (Exception ex)
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) thisOrContextObject, (object) ex, nameof (CreateContentReadStreamAsyncCore));
        throw;
      }
    }

    private void SerializeHeadersToStream(Stream stream, HttpContent content, bool writeDivider)
    {
      if (writeDivider)
      {
        MultipartContent.WriteToStream(stream, "\r\n--");
        MultipartContent.WriteToStream(stream, this._boundary);
        MultipartContent.WriteToStream(stream, "\r\n");
      }
      foreach (KeyValuePair<string, HeaderStringValues> keyValuePair in content.Headers.NonValidated)
      {
        System.Net.Http.HeaderEncodingSelector<HttpContent> encodingSelector = this.HeaderEncodingSelector;
        Encoding encoding = (encodingSelector != null ? encodingSelector(keyValuePair.Key, content) : (Encoding) null) ?? HttpRuleParser.DefaultHttpEncoding;
        MultipartContent.WriteToStream(stream, keyValuePair.Key);
        MultipartContent.WriteToStream(stream, ": ");
        string content1 = string.Empty;
        foreach (string content2 in keyValuePair.Value)
        {
          MultipartContent.WriteToStream(stream, content1);
          MultipartContent.WriteToStream(stream, content2, encoding);
          content1 = ", ";
        }
        MultipartContent.WriteToStream(stream, "\r\n");
      }
      MultipartContent.WriteToStream(stream, "\r\n");
    }

    private static ValueTask EncodeStringToStreamAsync(
      Stream stream,
      string input,
      CancellationToken cancellationToken)
    {
      byte[] bytes = HttpRuleParser.DefaultHttpEncoding.GetBytes(input);
      return stream.WriteAsync(new ReadOnlyMemory<byte>(bytes), cancellationToken);
    }

    private static Stream EncodeStringToNewStream(string input) => (Stream) new MemoryStream(HttpRuleParser.DefaultHttpEncoding.GetBytes(input), false);

    private Stream EncodeHeadersToNewStream(HttpContent content, bool writeDivider)
    {
      MemoryStream newStream = new MemoryStream();
      this.SerializeHeadersToStream((Stream) newStream, content, writeDivider);
      newStream.Position = 0L;
      return (Stream) newStream;
    }

    internal override bool AllowDuplex => false;


    #nullable enable
    /// <summary>Determines whether the HTTP multipart content has a valid length in bytes.</summary>
    /// <param name="length">The length in bytes of the HHTP content.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="length" /> is a valid length; otherwise, <see langword="false" />.</returns>
    protected internal override bool TryComputeLength(out long length)
    {
      long num1 = (long) (2 + this._boundary.Length + 2);
      if (this._nestedContent.Count > 1)
        num1 += (long) ((this._nestedContent.Count - 1) * (4 + this._boundary.Length + 2));
      foreach (HttpContent context in this._nestedContent)
      {
        foreach (KeyValuePair<string, HeaderStringValues> keyValuePair in context.Headers.NonValidated)
        {
          num1 += (long) (keyValuePair.Key.Length + 2);
          System.Net.Http.HeaderEncodingSelector<HttpContent> encodingSelector = this.HeaderEncodingSelector;
          Encoding encoding = (encodingSelector != null ? encodingSelector(keyValuePair.Key, context) : (Encoding) null) ?? HttpRuleParser.DefaultHttpEncoding;
          int num2 = 0;
          foreach (string s in keyValuePair.Value)
          {
            num1 += (long) encoding.GetByteCount(s);
            ++num2;
          }
          if (num2 > 1)
            num1 += (long) ((num2 - 1) * 2);
          num1 += 2L;
        }
        num1 += 2L;
        long length1;
        if (!context.TryComputeLength(out length1))
        {
          length = 0L;
          return false;
        }
        num1 += length1;
      }
      long num3 = num1 + (long) (4 + this._boundary.Length + 2 + 2);
      length = num3;
      return true;
    }


    #nullable disable
    private static void WriteToStream(Stream stream, string content) => MultipartContent.WriteToStream(stream, content, HttpRuleParser.DefaultHttpEncoding);

    private static unsafe void WriteToStream(Stream stream, string content, Encoding encoding)
    {
      int maxByteCount = encoding.GetMaxByteCount(content.Length);
      byte[] array = (byte[]) null;
      // ISSUE: untyped stack allocation
      Span<byte> bytes1 = maxByteCount > 1024 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxByteCount)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(1024)), 1024);
      try
      {
        int bytes2 = encoding.GetBytes((ReadOnlySpan<char>) content, bytes1);
        stream.Write((ReadOnlySpan<byte>) bytes1.Slice(0, bytes2));
      }
      finally
      {
        if (array != null)
          ArrayPool<byte>.Shared.Return(array);
      }
    }

    private sealed class ContentReadStream : Stream
    {
      private readonly Stream[] _streams;
      private readonly long _length;
      private int _next;
      private Stream _current;
      private long _position;

      internal ContentReadStream(Stream[] streams)
      {
        this._streams = streams;
        foreach (Stream stream in streams)
          this._length += stream.Length;
      }

      protected override void Dispose(bool disposing)
      {
        if (!disposing)
          return;
        foreach (Stream stream in this._streams)
          stream.Dispose();
      }

      public override async ValueTask DisposeAsync()
      {
        Stream[] streamArray = this._streams;
        for (int index = 0; index < streamArray.Length; ++index)
          await streamArray[index].DisposeAsync().ConfigureAwait(false);
        streamArray = (Stream[]) null;
      }

      public override bool CanRead => true;

      public override bool CanSeek => true;

      public override bool CanWrite => false;

      public override int Read(byte[] buffer, int offset, int count)
      {
        Stream.ValidateBufferArguments(buffer, offset, count);
        if (count == 0)
          return 0;
        int num;
        while (true)
        {
          if (this._current != null)
          {
            num = this._current.Read(buffer, offset, count);
            if (num == 0)
              this._current = (Stream) null;
            else
              break;
          }
          if (this._next < this._streams.Length)
            this._current = this._streams[this._next++];
          else
            goto label_7;
        }
        this._position += (long) num;
        return num;
label_7:
        return 0;
      }

      public override int Read(Span<byte> buffer)
      {
        if (buffer.Length == 0)
          return 0;
        int num;
        while (true)
        {
          if (this._current != null)
          {
            num = this._current.Read(buffer);
            if (num == 0)
              this._current = (Stream) null;
            else
              break;
          }
          if (this._next < this._streams.Length)
            this._current = this._streams[this._next++];
          else
            goto label_7;
        }
        this._position += (long) num;
        return num;
label_7:
        return 0;
      }

      public override Task<int> ReadAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        Stream.ValidateBufferArguments(buffer, offset, count);
        return this.ReadAsyncPrivate(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
      }

      public override ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default (CancellationToken))
      {
        return this.ReadAsyncPrivate(buffer, cancellationToken);
      }

      public override IAsyncResult BeginRead(
        byte[] array,
        int offset,
        int count,
        AsyncCallback asyncCallback,
        object asyncState)
      {
        return TaskToApm.Begin((Task) this.ReadAsync(array, offset, count, CancellationToken.None), asyncCallback, asyncState);
      }

      public override int EndRead(IAsyncResult asyncResult) => TaskToApm.End<int>(asyncResult);

      public async ValueTask<int> ReadAsyncPrivate(
        Memory<byte> buffer,
        CancellationToken cancellationToken)
      {
        MultipartContent.ContentReadStream contentReadStream1 = this;
        if (buffer.Length == 0)
          return 0;
        int num1;
        while (true)
        {
          if (contentReadStream1._current != null)
          {
            num1 = await contentReadStream1._current.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
            if (num1 == 0)
              contentReadStream1._current = (Stream) null;
            else
              break;
          }
          if (contentReadStream1._next < contentReadStream1._streams.Length)
          {
            MultipartContent.ContentReadStream contentReadStream2 = contentReadStream1;
            Stream[] streams = contentReadStream1._streams;
            MultipartContent.ContentReadStream contentReadStream3 = contentReadStream1;
            int next = contentReadStream1._next;
            int num2 = next + 1;
            contentReadStream3._next = num2;
            int index = next;
            Stream stream = streams[index];
            contentReadStream2._current = stream;
          }
          else
            goto label_7;
        }
        contentReadStream1._position += (long) num1;
        return num1;
label_7:
        return 0;
      }

      public override long Position
      {
        get => this._position;
        set
        {
          if (value < 0L)
            throw new ArgumentOutOfRangeException(nameof (value));
          long num = 0;
          for (int index1 = 0; index1 < this._streams.Length; ++index1)
          {
            Stream stream = this._streams[index1];
            long length = stream.Length;
            if (value < num + length)
            {
              this._current = stream;
              int index2 = index1 + 1;
              this._next = index2;
              stream.Position = value - num;
              for (; index2 < this._streams.Length; ++index2)
                this._streams[index2].Position = 0L;
              this._position = value;
              return;
            }
            num += length;
          }
          this._current = (Stream) null;
          this._next = this._streams.Length;
          this._position = value;
        }
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
        switch (origin)
        {
          case SeekOrigin.Begin:
            this.Position = offset;
            break;
          case SeekOrigin.Current:
            this.Position += offset;
            break;
          case SeekOrigin.End:
            this.Position = this._length + offset;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (origin));
        }
        return this.Position;
      }

      public override long Length => this._length;

      public override void Flush()
      {
      }

      public override void SetLength(long value) => throw new NotSupportedException();

      public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

      public override void Write(ReadOnlySpan<byte> buffer) => throw new NotSupportedException();

      public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        throw new NotSupportedException();
      }

      public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default (CancellationToken))
      {
        throw new NotSupportedException();
      }
    }
  }
}
