// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskCompletionSource`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Represents the producer side of a <see cref="T:System.Threading.Tasks.Task`1" /> unbound to a delegate, providing access to the consumer side through the <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> property.</summary>
  /// <typeparam name="TResult">The type of the result value associated with this <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.</typeparam>
  public class TaskCompletionSource<TResult>
  {

    #nullable disable
    private readonly System.Threading.Tasks.Task<TResult> _task;

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.</summary>
    public TaskCompletionSource() => this._task = new System.Threading.Tasks.Task<TResult>();

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" /> with the specified options.</summary>
    /// <param name="creationOptions">The options to use when creating the underlying <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> represent options invalid for use with a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.</exception>
    public TaskCompletionSource(TaskCreationOptions creationOptions)
      : this((object) null, creationOptions)
    {
    }


    #nullable enable
    /// <summary>Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" /> with the specified state.</summary>
    /// <param name="state">The state to use as the underlying <see cref="T:System.Threading.Tasks.Task`1" />'s AsyncState.</param>
    public TaskCompletionSource(object? state)
      : this(state, TaskCreationOptions.None)
    {
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" /> with the specified state and options.</summary>
    /// <param name="state">The state to use as the underlying <see cref="T:System.Threading.Tasks.Task`1" />'s AsyncState.</param>
    /// <param name="creationOptions">The options to use when creating the underlying <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> represent options invalid for use with a <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.</exception>
    public TaskCompletionSource(object? state, TaskCreationOptions creationOptions) => this._task = new System.Threading.Tasks.Task<TResult>(state, creationOptions);

    /// <summary>Gets the <see cref="T:System.Threading.Tasks.Task`1" /> created by this <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.</summary>
    /// <returns>Returns the <see cref="T:System.Threading.Tasks.Task`1" /> created by this <see cref="T:System.Threading.Tasks.TaskCompletionSource`1" />.</returns>
    public System.Threading.Tasks.Task<TResult> Task => this._task;

    /// <summary>Transitions the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and binds it to a specified exception.</summary>
    /// <param name="exception">The exception to bind to this <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The underlying <see cref="T:System.Threading.Tasks.Task`1" /> is already in one of the three final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />.</exception>
    public void SetException(Exception exception)
    {
      if (this.TrySetException(exception))
        return;
      ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
    }

    /// <summary>Transitions the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and binds a collection of exception objects to it.</summary>
    /// <param name="exceptions">The collection of exceptions to bind to this <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="exceptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">There are one or more null elements in <paramref name="exceptions" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The underlying <see cref="T:System.Threading.Tasks.Task`1" /> is already in one of the three final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />.</exception>
    public void SetException(IEnumerable<Exception> exceptions)
    {
      if (this.TrySetException(exceptions))
        return;
      ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
    }

    /// <summary>Attempts to transition the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and binds it to a specified exception.</summary>
    /// <param name="exception">The exception to bind to this <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="exception" /> argument is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.</returns>
    public bool TrySetException(Exception exception)
    {
      if (exception == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exception);
      bool flag = this._task.TrySetException((object) exception);
      if (!flag && !this._task.IsCompleted)
        this._task.SpinUntilCompleted();
      return flag;
    }

    /// <summary>Attempts to transition the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" /> state and binds a collection of exception objects to it.</summary>
    /// <param name="exceptions">The collection of exceptions to bind to this <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="exceptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">There are one or more null elements in <paramref name="exceptions" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="exceptions" /> collection is empty.</exception>
    /// <returns>
    /// <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.</returns>
    public bool TrySetException(IEnumerable<Exception> exceptions)
    {
      if (exceptions == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exceptions);
      List<Exception> exceptionObject = new List<Exception>();
      foreach (Exception exception in exceptions)
      {
        if (exception == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.TaskCompletionSourceT_TrySetException_NullException, ExceptionArgument.exceptions);
        exceptionObject.Add(exception);
      }
      if (exceptionObject.Count == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.TaskCompletionSourceT_TrySetException_NoExceptions, ExceptionArgument.exceptions);
      bool flag = this._task.TrySetException((object) exceptionObject);
      if (!flag && !this._task.IsCompleted)
        this._task.SpinUntilCompleted();
      return flag;
    }

    /// <summary>Transitions the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" /> state.</summary>
    /// <param name="result">The result value to bind to this <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The underlying <see cref="T:System.Threading.Tasks.Task`1" /> is already in one of the three final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />.</exception>
    public void SetResult(TResult result)
    {
      if (this.TrySetResult(result))
        return;
      ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
    }

    /// <summary>Attempts to transition the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" /> state.</summary>
    /// <param name="result">The result value to bind to this <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.</returns>
    public bool TrySetResult(TResult result)
    {
      bool flag = this._task.TrySetResult(result);
      if (!flag)
        this._task.SpinUntilCompleted();
      return flag;
    }

    /// <summary>Transitions the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" /> state.</summary>
    /// <exception cref="T:System.InvalidOperationException">The underlying <see cref="T:System.Threading.Tasks.Task`1" /> is already in one of the three final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />, or if the underlying <see cref="T:System.Threading.Tasks.Task`1" /> has already been disposed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    public void SetCanceled() => this.SetCanceled(new CancellationToken());

    /// <summary>Transitions the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" /> state using the specified token.</summary>
    /// <param name="cancellationToken">The cancellation token with which to cancel the <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <exception cref="T:System.InvalidOperationException">The underlying <see cref="T:System.Threading.Tasks.Task`1" /> is already in one of the three final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />.</exception>
    public void SetCanceled(CancellationToken cancellationToken)
    {
      if (this.TrySetCanceled(cancellationToken))
        return;
      ThrowHelper.ThrowInvalidOperationException(ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted);
    }

    /// <summary>Attempts to transition the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" /> state.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="P:System.Threading.Tasks.TaskCompletionSource`1.Task" /> was disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the operation was successful; false if the operation was unsuccessful or the object has already been disposed.</returns>
    public bool TrySetCanceled() => this.TrySetCanceled(new CancellationToken());

    /// <summary>Attempts to transition the underlying <see cref="T:System.Threading.Tasks.Task`1" /> into the <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" /> state and enables a cancellation token to be stored in the canceled task.</summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>
    /// <see langword="true" /> if the operation is successful; otherwise, <see langword="false" />.</returns>
    public bool TrySetCanceled(CancellationToken cancellationToken)
    {
      bool flag = this._task.TrySetCanceled(cancellationToken);
      if (!flag && !this._task.IsCompleted)
        this._task.SpinUntilCompleted();
      return flag;
    }
  }
}
