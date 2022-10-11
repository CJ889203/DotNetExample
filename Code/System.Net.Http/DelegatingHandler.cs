// Decompiled with JetBrains decompiler
// Type: System.Net.Http.DelegatingHandler
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>A type for HTTP handlers that delegate the processing of HTTP response messages to another handler, called the inner handler.</summary>
  public abstract class DelegatingHandler : HttpMessageHandler
  {

    #nullable disable
    private HttpMessageHandler _innerHandler;
    private volatile bool _operationStarted;
    private volatile bool _disposed;


    #nullable enable
    /// <summary>Gets or sets the inner handler which processes the HTTP response messages.</summary>
    /// <returns>The inner handler for HTTP response messages.</returns>
    public HttpMessageHandler? InnerHandler
    {
      get => this._innerHandler;
      [param: DisallowNull] set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.CheckDisposedOrStarted();
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Associate((object) this, (object) value, nameof (InnerHandler));
        this._innerHandler = value;
      }
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.DelegatingHandler" /> class.</summary>
    protected DelegatingHandler()
    {
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.DelegatingHandler" /> class with a specific inner handler.</summary>
    /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
    protected DelegatingHandler(HttpMessageHandler innerHandler) => this.InnerHandler = innerHandler;

    /// <summary>Sends an HTTP request to the inner handler to send to the server.</summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>An HTTP response message.</returns>
    protected internal override HttpResponseMessage Send(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request), SR.net_http_handler_norequest);
      this.SetOperationStarted();
      return this._innerHandler.Send(request, cancellationToken);
    }

    /// <summary>Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected internal override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request), SR.net_http_handler_norequest);
      this.SetOperationStarted();
      return this._innerHandler.SendAsync(request, cancellationToken);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.DelegatingHandler" />, and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && !this._disposed)
      {
        this._disposed = true;
        if (this._innerHandler != null)
          this._innerHandler.Dispose();
      }
      base.Dispose(disposing);
    }

    private void CheckDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().ToString());
    }

    private void CheckDisposedOrStarted()
    {
      this.CheckDisposed();
      if (this._operationStarted)
        throw new InvalidOperationException(SR.net_http_operation_started);
    }

    private void SetOperationStarted()
    {
      this.CheckDisposed();
      if (this._innerHandler == null)
        throw new InvalidOperationException(SR.net_http_handler_not_assigned);
      if (this._operationStarted)
        return;
      this._operationStarted = true;
    }
  }
}
