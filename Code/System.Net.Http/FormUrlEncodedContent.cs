// Decompiled with JetBrains decompiler
// Type: System.Net.Http.FormUrlEncodedContent
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>A container for name/value tuples encoded using application/x-www-form-urlencoded MIME type.</summary>
  public class FormUrlEncodedContent : ByteArrayContent
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.FormUrlEncodedContent" /> class with a specific collection of name/value pairs.</summary>
    /// <param name="nameValueCollection">A collection of name/value pairs.</param>
    public FormUrlEncodedContent(
      IEnumerable<KeyValuePair<
      #nullable disable
      string, string>> nameValueCollection)
      : base(FormUrlEncodedContent.GetContentByteArray(nameValueCollection))
    {
      this.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
    }

    private static byte[] GetContentByteArray(
      IEnumerable<KeyValuePair<string, string>> nameValueCollection)
    {
      if (nameValueCollection == null)
        throw new ArgumentNullException(nameof (nameValueCollection));
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> nameValue in nameValueCollection)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append('&');
        stringBuilder.Append(FormUrlEncodedContent.Encode(nameValue.Key));
        stringBuilder.Append('=');
        stringBuilder.Append(FormUrlEncodedContent.Encode(nameValue.Value));
      }
      return HttpRuleParser.DefaultHttpEncoding.GetBytes(stringBuilder.ToString());
    }

    private static string Encode(string data) => string.IsNullOrEmpty(data) ? string.Empty : Uri.EscapeDataString(data).Replace("%20", "+");


    #nullable enable
    /// <summary>Serialize and write all name/value tuples provided in the constructor to an HTTP content stream as an asynchronous operation.</summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport, like channel binding token. This parameter may be <see langword="null" />.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return !(this.GetType() == typeof (FormUrlEncodedContent)) ? base.SerializeToStreamAsync(stream, context, cancellationToken) : this.SerializeToStreamAsyncCore(stream, cancellationToken);
    }


    #nullable disable
    internal override Stream TryCreateContentReadStream() => !(this.GetType() == typeof (FormUrlEncodedContent)) ? (Stream) null : (Stream) this.CreateMemoryStreamForByteArray();
  }
}
