// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpMessageInvoker
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>A specialty class that allows applications to call the <see cref="M:System.Net.Http.HttpMessageInvoker.SendAsync(System.Net.Http.HttpRequestMessage,System.Threading.CancellationToken)" /> method on an HTTP handler chain.</summary>
  public class HttpMessageInvoker : IDisposable
  {
    private volatile bool _disposed;
    private readonly bool _disposeHandler;

    #nullable disable
    private readonly HttpMessageHandler _handler;


    #nullable enable
    /// <summary>Initializes an instance of a <see cref="T:System.Net.Http.HttpMessageInvoker" /> class with a specific <see cref="T:System.Net.Http.HttpMessageHandler" />.</summary>
    /// <param name="handler">The <see cref="T:System.Net.Http.HttpMessageHandler" /> responsible for processing the HTTP response messages.</param>
    public HttpMessageInvoker(HttpMessageHandler handler)
      : this(handler, true)
    {
    }

    /// <summary>Initializes an instance of a <see cref="T:System.Net.Http.HttpMessageInvoker" /> class with a specific <see cref="T:System.Net.Http.HttpMessageHandler" />.</summary>
    /// <param name="handler">The <see cref="T:System.Net.Http.HttpMessageHandler" /> responsible for processing the HTTP response messages.</param>
    /// <param name="disposeHandler">
    /// <see langword="true" /> if the inner handler should be disposed of by Dispose(), <see langword="false" /> if you intend to reuse the inner handler.</param>
    public HttpMessageInvoker(HttpMessageHandler handler, bool disposeHandler)
    {
      if (handler == null)
        throw new ArgumentNullException(nameof (handler));
      if (NetEventSource.Log.IsEnabled())
        NetEventSource.Associate((object) this, (object) handler, ".ctor");
      this._handler = handler;
      this._disposeHandler = disposeHandler;
    }

    /// <summary>Sends an HTTP request with the specified request and cancellation token.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
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
    /// <returns>The HTTP response message.</returns>
    [UnsupportedOSPlatform("browser")]
    public virtual HttpResponseMessage Send(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request));
      this.CheckDisposed();
      if (!HttpMessageInvoker.ShouldSendWithTelemetry(request))
        return this._handler.Send(request, cancellationToken);
      HttpTelemetry.Log.RequestStart(request);
      try
      {
        return this._handler.Send(request, cancellationToken);
      }
      catch (Exception ex) when (HttpMessageInvoker.LogRequestFailed(true))
      {
        throw;
      }
      finally
      {
        HttpTelemetry.Log.RequestStop();
      }
    }

    /// <summary>Send an HTTP request as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public virtual Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request));
      this.CheckDisposed();
      return HttpMessageInvoker.ShouldSendWithTelemetry(request) ? SendAsyncWithTelemetry(this._handler, request, cancellationToken) : this._handler.SendAsync(request, cancellationToken);


      #nullable disable
      static async Task<HttpResponseMessage> SendAsyncWithTelemetry(
        HttpMessageHandler handler,
        HttpRequestMessage request,
        CancellationToken cancellationToken)
      {
        HttpTelemetry.Log.RequestStart(request);
        HttpResponseMessage httpResponseMessage;
        try
        {
          httpResponseMessage = await handler.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (HttpMessageInvoker.LogRequestFailed(true))
        {
          throw;
        }
        finally
        {
          HttpTelemetry.Log.RequestStop();
        }
        return httpResponseMessage;
      }
    }

    private static bool ShouldSendWithTelemetry(HttpRequestMessage request)
    {
      if (HttpTelemetry.Log.IsEnabled() && !request.WasSentByHttpClient())
      {
        Uri requestUri = request.RequestUri;
        if ((object) requestUri != null)
          return requestUri.IsAbsoluteUri;
      }
      return false;
    }

    internal static bool LogRequestFailed(bool telemetryStarted)
    {
      if (HttpTelemetry.Log.IsEnabled() & telemetryStarted)
        HttpTelemetry.Log.RequestFailed();
      return false;
    }

    /// <summary>Releases the unmanaged resources and disposes of the managed resources used by the <see cref="T:System.Net.Http.HttpMessageInvoker" />.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpMessageInvoker" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._disposed)
        return;
      this._disposed = true;
      if (!this._disposeHandler)
        return;
      this._handler.Dispose();
    }

    private void CheckDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().ToString());
    }
  }
}
