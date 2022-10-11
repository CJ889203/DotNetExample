// Decompiled with JetBrains decompiler
// Type: System.Threading.CancellationTokenSource
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


#nullable enable
namespace System.Threading
{
  /// <summary>Signals to a <see cref="T:System.Threading.CancellationToken" /> that it should be canceled.</summary>
  public class CancellationTokenSource : IDisposable
  {

    #nullable disable
    internal static readonly CancellationTokenSource s_canceledSource = new CancellationTokenSource()
    {
      _state = 2
    };
    internal static readonly CancellationTokenSource s_neverCanceledSource = new CancellationTokenSource();
    private static readonly System.Threading.TimerCallback s_timerCallback = new System.Threading.TimerCallback(CancellationTokenSource.TimerCallback);
    private volatile int _state;
    private bool _disposed;
    private volatile TimerQueueTimer _timer;
    private volatile ManualResetEvent _kernelEvent;
    private CancellationTokenSource.Registrations _registrations;

    private static void TimerCallback(object state) => ((CancellationTokenSource) state).NotifyCancellation(false);

    /// <summary>Gets whether cancellation has been requested for this <see cref="T:System.Threading.CancellationTokenSource" />.</summary>
    /// <returns>
    /// <see langword="true" /> if cancellation has been requested for this <see cref="T:System.Threading.CancellationTokenSource" />; otherwise, <see langword="false" />.</returns>
    public bool IsCancellationRequested => this._state != 0;

    internal bool IsCancellationCompleted => this._state == 2;

    /// <summary>Gets the <see cref="T:System.Threading.CancellationToken" /> associated with this <see cref="T:System.Threading.CancellationTokenSource" />.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The token source has been disposed.</exception>
    /// <returns>The <see cref="T:System.Threading.CancellationToken" /> associated with this <see cref="T:System.Threading.CancellationTokenSource" />.</returns>
    public CancellationToken Token
    {
      get
      {
        this.ThrowIfDisposed();
        return new CancellationToken(this);
      }
    }


    #nullable enable
    internal WaitHandle WaitHandle
    {
      get
      {
        this.ThrowIfDisposed();
        if (this._kernelEvent != null)
          return (WaitHandle) this._kernelEvent;
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        if (Interlocked.CompareExchange<ManualResetEvent>(ref this._kernelEvent, manualResetEvent, (ManualResetEvent) null) != null)
          manualResetEvent.Dispose();
        if (this.IsCancellationRequested)
          this._kernelEvent.Set();
        return (WaitHandle) this._kernelEvent;
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class.</summary>
    public CancellationTokenSource()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class that will be canceled after the specified time span.</summary>
    /// <param name="delay">The time interval to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="delay" />.<see cref="P:System.TimeSpan.TotalMilliseconds" /> is less than -1 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    public CancellationTokenSource(TimeSpan delay)
    {
      long totalMilliseconds = (long) delay.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > 4294967294L)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.delay);
      this.InitializeWithTimer((uint) totalMilliseconds);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class that will be canceled after the specified delay in milliseconds.</summary>
    /// <param name="millisecondsDelay">The time interval in milliseconds to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsDelay" /> is less than -1.</exception>
    public CancellationTokenSource(int millisecondsDelay)
    {
      if (millisecondsDelay < -1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsDelay);
      this.InitializeWithTimer((uint) millisecondsDelay);
    }

    private void InitializeWithTimer(uint millisecondsDelay)
    {
      if (millisecondsDelay == 0U)
        this._state = 2;
      else
        this._timer = new TimerQueueTimer(CancellationTokenSource.s_timerCallback, (object) this, millisecondsDelay, uint.MaxValue, false);
    }

    /// <summary>Communicates a request for cancellation.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">An aggregate exception containing all the exceptions thrown by the registered callbacks on the associated <see cref="T:System.Threading.CancellationToken" />.</exception>
    public void Cancel() => this.Cancel(false);

    /// <summary>Communicates a request for cancellation, and specifies whether remaining callbacks and cancelable operations should be processed if an exception occurs.</summary>
    /// <param name="throwOnFirstException">
    /// <see langword="true" /> if exceptions should immediately propagate; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.AggregateException">An aggregate exception containing all the exceptions thrown by the registered callbacks on the associated <see cref="T:System.Threading.CancellationToken" />.</exception>
    public void Cancel(bool throwOnFirstException)
    {
      this.ThrowIfDisposed();
      this.NotifyCancellation(throwOnFirstException);
    }

    /// <summary>Schedules a cancel operation on this <see cref="T:System.Threading.CancellationTokenSource" /> after the specified time span.</summary>
    /// <param name="delay">The time span to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The exception thrown when this <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when <paramref name="delay" /> is less than -1 or greater than Int32.MaxValue.</exception>
    public void CancelAfter(TimeSpan delay)
    {
      long totalMilliseconds = (long) delay.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > 4294967294L)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.delay);
      this.CancelAfter((uint) totalMilliseconds);
    }

