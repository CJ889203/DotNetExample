// Decompiled with JetBrains decompiler
// Type: System.Threading.ManualResetEventSlim
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents a thread synchronization event that, when signaled, must be reset manually. This class is a lightweight alternative to <see cref="T:System.Threading.ManualResetEvent" />.</summary>
  [DebuggerDisplay("Set = {IsSet}")]
  public class ManualResetEventSlim : IDisposable
  {

    #nullable disable
    private volatile object m_lock;
    private volatile ManualResetEvent m_eventObj;
    private volatile int m_combinedState;
    private static readonly Action<object> s_cancellationTokenCallback = new Action<object>(ManualResetEventSlim.CancellationTokenCallback);


    #nullable enable
    /// <summary>Gets the underlying <see cref="T:System.Threading.WaitHandle" /> object for this <see cref="T:System.Threading.ManualResetEventSlim" />.</summary>
    /// <returns>The underlying <see cref="T:System.Threading.WaitHandle" /> event object for this <see cref="T:System.Threading.ManualResetEventSlim" />.</returns>
    public WaitHandle WaitHandle
    {
      get
      {
        this.ThrowIfDisposed();
        if (this.m_eventObj == null)
          this.LazyInitializeEvent();
        return (WaitHandle) this.m_eventObj;
      }
    }

    /// <summary>Gets whether the event is set.</summary>
    /// <returns>true if the event is set; otherwise, false.</returns>
    public bool IsSet
    {
      get => ManualResetEventSlim.ExtractStatePortion(this.m_combinedState, int.MinValue) != 0;
      private set => this.UpdateStateAtomically((value ? 1 : 0) << 31, int.MinValue);
    }

    /// <summary>Gets the number of spin waits that will occur before falling back to a kernel-based wait operation.</summary>
    /// <returns>Returns the number of spin waits that will occur before falling back to a kernel-based wait operation.</returns>
    public int SpinCount
    {
      get => ManualResetEventSlim.ExtractStatePortionAndShiftRight(this.m_combinedState, 1073217536, 19);
      private set => this.m_combinedState = this.m_combinedState & -1073217537 | value << 19;
    }

    private int Waiters
    {
      get => ManualResetEventSlim.ExtractStatePortionAndShiftRight(this.m_combinedState, 524287, 0);
      set
      {
        if (value >= 524287)
          throw new InvalidOperationException(SR.Format(SR.ManualResetEventSlim_ctor_TooManyWaiters, (object) 524287));
        this.UpdateStateAtomically(value, 524287);
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ManualResetEventSlim" /> class with an initial state of nonsignaled.</summary>
    public ManualResetEventSlim()
      : this(false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ManualResetEventSlim" /> class with a Boolean value indicating whether to set the initial state to signaled.</summary>
    /// <param name="initialState">true to set the initial state signaled; false to set the initial state to nonsignaled.</param>
    public ManualResetEventSlim(bool initialState) => this.Initialize(initialState, SpinWait.SpinCountforSpinBeforeWait);

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ManualResetEventSlim" /> class with a Boolean value indicating whether to set the initial state to signaled and a specified spin count.</summary>
    /// <param name="initialState">true to set the initial state to signaled; false to set the initial state to nonsignaled.</param>
    /// <param name="spinCount">The number of spin waits that will occur before falling back to a kernel-based wait operation.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="spinCount" /> is less than 0 or greater than the maximum allowed value.</exception>
    public ManualResetEventSlim(bool initialState, int spinCount)
    {
      if (spinCount < 0)
        throw new ArgumentOutOfRangeException(nameof (spinCount));
      if (spinCount > 2047)
        throw new ArgumentOutOfRangeException(nameof (spinCount), SR.Format(SR.ManualResetEventSlim_ctor_SpinCountOutOfRange, (object) 2047));
      this.Initialize(initialState, spinCount);
    }

    private void Initialize(bool initialState, int spinCount)
    {
      this.m_combinedState = initialState ? int.MinValue : 0;
      this.SpinCount = Environment.IsSingleProcessor ? 1 : spinCount;
    }

    private void EnsureLockObjectCreated()
    {
      if (this.m_lock != null)
        return;
      Interlocked.CompareExchange(ref this.m_lock, new object(), (object) null);
    }

    private void LazyInitializeEvent()
    {
      bool isSet = this.IsSet;
      ManualResetEvent manualResetEvent = new ManualResetEvent(isSet);
      if (Interlocked.CompareExchange<ManualResetEvent>(ref this.m_eventObj, manualResetEvent, (ManualResetEvent) null) != null)
      {
        manualResetEvent.Dispose();
      }
      else
      {
        if (this.IsSet == isSet)
          return;
        lock (manualResetEvent)
        {
          if (this.m_eventObj != manualResetEvent)
            return;
          manualResetEvent.Set();
        }
      }
    }

    /// <summary>Sets the state of the event to signaled, which allows one or more threads waiting on the event to proceed.</summary>
    public void Set() => this.Set(false);

    private void Set(bool duringCancellation)
    {
      this.IsSet = true;
      if (this.Waiters > 0)
      {
        lock (this.m_lock)
          Monitor.PulseAll(this.m_lock);
      }
      ManualResetEvent eventObj = this.m_eventObj;
      if (eventObj == null || duringCancellation)
        return;
      lock (eventObj)
      {
        if (this.m_eventObj == null)
          return;
        this.m_eventObj.Set();
      }
    }

    /// <summary>Sets the state of the event to nonsignaled, which causes threads to block.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
    public void Reset()
    {
      this.ThrowIfDisposed();
      if (this.m_eventObj != null)
        this.m_eventObj.Reset();
      this.IsSet = false;
    }

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set.</summary>
    /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Wait() => this.Wait(-1, CancellationToken.None);

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a signal, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was
    ///     canceled.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Wait(CancellationToken cancellationToken) => this.Wait(-1, cancellationToken);

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? this.Wait((int) totalMilliseconds, CancellationToken.None) : throw new ArgumentOutOfRangeException(nameof (timeout));
    }

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? this.Wait((int) totalMilliseconds, cancellationToken) : throw new ArgumentOutOfRangeException(nameof (timeout));
    }

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(int millisecondsTimeout) => this.Wait(millisecondsTimeout, CancellationToken.None);

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      this.ThrowIfDisposed();
      cancellationToken.ThrowIfCancellationRequested();
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout));
      if (!this.IsSet)
      {
        if (millisecondsTimeout == 0)
          return false;
        uint startTime = 0;
        bool flag = false;
        int millisecondsTimeout1 = millisecondsTimeout;
        if (millisecondsTimeout != -1)
        {
          startTime = TimeoutHelper.GetTime();
          flag = true;
        }
        int spinCount = this.SpinCount;
        SpinWait spinWait = new SpinWait();
        while (spinWait.Count < spinCount)
        {
          spinWait.SpinOnce(-1);
          if (this.IsSet)
            return true;
          if (spinWait.Count >= 100 && spinWait.Count % 10 == 0)
            cancellationToken.ThrowIfCancellationRequested();
        }
        this.EnsureLockObjectCreated();
        using (cancellationToken.UnsafeRegister(ManualResetEventSlim.s_cancellationTokenCallback, (object) this))
        {
          lock (this.m_lock)
          {
            while (!this.IsSet)
            {
              cancellationToken.ThrowIfCancellationRequested();
              if (flag)
              {
                millisecondsTimeout1 = TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout);
                if (millisecondsTimeout1 <= 0)
                  return false;
              }
              ++this.Waiters;
              if (this.IsSet)
              {
                --this.Waiters;
                return true;
              }
              try
              {
                if (!Monitor.Wait(this.m_lock, millisecondsTimeout1))
                  return false;
              }
              finally
              {
                --this.Waiters;
              }
            }
          }
        }
      }
      return true;
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.ManualResetEventSlim" /> class.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Threading.ManualResetEventSlim" />, and optionally releases the managed resources.</summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if ((this.m_combinedState & 1073741824) != 0)
        return;
      this.m_combinedState |= 1073741824;
      if (!disposing)
        return;
      ManualResetEvent eventObj = this.m_eventObj;
      if (eventObj == null)
        return;
      lock (eventObj)
      {
        eventObj.Dispose();
        this.m_eventObj = (ManualResetEvent) null;
      }
    }

    private void ThrowIfDisposed()
    {
      if ((this.m_combinedState & 1073741824) != 0)
        throw new ObjectDisposedException(SR.ManualResetEventSlim_Disposed);
    }


    #nullable disable
    private static void CancellationTokenCallback(object obj)
    {
      ManualResetEventSlim manualResetEventSlim = (ManualResetEventSlim) obj;
      lock (manualResetEventSlim.m_lock)
        Monitor.PulseAll(manualResetEventSlim.m_lock);
    }

    private void UpdateStateAtomically(int newBits, int updateBitsMask)
    {
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int combinedState = this.m_combinedState;
        if (Interlocked.CompareExchange(ref this.m_combinedState, combinedState & ~updateBitsMask | newBits, combinedState) != combinedState)
          spinWait.SpinOnce(-1);
        else
          break;
      }
    }

    private static int ExtractStatePortionAndShiftRight(
      int state,
      int mask,
      int rightBitShiftCount)
    {
      return (int) ((uint) (state & mask) >> rightBitShiftCount);
    }

    private static int ExtractStatePortion(int state, int mask) => state & mask;
  }
}
