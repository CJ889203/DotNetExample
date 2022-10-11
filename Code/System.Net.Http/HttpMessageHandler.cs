// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpMessageHandler
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>A base type for HTTP message handlers.</summary>
  public abstract class HttpMessageHandler : IDisposable
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpMessageHandler" /> class.</summary>
    protected HttpMessageHandler()
    {
      if (!NetEventSource.Log.IsEnabled())
        return;
      NetEventSource.Info((object) this, (FormattableString) null, ".ctor");
    }

    /// <summary>When overridden in a derived class, sends an HTTP request with the specified request and cancellation token. Otherwise, throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="T:System.NotSupportedException">The method is not overridden in the derived class.</exception>
    /// <returns>The HTTP response message.</returns>
    protected internal virtual HttpResponseMessage Send(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      throw new NotSupportedException(SR.Format(SR.net_http_missing_sync_implementation, (object) this.GetType(), (object) nameof (HttpMessageHandler), (object) nameof (Send)));
    }

    /// <summary>Send an HTTP request as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected internal abstract Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken);

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpMessageHandler" /> and optionally disposes of the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    /// <summary>Releases the unmanaged resources and disposes of the managed resources used by the <see cref="T:System.Net.Http.HttpMessageHandler" />.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
