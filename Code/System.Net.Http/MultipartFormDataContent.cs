// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartFormDataContent
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Provides a container for content encoded using multipart/form-data MIME type.</summary>
  public class MultipartFormDataContent : MultipartContent
  {
    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartFormDataContent" /> class.</summary>
    public MultipartFormDataContent()
      : base("form-data")
    {
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.MultipartFormDataContent" /> class.</summary>
    /// <param name="boundary">The boundary string for the multipart form data content.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="boundary" /> was <see langword="null" /> or contains only white space characters.
    /// 
    /// -or-
    /// 
    /// The <paramref name="boundary" /> ends with a space character.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The length of the <paramref name="boundary" /> was greater than 70.</exception>
    public MultipartFormDataContent(string boundary)
      : base("form-data", boundary)
    {
    }

    /// <summary>Add HTTP content to a collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized to multipart/form-data MIME type.</summary>
    /// <param name="content">The HTTP content to add to the collection.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> was <see langword="null" />.</exception>
    public override void Add(HttpContent content)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (content.Headers.ContentDisposition == null)
        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
      base.Add(content);
    }

    /// <summary>Add HTTP content to a collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized to multipart/form-data MIME type.</summary>
    /// <param name="content">The HTTP content to add to the collection.</param>
    /// <param name="name">The name for the HTTP content to add.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="name" /> was <see langword="null" /> or contains only white space characters.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> was <see langword="null" />.</exception>
    public void Add(HttpContent content, string name)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(SR.net_http_argument_empty_string, nameof (name));
      this.AddInternal(content, name, (string) null);
    }

    /// <summary>Add HTTP content to a collection of <see cref="T:System.Net.Http.HttpContent" /> objects that get serialized to multipart/form-data MIME type.</summary>
    /// <param name="content">The HTTP content to add to the collection.</param>
    /// <param name="name">The name for the HTTP content to add.</param>
    /// <param name="fileName">The file name for the HTTP content to add to the collection.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="name" /> was <see langword="null" /> or contains only white space characters.
    /// 
    /// -or-
    /// 
    /// The <paramref name="fileName" /> was <see langword="null" /> or contains only white space characters.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="content" /> was <see langword="null" />.</exception>
    public void Add(HttpContent content, string name, string fileName)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(SR.net_http_argument_empty_string, nameof (name));
      if (string.IsNullOrWhiteSpace(fileName))
        throw new ArgumentException(SR.net_http_argument_empty_string, nameof (fileName));
      this.AddInternal(content, name, fileName);
    }


    #nullable disable
    private void AddInternal(HttpContent content, string name, string fileName)
    {
      if (content.Headers.ContentDisposition == null)
        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
          Name = name,
          FileName = fileName,
          FileNameStar = fileName
        };
      base.Add(content);
    }


    #nullable enable
    /// <summary>Serialize and write the content provided in the constructor to an HTTP content stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport, like channel binding token. This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return !(this.GetType() == typeof (MultipartFormDataContent)) ? base.SerializeToStreamAsync(stream, context, cancellationToken) : this.SerializeToStreamAsyncCore(stream, context, cancellationToken);
    }
  }
}
