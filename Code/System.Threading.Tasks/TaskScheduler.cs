// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskScheduler
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Represents an object that handles the low-level work of queuing tasks onto threads.</summary>
  [DebuggerDisplay("Id={Id}")]
  [DebuggerTypeProxy(typeof (TaskScheduler.SystemThreadingTasks_TaskSchedulerDebugView))]
  public abstract class TaskScheduler
  {

    #nullable disable
    private static ConditionalWeakTable<TaskScheduler, object> s_activeTaskSchedulers;
    private static readonly TaskScheduler s_defaultTaskScheduler = (TaskScheduler) new ThreadPoolTaskScheduler();
    internal static int s_taskSchedulerIdCounter;
    private volatile int m_taskSchedulerId;


    #nullable enable
    /// <summary>Queues a <see cref="T:System.Threading.Tasks.Task" /> to the scheduler.</summary>
    /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be queued.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
    protected internal abstract void QueueTask(Task task);

    /// <summary>Determines whether the provided <see cref="T:System.Threading.Tasks.Task" /> can be executed synchronously in this call, and if it can, executes it.</summary>
    /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be executed.</param>
    /// <param name="taskWasPreviouslyQueued">A Boolean denoting whether or not task has previously been queued. If this parameter is True, then the task may have been previously queued (scheduled); if False, then the task is known not to have been queued, and this call is being made in order to execute the task inline without queuing it.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="task" /> was already executed.</exception>
    /// <returns>A Boolean value indicating whether the task was executed inline.</returns>
    protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);

    /// <summary>For debugger support only, generates an enumerable of <see cref="T:System.Threading.Tasks.Task" /> instances currently queued to the scheduler waiting to be executed.</summary>
    /// <exception cref="T:System.NotSupportedException">This scheduler is unable to generate a list of queued tasks at this time.</exception>
    /// <returns>An enumerable that allows a debugger to traverse the tasks currently queued to this scheduler.</returns>
    protected abstract IEnumerable<Task>? GetScheduledTasks();

    /// <summary>Indicates the maximum concurrency level this <see cref="T:System.Threading.Tasks.TaskScheduler" /> is able to support.</summary>
    /// <returns>Returns an integer that represents the maximum concurrency level. The default scheduler returns <see cref="F:System.Int32.MaxValue" />.</returns>
    public virtual int MaximumConcurrencyLevel => int.MaxValue;


    #nullable disable
    internal bool TryRunInline(Task task, bool taskWasPreviouslyQueued)
    {
      TaskScheduler executingTaskScheduler = task.ExecutingTaskScheduler;
      if (executingTaskScheduler != this && executingTaskScheduler != null)
        return executingTaskScheduler.TryRunInline(task, taskWasPreviouslyQueued);
      if (executingTaskScheduler == null || (object) task.m_action == null || task.IsDelegateInvoked || task.IsCanceled || !RuntimeHelpers.TryEnsureSufficientExecutionStack())
        return false;
      if (TplEventSource.Log.IsEnabled())
        task.FireTaskScheduledIfNeeded(this);
      bool flag = this.TryExecuteTaskInline(task, taskWasPreviouslyQueued);
      if (flag && !task.IsDelegateInvoked && !task.IsCanceled)
        throw new InvalidOperationException(SR.TaskScheduler_InconsistentStateAfterTryExecuteTaskInline);
      return flag;
    }


    #nullable enable
    /// <summary>Attempts to dequeue a <see cref="T:System.Threading.Tasks.Task" /> that was previously queued to this scheduler.</summary>
    /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be dequeued.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="task" /> argument is null.</exception>
    /// <returns>A Boolean denoting whether the <paramref name="task" /> argument was successfully dequeued.</returns>
    protected internal virtual bool TryDequeue(Task task) => false;

    internal virtual void NotifyWorkItemProgress()
    {
    }


    #nullable disable
    internal void InternalQueueTask(Task task)
    {
      if (TplEventSource.Log.IsEnabled())
        task.FireTaskScheduledIfNeeded(this);
      this.QueueTask(task);
    }

    /// <summary>Initializes the <see cref="T:System.Threading.Tasks.TaskScheduler" />.</summary>
    protected TaskScheduler()
    {
      if (!Debugger.IsAttached)
        return;
      this.AddToActiveTaskSchedulers();
    }

    private void AddToActiveTaskSchedulers()
    {
      ConditionalWeakTable<TaskScheduler, object> activeTaskSchedulers = TaskScheduler.s_activeTaskSchedulers;
      if (activeTaskSchedulers == null)
      {
        Interlocked.CompareExchange<ConditionalWeakTable<TaskScheduler, object>>(ref TaskScheduler.s_activeTaskSchedulers, new ConditionalWeakTable<TaskScheduler, object>(), (ConditionalWeakTable<TaskScheduler, object>) null);
        activeTaskSchedulers = TaskScheduler.s_activeTaskSchedulers;
      }
      activeTaskSchedulers.Add(this, (object) null);
    }


    #nullable enable
    /// <summary>Gets the default <see cref="T:System.Threading.Tasks.TaskScheduler" /> instance that is provided by .NET.</summary>
    /// <returns>Returns the default <see cref="T:System.Threading.Tasks.TaskScheduler" /> instance.</returns>
    public static TaskScheduler Default => TaskScheduler.s_defaultTaskScheduler;

    /// <summary>Gets the <see cref="T:System.Threading.Tasks.TaskScheduler" /> associated with the currently executing task.</summary>
    /// <returns>Returns the <see cref="T:System.Threading.Tasks.TaskScheduler" /> associated with the currently executing task.</returns>
    public static TaskScheduler Current => TaskScheduler.InternalCurrent ?? TaskScheduler.Default;

    internal static TaskScheduler? InternalCurrent
    {
      get
      {
        Task internalCurrent = Task.InternalCurrent;
        return internalCurrent == null || (internalCurrent.CreationOptions & TaskCreationOptions.HideScheduler) != TaskCreationOptions.None ? (TaskScheduler) null : internalCurrent.ExecutingTaskScheduler;
      }
    }

    /// <summary>Creates a <see cref="T:System.Threading.Tasks.TaskScheduler" /> associated with the current <see cref="T:System.Threading.SynchronizationContext" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The current SynchronizationContext may not be used as a TaskScheduler.</exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.TaskScheduler" /> associated with the current <see cref="T:System.Threading.SynchronizationContext" />, as determined by <see cref="P:System.Threading.SynchronizationContext.Current" />.</returns>
    public static TaskScheduler FromCurrentSynchronizationContext() => (TaskScheduler) new SynchronizationContextTaskScheduler();

    /// <summary>Gets the unique ID for this <see cref="T:System.Threading.Tasks.TaskScheduler" />.</summary>
    /// <returns>Returns the unique ID for this <see cref="T:System.Threading.Tasks.TaskScheduler" />.</returns>
    public int Id
    {
      get
      {
        if (this.m_taskSchedulerId == 0)
        {
          int num;
          do
          {
            num = Interlocked.Increment(ref TaskScheduler.s_taskSchedulerIdCounter);
          }
          while (num == 0);
          Interlocked.CompareExchange(ref this.m_taskSchedulerId, num, 0);
        }
        return this.m_taskSchedulerId;
      }
    }

    /// <summary>Attempts to execute the provided <see cref="T:System.Threading.Tasks.Task" /> on this scheduler.</summary>
    /// <param name="task">A <see cref="T:System.Threading.Tasks.Task" /> object to be executed.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="task" /> is not associated with this scheduler.</exception>
    /// <returns>A Boolean that is true if <paramref name="task" /> was successfully executed, false if it was not. A common reason for execution failure is that the task had previously been executed or is in the process of being executed by another thread.</returns>
    protected bool TryExecuteTask(Task task) => task.ExecutingTaskScheduler == this ? task.ExecuteEntry() : throw new InvalidOperationException(SR.TaskScheduler_ExecuteTask_WrongTaskScheduler);

    /// <summary>Occurs when a faulted task's unobserved exception is about to trigger exception escalation policy, which, by default, would terminate the process.</summary>
    public static event EventHandler<UnobservedTaskExceptionEventArgs>? UnobservedTaskException;


    #nullable disable
    internal static void PublishUnobservedTaskException(
      object sender,
      UnobservedTaskExceptionEventArgs ueea)
    {
      EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskException = TaskScheduler.UnobservedTaskException;
      if (unobservedTaskException == null)
        return;
      unobservedTaskException(sender, ueea);
    }

    internal Task[] GetScheduledTasksForDebugger()
    {
      IEnumerable<Task> scheduledTasks = this.GetScheduledTasks();
      if (scheduledTasks == null)
        return (Task[]) null;
      if (!(scheduledTasks is Task[] tasksForDebugger))
        tasksForDebugger = new List<Task>(scheduledTasks).ToArray();
      foreach (Task task in tasksForDebugger)
      {
        int id = task.Id;
      }
      return tasksForDebugger;
    }

    internal static TaskScheduler[] GetTaskSchedulersForDebugger()
    {
      if (TaskScheduler.s_activeTaskSchedulers == null)
        return new TaskScheduler[1]
        {
          TaskScheduler.s_defaultTaskScheduler
        };
      List<TaskScheduler> taskSchedulerList = new List<TaskScheduler>();
      foreach (KeyValuePair<TaskScheduler, object> activeTaskScheduler in (IEnumerable<KeyValuePair<TaskScheduler, object>>) TaskScheduler.s_activeTaskSchedulers)
        taskSchedulerList.Add(activeTaskScheduler.Key);
      if (!taskSchedulerList.Contains(TaskScheduler.s_defaultTaskScheduler))
        taskSchedulerList.Add(TaskScheduler.s_defaultTaskScheduler);
      TaskScheduler[] array = taskSchedulerList.ToArray();
      foreach (TaskScheduler taskScheduler in array)
      {
        int id = taskScheduler.Id;
      }
      return array;
    }

    internal TaskScheduler.TaskSchedulerAwaiter GetAwaiter() => new TaskScheduler.TaskSchedulerAwaiter(this);

    internal sealed class SystemThreadingTasks_TaskSchedulerDebugView
    {
      private readonly TaskScheduler m_taskScheduler;

      public SystemThreadingTasks_TaskSchedulerDebugView(TaskScheduler scheduler) => this.m_taskScheduler = scheduler;

      public int Id => this.m_taskScheduler.Id;

      public IEnumerable<Task> ScheduledTasks => this.m_taskScheduler.GetScheduledTasks();
    }

    internal readonly struct TaskSchedulerAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
      private readonly TaskScheduler _scheduler;

      public TaskSchedulerAwaiter(TaskScheduler scheduler) => this._scheduler = scheduler;

      public bool IsCompleted => false;

      public void GetResult()
      {
      }

      public void OnCompleted(Action continuation) => Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.DenyChildAttach, this._scheduler);

      public void UnsafeOnCompleted(Action continuation)
      {
        if (this._scheduler == TaskScheduler.Default)
          ThreadPool.UnsafeQueueUserWorkItem<Action>((Action<Action>) (s => s()), continuation, true);
        else
          this.OnCompleted(continuation);
      }
    }
  }
}
