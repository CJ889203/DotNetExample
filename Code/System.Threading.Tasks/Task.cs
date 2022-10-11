﻿// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.Task
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Represents an asynchronous operation.</summary>
  [DebuggerTypeProxy(typeof (SystemThreadingTasks_TaskDebugView))]
  [DebuggerDisplay("Id = {Id}, Status = {Status}, Method = {DebuggerDisplayMethodDescription}")]
  public class Task : IAsyncResult, IDisposable
  {

    #nullable disable
    [ThreadStatic]
    internal static Task t_currentTask;
    internal static int s_taskIdCounter;
    private int m_taskId;
    internal Delegate m_action;
    internal object m_stateObject;
    internal TaskScheduler m_taskScheduler;
    internal volatile int m_stateFlags;
    private volatile object m_continuationObject;
    private static readonly object s_taskCompletionSentinel = new object();
    internal static bool s_asyncDebuggingEnabled;
    private static Dictionary<int, Task> s_currentActiveTasks;
    internal Task.ContingentProperties m_contingentProperties;
    internal static readonly Task<VoidTaskResult> s_cachedCompleted = new Task<VoidTaskResult>(false, new VoidTaskResult(), (TaskCreationOptions) 16384, new CancellationToken());
    private static readonly ContextCallback s_ecCallback = (ContextCallback) (obj => Unsafe.As<Task>(obj).InnerInvoke());


    #nullable enable
    private Task? ParentForDebugger => this.m_contingentProperties?.m_parent;

    private int StateFlagsForDebugger => this.m_stateFlags;

    private Task.TaskStateFlags StateFlags => (Task.TaskStateFlags) (this.m_stateFlags & -65536);


    #nullable disable
    internal static bool AddToActiveTasks(Task task)
    {
      Dictionary<int, Task> dictionary = Volatile.Read<Dictionary<int, Task>>(ref Task.s_currentActiveTasks) ?? Interlocked.CompareExchange<Dictionary<int, Task>>(ref Task.s_currentActiveTasks, new Dictionary<int, Task>(), (Dictionary<int, Task>) null) ?? Task.s_currentActiveTasks;
      int id = task.Id;
      lock (dictionary)
        dictionary[id] = task;
      return true;
    }

    internal static void RemoveFromActiveTasks(Task task)
    {
      Dictionary<int, Task> currentActiveTasks = Task.s_currentActiveTasks;
      if (currentActiveTasks == null)
        return;
      int id = task.Id;
      lock (currentActiveTasks)
        currentActiveTasks.Remove(id);
    }

    internal Task(bool canceled, TaskCreationOptions creationOptions, CancellationToken ct)
    {
      int num = (int) creationOptions;
      if (canceled)
      {
        this.m_stateFlags = 5242880 | num;
        this.m_contingentProperties = new Task.ContingentProperties()
        {
          m_cancellationToken = ct,
          m_internalCancellationRequested = 1
        };
      }
      else
        this.m_stateFlags = 16777216 | num;
    }

    internal Task() => this.m_stateFlags = 33555456;

    internal Task(object state, TaskCreationOptions creationOptions, bool promiseStyle)
    {
      if ((creationOptions & ~(TaskCreationOptions.AttachedToParent | TaskCreationOptions.RunContinuationsAsynchronously)) != TaskCreationOptions.None)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.creationOptions);
      if ((creationOptions & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None)
      {
        Task internalCurrent = Task.InternalCurrent;
        if (internalCurrent != null)
          this.EnsureContingentPropertiesInitializedUnsafe().m_parent = internalCurrent;
      }
      this.TaskConstructorCore((Delegate) null, state, new CancellationToken(), creationOptions, InternalTaskOptions.PromiseTask, (TaskScheduler) null);
    }


    #nullable enable
    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is <see langword="null" />.</exception>
    public Task(Action action)
      : this((Delegate) action, (object) null, (Task) null, new CancellationToken(), TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that the new  task will observe.</param>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
    public Task(Action action, CancellationToken cancellationToken)
      : this((Delegate) action, (object) null, (Task) null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and creation options.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    public Task(Action action, TaskCreationOptions creationOptions)
      : this((Delegate) action, (object) null, Task.InternalCurrentIfAttached(creationOptions), new CancellationToken(), creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and creation options.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that the new task will observe.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    public Task(
      Action action,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions)
      : this((Delegate) action, (object) null, Task.InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action and state.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <param name="state">An object representing data to be used by the action.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
    public Task(Action<object?> action, object? state)
      : this((Delegate) action, state, (Task) null, new CancellationToken(), TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action, state, and options.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <param name="state">An object representing data to be used by the action.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that the new task will observe.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
    public Task(Action<object?> action, object? state, CancellationToken cancellationToken)
      : this((Delegate) action, state, (Task) null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action, state, and options.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <param name="state">An object representing data to be used by the action.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    public Task(Action<object?> action, object? state, TaskCreationOptions creationOptions)
      : this((Delegate) action, state, Task.InternalCurrentIfAttached(creationOptions), new CancellationToken(), creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task" /> with the specified action, state, and options.</summary>
    /// <param name="action">The delegate that represents the code to execute in the task.</param>
    /// <param name="state">An object representing data to be used by the action.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that the new task will observe.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    public Task(
      Action<object?> action,
      object? state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions)
      : this((Delegate) action, state, Task.InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }


    #nullable disable
    internal Task(
      Delegate action,
      object state,
      Task parent,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
    {
      if ((object) action == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.action);
      if (parent != null && (creationOptions & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None)
        this.EnsureContingentPropertiesInitializedUnsafe().m_parent = parent;
      this.TaskConstructorCore(action, state, cancellationToken, creationOptions, internalOptions, scheduler);
      this.CapturedContext = ExecutionContext.Capture();
    }

    internal void TaskConstructorCore(
      Delegate action,
      object state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
    {
      this.m_action = action;
      this.m_stateObject = state;
      this.m_taskScheduler = scheduler;
      if ((creationOptions & ~(TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler | TaskCreationOptions.RunContinuationsAsynchronously)) != TaskCreationOptions.None)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.creationOptions);
      int num = (int) (creationOptions | (TaskCreationOptions) internalOptions);
      this.m_stateFlags = (object) this.m_action == null || (internalOptions & InternalTaskOptions.ContinuationTask) != InternalTaskOptions.None ? num | 33554432 : num;
      Task.ContingentProperties contingentProperties = this.m_contingentProperties;
      if (contingentProperties != null)
      {
        Task parent = contingentProperties.m_parent;
        if (parent != null && (creationOptions & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None && (parent.CreationOptions & TaskCreationOptions.DenyChildAttach) == TaskCreationOptions.None)
          parent.AddNewChild();
      }
      if (!cancellationToken.CanBeCanceled)
        return;
      this.AssignCancellationToken(cancellationToken, (Task) null, (TaskContinuation) null);
    }

    private void AssignCancellationToken(
      CancellationToken cancellationToken,
      Task antecedent,
      TaskContinuation continuation)
    {
      Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitializedUnsafe();
      contingentProperties.m_cancellationToken = cancellationToken;
      try
      {
        if ((this.Options & (TaskCreationOptions) 13312) != TaskCreationOptions.None)
          return;
        if (cancellationToken.IsCancellationRequested)
        {
          this.InternalCancel();
        }
        else
        {
          CancellationTokenRegistration tokenRegistration = antecedent != null ? cancellationToken.UnsafeRegister((Action<object>) (t =>
          {
            TupleSlim<Task, Task, TaskContinuation> tupleSlim = (TupleSlim<Task, Task, TaskContinuation>) t;
            Task task = tupleSlim.Item1;
            tupleSlim.Item2.RemoveContinuation((object) tupleSlim.Item3);
            task.InternalCancel();
          }), (object) new TupleSlim<Task, Task, TaskContinuation>(this, antecedent, continuation)) : cancellationToken.UnsafeRegister((Action<object>) (t => ((Task) t).InternalCancel()), (object) this);
          contingentProperties.m_cancellationRegistration = new StrongBox<CancellationTokenRegistration>(tokenRegistration);
        }
      }
      catch
      {
        Task parent = this.m_contingentProperties?.m_parent;
        if (parent != null && (this.Options & TaskCreationOptions.AttachedToParent) != TaskCreationOptions.None && (parent.Options & TaskCreationOptions.DenyChildAttach) == TaskCreationOptions.None)
          parent.DisregardChild();
        throw;
      }
    }


    #nullable enable
    private string DebuggerDisplayMethodDescription => this.m_action?.Method.ToString() ?? "{null}";

    internal TaskCreationOptions Options => Task.OptionsMethod(this.m_stateFlags);

    internal static TaskCreationOptions OptionsMethod(int flags) => (TaskCreationOptions) (flags & (int) ushort.MaxValue);

    internal bool AtomicStateUpdate(int newBits, int illegalBits)
    {
      int stateFlags = this.m_stateFlags;
      if ((stateFlags & illegalBits) != 0)
        return false;
      return Interlocked.CompareExchange(ref this.m_stateFlags, stateFlags | newBits, stateFlags) == stateFlags || this.AtomicStateUpdateSlow(newBits, illegalBits);
    }

    private bool AtomicStateUpdateSlow(int newBits, int illegalBits)
    {
      int num;
      for (int comparand = this.m_stateFlags; (comparand & illegalBits) == 0; comparand = num)
      {
        num = Interlocked.CompareExchange(ref this.m_stateFlags, comparand | newBits, comparand);
        if (num == comparand)
          return true;
      }
      return false;
    }


    #nullable disable
    internal bool AtomicStateUpdate(int newBits, int illegalBits, ref int oldFlags)
    {
      for (int comparand = oldFlags = this.m_stateFlags; (comparand & illegalBits) == 0; comparand = oldFlags)
      {
        oldFlags = Interlocked.CompareExchange(ref this.m_stateFlags, comparand | newBits, comparand);
        if (oldFlags == comparand)
          return true;
      }
      return false;
    }

    internal void SetNotificationForWaitCompletion(bool enabled)
    {
      if (enabled)
        this.AtomicStateUpdate(268435456, 90177536);
      else
        Interlocked.And(ref this.m_stateFlags, -268435457);
    }

    internal bool NotifyDebuggerOfWaitCompletionIfNecessary()
    {
      if (!this.IsWaitNotificationEnabled || !this.ShouldNotifyDebuggerOfWaitCompletion)
        return false;
      this.NotifyDebuggerOfWaitCompletion();
      return true;
    }

    internal static bool AnyTaskRequiresNotifyDebuggerOfWaitCompletion(Task[] tasks)
    {
      foreach (Task task in tasks)
      {
        if (task != null && task.IsWaitNotificationEnabled && task.ShouldNotifyDebuggerOfWaitCompletion)
          return true;
      }
      return false;
    }

    internal bool IsWaitNotificationEnabledOrNotRanToCompletion
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (this.m_stateFlags & 285212672) != 16777216;
    }

    private protected virtual bool ShouldNotifyDebuggerOfWaitCompletion => this.IsWaitNotificationEnabled;

    internal bool IsWaitNotificationEnabled => (this.m_stateFlags & 268435456) != 0;

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private void NotifyDebuggerOfWaitCompletion() => this.SetNotificationForWaitCompletion(false);

    internal bool MarkStarted() => this.AtomicStateUpdate(65536, 4259840);

    internal void FireTaskScheduledIfNeeded(TaskScheduler ts)
    {
      if ((this.m_stateFlags & 1073741824) != 0)
        return;
      this.m_stateFlags |= 1073741824;
      if (!TplEventSource.Log.IsEnabled())
        return;
      Task internalCurrent = Task.InternalCurrent;
      Task parent = this.m_contingentProperties?.m_parent;
      TplEventSource.Log.TaskScheduled(ts.Id, internalCurrent == null ? 0 : internalCurrent.Id, this.Id, parent == null ? 0 : parent.Id, (int) this.Options);
    }

    internal void AddNewChild()
    {
      Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitialized();
      if (contingentProperties.m_completionCountdown == 1)
        ++contingentProperties.m_completionCountdown;
      else
        Interlocked.Increment(ref contingentProperties.m_completionCountdown);
    }

    internal void DisregardChild() => Interlocked.Decrement(ref this.EnsureContingentPropertiesInitialized().m_completionCountdown);

    /// <summary>Starts the <see cref="T:System.Threading.Tasks.Task" />, scheduling it for execution to the current <see cref="T:System.Threading.Tasks.TaskScheduler" />.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started, executed, or canceled, or it may have been created in a manner that doesn't support direct scheduling.</exception>
    public void Start() => this.Start(TaskScheduler.Current);


    #nullable enable
    /// <summary>Starts the <see cref="T:System.Threading.Tasks.Task" />, scheduling it for execution to the specified <see cref="T:System.Threading.Tasks.TaskScheduler" />.</summary>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> with which to associate and execute this task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started, executed, or canceled, or it may have been created in a manner that doesn't support direct scheduling.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskSchedulerException">The scheduler was unable to queue this task.</exception>
    public void Start(TaskScheduler scheduler)
    {
      int stateFlags = this.m_stateFlags;
      if (Task.IsCompletedMethod(stateFlags))
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_TaskCompleted);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions taskCreationOptions = Task.OptionsMethod(stateFlags);
      if ((taskCreationOptions & (TaskCreationOptions) 1024) != TaskCreationOptions.None)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_Promise);
      if ((taskCreationOptions & (TaskCreationOptions) 512) != TaskCreationOptions.None)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_ContinuationTask);
      if (Interlocked.CompareExchange<TaskScheduler>(ref this.m_taskScheduler, scheduler, (TaskScheduler) null) != null)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Start_AlreadyStarted);
      this.ScheduleAndStart(true);
    }

    /// <summary>Runs the <see cref="T:System.Threading.Tasks.Task" /> synchronously on the current <see cref="T:System.Threading.Tasks.TaskScheduler" />.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started, executed, or canceled, or it may have been created in a manner that doesn't support direct scheduling.</exception>
    public void RunSynchronously() => this.InternalRunSynchronously(TaskScheduler.Current, true);

    /// <summary>Runs the <see cref="T:System.Threading.Tasks.Task" /> synchronously on the <see cref="T:System.Threading.Tasks.TaskScheduler" /> provided.</summary>
    /// <param name="scheduler">The scheduler on which to attempt to run this task inline.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> instance has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.Tasks.Task" /> is not in a valid state to be started. It may have already been started, executed, or canceled, or it may have been created in a manner that doesn't support direct scheduling.</exception>
    public void RunSynchronously(TaskScheduler scheduler)
    {
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      this.InternalRunSynchronously(scheduler, true);
    }


    #nullable disable
    internal void InternalRunSynchronously(TaskScheduler scheduler, bool waitForCompletion)
    {
      int stateFlags = this.m_stateFlags;
      TaskCreationOptions taskCreationOptions = Task.OptionsMethod(stateFlags);
      if ((taskCreationOptions & (TaskCreationOptions) 512) != TaskCreationOptions.None)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_Continuation);
      if ((taskCreationOptions & (TaskCreationOptions) 1024) != TaskCreationOptions.None)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_Promise);
      if (Task.IsCompletedMethod(stateFlags))
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_TaskCompleted);
      if (Interlocked.CompareExchange<TaskScheduler>(ref this.m_taskScheduler, scheduler, (TaskScheduler) null) != null)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_AlreadyStarted);
      if (this.MarkStarted())
      {
        bool flag = false;
        try
        {
          if (!scheduler.TryRunInline(this, false))
          {
            scheduler.InternalQueueTask(this);
            flag = true;
          }
          if (!waitForCompletion || this.IsCompleted)
            return;
          this.SpinThenBlockingWait(-1, new CancellationToken());
        }
        catch (System.Exception ex)
        {
          if (!flag)
          {
            TaskSchedulerException exceptionObject = new TaskSchedulerException(ex);
            this.AddException((object) exceptionObject);
            this.Finish(false);
            this.m_contingentProperties.m_exceptionsHolder.MarkAsHandled(false);
            throw exceptionObject;
          }
          throw;
        }
      }
      else
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_RunSynchronously_TaskCompleted);
    }

    internal static Task InternalStartNew(
      Task creatingTask,
      Delegate action,
      object state,
      CancellationToken cancellationToken,
      TaskScheduler scheduler,
      TaskCreationOptions options,
      InternalTaskOptions internalOptions)
    {
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      Task task = new Task(action, state, creatingTask, cancellationToken, options, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler);
      task.ScheduleAndStart(false);
      return task;
    }

    internal static int NewId()
    {
      int TaskID;
      do
      {
        TaskID = Interlocked.Increment(ref Task.s_taskIdCounter);
      }
      while (TaskID == 0);
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.NewID(TaskID);
      return TaskID;
    }

    /// <summary>Gets an ID for this <see cref="T:System.Threading.Tasks.Task" /> instance.</summary>
    /// <returns>The identifier that is assigned by the system to this <see cref="T:System.Threading.Tasks.Task" /> instance.</returns>
    public int Id
    {
      get
      {
        if (Volatile.Read(ref this.m_taskId) == 0)
          Interlocked.CompareExchange(ref this.m_taskId, Task.NewId(), 0);
        return this.m_taskId;
      }
    }


    #nullable enable
    /// <summary>Returns the ID of the currently executing <see cref="T:System.Threading.Tasks.Task" />.</summary>
    /// <returns>An integer that was assigned by the system to the currently-executing task.</returns>
    public static int? CurrentId => Task.InternalCurrent?.Id;

    internal static Task? InternalCurrent => Task.t_currentTask;


    #nullable disable
    internal static Task InternalCurrentIfAttached(TaskCreationOptions creationOptions) => (creationOptions & TaskCreationOptions.AttachedToParent) == TaskCreationOptions.None ? (Task) null : Task.InternalCurrent;


    #nullable enable
    /// <summary>Gets the <see cref="T:System.AggregateException" /> that caused the <see cref="T:System.Threading.Tasks.Task" /> to end prematurely. If the <see cref="T:System.Threading.Tasks.Task" /> completed successfully or has not yet thrown any exceptions, this will return <see langword="null" />.</summary>
    /// <returns>The <see cref="T:System.AggregateException" /> that caused the <see cref="T:System.Threading.Tasks.Task" /> to end prematurely.</returns>
    public AggregateException? Exception
    {
      get
      {
        AggregateException exception = (AggregateException) null;
        if (this.IsFaulted)
          exception = this.GetExceptions(false);
        return exception;
      }
    }

    /// <summary>Gets the <see cref="T:System.Threading.Tasks.TaskStatus" /> of this task.</summary>
    /// <returns>The current <see cref="T:System.Threading.Tasks.TaskStatus" /> of this task instance.</returns>
    public TaskStatus Status
    {
      get
      {
        int stateFlags = this.m_stateFlags;
        return (stateFlags & 2097152) == 0 ? ((stateFlags & 4194304) == 0 ? ((stateFlags & 16777216) == 0 ? ((stateFlags & 8388608) == 0 ? ((stateFlags & 131072) == 0 ? ((stateFlags & 65536) == 0 ? ((stateFlags & 33554432) == 0 ? TaskStatus.Created : TaskStatus.WaitingForActivation) : TaskStatus.WaitingToRun) : TaskStatus.Running) : TaskStatus.WaitingForChildrenToComplete) : TaskStatus.RanToCompletion) : TaskStatus.Canceled) : TaskStatus.Faulted;
      }
    }

    /// <summary>Gets whether this <see cref="T:System.Threading.Tasks.Task" /> instance has completed execution due to being canceled.</summary>
    /// <returns>
    /// <see langword="true" /> if the task has completed due to being canceled; otherwise <see langword="false" />.</returns>
    public bool IsCanceled => (this.m_stateFlags & 6291456) == 4194304;

    internal bool IsCancellationRequested
    {
      get
      {
        Task.ContingentProperties contingentProperties = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
        if (contingentProperties == null)
          return false;
        return contingentProperties.m_internalCancellationRequested == 1 || contingentProperties.m_cancellationToken.IsCancellationRequested;
      }
    }


    #nullable disable
    internal Task.ContingentProperties EnsureContingentPropertiesInitialized()
    {
      return Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties) ?? InitializeContingentProperties();

      Task.ContingentProperties InitializeContingentProperties()
      {
        Interlocked.CompareExchange<Task.ContingentProperties>(ref this.m_contingentProperties, new Task.ContingentProperties(), (Task.ContingentProperties) null);
        return this.m_contingentProperties;
      }
    }

    internal Task.ContingentProperties EnsureContingentPropertiesInitializedUnsafe() => this.m_contingentProperties ?? (this.m_contingentProperties = new Task.ContingentProperties());

    internal CancellationToken CancellationToken
    {
      get
      {
        Task.ContingentProperties contingentProperties = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
        return contingentProperties != null ? contingentProperties.m_cancellationToken : new CancellationToken();
      }
    }

    internal bool IsCancellationAcknowledged => (this.m_stateFlags & 1048576) != 0;

    /// <summary>Gets a value that indicates whether the task has completed.</summary>
    /// <returns>
    /// <see langword="true" /> if the task has completed (that is, the task is in one of the three final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />); otherwise, <see langword="false" />.</returns>
    public bool IsCompleted => Task.IsCompletedMethod(this.m_stateFlags);

    private static bool IsCompletedMethod(int flags) => (flags & 23068672) != 0;

    /// <summary>Gets whether the task ran to completion.</summary>
    /// <returns>
    /// <see langword="true" /> if the task ran to completion; otherwise <see langword="false" />.</returns>
    public bool IsCompletedSuccessfully => (this.m_stateFlags & 23068672) == 16777216;

    /// <summary>Gets the <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to create this task.</summary>
    /// <returns>The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to create this task.</returns>
    public TaskCreationOptions CreationOptions => this.Options & (TaskCreationOptions) -65281;

    internal void SpinUntilCompleted()
    {
      System.Threading.SpinWait spinWait = new System.Threading.SpinWait();
      while (!this.IsCompleted)
        spinWait.SpinOnce();
    }


    #nullable enable
    /// <summary>Gets a <see cref="T:System.Threading.WaitHandle" /> that can be used to wait for the task to complete.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <returns>A <see cref="T:System.Threading.WaitHandle" /> that can be used to wait for the task to complete.</returns>
    WaitHandle IAsyncResult.AsyncWaitHandle
    {
      get
      {
        if ((this.m_stateFlags & 262144) != 0)
          ThrowHelper.ThrowObjectDisposedException(ExceptionResource.Task_ThrowIfDisposed);
        return this.CompletedEvent.WaitHandle;
      }
    }

    /// <summary>Gets the state object supplied when the <see cref="T:System.Threading.Tasks.Task" /> was created, or null if none was supplied.</summary>
    /// <returns>An <see cref="T:System.Object" /> that represents the state data that was passed in to the task when it was created.</returns>
    public object? AsyncState => this.m_stateObject;

    /// <summary>Gets an indication of whether the operation completed synchronously.</summary>
    /// <returns>
    /// <see langword="true" /> if the operation completed synchronously; otherwise, <see langword="false" />.</returns>
    bool IAsyncResult.CompletedSynchronously => false;

    internal TaskScheduler? ExecutingTaskScheduler => this.m_taskScheduler;

    /// <summary>Provides access to factory methods for creating and configuring <see cref="T:System.Threading.Tasks.Task" /> and <see cref="T:System.Threading.Tasks.Task`1" /> instances.</summary>
    /// <returns>A factory object that can create a variety of <see cref="T:System.Threading.Tasks.Task" /> and <see cref="T:System.Threading.Tasks.Task`1" /> objects.</returns>
    public static TaskFactory Factory { get; } = new TaskFactory();

    /// <summary>Gets a task that has already completed successfully.</summary>
    /// <returns>The successfully completed task.</returns>
    public static Task CompletedTask => (Task) Task.s_cachedCompleted;

    internal ManualResetEventSlim CompletedEvent
    {
      get
      {
        Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitialized();
        if (contingentProperties.m_completionEvent == null)
        {
          bool isCompleted = this.IsCompleted;
          ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(isCompleted);
          if (Interlocked.CompareExchange<ManualResetEventSlim>(ref contingentProperties.m_completionEvent, manualResetEventSlim, (ManualResetEventSlim) null) != null)
            manualResetEventSlim.Dispose();
          else if (!isCompleted && this.IsCompleted)
            manualResetEventSlim.Set();
        }
        return contingentProperties.m_completionEvent;
      }
    }

    internal bool ExceptionRecorded
    {
      get
      {
        Task.ContingentProperties contingentProperties = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
        return contingentProperties != null && contingentProperties.m_exceptionsHolder != null && contingentProperties.m_exceptionsHolder.ContainsFaultList;
      }
    }

    /// <summary>Gets whether the <see cref="T:System.Threading.Tasks.Task" /> completed due to an unhandled exception.</summary>
    /// <returns>
    /// <see langword="true" /> if the task has thrown an unhandled exception; otherwise <see langword="false" />.</returns>
    public bool IsFaulted => (this.m_stateFlags & 2097152) != 0;

    internal ExecutionContext? CapturedContext
    {
      get => (this.m_stateFlags & 536870912) == 536870912 ? (ExecutionContext) null : this.m_contingentProperties?.m_capturedContext ?? ExecutionContext.Default;
      set
      {
        if (value == null)
        {
          this.m_stateFlags |= 536870912;
        }
        else
        {
          if (value == ExecutionContext.Default)
            return;
          this.EnsureContingentPropertiesInitializedUnsafe().m_capturedContext = value;
        }
      }
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.Tasks.Task" /> class.</summary>
    /// <exception cref="T:System.InvalidOperationException">The task is not in one of the final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />.</exception>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Disposes the <see cref="T:System.Threading.Tasks.Task" />, releasing all of its unmanaged resources.</summary>
    /// <param name="disposing">A Boolean value that indicates whether this method is being called due to a call to <see cref="M:System.Threading.Tasks.Task.Dispose" />.</param>
    /// <exception cref="T:System.InvalidOperationException">The task is not in one of the final states: <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />, <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />, or <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />.</exception>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        if ((this.Options & (TaskCreationOptions) 16384) != TaskCreationOptions.None)
          return;
        if (!this.IsCompleted)
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Task_Dispose_NotCompleted);
        Task.ContingentProperties contingentProperties = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
        if (contingentProperties != null)
        {
          ManualResetEventSlim completionEvent = contingentProperties.m_completionEvent;
          if (completionEvent != null)
          {
            contingentProperties.m_completionEvent = (ManualResetEventSlim) null;
            if (!completionEvent.IsSet)
              completionEvent.Set();
            completionEvent.Dispose();
          }
        }
      }
      this.m_stateFlags |= 262144;
    }

    internal void ScheduleAndStart(bool needsProtection)
    {
      if (needsProtection)
      {
        if (!this.MarkStarted())
          return;
      }
      else
        this.m_stateFlags |= 65536;
      if (Task.s_asyncDebuggingEnabled)
        Task.AddToActiveTasks(this);
      if (TplEventSource.Log.IsEnabled() && (this.Options & (TaskCreationOptions) 512) == TaskCreationOptions.None)
        TplEventSource.Log.TraceOperationBegin(this.Id, "Task: " + this.m_action.Method.Name, 0L);
      try
      {
        this.m_taskScheduler.InternalQueueTask(this);
      }
      catch (System.Exception ex)
      {
        TaskSchedulerException exceptionObject = new TaskSchedulerException(ex);
        this.AddException((object) exceptionObject);
        this.Finish(false);
        if ((this.Options & (TaskCreationOptions) 512) == TaskCreationOptions.None)
          this.m_contingentProperties.m_exceptionsHolder.MarkAsHandled(false);
        throw exceptionObject;
      }
    }


    #nullable disable
    internal void AddException(object exceptionObject) => this.AddException(exceptionObject, false);

    internal void AddException(object exceptionObject, bool representsCancellation)
    {
      Task.ContingentProperties contingentProperties = this.EnsureContingentPropertiesInitialized();
      if (contingentProperties.m_exceptionsHolder == null)
      {
        TaskExceptionHolder taskExceptionHolder = new TaskExceptionHolder(this);
        if (Interlocked.CompareExchange<TaskExceptionHolder>(ref contingentProperties.m_exceptionsHolder, taskExceptionHolder, (TaskExceptionHolder) null) != null)
          taskExceptionHolder.MarkAsHandled(false);
      }
      lock (contingentProperties)
        contingentProperties.m_exceptionsHolder.Add(exceptionObject, representsCancellation);
    }

    private AggregateException GetExceptions(bool includeTaskCanceledExceptions)
    {
      System.Exception includeThisException = (System.Exception) null;
      if (includeTaskCanceledExceptions && this.IsCanceled)
      {
        includeThisException = (System.Exception) new TaskCanceledException(this);
        includeThisException.SetCurrentStackTrace();
      }
      if (this.ExceptionRecorded)
        return this.m_contingentProperties.m_exceptionsHolder.CreateExceptionObject(false, includeThisException);
      if (includeThisException == null)
        return (AggregateException) null;
      return new AggregateException(new System.Exception[1]
      {
        includeThisException
      });
    }

    internal List<ExceptionDispatchInfo> GetExceptionDispatchInfos() => this.m_contingentProperties.m_exceptionsHolder.GetExceptionDispatchInfos();

    internal ExceptionDispatchInfo GetCancellationExceptionDispatchInfo() => Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties)?.m_exceptionsHolder?.GetCancellationExceptionDispatchInfo();

    internal void MarkExceptionsAsHandled() => Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties)?.m_exceptionsHolder?.MarkAsHandled(false);

    internal void ThrowIfExceptional(bool includeTaskCanceledExceptions)
    {
      System.Exception exceptions = (System.Exception) this.GetExceptions(includeTaskCanceledExceptions);
      if (exceptions != null)
      {
        this.UpdateExceptionObservedStatus();
        throw exceptions;
      }
    }

    internal static void ThrowAsync(System.Exception exception, SynchronizationContext targetContext)
    {
      ExceptionDispatchInfo state1 = ExceptionDispatchInfo.Capture(exception);
      if (targetContext != null)
      {
        try
        {
          targetContext.Post((SendOrPostCallback) (state => ((ExceptionDispatchInfo) state).Throw()), (object) state1);
          return;
        }
        catch (System.Exception ex)
        {
          state1 = ExceptionDispatchInfo.Capture((System.Exception) new AggregateException(new System.Exception[2]
          {
            exception,
            ex
          }));
        }
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) (state => ((ExceptionDispatchInfo) state).Throw()), (object) state1);
    }

    internal void UpdateExceptionObservedStatus()
    {
      Task parent = this.m_contingentProperties?.m_parent;
      if (parent == null || (this.Options & TaskCreationOptions.AttachedToParent) == TaskCreationOptions.None || (parent.CreationOptions & TaskCreationOptions.DenyChildAttach) != TaskCreationOptions.None || Task.InternalCurrent != parent)
        return;
      this.m_stateFlags |= 524288;
    }

    internal bool IsExceptionObservedByParent => (this.m_stateFlags & 524288) != 0;

    internal bool IsDelegateInvoked => (this.m_stateFlags & 131072) != 0;

    internal void Finish(bool userDelegateExecute)
    {
      if (this.m_contingentProperties == null)
        this.FinishStageTwo();
      else
        this.FinishSlow(userDelegateExecute);
    }

    private void FinishSlow(bool userDelegateExecute)
    {
      if (!userDelegateExecute)
      {
        this.FinishStageTwo();
      }
      else
      {
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        if (contingentProperties.m_completionCountdown == 1 || Interlocked.Decrement(ref contingentProperties.m_completionCountdown) == 0)
          this.FinishStageTwo();
        else
          this.AtomicStateUpdate(8388608, 23068672);
        List<Task> exceptionalChildren = contingentProperties.m_exceptionalChildren;
        if (exceptionalChildren == null)
          return;
        lock (exceptionalChildren)
          exceptionalChildren.RemoveAll((Predicate<Task>) (t => t.IsExceptionObservedByParent));
      }
    }

    private void FinishStageTwo()
    {
      Task.ContingentProperties props = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
      if (props != null)
        this.AddExceptionsFromChildren(props);
      int num;
      if (this.ExceptionRecorded)
      {
        num = 2097152;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Error);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks(this);
      }
      else if (this.IsCancellationRequested && this.IsCancellationAcknowledged)
      {
        num = 4194304;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Canceled);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks(this);
      }
      else
      {
        num = 16777216;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks(this);
      }
      Interlocked.Exchange(ref this.m_stateFlags, this.m_stateFlags | num);
      Task.ContingentProperties contingentProperties = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
      if (contingentProperties != null)
      {
        contingentProperties.SetCompleted();
        contingentProperties.UnregisterCancellationCallback();
      }
      this.FinishStageThree();
    }

    internal void FinishStageThree()
    {
      this.m_action = (Delegate) null;
      Task.ContingentProperties contingentProperties = this.m_contingentProperties;
      if (contingentProperties != null)
      {
        contingentProperties.m_capturedContext = (ExecutionContext) null;
        this.NotifyParentIfPotentiallyAttachedTask();
      }
      this.FinishContinuations();
    }

    internal void NotifyParentIfPotentiallyAttachedTask()
    {
      Task parent = this.m_contingentProperties?.m_parent;
      if (parent == null || (parent.CreationOptions & TaskCreationOptions.DenyChildAttach) != TaskCreationOptions.None || (this.m_stateFlags & (int) ushort.MaxValue & 4) == 0)
        return;
      parent.ProcessChildCompletion(this);
    }

    internal void ProcessChildCompletion(Task childTask)
    {
      Task.ContingentProperties contingentProperties = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
      if (childTask.IsFaulted && !childTask.IsExceptionObservedByParent)
      {
        if (contingentProperties.m_exceptionalChildren == null)
          Interlocked.CompareExchange<List<Task>>(ref contingentProperties.m_exceptionalChildren, new List<Task>(), (List<Task>) null);
        List<Task> exceptionalChildren = contingentProperties.m_exceptionalChildren;
        if (exceptionalChildren != null)
        {
          lock (exceptionalChildren)
            exceptionalChildren.Add(childTask);
        }
      }
      if (Interlocked.Decrement(ref contingentProperties.m_completionCountdown) != 0)
        return;
      this.FinishStageTwo();
    }

    internal void AddExceptionsFromChildren(Task.ContingentProperties props)
    {
      List<Task> exceptionalChildren = props.m_exceptionalChildren;
      if (exceptionalChildren == null)
        return;
      lock (exceptionalChildren)
      {
        foreach (Task task in exceptionalChildren)
        {
          if (task.IsFaulted && !task.IsExceptionObservedByParent)
            this.AddException((object) Volatile.Read<Task.ContingentProperties>(ref task.m_contingentProperties).m_exceptionsHolder.CreateExceptionObject(false, (System.Exception) null));
        }
      }
      props.m_exceptionalChildren = (List<Task>) null;
    }

    internal bool ExecuteEntry()
    {
      int oldFlags = 0;
      if (!this.AtomicStateUpdate(131072, 23199744, ref oldFlags) && (oldFlags & 4194304) == 0)
        return false;
      if (!this.IsCancellationRequested & !this.IsCanceled)
        this.ExecuteWithThreadLocal(ref Task.t_currentTask);
      else
        this.ExecuteEntryCancellationRequestedOrCanceled();
      return true;
    }

    internal virtual void ExecuteFromThreadPool(Thread threadPoolThread) => this.ExecuteEntryUnsafe(threadPoolThread);

    internal void ExecuteEntryUnsafe(Thread threadPoolThread)
    {
      this.m_stateFlags |= 131072;
      if (!this.IsCancellationRequested & !this.IsCanceled)
        this.ExecuteWithThreadLocal(ref Task.t_currentTask, threadPoolThread);
      else
        this.ExecuteEntryCancellationRequestedOrCanceled();
    }

    internal void ExecuteEntryCancellationRequestedOrCanceled()
    {
      if (this.IsCanceled || (Interlocked.Exchange(ref this.m_stateFlags, this.m_stateFlags | 4194304) & 4194304) != 0)
        return;
      this.CancellationCleanupLogic();
    }

    private void ExecuteWithThreadLocal(ref Task currentTaskSlot, Thread threadPoolThread = null)
    {
      Task task = currentTaskSlot;
      TplEventSource log = TplEventSource.Log;
      Guid oldActivityThatWillContinue = new Guid();
      bool flag = log.IsEnabled();
      if (flag)
      {
        if (log.TasksSetActivityIds)
          EventSource.SetCurrentThreadActivityId(TplEventSource.CreateGuidForTaskID(this.Id), out oldActivityThatWillContinue);
        if (task != null)
          log.TaskStarted(task.m_taskScheduler.Id, task.Id, this.Id);
        else
          log.TaskStarted(TaskScheduler.Current.Id, 0, this.Id);
        log.TraceSynchronousWorkBegin(this.Id, CausalitySynchronousWork.Execution);
      }
      try
      {
        currentTaskSlot = this;
        try
        {
          ExecutionContext capturedContext = this.CapturedContext;
          if (capturedContext == null)
            this.InnerInvoke();
          else if (threadPoolThread == null)
            ExecutionContext.RunInternal(capturedContext, Task.s_ecCallback, (object) this);
          else
            ExecutionContext.RunFromThreadPoolDispatchLoop(threadPoolThread, capturedContext, Task.s_ecCallback, (object) this);
        }
        catch (System.Exception ex)
        {
          this.HandleException(ex);
        }
        if (flag)
          log.TraceSynchronousWorkEnd(CausalitySynchronousWork.Execution);
        this.Finish(true);
      }
      finally
      {
        currentTaskSlot = task;
        if (flag)
        {
          if (task != null)
            log.TaskCompleted(task.m_taskScheduler.Id, task.Id, this.Id, this.IsFaulted);
          else
            log.TaskCompleted(TaskScheduler.Current.Id, 0, this.Id, this.IsFaulted);
          if (log.TasksSetActivityIds)
            EventSource.SetCurrentThreadActivityId(oldActivityThatWillContinue);
        }
      }
    }

    internal virtual void InnerInvoke()
    {
      if (this.m_action is Action action1)
      {
        action1();
      }
      else
      {
        if (!(this.m_action is Action<object> action))
          return;
        action(this.m_stateObject);
      }
    }

    private void HandleException(System.Exception unhandledException)
    {
      if (unhandledException is OperationCanceledException exceptionObject && this.IsCancellationRequested && this.m_contingentProperties.m_cancellationToken == exceptionObject.CancellationToken)
      {
        this.SetCancellationAcknowledged();
        this.AddException((object) exceptionObject, true);
      }
      else
        this.AddException((object) unhandledException);
    }

    /// <summary>Gets an awaiter used to await this <see cref="T:System.Threading.Tasks.Task" />.</summary>
    /// <returns>An awaiter instance.</returns>
    public TaskAwaiter GetAwaiter() => new TaskAwaiter(this);

    /// <summary>Configures an awaiter used to await this <see cref="T:System.Threading.Tasks.Task" />.</summary>
    /// <param name="continueOnCapturedContext">
    /// <see langword="true" /> to attempt to marshal the continuation back to the original context captured; otherwise, <see langword="false" />.</param>
    /// <returns>An object used to await this task.</returns>
    public ConfiguredTaskAwaitable ConfigureAwait(
      bool continueOnCapturedContext)
    {
      return new ConfiguredTaskAwaitable(this, continueOnCapturedContext);
    }

    internal void SetContinuationForAwait(
      Action continuationAction,
      bool continueOnCapturedContext,
      bool flowExecutionContext)
    {
      TaskContinuation tc = (TaskContinuation) null;
      if (continueOnCapturedContext)
      {
        SynchronizationContext current = SynchronizationContext.Current;
        if (current != null && current.GetType() != typeof (SynchronizationContext))
        {
          tc = (TaskContinuation) new SynchronizationContextAwaitTaskContinuation(current, continuationAction, flowExecutionContext);
        }
        else
        {
          TaskScheduler internalCurrent = TaskScheduler.InternalCurrent;
          if (internalCurrent != null && internalCurrent != TaskScheduler.Default)
            tc = (TaskContinuation) new TaskSchedulerAwaitTaskContinuation(internalCurrent, continuationAction, flowExecutionContext);
        }
      }
      if (tc == null & flowExecutionContext)
        tc = (TaskContinuation) new AwaitTaskContinuation(continuationAction, true);
      if (tc != null)
      {
        if (this.AddTaskContinuation((object) tc, false))
          return;
        tc.Run(this, false);
      }
      else
      {
        if (this.AddTaskContinuation((object) continuationAction, false))
          return;
        AwaitTaskContinuation.UnsafeScheduleAction(continuationAction, this);
      }
    }

    internal void UnsafeSetContinuationForAwait(
      IAsyncStateMachineBox stateMachineBox,
      bool continueOnCapturedContext)
    {
      if (continueOnCapturedContext)
      {
        SynchronizationContext current = SynchronizationContext.Current;
        if (current != null && current.GetType() != typeof (SynchronizationContext))
        {
          SynchronizationContextAwaitTaskContinuation tc = new SynchronizationContextAwaitTaskContinuation(current, stateMachineBox.MoveNextAction, false);
          if (this.AddTaskContinuation((object) tc, false))
            return;
          tc.Run(this, false);
          return;
        }
        TaskScheduler internalCurrent = TaskScheduler.InternalCurrent;
        if (internalCurrent != null && internalCurrent != TaskScheduler.Default)
        {
          TaskSchedulerAwaitTaskContinuation tc = new TaskSchedulerAwaitTaskContinuation(internalCurrent, stateMachineBox.MoveNextAction, false);
          if (this.AddTaskContinuation((object) tc, false))
            return;
          tc.Run(this, false);
          return;
        }
      }
      if (this.AddTaskContinuation((object) stateMachineBox, false))
        return;
      ThreadPool.UnsafeQueueUserWorkItemInternal((object) stateMachineBox, true);
    }

    /// <summary>Creates an awaitable task that asynchronously yields back to the current context when awaited.</summary>
    /// <returns>A context that, when awaited, will asynchronously transition back into the current context at the time of the await. If the current <see cref="T:System.Threading.SynchronizationContext" /> is non-null, it is treated as the current context. Otherwise, the task scheduler that is associated with the currently executing task is treated as the current context.</returns>
    public static YieldAwaitable Yield() => new YieldAwaitable();

    /// <summary>Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">The task was canceled. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains a <see cref="T:System.Threading.Tasks.TaskCanceledException" /> object.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of the task. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains information about the exception or exceptions.</exception>
    public void Wait() => this.Wait(-1, new CancellationToken());

    /// <summary>Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution within a specified time interval.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.AggregateException">The task was canceled. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains a <see cref="T:System.Threading.Tasks.TaskCanceledException" /> object.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of the task. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains information about the exception or exceptions.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.Tasks.Task" /> completed execution within the allotted time; otherwise, <see langword="false" />.</returns>
    public bool Wait(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.timeout);
      return this.Wait((int) totalMilliseconds, new CancellationToken());
    }

    /// <summary>Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution. The wait terminates if a cancellation token is canceled before the task completes.</summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <exception cref="T:System.OperationCanceledException">The <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The task has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">The task was canceled. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains a <see cref="T:System.Threading.Tasks.TaskCanceledException" /> object.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of the task. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains information about the exception or exceptions.</exception>
    public void Wait(CancellationToken cancellationToken) => this.Wait(-1, cancellationToken);

    /// <summary>Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution within a specified number of milliseconds.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.AggregateException">The task was canceled. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains a <see cref="T:System.Threading.Tasks.TaskCanceledException" /> object.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of the task. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains information about the exception or exceptions.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.Tasks.Task" /> completed execution within the allotted time; otherwise, <see langword="false" />.</returns>
    public bool Wait(int millisecondsTimeout) => this.Wait(millisecondsTimeout, new CancellationToken());

    /// <summary>Waits for the <see cref="T:System.Threading.Tasks.Task" /> to complete execution. The wait terminates if a timeout interval elapses or a cancellation token is canceled before the task completes.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <exception cref="T:System.OperationCanceledException">The <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.AggregateException">The task was canceled. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains a <see cref="T:System.Threading.Tasks.TaskCanceledException" /> object.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of the task. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains information about the exception or exceptions.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.Tasks.Task" /> completed execution within the allotted time; otherwise, <see langword="false" />.</returns>
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      if (millisecondsTimeout < -1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsTimeout);
      if (!this.IsWaitNotificationEnabledOrNotRanToCompletion)
        return true;
      if (!this.InternalWait(millisecondsTimeout, cancellationToken))
        return false;
      if (this.IsWaitNotificationEnabledOrNotRanToCompletion)
      {
        this.NotifyDebuggerOfWaitCompletionIfNecessary();
        if (this.IsCanceled)
          cancellationToken.ThrowIfCancellationRequested();
        this.ThrowIfExceptional(true);
      }
      return true;
    }


    #nullable enable
    /// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that will complete when this <see cref="T:System.Threading.Tasks.Task" /> completes or when the specified <see cref="P:System.Threading.CancellationToken" /> has cancellation requested.</summary>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.CancellationToken" /> to monitor for a cancellation request.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous wait. It may or may not be the same instance as the current instance.</returns>
    public Task WaitAsync(CancellationToken cancellationToken) => this.WaitAsync(uint.MaxValue, cancellationToken);

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that will complete when this <see cref="T:System.Threading.Tasks.Task" /> completes or when the specified timeout expires.</summary>
    /// <param name="timeout">The timeout after which the <see cref="T:System.Threading.Tasks.Task" /> should be faulted with a <see cref="T:System.TimeoutException" /> if it hasn't otherwise completed.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous wait. It may or may not be the same instance as the current instance.</returns>
    public Task WaitAsync(TimeSpan timeout) => this.WaitAsync(Task.ValidateTimeout(timeout, ExceptionArgument.timeout), new CancellationToken());

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that will complete when this <see cref="T:System.Threading.Tasks.Task" /> completes, when the specified timeout expires, or when the specified <see cref="P:System.Threading.CancellationToken" /> has cancellation requested.</summary>
    /// <param name="timeout">The timeout after which the <see cref="T:System.Threading.Tasks.Task" /> should be faulted with a <see cref="T:System.TimeoutException" /> if it hasn't otherwise completed.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.CancellationToken" /> to monitor for a cancellation request.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous wait. It may or may not be the same instance as the current instance.</returns>
    public Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken) => this.WaitAsync(Task.ValidateTimeout(timeout, ExceptionArgument.timeout), cancellationToken);


    #nullable disable
    private Task WaitAsync(uint millisecondsTimeout, CancellationToken cancellationToken)
    {
      if (this.IsCompleted || !cancellationToken.CanBeCanceled && millisecondsTimeout == uint.MaxValue)
        return this;
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      return millisecondsTimeout == 0U ? Task.FromException((System.Exception) new TimeoutException()) : (Task) new Task.CancellationPromise<VoidTaskResult>(this, millisecondsTimeout, cancellationToken);
    }

    private bool WrappedTryRunInline()
    {
      if (this.m_taskScheduler == null)
        return false;
      try
      {
        return this.m_taskScheduler.TryRunInline(this, true);
      }
      catch (System.Exception ex)
      {
        throw new TaskSchedulerException(ex);
      }
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    internal bool InternalWait(int millisecondsTimeout, CancellationToken cancellationToken) => this.InternalWaitCore(millisecondsTimeout, cancellationToken);

    private bool InternalWaitCore(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      if (this.IsCompleted)
        return true;
      TplEventSource log = TplEventSource.Log;
      bool flag1 = log.IsEnabled();
      if (flag1)
      {
        Task internalCurrent = Task.InternalCurrent;
        log.TaskWaitBegin(internalCurrent != null ? internalCurrent.m_taskScheduler.Id : TaskScheduler.Default.Id, internalCurrent != null ? internalCurrent.Id : 0, this.Id, TplEventSource.TaskWaitBehavior.Synchronous, 0);
      }
      Debugger.NotifyOfCrossThreadDependency();
      bool flag2 = millisecondsTimeout == -1 && !cancellationToken.CanBeCanceled && this.WrappedTryRunInline() && this.IsCompleted || this.SpinThenBlockingWait(millisecondsTimeout, cancellationToken);
      if (flag1)
      {
        Task internalCurrent = Task.InternalCurrent;
        if (internalCurrent != null)
          log.TaskWaitEnd(internalCurrent.m_taskScheduler.Id, internalCurrent.Id, this.Id);
        else
          log.TaskWaitEnd(TaskScheduler.Default.Id, 0, this.Id);
        log.TaskWaitContinuationComplete(this.Id);
      }
      return flag2;
    }

    private bool SpinThenBlockingWait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      bool flag1 = millisecondsTimeout == -1;
      uint num1 = flag1 ? 0U : (uint) Environment.TickCount;
      bool flag2 = this.SpinWait(millisecondsTimeout);
      if (!flag2)
      {
        Task.SetOnInvokeMres setOnInvokeMres = new Task.SetOnInvokeMres();
        try
        {
          this.AddCompletionAction((ITaskCompletionAction) setOnInvokeMres, true);
          if (flag1)
          {
            bool flag3 = ThreadPool.NotifyThreadBlocked();
            try
            {
              flag2 = setOnInvokeMres.Wait(-1, cancellationToken);
            }
            finally
            {
              if (flag3)
                ThreadPool.NotifyThreadUnblocked();
            }
          }
          else
          {
            uint num2 = (uint) Environment.TickCount - num1;
            if ((long) num2 < (long) millisecondsTimeout)
            {
              bool flag4 = ThreadPool.NotifyThreadBlocked();
              try
              {
                flag2 = setOnInvokeMres.Wait((int) ((long) millisecondsTimeout - (long) num2), cancellationToken);
              }
              finally
              {
                if (flag4)
                  ThreadPool.NotifyThreadUnblocked();
              }
            }
          }
        }
        finally
        {
          if (!this.IsCompleted)
            this.RemoveContinuation((object) setOnInvokeMres);
        }
      }
      return flag2;
    }

    private bool SpinWait(int millisecondsTimeout)
    {
      if (this.IsCompleted)
        return true;
      if (millisecondsTimeout == 0)
        return false;
      int countforSpinBeforeWait = System.Threading.SpinWait.SpinCountforSpinBeforeWait;
      System.Threading.SpinWait spinWait = new System.Threading.SpinWait();
      while (spinWait.Count < countforSpinBeforeWait)
      {
        spinWait.SpinOnce(-1);
        if (this.IsCompleted)
          return true;
      }
      return false;
    }

    internal void InternalCancel()
    {
      TaskSchedulerException schedulerException = (TaskSchedulerException) null;
      bool flag1 = false;
      if ((this.m_stateFlags & 65536) != 0)
      {
        TaskScheduler taskScheduler = this.m_taskScheduler;
        try
        {
          flag1 = taskScheduler != null && taskScheduler.TryDequeue(this);
        }
        catch (System.Exception ex)
        {
          schedulerException = new TaskSchedulerException(ex);
        }
      }
      this.RecordInternalCancellationRequest();
      bool flag2 = false;
      if (flag1)
        flag2 = this.AtomicStateUpdate(4194304, 4325376);
      else if ((this.m_stateFlags & 65536) == 0)
        flag2 = this.AtomicStateUpdate(4194304, 23265280);
      if (flag2)
        this.CancellationCleanupLogic();
      if (schedulerException != null)
        throw schedulerException;
    }

    internal void InternalCancelContinueWithInitialState()
    {
      this.m_stateFlags |= 4194304;
      this.CancellationCleanupLogic();
    }

    internal void RecordInternalCancellationRequest() => this.EnsureContingentPropertiesInitialized().m_internalCancellationRequested = 1;

    internal void RecordInternalCancellationRequest(
      CancellationToken tokenToRecord,
      object cancellationException)
    {
      this.RecordInternalCancellationRequest();
      if (tokenToRecord != new CancellationToken())
        this.m_contingentProperties.m_cancellationToken = tokenToRecord;
      if (cancellationException == null)
        return;
      this.AddException(cancellationException, true);
    }

    internal void CancellationCleanupLogic()
    {
      Interlocked.Exchange(ref this.m_stateFlags, this.m_stateFlags | 4194304);
      Task.ContingentProperties contingentProperties = Volatile.Read<Task.ContingentProperties>(ref this.m_contingentProperties);
      if (contingentProperties != null)
      {
        contingentProperties.SetCompleted();
        contingentProperties.UnregisterCancellationCallback();
      }
      if (TplEventSource.Log.IsEnabled())
        TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Canceled);
      if (Task.s_asyncDebuggingEnabled)
        Task.RemoveFromActiveTasks(this);
      this.FinishStageThree();
    }

    private void SetCancellationAcknowledged() => this.m_stateFlags |= 1048576;

    internal bool TrySetResult()
    {
      if (!this.AtomicStateUpdate(83886080, 90177536))
        return false;
      Task.ContingentProperties contingentProperties = this.m_contingentProperties;
      if (contingentProperties != null)
      {
        this.NotifyParentIfPotentiallyAttachedTask();
        contingentProperties.SetCompleted();
      }
      this.FinishContinuations();
      return true;
    }

    internal bool TrySetException(object exceptionObject)
    {
      bool flag = false;
      this.EnsureContingentPropertiesInitialized();
      if (this.AtomicStateUpdate(67108864, 90177536))
      {
        this.AddException(exceptionObject);
        this.Finish(false);
        flag = true;
      }
      return flag;
    }

    internal bool TrySetCanceled(CancellationToken tokenToRecord) => this.TrySetCanceled(tokenToRecord, (object) null);

    internal bool TrySetCanceled(CancellationToken tokenToRecord, object cancellationException)
    {
      bool flag = false;
      if (this.AtomicStateUpdate(67108864, 90177536))
      {
        this.RecordInternalCancellationRequest(tokenToRecord, cancellationException);
        this.CancellationCleanupLogic();
        flag = true;
      }
      return flag;
    }

    internal void FinishContinuations()
    {
      object continuationObject = Interlocked.Exchange(ref this.m_continuationObject, Task.s_taskCompletionSentinel);
      if (continuationObject == null)
        return;
      this.RunContinuations(continuationObject);
    }

    private void RunContinuations(object continuationObject)
    {
      TplEventSource log = TplEventSource.Log;
      bool flag1 = log.IsEnabled();
      if (flag1)
        log.TraceSynchronousWorkBegin(this.Id, CausalitySynchronousWork.CompletionNotification);
      bool flag2 = (this.m_stateFlags & 64) == 0 && RuntimeHelpers.TryEnsureSufficientExecutionStack();
      switch (continuationObject)
      {
        case IAsyncStateMachineBox box1:
          AwaitTaskContinuation.RunOrScheduleAction(box1, flag2);
          Task.LogFinishCompletionNotification();
          break;
        case Action action1:
          AwaitTaskContinuation.RunOrScheduleAction(action1, flag2);
          Task.LogFinishCompletionNotification();
          break;
        case TaskContinuation taskContinuation1:
          taskContinuation1.Run(this, flag2);
          Task.LogFinishCompletionNotification();
          break;
        case ITaskCompletionAction completionAction:
          this.RunOrQueueCompletionAction(completionAction, flag2);
          Task.LogFinishCompletionNotification();
          break;
        default:
          List<object> objectList = (List<object>) continuationObject;
          lock (objectList)
            ;
          int count = objectList.Count;
          if (flag2)
          {
            bool flag3 = false;
            for (int index = 0; index < count; ++index)
            {
              object Object = objectList[index];
              switch (Object)
              {
                case null:
                case ITaskCompletionAction _:
                  continue;
                case ContinueWithTaskContinuation taskContinuation:
                  if ((taskContinuation.m_options & TaskContinuationOptions.ExecuteSynchronously) == TaskContinuationOptions.None)
                  {
                    objectList[index] = (object) null;
                    if (flag1)
                      log.RunningContinuationList(this.Id, index, (object) taskContinuation);
                    taskContinuation.Run(this, false);
                    continue;
                  }
                  continue;
                default:
                  if (flag3)
                  {
                    objectList[index] = (object) null;
                    if (flag1)
                      log.RunningContinuationList(this.Id, index, Object);
                    switch (Object)
                    {
                      case IAsyncStateMachineBox box:
                        AwaitTaskContinuation.RunOrScheduleAction(box, false);
                        break;
                      case Action action:
                        AwaitTaskContinuation.RunOrScheduleAction(action, false);
                        break;
                      default:
                        ((TaskContinuation) Object).Run(this, false);
                        break;
                    }
                  }
                  flag3 = true;
                  continue;
              }
            }
          }
          for (int index = 0; index < count; ++index)
          {
            object Object = objectList[index];
            if (Object != null)
            {
              objectList[index] = (object) null;
              if (flag1)
                log.RunningContinuationList(this.Id, index, Object);
              switch (Object)
              {
                case IAsyncStateMachineBox box:
                  AwaitTaskContinuation.RunOrScheduleAction(box, flag2);
                  continue;
                case Action action:
                  AwaitTaskContinuation.RunOrScheduleAction(action, flag2);
                  continue;
                case TaskContinuation taskContinuation:
                  taskContinuation.Run(this, flag2);
                  continue;
                default:
                  this.RunOrQueueCompletionAction((ITaskCompletionAction) Object, flag2);
                  continue;
              }
            }
          }
          Task.LogFinishCompletionNotification();
          break;
      }
    }

    private void RunOrQueueCompletionAction(
      ITaskCompletionAction completionAction,
      bool allowInlining)
    {
      if (allowInlining || !completionAction.InvokeMayRunArbitraryCode)
        completionAction.Invoke(this);
      else
        ThreadPool.UnsafeQueueUserWorkItemInternal((object) new CompletionActionInvoker(completionAction, this), true);
    }

    private static void LogFinishCompletionNotification()
    {
      if (!TplEventSource.Log.IsEnabled())
        return;
      TplEventSource.Log.TraceSynchronousWorkEnd(CausalitySynchronousWork.CompletionNotification);
    }


    #nullable enable
    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(Action<Task> continuationAction) => this.ContinueWith(continuationAction, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);

    /// <summary>Creates a continuation that receives a cancellation token and executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created the token has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is null.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task> continuationAction,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith(continuationAction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes. The continuation uses a specified scheduler.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is null.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler) => this.ContinueWith(continuationAction, scheduler, new CancellationToken(), TaskContinuationOptions.None);

    /// <summary>Creates a continuation that executes when the target task completes according to the specified <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</summary>
    /// <param name="continuationAction">An action to run according to the specified <paramref name="continuationOptions" />. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task> continuationAction,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith(continuationAction, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that executes when the target task competes according to the specified <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />. The continuation receives a cancellation token and uses a specified scheduler.</summary>
    /// <param name="continuationAction">An action to run according to the specified <paramref name="continuationOptions" />. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created the token has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is null.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task> continuationAction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith(continuationAction, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    private Task ContinueWith(
      Action<Task> continuationAction,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationAction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task continuationTask = (Task) new ContinuationTaskFromTask(this, (Delegate) continuationAction, (object) null, creationOptions, internalOptions);
      this.ContinueWithCore(continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }


    #nullable enable
    /// <summary>Creates a continuation that receives caller-supplied state information and executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the task completes. When run, the delegate is passed the completed task and a caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation task.</returns>
    public Task ContinueWith(Action<Task, object?> continuationAction, object? state) => this.ContinueWith(continuationAction, state, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);

    /// <summary>Creates a continuation that receives caller-supplied state information and a cancellation token and that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task, object?> continuationAction,
      object? state,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith(continuationAction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that receives caller-supplied state information and executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes. The continuation uses a specified scheduler.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes.  When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task, object?> continuationAction,
      object? state,
      TaskScheduler scheduler)
    {
      return this.ContinueWith(continuationAction, state, scheduler, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that receives caller-supplied state information and executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes. The continuation executes based on a set of specified conditions.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task, object?> continuationAction,
      object? state,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith(continuationAction, state, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that receives caller-supplied state information and a cancellation token and that executes when the target <see cref="T:System.Threading.Tasks.Task" /> completes. The continuation executes based on a set of specified conditions and uses a specified scheduler.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its  execution.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task, object?> continuationAction,
      object? state,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith(continuationAction, state, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    private Task ContinueWith(
      Action<Task, object> continuationAction,
      object state,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationAction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task continuationTask = (Task) new ContinuationTaskFromTask(this, (Delegate) continuationAction, state, creationOptions, internalOptions);
      this.ContinueWithCore(continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }


    #nullable enable
    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes and returns a value.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is null.</exception>
    /// <returns>A new continuation task.</returns>
    public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction) => this.ContinueWith<TResult>(continuationFunction, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes and returns a value. The continuation receives a cancellation token.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created the token has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is null.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith<TResult>(continuationFunction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes and returns a value. The continuation uses a specified scheduler.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is null.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is null.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TResult>(continuationFunction, scheduler, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes according to the specified continuation options and returns a value.</summary>
    /// <param name="continuationFunction">A function to run according to the condition specified in <paramref name="continuationOptions" />. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith<TResult>(continuationFunction, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that executes according to the specified continuation options and returns a value. The continuation is passed a cancellation token and uses a specified scheduler.</summary>
    /// <param name="continuationFunction">A function to run according to the specified <c>continuationOptions.</c> When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="cancellationToken">The <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created the token has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is null.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TResult>(continuationFunction, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    private Task<TResult> ContinueWith<TResult>(
      Func<Task, TResult> continuationFunction,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task<TResult> continuationTask = (Task<TResult>) new ContinuationResultTaskFromTask<TResult>(this, (Delegate) continuationFunction, (object) null, creationOptions, internalOptions);
      this.ContinueWithCore((Task) continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }


    #nullable enable
    /// <summary>Creates a continuation that receives caller-supplied state information and executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes and returns a value.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object?, TResult> continuationFunction,
      object? state)
    {
      return this.ContinueWith<TResult>(continuationFunction, state, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes and returns a value. The continuation receives caller-supplied state information and a cancellation token.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object?, TResult> continuationFunction,
      object? state,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith<TResult>(continuationFunction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task" /> completes. The continuation receives caller-supplied state information and uses a specified scheduler.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes.  When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object?, TResult> continuationFunction,
      object? state,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TResult>(continuationFunction, state, scheduler, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes based on the specified task continuation options when the target <see cref="T:System.Threading.Tasks.Task" /> completes. The continuation receives caller-supplied state information.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object?, TResult> continuationFunction,
      object? state,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith<TResult>(continuationFunction, state, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that executes based on the specified task continuation options when the target <see cref="T:System.Threading.Tasks.Task" /> completes and returns a value. The continuation receives caller-supplied state information and a cancellation token and uses the specified scheduler.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its  execution.</param>
    /// <typeparam name="TResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TResult> ContinueWith<TResult>(
      Func<Task, object?, TResult> continuationFunction,
      object? state,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TResult>(continuationFunction, state, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    private Task<TResult> ContinueWith<TResult>(
      Func<Task, object, TResult> continuationFunction,
      object state,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task<TResult> continuationTask = (Task<TResult>) new ContinuationResultTaskFromTask<TResult>(this, (Delegate) continuationFunction, state, creationOptions, internalOptions);
      this.ContinueWithCore((Task) continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }

    internal static void CreationOptionsFromContinuationOptions(
      TaskContinuationOptions continuationOptions,
      out TaskCreationOptions creationOptions,
      out InternalTaskOptions internalOptions)
    {
      if ((continuationOptions & (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously)) == (TaskContinuationOptions.LongRunning | TaskContinuationOptions.ExecuteSynchronously))
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.continuationOptions, ExceptionResource.Task_ContinueWith_ESandLR);
      if ((continuationOptions & ~(TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.HideScheduler | TaskContinuationOptions.LazyCancellation | TaskContinuationOptions.RunContinuationsAsynchronously | TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously)) != TaskContinuationOptions.None)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.continuationOptions);
      if ((continuationOptions & (TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion)) == (TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnRanToCompletion))
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.continuationOptions, ExceptionResource.Task_ContinueWith_NotOnAnything);
      creationOptions = (TaskCreationOptions) (continuationOptions & (TaskContinuationOptions.PreferFairness | TaskContinuationOptions.LongRunning | TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.HideScheduler | TaskContinuationOptions.RunContinuationsAsynchronously));
      internalOptions = (continuationOptions & TaskContinuationOptions.LazyCancellation) != TaskContinuationOptions.None ? InternalTaskOptions.ContinuationTask | InternalTaskOptions.LazyCancellation : InternalTaskOptions.ContinuationTask;
    }

    internal void ContinueWithCore(
      Task continuationTask,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions options)
    {
      TaskContinuation taskContinuation = (TaskContinuation) new ContinueWithTaskContinuation(continuationTask, options, scheduler);
      if (cancellationToken.CanBeCanceled)
      {
        if (this.IsCompleted || cancellationToken.IsCancellationRequested)
          continuationTask.AssignCancellationToken(cancellationToken, (Task) null, (TaskContinuation) null);
        else
          continuationTask.AssignCancellationToken(cancellationToken, this, taskContinuation);
      }
      if (continuationTask.IsCompleted)
        return;
      if ((this.Options & (TaskCreationOptions) 1024) != TaskCreationOptions.None && !(this is ITaskCompletionAction))
      {
        TplEventSource log = TplEventSource.Log;
        if (log.IsEnabled())
          log.AwaitTaskContinuationScheduled(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), continuationTask.Id);
      }
      if (this.AddTaskContinuation((object) taskContinuation, false))
        return;
      taskContinuation.Run(this, true);
    }

    internal void AddCompletionAction(ITaskCompletionAction action, bool addBeforeOthers = false)
    {
      if (this.AddTaskContinuation((object) action, addBeforeOthers))
        return;
      action.Invoke(this);
    }

    private bool AddTaskContinuationComplex(object tc, bool addBeforeOthers)
    {
      object continuationObject1 = this.m_continuationObject;
      if (continuationObject1 != Task.s_taskCompletionSentinel && !(continuationObject1 is List<object>))
      {
        ref object local = ref this.m_continuationObject;
        List<object> objectList = new List<object>();
        objectList.Add(continuationObject1);
        object comparand = continuationObject1;
        Interlocked.CompareExchange(ref local, (object) objectList, comparand);
      }
      if (this.m_continuationObject is List<object> continuationObject2)
      {
        lock (continuationObject2)
        {
          if (this.m_continuationObject != Task.s_taskCompletionSentinel)
          {
            if (continuationObject2.Count == continuationObject2.Capacity)
              continuationObject2.RemoveAll((Predicate<object>) (l => l == null));
            if (addBeforeOthers)
              continuationObject2.Insert(0, tc);
            else
              continuationObject2.Add(tc);
            return true;
          }
        }
      }
      return false;
    }

    private bool AddTaskContinuation(object tc, bool addBeforeOthers)
    {
      if (this.IsCompleted)
        return false;
      return this.m_continuationObject == null && Interlocked.CompareExchange(ref this.m_continuationObject, tc, (object) null) == null || this.AddTaskContinuationComplex(tc, addBeforeOthers);
    }

    internal void RemoveContinuation(object continuationObject)
    {
      object continuationObject1 = this.m_continuationObject;
      if (continuationObject1 == Task.s_taskCompletionSentinel)
        return;
      if (!(continuationObject1 is List<object> objectList))
      {
        if (Interlocked.CompareExchange(ref this.m_continuationObject, (object) new List<object>(), continuationObject) == continuationObject)
          return;
        objectList = this.m_continuationObject as List<object>;
      }
      if (objectList == null)
        return;
      lock (objectList)
      {
        if (this.m_continuationObject == Task.s_taskCompletionSentinel)
          return;
        int index = objectList.IndexOf(continuationObject);
        if (index == -1)
          return;
        objectList[index] = (object) null;
      }
    }


    #nullable enable
    /// <summary>Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <exception cref="T:System.ObjectDisposedException">One or more of the <see cref="T:System.Threading.Tasks.Task" /> objects in <paramref name="tasks" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <exception cref="T:System.AggregateException">At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled. If a task was canceled, the <see cref="T:System.AggregateException" /> exception contains an <see cref="T:System.OperationCanceledException" /> exception in its <see cref="P:System.AggregateException.InnerExceptions" /> collection.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.</exception>
    [UnsupportedOSPlatform("browser")]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void WaitAll(params Task[] tasks) => Task.WaitAllCore(tasks, -1, new CancellationToken());

    /// <summary>Waits for all of the provided cancellable <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution within a specified time interval.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">One or more of the <see cref="T:System.Threading.Tasks.Task" /> objects in <paramref name="tasks" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled. If a task was canceled, the <see cref="T:System.AggregateException" /> contains an <see cref="T:System.OperationCanceledException" /> in its <see cref="P:System.AggregateException.InnerExceptions" /> collection.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <returns>
    /// <see langword="true" /> if all of the <see cref="T:System.Threading.Tasks.Task" /> instances completed execution within the allotted time; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static bool WaitAll(Task[] tasks, TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.timeout);
      return Task.WaitAllCore(tasks, (int) totalMilliseconds, new CancellationToken());
    }

    /// <summary>Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution within a specified number of milliseconds.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">One or more of the <see cref="T:System.Threading.Tasks.Task" /> objects in <paramref name="tasks" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled. If a task was canceled, the <see cref="T:System.AggregateException" /> contains an <see cref="T:System.OperationCanceledException" /> in its <see cref="P:System.AggregateException.InnerExceptions" /> collection.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <returns>
    /// <see langword="true" /> if all of the <see cref="T:System.Threading.Tasks.Task" /> instances completed execution within the allotted time; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static bool WaitAll(Task[] tasks, int millisecondsTimeout) => Task.WaitAllCore(tasks, millisecondsTimeout, new CancellationToken());

    /// <summary>Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution unless the wait is cancelled.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="cancellationToken">A <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> to observe while waiting for the tasks to complete.</param>
    /// <exception cref="T:System.OperationCanceledException">The <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled. If a task was canceled, the <see cref="T:System.AggregateException" /> contains an <see cref="T:System.OperationCanceledException" /> in its <see cref="P:System.AggregateException.InnerExceptions" /> collection.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <exception cref="T:System.ObjectDisposedException">One or more of the <see cref="T:System.Threading.Tasks.Task" /> objects in <paramref name="tasks" /> has been disposed.</exception>
    [UnsupportedOSPlatform("browser")]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static void WaitAll(Task[] tasks, CancellationToken cancellationToken) => Task.WaitAllCore(tasks, -1, cancellationToken);

    /// <summary>Waits for all of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution within a specified number of milliseconds or until the wait is cancelled.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">A <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> to observe while waiting for the tasks to complete.</param>
    /// <exception cref="T:System.ObjectDisposedException">One or more of the <see cref="T:System.Threading.Tasks.Task" /> objects in <paramref name="tasks" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">At least one of the <see cref="T:System.Threading.Tasks.Task" /> instances was canceled. If a task was canceled, the <see cref="T:System.AggregateException" /> contains an <see cref="T:System.OperationCanceledException" /> in its <see cref="P:System.AggregateException.InnerExceptions" /> collection.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of at least one of the <see cref="T:System.Threading.Tasks.Task" /> instances.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <paramref name="cancellationToken" /> was canceled.</exception>
    /// <returns>
    /// <see langword="true" /> if all of the <see cref="T:System.Threading.Tasks.Task" /> instances completed execution within the allotted time; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static bool WaitAll(
      Task[] tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      return Task.WaitAllCore(tasks, millisecondsTimeout, cancellationToken);
    }


    #nullable disable
    [UnsupportedOSPlatform("browser")]
    private static bool WaitAllCore(
      Task[] tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      if (millisecondsTimeout < -1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsTimeout);
      cancellationToken.ThrowIfCancellationRequested();
      List<System.Exception> exceptions = (List<System.Exception>) null;
      List<Task> list1 = (List<Task>) null;
      List<Task> list2 = (List<Task>) null;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = true;
      for (int index = tasks.Length - 1; index >= 0; --index)
      {
        Task task = tasks[index];
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_WaitMulti_NullTask, ExceptionArgument.tasks);
        bool flag4 = task.IsCompleted;
        if (!flag4)
        {
          if (millisecondsTimeout != -1 || cancellationToken.CanBeCanceled)
          {
            Task.AddToList<Task>(task, ref list1, tasks.Length);
          }
          else
          {
            flag4 = task.WrappedTryRunInline() && task.IsCompleted;
            if (!flag4)
              Task.AddToList<Task>(task, ref list1, tasks.Length);
          }
        }
        if (flag4)
        {
          if (task.IsFaulted)
            flag1 = true;
          else if (task.IsCanceled)
            flag2 = true;
          if (task.IsWaitNotificationEnabled)
            Task.AddToList<Task>(task, ref list2, 1);
        }
      }
      if (list1 != null)
      {
        flag3 = Task.WaitAllBlockingCore(list1, millisecondsTimeout, cancellationToken);
        if (flag3)
        {
          foreach (Task task in list1)
          {
            if (task.IsFaulted)
              flag1 = true;
            else if (task.IsCanceled)
              flag2 = true;
            if (task.IsWaitNotificationEnabled)
              Task.AddToList<Task>(task, ref list2, 1);
          }
        }
        GC.KeepAlive((object) tasks);
      }
      if (flag3 && list2 != null)
      {
        foreach (Task task in list2)
        {
          if (task.NotifyDebuggerOfWaitCompletionIfNecessary())
            break;
        }
      }
      if (flag3 && flag1 | flag2)
      {
        if (!flag1)
          cancellationToken.ThrowIfCancellationRequested();
        foreach (Task task in tasks)
          Task.AddExceptionsForCompletedTask(ref exceptions, task);
        ThrowHelper.ThrowAggregateException(exceptions);
      }
      return flag3;
    }

    private static void AddToList<T>(T item, ref List<T> list, int initSize)
    {
      if (list == null)
        list = new List<T>(initSize);
      list.Add(item);
    }

    [UnsupportedOSPlatform("browser")]
    private static bool WaitAllBlockingCore(
      List<Task> tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      bool flag = false;
      Task.SetOnCountdownMres setOnCountdownMres = new Task.SetOnCountdownMres(tasks.Count);
      try
      {
        foreach (Task task in tasks)
          task.AddCompletionAction((ITaskCompletionAction) setOnCountdownMres, true);
        flag = setOnCountdownMres.Wait(millisecondsTimeout, cancellationToken);
        return flag;
      }
      finally
      {
        if (!flag)
        {
          foreach (Task task in tasks)
          {
            if (!task.IsCompleted)
              task.RemoveContinuation((object) setOnCountdownMres);
          }
        }
      }
    }

    internal static void AddExceptionsForCompletedTask(ref List<System.Exception> exceptions, Task t)
    {
      AggregateException exceptions1 = t.GetExceptions(true);
      if (exceptions1 == null)
        return;
      t.UpdateExceptionObservedStatus();
      if (exceptions == null)
        exceptions = new List<System.Exception>(exceptions1.InnerExceptionCount);
      exceptions.AddRange((IEnumerable<System.Exception>) exceptions1.InternalInnerExceptions);
    }


    #nullable enable
    /// <summary>Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <returns>The index of the completed <see cref="T:System.Threading.Tasks.Task" /> object in the <paramref name="tasks" /> array.</returns>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(params Task[] tasks) => Task.WaitAnyCore(tasks, -1, new CancellationToken());

    /// <summary>Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution within a specified time interval.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="P:System.TimeSpan.TotalMilliseconds" /> property of the <paramref name="timeout" /> argument is a negative number other than -1, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// The <see cref="P:System.TimeSpan.TotalMilliseconds" /> property of the <paramref name="timeout" /> argument is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <returns>The index of the completed task in the <paramref name="tasks" /> array argument, or -1 if the timeout occurred.</returns>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(Task[] tasks, TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.timeout);
      return Task.WaitAnyCore(tasks, (int) totalMilliseconds, new CancellationToken());
    }

    /// <summary>Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution unless the wait is cancelled.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="cancellationToken">A <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> to observe while waiting for a task to complete.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <paramref name="cancellationToken" /> was canceled.</exception>
    /// <returns>The index of the completed task in the <paramref name="tasks" /> array argument.</returns>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(Task[] tasks, CancellationToken cancellationToken) => Task.WaitAnyCore(tasks, -1, cancellationToken);

    /// <summary>Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution within a specified number of milliseconds.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <returns>The index of the completed task in the <paramref name="tasks" /> array argument, or -1 if the timeout occurred.</returns>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(Task[] tasks, int millisecondsTimeout) => Task.WaitAnyCore(tasks, millisecondsTimeout, new CancellationToken());

    /// <summary>Waits for any of the provided <see cref="T:System.Threading.Tasks.Task" /> objects to complete execution within a specified number of milliseconds or until a cancellation token is cancelled.</summary>
    /// <param name="tasks">An array of <see cref="T:System.Threading.Tasks.Task" /> instances on which to wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">A <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" /> to observe while waiting for a task to complete.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null element.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <paramref name="cancellationToken" /> was canceled.</exception>
    /// <returns>The index of the completed task in the <paramref name="tasks" /> array argument, or -1 if the timeout occurred.</returns>
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public static int WaitAny(
      Task[] tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      return Task.WaitAnyCore(tasks, millisecondsTimeout, cancellationToken);
    }


    #nullable disable
    private static int WaitAnyCore(
      Task[] tasks,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      if (millisecondsTimeout < -1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsTimeout);
      cancellationToken.ThrowIfCancellationRequested();
      int num = -1;
      for (int index = 0; index < tasks.Length; ++index)
      {
        Task task = tasks[index];
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_WaitMulti_NullTask, ExceptionArgument.tasks);
        if (num == -1 && task.IsCompleted)
          num = index;
      }
      if (num == -1 && tasks.Length != 0)
      {
        Task<Task> continuation = TaskFactory.CommonCWAnyLogic((IList<Task>) tasks, true);
        if (continuation.Wait(millisecondsTimeout, cancellationToken))
          num = Array.IndexOf<Task>(tasks, continuation.Result);
        else
          TaskFactory.CommonCWAnyLogicCleanup(continuation);
      }
      GC.KeepAlive((object) tasks);
      return num;
    }


    #nullable enable
    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that's completed successfully with the specified result.</summary>
    /// <param name="result">The result to store into the completed task.</param>
    /// <typeparam name="TResult">The type of the result returned by the task.</typeparam>
    /// <returns>The successfully completed task.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<TResult> FromResult<TResult>(TResult result)
    {
      if ((object) result == null)
        return Task<TResult>.s_defaultResultTask;
      if (typeof (TResult).IsValueType)
      {
        if (typeof (TResult) == typeof (bool))
          return Unsafe.As<Task<TResult>>((bool) (object) result ? (object) TaskCache.s_trueTask : (object) TaskCache.s_falseTask);
        if (typeof (TResult) == typeof (int))
        {
          int num = (int) (object) result;
          if ((uint) (num - -1) < 10U)
            return Unsafe.As<Task<TResult>>((object) TaskCache.s_int32Tasks[num - -1]);
        }
        else if (typeof (TResult) == typeof (uint) && (uint) (object) result == 0U || typeof (TResult) == typeof (byte) && (byte) (object) result == (byte) 0 || typeof (TResult) == typeof (sbyte) && (sbyte) (object) result == (sbyte) 0 || typeof (TResult) == typeof (char) && (char) (object) result == char.MinValue || typeof (TResult) == typeof (long) && (long) (object) result == 0L || typeof (TResult) == typeof (ulong) && (ulong) (object) result == 0UL || typeof (TResult) == typeof (short) && (short) (object) result == (short) 0 || typeof (TResult) == typeof (ushort) && (ushort) (object) result == (ushort) 0 || typeof (TResult) == typeof (IntPtr) && IntPtr.Zero == (IntPtr) (object) result || typeof (TResult) == typeof (UIntPtr) && UIntPtr.Zero == (UIntPtr) (object) result)
          return Task<TResult>.s_defaultResultTask;
      }
      return new Task<TResult>(result);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that has completed with a specified exception.</summary>
    /// <param name="exception">The exception with which to complete the task.</param>
    /// <returns>The faulted task.</returns>
    public static Task FromException(System.Exception exception)
    {
      if (exception == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exception);
      Task task = new Task();
      task.TrySetException((object) exception);
      return task;
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that's completed with a specified exception.</summary>
    /// <param name="exception">The exception with which to complete the task.</param>
    /// <typeparam name="TResult">The type of the result returned by the task.</typeparam>
    /// <returns>The faulted task.</returns>
    public static Task<TResult> FromException<TResult>(System.Exception exception)
    {
      if (exception == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.exception);
      Task<TResult> task = new Task<TResult>();
      task.TrySetException((object) exception);
      return task;
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task" /> that's completed due to cancellation with a specified cancellation token.</summary>
    /// <param name="cancellationToken">The cancellation token with which to complete the task.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Cancellation has not been requested for <paramref name="cancellationToken" />; its <see cref="P:System.Threading.CancellationToken.IsCancellationRequested" /> property is <see langword="false" />.</exception>
    /// <returns>The canceled task.</returns>
    public static Task FromCanceled(CancellationToken cancellationToken)
    {
      if (!cancellationToken.IsCancellationRequested)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.cancellationToken);
      return new Task(true, TaskCreationOptions.None, cancellationToken);
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.Task`1" /> that's completed due to cancellation with a specified cancellation token.</summary>
    /// <param name="cancellationToken">The cancellation token with which to complete the task.</param>
    /// <typeparam name="TResult">The type of the result returned by the task.</typeparam>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Cancellation has not been requested for <paramref name="cancellationToken" />; its <see cref="P:System.Threading.CancellationToken.IsCancellationRequested" /> property is <see langword="false" />.</exception>
    /// <returns>The canceled task.</returns>
    public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
    {
      if (!cancellationToken.IsCancellationRequested)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.cancellationToken);
      return new Task<TResult>(true, default (TResult), TaskCreationOptions.None, cancellationToken);
    }


    #nullable disable
    internal static Task FromCanceled(OperationCanceledException exception)
    {
      Task task = new Task();
      task.TrySetCanceled(exception.CancellationToken, (object) exception);
      return task;
    }

    internal static Task<TResult> FromCanceled<TResult>(OperationCanceledException exception)
    {
      Task<TResult> task = new Task<TResult>();
      task.TrySetCanceled(exception.CancellationToken, (object) exception);
      return task;
    }


    #nullable enable
    /// <summary>Queues the specified work to run on the thread pool and returns a <see cref="T:System.Threading.Tasks.Task" /> object that represents that work.</summary>
    /// <param name="action">The work to execute asynchronously.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> parameter was <see langword="null" />.</exception>
    /// <returns>A task that represents the work queued to execute in the ThreadPool.</returns>
    public static Task Run(Action action) => Task.InternalStartNew((Task) null, (Delegate) action, (object) null, new CancellationToken(), TaskScheduler.Default, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None);

    /// <summary>Queues the specified work to run on the thread pool and returns a <see cref="T:System.Threading.Tasks.Task" /> object that represents that work. A cancellation token allows the work to be cancelled if it has not yet started.</summary>
    /// <param name="action">The work to execute asynchronously.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work if it has not yet started. <see cref="M:System.Threading.Tasks.Task.Run(System.Action,System.Threading.CancellationToken)" /> does not pass <paramref name="cancellationToken" /> to <paramref name="action" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> parameter was <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with <paramref name="cancellationToken" /> was disposed.</exception>
    /// <returns>A task that represents the work queued to execute in the thread pool.</returns>
    public static Task Run(Action action, CancellationToken cancellationToken) => Task.InternalStartNew((Task) null, (Delegate) action, (object) null, cancellationToken, TaskScheduler.Default, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None);

    /// <summary>Queues the specified work to run on the thread pool and returns a <see cref="T:System.Threading.Tasks.Task`1" /> object that represents that work. A cancellation token allows the work to be cancelled if it has not yet started.</summary>
    /// <param name="function">The work to execute asynchronously.</param>
    /// <typeparam name="TResult">The return type of the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> parameter is <see langword="null" />.</exception>
    /// <returns>A task object that represents the work queued to execute in the thread pool.</returns>
    public static Task<TResult> Run<TResult>(Func<TResult> function) => Task<TResult>.StartNew((Task) null, function, new CancellationToken(), TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, TaskScheduler.Default);

    /// <summary>Queues the specified work to run on the thread pool and returns a <see langword="Task(TResult)" /> object that represents that work.</summary>
    /// <param name="function">The work to execute asynchronously.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work if it has not yet started. <see cref="M:System.Threading.Tasks.Task.Run``1(System.Func{``0},System.Threading.CancellationToken)" /> does not pass <paramref name="cancellationToken" /> to <paramref name="action" />.</param>
    /// <typeparam name="TResult">The result type of the task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with <paramref name="cancellationToken" /> was disposed.</exception>
    /// <returns>A <see langword="Task(TResult)" /> that represents the work queued to execute in the thread pool.</returns>
    public static Task<TResult> Run<TResult>(
      Func<TResult> function,
      CancellationToken cancellationToken)
    {
      return Task<TResult>.StartNew((Task) null, function, cancellationToken, TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None, TaskScheduler.Default);
    }

    /// <summary>Queues the specified work to run on the thread pool and returns a proxy for the  task returned by <paramref name="function" />.</summary>
    /// <param name="function">The work to execute asynchronously.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> parameter was <see langword="null" />.</exception>
    /// <returns>A task that represents a proxy for the task returned by <paramref name="function" />.</returns>
    public static Task Run(Func<Task?> function) => Task.Run(function, new CancellationToken());

    /// <summary>Queues the specified work to run on the thread pool and returns a proxy for the task returned by <paramref name="function" />. A cancellation token allows the work to be cancelled if it has not yet started.</summary>
    /// <param name="function">The work to execute asynchronously.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work if it has not yet started. <see cref="M:System.Threading.Tasks.Task.Run(System.Func{System.Threading.Tasks.Task},System.Threading.CancellationToken)" /> does not pass <paramref name="cancellationToken" /> to <paramref name="action" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> parameter was <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with <paramref name="cancellationToken" /> was disposed.</exception>
    /// <returns>A task that represents a proxy for the task returned by <paramref name="function" />.</returns>
    public static Task Run(Func<Task?> function, CancellationToken cancellationToken)
    {
      if (function == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.function);
      return cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) : (Task) new UnwrapPromise<VoidTaskResult>((Task) Task<Task>.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default), true);
    }

    /// <summary>Queues the specified work to run on the thread pool and returns a proxy for the <see langword="Task(TResult)" /> returned by <paramref name="function" />. A cancellation token allows the work to be cancelled if it has not yet started.</summary>
    /// <param name="function">The work to execute asynchronously.</param>
    /// <typeparam name="TResult">The type of the result returned by the proxy task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> parameter was <see langword="null" />.</exception>
    /// <returns>A <see langword="Task(TResult)" /> that represents a proxy for the <see langword="Task(TResult)" /> returned by <paramref name="function" />.</returns>
    public static Task<TResult> Run<TResult>(Func<Task<TResult>?> function) => Task.Run<TResult>(function, new CancellationToken());

    /// <summary>Queues the specified work to run on the thread pool and returns a proxy for the <see langword="Task(TResult)" /> returned by <paramref name="function" />.</summary>
    /// <param name="function">The work to execute asynchronously.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work if it has not yet started. <see cref="M:System.Threading.Tasks.Task.Run``1(System.Func{System.Threading.Tasks.Task{``0}},System.Threading.CancellationToken)" /> does not pass <paramref name="cancellationToken" /> to <paramref name="action" />.</param>
    /// <typeparam name="TResult">The type of the result returned by the proxy task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> parameter was <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with <paramref name="cancellationToken" /> was disposed.</exception>
    /// <returns>A <see langword="Task(TResult)" /> that represents a proxy for the <see langword="Task(TResult)" /> returned by <paramref name="function" />.</returns>
    public static Task<TResult> Run<TResult>(
      Func<Task<TResult>?> function,
      CancellationToken cancellationToken)
    {
      if (function == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.function);
      return cancellationToken.IsCancellationRequested ? Task.FromCanceled<TResult>(cancellationToken) : (Task<TResult>) new UnwrapPromise<TResult>((Task) Task<Task<TResult>>.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default), true);
    }

    /// <summary>Creates a task that completes after a specified time interval.</summary>
    /// <param name="delay">The time span to wait before completing the returned task, or <see langword="TimeSpan.FromMilliseconds(-1)" /> to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="delay" /> represents a negative time interval other than <see langword="TimeSpan.FromMilliseconds(-1)" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="delay" /> argument's <see cref="P:System.TimeSpan.TotalMilliseconds" /> property is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A task that represents the time delay.</returns>
    public static Task Delay(TimeSpan delay) => Task.Delay(delay, new CancellationToken());

    /// <summary>Creates a cancellable task that completes after a specified time interval.</summary>
    /// <param name="delay">The time span to wait before completing the returned task, or <see langword="TimeSpan.FromMilliseconds(-1)" /> to wait indefinitely.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="delay" /> represents a negative time interval other than <see langword="TimeSpan.FromMilliseconds(-1)" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="delay" /> argument's <see cref="P:System.TimeSpan.TotalMilliseconds" /> property is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>A task that represents the time delay.</returns>
    public static Task Delay(TimeSpan delay, CancellationToken cancellationToken) => Task.Delay(Task.ValidateTimeout(delay, ExceptionArgument.delay), cancellationToken);

    /// <summary>Creates a task that completes after a specified number of milliseconds.</summary>
    /// <param name="millisecondsDelay">The number of milliseconds to wait before completing the returned task, or -1 to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsDelay" /> argument is less than -1.</exception>
    /// <returns>A task that represents the time delay.</returns>
    public static Task Delay(int millisecondsDelay) => Task.Delay(millisecondsDelay, new CancellationToken());

    /// <summary>Creates a cancellable task that completes after a specified number of milliseconds.</summary>
    /// <param name="millisecondsDelay">The number of milliseconds to wait before completing the returned task, or -1 to wait indefinitely.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsDelay" /> argument is less than -1.</exception>
    /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>A task that represents the time delay.</returns>
    public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
    {
      if (millisecondsDelay < -1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsDelay, ExceptionResource.Task_Delay_InvalidMillisecondsDelay);
      return Task.Delay((uint) millisecondsDelay, cancellationToken);
    }


    #nullable disable
    private static Task Delay(uint millisecondsDelay, CancellationToken cancellationToken)
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (millisecondsDelay == 0U)
        return Task.CompletedTask;
      return !cancellationToken.CanBeCanceled ? (Task) new Task.DelayPromise(millisecondsDelay) : (Task) new Task.DelayPromiseWithCancellation(millisecondsDelay, cancellationToken);
    }

    internal static uint ValidateTimeout(TimeSpan timeout, ExceptionArgument argument)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > 4294967294L)
        ThrowHelper.ThrowArgumentOutOfRangeException(argument, ExceptionResource.Task_InvalidTimerTimeSpan);
      return (uint) totalMilliseconds;
    }


    #nullable enable
    /// <summary>Creates a task that will complete when all of the <see cref="T:System.Threading.Tasks.Task" /> objects in an enumerable collection have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> collection contained a <see langword="null" /> task.</exception>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static Task WhenAll(IEnumerable<Task> tasks)
    {
      if (tasks is ICollection<Task> tasks2)
      {
        if (tasks is Task[] taskArray)
          return Task.WhenAll(taskArray);
        int num = 0;
        Task[] tasks1 = new Task[tasks2.Count];
        foreach (Task task in tasks)
        {
          if (task == null)
            ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
          tasks1[num++] = task;
        }
        return Task.InternalWhenAll(tasks1);
      }
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      List<Task> taskList = new List<Task>();
      foreach (Task task in tasks)
      {
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
        taskList.Add(task);
      }
      return Task.InternalWhenAll(taskList.ToArray());
    }

    /// <summary>Creates a task that will complete when all of the <see cref="T:System.Threading.Tasks.Task" /> objects in an array have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contained a <see langword="null" /> task.</exception>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static Task WhenAll(params Task[] tasks)
    {
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      int length = tasks.Length;
      if (length == 0)
        return Task.InternalWhenAll(tasks);
      Task[] tasks1 = new Task[length];
      for (int index = 0; index < length; ++index)
      {
        Task task = tasks[index];
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
        tasks1[index] = task;
      }
      return Task.InternalWhenAll(tasks1);
    }


    #nullable disable
    private static Task InternalWhenAll(Task[] tasks) => tasks.Length != 0 ? (Task) new Task.WhenAllPromise(tasks) : Task.CompletedTask;


    #nullable enable
    /// <summary>Creates a task that will complete when all of the <see cref="T:System.Threading.Tasks.Task`1" /> objects in an enumerable collection have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> collection contained a <see langword="null" /> task.</exception>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
    {
      switch (tasks)
      {
        case Task<TResult>[] taskArray:
          return Task.WhenAll<TResult>(taskArray);
        case ICollection<Task<TResult>> tasks2:
          int num = 0;
          Task<TResult>[] tasks1 = new Task<TResult>[tasks2.Count];
          foreach (Task<TResult> task in tasks)
          {
            if (task == null)
              ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
            tasks1[num++] = task;
          }
          return Task.InternalWhenAll<TResult>(tasks1);
        case null:
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
          break;
      }
      List<Task<TResult>> taskList = new List<Task<TResult>>();
      foreach (Task<TResult> task in tasks)
      {
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
        taskList.Add(task);
      }
      return Task.InternalWhenAll<TResult>(taskList.ToArray());
    }

    /// <summary>Creates a task that will complete when all of the <see cref="T:System.Threading.Tasks.Task`1" /> objects in an array have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contained a <see langword="null" /> task.</exception>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
    {
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      int length = tasks.Length;
      if (length == 0)
        return Task.InternalWhenAll<TResult>(tasks);
      Task<TResult>[] tasks1 = new Task<TResult>[length];
      for (int index = 0; index < length; ++index)
      {
        Task<TResult> task = tasks[index];
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
        tasks1[index] = task;
      }
      return Task.InternalWhenAll<TResult>(tasks1);
    }


    #nullable disable
    private static Task<TResult[]> InternalWhenAll<TResult>(Task<TResult>[] tasks) => tasks.Length != 0 ? (Task<TResult[]>) new Task.WhenAllPromise<TResult>(tasks) : new Task<TResult[]>(false, Array.Empty<TResult>(), TaskCreationOptions.None, new CancellationToken());


    #nullable enable
    /// <summary>Creates a task that will complete when any of the supplied tasks have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was null.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contained a null task, or was empty.</exception>
    /// <returns>A task that represents the completion of one of the supplied tasks.  The return task's Result is the task that completed.</returns>
    public static Task<Task> WhenAny(params Task[] tasks)
    {
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      if (tasks.Length == 2)
        return Task.WhenAny(tasks[0], tasks[1]);
      if (tasks.Length == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList, ExceptionArgument.tasks);
      int length = tasks.Length;
      Task[] tasks1 = new Task[length];
      for (int index = 0; index < length; ++index)
      {
        Task task = tasks[index];
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
        tasks1[index] = task;
      }
      return TaskFactory.CommonCWAnyLogic((IList<Task>) tasks1);
    }

    /// <summary>Creates a task that will complete when either of the supplied tasks have completed.</summary>
    /// <param name="task1">The first task to wait on for completion.</param>
    /// <param name="task2">The second task to wait on for completion.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="task1" /> or <paramref name="task2" /> was <see langword="null" />.</exception>
    /// <returns>A new task that represents the completion of one of the supplied tasks. Its <see langword="Result" /> is the task that completed first.</returns>
    public static Task<Task> WhenAny(Task task1, Task task2)
    {
      if (task1 == null || task2 == null)
        throw new ArgumentNullException(task1 == null ? nameof (task1) : nameof (task2));
      if (task1.IsCompleted)
        return Task.FromResult<Task>(task1);
      return !task2.IsCompleted ? (Task<Task>) new Task.TwoTaskWhenAnyPromise<Task>(task1, task2) : Task.FromResult<Task>(task2);
    }

    /// <summary>Creates a task that will complete when any of the supplied tasks have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contained a null task, or was empty.</exception>
    /// <returns>A task that represents the completion of one of the supplied tasks.  The return task's Result is the task that completed.</returns>
    public static Task<Task> WhenAny(IEnumerable<Task> tasks)
    {
      if (tasks is ICollection<Task> tasks2)
      {
        if (tasks is Task[] taskArray)
          return Task.WhenAny(taskArray);
        int count = tasks2.Count;
        if (count <= 0)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList, ExceptionArgument.tasks);
        int num = 0;
        Task[] tasks1 = new Task[count];
        foreach (Task task in tasks)
        {
          if (task == null)
            ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
          tasks1[num++] = task;
        }
        return TaskFactory.CommonCWAnyLogic((IList<Task>) tasks1);
      }
      if (tasks == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.tasks);
      List<Task> tasks3 = new List<Task>();
      foreach (Task task in tasks)
      {
        if (task == null)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_NullTask, ExceptionArgument.tasks);
        tasks3.Add(task);
      }
      if (tasks3.Count == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList, ExceptionArgument.tasks);
      return TaskFactory.CommonCWAnyLogic((IList<Task>) tasks3);
    }

    /// <summary>Creates a task that will complete when any of the supplied tasks have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was null.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contained a null task, or was empty.</exception>
    /// <returns>A task that represents the completion of one of the supplied tasks.  The return task's Result is the task that completed.</returns>
    public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks) => tasks != null && tasks.Length == 2 ? Task.WhenAny<TResult>(tasks[0], tasks[1]) : Task.WhenAny((Task[]) tasks).ContinueWith<Task<TResult>>(Task<TResult>.TaskWhenAnyCast.Value, new CancellationToken(), TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

    /// <summary>Creates a task that will complete when either of the supplied tasks have completed.</summary>
    /// <param name="task1">The first task to wait on for completion.</param>
    /// <param name="task2">The second task to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the result of the returned task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="task1" /> or <paramref name="task2" /> was <see langword="null" />.</exception>
    /// <returns>A task that represents the completion of one of the supplied tasks. The returned task's <typeparamref name="TResult" /> is the task that completed first.</returns>
    public static Task<Task<TResult>> WhenAny<TResult>(
      Task<TResult> task1,
      Task<TResult> task2)
    {
      if (task1 == null || task2 == null)
        throw new ArgumentNullException(task1 == null ? nameof (task1) : nameof (task2));
      if (task1.IsCompleted)
        return Task.FromResult<Task<TResult>>(task1);
      return !task2.IsCompleted ? (Task<Task<TResult>>) new Task.TwoTaskWhenAnyPromise<Task<TResult>>(task1, task2) : Task.FromResult<Task<TResult>>(task2);
    }

    /// <summary>Creates a task that will complete when any of the supplied tasks have completed.</summary>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> array contained a null task, or was empty.</exception>
    /// <returns>A task that represents the completion of one of the supplied tasks.  The return task's Result is the task that completed.</returns>
    public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks) => Task.WhenAny((IEnumerable<Task>) tasks).ContinueWith<Task<TResult>>(Task<TResult>.TaskWhenAnyCast.Value, new CancellationToken(), TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);


    #nullable disable
    internal static Task<TResult> CreateUnwrapPromise<TResult>(Task outerTask, bool lookForOce) => (Task<TResult>) new UnwrapPromise<TResult>(outerTask, lookForOce);

    internal virtual Delegate[] GetDelegateContinuationsForDebugger() => this.m_continuationObject != this ? Task.GetDelegatesFromContinuationObject(this.m_continuationObject) : (Delegate[]) null;

    private static Delegate[] GetDelegatesFromContinuationObject(object continuationObject)
    {
      if (continuationObject != null)
      {
        if (continuationObject is Action action)
          return new Delegate[1]
          {
            (Delegate) AsyncMethodBuilderCore.TryGetStateMachineForDebugger(action)
          };
        if (continuationObject is TaskContinuation taskContinuation)
          return taskContinuation.GetDelegateContinuationsForDebugger();
        if (continuationObject is Task task)
        {
          Delegate[] continuationsForDebugger = task.GetDelegateContinuationsForDebugger();
          if (continuationsForDebugger != null)
            return continuationsForDebugger;
        }
        if (continuationObject is ITaskCompletionAction completionAction)
          return new Delegate[1]
          {
            (Delegate) new Action<Task>(completionAction.Invoke)
          };
        if (continuationObject is List<object> objectList)
        {
          List<Delegate> delegateList = new List<Delegate>();
          foreach (object continuationObject1 in objectList)
          {
            Delegate[] continuationObject2 = Task.GetDelegatesFromContinuationObject(continuationObject1);
            if (continuationObject2 != null)
            {
              foreach (Delegate @delegate in continuationObject2)
              {
                if ((object) @delegate != null)
                  delegateList.Add(@delegate);
              }
            }
          }
          return delegateList.ToArray();
        }
      }
      return (Delegate[]) null;
    }

    private static Task GetActiveTaskFromId(int taskId)
    {
      Task activeTaskFromId = (Task) null;
      Task.s_currentActiveTasks?.TryGetValue(taskId, out activeTaskFromId);
      return activeTaskFromId;
    }

    [Flags]
    internal enum TaskStateFlags
    {
      Started = 65536, // 0x00010000
      DelegateInvoked = 131072, // 0x00020000
      Disposed = 262144, // 0x00040000
      ExceptionObservedByParent = 524288, // 0x00080000
      CancellationAcknowledged = 1048576, // 0x00100000
      Faulted = 2097152, // 0x00200000
      Canceled = 4194304, // 0x00400000
      WaitingOnChildren = 8388608, // 0x00800000
      RanToCompletion = 16777216, // 0x01000000
      WaitingForActivation = 33554432, // 0x02000000
      CompletionReserved = 67108864, // 0x04000000
      WaitCompletionNotification = 268435456, // 0x10000000
      ExecutionContextIsNull = 536870912, // 0x20000000
      TaskScheduledWasFired = 1073741824, // 0x40000000
      CompletedMask = RanToCompletion | Canceled | Faulted, // 0x01600000
      OptionsMask = 65535, // 0x0000FFFF
    }

    internal sealed class ContingentProperties
    {
      internal ExecutionContext m_capturedContext;
      internal volatile ManualResetEventSlim m_completionEvent;
      internal volatile TaskExceptionHolder m_exceptionsHolder;
      internal CancellationToken m_cancellationToken;
      internal StrongBox<CancellationTokenRegistration> m_cancellationRegistration;
      internal volatile int m_internalCancellationRequested;
      internal volatile int m_completionCountdown = 1;
      internal volatile List<Task> m_exceptionalChildren;
      internal Task m_parent;

      internal void SetCompleted() => this.m_completionEvent?.Set();

      internal void UnregisterCancellationCallback()
      {
        if (this.m_cancellationRegistration == null)
          return;
        try
        {
          this.m_cancellationRegistration.Value.Dispose();
        }
        catch (ObjectDisposedException ex)
        {
        }
        this.m_cancellationRegistration = (StrongBox<CancellationTokenRegistration>) null;
      }
    }

    private protected sealed class CancellationPromise<TResult> : 
      Task<TResult>,
      ITaskCompletionAction
    {
      private readonly Task _task;
      private readonly CancellationTokenRegistration _registration;
      private readonly TimerQueueTimer _timer;

      internal CancellationPromise(Task source, uint millisecondsDelay, CancellationToken token)
      {
        this._task = source;
        source.AddCompletionAction((ITaskCompletionAction) this);
        if (millisecondsDelay != uint.MaxValue)
          this._timer = new TimerQueueTimer((TimerCallback) (state =>
          {
            Task.CancellationPromise<TResult> cancellationPromise = (Task.CancellationPromise<TResult>) state;
            if (!cancellationPromise.TrySetException((object) new TimeoutException()))
              return;
            cancellationPromise.Cleanup();
          }), (object) this, millisecondsDelay, uint.MaxValue, false);
        this._registration = token.UnsafeRegister((Action<object, CancellationToken>) ((state, cancellationToken) =>
        {
          Task.CancellationPromise<TResult> cancellationPromise = (Task.CancellationPromise<TResult>) state;
          if (!cancellationPromise.TrySetCanceled(cancellationToken))
            return;
          cancellationPromise.Cleanup();
        }), (object) this);
        if (!this.IsCompleted)
          return;
        this.Cleanup();
      }

      bool ITaskCompletionAction.InvokeMayRunArbitraryCode => true;

      void ITaskCompletionAction.Invoke(Task completingTask)
      {
        bool flag;
        switch (completingTask.Status)
        {
          case TaskStatus.Canceled:
            flag = this.TrySetCanceled(completingTask.CancellationToken, (object) completingTask.GetCancellationExceptionDispatchInfo());
            break;
          case TaskStatus.Faulted:
            flag = this.TrySetException((object) completingTask.GetExceptionDispatchInfos());
            break;
          default:
            flag = completingTask is Task<TResult> task ? this.TrySetResult(task.Result) : this.TrySetResult();
            break;
        }
        if (!flag)
          return;
        this.Cleanup();
      }

      private void Cleanup()
      {
        this._registration.Dispose();
        this._timer?.Close();
        this._task.RemoveContinuation((object) this);
      }
    }

    private sealed class SetOnInvokeMres : ManualResetEventSlim, ITaskCompletionAction
    {
      internal SetOnInvokeMres()
        : base(false, 0)
      {
      }

      public void Invoke(Task completingTask) => this.Set();

      public bool InvokeMayRunArbitraryCode => false;
    }

    private sealed class SetOnCountdownMres : ManualResetEventSlim, ITaskCompletionAction
    {
      private int _count;

      internal SetOnCountdownMres(int count) => this._count = count;

      public void Invoke(Task completingTask)
      {
        if (Interlocked.Decrement(ref this._count) != 0)
          return;
        this.Set();
      }

      public bool InvokeMayRunArbitraryCode => false;
    }

    private class DelayPromise : Task
    {
      private static readonly TimerCallback s_timerCallback = new TimerCallback(Task.DelayPromise.TimerCallback);
      private readonly TimerQueueTimer _timer;

      internal DelayPromise(uint millisecondsDelay)
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationBegin(this.Id, "Task.Delay", 0L);
        if (Task.s_asyncDebuggingEnabled)
          Task.AddToActiveTasks((Task) this);
        if (millisecondsDelay == uint.MaxValue)
          return;
        this._timer = new TimerQueueTimer(Task.DelayPromise.s_timerCallback, (object) this, millisecondsDelay, uint.MaxValue, false);
        if (!this.IsCompleted)
          return;
        this._timer.Close();
      }

      private static void TimerCallback(object state) => ((Task.DelayPromise) state).CompleteTimedOut();

      private void CompleteTimedOut()
      {
        if (!this.TrySetResult())
          return;
        this.Cleanup();
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) this);
        if (!TplEventSource.Log.IsEnabled())
          return;
        TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
      }

      protected virtual void Cleanup() => this._timer?.Close();
    }

    private sealed class DelayPromiseWithCancellation : Task.DelayPromise
    {
      private readonly CancellationTokenRegistration _registration;

      internal DelayPromiseWithCancellation(uint millisecondsDelay, CancellationToken token)
        : base(millisecondsDelay)
      {
        this._registration = token.UnsafeRegister((Action<object, CancellationToken>) ((state, cancellationToken) =>
        {
          Task.DelayPromiseWithCancellation withCancellation = (Task.DelayPromiseWithCancellation) state;
          if (!withCancellation.TrySetCanceled(cancellationToken))
            return;
          withCancellation.Cleanup();
        }), (object) this);
        if (!this.IsCompleted)
          return;
        this._registration.Dispose();
      }

      protected override void Cleanup()
      {
        this._registration.Dispose();
        base.Cleanup();
      }
    }

    private sealed class WhenAllPromise : Task, ITaskCompletionAction
    {
      private readonly Task[] m_tasks;
      private int m_count;

      internal WhenAllPromise(Task[] tasks)
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationBegin(this.Id, "Task.WhenAll", 0L);
        if (Task.s_asyncDebuggingEnabled)
          Task.AddToActiveTasks((Task) this);
        this.m_tasks = tasks;
        this.m_count = tasks.Length;
        foreach (Task task in tasks)
        {
          if (task.IsCompleted)
            this.Invoke(task);
          else
            task.AddCompletionAction((ITaskCompletionAction) this);
        }
      }

      public void Invoke(Task completedTask)
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationRelation(this.Id, CausalityRelation.Join);
        if (Interlocked.Decrement(ref this.m_count) != 0)
          return;
        List<ExceptionDispatchInfo> exceptionObject = (List<ExceptionDispatchInfo>) null;
        Task task1 = (Task) null;
        for (int index = 0; index < this.m_tasks.Length; ++index)
        {
          Task task2 = this.m_tasks[index];
          if (task2.IsFaulted)
          {
            if (exceptionObject == null)
              exceptionObject = new List<ExceptionDispatchInfo>();
            exceptionObject.AddRange((IEnumerable<ExceptionDispatchInfo>) task2.GetExceptionDispatchInfos());
          }
          else if (task2.IsCanceled && task1 == null)
            task1 = task2;
          if (task2.IsWaitNotificationEnabled)
            this.SetNotificationForWaitCompletion(true);
          else
            this.m_tasks[index] = (Task) null;
        }
        if (exceptionObject != null)
          this.TrySetException((object) exceptionObject);
        else if (task1 != null)
        {
          this.TrySetCanceled(task1.CancellationToken, (object) task1.GetCancellationExceptionDispatchInfo());
        }
        else
        {
          if (TplEventSource.Log.IsEnabled())
            TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
          if (Task.s_asyncDebuggingEnabled)
            Task.RemoveFromActiveTasks((Task) this);
          this.TrySetResult();
        }
      }

      public bool InvokeMayRunArbitraryCode => true;

      private protected override bool ShouldNotifyDebuggerOfWaitCompletion => base.ShouldNotifyDebuggerOfWaitCompletion && Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion(this.m_tasks);
    }

    private sealed class WhenAllPromise<T> : Task<T[]>, ITaskCompletionAction
    {
      private readonly Task<T>[] m_tasks;
      private int m_count;

      internal WhenAllPromise(Task<T>[] tasks)
      {
        this.m_tasks = tasks;
        this.m_count = tasks.Length;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationBegin(this.Id, "Task.WhenAll", 0L);
        if (Task.s_asyncDebuggingEnabled)
          Task.AddToActiveTasks((Task) this);
        foreach (Task<T> task in tasks)
        {
          if (task.IsCompleted)
            this.Invoke((Task) task);
          else
            task.AddCompletionAction((ITaskCompletionAction) this);
        }
      }

      public void Invoke(Task ignored)
      {
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationRelation(this.Id, CausalityRelation.Join);
        if (Interlocked.Decrement(ref this.m_count) != 0)
          return;
        T[] result = new T[this.m_tasks.Length];
        List<ExceptionDispatchInfo> exceptionObject = (List<ExceptionDispatchInfo>) null;
        Task task1 = (Task) null;
        for (int index = 0; index < this.m_tasks.Length; ++index)
        {
          Task<T> task2 = this.m_tasks[index];
          if (task2.IsFaulted)
          {
            if (exceptionObject == null)
              exceptionObject = new List<ExceptionDispatchInfo>();
            exceptionObject.AddRange((IEnumerable<ExceptionDispatchInfo>) task2.GetExceptionDispatchInfos());
          }
          else if (task2.IsCanceled)
          {
            if (task1 == null)
              task1 = (Task) task2;
          }
          else
            result[index] = task2.GetResultCore(false);
          if (task2.IsWaitNotificationEnabled)
            this.SetNotificationForWaitCompletion(true);
          else
            this.m_tasks[index] = (Task<T>) null;
        }
        if (exceptionObject != null)
          this.TrySetException((object) exceptionObject);
        else if (task1 != null)
        {
          this.TrySetCanceled(task1.CancellationToken, (object) task1.GetCancellationExceptionDispatchInfo());
        }
        else
        {
          if (TplEventSource.Log.IsEnabled())
            TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
          if (Task.s_asyncDebuggingEnabled)
            Task.RemoveFromActiveTasks((Task) this);
          this.TrySetResult(result);
        }
      }

      public bool InvokeMayRunArbitraryCode => true;

      private protected override bool ShouldNotifyDebuggerOfWaitCompletion => base.ShouldNotifyDebuggerOfWaitCompletion && Task.AnyTaskRequiresNotifyDebuggerOfWaitCompletion((Task[]) this.m_tasks);
    }

    private sealed class TwoTaskWhenAnyPromise<TTask> : Task<TTask>, ITaskCompletionAction
      where TTask : Task
    {
      private TTask _task1;
      private TTask _task2;

      public TwoTaskWhenAnyPromise(TTask task1, TTask task2)
      {
        this._task1 = task1;
        this._task2 = task2;
        if (TplEventSource.Log.IsEnabled())
          TplEventSource.Log.TraceOperationBegin(this.Id, "Task.WhenAny", 0L);
        if (Task.s_asyncDebuggingEnabled)
          Task.AddToActiveTasks((Task) this);
        task1.AddCompletionAction((ITaskCompletionAction) this, false);
        task2.AddCompletionAction((ITaskCompletionAction) this, false);
        if (!task1.IsCompleted)
          return;
        task2.RemoveContinuation((object) this);
      }

      public void Invoke(Task completingTask)
      {
        ref TTask local = ref this._task1;
        TTask task1 = default (TTask);
        Task task2;
        if ((__Boxed<TTask>) (task2 = (Task) Interlocked.Exchange<TTask>(ref local, task1)) == null)
          return;
        Task task2_1 = (Task) this._task2;
        this._task2 = default (TTask);
        if (TplEventSource.Log.IsEnabled())
        {
          TplEventSource.Log.TraceOperationRelation(this.Id, CausalityRelation.Choice);
          TplEventSource.Log.TraceOperationEnd(this.Id, AsyncCausalityStatus.Completed);
        }
        if (Task.s_asyncDebuggingEnabled)
          Task.RemoveFromActiveTasks((Task) this);
        if (!task2.IsCompleted)
          task2.RemoveContinuation((object) this);
        else
          task2_1.RemoveContinuation((object) this);
        this.TrySetResult((TTask) completingTask);
      }

      public bool InvokeMayRunArbitraryCode => true;
    }
  }
}
