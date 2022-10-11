// Decompiled with JetBrains decompiler
// Type: System.Threading.Timer
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides a mechanism for executing a method on a thread pool thread at specified intervals. This class cannot be inherited.</summary>
  [DebuggerDisplay("{DisplayString,nq}")]
  [DebuggerTypeProxy(typeof (TimerQueueTimer.TimerDebuggerTypeProxy))]
  public sealed class Timer : MarshalByRefObject, IDisposable, IAsyncDisposable
  {

    #nullable disable
    internal TimerHolder _timer;


    #nullable enable
    /// <summary>Initializes a new instance of the <see langword="Timer" /> class, using a 32-bit signed integer to specify the time interval.</summary>
    /// <param name="callback">A <see cref="T:System.Threading.TimerCallback" /> delegate representing a method to be executed.</param>
    /// <param name="state">An object containing information to be used by the callback method, or <see langword="null" />.</param>
    /// <param name="dueTime">The amount of time to delay before <paramref name="callback" /> is invoked, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to prevent the timer from starting. Specify zero (0) to start the timer immediately.</param>
    /// <param name="period">The time interval between invocations of <paramref name="callback" />, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="dueTime" /> or <paramref name="period" /> parameter is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="callback" /> parameter is <see langword="null" />.</exception>
    public Timer(TimerCallback callback, object? state, int dueTime, int period)
      : this(callback, state, dueTime, period, true)
    {
    }


    #nullable disable
    internal Timer(
      TimerCallback callback,
      object state,
      int dueTime,
      int period,
      bool flowExecutionContext)
    {
      if (dueTime < -1)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (period < -1)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      this.TimerSetup(callback, state, (uint) dueTime, (uint) period, flowExecutionContext);
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see langword="Timer" /> class, using <see cref="T:System.TimeSpan" /> values to measure time intervals.</summary>
    /// <param name="callback">A delegate representing a method to be executed.</param>
    /// <param name="state">An object containing information to be used by the callback method, or <see langword="null" />.</param>
    /// <param name="dueTime">The amount of time to delay before the <paramref name="callback" /> is invoked. Specify <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" /> to prevent the timer from starting. Specify <see cref="F:System.TimeSpan.Zero" /> to start the timer immediately.</param>
    /// <param name="period">The time interval between invocations of <paramref name="callback" />. Specify <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of milliseconds in the value of <paramref name="dueTime" /> or <paramref name="period" /> is negative and not equal to <see cref="F:System.Threading.Timeout.Infinite" />, or is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="callback" /> parameter is <see langword="null" />.</exception>
    public Timer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
      long totalMilliseconds1 = (long) dueTime.TotalMilliseconds;
      if (totalMilliseconds1 < -1L)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (totalMilliseconds1 > 4294967294L)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_TimeoutTooLarge);
      long totalMilliseconds2 = (long) period.TotalMilliseconds;
      if (totalMilliseconds2 < -1L)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (totalMilliseconds2 > 4294967294L)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_PeriodTooLarge);
      this.TimerSetup(callback, state, (uint) totalMilliseconds1, (uint) totalMilliseconds2);
    }

    /// <summary>Initializes a new instance of the <see langword="Timer" /> class, using 32-bit unsigned integers to measure time intervals.</summary>
    /// <param name="callback">A delegate representing a method to be executed.</param>
    /// <param name="state">An object containing information to be used by the callback method, or <see langword="null" />.</param>
    /// <param name="dueTime">The amount of time to delay before <paramref name="callback" /> is invoked, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to prevent the timer from starting. Specify zero (0) to start the timer immediately.</param>
    /// <param name="period">The time interval between invocations of <paramref name="callback" />, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="dueTime" /> or <paramref name="period" /> parameter is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="callback" /> parameter is <see langword="null" />.</exception>
    [CLSCompliant(false)]
    public Timer(TimerCallback callback, object? state, uint dueTime, uint period) => this.TimerSetup(callback, state, dueTime, period);

    /// <summary>Initializes a new instance of the <see langword="Timer" /> class, using 64-bit signed integers to measure time intervals.</summary>
    /// <param name="callback">A <see cref="T:System.Threading.TimerCallback" /> delegate representing a method to be executed.</param>
    /// <param name="state">An object containing information to be used by the callback method, or <see langword="null" />.</param>
    /// <param name="dueTime">The amount of time to delay before <paramref name="callback" /> is invoked, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to prevent the timer from starting. Specify zero (0) to start the timer immediately.</param>
    /// <param name="period">The time interval between invocations of <paramref name="callback" />, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="dueTime" /> or <paramref name="period" /> parameter is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="dueTime" /> or <paramref name="period" /> parameter is greater than 4294967294.</exception>
    public Timer(TimerCallback callback, object? state, long dueTime, long period)
    {
      if (dueTime < -1L)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (period < -1L)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (dueTime > 4294967294L)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_TimeoutTooLarge);
      if (period > 4294967294L)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_PeriodTooLarge);
      this.TimerSetup(callback, state, (uint) dueTime, (uint) period);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Timer" /> class with an infinite period and an infinite due time, using the newly created <see cref="T:System.Threading.Timer" /> object as the state object.</summary>
    /// <param name="callback">A <see cref="T:System.Threading.TimerCallback" /> delegate representing a method to be executed.</param>
    public Timer(TimerCallback callback) => this.TimerSetup(callback, (object) this, uint.MaxValue, uint.MaxValue);


    #nullable disable
    [MemberNotNull("_timer")]
    private void TimerSetup(
      TimerCallback callback,
      object state,
      uint dueTime,
      uint period,
      bool flowExecutionContext = true)
    {
      if (callback == null)
        throw new ArgumentNullException(nameof (callback));
      this._timer = new TimerHolder(new TimerQueueTimer(callback, state, dueTime, period, flowExecutionContext));
    }

    /// <summary>Changes the start time and the interval between method invocations for a timer, using 32-bit signed integers to measure time intervals.</summary>
    /// <param name="dueTime">The amount of time to delay before the invoking the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to prevent the timer from restarting. Specify zero (0) to restart the timer immediately.</param>
    /// <param name="period">The time interval between invocations of the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Timer" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="dueTime" /> or <paramref name="period" /> parameter is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the timer was successfully updated; otherwise, <see langword="false" />.</returns>
    public bool Change(int dueTime, int period)
    {
      if (dueTime < -1)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (period < -1)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      return this._timer._timer.Change((uint) dueTime, (uint) period);
    }

    /// <summary>Changes the start time and the interval between method invocations for a timer, using <see cref="T:System.TimeSpan" /> values to measure time intervals.</summary>
    /// <param name="dueTime">A <see cref="T:System.TimeSpan" /> representing the amount of time to delay before invoking the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed. Specify <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" /> to prevent the timer from restarting. Specify <see cref="F:System.TimeSpan.Zero" /> to restart the timer immediately.</param>
    /// <param name="period">The time interval between invocations of the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed. Specify <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Timer" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="dueTime" /> or <paramref name="period" /> parameter, in milliseconds, is less than -1.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="dueTime" /> or <paramref name="period" /> parameter, in milliseconds, is greater than 4294967294.</exception>
    /// <returns>
    /// <see langword="true" /> if the timer was successfully updated; otherwise, <see langword="false" />.</returns>
    public bool Change(TimeSpan dueTime, TimeSpan period) => this.Change((long) dueTime.TotalMilliseconds, (long) period.TotalMilliseconds);

    /// <summary>Changes the start time and the interval between method invocations for a timer, using 32-bit unsigned integers to measure time intervals.</summary>
    /// <param name="dueTime">The amount of time to delay before the invoking the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to prevent the timer from restarting. Specify zero (0) to restart the timer immediately.</param>
    /// <param name="period">The time interval between invocations of the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Timer" /> has already been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the timer was successfully updated; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool Change(uint dueTime, uint period) => this._timer._timer.Change(dueTime, period);

    /// <summary>Changes the start time and the interval between method invocations for a timer, using 64-bit signed integers to measure time intervals.</summary>
    /// <param name="dueTime">The amount of time to delay before the invoking the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to prevent the timer from restarting. Specify zero (0) to restart the timer immediately.  This value must be less than or equal to 4294967294.</param>
    /// <param name="period">The time interval between invocations of the callback method specified when the <see cref="T:System.Threading.Timer" /> was constructed, in milliseconds. Specify <see cref="F:System.Threading.Timeout.Infinite" /> to disable periodic signaling.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Timer" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="dueTime" /> or <paramref name="period" /> is less than -1.
    /// 
    /// -or-
    /// 
    /// <paramref name="dueTime" /> or <paramref name="period" /> is greater than 4294967294.</exception>
    /// <returns>
    /// <see langword="true" /> if the timer was successfully updated; otherwise, <see langword="false" />.</returns>
    public bool Change(long dueTime, long period)
    {
      if (dueTime < -1L)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (period < -1L)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (dueTime > 4294967294L)
        throw new ArgumentOutOfRangeException(nameof (dueTime), SR.ArgumentOutOfRange_TimeoutTooLarge);
      if (period > 4294967294L)
        throw new ArgumentOutOfRangeException(nameof (period), SR.ArgumentOutOfRange_PeriodTooLarge);
      return this._timer._timer.Change((uint) dueTime, (uint) period);
    }

    /// <summary>Gets the number of timers that are currently active. An active timer is registered to tick at some point in the future, and has not yet been canceled.</summary>
    /// <returns>The number of timers that are currently active.</returns>
    public static long ActiveCount
    {
      get
      {
        long activeCount = 0;
        foreach (TimerQueue instance in TimerQueue.Instances)
        {
          lock (instance)
            activeCount += instance.ActiveCount;
        }
        return activeCount;
      }
    }


    #nullable enable
    /// <summary>Releases all resources used by the current instance of <see cref="T:System.Threading.Timer" /> and signals when the timer has been disposed of.</summary>
    /// <param name="notifyObject">The <see cref="T:System.Threading.WaitHandle" /> to be signaled when the <see langword="Timer" /> has been disposed of.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="notifyObject" /> parameter is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the function succeeds; otherwise, <see langword="false" />.</returns>
    public bool Dispose(WaitHandle notifyObject) => notifyObject != null ? this._timer.Close(notifyObject) : throw new ArgumentNullException(nameof (notifyObject));

    /// <summary>Releases all resources used by the current instance of <see cref="T:System.Threading.Timer" />.</summary>
    public void Dispose() => this._timer.Close();

    /// <summary>Releases all resources used by the current instance of <see cref="T:System.Threading.Timer" />.</summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.ValueTask" /> that completes when all work associated with the timer has ceased.</returns>
    public ValueTask DisposeAsync() => this._timer.CloseAsync();

    private string DisplayString => this._timer._timer.DisplayString;

    private static IEnumerable<TimerQueueTimer> AllTimers
    {
      get
      {
        List<TimerQueueTimer> allTimers = new List<TimerQueueTimer>();
        foreach (TimerQueue instance in TimerQueue.Instances)
          allTimers.AddRange(instance.GetTimersForDebugger());
        allTimers.Sort((Comparison<TimerQueueTimer>) ((t1, t2) => t1._dueTime.CompareTo(t2._dueTime)));
        return (IEnumerable<TimerQueueTimer>) allTimers;
      }
    }
  }
}
