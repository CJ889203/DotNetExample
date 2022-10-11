// Decompiled with JetBrains decompiler
// Type: System.Net.Http.SocketsHttpHandler
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Security;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Provides the default message handler used by <see cref="T:System.Net.Http.HttpClient" /> in .NET Core 2.1 and later.</summary>
  [UnsupportedOSPlatform("browser")]
  public sealed class SocketsHttpHandler : HttpMessageHandler
  {

    #nullable disable
    private readonly HttpConnectionSettings _settings = new HttpConnectionSettings();
    private HttpMessageHandlerStage _handler;
    private bool _disposed;

    private void CheckDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (SocketsHttpHandler));
    }

    private void CheckDisposedOrStarted()
    {
      this.CheckDisposed();
      if (this._handler != null)
        throw new InvalidOperationException(SR.net_http_operation_started);
    }

    /// <summary>Gets a value that indicates whether the handler is supported on the current platform.</summary>
    /// <returns>
    /// <see langword="true" /> if the handler is supported; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatformGuard("browser")]
    public static bool IsSupported => !OperatingSystem.IsBrowser();

    /// <summary>Gets or sets a value that indicates whether the handler should use cookies.</summary>
    /// <returns>A value that indicates whether the handler should use cookies.</returns>
    public bool UseCookies
    {
      get => this._settings._useCookies;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._useCookies = value;
      }
    }


    #nullable enable
    /// <summary>Gets or sets the managed cookie container object.</summary>
    /// <returns>The managed cookie container object.</returns>
    public CookieContainer CookieContainer
    {
      get => this._settings._cookieContainer ?? (this._settings._cookieContainer = new CookieContainer());
      [param: AllowNull] set
      {
        this.CheckDisposedOrStarted();
        this._settings._cookieContainer = value;
      }
    }

    /// <summary>Gets or sets the type of decompression method used by the handler for automatic decompression of the HTTP content response.</summary>
    /// <returns>The type of decompression method used by the handler for automatic decompression of the HTTP content response.</returns>
    public DecompressionMethods AutomaticDecompression
    {
      get => this._settings._automaticDecompression;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._automaticDecompression = value;
      }
    }

    /// <summary>Gets or sets a value that indicates whether the handler should use a proxy.</summary>
    /// <returns>A value that indicates whether the handler should use a proxy.</returns>
    public bool UseProxy
    {
      get => this._settings._useProxy;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._useProxy = value;
      }
    }

    /// <summary>Gets or sets the custom proxy when the <see cref="P:System.Net.Http.SocketsHttpHandler.UseProxy" /> property is <see langword="true" />.</summary>
    /// <returns>The custom proxy.</returns>
    public IWebProxy? Proxy
    {
      get => this._settings._proxy;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._proxy = value;
      }
    }

    /// <summary>When the default (system) proxy is used, gets or sets the credentials used to submit to the default proxy server for authentication.</summary>
    /// <returns>The credentials used to authenticate the user to an authenticating proxy.</returns>
    public ICredentials? DefaultProxyCredentials
    {
      get => this._settings._defaultProxyCredentials;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._defaultProxyCredentials = value;
      }
    }

    /// <summary>Gets or sets a value that indicates whether the handler sends an Authorization header with the request.</summary>
    /// <returns>
    /// <see langword="true" /> if the handler sends an Authorization header with the request; otherwise, <see langword="false" />.</returns>
    public bool PreAuthenticate
    {
      get => this._settings._preAuthenticate;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._preAuthenticate = value;
      }
    }

    /// <summary>Gets or sets authentication information used by this handler.</summary>
    /// <returns>The authentication credentials associated with the handler. The default value is <see langword="null" />.</returns>
    public ICredentials? Credentials
    {
      get => this._settings._credentials;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._credentials = value;
      }
    }

    /// <summary>Gets or sets a value that indicates whether the handler should follow redirection responses.</summary>
    /// <returns>
    /// <see langword="true" /> if the handler should follow redirection responses; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
    public bool AllowAutoRedirect
    {
      get => this._settings._allowAutoRedirect;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._allowAutoRedirect = value;
      }
    }

    /// <summary>Gets or sets the maximum number of allowed HTTP redirects.</summary>
    /// <returns>The maximum number of allowed HTTP redirects.</returns>
    public int MaxAutomaticRedirections
    {
      get => this._settings._maxAutomaticRedirections;
      set
      {
        if (value <= 0)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.Format(SR.net_http_value_must_be_greater_than, (object) 0));
        this.CheckDisposedOrStarted();
        this._settings._maxAutomaticRedirections = value;
      }
    }

    /// <summary>Gets or sets the maximum number of simultaneous TCP connections allowed to a single server.</summary>
    /// <returns>The maximum number of simultaneous TCP connections allowed to a single server.</returns>
    public int MaxConnectionsPerServer
    {
      get => this._settings._maxConnectionsPerServer;
      set
      {
        if (value < 1)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.Format(SR.net_http_value_must_be_greater_than, (object) 0));
        this.CheckDisposedOrStarted();
        this._settings._maxConnectionsPerServer = value;
      }
    }

    /// <summary>Gets or sets the maximum amount of data that can be drained from responses in bytes.</summary>
    /// <returns>The maximum amount of data that can be drained from responses in bytes.</returns>
    public int MaxResponseDrainSize
    {
      get => this._settings._maxResponseDrainSize;
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.ArgumentOutOfRange_NeedNonNegativeNum);
        this.CheckDisposedOrStarted();
        this._settings._maxResponseDrainSize = value;
      }
    }

    /// <summary>Gets or sets the timespan to wait for data to be drained from responses.</summary>
    /// <returns>The timespan to wait for data to be drained from responses.</returns>
    public TimeSpan ResponseDrainTimeout
    {
      get => this._settings._maxResponseDrainTime;
      set
      {
        if (value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan || value.TotalMilliseconds > (double) int.MaxValue)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.CheckDisposedOrStarted();
        this._settings._maxResponseDrainTime = value;
      }
    }

    /// <summary>Gets or sets the maximum length, in kilobytes (1024 bytes), of the response headers.</summary>
    /// <returns>The maximum size of the header portion from the server response, in kilobytes.</returns>
    public int MaxResponseHeadersLength
    {
      get => this._settings._maxResponseHeadersLength;
      set
      {
        if (value <= 0)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.Format(SR.net_http_value_must_be_greater_than, (object) 0));
        this.CheckDisposedOrStarted();
        this._settings._maxResponseHeadersLength = value;
      }
    }

    /// <summary>Gets or sets the set of options used for client TLS authentication.</summary>
    /// <returns>The set of options used for client TLS authentication.</returns>
    public SslClientAuthenticationOptions SslOptions
    {
      get => this._settings._sslOptions ?? (this._settings._sslOptions = new SslClientAuthenticationOptions());
      [param: AllowNull] set
      {
        this.CheckDisposedOrStarted();
        this._settings._sslOptions = value;
      }
    }

    /// <summary>Gets or sets how long a connection can be in the pool to be considered reusable.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than <see cref="F:System.TimeSpan.Zero" /> or is equal to <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.</exception>
    /// <returns>The maximum time for a connection to be in the pool. The default value for this property is <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.</returns>
    public TimeSpan PooledConnectionLifetime
    {
      get => this._settings._pooledConnectionLifetime;
      set
      {
        if (value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.CheckDisposedOrStarted();
        this._settings._pooledConnectionLifetime = value;
      }
    }

    /// <summary>Gets or sets how long a connection can be idle in the pool to be considered reusable.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than <see cref="F:System.TimeSpan.Zero" /> or is equal to <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.</exception>
    /// <returns>The maximum idle time for a connection in the pool. The default value for this property is 2 minutes.</returns>
    public TimeSpan PooledConnectionIdleTimeout
    {
      get => this._settings._pooledConnectionIdleTimeout;
      set
      {
        if (value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.CheckDisposedOrStarted();
        this._settings._pooledConnectionIdleTimeout = value;
      }
    }

    /// <summary>Gets or sets the timespan to wait before the connection establishing times out.</summary>
    /// <returns>The timespan to wait before the connection establishing times out. The default value is <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.</returns>
    public TimeSpan ConnectTimeout
    {
      get => this._settings._connectTimeout;
      set
      {
        if (value <= TimeSpan.Zero && value != Timeout.InfiniteTimeSpan || value.TotalMilliseconds > (double) int.MaxValue)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.CheckDisposedOrStarted();
        this._settings._connectTimeout = value;
      }
    }

    /// <summary>Gets or sets the time-out value for server HTTP 100 Continue response.</summary>
    /// <returns>The timespan to wait for the HTTP 100 Continue. The default value is 1 second.</returns>
    public TimeSpan Expect100ContinueTimeout
    {
      get => this._settings._expect100ContinueTimeout;
      set
      {
        if (value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan || value.TotalMilliseconds > (double) int.MaxValue)
          throw new ArgumentOutOfRangeException(nameof (value));
        this.CheckDisposedOrStarted();
        this._settings._expect100ContinueTimeout = value;
      }
    }

    /// <summary>Defines the initial HTTP2 stream receive window size for all connections opened by the this <see cref="T:System.Net.Http.SocketsHttpHandler" />.</summary>
    public int InitialHttp2StreamWindowSize
    {
      get => this._settings._initialHttp2StreamWindowSize;
      set
      {
        if (value < (int) ushort.MaxValue || value > GlobalHttpSettings.SocketsHttpHandler.MaxHttp2StreamWindowSize)
          throw new ArgumentOutOfRangeException(nameof (InitialHttp2StreamWindowSize), SR.Format(SR.net_http_http2_invalidinitialstreamwindowsize, (object) (int) ushort.MaxValue, (object) GlobalHttpSettings.SocketsHttpHandler.MaxHttp2StreamWindowSize));
        this.CheckDisposedOrStarted();
        this._settings._initialHttp2StreamWindowSize = value;
      }
    }

    /// <summary>Gets or sets the keep alive ping delay.</summary>
    /// <returns>The keep alive ping delay. Defaults to <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.</returns>
    public TimeSpan KeepAlivePingDelay
    {
      get => this._settings._keepAlivePingDelay;
      set
      {
        if (value.Ticks < 10000000L && value != Timeout.InfiniteTimeSpan)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.Format(SR.net_http_value_must_be_greater_than_or_equal, (object) value, (object) TimeSpan.FromSeconds(1.0)));
        this.CheckDisposedOrStarted();
        this._settings._keepAlivePingDelay = value;
      }
    }

    /// <summary>Gets or sets the keep alive ping timeout.</summary>
    /// <returns>The keep alive ping timeout. Defaults to 20 seconds.</returns>
    public TimeSpan KeepAlivePingTimeout
    {
      get => this._settings._keepAlivePingTimeout;
      set
      {
        if (value.Ticks < 10000000L && value != Timeout.InfiniteTimeSpan)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.Format(SR.net_http_value_must_be_greater_than_or_equal, (object) value, (object) TimeSpan.FromSeconds(1.0)));
        this.CheckDisposedOrStarted();
        this._settings._keepAlivePingTimeout = value;
      }
    }

    /// <summary>Gets or sets the keep alive ping behaviour.</summary>
    /// <returns>The keep alive ping behaviour.</returns>
    public HttpKeepAlivePingPolicy KeepAlivePingPolicy
    {
      get => this._settings._keepAlivePingPolicy;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._keepAlivePingPolicy = value;
      }
    }

    /// <summary>Gets or sets a value that indicates whether additional HTTP/2 connections can be established to the same server when the maximum number of concurrent streams is reached on all existing connections.</summary>
    /// <returns>
    /// <see langword="true" /> if additional HTTP/2 connections are allowed to be created; otherwise, <see langword="false" />.</returns>
    public bool EnableMultipleHttp2Connections
    {
      get => this._settings._enableMultipleHttp2Connections;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._enableMultipleHttp2Connections = value;
      }
    }

    /// <summary>Gets or sets a custom callback used to open new connections.</summary>
    /// <returns>A callback method to create a stream.</returns>
    public Func<SocketsHttpConnectionContext, CancellationToken, ValueTask<Stream>>? ConnectCallback
    {
      get => this._settings._connectCallback;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._connectCallback = value;
      }
    }

    /// <summary>Gets or sets a custom callback that provides access to the plaintext HTTP protocol stream.</summary>
    /// <returns>A callback that provides access to the plaintext HTTP protocol stream.</returns>
    public Func<SocketsHttpPlaintextStreamFilterContext, CancellationToken, ValueTask<Stream>>? PlaintextStreamFilter
    {
      get => this._settings._plaintextStreamFilter;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._plaintextStreamFilter = value;
      }
    }

    /// <summary>Gets a writable dictionary (that is, a map) of custom properties for the HttpClient requests. The dictionary is initialized empty; you can insert and query key-value pairs for your custom handlers and special processing.</summary>
    public IDictionary<string, object?> Properties => this._settings._properties ?? (this._settings._properties = (IDictionary<string, object>) new Dictionary<string, object>());

    /// <summary>Gets or sets a callback that selects the <see cref="System.Text.Encoding" /> to encode request header values.</summary>
    /// <returns>The header encoding selector callback that selects the <see cref="System.Text.Encoding" /> to encode the value for the specified request header name, or <see langword="null" /> to indicate the default behavior.</returns>
    public HeaderEncodingSelector<HttpRequestMessage>? RequestHeaderEncodingSelector
    {
      get => this._settings._requestHeaderEncodingSelector;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._requestHeaderEncodingSelector = value;
      }
    }

    /// <summary>Gets or sets a callback that selects the <see cref="System.Text.Encoding" /> to decode response header values.</summary>
    /// <returns>The header encoding selector callback that selects the <see cref="System.Text.Encoding" /> to decode the value for the specified response header name, or <see langword="null" /> to indicate the default behavior.</returns>
    public HeaderEncodingSelector<HttpRequestMessage>? ResponseHeaderEncodingSelector
    {
      get => this._settings._responseHeaderEncodingSelector;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._responseHeaderEncodingSelector = value;
      }
    }

    /// <summary>Gets or sets the <see cref="T:System.Diagnostics.DistributedContextPropagator" /> to use when propagating the distributed trace and context.
    /// Use <see langword="null" /> to disable propagation. Defaults to <see cref="P:System.Diagnostics.DistributedContextPropagator.Current" />.</summary>
    [CLSCompliant(false)]
    public DistributedContextPropagator? ActivityHeadersPropagator
    {
      get => this._settings._activityHeadersPropagator;
      set
      {
        this.CheckDisposedOrStarted();
        this._settings._activityHeadersPropagator = value;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && !this._disposed)
      {
        this._disposed = true;
        this._handler?.Dispose();
      }
      base.Dispose(disposing);
    }


    #nullable disable
    private HttpMessageHandlerStage SetupHandlerChain()
    {
      HttpConnectionSettings settings = this._settings.CloneAndNormalize();
      HttpConnectionPoolManager poolManager = new HttpConnectionPoolManager(settings);
      HttpMessageHandlerStage messageHandlerStage = settings._credentials != null ? (HttpMessageHandlerStage) new HttpAuthenticatedConnectionHandler(poolManager) : (HttpMessageHandlerStage) new HttpConnectionHandler(poolManager);
      if (DiagnosticsHandler.IsGloballyEnabled())
      {
        DistributedContextPropagator headersPropagator = settings._activityHeadersPropagator;
        if (headersPropagator != null)
          messageHandlerStage = (HttpMessageHandlerStage) new DiagnosticsHandler((HttpMessageHandler) messageHandlerStage, headersPropagator, settings._allowAutoRedirect);
      }
      if (settings._allowAutoRedirect)
      {
        HttpMessageHandlerStage redirectInnerHandler = settings._credentials == null || settings._credentials is CredentialCache ? messageHandlerStage : (HttpMessageHandlerStage) new HttpConnectionHandler(poolManager);
        messageHandlerStage = (HttpMessageHandlerStage) new RedirectHandler(settings._maxAutomaticRedirections, messageHandlerStage, redirectInnerHandler);
      }
      if (settings._automaticDecompression != DecompressionMethods.None)
        messageHandlerStage = (HttpMessageHandlerStage) new DecompressionHandler(settings._automaticDecompression, messageHandlerStage);
      if (Interlocked.CompareExchange<HttpMessageHandlerStage>(ref this._handler, messageHandlerStage, (HttpMessageHandlerStage) null) != null)
        messageHandlerStage.Dispose();
      return this._handler;
    }


    #nullable enable
    protected internal override HttpResponseMessage Send(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request), SR.net_http_handler_norequest);
      if (request.Version.Major >= 2)
        throw new NotSupportedException(SR.Format(SR.net_http_http2_sync_not_supported, (object) this.GetType()));
      if (request.VersionPolicy == HttpVersionPolicy.RequestVersionOrHigher)
        throw new NotSupportedException(SR.Format(SR.net_http_upgrade_not_enabled_sync, (object) nameof (Send), (object) request.VersionPolicy));
      this.CheckDisposed();
      cancellationToken.ThrowIfCancellationRequested();
      HttpMessageHandlerStage messageHandlerStage = this._handler ?? this.SetupHandlerChain();
      Exception exception = this.ValidateAndNormalizeRequest(request);
      if (exception != null)
        throw exception;
      return messageHandlerStage.Send(request, cancellationToken);
    }

    protected internal override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request), SR.net_http_handler_norequest);
      this.CheckDisposed();
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled<HttpResponseMessage>(cancellationToken);
      HttpMessageHandler httpMessageHandler = (HttpMessageHandler) (this._handler ?? this.SetupHandlerChain());
      Exception exception = this.ValidateAndNormalizeRequest(request);
      return exception != null ? Task.FromException<HttpResponseMessage>(exception) : httpMessageHandler.SendAsync(request, cancellationToken);
    }


    #nullable disable
    private Exception ValidateAndNormalizeRequest(HttpRequestMessage request)
    {
      if (request.Version.Major == 0)
        return (Exception) new NotSupportedException(SR.net_http_unsupported_version);
      if (request.HasHeaders && request.Headers.TransferEncodingChunked.GetValueOrDefault())
      {
        if (request.Content == null)
          return (Exception) new HttpRequestException(SR.net_http_client_execution_error, (Exception) new InvalidOperationException(SR.net_http_chunked_not_allowed_with_empty_content));
        request.Content.Headers.ContentLength = new long?();
      }
      else if (request.Content != null && !request.Content.Headers.ContentLength.HasValue)
        request.Headers.TransferEncodingChunked = new bool?(true);
      if (request.Version.Minor == 0 && request.Version.Major == 1 && request.HasHeaders)
      {
        bool? nullable = request.Headers.TransferEncodingChunked;
        bool flag1 = true;
        if (nullable.GetValueOrDefault() == flag1 & nullable.HasValue)
          return (Exception) new NotSupportedException(SR.net_http_unsupported_chunking);
        nullable = request.Headers.ExpectContinue;
        bool flag2 = true;
        if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
          request.Headers.ExpectContinue = new bool?(false);
      }
      Uri requestUri = request.RequestUri;
      if ((object) requestUri == null || !requestUri.IsAbsoluteUri)
        return (Exception) new InvalidOperationException(SR.net_http_client_invalid_requesturi);
      return !HttpUtilities.IsSupportedScheme(requestUri.Scheme) ? (Exception) new NotSupportedException(SR.Format(SR.net_http_unsupported_requesturi_scheme, (object) requestUri.Scheme)) : (Exception) null;
    }
  }
}
