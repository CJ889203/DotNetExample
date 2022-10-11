// Decompiled with JetBrains decompiler
// Type: System.Net.Http.StreamContent
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Provides HTTP content based on a stream.</summary>
  public class StreamContent : HttpContent
  {

    #nullable disable
    private Stream _content;
    private int _bufferSize;
    private bool _contentConsumed;
    private long _start;


    #nullable enable
    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.StreamContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.StreamContent" />.</param>
    public StreamContent(Stream content)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      this.InitializeContent(content, 0);
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.StreamContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.StreamContent" />.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer for the <see cref="T:System.Net.Http.StreamContent" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="bufferSize" /> was less than or equal to zero.</exception>
    public StreamContent(Stream content, int bufferSize)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      this.InitializeContent(content, bufferSize);
    }


    #nullable disable
    [MemberNotNull("_content")]
    private void InitializeContent(Stream content, int bufferSize)
    {
      this._content = content;
      this._bufferSize = bufferSize;
      if (content.CanSeek)
        this._start = content.Position;
      if (!NetEventSource.Log.IsEnabled())
        return;
      NetEventSource.Associate((object) this, (object) content, nameof (InitializeContent));
    }


    #nullable enable
    /// <summary>Serializes the multipart HTTP content to a stream.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (for example, the channel binding token). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    protected override void SerializeToStream(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      this.PrepareContent();
      StreamToStreamCopy.Copy(this._content, stream, this._bufferSize, !this._content.CanSeek);
    }

    /// <summary>Serialize the HTTP content to a stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) => this.SerializeToStreamAsyncCore(stream, new CancellationToken());

    /// <summary>Serialize the HTTP content to a stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return !(this.GetType() == typeof (StreamContent)) ? base.SerializeToStreamAsync(stream, context, cancellationToken) : this.SerializeToStreamAsyncCore(stream, cancellationToken);
    }


    #nullable disable
    private Task SerializeToStreamAsyncCore(Stream stream, CancellationToken cancellationToken)
    {
      this.PrepareContent();
      return StreamToStreamCopy.CopyAsync(this._content, stream, this._bufferSize, !this._content.CanSeek, cancellationToken);
    }


    #nullable enable
    /// <summary>Determines whether the stream content has a valid length in bytes.</summary>
    /// <param name="length">The length in bytes of the stream content.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="length" /> is a valid length; otherwise, <see langword="false" />.</returns>
    protected internal override bool TryComputeLength(out long length)
    {
      if (this._content.CanSeek)
      {
        length = this._content.Length - this._start;
        return true;
      }
      length = 0L;
      return false;
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.StreamContent" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this._content.Dispose();
      base.Dispose(disposing);
    }

    /// <summary>Returns the HTTP stream as a read-only stream.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The HTTP content stream.</returns>
    protected override Stream CreateContentReadStream(CancellationToken cancellationToken) => (Stream) new StreamContent.ReadOnlyStream(this._content);

    /// <summary>Returns the HTTP stream as a read-only stream as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task<Stream> CreateContentReadStreamAsync() => Task.FromResult<Stream>((Stream) new StreamContent.ReadOnlyStream(this._content));


    #nullable disable
    internal override Stream TryCreateContentReadStream() => !(this.GetType() == typeof (StreamContent)) ? (Stream) null : (Stream) new StreamContent.ReadOnlyStream(this._content);

    internal override bool AllowDuplex => false;

    private void PrepareContent()
    {
      if (this._contentConsumed)
      {
        if (!this._content.CanSeek)
          throw new InvalidOperationException(SR.net_http_content_stream_already_read);
        this._content.Position = this._start;
      }
      this._contentConsumed = true;
    }

    private sealed class ReadOnlyStream : DelegatingStream
    {
      public override bool CanWrite => false;

      public override int WriteTimeout
      {
        get => throw new NotSupportedException(SR.net_http_content_readonly_stream);
        set => throw new NotSupportedException(SR.net_http_content_readonly_stream);
      }

      public ReadOnlyStream(Stream innerStream)
        : base(innerStream)
      {
      }

      public override void Flush() => throw new NotSupportedException(SR.net_http_content_readonly_stream);

      public override Task FlushAsync(CancellationToken cancellationToken) => throw new NotSupportedException(SR.net_http_content_readonly_stream);

      public override void SetLength(long value) => throw new NotSupportedException(SR.net_http_content_readonly_stream);

      public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException(SR.net_http_content_readonly_stream);

      public override void Write(ReadOnlySpan<byte> buffer) => throw new NotSupportedException(SR.net_http_content_readonly_stream);

      public override void WriteByte(byte value) => throw new NotSupportedException(SR.net_http_content_readonly_stream);

      public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        throw new NotSupportedException(SR.net_http_content_readonly_stream);
      }

      public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default (CancellationToken))
      {
        throw new NotSupportedException(SR.net_http_content_readonly_stream);
      }
    }
  }
}
