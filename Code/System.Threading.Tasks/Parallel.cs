// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.Parallel
// Assembly: System.Threading.Tasks.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: CD664842-108A-425B-971D-793D618C3E3A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.Tasks.Parallel.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Tasks.Parallel.xml

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Provides support for parallel loops and regions.</summary>
  public static class Parallel
  {
    internal static int s_forkJoinContextID;

    #nullable disable
    internal static readonly ParallelOptions s_defaultParallelOptions = new ParallelOptions();


    #nullable enable
    /// <summary>Executes each of the provided actions, possibly in parallel.</summary>
    /// <param name="actions">An array of <see cref="T:System.Action" /> to execute.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="actions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that is thrown when any action in the <paramref name="actions" /> array throws an exception.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="actions" /> array contains a <see langword="null" /> element.</exception>
    public static void Invoke(params Action[] actions) => Parallel.Invoke(Parallel.s_defaultParallelOptions, actions);

    /// <summary>Executes each of the provided actions, possibly in parallel, unless the operation is cancelled by the user.</summary>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="actions">An array of actions to execute.</param>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> is set.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="actions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that is thrown when any action in the <paramref name="actions" /> array throws an exception.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="actions" /> array contains a <see langword="null" /> element.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    public static void Invoke(ParallelOptions parallelOptions, params Action[] actions)
    {
      if (actions == null)
        throw new ArgumentNullException(nameof (actions));
      CancellationToken cancellationToken = parallelOptions != null ? parallelOptions.CancellationToken : throw new ArgumentNullException(nameof (parallelOptions));
      cancellationToken.ThrowIfCancellationRequested();
      Action[] actionsCopy = new Action[actions.Length];
      for (int index = 0; index < actionsCopy.Length; ++index)
      {
        actionsCopy[index] = actions[index];
        if (actionsCopy[index] == null)
          throw new ArgumentException(SR.Parallel_Invoke_ActionNull);
      }
      int ForkJoinContextID = 0;
      if (ParallelEtwProvider.Log.IsEnabled())
      {
        ForkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
        ParallelEtwProvider.Log.ParallelInvokeBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), ForkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelInvoke, actionsCopy.Length);
      }
      if (actionsCopy.Length < 1)
        return;
      try
      {
        if (OperatingSystem.IsBrowser() || actionsCopy.Length > 10 || parallelOptions.MaxDegreeOfParallelism != -1 && parallelOptions.MaxDegreeOfParallelism < actionsCopy.Length)
        {
          ConcurrentQueue<Exception> exceptionQ = (ConcurrentQueue<Exception>) null;
          int actionIndex = 0;
          try
          {
            TaskReplicator.Run<object>((TaskReplicator.ReplicatableUserAction<object>) ((ref 
            #nullable disable
            object state, int timeout, out bool replicationDelegateYieldedBeforeCompletion) =>
            {
              replicationDelegateYieldedBeforeCompletion = false;
              for (int index = Interlocked.Increment(ref actionIndex); index <= actionsCopy.Length; index = Interlocked.Increment(ref actionIndex))
              {
                try
                {
                  actionsCopy[index - 1]();
                }
                catch (Exception ex)
                {
                  LazyInitializer.EnsureInitialized<ConcurrentQueue<Exception>>(ref exceptionQ, (Func<ConcurrentQueue<Exception>>) (() => new ConcurrentQueue<Exception>()));
                  exceptionQ.Enqueue(ex);
                }
                parallelOptions.CancellationToken.ThrowIfCancellationRequested();
              }
            }), parallelOptions, false);
          }
          catch (Exception ex)
          {
            LazyInitializer.EnsureInitialized<ConcurrentQueue<Exception>>(ref exceptionQ, (Func<ConcurrentQueue<Exception>>) (() => new ConcurrentQueue<Exception>()));
            switch (ex)
            {
              case ObjectDisposedException _:
                throw;
              case AggregateException aggregateException:
                using (IEnumerator<Exception> enumerator = aggregateException.InnerExceptions.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Exception current = enumerator.Current;
                    exceptionQ.Enqueue(current);
                  }
                  break;
                }
              default:
                exceptionQ.Enqueue(ex);
                break;
            }
          }
          if (exceptionQ == null || exceptionQ.IsEmpty)
            return;
          Parallel.ThrowSingleCancellationExceptionOrOtherException((ICollection) exceptionQ, parallelOptions.CancellationToken, (Exception) new AggregateException((IEnumerable<Exception>) exceptionQ));
        }
        else
        {
          Task[] taskArray = new Task[actionsCopy.Length];
          cancellationToken = parallelOptions.CancellationToken;
          cancellationToken.ThrowIfCancellationRequested();
          for (int index = 1; index < taskArray.Length; ++index)
            taskArray[index] = Task.Factory.StartNew(actionsCopy[index], parallelOptions.CancellationToken, TaskCreationOptions.None, parallelOptions.EffectiveTaskScheduler);
          taskArray[0] = new Task(actionsCopy[0], parallelOptions.CancellationToken, TaskCreationOptions.None);
          taskArray[0].RunSynchronously(parallelOptions.EffectiveTaskScheduler);
          try
          {
            Task.WaitAll(taskArray);
          }
          catch (AggregateException ex)
          {
            Parallel.ThrowSingleCancellationExceptionOrOtherException((ICollection) ex.InnerExceptions, parallelOptions.CancellationToken, (Exception) ex);
          }
        }
      }
      finally
      {
        if (ParallelEtwProvider.Log.IsEnabled())
          ParallelEtwProvider.Log.ParallelInvokeEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), ForkJoinContextID);
      }
    }


    #nullable enable
    /// <summary>Executes a <see langword="for" /> loop in which iterations may run in parallel.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      int fromInclusive,
      int toExclusive,
      Action<int> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForWorker<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, body, (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop with 64-bit indexes in which iterations may run in parallel.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      long fromInclusive,
      long toExclusive,
      Action<long> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForWorker64<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, body, (Action<long, ParallelLoopState>) null, (Func<long, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop in which iterations may run in parallel and loop options can be configured.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A  structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      int fromInclusive,
      int toExclusive,
      ParallelOptions parallelOptions,
      Action<int> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForWorker<object>(fromInclusive, toExclusive, parallelOptions, body, (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop with 64-bit indexes in which iterations may run in parallel and loop options can be configured.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      long fromInclusive,
      long toExclusive,
      ParallelOptions parallelOptions,
      Action<long> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, body, (Action<long, ParallelLoopState>) null, (Func<long, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop in which iterations may run in parallel and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A  structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      int fromInclusive,
      int toExclusive,
      Action<int, ParallelLoopState> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForWorker<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, (Action<int>) null, body, (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop with 64-bit indexes in which iterations may run in parallel and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A <see cref="T:System.Threading.Tasks.ParallelLoopResult" /> structure that contains information on what portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      long fromInclusive,
      long toExclusive,
      Action<long, ParallelLoopState> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForWorker64<object>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, (Action<long>) null, body, (Func<long, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      int fromInclusive,
      int toExclusive,
      ParallelOptions parallelOptions,
      Action<int, ParallelLoopState> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForWorker<object>(fromInclusive, toExclusive, parallelOptions, (Action<int>) null, body, (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop with 64-bit indexes in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For(
      long fromInclusive,
      long toExclusive,
      ParallelOptions parallelOptions,
      Action<long, ParallelLoopState> body)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForWorker64<object>(fromInclusive, toExclusive, parallelOptions, (Action<long>) null, body, (Func<long, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="for" /> loop with thread-local data in which iterations may run in parallel, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A  structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For<TLocal>(
      int fromInclusive,
      int toExclusive,
      Func<TLocal> localInit,
      Func<int, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      return Parallel.ForWorker<TLocal>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) null, body, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="for" /> loop with 64-bit indexes and thread-local data in which iterations may run in parallel, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For<TLocal>(
      long fromInclusive,
      long toExclusive,
      Func<TLocal> localInit,
      Func<long, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      return Parallel.ForWorker64<TLocal>(fromInclusive, toExclusive, Parallel.s_defaultParallelOptions, (Action<long>) null, (Action<long, ParallelLoopState>) null, body, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="for" /> loop with thread-local data in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For<TLocal>(
      int fromInclusive,
      int toExclusive,
      ParallelOptions parallelOptions,
      Func<TLocal> localInit,
      Func<int, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForWorker<TLocal>(fromInclusive, toExclusive, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) null, body, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="for" /> loop with 64-bit indexes and thread-local data in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="fromInclusive">The start index, inclusive.</param>
    /// <param name="toExclusive">The end index, exclusive.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each thread.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each thread.</param>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult For<TLocal>(
      long fromInclusive,
      long toExclusive,
      ParallelOptions parallelOptions,
      Func<TLocal> localInit,
      Func<long, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForWorker64<TLocal>(fromInclusive, toExclusive, parallelOptions, (Action<long>) null, (Action<long, ParallelLoopState>) null, body, localInit, localFinally);
    }

    private static bool CheckTimeoutReached(int timeoutOccursAt)
    {
      int tickCount = Environment.TickCount;
      return tickCount >= timeoutOccursAt && (0 <= timeoutOccursAt || 0 >= tickCount);
    }

    private static int ComputeTimeoutPoint(int timeoutLength) => Environment.TickCount + timeoutLength;


    #nullable disable
    private static ParallelLoopResult ForWorker<TLocal>(
      int fromInclusive,
      int toExclusive,
      ParallelOptions parallelOptions,
      Action<int> body,
      Action<int, ParallelLoopState> bodyWithState,
      Func<int, ParallelLoopState, TLocal, TLocal> bodyWithLocal,
      Func<TLocal> localInit,
      Action<TLocal> localFinally)
    {
      ParallelLoopResult parallelLoopResult = new ParallelLoopResult();
      if (toExclusive <= fromInclusive)
      {
        parallelLoopResult._completed = true;
        return parallelLoopResult;
      }
      ParallelLoopStateFlags32 sharedPStateFlags = new ParallelLoopStateFlags32();
      CancellationToken cancellationToken = parallelOptions.CancellationToken;
      cancellationToken.ThrowIfCancellationRequested();
      int nNumExpectedWorkers = parallelOptions.EffectiveMaxConcurrencyLevel == -1 ? Environment.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel;
      RangeManager rangeManager = new RangeManager((long) fromInclusive, (long) toExclusive, 1L, nNumExpectedWorkers);
      OperationCanceledException oce = (OperationCanceledException) null;
      cancellationToken = parallelOptions.CancellationToken;
      CancellationTokenRegistration tokenRegistration1;
      if (cancellationToken.CanBeCanceled)
      {
        cancellationToken = parallelOptions.CancellationToken;
        tokenRegistration1 = cancellationToken.UnsafeRegister((Action<object>) (o =>
        {
          oce = new OperationCanceledException(parallelOptions.CancellationToken);
          sharedPStateFlags.Cancel();
        }), (object) null);
      }
      else
        tokenRegistration1 = new CancellationTokenRegistration();
      CancellationTokenRegistration tokenRegistration2 = tokenRegistration1;
      int forkJoinContextID = 0;
      if (ParallelEtwProvider.Log.IsEnabled())
      {
        forkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
        ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelFor, (long) fromInclusive, (long) toExclusive);
      }
      try
      {
        try
        {
          TaskReplicator.Run<RangeWorker>((TaskReplicator.ReplicatableUserAction<RangeWorker>) ((ref RangeWorker currentWorker, int timeout, out bool replicationDelegateYieldedBeforeCompletion) =>
          {
            if (!currentWorker.IsInitialized)
              currentWorker = rangeManager.RegisterNewWorker();
            replicationDelegateYieldedBeforeCompletion = false;
            int nFromInclusiveLocal32;
            int nToExclusiveLocal32;
            if (!currentWorker.FindNewWork32(out nFromInclusiveLocal32, out nToExclusiveLocal32) || sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal32))
              return;
            if (ParallelEtwProvider.Log.IsEnabled())
              ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
            TLocal local = default (TLocal);
            bool flag = false;
            try
            {
              ParallelLoopState32 parallelLoopState32 = (ParallelLoopState32) null;
              if (bodyWithState != null)
                parallelLoopState32 = new ParallelLoopState32(sharedPStateFlags);
              else if (bodyWithLocal != null)
              {
                parallelLoopState32 = new ParallelLoopState32(sharedPStateFlags);
                if (localInit != null)
                {
                  local = localInit();
                  flag = true;
                }
              }
              int timeoutPoint = Parallel.ComputeTimeoutPoint(timeout);
              do
              {
                if (body != null)
                {
                  for (int index = nFromInclusiveLocal32; index < nToExclusiveLocal32 && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop()); ++index)
                    body(index);
                }
                else if (bodyWithState != null)
                {
                  for (int CallerIteration = nFromInclusiveLocal32; CallerIteration < nToExclusiveLocal32 && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(CallerIteration)); ++CallerIteration)
                  {
                    parallelLoopState32.CurrentIteration = CallerIteration;
                    bodyWithState(CallerIteration, (ParallelLoopState) parallelLoopState32);
                  }
                }
                else
                {
                  for (int CallerIteration = nFromInclusiveLocal32; CallerIteration < nToExclusiveLocal32 && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(CallerIteration)); ++CallerIteration)
                  {
                    parallelLoopState32.CurrentIteration = CallerIteration;
                    local = bodyWithLocal(CallerIteration, (ParallelLoopState) parallelLoopState32, local);
                  }
                }
                if (Parallel.CheckTimeoutReached(timeoutPoint))
                {
                  replicationDelegateYieldedBeforeCompletion = true;
                  break;
                }
              }
              while (currentWorker.FindNewWork32(out nFromInclusiveLocal32, out nToExclusiveLocal32) && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal32)));
            }
            catch (Exception ex)
            {
              sharedPStateFlags.SetExceptional();
              ExceptionDispatchInfo.Throw(ex);
            }
            finally
            {
              if (localFinally != null & flag)
                localFinally(local);
              if (ParallelEtwProvider.Log.IsEnabled())
                ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
            }
          }), parallelOptions, true);
        }
        finally
        {
          if (parallelOptions.CancellationToken.CanBeCanceled)
            tokenRegistration2.Dispose();
        }
        if (oce != null)
          throw oce;
      }
      catch (AggregateException ex)
      {
        Parallel.ThrowSingleCancellationExceptionOrOtherException((ICollection) ex.InnerExceptions, parallelOptions.CancellationToken, (Exception) ex);
      }
      finally
      {
        int loopStateFlags = sharedPStateFlags.LoopStateFlags;
        parallelLoopResult._completed = loopStateFlags == 0;
        if ((loopStateFlags & 2) != 0)
          parallelLoopResult._lowestBreakIteration = new long?((long) sharedPStateFlags.LowestBreakIteration);
        if (ParallelEtwProvider.Log.IsEnabled())
        {
          int TotalIterations = loopStateFlags != 0 ? ((loopStateFlags & 2) == 0 ? -1 : sharedPStateFlags.LowestBreakIteration - fromInclusive) : toExclusive - fromInclusive;
          ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, (long) TotalIterations);
        }
      }
      return parallelLoopResult;
    }

    private static ParallelLoopResult ForWorker64<TLocal>(
      long fromInclusive,
      long toExclusive,
      ParallelOptions parallelOptions,
      Action<long> body,
      Action<long, ParallelLoopState> bodyWithState,
      Func<long, ParallelLoopState, TLocal, TLocal> bodyWithLocal,
      Func<TLocal> localInit,
      Action<TLocal> localFinally)
    {
      ParallelLoopResult parallelLoopResult = new ParallelLoopResult();
      if (toExclusive <= fromInclusive)
      {
        parallelLoopResult._completed = true;
        return parallelLoopResult;
      }
      ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
      CancellationToken cancellationToken = parallelOptions.CancellationToken;
      cancellationToken.ThrowIfCancellationRequested();
      int nNumExpectedWorkers = parallelOptions.EffectiveMaxConcurrencyLevel == -1 ? Environment.ProcessorCount : parallelOptions.EffectiveMaxConcurrencyLevel;
      RangeManager rangeManager = new RangeManager(fromInclusive, toExclusive, 1L, nNumExpectedWorkers);
      OperationCanceledException oce = (OperationCanceledException) null;
      cancellationToken = parallelOptions.CancellationToken;
      CancellationTokenRegistration tokenRegistration1;
      if (cancellationToken.CanBeCanceled)
      {
        cancellationToken = parallelOptions.CancellationToken;
        tokenRegistration1 = cancellationToken.UnsafeRegister((Action<object>) (o =>
        {
          oce = new OperationCanceledException(parallelOptions.CancellationToken);
          sharedPStateFlags.Cancel();
        }), (object) null);
      }
      else
        tokenRegistration1 = new CancellationTokenRegistration();
      CancellationTokenRegistration tokenRegistration2 = tokenRegistration1;
      int forkJoinContextID = 0;
      if (ParallelEtwProvider.Log.IsEnabled())
      {
        forkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
        ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelFor, fromInclusive, toExclusive);
      }
      try
      {
        try
        {
          TaskReplicator.Run<RangeWorker>((TaskReplicator.ReplicatableUserAction<RangeWorker>) ((ref RangeWorker currentWorker, int timeout, out bool replicationDelegateYieldedBeforeCompletion) =>
          {
            if (!currentWorker.IsInitialized)
              currentWorker = rangeManager.RegisterNewWorker();
            replicationDelegateYieldedBeforeCompletion = false;
            long nFromInclusiveLocal;
            long nToExclusiveLocal;
            if (!currentWorker.FindNewWork(out nFromInclusiveLocal, out nToExclusiveLocal) || sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal))
              return;
            if (ParallelEtwProvider.Log.IsEnabled())
              ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
            TLocal local = default (TLocal);
            bool flag = false;
            try
            {
              ParallelLoopState64 parallelLoopState64 = (ParallelLoopState64) null;
              if (bodyWithState != null)
                parallelLoopState64 = new ParallelLoopState64(sharedPStateFlags);
              else if (bodyWithLocal != null)
              {
                parallelLoopState64 = new ParallelLoopState64(sharedPStateFlags);
                if (localInit != null)
                {
                  local = localInit();
                  flag = true;
                }
              }
              int timeoutPoint = Parallel.ComputeTimeoutPoint(timeout);
              do
              {
                if (body != null)
                {
                  for (long index = nFromInclusiveLocal; index < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop()); ++index)
                    body(index);
                }
                else if (bodyWithState != null)
                {
                  for (long CallerIteration = nFromInclusiveLocal; CallerIteration < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(CallerIteration)); ++CallerIteration)
                  {
                    parallelLoopState64.CurrentIteration = CallerIteration;
                    bodyWithState(CallerIteration, (ParallelLoopState) parallelLoopState64);
                  }
                }
                else
                {
                  for (long CallerIteration = nFromInclusiveLocal; CallerIteration < nToExclusiveLocal && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(CallerIteration)); ++CallerIteration)
                  {
                    parallelLoopState64.CurrentIteration = CallerIteration;
                    local = bodyWithLocal(CallerIteration, (ParallelLoopState) parallelLoopState64, local);
                  }
                }
                if (Parallel.CheckTimeoutReached(timeoutPoint))
                {
                  replicationDelegateYieldedBeforeCompletion = true;
                  break;
                }
              }
              while (currentWorker.FindNewWork(out nFromInclusiveLocal, out nToExclusiveLocal) && (sharedPStateFlags.LoopStateFlags == 0 || !sharedPStateFlags.ShouldExitLoop(nFromInclusiveLocal)));
            }
            catch (Exception ex)
            {
              sharedPStateFlags.SetExceptional();
              ExceptionDispatchInfo.Throw(ex);
            }
            finally
            {
              if (localFinally != null & flag)
                localFinally(local);
              if (ParallelEtwProvider.Log.IsEnabled())
                ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
            }
          }), parallelOptions, true);
        }
        finally
        {
          if (parallelOptions.CancellationToken.CanBeCanceled)
            tokenRegistration2.Dispose();
        }
        if (oce != null)
          throw oce;
      }
      catch (AggregateException ex)
      {
        Parallel.ThrowSingleCancellationExceptionOrOtherException((ICollection) ex.InnerExceptions, parallelOptions.CancellationToken, (Exception) ex);
      }
      finally
      {
        int loopStateFlags = sharedPStateFlags.LoopStateFlags;
        parallelLoopResult._completed = loopStateFlags == 0;
        if ((loopStateFlags & 2) != 0)
          parallelLoopResult._lowestBreakIteration = new long?(sharedPStateFlags.LowestBreakIteration);
        if (ParallelEtwProvider.Log.IsEnabled())
        {
          long TotalIterations = loopStateFlags != 0 ? ((loopStateFlags & 2) == 0 ? -1L : sharedPStateFlags.LowestBreakIteration - fromInclusive) : toExclusive - fromInclusive;
          ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, TotalIterations);
        }
      }
      return parallelLoopResult;
    }


    #nullable enable
    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      IEnumerable<TSource> source,
      Action<TSource> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, body, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel and loop options can be configured.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      IEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForEachWorker<TSource, object>(source, parallelOptions, body, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      IEnumerable<TSource> source,
      Action<TSource, ParallelLoopState> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, body, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      IEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource, ParallelLoopState> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForEachWorker<TSource, object>(source, parallelOptions, (Action<TSource>) null, body, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with 64-bit indexes on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      IEnumerable<TSource> source,
      Action<TSource, ParallelLoopState, long> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, body, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with 64-bit indexes on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      IEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource, ParallelLoopState, long> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForEachWorker<TSource, object>(source, parallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, body, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with thread-local data on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      IEnumerable<TSource> source,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      return Parallel.ForEachWorker<TSource, TLocal>(source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, body, (Func<TSource, ParallelLoopState, long, TLocal, TLocal>) null, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with thread-local data on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      IEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForEachWorker<TSource, TLocal>(source, parallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, body, (Func<TSource, ParallelLoopState, long, TLocal, TLocal>) null, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with thread-local data on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      IEnumerable<TSource> source,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      return Parallel.ForEachWorker<TSource, TLocal>(source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, TLocal, TLocal>) null, body, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with thread-local data and 64-bit indexes on an <see cref="T:System.Collections.IEnumerable" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      IEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.ForEachWorker<TSource, TLocal>(source, parallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, TLocal, TLocal>) null, body, localInit, localFinally);
    }


    #nullable disable
    private static ParallelLoopResult ForEachWorker<TSource, TLocal>(
      IEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource> body,
      Action<TSource, ParallelLoopState> bodyWithState,
      Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex,
      Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything,
      Func<TLocal> localInit,
      Action<TLocal> localFinally)
    {
      parallelOptions.CancellationToken.ThrowIfCancellationRequested();
      switch (source)
      {
        case TSource[] array:
          return Parallel.ForEachWorker<TSource, TLocal>(array, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
        case IList<TSource> list:
          return Parallel.ForEachWorker<TSource, TLocal>(list, parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
        default:
          return Parallel.PartitionerForEachWorker<TSource, TLocal>((Partitioner<TSource>) Partitioner.Create<TSource>(source), parallelOptions, body, bodyWithState, bodyWithStateAndIndex, bodyWithStateAndLocal, bodyWithEverything, localInit, localFinally);
      }
    }

    private static ParallelLoopResult ForEachWorker<TSource, TLocal>(
      TSource[] array,
      ParallelOptions parallelOptions,
      Action<TSource> body,
      Action<TSource, ParallelLoopState> bodyWithState,
      Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex,
      Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything,
      Func<TLocal> localInit,
      Action<TLocal> localFinally)
    {
      int lowerBound = array.GetLowerBound(0);
      int toExclusive = array.GetUpperBound(0) + 1;
      if (body != null)
        return Parallel.ForWorker<object>(lowerBound, toExclusive, parallelOptions, (Action<int>) (i => body(array[i])), (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
      if (bodyWithState != null)
        return Parallel.ForWorker<object>(lowerBound, toExclusive, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) ((i, state) => bodyWithState(array[i], state)), (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
      if (bodyWithStateAndIndex != null)
        return Parallel.ForWorker<object>(lowerBound, toExclusive, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) ((i, state) => bodyWithStateAndIndex(array[i], state, (long) i)), (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
      return bodyWithStateAndLocal != null ? Parallel.ForWorker<TLocal>(lowerBound, toExclusive, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, TLocal, TLocal>) ((i, state, local) => bodyWithStateAndLocal(array[i], state, local)), localInit, localFinally) : Parallel.ForWorker<TLocal>(lowerBound, toExclusive, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, TLocal, TLocal>) ((i, state, local) => bodyWithEverything(array[i], state, (long) i, local)), localInit, localFinally);
    }

    private static ParallelLoopResult ForEachWorker<TSource, TLocal>(
      IList<TSource> list,
      ParallelOptions parallelOptions,
      Action<TSource> body,
      Action<TSource, ParallelLoopState> bodyWithState,
      Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex,
      Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything,
      Func<TLocal> localInit,
      Action<TLocal> localFinally)
    {
      if (body != null)
        return Parallel.ForWorker<object>(0, list.Count, parallelOptions, (Action<int>) (i => body(list[i])), (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
      if (bodyWithState != null)
        return Parallel.ForWorker<object>(0, list.Count, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) ((i, state) => bodyWithState(list[i], state)), (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
      if (bodyWithStateAndIndex != null)
        return Parallel.ForWorker<object>(0, list.Count, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) ((i, state) => bodyWithStateAndIndex(list[i], state, (long) i)), (Func<int, ParallelLoopState, object, object>) null, (Func<object>) null, (Action<object>) null);
      return bodyWithStateAndLocal != null ? Parallel.ForWorker<TLocal>(0, list.Count, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, TLocal, TLocal>) ((i, state, local) => bodyWithStateAndLocal(list[i], state, local)), localInit, localFinally) : Parallel.ForWorker<TLocal>(0, list.Count, parallelOptions, (Action<int>) null, (Action<int, ParallelLoopState>) null, (Func<int, ParallelLoopState, TLocal, TLocal>) ((i, state, local) => bodyWithEverything(list[i], state, (long) i, local)), localInit, localFinally);
    }


    #nullable enable
    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on a <see cref="T:System.Collections.Concurrent.Partitioner" /> in which iterations may run in parallel.</summary>
    /// <param name="source">The partitioner that contains the original data source.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is  <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /> partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// The exception that is thrown when any methods in the <paramref name="source" /> partitioner return <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <see cref="M:System.Collections.Concurrent.Partitioner`1.GetPartitions(System.Int32)" /> method in the <paramref name="source" /> partitioner does not return the correct number of partitions.</exception>
    /// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception thrown from one of the specified delegates.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      Partitioner<TSource> source,
      Action<TSource> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.PartitionerForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, body, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on a <see cref="T:System.Collections.Concurrent.Partitioner" /> in which iterations may run in parallel, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The partitioner that contains the original data source.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /> partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// A method in the <paramref name="source" /> partitioner returns <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <see cref="M:System.Collections.Concurrent.Partitioner`1.GetPartitions(System.Int32)" /> method in the <paramref name="source" /> partitioner does not return the correct number of partitions.</exception>
    /// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception thrown from one of the specified delegates.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      Partitioner<TSource> source,
      Action<TSource, ParallelLoopState> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.PartitionerForEachWorker<TSource, object>(source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, body, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner`1" /> in which iterations may run in parallel and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The orderable partitioner that contains the original data source.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /> orderable partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// The <see cref="P:System.Collections.Concurrent.OrderablePartitioner`1.KeysNormalized" /> property in the source orderable partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// Any methods in the source orderable partitioner return <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception thrown from one of the specified delegates.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      OrderablePartitioner<TSource> source,
      Action<TSource, ParallelLoopState, long> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (!source.KeysNormalized)
        throw new InvalidOperationException(SR.Parallel_ForEach_OrderedPartitionerKeysNotNormalized);
      return Parallel.PartitionerForEachWorker<TSource, object>((Partitioner<TSource>) source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, body, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with thread-local data on a <see cref="T:System.Collections.Concurrent.Partitioner" /> in which iterations may run in parallel and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The partitioner that contains the original data source.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /><see cref="T:System.Collections.Concurrent.Partitioner" /> returns <see langword="false" /> or the partitioner returns <see langword="null" /> partitions.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      Partitioner<TSource> source,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      return Parallel.PartitionerForEachWorker<TSource, TLocal>(source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, body, (Func<TSource, ParallelLoopState, long, TLocal, TLocal>) null, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with thread-local data on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner`1" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The orderable partitioner that contains the original data source.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /><see cref="T:System.Collections.Concurrent.Partitioner" /> returns <see langword="false" /> or the partitioner returns <see langword="null" /> partitions.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      OrderablePartitioner<TSource> source,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      if (!source.KeysNormalized)
        throw new InvalidOperationException(SR.Parallel_ForEach_OrderedPartitionerKeysNotNormalized);
      return Parallel.PartitionerForEachWorker<TSource, TLocal>((Partitioner<TSource>) source, Parallel.s_defaultParallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, TLocal, TLocal>) null, body, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on a <see cref="T:System.Collections.Concurrent.Partitioner" /> in which iterations may run in parallel and loop options can be configured.</summary>
    /// <param name="source">The partitioner that contains the original data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /> partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// The exception that is thrown when any methods in the <paramref name="source" /> partitioner return <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception thrown from one of the specified delegates.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      Partitioner<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.PartitionerForEachWorker<TSource, object>(source, parallelOptions, body, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on a <see cref="T:System.Collections.Concurrent.Partitioner" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The partitioner that contains the original data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /> partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// The exception that is thrown when any methods in the <paramref name="source" /> partitioner return <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception thrown from one of the specified delegates.</exception>
    /// <returns>A  structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      Partitioner<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource, ParallelLoopState> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.PartitionerForEachWorker<TSource, object>(source, parallelOptions, (Action<TSource>) null, body, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner`1" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The orderable partitioner that contains the original data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is  <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /> orderable partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// The <see cref="P:System.Collections.Concurrent.OrderablePartitioner`1.KeysNormalized" /> property in the <paramref name="source" /> orderable partitioner returns <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// The exception that is thrown when any methods in the <paramref name="source" /> orderable partitioner return <see langword="null" />.</exception>
    /// <exception cref="T:System.AggregateException">The exception that is thrown to contain an exception thrown from one of the specified delegates.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource>(
      OrderablePartitioner<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource, ParallelLoopState, long> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      if (!source.KeysNormalized)
        throw new InvalidOperationException(SR.Parallel_ForEach_OrderedPartitionerKeysNotNormalized);
      return Parallel.PartitionerForEachWorker<TSource, object>((Partitioner<TSource>) source, parallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, body, (Func<TSource, ParallelLoopState, object, object>) null, (Func<TSource, ParallelLoopState, long, object, object>) null, (Func<object>) null, (Action<object>) null);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation  with thread-local data on a <see cref="T:System.Collections.Concurrent.Partitioner" /> in which iterations may run in parallel, loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The partitioner that contains the original data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /><see cref="T:System.Collections.Concurrent.Partitioner" /> returns <see langword="false" /> or the partitioner returns <see langword="null" /> partitions.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      Partitioner<TSource> source,
      ParallelOptions parallelOptions,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      return Parallel.PartitionerForEachWorker<TSource, TLocal>(source, parallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, body, (Func<TSource, ParallelLoopState, long, TLocal, TLocal>) null, localInit, localFinally);
    }

    /// <summary>Executes a <see langword="foreach" /> (<see langword="For Each" /> in Visual Basic) operation with 64-bit indexes and  with thread-local data on a <see cref="T:System.Collections.Concurrent.OrderablePartitioner`1" /> in which iterations may run in parallel , loop options can be configured, and the state of the loop can be monitored and manipulated.</summary>
    /// <param name="source">The orderable partitioner that contains the original data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="localInit">The function delegate that returns the initial state of the local data for each task.</param>
    /// <param name="body">The delegate that is invoked once per iteration.</param>
    /// <param name="localFinally">The delegate that performs a final action on the local state of each task.</param>
    /// <typeparam name="TSource">The type of the elements in <paramref name="source" />.</typeparam>
    /// <typeparam name="TLocal">The type of the thread-local data.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="parallelOptions" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="body" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="localInit" /> or <paramref name="localFinally" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Collections.Concurrent.Partitioner`1.SupportsDynamicPartitions" /> property in the <paramref name="source" /><see cref="T:System.Collections.Concurrent.Partitioner" /> returns <see langword="false" /> or the partitioner returns <see langword="null" /> partitions.</exception>
    /// <exception cref="T:System.AggregateException">The exception that contains all the individual exceptions thrown on all threads.</exception>
    /// <exception cref="T:System.OperationCanceledException">The <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> argument is canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> associated with the <see cref="T:System.Threading.CancellationToken" /> in the <paramref name="parallelOptions" /> has been disposed.</exception>
    /// <returns>A structure that contains information about which portion of the loop completed.</returns>
    public static ParallelLoopResult ForEach<TSource, TLocal>(
      OrderablePartitioner<TSource> source,
      ParallelOptions parallelOptions,
      Func<TLocal> localInit,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> body,
      Action<TLocal> localFinally)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      if (localInit == null)
        throw new ArgumentNullException(nameof (localInit));
      if (localFinally == null)
        throw new ArgumentNullException(nameof (localFinally));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      if (!source.KeysNormalized)
        throw new InvalidOperationException(SR.Parallel_ForEach_OrderedPartitionerKeysNotNormalized);
      return Parallel.PartitionerForEachWorker<TSource, TLocal>((Partitioner<TSource>) source, parallelOptions, (Action<TSource>) null, (Action<TSource, ParallelLoopState>) null, (Action<TSource, ParallelLoopState, long>) null, (Func<TSource, ParallelLoopState, TLocal, TLocal>) null, body, localInit, localFinally);
    }


    #nullable disable
    private static ParallelLoopResult PartitionerForEachWorker<TSource, TLocal>(
      Partitioner<TSource> source,
      ParallelOptions parallelOptions,
      Action<TSource> simpleBody,
      Action<TSource, ParallelLoopState> bodyWithState,
      Action<TSource, ParallelLoopState, long> bodyWithStateAndIndex,
      Func<TSource, ParallelLoopState, TLocal, TLocal> bodyWithStateAndLocal,
      Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyWithEverything,
      Func<TLocal> localInit,
      Action<TLocal> localFinally)
    {
      OrderablePartitioner<TSource> orderedSource = source as OrderablePartitioner<TSource>;
      if (!source.SupportsDynamicPartitions)
        throw new InvalidOperationException(SR.Parallel_ForEach_PartitionerNotDynamic);
      CancellationToken cancellationToken = parallelOptions.CancellationToken;
      cancellationToken.ThrowIfCancellationRequested();
      int forkJoinContextID = 0;
      if (ParallelEtwProvider.Log.IsEnabled())
      {
        forkJoinContextID = Interlocked.Increment(ref Parallel.s_forkJoinContextID);
        ParallelEtwProvider.Log.ParallelLoopBegin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, ParallelEtwProvider.ForkJoinOperationType.ParallelForEach, 0L, 0L);
      }
      ParallelLoopStateFlags64 sharedPStateFlags = new ParallelLoopStateFlags64();
      ParallelLoopResult parallelLoopResult = new ParallelLoopResult();
      OperationCanceledException oce = (OperationCanceledException) null;
      cancellationToken = parallelOptions.CancellationToken;
      CancellationTokenRegistration tokenRegistration1;
      if (cancellationToken.CanBeCanceled)
      {
        cancellationToken = parallelOptions.CancellationToken;
        tokenRegistration1 = cancellationToken.UnsafeRegister((Action<object>) (o =>
        {
          oce = new OperationCanceledException(parallelOptions.CancellationToken);
          sharedPStateFlags.Cancel();
        }), (object) null);
      }
      else
        tokenRegistration1 = new CancellationTokenRegistration();
      CancellationTokenRegistration tokenRegistration2 = tokenRegistration1;
      IEnumerable<TSource> partitionerSource = (IEnumerable<TSource>) null;
      IEnumerable<KeyValuePair<long, TSource>> orderablePartitionerSource = (IEnumerable<KeyValuePair<long, TSource>>) null;
      if (orderedSource != null)
      {
        orderablePartitionerSource = orderedSource.GetOrderableDynamicPartitions();
        if (orderablePartitionerSource == null)
          throw new InvalidOperationException(SR.Parallel_ForEach_PartitionerReturnedNull);
      }
      else
      {
        partitionerSource = source.GetDynamicPartitions();
        if (partitionerSource == null)
          throw new InvalidOperationException(SR.Parallel_ForEach_PartitionerReturnedNull);
      }
      try
      {
        try
        {
          TaskReplicator.Run<IEnumerator>((TaskReplicator.ReplicatableUserAction<IEnumerator>) ((ref IEnumerator partitionState, int timeout, out bool replicationDelegateYieldedBeforeCompletion) =>
          {
            replicationDelegateYieldedBeforeCompletion = false;
            if (ParallelEtwProvider.Log.IsEnabled())
              ParallelEtwProvider.Log.ParallelFork(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
            TLocal local = default (TLocal);
            bool flag = false;
            try
            {
              ParallelLoopState64 parallelLoopState64 = (ParallelLoopState64) null;
              if (bodyWithState != null || bodyWithStateAndIndex != null)
                parallelLoopState64 = new ParallelLoopState64(sharedPStateFlags);
              else if (bodyWithStateAndLocal != null || bodyWithEverything != null)
              {
                parallelLoopState64 = new ParallelLoopState64(sharedPStateFlags);
                if (localInit != null)
                {
                  local = localInit();
                  flag = true;
                }
              }
              int timeoutPoint = Parallel.ComputeTimeoutPoint(timeout);
              if (orderedSource != null)
              {
                if (!(partitionState is IEnumerator<KeyValuePair<long, TSource>> enumerator3))
                {
                  enumerator3 = orderablePartitionerSource.GetEnumerator();
                  partitionState = (IEnumerator) enumerator3;
                }
                if (enumerator3 == null)
                  throw new InvalidOperationException(SR.Parallel_ForEach_NullEnumerator);
                while (enumerator3.MoveNext())
                {
                  KeyValuePair<long, TSource> current = enumerator3.Current;
                  long key = current.Key;
                  TSource source1 = current.Value;
                  if (parallelLoopState64 != null)
                    parallelLoopState64.CurrentIteration = key;
                  if (simpleBody != null)
                    simpleBody(source1);
                  else if (bodyWithState != null)
                    bodyWithState(source1, (ParallelLoopState) parallelLoopState64);
                  else if (bodyWithStateAndIndex != null)
                    bodyWithStateAndIndex(source1, (ParallelLoopState) parallelLoopState64, key);
                  else
                    local = bodyWithStateAndLocal == null ? bodyWithEverything(source1, (ParallelLoopState) parallelLoopState64, key, local) : bodyWithStateAndLocal(source1, (ParallelLoopState) parallelLoopState64, local);
                  if (sharedPStateFlags.ShouldExitLoop(key))
                    break;
                  if (Parallel.CheckTimeoutReached(timeoutPoint))
                  {
                    replicationDelegateYieldedBeforeCompletion = true;
                    break;
                  }
                }
              }
              else
              {
                if (!(partitionState is IEnumerator<TSource> enumerator4))
                {
                  enumerator4 = partitionerSource.GetEnumerator();
                  partitionState = (IEnumerator) enumerator4;
                }
                if (enumerator4 == null)
                  throw new InvalidOperationException(SR.Parallel_ForEach_NullEnumerator);
                if (parallelLoopState64 != null)
                  parallelLoopState64.CurrentIteration = 0L;
                while (enumerator4.MoveNext())
                {
                  TSource current = enumerator4.Current;
                  if (simpleBody != null)
                    simpleBody(current);
                  else if (bodyWithState != null)
                    bodyWithState(current, (ParallelLoopState) parallelLoopState64);
                  else if (bodyWithStateAndLocal != null)
                    local = bodyWithStateAndLocal(current, (ParallelLoopState) parallelLoopState64, local);
                  if (sharedPStateFlags.LoopStateFlags != 0)
                    break;
                  if (Parallel.CheckTimeoutReached(timeoutPoint))
                  {
                    replicationDelegateYieldedBeforeCompletion = true;
                    break;
                  }
                }
              }
            }
            catch (Exception ex)
            {
              sharedPStateFlags.SetExceptional();
              ExceptionDispatchInfo.Throw(ex);
            }
            finally
            {
              if (localFinally != null & flag)
                localFinally(local);
              if (!replicationDelegateYieldedBeforeCompletion && partitionState is IDisposable disposable2)
                disposable2.Dispose();
              if (ParallelEtwProvider.Log.IsEnabled())
                ParallelEtwProvider.Log.ParallelJoin(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID);
            }
          }), parallelOptions, true);
        }
        finally
        {
          if (parallelOptions.CancellationToken.CanBeCanceled)
            tokenRegistration2.Dispose();
        }
        if (oce != null)
          throw oce;
      }
      catch (AggregateException ex)
      {
        Parallel.ThrowSingleCancellationExceptionOrOtherException((ICollection) ex.InnerExceptions, parallelOptions.CancellationToken, (Exception) ex);
      }
      finally
      {
        int loopStateFlags = sharedPStateFlags.LoopStateFlags;
        parallelLoopResult._completed = loopStateFlags == 0;
        if ((loopStateFlags & 2) != 0)
          parallelLoopResult._lowestBreakIteration = new long?(sharedPStateFlags.LowestBreakIteration);
        (orderablePartitionerSource == null ? partitionerSource as IDisposable : orderablePartitionerSource as IDisposable)?.Dispose();
        if (ParallelEtwProvider.Log.IsEnabled())
          ParallelEtwProvider.Log.ParallelLoopEnd(TaskScheduler.Current.Id, Task.CurrentId.GetValueOrDefault(), forkJoinContextID, 0L);
      }
      return parallelLoopResult;
    }

    private static OperationCanceledException ReduceToSingleCancellationException(
      ICollection exceptions,
      CancellationToken cancelToken)
    {
      if (exceptions == null || exceptions.Count == 0)
        return (OperationCanceledException) null;
      if (!cancelToken.IsCancellationRequested)
        return (OperationCanceledException) null;
      Exception cancellationException = (Exception) null;
      foreach (Exception exception in (IEnumerable) exceptions)
      {
        if (cancellationException == null)
          cancellationException = exception;
        if (!(exception is OperationCanceledException canceledException) || !cancelToken.Equals(canceledException.CancellationToken))
          return (OperationCanceledException) null;
      }
      return (OperationCanceledException) cancellationException;
    }

    private static void ThrowSingleCancellationExceptionOrOtherException(
      ICollection exceptions,
      CancellationToken cancelToken,
      Exception otherException)
    {
      ExceptionDispatchInfo.Throw((Exception) Parallel.ReduceToSingleCancellationException(exceptions, cancelToken) ?? otherException);
    }


    #nullable enable
    /// <summary>Executes a <c>for-each</c> operation on an <see cref="T:System.Collections.Generic.IEnumerable`1" /> in which iterations may run in parallel.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="body" /> is <see langword="null" />.</exception>
    /// <returns>A task that represents the entire <c>for-each</c> operation.</returns>
    public static Task ForEachAsync<TSource>(
      IEnumerable<TSource> source,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachAsync<TSource>(source, Parallel.DefaultDegreeOfParallelism, TaskScheduler.Default, new CancellationToken(), body);
    }

    /// <summary>Executes a <c>for-each</c> operation on an <see cref="T:System.Collections.Generic.IEnumerable`1" /> in which iterations may run in parallel.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="cancellationToken">A cancellation token that may be used to cancel the <c>for-each</c> operation.</param>
    /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="body" /> is <see langword="null" />.</exception>
    /// <returns>A task that represents the entire <c>for-each</c> operation.</returns>
    public static Task ForEachAsync<TSource>(
      IEnumerable<TSource> source,
      CancellationToken cancellationToken,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachAsync<TSource>(source, Parallel.DefaultDegreeOfParallelism, TaskScheduler.Default, cancellationToken, body);
    }

    /// <summary>Executes a <c>for-each</c> operation on an <see cref="T:System.Collections.Generic.IEnumerable`1" /> in which iterations may run in parallel.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="body" /> is <see langword="null" />.</exception>
    /// <returns>A task that represents the entire <c>for-each</c> operation.</returns>
    public static Task ForEachAsync<TSource>(
      IEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachAsync<TSource>(source, parallelOptions.EffectiveMaxConcurrencyLevel, parallelOptions.EffectiveTaskScheduler, parallelOptions.CancellationToken, body);
    }


    #nullable disable
    private static Task ForEachAsync<TSource>(
      IEnumerable<TSource> source,
      int dop,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (dop < 0)
        dop = Parallel.DefaultDegreeOfParallelism;
      Func<object, Task> taskBody = (Func<object, Task>) (async o =>
      {
        Parallel.SyncForEachAsyncState<TSource> state = (Parallel.SyncForEachAsyncState<TSource>) o;
        bool launchedNext = false;
        try
        {
          while (!state.Cancellation.IsCancellationRequested)
          {
            TSource current;
            lock (state)
            {
              if (!state.Enumerator.MoveNext())
              {
                state = (Parallel.SyncForEachAsyncState<TSource>) null;
                return;
              }
              current = state.Enumerator.Current;
            }
            if (!launchedNext)
            {
              launchedNext = true;
              state.QueueWorkerIfDopAvailable();
            }
            await state.LoopBody(current, state.Cancellation.Token);
          }
          state = (Parallel.SyncForEachAsyncState<TSource>) null;
        }
        catch (Exception ex)
        {
          state.RecordException(ex);
          state = (Parallel.SyncForEachAsyncState<TSource>) null;
        }
        finally
        {
          if (state.SignalWorkerCompletedIterating())
          {
            try
            {
              state.Dispose();
            }
            catch (Exception ex)
            {
              state.RecordException(ex);
            }
            state.Complete();
          }
        }
      });
      try
      {
        Parallel.SyncForEachAsyncState<TSource> forEachAsyncState = new Parallel.SyncForEachAsyncState<TSource>(source, taskBody, dop, scheduler, cancellationToken, body);
        forEachAsyncState.QueueWorkerIfDopAvailable();
        return forEachAsyncState.Task;
      }
      catch (Exception ex)
      {
        return Task.FromException(ex);
      }
    }


    #nullable enable
    /// <summary>Executes a <c>for-each</c> operation on an <see cref="T:System.Collections.Generic.IEnumerable`1" /> in which iterations may run in parallel.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="body" /> is <see langword="null" />.</exception>
    /// <returns>A task that represents the entire <c>for-each</c> operation.</returns>
    public static Task ForEachAsync<TSource>(
      IAsyncEnumerable<TSource> source,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachAsync<TSource>(source, Parallel.DefaultDegreeOfParallelism, TaskScheduler.Default, new CancellationToken(), body);
    }

    /// <summary>Executes a <c>for-each</c> operation on an <see cref="T:System.Collections.Generic.IEnumerable`1" /> in which iterations may run in parallel.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="cancellationToken">A cancellation token that may be used to cancel the <c>for-each</c> operation.</param>
    /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="body" /> is <see langword="null" />.</exception>
    /// <returns>A task that represents the entire <c>for-each</c> operation.</returns>
    public static Task ForEachAsync<TSource>(
      IAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachAsync<TSource>(source, Parallel.DefaultDegreeOfParallelism, TaskScheduler.Default, cancellationToken, body);
    }

    /// <summary>Executes a <c>for-each</c> operation on an <see cref="T:System.Collections.Generic.IEnumerable`1" /> in which iterations may run in parallel.</summary>
    /// <param name="source">An enumerable data source.</param>
    /// <param name="parallelOptions">An object that configures the behavior of this operation.</param>
    /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
    /// <typeparam name="TSource">The type of the data in the source.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="body" /> is <see langword="null" />.</exception>
    /// <returns>A task that represents the entire <c>for-each</c> operation.</returns>
    public static Task ForEachAsync<TSource>(
      IAsyncEnumerable<TSource> source,
      ParallelOptions parallelOptions,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (parallelOptions == null)
        throw new ArgumentNullException(nameof (parallelOptions));
      if (body == null)
        throw new ArgumentNullException(nameof (body));
      return Parallel.ForEachAsync<TSource>(source, parallelOptions.EffectiveMaxConcurrencyLevel, parallelOptions.EffectiveTaskScheduler, parallelOptions.CancellationToken, body);
    }


    #nullable disable
    private static Task ForEachAsync<TSource>(
      IAsyncEnumerable<TSource> source,
      int dop,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      Func<TSource, CancellationToken, ValueTask> body)
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (dop < 0)
        dop = Parallel.DefaultDegreeOfParallelism;
      Func<object, Task> taskBody = (Func<object, Task>) (async o =>
      {
        Parallel.AsyncForEachAsyncState<TSource> state = (Parallel.AsyncForEachAsyncState<TSource>) o;
        bool launchedNext = false;
        try
        {
          while (!state.Cancellation.IsCancellationRequested)
          {
            await state.Lock.WaitAsync(state.Cancellation.Token);
            TSource current;
            try
            {
              if (await state.Enumerator.MoveNextAsync())
                current = state.Enumerator.Current;
              else
                break;
            }
            finally
            {
              state.Lock.Release();
            }
            if (!launchedNext)
            {
              launchedNext = true;
              state.QueueWorkerIfDopAvailable();
            }
            await state.LoopBody(current, state.Cancellation.Token);
          }
        }
        catch (Exception ex)
        {
          state.RecordException(ex);
        }
        finally
        {
          if (state.SignalWorkerCompletedIterating())
          {
            try
            {
              await state.DisposeAsync();
            }
            catch (Exception ex)
            {
              state.RecordException(ex);
            }
            state.Complete();
          }
        }
        state = (Parallel.AsyncForEachAsyncState<TSource>) null;
      });
      try
      {
        Parallel.AsyncForEachAsyncState<TSource> forEachAsyncState = new Parallel.AsyncForEachAsyncState<TSource>(source, taskBody, dop, scheduler, cancellationToken, body);
        forEachAsyncState.QueueWorkerIfDopAvailable();
        return forEachAsyncState.Task;
      }
      catch (Exception ex)
      {
        return Task.FromException(ex);
      }
    }

    private static int DefaultDegreeOfParallelism => Environment.ProcessorCount;

    private abstract class ForEachAsyncState<TSource> : TaskCompletionSource, IThreadPoolWorkItem
    {
      private readonly CancellationToken _externalCancellationToken;
      protected readonly CancellationTokenRegistration _registration;
      private readonly Func<object, Task> _taskBody;
      private readonly TaskScheduler _scheduler;
      private readonly ExecutionContext _executionContext;
      private int _completionRefCount;
      private List<Exception> _exceptions;
      private int _remainingDop;
      public readonly Func<TSource, CancellationToken, ValueTask> LoopBody;
      public readonly CancellationTokenSource Cancellation = new CancellationTokenSource();

      protected ForEachAsyncState(
        Func<object, Task> taskBody,
        int dop,
        TaskScheduler scheduler,
        CancellationToken cancellationToken,
        Func<TSource, CancellationToken, ValueTask> body)
      {
        this._taskBody = taskBody;
        this._remainingDop = dop;
        this.LoopBody = body;
        this._scheduler = scheduler;
        if (scheduler == TaskScheduler.Default)
          this._executionContext = ExecutionContext.Capture();
        this._externalCancellationToken = cancellationToken;
        this._registration = cancellationToken.UnsafeRegister((Action<object>) (o => ((Parallel.ForEachAsyncState<TSource>) o).Cancellation.Cancel()), (object) this);
      }

      public void QueueWorkerIfDopAvailable()
      {
        if (this._remainingDop <= 0)
          return;
        --this._remainingDop;
        Interlocked.Increment(ref this._completionRefCount);
        if (this._scheduler == TaskScheduler.Default)
          ThreadPool.UnsafeQueueUserWorkItem((IThreadPoolWorkItem) this, false);
        else
          Task.Factory.StartNew<Task>(this._taskBody, (object) this, new CancellationToken(), TaskCreationOptions.DenyChildAttach, this._scheduler);
      }

      public bool SignalWorkerCompletedIterating() => Interlocked.Decrement(ref this._completionRefCount) == 0;

      public void RecordException(Exception e)
      {
        lock (this)
          (this._exceptions ?? (this._exceptions = new List<Exception>())).Add(e);
        this.Cancellation.Cancel();
      }

      public void Complete()
      {
        bool flag;
        if (this._externalCancellationToken.IsCancellationRequested)
          flag = this.TrySetCanceled(this._externalCancellationToken);
        else if (this._exceptions == null)
          flag = this.TrySetResult();
        else
          flag = this.TrySetException((IEnumerable<Exception>) this._exceptions);
      }

      void IThreadPoolWorkItem.Execute()
      {
        if (this._executionContext == null)
        {
          Task task1 = this._taskBody((object) this);
        }
        else
        {
          Task task2;
          ExecutionContext.Run(this._executionContext, (ContextCallback) (o => task2 = ((Parallel.ForEachAsyncState<TSource>) o)._taskBody(o)), (object) this);
        }
      }
    }

    private sealed class SyncForEachAsyncState<TSource> : 
      Parallel.ForEachAsyncState<TSource>,
      IDisposable
    {
      public readonly IEnumerator<TSource> Enumerator;

      public SyncForEachAsyncState(
        IEnumerable<TSource> source,
        Func<object, Task> taskBody,
        int dop,
        TaskScheduler scheduler,
        CancellationToken cancellationToken,
        Func<TSource, CancellationToken, ValueTask> body)
        : base(taskBody, dop, scheduler, cancellationToken, body)
      {
        this.Enumerator = source.GetEnumerator() ?? throw new InvalidOperationException(SR.Parallel_ForEach_NullEnumerator);
      }

      public void Dispose()
      {
        this._registration.Dispose();
        this.Enumerator.Dispose();
      }
    }

    private sealed class AsyncForEachAsyncState<TSource> : 
      Parallel.ForEachAsyncState<TSource>,
      IAsyncDisposable
    {
      public readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);
      public readonly IAsyncEnumerator<TSource> Enumerator;

      public AsyncForEachAsyncState(
        IAsyncEnumerable<TSource> source,
        Func<object, Task> taskBody,
        int dop,
        TaskScheduler scheduler,
        CancellationToken cancellationToken,
        Func<TSource, CancellationToken, ValueTask> body)
        : base(taskBody, dop, scheduler, cancellationToken, body)
      {
        this.Enumerator = source.GetAsyncEnumerator(this.Cancellation.Token) ?? throw new InvalidOperationException(SR.Parallel_ForEach_NullEnumerator);
      }

      public ValueTask DisposeAsync()
      {
        this._registration.Dispose();
        return this.Enumerator.DisposeAsync();
      }
    }
  }
}