    /// <summary>Schedules a cancel operation on this <see cref="T:System.Threading.CancellationTokenSource" /> after the specified number of milliseconds.</summary>
    /// <param name="millisecondsDelay">The time span to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The exception thrown when this <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The exception thrown when <paramref name="millisecondsDelay" /> is less than -1.</exception>
    public void CancelAfter(int millisecondsDelay)
    {
      if (millisecondsDelay < -1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.millisecondsDelay);
      this.CancelAfter((uint) millisecondsDelay);
    }

    private void CancelAfter(uint millisecondsDelay)
    {
      this.ThrowIfDisposed();
      if (this.IsCancellationRequested)
        return;
      TimerQueueTimer timerQueueTimer1 = this._timer;
      if (timerQueueTimer1 == null)
      {
        timerQueueTimer1 = new TimerQueueTimer(CancellationTokenSource.s_timerCallback, (object) this, uint.MaxValue, uint.MaxValue, false);
        TimerQueueTimer timerQueueTimer2 = Interlocked.CompareExchange<TimerQueueTimer>(ref this._timer, timerQueueTimer1, (TimerQueueTimer) null);
        if (timerQueueTimer2 != null)
        {
          timerQueueTimer1.Close();
          timerQueueTimer1 = timerQueueTimer2;
        }
      }
      try
      {
        timerQueueTimer1.Change(millisecondsDelay, uint.MaxValue);
      }
      catch (ObjectDisposedException ex)
      {
      }
    }

    /// <summary>Attempts to reset the <see cref="T:System.Threading.CancellationTokenSource" /> to be used for an unrelated operation.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.CancellationTokenSource" /> has not had cancellation requested and could have its state reset to be reused for a subsequent operation; otherwise, <see langword="false" />.</returns>
    public bool TryReset()
    {
      this.ThrowIfDisposed();
      if (this._state == 0)
      {
        bool flag = false;
        try
        {
          TimerQueueTimer timer = this._timer;
          flag = timer == null || timer.Change(uint.MaxValue, uint.MaxValue) && !timer._everQueued;
        }
        catch (ObjectDisposedException ex)
        {
        }
        if (flag)
        {
          Volatile.Read<CancellationTokenSource.Registrations>(ref this._registrations)?.UnregisterAll();
          return true;
        }
      }
      return false;
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Threading.CancellationTokenSource" /> class and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._disposed)
        return;
      TimerQueueTimer timer = this._timer;
      if (timer != null)
      {
        this._timer = (TimerQueueTimer) null;
        timer.Close();
      }
      this._registrations = (CancellationTokenSource.Registrations) null;
      if (this._kernelEvent != null)
      {
        ManualResetEvent manualResetEvent = Interlocked.Exchange<ManualResetEvent>(ref this._kernelEvent, (ManualResetEvent) null);
        if (manualResetEvent != null && this._state != 1)
          manualResetEvent.Dispose();
      }
      this._disposed = true;
    }

