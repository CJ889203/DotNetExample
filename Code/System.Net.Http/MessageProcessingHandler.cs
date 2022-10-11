// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MessageProcessingHandler
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http
{
  /// <summary>A base type for handlers which only do some small processing of request and/or response messages.</summary>
  public abstract class MessageProcessingHandler : DelegatingHandler
  {
    /// <summary>Creates an instance of a <see cref="T:System.Net.Http.MessageProcessingHandler" /> class.</summary>
    protected MessageProcessingHandler()
    {
    }

    /// <summary>Creates an instance of a <see cref="T:System.Net.Http.MessageProcessingHandler" /> class with a specific inner handler.</summary>
    /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
    protected MessageProcessingHandler(HttpMessageHandler innerHandler)
      : base(innerHandler)
    {
    }

    /// <summary>Performs processing on each request sent to the server.</summary>
    /// <param name="request">The HTTP request message to process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The HTTP request message that was processed.</returns>
    protected abstract HttpRequestMessage ProcessRequest(
      HttpRequestMessage request,
      CancellationToken cancellationToken);

    /// <summary>Perform processing on each response from the server.</summary>
    /// <param name="response">The HTTP response message to process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The HTTP response message that was processed.</returns>
    protected abstract HttpResponseMessage ProcessResponse(
      HttpResponseMessage response,
      CancellationToken cancellationToken);

    /// <summary>Sends an HTTP request to the inner handler to send to the server.</summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The HTTP response message.</returns>
    protected internal override sealed HttpResponseMessage Send(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      return request != null ? this.ProcessResponse(base.Send(this.ProcessRequest(request, cancellationToken), cancellationToken), cancellationToken) : throw new ArgumentNullException(nameof (request), SR.net_http_handler_norequest);
    }

    /// <summary>Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.</summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="request" /> was <see langword="null" />.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected internal override sealed Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request), SR.net_http_handler_norequest);
      MessageProcessingHandler.SendState sendState = new MessageProcessingHandler.SendState(this, cancellationToken);
      try
      {
        base.SendAsync(this.ProcessRequest(request, cancellationToken), cancellationToken).ContinueWith((Action<Task<HttpResponseMessage>, object>) ((task, state) =>
        {
          MessageProcessingHandler.SendState tcs = (MessageProcessingHandler.SendState) state;
          MessageProcessingHandler handler = tcs._handler;
          CancellationToken token = tcs._token;
          if (task.IsFaulted)
            tcs.TrySetException(task.Exception.GetBaseException());
          else if (task.IsCanceled)
            tcs.TrySetCanceled(token);
          else if (task.Result == null)
          {
            tcs.TrySetException(ExceptionDispatchInfo.SetCurrentStackTrace((Exception) new InvalidOperationException(SR.net_http_handler_noresponse)));
          }
          else
          {
            try
            {
              HttpResponseMessage result = handler.ProcessResponse(task.Result, token);
              tcs.TrySetResult(result);
            }
            catch (OperationCanceledException ex)
            {
              MessageProcessingHandler.HandleCanceledOperations(token, (TaskCompletionSource<HttpResponseMessage>) tcs, ex);
            }
            catch (Exception ex)
            {
              tcs.TrySetException(ex);
            }
          }
        }), (object) sendState, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
      }
      catch (OperationCanceledException ex)
      {
        MessageProcessingHandler.HandleCanceledOperations(cancellationToken, (TaskCompletionSource<HttpResponseMessage>) sendState, ex);
      }
      catch (Exception ex)
      {
        sendState.TrySetException(ex);
      }
      return sendState.Task;
    }


    #nullable disable
    private static void HandleCanceledOperations(
      CancellationToken cancellationToken,
      TaskCompletionSource<HttpResponseMessage> tcs,
      OperationCanceledException e)
    {
      if (cancellationToken.IsCancellationRequested && e.CancellationToken == cancellationToken)
        tcs.TrySetCanceled(cancellationToken);
      else
        tcs.TrySetException((Exception) e);
    }

    private sealed class SendState : TaskCompletionSource<HttpResponseMessage>
    {
      internal readonly MessageProcessingHandler _handler;
      internal readonly CancellationToken _token;

      public SendState(MessageProcessingHandler handler, CancellationToken token)
      {
        this._handler = handler;
        this._token = token;
      }
    }
  }
}
