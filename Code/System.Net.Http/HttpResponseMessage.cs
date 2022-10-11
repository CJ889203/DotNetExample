// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpResponseMessage
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Represents a HTTP response message including the status code and data.</summary>
  public class HttpResponseMessage : IDisposable
  {
    private HttpStatusCode _statusCode;

    #nullable disable
    private HttpResponseHeaders _headers;
    private HttpResponseHeaders _trailingHeaders;
    private string _reasonPhrase;
    private HttpRequestMessage _requestMessage;
    private Version _version;
    private HttpContent _content;
    private bool _disposed;


    #nullable enable
    private static Version DefaultResponseVersion => HttpVersion.Version11;

    /// <summary>Gets or sets the HTTP message version.</summary>
    /// <returns>The HTTP message version. The default is 1.1.</returns>
    public Version Version
    {
      get => this._version;
      set
      {
        if (value == (Version) null)
          throw new ArgumentNullException(nameof (value));
        this.CheckDisposed();
        this._version = value;
      }
    }


    #nullable disable
    internal void SetVersionWithoutValidation(Version value) => this._version = value;


    #nullable enable
    /// <summary>Gets or sets the content of a HTTP response message.</summary>
    /// <returns>The content of the HTTP response message.</returns>
    public HttpContent Content
    {
      get => this._content ?? (this._content = (HttpContent) new EmptyContent());
      [param: AllowNull] set
      {
        this.CheckDisposed();
        if (NetEventSource.Log.IsEnabled())
        {
          if (value == null)
            NetEventSource.ContentNull((object) this);
          else
            NetEventSource.Associate((object) this, (object) value, nameof (Content));
        }
        this._content = value;
      }
    }

    /// <summary>Gets or sets the status code of the HTTP response.</summary>
    /// <returns>The status code of the HTTP response.</returns>
    public HttpStatusCode StatusCode
    {
      get => this._statusCode;
      set
      {
        if (value < (HttpStatusCode) 0 || value > (HttpStatusCode) 999)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.CheckDisposed();
        this._statusCode = value;
      }
    }

    internal void SetStatusCodeWithoutValidation(HttpStatusCode value) => this._statusCode = value;

    /// <summary>Gets or sets the reason phrase which typically is sent by servers together with the status code.</summary>
    /// <returns>The reason phrase sent by the server.</returns>
    public string? ReasonPhrase
    {
      get => this._reasonPhrase != null ? this._reasonPhrase : HttpStatusDescription.Get(this.StatusCode);
      set
      {
        if (value != null && this.ContainsNewLineCharacter(value))
          throw new FormatException(SR.net_http_reasonphrase_format_error);
        this.CheckDisposed();
        this._reasonPhrase = value;
      }
    }


    #nullable disable
    internal void SetReasonPhraseWithoutValidation(string value) => this._reasonPhrase = value;


    #nullable enable
    /// <summary>Gets the collection of HTTP response headers.</summary>
    /// <returns>The collection of HTTP response headers.</returns>
    public HttpResponseHeaders Headers => this._headers ?? (this._headers = new HttpResponseHeaders());

    /// <summary>Gets the collection of trailing headers included in an HTTP response.</summary>
    /// <exception cref="T:System.Net.Http.HttpRequestException">PROTOCOL_ERROR: The HTTP/2 response contains pseudo-headers in the Trailing Headers Frame.</exception>
    /// <returns>The collection of trailing headers in the HTTP response.</returns>
    public HttpResponseHeaders TrailingHeaders => this._trailingHeaders ?? (this._trailingHeaders = new HttpResponseHeaders(true));


    #nullable disable
    internal void StoreReceivedTrailingHeaders(HttpResponseHeaders headers)
    {
      if (this._trailingHeaders == null)
        this._trailingHeaders = headers;
      else
        this._trailingHeaders.AddHeaders((HttpHeaders) headers);
    }


    #nullable enable
    /// <summary>Gets or sets the request message which led to this response message.</summary>
    /// <returns>The request message which led to this response message.</returns>
    public HttpRequestMessage? RequestMessage
    {
      get => this._requestMessage;
      set
      {
        this.CheckDisposed();
        if (value != null && NetEventSource.Log.IsEnabled())
          NetEventSource.Associate((object) this, (object) value, nameof (RequestMessage));
        this._requestMessage = value;
      }
    }

    /// <summary>Gets a value that indicates if the HTTP response was successful.</summary>
    /// <returns>
    /// <see langword="true" /> if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 200-299; otherwise, <see langword="false" />.</returns>
    public bool IsSuccessStatusCode => this._statusCode >= HttpStatusCode.OK && this._statusCode <= (HttpStatusCode) 299;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpResponseMessage" /> class.</summary>
    public HttpResponseMessage()
      : this(HttpStatusCode.OK)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpResponseMessage" /> class with a specific <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" />.</summary>
    /// <param name="statusCode">The status code of the HTTP response.</param>
    public HttpResponseMessage(HttpStatusCode statusCode)
    {
      this._statusCode = statusCode >= (HttpStatusCode) 0 && statusCode <= (HttpStatusCode) 999 ? statusCode : throw new ArgumentOutOfRangeException(nameof (statusCode));
      this._version = HttpResponseMessage.DefaultResponseVersion;
    }

    /// <summary>Throws an exception if the <see cref="P:System.Net.Http.HttpResponseMessage.IsSuccessStatusCode" /> property for the HTTP response is <see langword="false" />.</summary>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The HTTP response is unsuccessful.</exception>
    /// <returns>The HTTP response message if the call is successful.</returns>
    public HttpResponseMessage EnsureSuccessStatusCode()
    {
      if (!this.IsSuccessStatusCode)
        throw new HttpRequestException(SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_message_not_success_statuscode, (object) (int) this._statusCode, (object) this.ReasonPhrase), (Exception) null, new HttpStatusCode?(this._statusCode));
      return this;
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string representation of the current object.</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("StatusCode: ");
      sb.Append((int) this._statusCode);
      sb.Append(", ReasonPhrase: '");
      sb.Append(this.ReasonPhrase ?? "<null>");
      sb.Append("', Version: ");
      sb.Append((object) this._version);
      sb.Append(", Content: ");
      sb.Append(this._content == null ? "<null>" : this._content.GetType().ToString());
      sb.AppendLine(", Headers:");
      HeaderUtilities.DumpHeaders(sb, (HttpHeaders) this._headers, (HttpHeaders) this._content?.Headers);
      if (this._trailingHeaders != null)
      {
        sb.AppendLine(", Trailing Headers:");
        HeaderUtilities.DumpHeaders(sb, (HttpHeaders) this._trailingHeaders);
      }
      return sb.ToString();
    }


    #nullable disable
    private bool ContainsNewLineCharacter(string value)
    {
      foreach (char ch in value)
      {
        switch (ch)
        {
          case '\n':
          case '\r':
            return true;
          default:
            continue;
        }
      }
      return false;
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpResponseMessage" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._disposed)
        return;
      this._disposed = true;
      if (this._content == null)
        return;
      this._content.Dispose();
    }

    /// <summary>Releases the unmanaged resources and disposes of unmanaged resources used by the <see cref="T:System.Net.Http.HttpResponseMessage" />.</summary>
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
  }
}