    private void ThrowIfDisposed()
    {
      if (!this._disposed)
        return;
      ThrowHelper.ThrowObjectDisposedException(ExceptionResource.CancellationTokenSource_Disposed);
    }


    #nullable disable
    internal CancellationTokenRegistration Register(
      Delegate callback,
      object stateForCallback,
      SynchronizationContext syncContext,
      ExecutionContext executionContext)
    {
      if (!this.IsCancellationRequested)
      {
        if (this._disposed)
          return new CancellationTokenRegistration();
        CancellationTokenSource.Registrations registrations1 = Volatile.Read<CancellationTokenSource.Registrations>(ref this._registrations);
        if (registrations1 == null)
        {
          CancellationTokenSource.Registrations registrations2 = new CancellationTokenSource.Registrations(this);
          registrations1 = Interlocked.CompareExchange<CancellationTokenSource.Registrations>(ref this._registrations, registrations2, (CancellationTokenSource.Registrations) null) ?? registrations2;
        }
        CancellationTokenSource.CallbackNode node = (CancellationTokenSource.CallbackNode) null;
        long id = 0;
        if (registrations1.FreeNodeList != null)
        {
          registrations1.EnterLock();
          try
          {
            node = registrations1.FreeNodeList;
            if (node != null)
            {
              registrations1.FreeNodeList = node.Next;
              CancellationTokenSource.CallbackNode callbackNode = node;
              long num1 = registrations1.NextAvailableId++;
              long num2;
              id = num2 = num1;
              callbackNode.Id = num2;
              node.Callback = callback;
              node.CallbackState = stateForCallback;
              node.ExecutionContext = executionContext;
              node.SynchronizationContext = syncContext;
              node.Next = registrations1.Callbacks;
              registrations1.Callbacks = node;
              if (node.Next != null)
                node.Next.Prev = node;
            }
          }
          finally
          {
            registrations1.ExitLock();
          }
        }
        if (node == null)
        {
          node = new CancellationTokenSource.CallbackNode(registrations1);
          node.Callback = callback;
          node.CallbackState = stateForCallback;
          node.ExecutionContext = executionContext;
          node.SynchronizationContext = syncContext;
          registrations1.EnterLock();
          try
          {
            CancellationTokenSource.CallbackNode callbackNode = node;
            long num3 = registrations1.NextAvailableId++;
            long num4;
            id = num4 = num3;
            callbackNode.Id = num4;
            node.Next = registrations1.Callbacks;
            if (node.Next != null)
              node.Next.Prev = node;
            registrations1.Callbacks = node;
          }
          finally
          {
            registrations1.ExitLock();
          }
        }
        if (!this.IsCancellationRequested || !registrations1.Unregister(id, node))
          return new CancellationTokenRegistration(id, node);
      }
      CancellationTokenSource.Invoke(callback, stateForCallback, this);
      return new CancellationTokenRegistration();
    }

    private void NotifyCancellation(bool throwOnFirstException)
    {
      if (this.IsCancellationRequested || Interlocked.CompareExchange(ref this._state, 1, 0) != 0)
        return;
      TimerQueueTimer timer = this._timer;
      if (timer != null)
      {
        this._timer = (TimerQueueTimer) null;
        timer.Close();
      }
      this._kernelEvent?.Set();
      this.ExecuteCallbackHandlers(throwOnFirstException);
    }

