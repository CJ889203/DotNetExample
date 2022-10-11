// Decompiled with JetBrains decompiler
// Type: System.Net.Http.ByteArrayContent
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.IO;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Provides HTTP content based on a byte array.</summary>
  public class ByteArrayContent : HttpContent
  {

    #nullable disable
    private readonly byte[] _content;
    private readonly int _offset;
    private readonly int _count;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.ByteArrayContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.ByteArrayContent" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> parameter is <see langword="null" />.</exception>
    public ByteArrayContent(byte[] content)
    {
      this._content = content != null ? content : throw new ArgumentNullException(nameof (content));
      this._count = content.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.ByteArrayContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.ByteArrayContent" />.</param>
    /// <param name="offset">The offset, in bytes, in the <paramref name="content" /> parameter used to initialize the <see cref="T:System.Net.Http.ByteArrayContent" />.</param>
    /// <param name="count">The number of bytes in the <paramref name="content" /> starting from the <paramref name="offset" /> parameter used to initialize the <see cref="T:System.Net.Http.ByteArrayContent" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> parameter is less than zero.
    /// 
    /// -or-
    /// 
    /// The <paramref name="offset" /> parameter is greater than the length of content specified by the <paramref name="content" /> parameter.
    /// 
    /// -or-
    /// 
    /// The <paramref name="count" /> parameter is less than zero.
    /// 
    /// -or-
    /// 
    /// The <paramref name="count" /> parameter is greater than the length of content specified by the <paramref name="content" /> parameter - minus the <paramref name="offset" /> parameter.</exception>
    public ByteArrayContent(byte[] content, int offset, int count)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (offset < 0 || offset > content.Length)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0 || count > content.Length - offset)
        throw new ArgumentOutOfRangeException(nameof (count));
      this._content = content;
      this._offset = offset;
      this._count = count;
    }

    /// <summary>Serializes and writes the byte array provided in the constructor to an HTTP content stream.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Optional information about the transport, like the channel binding token. This parameter can be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    protected override void SerializeToStream(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      stream.Write(this._content, this._offset, this._count);
    }

    /// <summary>Serialize and write the byte array provided in the constructor to an HTTP content stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport, like channel binding token. This parameter may be <see langword="null" />.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) => this.SerializeToStreamAsyncCore(stream, new CancellationToken());

    /// <summary>Serialize and write the byte array provided in the constructor to an HTTP content stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport, like channel binding token. This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return !(this.GetType() == typeof (ByteArrayContent)) ? base.SerializeToStreamAsync(stream, context, cancellationToken) : this.SerializeToStreamAsyncCore(stream, cancellationToken);
    }


    #nullable disable
    private protected Task SerializeToStreamAsyncCore(
      Stream stream,
      CancellationToken cancellationToken)
    {
      return stream.WriteAsync(this._content, this._offset, this._count, cancellationToken);
    }


    #nullable enable
    /// <summary>Determines whether a byte array has a valid length in bytes.</summary>
    /// <param name="length">The length in bytes of the byte array.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="length" /> is a valid length; otherwise, <see langword="false" />.</returns>
    protected internal override bool TryComputeLength(out long length)
    {
      length = (long) this._count;
      return true;
    }

    /// <summary>Creates an HTTP content stream for reading. It uses the memory from the <see cref="T:System.Net.Http.ByteArrayContent" /> as a backing store.</summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The HTTP content stream.</returns>
    protected override Stream CreateContentReadStream(CancellationToken cancellationToken) => (Stream) this.CreateMemoryStreamForByteArray();

    /// <summary>Creates an HTTP content stream as an asynchronous operation for reading whose backing store is memory from the <see cref="T:System.Net.Http.ByteArrayContent" />.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task<Stream> CreateContentReadStreamAsync() => Task.FromResult<Stream>((Stream) this.CreateMemoryStreamForByteArray());


    #nullable disable
    internal override Stream TryCreateContentReadStream() => !(this.GetType() == typeof (ByteArrayContent)) ? (Stream) null : (Stream) this.CreateMemoryStreamForByteArray();

    internal MemoryStream CreateMemoryStreamForByteArray() => new MemoryStream(this._content, this._offset, this._count, false);

    internal override bool AllowDuplex => false;
  }
}
