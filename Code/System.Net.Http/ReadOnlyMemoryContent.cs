// Decompiled with JetBrains decompiler
// Type: System.Net.Http.ReadOnlyMemoryContent
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
  /// <summary>Provides HTTP content based on a <see cref="T:System.ReadOnlyMemory`1" />.</summary>
  public sealed class ReadOnlyMemoryContent : HttpContent
  {

    #nullable disable
    private readonly ReadOnlyMemory<byte> _content;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.ReadOnlyMemoryContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.ReadOnlyMemoryContent" />.</param>
    public ReadOnlyMemoryContent(ReadOnlyMemory<byte> content) => this._content = content;

    protected override void SerializeToStream(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      stream.Write(this._content.Span);
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) => stream.WriteAsync(this._content).AsTask();

    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return stream.WriteAsync(this._content, cancellationToken).AsTask();
    }

    protected internal override bool TryComputeLength(out long length)
    {
      length = (long) this._content.Length;
      return true;
    }

    protected override Stream CreateContentReadStream(CancellationToken cancellationToken) => (Stream) new ReadOnlyMemoryStream(this._content);

    protected override Task<Stream> CreateContentReadStreamAsync() => Task.FromResult<Stream>((Stream) new ReadOnlyMemoryStream(this._content));


    #nullable disable
    internal override Stream TryCreateContentReadStream() => (Stream) new ReadOnlyMemoryStream(this._content);

    internal override bool AllowDuplex => false;
  }
}