    private void ExecuteCallbackHandlers(bool throwOnFirstException)
    {
      CancellationTokenSource.Registrations registrations = Interlocked.Exchange<CancellationTokenSource.Registrations>(ref this._registrations, (CancellationTokenSource.Registrations) null);
      if (registrations == null)
      {
        Interlocked.Exchange(ref this._state, 2);
      }
      else
      {
        registrations.ThreadIDExecutingCallbacks = Environment.CurrentManagedThreadId;
        List<Exception> innerExceptions = (List<Exception>) null;
        try
        {
          while (true)
          {
            registrations.EnterLock();
            CancellationTokenSource.CallbackNode callbacks;
            try
            {
              callbacks = registrations.Callbacks;
              if (callbacks != null)
              {
                if (callbacks.Next != null)
                  callbacks.Next.Prev = (CancellationTokenSource.CallbackNode) null;
                registrations.Callbacks = callbacks.Next;
                registrations.ExecutingCallbackId = callbacks.Id;
                callbacks.Id = 0L;
              }
              else
                break;
            }
            finally
            {
              registrations.ExitLock();
            }
            try
            {
              if (callbacks.SynchronizationContext != null)
              {
                callbacks.SynchronizationContext.Send((SendOrPostCallback) (s =>
                {
                  CancellationTokenSource.CallbackNode callbackNode = (CancellationTokenSource.CallbackNode) s;
                  callbackNode.Registrations.ThreadIDExecutingCallbacks = Environment.CurrentManagedThreadId;
                  callbackNode.ExecuteCallback();
                }), (object) callbacks);
                registrations.ThreadIDExecutingCallbacks = Environment.CurrentManagedThreadId;
              }
              else
                callbacks.ExecuteCallback();
            }
            catch (Exception ex) when (!throwOnFirstException)
            {
              (innerExceptions ?? (innerExceptions = new List<Exception>())).Add(ex);
            }
          }
        }
        finally
        {
          this._state = 2;
          Interlocked.Exchange(ref registrations.ExecutingCallbackId, 0L);
        }
        if (innerExceptions != null)
          throw new AggregateException((IEnumerable<Exception>) innerExceptions);
      }
    }


    #nullable enable
    /// <summary>Creates a <see cref="T:System.Threading.CancellationTokenSource" /> that will be in the canceled state when any of the source tokens are in the canceled state.</summary>
    /// <param name="token1">The first cancellation token to observe.</param>
    /// <param name="token2">The second cancellation token to observe.</param>
    /// <exception cref="T:System.ObjectDisposedException">A <see cref="T:System.Threading.CancellationTokenSource" /> associated with one of the source tokens has been disposed.</exception>
    /// <returns>A <see cref="T:System.Threading.CancellationTokenSource" /> that is linked to the source tokens.</returns>
    public static CancellationTokenSource CreateLinkedTokenSource(
      CancellationToken token1,
      CancellationToken token2)
    {
      if (!token1.CanBeCanceled)
        return CancellationTokenSource.CreateLinkedTokenSource(token2);
      return !token2.CanBeCanceled ? (CancellationTokenSource) new CancellationTokenSource.Linked1CancellationTokenSource(token1) : (CancellationTokenSource) new CancellationTokenSource.Linked2CancellationTokenSource(token1, token2);
    }

    /// <summary>Creates a <see cref="T:System.Threading.CancellationTokenSource" /> that will be in the canceled state when the supplied token is in the canceled state.</summary>
    /// <param name="token">The cancellation token to observe.</param>
    /// <returns>An object that's linked to the source token.</returns>
    public static CancellationTokenSource CreateLinkedTokenSource(
      CancellationToken token)
    {
      return !token.CanBeCanceled ? new CancellationTokenSource() : (CancellationTokenSource) new CancellationTokenSource.Linked1CancellationTokenSource(token);
    }

