// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskFactory`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides support for creating and scheduling <see cref="T:System.Threading.Tasks.Task`1" /> objects.</summary>
  /// <typeparam name="TResult">The return value of the <see cref="T:System.Threading.Tasks.Task`1" /> objects that the methods of this class create.</typeparam>
  public class TaskFactory<TResult>
  {
    private readonly CancellationToken m_defaultCancellationToken;

    #nullable disable
    private readonly TaskScheduler m_defaultScheduler;
    private readonly TaskCreationOptions m_defaultCreationOptions;
    private readonly TaskContinuationOptions m_defaultContinuationOptions;


    #nullable enable
    private TaskScheduler DefaultScheduler => this.m_defaultScheduler ?? TaskScheduler.Current;


    #nullable disable
    private TaskScheduler GetDefaultScheduler(Task currTask)
    {
      if (this.m_defaultScheduler != null)
        return this.m_defaultScheduler;
      return currTask != null && (currTask.CreationOptions & TaskCreationOptions.HideScheduler) == TaskCreationOptions.None ? currTask.ExecutingTaskScheduler : TaskScheduler.Default;
    }

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the default configuration.</summary>
    public TaskFactory()
    {
    }

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the default configuration.</summary>
    /// <param name="cancellationToken">The default cancellation token that will be assigned to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another cancellation token is explicitly specified when calling the factory methods.</param>
    public TaskFactory(CancellationToken cancellationToken) => this.m_defaultCancellationToken = cancellationToken;


    #nullable enable
    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.</summary>
    /// <param name="scheduler">The scheduler to use to schedule any tasks created with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />. A null value indicates that the current <see cref="T:System.Threading.Tasks.TaskScheduler" /> should be used.</param>
    public TaskFactory(TaskScheduler? scheduler) => this.m_defaultScheduler = scheduler;

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.</summary>
    /// <param name="creationOptions">The default options to use when creating tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
    /// <param name="continuationOptions">The default options to use when creating continuation tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> or <paramref name="continuationOptions" /> specifies an invalid value.</exception>
    public TaskFactory(
      TaskCreationOptions creationOptions,
      TaskContinuationOptions continuationOptions)
    {
      TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
      TaskFactory.CheckCreationOptions(creationOptions);
      this.m_defaultCreationOptions = creationOptions;
      this.m_defaultContinuationOptions = continuationOptions;
    }

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory`1" /> instance with the specified configuration.</summary>
    /// <param name="cancellationToken">The default cancellation token that will be assigned to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another cancellation token is explicitly specified when calling the factory methods.</param>
    /// <param name="creationOptions">The default options to use when creating tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
    /// <param name="continuationOptions">The default options to use when creating continuation tasks with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />.</param>
    /// <param name="scheduler">The default scheduler to use to schedule any tasks created with this <see cref="T:System.Threading.Tasks.TaskFactory`1" />. A null value indicates that <see cref="P:System.Threading.Tasks.TaskScheduler.Current" /> should be used.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> or <paramref name="continuationOptions" /> specifies an invalid value.</exception>
    public TaskFactory(
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      TaskContinuationOptions continuationOptions,
      TaskScheduler? scheduler)
      : this(creationOptions, continuationOptions)
    {
      this.m_defaultCancellationToken = cancellationToken;
      this.m_defaultScheduler = scheduler;
    }

    /// <summary>Gets the default cancellation token for this task factory.</summary>
    /// <returns>The default cancellation token for this task factory.</returns>
    public CancellationToken CancellationToken => this.m_defaultCancellationToken;

    /// <summary>Gets the task scheduler for this task factory.</summary>
    /// <returns>The task scheduler for this task factory.</returns>
    public TaskScheduler? Scheduler => this.m_defaultScheduler;

    /// <summary>Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> enumeration value for this task factory.</summary>
    /// <returns>One of the enumeration values that specifies the default creation options for this task factory.</returns>
    public TaskCreationOptions CreationOptions => this.m_defaultCreationOptions;

    /// <summary>Gets the <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> enumeration value for this task factory.</summary>
    /// <returns>One of the enumeration values that specifies the default continuation options for this task factory.</returns>
    public TaskContinuationOptions ContinuationOptions => this.m_defaultContinuationOptions;

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew(Func<TResult> function)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, this.m_defaultCancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, cancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The started <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> StartNew(Func<TResult> function, TaskCreationOptions creationOptions)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, this.m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew(
      Func<TResult> function,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
    }

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew(Func<object?, TResult> function, object? state)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, state, this.m_defaultCancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew(
      Func<object?, TResult> function,
      object? state,
      CancellationToken cancellationToken)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, state, cancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew(
      Func<object?, TResult> function,
      object? state,
      TaskCreationOptions creationOptions)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, state, this.m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object that contains data to be used by the <paramref name="function" /> delegate.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The cancellation token source that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew(
      Func<object?, TResult> function,
      object? state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, state, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
    }


    #nullable disable
    private static void FromAsyncCoreLogic(
      IAsyncResult iar,
      Func<IAsyncResult, TResult> endFunction,
      Action<IAsyncResult> endAction,
      Task<TResult> promise,
      bool requiresSynchronization)
    {
      Exception exceptionObject = (Exception) null;
      OperationCanceledException cancellationException = (OperationCanceledException) null;
      TResult result = default (TResult);
      try
      {
        if (endFunction != null)
          result = endFunction(iar);
        else
          endAction(iar);
      }
      catch (OperationCanceledException ex)
      {
        cancellationException = ex;
      }
      catch (Exception ex)
      {
        exceptionObject = ex;
      }
      finally
      {
        if (cancellationException != null)
          promise.TrySetCanceled(cancellationException.CancellationToken, (object) cancellationException);
        else if (exceptionObject != null)
        {
          promise.TrySetException((object) exceptionObject);
        }
        else
        {
          if (TplEventSource.Log.IsEnabled())
            TplEventSource.Log.TraceOperationEnd(promise.Id, AsyncCausalityStatus.Completed);
          if (Task.s_asyncDebuggingEnabled)
            Task.RemoveFromActiveTasks((Task) promise);
          if (requiresSynchronization)
            promise.TrySetResult(result);
          else
            promise.DangerousSetResult(result);
        }
      }
    }


    #nullable enable
    /// <summary>Creates a task that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync(
      IAsyncResult asyncResult,
      Func<IAsyncResult, TResult> endMethod)
    {
      return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, (Action<IAsyncResult>) null, this.m_defaultCreationOptions, this.DefaultScheduler);
    }

    /// <summary>Creates a task that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value.</exception>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync(
      IAsyncResult asyncResult,
      Func<IAsyncResult, TResult> endMethod,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, (Action<IAsyncResult>) null, creationOptions, this.DefaultScheduler);
    }

    /// <summary>Creates a task that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the task that executes the end method.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync(
      IAsyncResult asyncResult,
      Func<IAsyncResult, TResult> endMethod,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, (Action<IAsyncResult>) null, creationOptions, scheduler);
    }


    #nullable disable
    internal static Task<TResult> FromAsyncImpl(
      IAsyncResult asyncResult,
      Func<IAsyncResult, TResult> endFunction,
      Action<IAsyncResult> endAction,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      if (asyncResult == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.asyncResult);
      if (endFunction == null && endAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.endMethod);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskFactory.CheckFromAsyncOptions(creationOptions, false);
      Task<TResult> promise = new Task<TResult>((object) null, creationOptions);
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.TraceOperationBegin(promise.Id, "TaskFactory.FromAsync", 0L);
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks((Task) promise);
      Task t = new Task((Delegate) (_param1 => TaskFactory<TResult>.FromAsyncCoreLogic(asyncResult, endFunction, endAction, promise, true)), (object) null, (Task) null, new CancellationToken(), TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null);
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.TraceOperationBegin(t.Id, "TaskFactory.FromAsync Callback", 0L);
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks(t);
      if (asyncResult.IsCompleted)
      {
        try
        {
          t.InternalRunSynchronously(scheduler, false);
        }
        catch (Exception ex)
        {
          promise.TrySetException((object) ex);
        }
      }
      else
        ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, (WaitOrTimerCallback) ((_param1, _param2) =>
        {
          try
          {
            t.InternalRunSynchronously(scheduler, false);
          }
          catch (Exception ex)
          {
            promise.TrySetException((object) ex);
          }
        }), (object) null, -1, true);
      return promise;
    }


    #nullable enable
    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync(
      Func<AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, (Action<IAsyncResult>) null, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync(
      Func<AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, (Action<IAsyncResult>) null, state, creationOptions);
    }


    #nullable disable
    internal static Task<TResult> FromAsyncImpl(
      Func<AsyncCallback, object, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endFunction,
      Action<IAsyncResult> endAction,
      object state,
      TaskCreationOptions creationOptions)
    {
      if (beginMethod == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.beginMethod);
      if (endFunction == null && endAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.endMethod);
      TaskFactory.CheckFromAsyncOptions(creationOptions, true);
      Task<TResult> promise = new Task<TResult>(state, creationOptions);
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.TraceOperationBegin(promise.Id, "TaskFactory.FromAsync: " + beginMethod.Method.Name, 0L);
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks((Task) promise);
      try
      {
        IAsyncResult iar1 = beginMethod((AsyncCallback) (iar =>
        {
          if (iar.CompletedSynchronously)
            return;
          TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
        }), state);
        if (iar1.CompletedSynchronously)
          TaskFactory<TResult>.FromAsyncCoreLogic(iar1, endFunction, endAction, promise, false);
      }
      catch
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(promise.Id, AsyncCausalityStatus.Error);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) promise);
        promise.TrySetResult();
        throw;
      }
      return promise;
    }


    #nullable enable
    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1>(
      Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1>(
      Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, state, creationOptions);
    }


    #nullable disable
    internal static Task<TResult> FromAsyncImpl<TArg1>(
      Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endFunction,
      Action<IAsyncResult> endAction,
      TArg1 arg1,
      object state,
      TaskCreationOptions creationOptions)
    {
      if (beginMethod == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.beginMethod);
      if (endFunction == null && endAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.endFunction);
      TaskFactory.CheckFromAsyncOptions(creationOptions, true);
      Task<TResult> promise = new Task<TResult>(state, creationOptions);
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.TraceOperationBegin(promise.Id, "TaskFactory.FromAsync: " + beginMethod.Method.Name, 0L);
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks((Task) promise);
      try
      {
        IAsyncResult iar1 = beginMethod(arg1, (AsyncCallback) (iar =>
        {
          if (iar.CompletedSynchronously)
            return;
          TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
        }), state);
        if (iar1.CompletedSynchronously)
          TaskFactory<TResult>.FromAsyncCoreLogic(iar1, endFunction, endAction, promise, false);
      }
      catch
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(promise.Id, AsyncCausalityStatus.Error);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) promise);
        promise.TrySetResult();
        throw;
      }
      return promise;
    }


    #nullable enable
    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2>(
      Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, arg2, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">An object that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2>(
      Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, arg2, state, creationOptions);
    }


    #nullable disable
    internal static Task<TResult> FromAsyncImpl<TArg1, TArg2>(
      Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endFunction,
      Action<IAsyncResult> endAction,
      TArg1 arg1,
      TArg2 arg2,
      object state,
      TaskCreationOptions creationOptions)
    {
      if (beginMethod == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.beginMethod);
      if (endFunction == null && endAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.endMethod);
      TaskFactory.CheckFromAsyncOptions(creationOptions, true);
      Task<TResult> promise = new Task<TResult>(state, creationOptions);
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.TraceOperationBegin(promise.Id, "TaskFactory.FromAsync: " + beginMethod.Method.Name, 0L);
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks((Task) promise);
      try
      {
        IAsyncResult iar1 = beginMethod(arg1, arg2, (AsyncCallback) (iar =>
        {
          if (iar.CompletedSynchronously)
            return;
          TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
        }), state);
        if (iar1.CompletedSynchronously)
          TaskFactory<TResult>.FromAsyncCoreLogic(iar1, endFunction, endAction, promise, false);
      }
      catch
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(promise.Id, AsyncCausalityStatus.Error);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) promise);
        promise.TrySetResult();
        throw;
      }
      return promise;
    }


    #nullable enable
    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2, TArg3>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, arg2, arg3, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a task that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">An object that controls the behavior of the created task.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="beginMethod" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="endMethod" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> parameter specifies an invalid value.</exception>
    /// <returns>The created task that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2, TArg3>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, arg2, arg3, state, creationOptions);
    }


    #nullable disable
    internal static Task<TResult> FromAsyncImpl<TArg1, TArg2, TArg3>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endFunction,
      Action<IAsyncResult> endAction,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      object state,
      TaskCreationOptions creationOptions)
    {
      if (beginMethod == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.beginMethod);
      if (endFunction == null && endAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.endMethod);
      TaskFactory.CheckFromAsyncOptions(creationOptions, true);
      Task<TResult> promise = new Task<TResult>(state, creationOptions);
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.TraceOperationBegin(promise.Id, "TaskFactory.FromAsync: " + beginMethod.Method.Name, 0L);
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks((Task) promise);
      try
      {
        IAsyncResult iar1 = beginMethod(arg1, arg2, arg3, (AsyncCallback) (iar =>
        {
          if (iar.CompletedSynchronously)
            return;
          TaskFactory<TResult>.FromAsyncCoreLogic(iar, endFunction, endAction, promise, true);
        }), state);
        if (iar1.CompletedSynchronously)
          TaskFactory<TResult>.FromAsyncCoreLogic(iar1, endFunction, endAction, promise, false);
      }
      catch
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(promise.Id, AsyncCausalityStatus.Error);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) promise);
        promise.TrySetResult();
        throw;
      }
      return promise;
    }

    internal static Task<TResult> FromAsyncTrim<TInstance, TArgs>(
      TInstance thisRef,
      TArgs args,
      Func<TInstance, TArgs, AsyncCallback, object, IAsyncResult> beginMethod,
      Func<TInstance, IAsyncResult, TResult> endMethod)
      where TInstance : class
    {
      TaskFactory<TResult>.FromAsyncTrimPromise<TInstance> asyncTrimPromise = new TaskFactory<TResult>.FromAsyncTrimPromise<TInstance>(thisRef, endMethod);
      IAsyncResult asyncResult = beginMethod(thisRef, args, TaskFactory<TResult>.FromAsyncTrimPromise<TInstance>.s_completeFromAsyncResult, (object) asyncTrimPromise);
      if (asyncResult.CompletedSynchronously)
        asyncTrimPromise.Complete(thisRef, endMethod, asyncResult, false);
      return (Task<TResult>) asyncTrimPromise;
    }

    private static Task<TResult> CreateCanceledTask(
      TaskContinuationOptions continuationOptions,
      CancellationToken ct)
    {
      TaskCreationOptions creationOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out InternalTaskOptions _);
      return new Task<TResult>(true, default (TResult), creationOptions, ct);
    }


    #nullable enable
    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided Tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided Tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <param name="scheduler">The scheduler that is used to schedule the created continuation task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of a set of provided tasks.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <param name="scheduler">The scheduler that is used to schedule the created continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, continuationOptions, cancellationToken, scheduler);
    }


    #nullable disable
    internal static Task<TResult> ContinueWhenAllImpl<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction,
      Action<Task<TAntecedentResult>[]> continuationAction,
      TaskContinuationOptions continuationOptions,
      CancellationToken cancellationToken,
      TaskScheduler scheduler)
    {
      TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      Task<TAntecedentResult>[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy<TAntecedentResult>(tasks);
      if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
        return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
      Task<Task<TAntecedentResult>[]> task = TaskFactory.CommonCWAllLogic<TAntecedentResult>(tasksCopy);
      return continuationFunction != null ? task.ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAllFuncDelegate, (object) continuationFunction, scheduler, cancellationToken, continuationOptions) : task.ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAllActionDelegate, (object) continuationAction, scheduler, cancellationToken, continuationOptions);
    }

    internal static Task<TResult> ContinueWhenAllImpl(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction,
      Action<Task[]> continuationAction,
      TaskContinuationOptions continuationOptions,
      CancellationToken cancellationToken,
      TaskScheduler scheduler)
    {
      TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      Task[] tasksCopy = TaskFactory.CheckMultiContinuationTasksAndCopy(tasks);
      if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
        return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
      Task<Task[]> task = TaskFactory.CommonCWAllLogic(tasksCopy);
      return continuationFunction != null ? task.ContinueWith<TResult>((Func<Task<Task[]>, object, TResult>) ((completedTasks, state) =>
      {
        completedTasks.NotifyDebuggerOfWaitCompletionIfNecessary();
        return ((Func<Task[], TResult>) state)(completedTasks.Result);
      }), (object) continuationFunction, scheduler, cancellationToken, continuationOptions) : task.ContinueWith<TResult>((Func<Task<Task[]>, object, TResult>) ((completedTasks, state) =>
      {
        completedTasks.NotifyDebuggerOfWaitCompletionIfNecessary();
        ((Action<Task[]>) state)(completedTasks.Result);
        return default (TResult);
      }), (object) continuationAction, scheduler, cancellationToken, continuationOptions);
    }


    #nullable enable
    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value or is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is null.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAny(
      Task[] tasks,
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid enumeration value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAny(
      Task[] tasks,
      Func<Task, TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the created continuation task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAny(
      Task[] tasks,
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid enumeration value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that will be started upon the completion of any task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">One of the enumeration values that controls the behavior of the created continuation task. The <see langword="NotOn*" /> and <see langword="OnlyOn*" /> values are not valid.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, continuationOptions, cancellationToken, scheduler);
    }


    #nullable disable
    internal static Task<TResult> ContinueWhenAnyImpl(
      Task[] tasks,
      Func<Task, TResult> continuationFunction,
      Action<Task> continuationAction,
      TaskContinuationOptions continuationOptions,
      CancellationToken cancellationToken,
      TaskScheduler scheduler)
    {
      TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      if (tasks.Length == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList, ExceptionArgument.tasks);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      Task<Task> task = TaskFactory.CommonCWAnyLogic((IList<Task>) tasks);
      if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
        return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
      return continuationFunction != null ? task.ContinueWith<TResult>((Func<Task<Task>, object, TResult>) ((completedTask, state) => ((Func<Task, TResult>) state)(completedTask.Result)), (object) continuationFunction, scheduler, cancellationToken, continuationOptions) : task.ContinueWith<TResult>((Func<Task<Task>, object, TResult>) ((completedTask, state) =>
      {
        ((Action<Task>) state)(completedTask.Result);
        return default (TResult);
      }), (object) continuationAction, scheduler, cancellationToken, continuationOptions);
    }

    internal static Task<TResult> ContinueWhenAnyImpl<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction,
      Action<Task<TAntecedentResult>> continuationAction,
      TaskContinuationOptions continuationOptions,
      CancellationToken cancellationToken,
      TaskScheduler scheduler)
    {
      TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      if (tasks.Length == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList, ExceptionArgument.tasks);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      Task<Task> task = TaskFactory.CommonCWAnyLogic((IList<Task>) tasks);
      if (cancellationToken.IsCancellationRequested && (continuationOptions & TaskContinuationOptions.LazyCancellation) == TaskContinuationOptions.None)
        return TaskFactory<TResult>.CreateCanceledTask(continuationOptions, cancellationToken);
      return continuationFunction != null ? task.ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAnyFuncDelegate, (object) continuationFunction, scheduler, cancellationToken, continuationOptions) : task.ContinueWith<TResult>(GenericDelegateCache<TAntecedentResult, TResult>.CWAnyActionDelegate, (object) continuationAction, scheduler, cancellationToken, continuationOptions);
    }

    private sealed class FromAsyncTrimPromise<TInstance> : Task<TResult> where TInstance : class
    {
      internal static readonly AsyncCallback s_completeFromAsyncResult = new AsyncCallback(TaskFactory<TResult>.FromAsyncTrimPromise<TInstance>.CompleteFromAsyncResult);
      private TInstance m_thisRef;
      private Func<TInstance, IAsyncResult, TResult> m_endMethod;

      internal FromAsyncTrimPromise(
        TInstance thisRef,
        Func<TInstance, IAsyncResult, TResult> endMethod)
      {
        this.m_thisRef = thisRef;
        this.m_endMethod = endMethod;
      }

      internal static void CompleteFromAsyncResult(IAsyncResult asyncResult)
      {
        if (asyncResult == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.asyncResult);
        if (!(asyncResult.AsyncState is TaskFactory<TResult>.FromAsyncTrimPromise<TInstance> asyncState))
          ThrowHelper.ThrowArgumentException(ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple, ExceptionArgument.asyncResult);
        TInstance thisRef = asyncState.m_thisRef;
        Func<TInstance, IAsyncResult, TResult> endMethod = asyncState.m_endMethod;
        asyncState.m_thisRef = default (TInstance);
        asyncState.m_endMethod = (Func<TInstance, IAsyncResult, TResult>) null;
        if (endMethod == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple, ExceptionArgument.asyncResult);
        if (asyncResult.CompletedSynchronously)
          return;
        asyncState.Complete(thisRef, endMethod, asyncResult, true);
      }

      internal void Complete(
        TInstance thisRef,
        Func<TInstance, IAsyncResult, TResult> endMethod,
        IAsyncResult asyncResult,
        bool requiresSynchronization)
      {
        bool flag;
        try
        {
          TResult result = endMethod(thisRef, asyncResult);
          if (requiresSynchronization)
          {
            flag = this.TrySetResult(result);
          }
          else
          {
            this.DangerousSetResult(result);
            flag = true;
          }
        }
        catch (OperationCanceledException ex)
        {
          flag = this.TrySetCanceled(ex.CancellationToken, (object) ex);
        }
        catch (Exception ex)
        {
          flag = this.TrySetException((object) ex);
        }
      }
    }
  }
}
