// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpClient
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Provides a class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.</summary>
  public class HttpClient : HttpMessageInvoker
  {

    #nullable disable
    private static IWebProxy s_defaultProxy;
    private static readonly TimeSpan s_defaultTimeout = TimeSpan.FromSeconds(100.0);
    private static readonly TimeSpan s_maxTimeout = TimeSpan.FromMilliseconds((double) int.MaxValue);
    private static readonly TimeSpan s_infiniteTimeout = System.Threading.Timeout.InfiniteTimeSpan;
    private volatile bool _operationStarted;
    private volatile bool _disposed;
    private CancellationTokenSource _pendingRequestsCts;
    private HttpRequestHeaders _defaultRequestHeaders;
    private Version _defaultRequestVersion = HttpRequestMessage.DefaultRequestVersion;
    private HttpVersionPolicy _defaultVersionPolicy = HttpRequestMessage.DefaultVersionPolicy;
    private Uri _baseAddress;
    private TimeSpan _timeout;
    private int _maxResponseContentBufferSize;


    #nullable enable
    /// <summary>Gets or sets the global Http proxy.</summary>
    /// <exception cref="T:System.ArgumentNullException">The value passed cannot be <see langword="null" />.</exception>
    /// <returns>A proxy used by every call that instantiates a <see cref="T:System.Net.HttpWebRequest" />.</returns>
    public static IWebProxy DefaultProxy
    {
      get => LazyInitializer.EnsureInitialized<IWebProxy>(ref HttpClient.s_defaultProxy, (Func<IWebProxy>) (() => SystemProxyInfo.Proxy));
      set => HttpClient.s_defaultProxy = value ?? throw new ArgumentNullException(nameof (value));
    }

    /// <summary>Gets the headers which should be sent with each request.</summary>
    /// <returns>The headers which should be sent with each request.</returns>
    public HttpRequestHeaders DefaultRequestHeaders => this._defaultRequestHeaders ?? (this._defaultRequestHeaders = new HttpRequestHeaders());

    /// <summary>Gets or sets the default HTTP version used on subsequent requests made by this <see cref="T:System.Net.Http.HttpClient" /> instance.</summary>
    /// <exception cref="T:System.ArgumentNullException">In a set operation, <see langword="DefaultRequestVersion" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Http.HttpClient" /> instance has already started one or more requests.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Http.HttpClient" /> instance has already been disposed.</exception>
    /// <returns>The default version to use for any requests made with this <see cref="T:System.Net.Http.HttpClient" /> instance.</returns>
    public Version DefaultRequestVersion
    {
      get => this._defaultRequestVersion;
      set
      {
        this.CheckDisposedOrStarted();
        this._defaultRequestVersion = value ?? throw new ArgumentNullException(nameof (value));
      }
    }

    /// <summary>Gets or sets the default version policy for implicitly created requests in convenience methods, for example, <see cref="M:System.Net.Http.HttpClient.GetAsync(System.String)" /> and <see cref="M:System.Net.Http.HttpClient.PostAsync(System.String,System.Net.Http.HttpContent)" />.</summary>
    /// <returns>The HttpVersionPolicy used when the HTTP connection is established.</returns>
    public HttpVersionPolicy DefaultVersionPolicy
    {
      get => this._defaultVersionPolicy;
      set
      {
        this.CheckDisposedOrStarted();
        this._defaultVersionPolicy = value;
      }
    }

    /// <summary>Gets or sets the base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.</summary>
    /// <returns>The base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.</returns>
    public Uri? BaseAddress
    {
      get => this._baseAddress;
      set
      {
        if ((object) value != null && !value.IsAbsoluteUri)
          throw new ArgumentException(SR.net_http_client_absolute_baseaddress_required, nameof (value));
        this.CheckDisposedOrStarted();
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.UriBaseAddress((object) this, value);
        this._baseAddress = value;
      }
    }

    /// <summary>Gets or sets the timespan to wait before the request times out.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The timeout specified is less than or equal to zero and is not <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">An operation has already been started on the current instance.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed.</exception>
    /// <returns>The timespan to wait before the request times out.</returns>
    public TimeSpan Timeout
    {
      get => this._timeout;
      set
      {
        if (value != HttpClient.s_infiniteTimeout && (value <= TimeSpan.Zero || value > HttpClient.s_maxTimeout))
          throw new ArgumentOutOfRangeException(nameof (value));
        this.CheckDisposedOrStarted();
        this._timeout = value;
      }
    }

    /// <summary>Gets or sets the maximum number of bytes to buffer when reading the response content.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The size specified is less than or equal to zero.</exception>
    /// <exception cref="T:System.InvalidOperationException">An operation has already been started on the current instance.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed.</exception>
    /// <returns>The maximum number of bytes to buffer when reading the response content. The default value for this property is 2 gigabytes.</returns>
    public long MaxResponseContentBufferSize
    {
      get => (long) this._maxResponseContentBufferSize;
      set
      {
        if (value <= 0L)
          throw new ArgumentOutOfRangeException(nameof (value));
        if (value > (long) int.MaxValue)
          throw new ArgumentOutOfRangeException(nameof (value), (object) value, SR.Format((IFormatProvider) CultureInfo.InvariantCulture, SR.net_http_content_buffersize_limit, (object) int.MaxValue));
        this.CheckDisposedOrStarted();
        this._maxResponseContentBufferSize = (int) value;
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpClient" /> class using a <see cref="T:System.Net.Http.HttpClientHandler" /> that is disposed when this instance is disposed.</summary>
    public HttpClient()
      : this((HttpMessageHandler) new HttpClientHandler())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpClient" /> class with the specified handler. The handler is disposed when this instance is disposed.</summary>
    /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="handler" /> is <see langword="null" />.</exception>
    public HttpClient(HttpMessageHandler handler)
      : this(handler, true)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpClient" /> class with the provided handler, and specifies whether that handler should be disposed when this instance is disposed.</summary>
    /// <param name="handler">The <see cref="T:System.Net.Http.HttpMessageHandler" /> responsible for processing the HTTP response messages.</param>
    /// <param name="disposeHandler">
    /// <see langword="true" /> if the inner handler should be disposed of by HttpClient.Dispose; <see langword="false" /> if you intend to reuse the inner handler.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="handler" /> is <see langword="null" />.</exception>
    public HttpClient(HttpMessageHandler handler, bool disposeHandler)
      : base(handler, disposeHandler)
    {
      this._timeout = HttpClient.s_defaultTimeout;
      this._maxResponseContentBufferSize = int.MaxValue;
      this._pendingRequestsCts = new CancellationTokenSource();
    }

    /// <summary>Send a GET request to the specified Uri and return the response body as a string in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<string> GetStringAsync(string? requestUri) => this.GetStringAsync(this.CreateUri(requestUri));

    /// <summary>Send a GET request to the specified Uri and return the response body as a string in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<string> GetStringAsync(Uri? requestUri) => this.GetStringAsync(requestUri, CancellationToken.None);

    /// <summary>Send a GET request to the specified Uri and return the response body as a string in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<string> GetStringAsync(string? requestUri, CancellationToken cancellationToken) => this.GetStringAsync(this.CreateUri(requestUri), cancellationToken);

    /// <summary>Send a GET request to the specified Uri and return the response body as a string in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<string> GetStringAsync(Uri? requestUri, CancellationToken cancellationToken)
    {
      HttpRequestMessage requestMessage = this.CreateRequestMessage(HttpMethod.Get, requestUri);
      this.CheckRequestBeforeSend(requestMessage);
      return this.GetStringAsyncCore(requestMessage, cancellationToken);
    }


    #nullable disable
    private async Task<string> GetStringAsyncCore(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      bool telemetryStarted = HttpClient.StartSend(request);
      bool responseContentTelemetryStarted = false;
      (CancellationTokenSource cancellationTokenSource1, bool flag, CancellationTokenSource cancellationTokenSource2) = this.PrepareCancellationTokenSource(cancellationToken);
      HttpResponseMessage response = (HttpResponseMessage) null;
      string stringAsyncCore;
      try
      {
        response = await base.SendAsync(request, cancellationTokenSource1.Token).ConfigureAwait(false);
        HttpClient.ThrowForNullResponse(response);
        response.EnsureSuccessStatusCode();
        HttpContent c = response.Content;
        if (HttpTelemetry.Log.IsEnabled() & telemetryStarted)
        {
          HttpTelemetry.Log.ResponseContentStart();
          responseContentTelemetryStarted = true;
        }
        Stream stream = c.TryReadAsStream();
        if (stream == null)
          stream = await c.ReadAsStreamAsync(cancellationTokenSource1.Token).ConfigureAwait(false);
        using (Stream responseStream = stream)
        {
          using (HttpContent.LimitArrayPoolWriteStream buffer = new HttpContent.LimitArrayPoolWriteStream(this._maxResponseContentBufferSize, (long) (int) c.Headers.ContentLength.GetValueOrDefault()))
          {
            try
            {
              await responseStream.CopyToAsync((Stream) buffer, cancellationTokenSource1.Token).ConfigureAwait(false);
            }
            catch (Exception ex) when (HttpContent.StreamCopyExceptionNeedsWrapping(ex))
            {
              throw HttpContent.WrapStreamCopyException(ex);
            }
            stringAsyncCore = buffer.Length <= 0L ? string.Empty : HttpContent.ReadBufferAsString(buffer.GetBuffer(), c.Headers);
          }
        }
      }
      catch (Exception ex)
      {
        this.HandleFailure(ex, telemetryStarted, response, cancellationTokenSource1, cancellationToken, cancellationTokenSource2);
        throw;
      }
      finally
      {
        HttpClient.FinishSend(cancellationTokenSource1, flag, telemetryStarted, responseContentTelemetryStarted);
      }
      cancellationTokenSource1 = (CancellationTokenSource) null;
      cancellationTokenSource2 = (CancellationTokenSource) null;
      response = (HttpResponseMessage) null;
      return stringAsyncCore;
    }


    #nullable enable
    /// <summary>Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<byte[]> GetByteArrayAsync(string? requestUri) => this.GetByteArrayAsync(this.CreateUri(requestUri));

    /// <summary>Send a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<byte[]> GetByteArrayAsync(Uri? requestUri) => this.GetByteArrayAsync(requestUri, CancellationToken.None);

    /// <summary>Sends a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<byte[]> GetByteArrayAsync(
      string? requestUri,
      CancellationToken cancellationToken)
    {
      return this.GetByteArrayAsync(this.CreateUri(requestUri), cancellationToken);
    }

    /// <summary>Send a GET request to the specified Uri and return the response body as a byte array in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<byte[]> GetByteArrayAsync(Uri? requestUri, CancellationToken cancellationToken)
    {
      HttpRequestMessage requestMessage = this.CreateRequestMessage(HttpMethod.Get, requestUri);
      this.CheckRequestBeforeSend(requestMessage);
      return this.GetByteArrayAsyncCore(requestMessage, cancellationToken);
    }


    #nullable disable
    private async Task<byte[]> GetByteArrayAsyncCore(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      bool telemetryStarted = HttpClient.StartSend(request);
      bool responseContentTelemetryStarted = false;
      (CancellationTokenSource cancellationTokenSource1, bool flag, CancellationTokenSource cancellationTokenSource2) = this.PrepareCancellationTokenSource(cancellationToken);
      HttpResponseMessage response = (HttpResponseMessage) null;
      byte[] byteArrayAsyncCore;
      try
      {
        response = await base.SendAsync(request, cancellationTokenSource1.Token).ConfigureAwait(false);
        HttpClient.ThrowForNullResponse(response);
        response.EnsureSuccessStatusCode();
        HttpContent content = response.Content;
        if (HttpTelemetry.Log.IsEnabled() & telemetryStarted)
        {
          HttpTelemetry.Log.ResponseContentStart();
          responseContentTelemetryStarted = true;
        }
        long? contentLength = content.Headers.ContentLength;
        using (Stream buffer = contentLength.HasValue ? (Stream) new HttpContent.LimitMemoryStream(this._maxResponseContentBufferSize, (int) contentLength.GetValueOrDefault()) : (Stream) new HttpContent.LimitArrayPoolWriteStream(this._maxResponseContentBufferSize))
        {
          Stream stream = content.TryReadAsStream();
          if (stream == null)
            stream = await content.ReadAsStreamAsync(cancellationTokenSource1.Token).ConfigureAwait(false);
          using (Stream responseStream = stream)
          {
            try
            {
              await responseStream.CopyToAsync(buffer, cancellationTokenSource1.Token).ConfigureAwait(false);
            }
            catch (Exception ex) when (HttpContent.StreamCopyExceptionNeedsWrapping(ex))
            {
              throw HttpContent.WrapStreamCopyException(ex);
            }
            byteArrayAsyncCore = buffer.Length == 0L ? Array.Empty<byte>() : (buffer is HttpContent.LimitMemoryStream limitMemoryStream ? limitMemoryStream.GetSizedBuffer() : ((HttpContent.LimitArrayPoolWriteStream) buffer).ToArray());
          }
        }
      }
      catch (Exception ex)
      {
        this.HandleFailure(ex, telemetryStarted, response, cancellationTokenSource1, cancellationToken, cancellationTokenSource2);
        throw;
      }
      finally
      {
        HttpClient.FinishSend(cancellationTokenSource1, flag, telemetryStarted, responseContentTelemetryStarted);
      }
      cancellationTokenSource1 = (CancellationTokenSource) null;
      cancellationTokenSource2 = (CancellationTokenSource) null;
      response = (HttpResponseMessage) null;
      return byteArrayAsyncCore;
    }


    #nullable enable
    /// <summary>Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<Stream> GetStreamAsync(string? requestUri) => this.GetStreamAsync(this.CreateUri(requestUri));

    /// <summary>Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<Stream> GetStreamAsync(
      string? requestUri,
      CancellationToken cancellationToken)
    {
      return this.GetStreamAsync(this.CreateUri(requestUri), cancellationToken);
    }

    /// <summary>Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<Stream> GetStreamAsync(Uri? requestUri) => this.GetStreamAsync(requestUri, CancellationToken.None);

    /// <summary>Send a GET request to the specified Uri and return the response body as a stream in an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation (or timeout for .NET Framework only).</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<Stream> GetStreamAsync(
      Uri? requestUri,
      CancellationToken cancellationToken)
    {
      HttpRequestMessage requestMessage = this.CreateRequestMessage(HttpMethod.Get, requestUri);
      this.CheckRequestBeforeSend(requestMessage);
      return this.GetStreamAsyncCore(requestMessage, cancellationToken);
    }


    #nullable disable
    private async Task<Stream> GetStreamAsyncCore(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      bool telemetryStarted = HttpClient.StartSend(request);
      (CancellationTokenSource cancellationTokenSource1, bool flag, CancellationTokenSource cancellationTokenSource2) = this.PrepareCancellationTokenSource(cancellationToken);
      HttpResponseMessage response = (HttpResponseMessage) null;
      Stream streamAsyncCore;
      try
      {
        response = await base.SendAsync(request, cancellationTokenSource1.Token).ConfigureAwait(false);
        HttpClient.ThrowForNullResponse(response);
        response.EnsureSuccessStatusCode();
        HttpContent content = response.Content;
        Stream stream = content.TryReadAsStream();
        if (stream == null)
          stream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        streamAsyncCore = stream;
      }
      catch (Exception ex)
      {
        this.HandleFailure(ex, telemetryStarted, response, cancellationTokenSource1, cancellationToken, cancellationTokenSource2);
        throw;
      }
      finally
      {
        HttpClient.FinishSend(cancellationTokenSource1, flag, telemetryStarted, false);
      }
      cancellationTokenSource1 = (CancellationTokenSource) null;
      cancellationTokenSource2 = (CancellationTokenSource) null;
      response = (HttpResponseMessage) null;
      return streamAsyncCore;
    }


    #nullable enable
    /// <summary>Send a GET request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(string? requestUri) => this.GetAsync(this.CreateUri(requestUri));

    /// <summary>Send a GET request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(Uri? requestUri) => this.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead);

    /// <summary>Send a GET request to the specified Uri with an HTTP completion option as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(
      string? requestUri,
      HttpCompletionOption completionOption)
    {
      return this.GetAsync(this.CreateUri(requestUri), completionOption);
    }

    /// <summary>Send a GET request to the specified Uri with an HTTP completion option as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(
      Uri? requestUri,
      HttpCompletionOption completionOption)
    {
      return this.GetAsync(requestUri, completionOption, CancellationToken.None);
    }

    /// <summary>Send a GET request to the specified Uri with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(
      string? requestUri,
      CancellationToken cancellationToken)
    {
      return this.GetAsync(this.CreateUri(requestUri), cancellationToken);
    }

    /// <summary>Send a GET request to the specified Uri with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(
      Uri? requestUri,
      CancellationToken cancellationToken)
    {
      return this.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    /// <summary>Send a GET request to the specified Uri with an HTTP completion option and a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="completionOption">An HTTP  completion option value that indicates when the operation should be considered completed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(
      string? requestUri,
      HttpCompletionOption completionOption,
      CancellationToken cancellationToken)
    {
      return this.GetAsync(this.CreateUri(requestUri), completionOption, cancellationToken);
    }

    /// <summary>Send a GET request to the specified Uri with an HTTP completion option and a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="completionOption">An HTTP  completion option value that indicates when the operation should be considered completed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> GetAsync(
      Uri? requestUri,
      HttpCompletionOption completionOption,
      CancellationToken cancellationToken)
    {
      return this.SendAsync(this.CreateRequestMessage(HttpMethod.Get, requestUri), completionOption, cancellationToken);
    }

    /// <summary>Send a POST request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PostAsync(
      string? requestUri,
      HttpContent? content)
    {
      return this.PostAsync(this.CreateUri(requestUri), content);
    }

    /// <summary>Send a POST request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PostAsync(
      Uri? requestUri,
      HttpContent? content)
    {
      return this.PostAsync(requestUri, content, CancellationToken.None);
    }

    /// <summary>Send a POST request with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PostAsync(
      string? requestUri,
      HttpContent? content,
      CancellationToken cancellationToken)
    {
      return this.PostAsync(this.CreateUri(requestUri), content, cancellationToken);
    }

    /// <summary>Send a POST request with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PostAsync(
      Uri? requestUri,
      HttpContent? content,
      CancellationToken cancellationToken)
    {
      HttpRequestMessage requestMessage = this.CreateRequestMessage(HttpMethod.Post, requestUri);
      requestMessage.Content = content;
      return this.SendAsync(requestMessage, cancellationToken);
    }

    /// <summary>Send a PUT request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PutAsync(
      string? requestUri,
      HttpContent? content)
    {
      return this.PutAsync(this.CreateUri(requestUri), content);
    }

    /// <summary>Send a PUT request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PutAsync(
      Uri? requestUri,
      HttpContent? content)
    {
      return this.PutAsync(requestUri, content, CancellationToken.None);
    }

    /// <summary>Send a PUT request with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PutAsync(
      string? requestUri,
      HttpContent? content,
      CancellationToken cancellationToken)
    {
      return this.PutAsync(this.CreateUri(requestUri), content, cancellationToken);
    }

    /// <summary>Send a PUT request with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="requestUri" /> must be an absolute URI or <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> must be set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PutAsync(
      Uri? requestUri,
      HttpContent? content,
      CancellationToken cancellationToken)
    {
      HttpRequestMessage requestMessage = this.CreateRequestMessage(HttpMethod.Put, requestUri);
      requestMessage.Content = content;
      return this.SendAsync(requestMessage, cancellationToken);
    }

    /// <summary>Sends a PATCH request to a Uri designated as a string as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PatchAsync(
      string? requestUri,
      HttpContent? content)
    {
      return this.PatchAsync(this.CreateUri(requestUri), content);
    }

    /// <summary>Sends a PATCH request as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PatchAsync(
      Uri? requestUri,
      HttpContent? content)
    {
      return this.PatchAsync(requestUri, content, CancellationToken.None);
    }

    /// <summary>Sends a PATCH request with a cancellation token to a Uri represented as a string as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PatchAsync(
      string? requestUri,
      HttpContent? content,
      CancellationToken cancellationToken)
    {
      return this.PatchAsync(this.CreateUri(requestUri), content, cancellationToken);
    }

    /// <summary>Sends a PATCH request with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> PatchAsync(
      Uri? requestUri,
      HttpContent? content,
      CancellationToken cancellationToken)
    {
      HttpRequestMessage requestMessage = this.CreateRequestMessage(HttpMethod.Patch, requestUri);
      requestMessage.Content = content;
      return this.SendAsync(requestMessage, cancellationToken);
    }

    /// <summary>Send a DELETE request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.
    /// 
    /// -or-
    /// 
    /// The <paramref name="requestUri" /> is not an absolute URI.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> is not set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> DeleteAsync(string? requestUri) => this.DeleteAsync(this.CreateUri(requestUri));

    /// <summary>Send a DELETE request to the specified Uri as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.
    /// 
    /// -or-
    /// 
    /// The <paramref name="requestUri" /> is not an absolute URI.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> is not set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri) => this.DeleteAsync(requestUri, CancellationToken.None);

    /// <summary>Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.
    /// 
    /// -or-
    /// 
    /// The <paramref name="requestUri" /> is not an absolute URI.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> is not set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> DeleteAsync(
      string? requestUri,
      CancellationToken cancellationToken)
    {
      return this.DeleteAsync(this.CreateUri(requestUri), cancellationToken);
    }

    /// <summary>Send a DELETE request to the specified Uri with a cancellation token as an asynchronous operation.</summary>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.
    /// 
    /// -or-
    /// 
    /// The <paramref name="requestUri" /> is not an absolute URI.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Net.Http.HttpClient.BaseAddress" /> is not set.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> DeleteAsync(
      Uri? requestUri,
      CancellationToken cancellationToken)
    {
      return this.SendAsync(this.CreateRequestMessage(HttpMethod.Delete, requestUri), cancellationToken);
    }

    /// <summary>Sends an HTTP request with the specified request.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The HTTP version is 2.0 or higher or the version policy is set to <see cref="F:System.Net.Http.HttpVersionPolicy.RequestVersionOrHigher" />.
    /// 
    ///  -or-
    /// 
    /// The custom class derived from <see cref="T:System.Net.Http.HttpContent" /> does not override the <see cref="M:System.Net.Http.HttpContent.SerializeToStream(System.IO.Stream,System.Net.TransportContext,System.Threading.CancellationToken)" /> method.
    /// 
    ///  -or-
    /// 
    /// The custom <see cref="T:System.Net.Http.HttpMessageHandler" /> does not override the <see cref="M:System.Net.Http.HttpMessageHandler.Send(System.Net.Http.HttpRequestMessage,System.Threading.CancellationToken)" /> method.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, or server certificate validation.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">If the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> exception nests the <see cref="T:System.TimeoutException" />:
    /// The request failed due to timeout.</exception>
    /// <returns>An HTTP response message.</returns>
    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request) => this.Send(request, HttpCompletionOption.ResponseContentRead, new CancellationToken());

    /// <summary>Sends an HTTP request.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="completionOption">One of the enumeration values that specifies when the operation should complete (as soon as a response is available or after reading the response content).</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The HTTP version is 2.0 or higher or the version policy is set to <see cref="F:System.Net.Http.HttpVersionPolicy.RequestVersionOrHigher" />.
    /// 
    ///  -or-
    /// 
    ///  The custom class derived from <see cref="T:System.Net.Http.HttpContent" /> does not override the <see cref="M:System.Net.Http.HttpContent.SerializeToStream(System.IO.Stream,System.Net.TransportContext,System.Threading.CancellationToken)" /> method.
    /// 
    ///  -or-
    /// 
    /// The custom <see cref="T:System.Net.Http.HttpMessageHandler" /> does not override the <see cref="M:System.Net.Http.HttpMessageHandler.Send(System.Net.Http.HttpRequestMessage,System.Threading.CancellationToken)" /> method.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, or server certificate validation.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">If the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> exception nests the <see cref="T:System.TimeoutException" />:
    /// The request failed due to timeout.</exception>
    /// <returns>The HTTP response message.</returns>
    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(
      HttpRequestMessage request,
      HttpCompletionOption completionOption)
    {
      return this.Send(request, completionOption, new CancellationToken());
    }

    /// <summary>Sends an HTTP request with the specified request and cancellation token.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The HTTP version is 2.0 or higher or the version policy is set to <see cref="F:System.Net.Http.HttpVersionPolicy.RequestVersionOrHigher" />.
    /// 
    ///  -or-
    /// 
    ///  The custom class derived from <see cref="T:System.Net.Http.HttpContent" /> does not override the <see cref="M:System.Net.Http.HttpContent.SerializeToStream(System.IO.Stream,System.Net.TransportContext,System.Threading.CancellationToken)" /> method.
    /// 
    ///  -or-
    /// 
    /// The custom <see cref="T:System.Net.Http.HttpMessageHandler" /> does not override the <see cref="M:System.Net.Http.HttpMessageHandler.Send(System.Net.Http.HttpRequestMessage,System.Threading.CancellationToken)" /> method.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, or server certificate validation.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The request was canceled.
    /// 
    /// -or-
    /// 
    /// If the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> exception nests the <see cref="T:System.TimeoutException" />:
    /// The request failed due to timeout.</exception>
    /// <returns>The HTTP response message.</returns>
    [UnsupportedOSPlatform("browser")]
    public override HttpResponseMessage Send(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      return this.Send(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    /// <summary>Sends an HTTP request with the specified request, completion option and cancellation token.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="completionOption">One of the enumeration values that specifies when the operation should complete (as soon as a response is available or after reading the response content).</param>
    /// <param name="cancellationToken">The token to cancel the operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The HTTP version is 2.0 or higher or the version policy is set to <see cref="F:System.Net.Http.HttpVersionPolicy.RequestVersionOrHigher" />.
    /// 
    ///  -or-
    /// 
    ///  The custom class derived from <see cref="T:System.Net.Http.HttpContent" /> does not override the <see cref="M:System.Net.Http.HttpContent.SerializeToStream(System.IO.Stream,System.Net.TransportContext,System.Threading.CancellationToken)" /> method.
    /// 
    ///  -or-
    /// 
    /// The custom <see cref="T:System.Net.Http.HttpMessageHandler" /> does not override the <see cref="M:System.Net.Http.HttpMessageHandler.Send(System.Net.Http.HttpRequestMessage,System.Threading.CancellationToken)" /> method.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, or server certificate validation.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The request was canceled.
    /// 
    /// -or-
    /// 
    /// If the <see cref="T:System.Threading.Tasks.TaskCanceledException" /> exception nests the <see cref="T:System.TimeoutException" />:
    /// The request failed due to timeout.</exception>
    /// <returns>The HTTP response message.</returns>
    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(
      HttpRequestMessage request,
      HttpCompletionOption completionOption,
      CancellationToken cancellationToken)
    {
      this.CheckRequestBeforeSend(request);
      (CancellationTokenSource cancellationTokenSource1, bool flag, CancellationTokenSource cancellationTokenSource2) = this.PrepareCancellationTokenSource(cancellationToken);
      bool telemetryStarted = HttpClient.StartSend(request);
      bool responseContentTelemetryStarted = false;
      HttpResponseMessage response = (HttpResponseMessage) null;
      try
      {
        response = base.Send(request, cancellationTokenSource1.Token);
        HttpClient.ThrowForNullResponse(response);
        if (HttpClient.ShouldBufferResponse(completionOption, request))
        {
          if (HttpTelemetry.Log.IsEnabled() & telemetryStarted)
          {
            HttpTelemetry.Log.ResponseContentStart();
            responseContentTelemetryStarted = true;
          }
          response.Content.LoadIntoBuffer((long) this._maxResponseContentBufferSize, cancellationTokenSource1.Token);
        }
        return response;
      }
      catch (Exception ex)
      {
        this.HandleFailure(ex, telemetryStarted, response, cancellationTokenSource1, cancellationToken, cancellationTokenSource2);
        throw;
      }
      finally
      {
        HttpClient.FinishSend(cancellationTokenSource1, flag, telemetryStarted, responseContentTelemetryStarted);
      }
    }

    /// <summary>Send an HTTP request as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => this.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None);

    /// <summary>Send an HTTP request as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      return this.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    /// <summary>Send an HTTP request as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="completionOption">When the operation should complete (as soon as a response is available or after reading the whole response content).</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      HttpCompletionOption completionOption)
    {
      return this.SendAsync(request, completionOption, CancellationToken.None);
    }

    /// <summary>Send an HTTP request as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="completionOption">When the operation should complete (as soon as a response is available or after reading the whole response content).</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The request message was already sent by the <see cref="T:System.Net.Http.HttpClient" /> instance.</exception>
    /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">.NET Core and .NET 5.0 and later only: The request failed due to timeout.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      HttpCompletionOption completionOption,
      CancellationToken cancellationToken)
    {
      this.CheckRequestBeforeSend(request);
      (CancellationTokenSource cancellationTokenSource1, bool flag, CancellationTokenSource cancellationTokenSource2) = this.PrepareCancellationTokenSource(cancellationToken);
      return Core(request, completionOption, cancellationTokenSource1, flag, cancellationTokenSource2, cancellationToken);


      #nullable disable
      async Task<HttpResponseMessage> Core(
        HttpRequestMessage request,
        HttpCompletionOption completionOption,
        CancellationTokenSource cts,
        bool disposeCts,
        CancellationTokenSource pendingRequestsCts,
        CancellationToken originalCancellationToken)
      {
        bool telemetryStarted = HttpClient.StartSend(request);
        bool responseContentTelemetryStarted = false;
        HttpResponseMessage response = (HttpResponseMessage) null;
        HttpResponseMessage httpResponseMessage;
        try
        {
          response = await base.SendAsync(request, cts.Token).ConfigureAwait(false);
          HttpClient.ThrowForNullResponse(response);
          if (HttpClient.ShouldBufferResponse(completionOption, request))
          {
            if (HttpTelemetry.Log.IsEnabled() & telemetryStarted)
            {
              HttpTelemetry.Log.ResponseContentStart();
              responseContentTelemetryStarted = true;
            }
            await response.Content.LoadIntoBufferAsync((long) this._maxResponseContentBufferSize, cts.Token).ConfigureAwait(false);
          }
          httpResponseMessage = response;
        }
        catch (Exception ex)
        {
          this.HandleFailure(ex, telemetryStarted, response, cts, originalCancellationToken, pendingRequestsCts);
          throw;
        }
        finally
        {
          HttpClient.FinishSend(cts, disposeCts, telemetryStarted, responseContentTelemetryStarted);
        }
        response = (HttpResponseMessage) null;
        return httpResponseMessage;
      }
    }

    private void CheckRequestBeforeSend(HttpRequestMessage request)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request), SR.net_http_handler_norequest);
      this.CheckDisposed();
      HttpClient.CheckRequestMessage(request);
      this.SetOperationStarted();
      this.PrepareRequestMessage(request);
    }

    private static void ThrowForNullResponse([NotNull] HttpResponseMessage response)
    {
      if (response == null)
        throw new InvalidOperationException(SR.net_http_handler_noresponse);
    }

    private static bool ShouldBufferResponse(
      HttpCompletionOption completionOption,
      HttpRequestMessage request)
    {
      return completionOption == HttpCompletionOption.ResponseContentRead && !string.Equals(request.Method.Method, "HEAD", StringComparison.OrdinalIgnoreCase);
    }

    private void HandleFailure(
      Exception e,
      bool telemetryStarted,
      HttpResponseMessage response,
      CancellationTokenSource cts,
      CancellationToken cancellationToken,
      CancellationTokenSource pendingRequestsCts)
    {
      HttpMessageInvoker.LogRequestFailed(telemetryStarted);
      response?.Dispose();
      Exception exception = (Exception) null;
      if (e is OperationCanceledException canceledException)
      {
        if (cancellationToken.IsCancellationRequested)
        {
          if (canceledException.CancellationToken != cancellationToken)
            e = exception = (Exception) new TaskCanceledException(canceledException.Message, canceledException.InnerException, cancellationToken);
        }
        else if (!pendingRequestsCts.IsCancellationRequested)
          e = exception = (Exception) new TaskCanceledException(SR.Format(SR.net_http_request_timedout, (object) this._timeout.TotalSeconds), (Exception) new TimeoutException(e.Message, e), canceledException.CancellationToken);
      }
      else if (e is HttpRequestException && cts.IsCancellationRequested)
      {
        CancellationToken token = cancellationToken.IsCancellationRequested ? cancellationToken : cts.Token;
        e = exception = (Exception) new OperationCanceledException(token);
      }
      if (NetEventSource.Log.IsEnabled())
        NetEventSource.Error((object) this, (object) e, nameof (HandleFailure));
      if (exception != null)
        throw exception;
    }

    private static bool StartSend(HttpRequestMessage request)
    {
      if (!HttpTelemetry.Log.IsEnabled())
        return false;
      HttpTelemetry.Log.RequestStart(request);
      return true;
    }

    private static void FinishSend(
      CancellationTokenSource cts,
      bool disposeCts,
      bool telemetryStarted,
      bool responseContentTelemetryStarted)
    {
      if (HttpTelemetry.Log.IsEnabled() & telemetryStarted)
      {
        if (responseContentTelemetryStarted)
          HttpTelemetry.Log.ResponseContentStop();
        HttpTelemetry.Log.RequestStop();
      }
      if (!disposeCts)
        return;
      cts.Dispose();
    }

    /// <summary>Cancel all pending requests on this instance.</summary>
    public void CancelPendingRequests()
    {
      this.CheckDisposed();
      CancellationTokenSource cancellationTokenSource = Interlocked.Exchange<CancellationTokenSource>(ref this._pendingRequestsCts, new CancellationTokenSource());
      cancellationTokenSource.Cancel();
      cancellationTokenSource.Dispose();
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpClient" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && !this._disposed)
      {
        this._disposed = true;
        this._pendingRequestsCts.Cancel();
        this._pendingRequestsCts.Dispose();
      }
      base.Dispose(disposing);
    }

    private void SetOperationStarted()
    {
      if (this._operationStarted)
        return;
      this._operationStarted = true;
    }

    private void CheckDisposedOrStarted()
    {
      this.CheckDisposed();
      if (this._operationStarted)
        throw new InvalidOperationException(SR.net_http_operation_started);
    }

    private void CheckDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().ToString());
    }

    private static void CheckRequestMessage(HttpRequestMessage request)
    {
      if (!request.MarkAsSent())
        throw new InvalidOperationException(SR.net_http_client_request_already_sent);
    }

    private void PrepareRequestMessage(HttpRequestMessage request)
    {
      Uri uri = (Uri) null;
      if (request.RequestUri == (Uri) null && this._baseAddress == (Uri) null)
        throw new InvalidOperationException(SR.net_http_client_invalid_requesturi);
      if (request.RequestUri == (Uri) null)
        uri = this._baseAddress;
      else if (!request.RequestUri.IsAbsoluteUri)
      {
        if (this._baseAddress == (Uri) null)
          throw new InvalidOperationException(SR.net_http_client_invalid_requesturi);
        uri = new Uri(this._baseAddress, request.RequestUri);
      }
      if (uri != (Uri) null)
        request.RequestUri = uri;
      if (this._defaultRequestHeaders == null)
        return;
      request.Headers.AddHeaders((HttpHeaders) this._defaultRequestHeaders);
    }

    private (CancellationTokenSource TokenSource, bool DisposeTokenSource, CancellationTokenSource PendingRequestsCts) PrepareCancellationTokenSource(
      CancellationToken cancellationToken)
    {
      CancellationTokenSource pendingRequestsCts = this._pendingRequestsCts;
      bool flag = this._timeout != HttpClient.s_infiniteTimeout;
      if (!flag && !cancellationToken.CanBeCanceled)
        return (pendingRequestsCts, false, pendingRequestsCts);
      CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, pendingRequestsCts.Token);
      if (flag)
        linkedTokenSource.CancelAfter(this._timeout);
      return (linkedTokenSource, true, pendingRequestsCts);
    }

    private Uri CreateUri(string uri) => !string.IsNullOrEmpty(uri) ? new Uri(uri, UriKind.RelativeOrAbsolute) : (Uri) null;

    private HttpRequestMessage CreateRequestMessage(HttpMethod method, Uri uri) => new HttpRequestMessage(method, uri)
    {
      Version = this._defaultRequestVersion,
      VersionPolicy = this._defaultVersionPolicy
    };
  }
}
