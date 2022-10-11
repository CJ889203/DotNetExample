// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpClientHandler
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using System.Runtime.Versioning;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>The default message handler used by <see cref="T:System.Net.Http.HttpClient" /> in .NET Framework and .NET Core 2.0 and earlier.</summary>
  public class HttpClientHandler : HttpMessageHandler
  {

    #nullable disable
    private readonly SocketsHttpHandler _underlyingHandler;
    private ClientCertificateOption _clientCertificateOptions;
    private volatile bool _disposed;
    private static Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> s_dangerousAcceptAnyServerCertificateValidator;


    #nullable enable
    private HttpMessageHandler Handler => (HttpMessageHandler) this._underlyingHandler;

    /// <summary>Creates an instance of a <see cref="T:System.Net.Http.HttpClientHandler" /> class.</summary>
    public HttpClientHandler()
    {
      this._underlyingHandler = new SocketsHttpHandler();
      this.ClientCertificateOptions = ClientCertificateOption.Manual;
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpClientHandler" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && !this._disposed)
      {
        this._disposed = true;
        this._underlyingHandler.Dispose();
      }
      base.Dispose(disposing);
    }

    /// <summary>Gets a value that indicates whether the handler supports automatic response content decompression.</summary>
    /// <returns>
    /// <see langword="true" /> if the if the handler supports automatic response content decompression; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
    public virtual bool SupportsAutomaticDecompression => true;

    /// <summary>Gets a value that indicates whether the handler supports proxy settings.</summary>
    /// <returns>
    /// <see langword="true" /> if the if the handler supports proxy settings; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
    public virtual bool SupportsProxy => true;

    /// <summary>Gets a value that indicates whether the handler supports configuration settings for the <see cref="P:System.Net.Http.HttpClientHandler.AllowAutoRedirect" /> and <see cref="P:System.Net.Http.HttpClientHandler.MaxAutomaticRedirections" /> properties.</summary>
    /// <returns>
    /// <see langword="true" /> if the if the handler supports configuration settings for the <see cref="P:System.Net.Http.HttpClientHandler.AllowAutoRedirect" /> and <see cref="P:System.Net.Http.HttpClientHandler.MaxAutomaticRedirections" /> properties; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
    public virtual bool SupportsRedirectConfiguration => true;

    /// <summary>Gets or sets a value that indicates whether the handler uses the  <see cref="P:System.Net.Http.HttpClientHandler.CookieContainer" /> property  to store server cookies and uses these cookies when sending requests.</summary>
    /// <returns>
    /// <see langword="true" /> if the if the handler supports uses the  <see cref="P:System.Net.Http.HttpClientHandler.CookieContainer" /> property  to store server cookies and uses these cookies when sending requests; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool UseCookies
    {
      get => this._underlyingHandler.UseCookies;
      set => this._underlyingHandler.UseCookies = value;
    }

    /// <summary>Gets or sets the cookie container used to store server cookies by the handler.</summary>
    /// <returns>The cookie container used to store server cookies by the handler.</returns>
    [UnsupportedOSPlatform("browser")]
    public CookieContainer CookieContainer
    {
      get => this._underlyingHandler.CookieContainer;
      set => this._underlyingHandler.CookieContainer = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>Gets or sets the type of decompression method used by the handler for automatic decompression of the HTTP content response.</summary>
    /// <returns>The automatic decompression method used by the handler.</returns>
    [UnsupportedOSPlatform("browser")]
    public DecompressionMethods AutomaticDecompression
    {
      get => this._underlyingHandler.AutomaticDecompression;
      set => this._underlyingHandler.AutomaticDecompression = value;
    }

    /// <summary>Gets or sets a value that indicates whether the handler uses a proxy for requests.</summary>
    /// <returns>
    /// <see langword="true" /> if the handler should use a proxy for requests; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool UseProxy
    {
      get => this._underlyingHandler.UseProxy;
      set => this._underlyingHandler.UseProxy = value;
    }

    /// <summary>Gets or sets proxy information used by the handler.</summary>
    /// <returns>The proxy information used by the handler. The default value is <see langword="null" />.</returns>
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public IWebProxy? Proxy
    {
      get => this._underlyingHandler.Proxy;
      set => this._underlyingHandler.Proxy = value;
    }

    /// <summary>When the default (system) proxy is being used, gets or sets the credentials to submit to the default proxy server for authentication. The default proxy is used only when <see cref="P:System.Net.Http.HttpClientHandler.UseProxy" /> is set to <see langword="true" /> and <see cref="P:System.Net.Http.HttpClientHandler.Proxy" /> is set to <see langword="null" />.</summary>
    /// <returns>The credentials needed to authenticate a request to the default proxy server.</returns>
    [UnsupportedOSPlatform("browser")]
    public ICredentials? DefaultProxyCredentials
    {
      get => this._underlyingHandler.DefaultProxyCredentials;
      set => this._underlyingHandler.DefaultProxyCredentials = value;
    }

    /// <summary>Gets or sets a value that indicates whether the handler sends an Authorization header with the request.</summary>
    /// <returns>
    /// <see langword="true" /> for the handler to send an HTTP Authorization header with requests after authentication has taken place; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool PreAuthenticate
    {
      get => this._underlyingHandler.PreAuthenticate;
      set => this._underlyingHandler.PreAuthenticate = value;
    }

    /// <summary>Gets or sets a value that controls whether default credentials are sent with requests by the handler.</summary>
    /// <returns>
    /// <see langword="true" /> if the default credentials are used; otherwise <see langword="false" />. The default value is <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool UseDefaultCredentials
    {
      get => this._underlyingHandler.Credentials == CredentialCache.DefaultCredentials;
      set
      {
        if (value)
        {
          this._underlyingHandler.Credentials = CredentialCache.DefaultCredentials;
        }
        else
        {
          if (this._underlyingHandler.Credentials != CredentialCache.DefaultCredentials)
            return;
          this._underlyingHandler.Credentials = (ICredentials) null;
        }
      }
    }

    /// <summary>Gets or sets authentication information used by this handler.</summary>
    /// <returns>The authentication credentials associated with the handler. The default is <see langword="null" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public ICredentials? Credentials
    {
      get => this._underlyingHandler.Credentials;
      set => this._underlyingHandler.Credentials = value;
    }

    /// <summary>Gets or sets a value that indicates whether the handler should follow redirection responses.</summary>
    /// <returns>
    /// <see langword="true" /> if the handler should follow redirection responses; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
    public bool AllowAutoRedirect
    {
      get => this._underlyingHandler.AllowAutoRedirect;
      set => this._underlyingHandler.AllowAutoRedirect = value;
    }

    /// <summary>Gets or sets the maximum number of redirects that the handler follows.</summary>
    /// <returns>The maximum number of redirection responses that the handler follows. The default value is 50.</returns>
    [UnsupportedOSPlatform("browser")]
    public int MaxAutomaticRedirections
    {
      get => this._underlyingHandler.MaxAutomaticRedirections;
      set => this._underlyingHandler.MaxAutomaticRedirections = value;
    }

    /// <summary>Gets or sets the maximum number of concurrent connections (per server endpoint) allowed when making requests using an <see cref="T:System.Net.Http.HttpClient" /> object. Note that the limit is per server endpoint, so for example a value of 256 would permit 256 concurrent connections to http://www.adatum.com/ and another 256 to http://www.adventure-works.com/.</summary>
    /// <returns>The maximum number of concurrent connections (per server endpoint) allowed by an <see cref="T:System.Net.Http.HttpClient" /> object.</returns>
    [UnsupportedOSPlatform("browser")]
    public int MaxConnectionsPerServer
    {
      get => this._underlyingHandler.MaxConnectionsPerServer;
      set => this._underlyingHandler.MaxConnectionsPerServer = value;
    }

    /// <summary>Gets or sets the maximum request content buffer size used by the handler.</summary>
    /// <returns>The maximum request content buffer size in bytes. The default value is 2 gigabytes.</returns>
    public long MaxRequestContentBufferSize
    {
      get => 0;
      set
      {
        if (value < 0L)
          throw new ArgumentOutOfRangeException(nameof (value));
        if (value > (long) int.MaxValue)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_content_buffersize_limit, (object) int.MaxValue));
        this.CheckDisposed();
      }
    }

    /// <summary>Gets or sets the maximum length, in kilobytes (1024 bytes), of the response headers. For example, if the value is 64, then 65536 bytes are allowed for the maximum response headers' length.</summary>
    /// <returns>The maximum length, in kilobytes (1024 bytes), of the response headers.</returns>
    [UnsupportedOSPlatform("browser")]
    public int MaxResponseHeadersLength
    {
      get => this._underlyingHandler.MaxResponseHeadersLength;
      set => this._underlyingHandler.MaxResponseHeadersLength = value;
    }

    /// <summary>Gets or sets a value that indicates if the certificate is automatically picked from the certificate store or if the caller is allowed to pass in a specific client certificate.</summary>
    /// <returns>The collection of security certificates associated with this handler.</returns>
    public ClientCertificateOption ClientCertificateOptions
    {
      get => this._clientCertificateOptions;
      set
      {
        if (value != ClientCertificateOption.Manual)
        {
          if (value != ClientCertificateOption.Automatic)
            throw new ArgumentOutOfRangeException(nameof (value));
          this.ThrowForModifiedManagedSslOptionsIfStarted();
          this._clientCertificateOptions = value;
          this._underlyingHandler.SslOptions.LocalCertificateSelectionCallback = (LocalCertificateSelectionCallback) ((sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => (X509Certificate) CertificateHelper.GetEligibleClientCertificate());
        }
        else
        {
          this.ThrowForModifiedManagedSslOptionsIfStarted();
          this._clientCertificateOptions = value;
          this._underlyingHandler.SslOptions.LocalCertificateSelectionCallback = (LocalCertificateSelectionCallback) ((sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => (X509Certificate) CertificateHelper.GetEligibleClientCertificate(this.ClientCertificates));
        }
      }
    }

    /// <summary>Gets the collection of security certificates that are associated with requests to the server.</summary>
    /// <returns>The X509CertificateCollection that is presented to the server when performing certificate based client authentication.</returns>
    [UnsupportedOSPlatform("browser")]
    public X509CertificateCollection ClientCertificates
    {
      get
      {
        if (this.ClientCertificateOptions != ClientCertificateOption.Manual)
          throw new InvalidOperationException(SR.Format(SR.net_http_invalid_enable_first, (object) "ClientCertificateOptions", (object) "Manual"));
        return this._underlyingHandler.SslOptions.ClientCertificates ?? (this._underlyingHandler.SslOptions.ClientCertificates = new X509CertificateCollection());
      }
    }

    /// <summary>Gets or sets a callback method to validate the server certificate.</summary>
    /// <returns>A callback method to validate the server certificate.</returns>
    [UnsupportedOSPlatform("browser")]
    public Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? ServerCertificateCustomValidationCallback
    {
      get => !(this._underlyingHandler.SslOptions.RemoteCertificateValidationCallback?.Target is ConnectHelper.CertificateCallbackMapper target) ? (Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>) null : target.FromHttpClientHandler;
      set
      {
        this.ThrowForModifiedManagedSslOptionsIfStarted();
        this._underlyingHandler.SslOptions.RemoteCertificateValidationCallback = value != null ? new ConnectHelper.CertificateCallbackMapper(value).ForSocketsHttpHandler : (RemoteCertificateValidationCallback) null;
      }
    }

    /// <summary>Gets or sets a value that indicates whether the certificate is checked against the certificate authority revocation list.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Framework 4.7.1 only: This property is not implemented.</exception>
    /// <returns>
    /// <see langword="true" /> if the certificate revocation list is checked; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool CheckCertificateRevocationList
    {
      get => this._underlyingHandler.SslOptions.CertificateRevocationCheckMode == X509RevocationMode.Online;
      set
      {
        this.ThrowForModifiedManagedSslOptionsIfStarted();
        this._underlyingHandler.SslOptions.CertificateRevocationCheckMode = value ? X509RevocationMode.Online : X509RevocationMode.NoCheck;
      }
    }

    /// <summary>Gets or sets the TLS/SSL protocol used by the <see cref="T:System.Net.Http.HttpClient" /> objects managed by the HttpClientHandler object.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Framework 4.7.1 only: This property is not implemented.</exception>
    /// <returns>One of the values defined in the <see cref="T:System.Security.Authentication.SslProtocols" /> enumeration.</returns>
    [UnsupportedOSPlatform("browser")]
    public SslProtocols SslProtocols
    {
      get => this._underlyingHandler.SslOptions.EnabledSslProtocols;
      set
      {
        this.ThrowForModifiedManagedSslOptionsIfStarted();
        this._underlyingHandler.SslOptions.EnabledSslProtocols = value;
      }
    }

    /// <summary>Gets a writable dictionary (that is, a map) of custom properties for the <see cref="T:System.Net.Http.HttpClient" /> requests. The dictionary is initialized empty; you can insert and query key-value pairs for your custom handlers and special processing.</summary>
    /// <returns>a writable dictionary of custom properties.</returns>
    public IDictionary<string, object?> Properties => this._underlyingHandler.Properties;

    /// <summary>Creates an instance of  <see cref="T:System.Net.Http.HttpResponseMessage" /> based on the information provided in the <see cref="T:System.Net.Http.HttpRequestMessage" />.</summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">For HTTP/2 and higher or when requesting version upgrade is enabled by <see cref="F:System.Net.Http.HttpVersionPolicy.RequestVersionOrHigher" />.
    /// 
    /// -or-
    /// 
    /// If using custom class derived from <see cref="T:System.Net.Http.HttpContent" /> not overriding <see cref="M:System.Net.Http.HttpContent.SerializeToStream(System.IO.Stream,System.Net.TransportContext,System.Threading.CancellationToken)" /> method.
    /// 
    /// -or-
    /// 
    /// If using custom <see cref="T:System.Net.Http.HttpMessageHandler" /> not overriding <see cref="M:System.Net.Http.HttpMessageHandler.Send(System.Net.Http.HttpRequestMessage,System.Threading.CancellationToken)" /> method.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The request was canceled.
    /// 
    /// -or-
    /// 
    /// If the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> exception nests the <see cref="T:System.TimeoutException" />:
    /// The request failed due to timeout.</exception>
    /// <returns>The HTTP response message.</returns>
    [UnsupportedOSPlatform("browser")]
    protected internal override HttpResponseMessage Send(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      return this.Handler.Send(request, cancellationToken);
    }

    /// <summary>Creates an instance of  <see cref="T:System.Net.Http.HttpResponseMessage" /> based on the information provided in the <see cref="T:System.Net.Http.HttpRequestMessage" /> as an operation that will not block.</summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected internal override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      return this.Handler.SendAsync(request, cancellationToken);
    }

    /// <summary>Gets a cached delegate that always returns <see langword="true" />.</summary>
    /// <returns>A cached delegate that always returns <see langword="true" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> DangerousAcceptAnyServerCertificateValidator => Volatile.Read<Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>>(ref HttpClientHandler.s_dangerousAcceptAnyServerCertificateValidator) ?? Interlocked.CompareExchange<Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>>(ref HttpClientHandler.s_dangerousAcceptAnyServerCertificateValidator, (Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>) ((_param1, _param2, _param3, _param4) => true), (Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>) null) ?? HttpClientHandler.s_dangerousAcceptAnyServerCertificateValidator;

    private void ThrowForModifiedManagedSslOptionsIfStarted() => this._underlyingHandler.SslOptions = this._underlyingHandler.SslOptions;

    private void CheckDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().ToString());
    }
  }
}
