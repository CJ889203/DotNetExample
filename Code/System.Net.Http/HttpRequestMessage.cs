// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpRequestMessage
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Represents a HTTP request message.</summary>
  public class HttpRequestMessage : IDisposable
  {
    private int _sendStatus;

    #nullable disable
    private HttpMethod _method;
    private Uri _requestUri;
    private HttpRequestHeaders _headers;
    private Version _version;
    private HttpVersionPolicy _versionPolicy;
    private HttpContent _content;
    private bool _disposed;
    private HttpRequestOptions _options;


    #nullable enable
    internal static Version DefaultRequestVersion => HttpVersion.Version11;

    internal static HttpVersionPolicy DefaultVersionPolicy => HttpVersionPolicy.RequestVersionOrLower;

    /// <summary>Gets or sets the HTTP message version.</summary>
    /// <returns>The HTTP message version. The default value is <c>1.1</c>, unless you're targeting .NET Core 2.1 or 2.2. In that case, the default value is <c>2.0</c>.</returns>
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

    /// <summary>Gets or sets the policy that determines how <see cref="System.Net.Http.HttpRequestMessage.Version" /> is interpreted and how the final HTTP version is negotiated with the server.</summary>
    /// <returns>The HttpVersionPolicy used when the HTTP connection is established.</returns>
    public HttpVersionPolicy VersionPolicy
    {
      get => this._versionPolicy;
      set
      {
        this.CheckDisposed();
        this._versionPolicy = value;
      }
    }

    /// <summary>Gets or sets the contents of the HTTP message.</summary>
    /// <returns>The content of a message.</returns>
    public HttpContent? Content
    {
      get => this._content;
      set
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

    /// <summary>Gets or sets the HTTP method used by the HTTP request message.</summary>
    /// <returns>The HTTP method used by the request message. The default is the GET method.</returns>
    public HttpMethod Method
    {
      get => this._method;
      set
      {
        if (value == (HttpMethod) null)
          throw new ArgumentNullException(nameof (value));
        this.CheckDisposed();
        this._method = value;
      }
    }

    /// <summary>Gets or sets the <see cref="T:System.Uri" /> used for the HTTP request.</summary>
    /// <returns>The <see cref="T:System.Uri" /> used for the HTTP request.</returns>
    public Uri? RequestUri
    {
      get => this._requestUri;
      set
      {
        this.CheckDisposed();
        this._requestUri = value;
      }
    }

    /// <summary>Gets the collection of HTTP request headers.</summary>
    /// <returns>The collection of HTTP request headers.</returns>
    public HttpRequestHeaders Headers => this._headers ?? (this._headers = new HttpRequestHeaders());

    internal bool HasHeaders => this._headers != null;

    /// <summary>Gets a set of properties for the HTTP request.</summary>
    /// <returns>Returns <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
    [Obsolete("HttpRequestMessage.Properties has been deprecated. Use Options instead.")]
    public IDictionary<string, object?> Properties => (IDictionary<string, object>) this.Options;

    /// <summary>Gets the collection of options to configure the HTTP request.</summary>
    public HttpRequestOptions Options => this._options ?? (this._options = new HttpRequestOptions());

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestMessage" /> class.</summary>
    public HttpRequestMessage()
      : this(HttpMethod.Get, (Uri) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestMessage" /> class with an HTTP method and a request <see cref="T:System.Uri" />.</summary>
    /// <param name="method">The HTTP method.</param>
    /// <param name="requestUri">The <see cref="T:System.Uri" /> to request.</param>
    public HttpRequestMessage(HttpMethod method, Uri? requestUri)
    {
      this._method = method ?? throw new ArgumentNullException(nameof (method));
      this._requestUri = requestUri;
      this._version = HttpRequestMessage.DefaultRequestVersion;
      this._versionPolicy = HttpRequestMessage.DefaultVersionPolicy;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestMessage" /> class with an HTTP method and a request <see cref="T:System.Uri" />.</summary>
    /// <param name="method">The HTTP method.</param>
    /// <param name="requestUri">A string that represents the request  <see cref="T:System.Uri" />.</param>
    public HttpRequestMessage(HttpMethod method, string? requestUri)
      : this(method, string.IsNullOrEmpty(requestUri) ? (Uri) null : new Uri(requestUri, UriKind.RelativeOrAbsolute))
    {
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string representation of the current object.</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("Method: ");
      sb.Append((object) this._method);
      sb.Append(", RequestUri: '");
      sb.Append(this._requestUri == (Uri) null ? "<null>" : this._requestUri.ToString());
      sb.Append("', Version: ");
      sb.Append((object) this._version);
      sb.Append(", Content: ");
      sb.Append(this._content == null ? "<null>" : this._content.GetType().ToString());
      sb.AppendLine(", Headers:");
      HeaderUtilities.DumpHeaders(sb, (HttpHeaders) this._headers, (HttpHeaders) this._content?.Headers);
      return sb.ToString();
    }

    internal bool MarkAsSent() => Interlocked.CompareExchange(ref this._sendStatus, 1, 0) == 0;

    internal bool WasSentByHttpClient() => (this._sendStatus & 1) != 0;

    internal void MarkAsRedirected() => this._sendStatus |= 2;

    internal bool WasRedirected() => (this._sendStatus & 2) != 0;

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpRequestMessage" /> and optionally disposes of the managed resources.</summary>
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

    /// <summary>Releases the unmanaged resources and disposes of the managed resources used by the <see cref="T:System.Net.Http.HttpRequestMessage" />.</summary>
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
