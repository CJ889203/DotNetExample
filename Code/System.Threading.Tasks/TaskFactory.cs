// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskFactory
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides support for creating and scheduling <see cref="T:System.Threading.Tasks.Task" /> objects.</summary>
  public class TaskFactory
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
      TaskScheduler defaultScheduler = this.m_defaultScheduler;
      if (defaultScheduler != null)
        return defaultScheduler;
      return currTask == null || (currTask.CreationOptions & TaskCreationOptions.HideScheduler) != TaskCreationOptions.None ? TaskScheduler.Default : currTask.ExecutingTaskScheduler;
    }

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the default configuration.</summary>
    public TaskFactory()
    {
    }

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.</summary>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another CancellationToken is explicitly specified while calling the factory methods.</param>
    public TaskFactory(CancellationToken cancellationToken) => this.m_defaultCancellationToken = cancellationToken;


    #nullable enable
    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.</summary>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to use to schedule any tasks created with this TaskFactory. A null value indicates that the current TaskScheduler should be used.</param>
    public TaskFactory(TaskScheduler? scheduler) => this.m_defaultScheduler = scheduler;

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.</summary>
    /// <param name="creationOptions">The default <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> to use when creating tasks with this TaskFactory.</param>
    /// <param name="continuationOptions">The default <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> to use when creating continuation tasks with this TaskFactory.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    public TaskFactory(
      TaskCreationOptions creationOptions,
      TaskContinuationOptions continuationOptions)
    {
      TaskFactory.CheckMultiTaskContinuationOptions(continuationOptions);
      TaskFactory.CheckCreationOptions(creationOptions);
      this.m_defaultCreationOptions = creationOptions;
      this.m_defaultContinuationOptions = continuationOptions;
    }

    /// <summary>Initializes a <see cref="T:System.Threading.Tasks.TaskFactory" /> instance with the specified configuration.</summary>
    /// <param name="cancellationToken">The default <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to tasks created by this <see cref="T:System.Threading.Tasks.TaskFactory" /> unless another CancellationToken is explicitly specified while calling the factory methods.</param>
    /// <param name="creationOptions">The default <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> to use when creating tasks with this TaskFactory.</param>
    /// <param name="continuationOptions">The default <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> to use when creating continuation tasks with this TaskFactory.</param>
    /// <param name="scheduler">The default <see cref="T:System.Threading.Tasks.TaskScheduler" /> to use to schedule any Tasks created with this TaskFactory. A null value indicates that TaskScheduler.Current should be used.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
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

    internal static void CheckCreationOptions(TaskCreationOptions creationOptions)
    {
      if ((creationOptions & ~(TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler | TaskCreationOptions.RunContinuationsAsynchronously)) == TaskCreationOptions.None)
        return;
      ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.creationOptions);
    }

    /// <summary>Gets the default cancellation token for this task factory.</summary>
    /// <returns>The default task cancellation token for this task factory.</returns>
    public CancellationToken CancellationToken => this.m_defaultCancellationToken;

    /// <summary>Gets the default task scheduler for this task factory.</summary>
    /// <returns>The default task scheduler for this task factory.</returns>
    public TaskScheduler? Scheduler => this.m_defaultScheduler;

    /// <summary>Gets the default task creation options for this task factory.</summary>
    /// <returns>The default task creation options for this task factory.</returns>
    public TaskCreationOptions CreationOptions => this.m_defaultCreationOptions;

    /// <summary>Gets the default task continuation options for this task factory.</summary>
    /// <returns>The default task continuation options for this task factory.</returns>
    public TaskContinuationOptions ContinuationOptions => this.m_defaultContinuationOptions;

    /// <summary>Creates and starts a task for the specified action delegate.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task StartNew(Action action)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task.InternalStartNew(internalCurrent, (Delegate) action, (object) null, this.m_defaultCancellationToken, this.GetDefaultScheduler(internalCurrent), this.m_defaultCreationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task for the specified action delegate and cancellation token.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="action" /> is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task StartNew(Action action, CancellationToken cancellationToken)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task.InternalStartNew(internalCurrent, (Delegate) action, (object) null, cancellationToken, this.GetDefaultScheduler(internalCurrent), this.m_defaultCreationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task for the specified action delegate and creation options.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="action" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value.</exception>
    /// <returns>The started task.</returns>
    public Task StartNew(Action action, TaskCreationOptions creationOptions)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task.InternalStartNew(internalCurrent, (Delegate) action, (object) null, this.m_defaultCancellationToken, this.GetDefaultScheduler(internalCurrent), creationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task for the specified action delegate, cancellation token, creation options and state.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="action" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid TaskCreationOptions value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The started task.</returns>
    public Task StartNew(
      Action action,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return Task.InternalStartNew(Task.InternalCurrentIfAttached(creationOptions), (Delegate) action, (object) null, cancellationToken, scheduler, creationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task for the specified action delegate and state.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="action" /> delegate.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task StartNew(Action<object?> action, object? state)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task.InternalStartNew(internalCurrent, (Delegate) action, state, this.m_defaultCancellationToken, this.GetDefaultScheduler(internalCurrent), this.m_defaultCreationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task for the specified action delegate, state and cancellation token.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="action" /> delegate.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="action" /> is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task StartNew(
      Action<object?> action,
      object? state,
      CancellationToken cancellationToken)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task.InternalStartNew(internalCurrent, (Delegate) action, state, cancellationToken, this.GetDefaultScheduler(internalCurrent), this.m_defaultCreationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task for the specified action delegate, state and creation options.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="action" /> delegate.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="action" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value.</exception>
    /// <returns>The started task.</returns>
    public Task StartNew(
      Action<object?> action,
      object? state,
      TaskCreationOptions creationOptions)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task.InternalStartNew(internalCurrent, (Delegate) action, state, this.m_defaultCancellationToken, this.GetDefaultScheduler(internalCurrent), creationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task for the specified action delegate, state, cancellation token, creation options and task scheduler.</summary>
    /// <param name="action">The action delegate to execute asynchronously.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="action" /> delegate.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="action" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> argument specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The started task.</returns>
    public Task StartNew(
      Action<object?> action,
      object? state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return Task.InternalStartNew(Task.InternalCurrentIfAttached(creationOptions), (Delegate) action, state, cancellationToken, scheduler, creationOptions, InternalTaskOptions.None);
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(Func<TResult> function)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, this.m_defaultCancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate and cancellation token.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="function" /> is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(
      Func<TResult> function,
      CancellationToken cancellationToken)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, cancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate and creation options.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="function" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(
      Func<TResult> function,
      TaskCreationOptions creationOptions)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, this.m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate, cancellation token, creation options and task scheduler.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="function" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(
      Func<TResult> function,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate and state.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="function" /> delegate.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="function" /> is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(Func<object?, TResult> function, object? state)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, state, this.m_defaultCancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate, state and cancellation token.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="function" /> delegate.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="function" /> is <see langword="null" />.</exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(
      Func<object?, TResult> function,
      object? state,
      CancellationToken cancellationToken)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, state, cancellationToken, this.m_defaultCreationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate, state and creation options.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="function" /> delegate.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="function" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(
      Func<object?, TResult> function,
      object? state,
      TaskCreationOptions creationOptions)
    {
      Task internalCurrent = Task.InternalCurrent;
      return Task<TResult>.StartNew(internalCurrent, function, state, this.m_defaultCancellationToken, creationOptions, InternalTaskOptions.None, this.GetDefaultScheduler(internalCurrent));
    }

    /// <summary>Creates and starts a task of type <typeparamref name="TResult" /> for the specified function delegate, state, cancellation token, creation options and task scheduler.</summary>
    /// <param name="function">A function delegate that returns the future result to be available through the task.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="function" /> delegate.</param>
    /// <param name="cancellationToken">The cancellation token that will be assigned to the new task.</param>
    /// <param name="creationOptions">One of the enumeration values that controls the behavior of the created task.</param>
    /// <param name="scheduler">The task scheduler that is used to schedule the created task.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="function" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The started task.</returns>
    public Task<TResult> StartNew<TResult>(
      Func<object?, TResult> function,
      object? state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return Task<TResult>.StartNew(Task.InternalCurrentIfAttached(creationOptions), function, state, cancellationToken, creationOptions, InternalTaskOptions.None, scheduler);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that executes an end method action when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The action delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="asyncResult" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod) => this.FromAsync(asyncResult, endMethod, this.m_defaultCreationOptions, this.DefaultScheduler);

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that executes an end method action when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The action delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="asyncResult" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">paramref name="creationOptions" /&gt; specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync(
      IAsyncResult asyncResult,
      Action<IAsyncResult> endMethod,
      TaskCreationOptions creationOptions)
    {
      return this.FromAsync(asyncResult, endMethod, creationOptions, this.DefaultScheduler);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that executes an end method action when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The action delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the task that executes the end method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="asyncResult" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync(
      IAsyncResult asyncResult,
      Action<IAsyncResult> endMethod,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return (Task) TaskFactory<VoidTaskResult>.FromAsyncImpl(asyncResult, (Func<IAsyncResult, VoidTaskResult>) null, endMethod, creationOptions, scheduler);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync(
      Func<AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      object? state)
    {
      return this.FromAsync(beginMethod, endMethod, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid TaskCreationOptions value.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync(
      Func<AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return (Task) TaskFactory<VoidTaskResult>.FromAsyncImpl(beginMethod, (Func<IAsyncResult, VoidTaskResult>) null, endMethod, state, creationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync<TArg1>(
      Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      TArg1 arg1,
      object? state)
    {
      return this.FromAsync<TArg1>(beginMethod, endMethod, arg1, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync<TArg1>(
      Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      TArg1 arg1,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return (Task) TaskFactory<VoidTaskResult>.FromAsyncImpl<TArg1>(beginMethod, (Func<IAsyncResult, VoidTaskResult>) null, endMethod, arg1, state, creationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync<TArg1, TArg2>(
      Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      object? state)
    {
      return this.FromAsync<TArg1, TArg2>(beginMethod, endMethod, arg1, arg2, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync<TArg1, TArg2>(
      Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return (Task) TaskFactory<VoidTaskResult>.FromAsyncImpl<TArg1, TArg2>(beginMethod, (Func<IAsyncResult, VoidTaskResult>) null, endMethod, arg1, arg2, state, creationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync<TArg1, TArg2, TArg3>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      object? state)
    {
      return this.FromAsync<TArg1, TArg2, TArg3>(beginMethod, endMethod, arg1, arg2, arg3, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public Task FromAsync<TArg1, TArg2, TArg3>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod,
      Action<IAsyncResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return (Task) TaskFactory<VoidTaskResult>.FromAsyncImpl<TArg1, TArg2, TArg3>(beginMethod, (Func<IAsyncResult, VoidTaskResult>) null, endMethod, arg1, arg2, arg3, state, creationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="asyncResult" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TResult>(
      IAsyncResult asyncResult,
      Func<IAsyncResult, TResult> endMethod)
    {
      return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, (Action<IAsyncResult>) null, this.m_defaultCreationOptions, this.DefaultScheduler);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="asyncResult" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TResult>(
      IAsyncResult asyncResult,
      Func<IAsyncResult, TResult> endMethod,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, (Action<IAsyncResult>) null, creationOptions, this.DefaultScheduler);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that executes an end method function when a specified <see cref="T:System.IAsyncResult" /> completes.</summary>
    /// <param name="asyncResult">The IAsyncResult whose completion should trigger the processing of the <paramref name="endMethod" />.</param>
    /// <param name="endMethod">The function delegate that processes the completed <paramref name="asyncResult" />.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the task that executes the end method.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="asyncResult" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid TaskCreationOptions value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TResult>(
      IAsyncResult asyncResult,
      Func<IAsyncResult, TResult> endMethod,
      TaskCreationOptions creationOptions,
      TaskScheduler scheduler)
    {
      return TaskFactory<TResult>.FromAsyncImpl(asyncResult, endMethod, (Action<IAsyncResult>) null, creationOptions, scheduler);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TResult>(
      Func<AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, (Action<IAsyncResult>) null, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TResult>(
      Func<AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl(beginMethod, endMethod, (Action<IAsyncResult>) null, state, creationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TResult>(
      Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TArg1">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TResult>(
      Func<TArg1, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, state, creationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2, TResult>(
      Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, arg2, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2, TResult>(
      Func<TArg1, TArg2, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      object? state,
      TaskCreationOptions creationOptions)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, arg2, state, creationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(
      Func<TArg1, TArg2, TArg3, AsyncCallback, object?, IAsyncResult> beginMethod,
      Func<IAsyncResult, TResult> endMethod,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      object? state)
    {
      return TaskFactory<TResult>.FromAsyncImpl<TArg1, TArg2, TArg3>(beginMethod, endMethod, (Action<IAsyncResult>) null, arg1, arg2, arg3, state, this.m_defaultCreationOptions);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that represents a pair of begin and end methods that conform to the Asynchronous Programming Model pattern.</summary>
    /// <param name="beginMethod">The delegate that begins the asynchronous operation.</param>
    /// <param name="endMethod">The delegate that ends the asynchronous operation.</param>
    /// <param name="arg1">The first argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg2">The second argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="arg3">The third argument passed to the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="state">An object containing data to be used by the <paramref name="beginMethod" /> delegate.</param>
    /// <param name="creationOptions">The TaskCreationOptions value that controls the behavior of the created <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TArg1">The type of the second argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg2">The type of the third argument passed to <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TArg3">The type of the first argument passed to the <paramref name="beginMethod" /> delegate.</typeparam>
    /// <typeparam name="TResult">The type of the result available through the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="beginMethod" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="endMethod" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="creationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> value. For more information, see the Remarks for <see cref="M:System.Threading.Tasks.TaskFactory.FromAsync(System.Func{System.AsyncCallback,System.Object,System.IAsyncResult},System.Action{System.IAsyncResult},System.Object,System.Threading.Tasks.TaskCreationOptions)" /></exception>
    /// <returns>The created <see cref="T:System.Threading.Tasks.Task`1" /> that represents the asynchronous operation.</returns>
    public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(
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

    internal static void CheckFromAsyncOptions(
      TaskCreationOptions creationOptions,
      bool hasBeginMethod)
    {
      if (hasBeginMethod)
      {
        if ((creationOptions & TaskCreationOptions.LongRunning) != TaskCreationOptions.None)
          throw new ArgumentOutOfRangeException(nameof (creationOptions), SR.Task_FromAsync_LongRunning);
        if ((creationOptions & TaskCreationOptions.PreferFairness) != TaskCreationOptions.None)
          throw new ArgumentOutOfRangeException(nameof (creationOptions), SR.Task_FromAsync_PreferFairness);
      }
      if ((creationOptions & ~(TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler)) == TaskCreationOptions.None)
        return;
      ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.creationOptions);
    }


    #nullable disable
    internal static Task<Task[]> CommonCWAllLogic(Task[] tasksCopy)
    {
      TaskFactory.CompleteOnCountdownPromise action = new TaskFactory.CompleteOnCountdownPromise(tasksCopy);
      for (int index = 0; index < tasksCopy.Length; ++index)
      {
        if (tasksCopy[index].IsCompleted)
          action.Invoke(tasksCopy[index]);
        else
          tasksCopy[index].AddCompletionAction((ITaskCompletionAction) action);
      }
      return (Task<Task[]>) action;
    }

    internal static Task<Task<T>[]> CommonCWAllLogic<T>(Task<T>[] tasksCopy)
    {
      TaskFactory.CompleteOnCountdownPromise<T> action = new TaskFactory.CompleteOnCountdownPromise<T>(tasksCopy);
      for (int index = 0; index < tasksCopy.Length; ++index)
      {
        if (tasksCopy[index].IsCompleted)
          action.Invoke((Task) tasksCopy[index]);
        else
          tasksCopy[index].AddCompletionAction((ITaskCompletionAction) action);
      }
      return (Task<Task<T>[]>) action;
    }


    #nullable enable
    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, (Func<Task[], VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll(
      Task[] tasks,
      Action<Task[]> continuationAction,
      CancellationToken cancellationToken)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, (Func<Task[], VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task. The NotOn and OnlyOn members are not supported.</param>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll(
      Task[] tasks,
      Action<Task[]> continuationAction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, (Func<Task[], VoidTaskResult>) null, continuationAction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task.</param>
    /// <param name="scheduler">The object that is used to schedule the new continuation task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll(
      Task[] tasks,
      Action<Task[]> continuationAction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl(tasks, (Func<Task[], VoidTaskResult>) null, continuationAction, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>[]> continuationAction)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>[], VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>[]> continuationAction,
      CancellationToken cancellationToken)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>[], VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task. The NotOn and OnlyOn members are not supported.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>[]> continuationAction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>[], VoidTaskResult>) null, continuationAction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationAction">The action delegate to execute when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task. The NotOn and OnlyOn members are not supported.</param>
    /// <param name="scheduler">The object that is used to schedule the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation task.</returns>
    public Task ContinueWhenAll<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>[]> continuationAction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>[], VoidTaskResult>) null, continuationAction, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TResult>(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TResult>(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task. The NotOn and OnlyOn members are not supported.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TResult>(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task. The NotOn and OnlyOn members are not supported.</param>
    /// <param name="scheduler">The object that is used to schedule the new continuation task.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TResult>(
      Task[] tasks,
      Func<Task[], TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl(tasks, continuationFunction, (Action<Task[]>) null, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task. The NotOn and OnlyOn members are not supported.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation task that starts when a set of specified tasks has completed.</summary>
    /// <param name="tasks">The array of tasks from which to continue.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when all tasks in the <paramref name="tasks" /> array have completed.</param>
    /// <param name="cancellationToken">The cancellation token to assign to the new continuation task.</param>
    /// <param name="continuationOptions">A bitwise combination of the enumeration values that control the behavior of the new continuation task. The NotOn and OnlyOn members are not supported.</param>
    /// <param name="scheduler">The object that is used to schedule the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array is empty or contains a null value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">An element in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation task.</returns>
    public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>[], TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAllImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>[]>) null, continuationOptions, cancellationToken, scheduler);
    }


    #nullable disable
    internal static Task<Task> CommonCWAnyLogic(IList<Task> tasks, bool isSyncBlocking = false)
    {
      TaskFactory.CompleteOnInvokePromise completeOnInvokePromise = new TaskFactory.CompleteOnInvokePromise(tasks, isSyncBlocking);
      bool flag = false;
      int count = tasks.Count;
      for (int index = 0; index < count; ++index)
      {
        Task task = tasks[index];
        if (task == null)
          throw new ArgumentException(SR.Task_MultiTaskContinuation_NullTask, nameof (tasks));
        if (!flag)
        {
          if (completeOnInvokePromise.IsCompleted)
            flag = true;
          else if (task.IsCompleted)
          {
            completeOnInvokePromise.Invoke(task);
            flag = true;
          }
          else
          {
            task.AddCompletionAction((ITaskCompletionAction) completeOnInvokePromise, isSyncBlocking);
            if (completeOnInvokePromise.IsCompleted)
              task.RemoveContinuation((object) completeOnInvokePromise);
          }
        }
      }
      return (Task<Task>) completeOnInvokePromise;
    }

    internal static void CommonCWAnyLogicCleanup(Task<Task> continuation) => ((TaskFactory.CompleteOnInvokePromise) continuation).Invoke((Task) null);


    #nullable enable
    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, (Func<Task, VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty .</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny(
      Task[] tasks,
      Action<Task> continuationAction,
      CancellationToken cancellationToken)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, (Func<Task, VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationAction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    ///  -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny(
      Task[] tasks,
      Action<Task> continuationAction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, (Func<Task, VoidTaskResult>) null, continuationAction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="continuationAction" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny(
      Task[] tasks,
      Action<Task> continuationAction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl(tasks, (Func<Task, VoidTaskResult>) null, continuationAction, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TResult>(
      Task[] tasks,
      Func<Task, TResult> continuationFunction)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TResult>(
      Task[] tasks,
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TResult>(
      Task[] tasks,
      Func<Task, TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TResult>(
      Task[] tasks,
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl(tasks, continuationFunction, (Action<Task>) null, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    ///  -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task`1" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationFunction">The function delegate to execute asynchronously when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <typeparam name="TResult">The type of the result that is returned by the <paramref name="continuationFunction" /> delegate and associated with the created <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    /// <paramref name="continuationFunction" /> is <see langword="null" />.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="scheduler" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(
      Task<TAntecedentResult>[] tasks,
      Func<Task<TAntecedentResult>, TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationFunction == null)
        throw new ArgumentNullException(nameof (continuationFunction));
      return TaskFactory<TResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, continuationFunction, (Action<Task<TAntecedentResult>>) null, continuationOptions, cancellationToken, scheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationAction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>> continuationAction)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>, VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.
    /// 
    /// -or-
    /// 
    /// The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationAction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    ///  -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>> continuationAction,
      CancellationToken cancellationToken)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>, VoidTaskResult>) null, continuationAction, this.m_defaultContinuationOptions, cancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">One of the elements in the <paramref name="tasks" /> array has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationAction" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid TaskContinuationOptions value.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a null value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>> continuationAction,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>, VoidTaskResult>) null, continuationAction, continuationOptions, this.m_defaultCancellationToken, this.DefaultScheduler);
    }

    /// <summary>Creates a continuation <see cref="T:System.Threading.Tasks.Task" /> that will be started upon the completion of any Task in the provided set.</summary>
    /// <param name="tasks">The array of tasks from which to continue when one task completes.</param>
    /// <param name="continuationAction">The action delegate to execute when one task in the <paramref name="tasks" /> array completes.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">The <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value that controls the behavior of the created continuation <see cref="T:System.Threading.Tasks.Task" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> that is used to schedule the created continuation <see cref="T:System.Threading.Tasks.Task`1" />.</param>
    /// <typeparam name="TAntecedentResult">The type of the result of the antecedent <paramref name="tasks" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="continuationAction" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// paramref name="scheduler" /&gt; is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contains a <see langword="null" /> value.
    /// 
    /// -or-
    /// 
    /// The <paramref name="tasks" /> array is empty.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="continuationOptions" /> specifies an invalid <see cref="T:System.Threading.Tasks.TaskContinuationOptions" /> value.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>The new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWhenAny<TAntecedentResult>(
      Task<TAntecedentResult>[] tasks,
      Action<Task<TAntecedentResult>> continuationAction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      if (continuationAction == null)
        throw new ArgumentNullException(nameof (continuationAction));
      return (Task) TaskFactory<VoidTaskResult>.ContinueWhenAnyImpl<TAntecedentResult>(tasks, (Func<Task<TAntecedentResult>, VoidTaskResult>) null, continuationAction, continuationOptions, cancellationToken, scheduler);
    }


    #nullable disable
    internal static Task[] CheckMultiContinuationTasksAndCopy(Task[] tasks)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      Task[] taskArray = tasks.Length != 0 ? new Task[tasks.Length] : throw new ArgumentException(SR.Task_MultiTaskContinuation_EmptyTaskList, nameof (tasks));
      for (int index = 0; index < tasks.Length; ++index)
      {
        taskArray[index] = tasks[index];
        if (taskArray[index] == null)
          throw new ArgumentException(SR.Task_MultiTaskContinuation_NullTask, nameof (tasks));
      }
      return taskArray;
    }

    internal static Task<TResult>[] CheckMultiContinuationTasksAndCopy<TResult>(
      Task<TResult>[] tasks)
    {
      if (tasks == null)
        throw new ArgumentNullException(nameof (tasks));
      Task<TResult>[] taskArray = tasks.Length != 0 ? new Task<TResult>[tasks.Length] : throw new ArgumentException(SR.Task_MultiTaskContinuation_EmptyTaskList, nameof (tasks));
      for (int index = 0; index < tasks.Length; ++index)
      {
        taskArray[index] = tasks[index];
        if (taskArray[index] == null)
          throw new ArgumentException(SR.Task_MultiTaskContinuation_NullTask, nameof (tasks));
      }
      return taskArray;
    }

    internal static void CheckMultiTaskContinuationOptions(
      TaskContinuationOptions continuationOptions)
    {
      if ((continuationOptions & (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously)) == (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously))
        throw new ArgumentOutOfRangeException(nameof (continuationOptions), SR.Task_ContinueWith_ESandLR);
      if ((continuationOptions & ~(TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.HideScheduler | TaskContinuationOptions.LazyCancellation | TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously)) != TaskContinuationOptions.None)
        throw new ArgumentOutOfRangeException(nameof (continuationOptions));
      if ((continuationOptions & (TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion)) != TaskContinuationOptions.None)
        throw new ArgumentOutOfRangeException(nameof (continuationOptions), SR.Task_MultiTaskContinuation_FireOptions);
    }

    private sealed class CompleteOnCountdownPromise : Task<Task[]>, ITaskCompletionAction
    {
      private readonly Task[] _tasks;
      private int _count;

      internal CompleteOnCountdownPromise(Task[] tasksCopy)
      {
        this._tasks = tasksCopy;
        this._count = tasksCopy.Length;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationBegin(this.Id, "TaskFactory.ContinueWhenAll", 0L);
        if (!Task.s_asyncDebuggingEnabled)
          return;
        Task.AddToActiveTasks((Task) this);
      }

      public void Invoke(Task completingTask)
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationRelation(this.Id, CausalityRelation.Join);
        if (completingTask.IsWaitNotificationEnabled)
          this.SetNotificationForWaitCompletion(true);
        if (Interlocked.Decrement(ref this._count) != 0)
          return;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) this);
        this.TrySetResult(this._tasks);
      }

      public bool InvokeMayRunArbitraryCode => true;

      private protected override bool ShouldNotifyDebuggerOfWaitCompletion => base.ShouldNotifyDebuggerOfWaitCompletion && Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion(this._tasks);
    }

    private sealed class CompleteOnCountdownPromise<T> : Task<Task<T>[]>, ITaskCompletionAction
    {
      private readonly Task<T>[] _tasks;
      private int _count;

      internal CompleteOnCountdownPromise(Task<T>[] tasksCopy)
      {
        this._tasks = tasksCopy;
        this._count = tasksCopy.Length;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationBegin(this.Id, "TaskFactory.ContinueWhenAll<>", 0L);
        if (!Task.s_asyncDebuggingEnabled)
          return;
        Task.AddToActiveTasks((Task) this);
      }

      public void Invoke(Task completingTask)
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationRelation(this.Id, CausalityRelation.Join);
        if (completingTask.IsWaitNotificationEnabled)
          this.SetNotificationForWaitCompletion(true);
        if (Interlocked.Decrement(ref this._count) != 0)
          return;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) this);
        this.TrySetResult(this._tasks);
      }

      public bool InvokeMayRunArbitraryCode => true;

      private protected override bool ShouldNotifyDebuggerOfWaitCompletion => base.ShouldNotifyDebuggerOfWaitCompletion && Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion((Task[]) this._tasks);
    }

    internal sealed class CompleteOnInvokePromise : Task<Task>, ITaskCompletionAction
    {
      private IList<Task> _tasks;
      private int _stateFlags;

      public CompleteOnInvokePromise(IList<Task> tasks, bool isSyncBlocking)
      {
        this._tasks = tasks;
        if (isSyncBlocking)
          this._stateFlags = 2;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationBegin(this.Id, "TaskFactory.ContinueWhenAny", 0L);
        if (!Task.s_asyncDebuggingEnabled)
          return;
        Task.AddToActiveTasks((Task) this);
      }

      public void Invoke(Task completingTask)
      {
        int stateFlags = this._stateFlags;
        int num = stateFlags & 2;
        if ((stateFlags & 1) != 0 || Interlocked.Exchange(ref this._stateFlags, num | 1) != num)
          return;
        if (TplEventSource.Log.IsEnabled())
        {
          TplEventSource.Log.TraceOperationRelation(this.Id, CausalityRelation.Choice);
          TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
        }
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) this);
        this.TrySetResult(completingTask);
        IList<Task> tasks = this._tasks;
        int count = tasks.Count;
        for (int index = 0; index < count; ++index)
        {
          Task task = tasks[index];
          if (task != null && !task.IsCompleted)
            task.RemoveContinuation((object) this);
        }
        this._tasks = (IList<Task>) null;
      }

      public bool InvokeMayRunArbitraryCode => (this._stateFlags & 2) == 0;
    }
  }
}
