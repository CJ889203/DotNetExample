// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.ConcurrentExclusiveSchedulerPair
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides task schedulers that coordinate to execute tasks while ensuring that concurrent tasks may run concurrently and exclusive tasks never do.</summary>
  [DebuggerDisplay("Concurrent={ConcurrentTaskCountForDebugger}, Exclusive={ExclusiveTaskCountForDebugger}, Mode={ModeForDebugger}")]
  [DebuggerTypeProxy(typeof (ConcurrentExclusiveSchedulerPair.DebugView))]
  public class ConcurrentExclusiveSchedulerPair
  {

    #nullable disable
    private readonly ThreadLocal<ConcurrentExclusiveSchedulerPair.ProcessingMode> m_threadProcessingMode = new ThreadLocal<ConcurrentExclusiveSchedulerPair.ProcessingMode>();
    private readonly ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler m_concurrentTaskScheduler;
    private readonly ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler m_exclusiveTaskScheduler;
    private readonly TaskScheduler m_underlyingTaskScheduler;
    private readonly int m_maxConcurrencyLevel;
    private readonly int m_maxItemsPerTask;
    private int m_processingCount;
    private ConcurrentExclusiveSchedulerPair.CompletionState m_completionState;
    private ConcurrentExclusiveSchedulerPair.SchedulerWorkItem m_threadPoolWorkItem;

    private static int DefaultMaxConcurrencyLevel => Environment.ProcessorCount;


    #nullable enable
    private object ValueLock => (object) this.m_threadProcessingMode;

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class.</summary>
    public ConcurrentExclusiveSchedulerPair()
      : this(TaskScheduler.Default, ConcurrentExclusiveSchedulerPair.DefaultMaxConcurrencyLevel, -1)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class that targets the specified scheduler.</summary>
    /// <param name="taskScheduler">The target scheduler on which this pair should execute.</param>
    public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler)
      : this(taskScheduler, ConcurrentExclusiveSchedulerPair.DefaultMaxConcurrencyLevel, -1)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class that targets the specified scheduler with a maximum concurrency level.</summary>
    /// <param name="taskScheduler">The target scheduler on which this pair should execute.</param>
    /// <param name="maxConcurrencyLevel">The maximum number of tasks to run concurrently.</param>
    public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler, int maxConcurrencyLevel)
      : this(taskScheduler, maxConcurrencyLevel, -1)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.ConcurrentExclusiveSchedulerPair" /> class that targets the specified scheduler with a maximum concurrency level and a maximum number of scheduled tasks that may be processed as a unit.</summary>
    /// <param name="taskScheduler">The target scheduler on which this pair should execute.</param>
    /// <param name="maxConcurrencyLevel">The maximum number of tasks to run concurrently.</param>
    /// <param name="maxItemsPerTask">The maximum number of tasks to process for each underlying scheduled task used by the pair.</param>
    public ConcurrentExclusiveSchedulerPair(
      TaskScheduler taskScheduler,
      int maxConcurrencyLevel,
      int maxItemsPerTask)
    {
      if (taskScheduler == null)
        throw new ArgumentNullException(nameof (taskScheduler));
      if (maxConcurrencyLevel == 0 || maxConcurrencyLevel < -1)
        throw new ArgumentOutOfRangeException(nameof (maxConcurrencyLevel));
      if (maxItemsPerTask == 0 || maxItemsPerTask < -1)
        throw new ArgumentOutOfRangeException(nameof (maxItemsPerTask));
      this.m_underlyingTaskScheduler = taskScheduler;
      this.m_maxConcurrencyLevel = maxConcurrencyLevel;
      this.m_maxItemsPerTask = maxItemsPerTask;
      int concurrencyLevel = taskScheduler.MaximumConcurrencyLevel;
      if (concurrencyLevel > 0 && concurrencyLevel < this.m_maxConcurrencyLevel)
        this.m_maxConcurrencyLevel = concurrencyLevel;
      if (this.m_maxConcurrencyLevel == -1)
        this.m_maxConcurrencyLevel = int.MaxValue;
      if (this.m_maxItemsPerTask == -1)
        this.m_maxItemsPerTask = int.MaxValue;
      this.m_exclusiveTaskScheduler = new ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler(this, 1, ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask);
      this.m_concurrentTaskScheduler = new ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler(this, this.m_maxConcurrencyLevel, ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingConcurrentTasks);
    }

    /// <summary>Informs the scheduler pair that it should not accept any more tasks.</summary>
    public void Complete()
    {
      lock (this.ValueLock)
      {
        if (this.CompletionRequested)
          return;
        this.RequestCompletion();
        this.CleanupStateIfCompletingAndQuiesced();
      }
    }

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.Task" /> that will complete when the scheduler has completed processing.</summary>
    /// <returns>The asynchronous operation that will complete when the scheduler finishes processing.</returns>
    public Task Completion => (Task) this.EnsureCompletionStateInitialized();


    #nullable disable
    private ConcurrentExclusiveSchedulerPair.CompletionState EnsureCompletionStateInitialized()
    {
      return Volatile.Read<ConcurrentExclusiveSchedulerPair.CompletionState>(ref this.m_completionState) ?? InitializeCompletionState();

      ConcurrentExclusiveSchedulerPair.CompletionState InitializeCompletionState()
      {
        Interlocked.CompareExchange<ConcurrentExclusiveSchedulerPair.CompletionState>(ref this.m_completionState, new ConcurrentExclusiveSchedulerPair.CompletionState(), (ConcurrentExclusiveSchedulerPair.CompletionState) null);
        return this.m_completionState;
      }
    }

    private bool CompletionRequested => this.m_completionState != null && Volatile.Read(ref this.m_completionState.m_completionRequested);

    private void RequestCompletion() => this.EnsureCompletionStateInitialized().m_completionRequested = true;

    private void CleanupStateIfCompletingAndQuiesced()
    {
      if (!this.ReadyToComplete)
        return;
      this.CompleteTaskAsync();
    }

    private bool ReadyToComplete
    {
      get
      {
        if (!this.CompletionRequested || this.m_processingCount != 0)
          return false;
        ConcurrentExclusiveSchedulerPair.CompletionState completionState = this.EnsureCompletionStateInitialized();
        if (completionState.m_exceptions != null && completionState.m_exceptions.Count > 0)
          return true;
        return this.m_concurrentTaskScheduler.m_tasks.IsEmpty && this.m_exclusiveTaskScheduler.m_tasks.IsEmpty;
      }
    }

    private void CompleteTaskAsync()
    {
      ConcurrentExclusiveSchedulerPair.CompletionState completionState = this.EnsureCompletionStateInitialized();
      if (completionState.m_completionQueued)
        return;
      completionState.m_completionQueued = true;
      ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
      {
        ConcurrentExclusiveSchedulerPair exclusiveSchedulerPair = (ConcurrentExclusiveSchedulerPair) state;
        List<Exception> exceptions = exclusiveSchedulerPair.m_completionState.m_exceptions;
        bool flag = exceptions == null || exceptions.Count <= 0 ? exclusiveSchedulerPair.m_completionState.TrySetResult() : exclusiveSchedulerPair.m_completionState.TrySetException((object) exceptions);
        exclusiveSchedulerPair.m_threadProcessingMode.Dispose();
      }), (object) this);
    }

    private void FaultWithTask(Task faultedTask)
    {
      AggregateException exception = faultedTask.Exception;
      ConcurrentExclusiveSchedulerPair.CompletionState completionState1 = this.EnsureCompletionStateInitialized();
      ConcurrentExclusiveSchedulerPair.CompletionState completionState2 = completionState1;
      if (completionState2.m_exceptions == null)
        completionState2.m_exceptions = new List<Exception>(exception.InnerExceptionCount);
      completionState1.m_exceptions.AddRange((IEnumerable<Exception>) exception.InternalInnerExceptions);
      this.RequestCompletion();
    }


    #nullable enable
    /// <summary>Gets a <see cref="T:System.Threading.Tasks.TaskScheduler" /> that can be used to schedule tasks to this pair that may run concurrently with other tasks on this pair.</summary>
    /// <returns>An object that can be used to schedule tasks concurrently.</returns>
    public TaskScheduler ConcurrentScheduler => (TaskScheduler) this.m_concurrentTaskScheduler;

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.TaskScheduler" /> that can be used to schedule tasks to this pair that must run exclusively with regards to other tasks on this pair.</summary>
    /// <returns>An object that can be used to schedule tasks that do not run concurrently with other tasks.</returns>
    public TaskScheduler ExclusiveScheduler => (TaskScheduler) this.m_exclusiveTaskScheduler;

    private int ConcurrentTaskCountForDebugger => this.m_concurrentTaskScheduler.m_tasks.Count;

    private int ExclusiveTaskCountForDebugger => this.m_exclusiveTaskScheduler.m_tasks.Count;

    private void ProcessAsyncIfNecessary(bool fairly = false)
    {
      if (this.m_processingCount < 0)
        return;
      bool flag = !this.m_exclusiveTaskScheduler.m_tasks.IsEmpty;
      Task task = (Task) null;
      if (this.m_processingCount == 0 & flag)
      {
        this.m_processingCount = -1;
        if (!this.TryQueueThreadPoolWorkItem(fairly))
        {
          try
          {
            task = new Task((Action<object>) (thisPair => ((ConcurrentExclusiveSchedulerPair) thisPair).ProcessExclusiveTasks()), (object) this, new CancellationToken(), ConcurrentExclusiveSchedulerPair.GetCreationOptionsForTask(fairly));
            task.Start(this.m_underlyingTaskScheduler);
          }
          catch (Exception ex)
          {
            this.m_processingCount = 0;
            this.FaultWithTask(task ?? Task.FromException(ex));
          }
        }
      }
      else
      {
        int count = this.m_concurrentTaskScheduler.m_tasks.Count;
        if (count > 0 && !flag && this.m_processingCount < this.m_maxConcurrencyLevel)
        {
          for (int index = 0; index < count && this.m_processingCount < this.m_maxConcurrencyLevel; ++index)
          {
            ++this.m_processingCount;
            if (!this.TryQueueThreadPoolWorkItem(fairly))
            {
              try
              {
                task = new Task((Action<object>) (thisPair => ((ConcurrentExclusiveSchedulerPair) thisPair).ProcessConcurrentTasks()), (object) this, new CancellationToken(), ConcurrentExclusiveSchedulerPair.GetCreationOptionsForTask(fairly));
                task.Start(this.m_underlyingTaskScheduler);
              }
              catch (Exception ex)
              {
                --this.m_processingCount;
                this.FaultWithTask(task ?? Task.FromException(ex));
              }
            }
          }
        }
      }
      this.CleanupStateIfCompletingAndQuiesced();
    }

    private bool TryQueueThreadPoolWorkItem(bool fairly)
    {
      if (TaskScheduler.Default != this.m_underlyingTaskScheduler)
        return false;
      ThreadPool.UnsafeQueueUserWorkItemInternal((object) (this.m_threadPoolWorkItem ?? (this.m_threadPoolWorkItem = new ConcurrentExclusiveSchedulerPair.SchedulerWorkItem(this))), !fairly);
      return true;
    }

    private void ProcessExclusiveTasks()
    {
      try
      {
        this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask;
        Task result;
        for (int index = 0; index < this.m_maxItemsPerTask && this.m_exclusiveTaskScheduler.m_tasks.TryDequeue(out result); ++index)
        {
          if (!result.IsFaulted)
            this.m_exclusiveTaskScheduler.ExecuteTask(result);
        }
      }
      finally
      {
        this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.NotCurrentlyProcessing;
        lock (this.ValueLock)
        {
          this.m_processingCount = 0;
          this.ProcessAsyncIfNecessary(true);
        }
      }
    }

    private void ProcessConcurrentTasks()
    {
      try
      {
        this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingConcurrentTasks;
        Task result;
        for (int index = 0; index < this.m_maxItemsPerTask && this.m_concurrentTaskScheduler.m_tasks.TryDequeue(out result); ++index)
        {
          if (!result.IsFaulted)
            this.m_concurrentTaskScheduler.ExecuteTask(result);
          if (!this.m_exclusiveTaskScheduler.m_tasks.IsEmpty)
            break;
        }
      }
      finally
      {
        this.m_threadProcessingMode.Value = ConcurrentExclusiveSchedulerPair.ProcessingMode.NotCurrentlyProcessing;
        lock (this.ValueLock)
        {
          if (this.m_processingCount > 0)
            --this.m_processingCount;
          this.ProcessAsyncIfNecessary(true);
        }
      }
    }

    private ConcurrentExclusiveSchedulerPair.ProcessingMode ModeForDebugger
    {
      get
      {
        if (this.m_completionState != null && this.m_completionState.IsCompleted)
          return ConcurrentExclusiveSchedulerPair.ProcessingMode.Completed;
        ConcurrentExclusiveSchedulerPair.ProcessingMode modeForDebugger = ConcurrentExclusiveSchedulerPair.ProcessingMode.NotCurrentlyProcessing;
        if (this.m_processingCount == -1)
          modeForDebugger |= ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask;
        if (this.m_processingCount >= 1)
          modeForDebugger |= ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingConcurrentTasks;
        if (this.CompletionRequested)
          modeForDebugger |= ConcurrentExclusiveSchedulerPair.ProcessingMode.Completing;
        return modeForDebugger;
      }
    }

    internal static TaskCreationOptions GetCreationOptionsForTask(
      bool isReplacementReplica = false)
    {
      TaskCreationOptions creationOptionsForTask = TaskCreationOptions.DenyChildAttach;
      if (isReplacementReplica)
        creationOptionsForTask |= TaskCreationOptions.PreferFairness;
      return creationOptionsForTask;
    }


    #nullable disable
    private sealed class CompletionState : Task
    {
      internal bool m_completionRequested;
      internal bool m_completionQueued;
      internal List<Exception> m_exceptions;
    }

    private sealed class SchedulerWorkItem : IThreadPoolWorkItem
    {
      private readonly ConcurrentExclusiveSchedulerPair _pair;

      internal SchedulerWorkItem(ConcurrentExclusiveSchedulerPair pair) => this._pair = pair;

      void IThreadPoolWorkItem.Execute()
      {
        if (this._pair.m_processingCount == -1)
          this._pair.ProcessExclusiveTasks();
        else
          this._pair.ProcessConcurrentTasks();
      }
    }

    [DebuggerDisplay("Count={CountForDebugger}, MaxConcurrencyLevel={m_maxConcurrencyLevel}, Id={Id}")]
    [DebuggerTypeProxy(typeof (ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler.DebugView))]
    private sealed class ConcurrentExclusiveTaskScheduler : TaskScheduler
    {
      private readonly ConcurrentExclusiveSchedulerPair m_pair;
      private readonly int m_maxConcurrencyLevel;
      private readonly ConcurrentExclusiveSchedulerPair.ProcessingMode m_processingMode;
      internal readonly IProducerConsumerQueue<Task> m_tasks;

      internal ConcurrentExclusiveTaskScheduler(
        ConcurrentExclusiveSchedulerPair pair,
        int maxConcurrencyLevel,
        ConcurrentExclusiveSchedulerPair.ProcessingMode processingMode)
      {
        this.m_pair = pair;
        this.m_maxConcurrencyLevel = maxConcurrencyLevel;
        this.m_processingMode = processingMode;
        this.m_tasks = processingMode == ConcurrentExclusiveSchedulerPair.ProcessingMode.ProcessingExclusiveTask ? (IProducerConsumerQueue<Task>) new SingleProducerSingleConsumerQueue<Task>() : (IProducerConsumerQueue<Task>) new MultiProducerMultiConsumerQueue<Task>();
      }

      public override int MaximumConcurrencyLevel => this.m_maxConcurrencyLevel;

      protected internal override void QueueTask(Task task)
      {
        lock (this.m_pair.ValueLock)
        {
          if (this.m_pair.CompletionRequested)
            throw new InvalidOperationException(this.GetType().ToString());
          this.m_tasks.Enqueue(task);
          this.m_pair.ProcessAsyncIfNecessary();
        }
      }

      internal void ExecuteTask(Task task) => this.TryExecuteTask(task);

      protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
      {
        if (!taskWasPreviouslyQueued && this.m_pair.CompletionRequested)
          return false;
        bool flag = this.m_pair.m_underlyingTaskScheduler == TaskScheduler.Default;
        if (flag & taskWasPreviouslyQueued && !Thread.CurrentThread.IsThreadPoolThread || this.m_pair.m_threadProcessingMode.Value != this.m_processingMode)
          return false;
        return !flag || taskWasPreviouslyQueued ? this.TryExecuteTaskInlineOnTargetScheduler(task) : this.TryExecuteTask(task);
      }

      private bool TryExecuteTaskInlineOnTargetScheduler(Task task)
      {
        Task<bool> task1 = new Task<bool>((Func<object, bool>) (s =>
        {
          TupleSlim<ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler, Task> tupleSlim = (TupleSlim<ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler, Task>) s;
          return tupleSlim.Item1.TryExecuteTask(tupleSlim.Item2);
        }), (object) new TupleSlim<ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler, Task>(this, task));
        try
        {
          task1.RunSynchronously(this.m_pair.m_underlyingTaskScheduler);
          return task1.Result;
        }
        catch
        {
          AggregateException exception = task1.Exception;
          throw;
        }
        finally
        {
          task1.Dispose();
        }
      }

      protected override IEnumerable<Task> GetScheduledTasks() => (IEnumerable<Task>) this.m_tasks;

      private int CountForDebugger => this.m_tasks.Count;

      private sealed class DebugView
      {
        private readonly ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler m_taskScheduler;

        public DebugView(
          ConcurrentExclusiveSchedulerPair.ConcurrentExclusiveTaskScheduler scheduler)
        {
          this.m_taskScheduler = scheduler;
        }

        public int MaximumConcurrencyLevel => this.m_taskScheduler.m_maxConcurrencyLevel;

        public IEnumerable<Task> ScheduledTasks => (IEnumerable<Task>) this.m_taskScheduler.m_tasks;

        public ConcurrentExclusiveSchedulerPair SchedulerPair => this.m_taskScheduler.m_pair;
      }
    }

    private sealed class DebugView
    {
      private readonly ConcurrentExclusiveSchedulerPair m_pair;

      public DebugView(ConcurrentExclusiveSchedulerPair pair) => this.m_pair = pair;

      public ConcurrentExclusiveSchedulerPair.ProcessingMode Mode => this.m_pair.ModeForDebugger;

      public IEnumerable<Task> ScheduledExclusive => (IEnumerable<Task>) this.m_pair.m_exclusiveTaskScheduler.m_tasks;

      public IEnumerable<Task> ScheduledConcurrent => (IEnumerable<Task>) this.m_pair.m_concurrentTaskScheduler.m_tasks;

      public int CurrentlyExecutingTaskCount => this.m_pair.m_processingCount != -1 ? this.m_pair.m_processingCount : 1;

      public TaskScheduler TargetScheduler => this.m_pair.m_underlyingTaskScheduler;
    }

    [Flags]
    private enum ProcessingMode : byte
    {
      NotCurrentlyProcessing = 0,
      ProcessingExclusiveTask = 1,
      ProcessingConcurrentTasks = 2,
      Completing = 4,
      Completed = 8,
    }
  }
}