    /// <summary>Creates a <see cref="T:System.Threading.CancellationTokenSource" /> that will be in the canceled state when any of the source tokens in the specified array are in the canceled state.</summary>
    /// <param name="tokens">An array that contains the cancellation token instances to observe.</param>
    /// <exception cref="T:System.ObjectDisposedException">A <see cref="T:System.Threading.CancellationTokenSource" /> associated with one of the source tokens has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="tokens" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="tokens" /> is empty.</exception>
    /// <returns>A <see cref="T:System.Threading.CancellationTokenSource" /> that is linked to the source tokens.</returns>
    public static CancellationTokenSource CreateLinkedTokenSource(
      params CancellationToken[] tokens)
    {
      if (tokens == null)
        throw new ArgumentNullException(nameof (tokens));
      CancellationTokenSource linkedTokenSource;
      switch (tokens.Length)
      {
        case 0:
          throw new ArgumentException(SR.CancellationToken_CreateLinkedToken_TokensIsEmpty);
        case 1:
          linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokens[0]);
          break;
        case 2:
          linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokens[0], tokens[1]);
          break;
        default:
          linkedTokenSource = (CancellationTokenSource) new CancellationTokenSource.LinkedNCancellationTokenSource(tokens);
          break;
      }
      return linkedTokenSource;
    }


    #nullable disable
    private static void Invoke(Delegate d, object state, CancellationTokenSource source)
    {
      if (d is Action<object> action)
        action(state);
      else
        ((Action<object, CancellationToken>) d)(state, new CancellationToken(source));
    }

    private sealed class Linked1CancellationTokenSource : CancellationTokenSource
    {
      private readonly CancellationTokenRegistration _reg1;

      internal Linked1CancellationTokenSource(CancellationToken token1) => this._reg1 = token1.UnsafeRegister(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, (object) this);

      protected override void Dispose(bool disposing)
      {
        if (!disposing || this._disposed)
          return;
        this._reg1.Dispose();
        base.Dispose(disposing);
      }
    }

    private sealed class Linked2CancellationTokenSource : CancellationTokenSource
    {
      private readonly CancellationTokenRegistration _reg1;
      private readonly CancellationTokenRegistration _reg2;

      internal Linked2CancellationTokenSource(CancellationToken token1, CancellationToken token2)
      {
        this._reg1 = token1.UnsafeRegister(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, (object) this);
        this._reg2 = token2.UnsafeRegister(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, (object) this);
      }

      protected override void Dispose(bool disposing)
      {
        if (!disposing || this._disposed)
          return;
        this._reg1.Dispose();
        this._reg2.Dispose();
        base.Dispose(disposing);
      }
    }

    private sealed class LinkedNCancellationTokenSource : CancellationTokenSource
    {
      internal static readonly Action<object> s_linkedTokenCancelDelegate = (Action<object>) (s => ((CancellationTokenSource) s).NotifyCancellation(false));
      private CancellationTokenRegistration[] _linkingRegistrations;

      internal LinkedNCancellationTokenSource(CancellationToken[] tokens)
      {
        this._linkingRegistrations = new CancellationTokenRegistration[tokens.Length];
        for (int index = 0; index < tokens.Length; ++index)
        {
          if (tokens[index].CanBeCanceled)
            this._linkingRegistrations[index] = tokens[index].UnsafeRegister(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, (object) this);
        }
      }

      protected override void Dispose(bool disposing)
      {
        if (!disposing || this._disposed)
          return;
        CancellationTokenRegistration[] linkingRegistrations = this._linkingRegistrations;
        if (linkingRegistrations != null)
        {
          this._linkingRegistrations = (CancellationTokenRegistration[]) null;
          for (int index = 0; index < linkingRegistrations.Length; ++index)
            linkingRegistrations[index].Dispose();
        }
        base.Dispose(disposing);
      }
    }

    internal sealed class Registrations
    {
      public readonly CancellationTokenSource Source;
      public CancellationTokenSource.CallbackNode Callbacks;
      public CancellationTokenSource.CallbackNode FreeNodeList;
      public long NextAvailableId = 1;
      public long ExecutingCallbackId;
      public volatile int ThreadIDExecutingCallbacks = -1;
      private int _lock;

      public Registrations(CancellationTokenSource source) => this.Source = source;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private void Recycle(CancellationTokenSource.CallbackNode node)
      {
        node.Id = 0L;
        node.Callback = (Delegate) null;
        node.CallbackState = (object) null;
        node.ExecutionContext = (ExecutionContext) null;
        node.SynchronizationContext = (SynchronizationContext) null;
        node.Prev = (CancellationTokenSource.CallbackNode) null;
        node.Next = this.FreeNodeList;
        this.FreeNodeList = node;
      }

      public bool Unregister(long id, CancellationTokenSource.CallbackNode node)
      {
        if (id == 0L)
          return false;
        this.EnterLock();
        try
        {
          if (node.Id != id)
            return false;
          if (this.Callbacks == node)
            this.Callbacks = node.Next;
          else
            node.Prev.Next = node.Next;
          if (node.Next != null)
            node.Next.Prev = node.Prev;
          this.Recycle(node);
          return true;
        }
        finally
        {
          this.ExitLock();
        }
      }

      public void UnregisterAll()
      {
        this.EnterLock();
        try
        {
          CancellationTokenSource.CallbackNode node = this.Callbacks;
          this.Callbacks = (CancellationTokenSource.CallbackNode) null;
          CancellationTokenSource.CallbackNode next;
          for (; node != null; node = next)
          {
            next = node.Next;
            this.Recycle(node);
          }
        }
        finally
        {
          this.ExitLock();
        }
      }

      public void WaitForCallbackToComplete(long id)
      {
        SpinWait spinWait = new SpinWait();
        while (Volatile.Read(ref this.ExecutingCallbackId) == id)
          spinWait.SpinOnce();
      }

      public ValueTask WaitForCallbackToCompleteAsync(long id) => Volatile.Read(ref this.ExecutingCallbackId) != id ? new ValueTask() : new ValueTask(Task.Factory.StartNew<Task>((Func<object, Task>) (async s =>
      {
        TupleSlim<CancellationTokenSource.Registrations, long> state = (TupleSlim<CancellationTokenSource.Registrations, long>) s;
        while (Volatile.Read(ref state.Item1.ExecutingCallbackId) == state.Item2)
          await Task.Yield();
        state = (TupleSlim<CancellationTokenSource.Registrations, long>) null;
      }), (object) new TupleSlim<CancellationTokenSource.Registrations, long>(this, id), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap());

      public void EnterLock()
      {
        ref int local = ref this._lock;
        if (Interlocked.Exchange(ref local, 1) == 0)
          return;
        Contention(ref local);

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Contention(ref int value)
        {
          SpinWait spinWait = new SpinWait();
          do
          {
            spinWait.SpinOnce();
          }
          while (Interlocked.Exchange(ref value, 1) == 1);
        }
      }

      public void ExitLock() => Volatile.Write(ref this._lock, 0);
    }

    internal sealed class CallbackNode
    {
      public readonly CancellationTokenSource.Registrations Registrations;
      public CancellationTokenSource.CallbackNode Prev;
      public CancellationTokenSource.CallbackNode Next;
      public long Id;
      public Delegate Callback;
      public object CallbackState;
      public ExecutionContext ExecutionContext;
      public SynchronizationContext SynchronizationContext;

      public CallbackNode(
        CancellationTokenSource.Registrations registrations)
      {
        this.Registrations = registrations;
      }

      public void ExecuteCallback()
      {
        ExecutionContext executionContext = this.ExecutionContext;
        if (executionContext == null)
          CancellationTokenSource.Invoke(this.Callback, this.CallbackState, this.Registrations.Source);
        else
          ExecutionContext.RunInternal(executionContext, (ContextCallback) (s =>
          {
            CancellationTokenSource.CallbackNode callbackNode = (CancellationTokenSource.CallbackNode) s;
            CancellationTokenSource.Invoke(callbackNode.Callback, callbackNode.CallbackState, callbackNode.Registrations.Source);
          }), (object) this);
      }
    }
  }
}
