// Decompiled with JetBrains decompiler
// Type: System.Net.Http.StringContent
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Provides HTTP content based on a string.</summary>
  public class StringContent : ByteArrayContent
  {
    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.StringContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.StringContent" />.</param>
    public StringContent(string content)
      : this(content, (Encoding) null, (string) null)
    {
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.StringContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.StringContent" />.</param>
    /// <param name="encoding">The encoding to use for the content.</param>
    public StringContent(string content, Encoding? encoding)
      : this(content, encoding, (string) null)
    {
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.StringContent" /> class.</summary>
    /// <param name="content">The content used to initialize the <see cref="T:System.Net.Http.StringContent" />.</param>
    /// <param name="encoding">The encoding to use for the content.</param>
    /// <param name="mediaType">The media type to use for the content.</param>
    public StringContent(string content, Encoding? encoding, string? mediaType)
      : base(StringContent.GetContentByteArray(content, encoding))
    {
      this.Headers.ContentType = new MediaTypeHeaderValue(mediaType == null ? "text/plain" : mediaType)
      {
        CharSet = encoding == null ? HttpContent.DefaultStringEncoding.WebName : encoding.WebName
      };
    }


    #nullable disable
    private static byte[] GetContentByteArray(string content, Encoding encoding)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (encoding == null)
        encoding = HttpContent.DefaultStringEncoding;
      return encoding.GetBytes(content);
    }


    #nullable enable
    /// <summary>Serialize and write the string provided in the constructor to an HTTP content stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport, like channel binding token. This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return !(this.GetType() == typeof (StringContent)) ? base.SerializeToStreamAsync(stream, context, cancellationToken) : this.SerializeToStreamAsyncCore(stream, cancellationToken);
    }


    #nullable disable
    internal override Stream TryCreateContentReadStream() => !(this.GetType() == typeof (StringContent)) ? (Stream) null : (Stream) this.CreateMemoryStreamForByteArray();
  }
}
