// Decompiled with JetBrains decompiler
// Type: System.Threading.CountdownEvent
// Assembly: System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3EED92AD-A1EE-4F59-AFCF-58DB2345788A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents a synchronization primitive that is signaled when its count reaches zero.</summary>
  [DebuggerDisplay("Initial Count={InitialCount}, Current Count={CurrentCount}")]
  public class CountdownEvent : IDisposable
  {
    private int _initialCount;
    private volatile int _currentCount;

    #nullable disable
    private readonly ManualResetEventSlim _event;
    private volatile bool _disposed;

    /// <summary>Initializes a new instance of <see cref="T:System.Threading.CountdownEvent" /> class with the specified count.</summary>
    /// <param name="initialCount">The number of signals initially required to set the <see cref="T:System.Threading.CountdownEvent" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="initialCount" /> is less than 0.</exception>
    public CountdownEvent(int initialCount)
    {
      this._initialCount = initialCount >= 0 ? initialCount : throw new ArgumentOutOfRangeException(nameof (initialCount));
      this._currentCount = initialCount;
      this._event = new ManualResetEventSlim();
      if (initialCount != 0)
        return;
      this._event.Set();
    }

    /// <summary>Gets the number of remaining signals required to set the event.</summary>
    /// <returns>The number of remaining signals required to set the event.</returns>
    public int CurrentCount
    {
      get
      {
        int currentCount = this._currentCount;
        return currentCount >= 0 ? currentCount : 0;
      }
    }

    /// <summary>Gets the numbers of signals initially required to set the event.</summary>
    /// <returns>The number of signals initially required to set the event.</returns>
    public int InitialCount => this._initialCount;

    /// <summary>Indicates whether the <see cref="T:System.Threading.CountdownEvent" /> object's current count has reached zero.</summary>
    /// <returns>
    /// <see langword="true" /> if the current count is zero; otherwise, <see langword="false" />.</returns>
    public bool IsSet => this._currentCount <= 0;


    #nullable enable
    /// <summary>Gets a <see cref="T:System.Threading.WaitHandle" /> that is used to wait for the event to be set.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <returns>A <see cref="T:System.Threading.WaitHandle" /> that is used to wait for the event to be set.</returns>
    public WaitHandle WaitHandle
    {
      get
      {
        this.ThrowIfDisposed();
        return this._event.WaitHandle;
      }
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.CountdownEvent" /> class.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Threading.CountdownEvent" />, and optionally releases the managed resources.</summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this._event.Dispose();
      this._disposed = true;
    }

    /// <summary>Registers a signal with the <see cref="T:System.Threading.CountdownEvent" />, decrementing the value of <see cref="P:System.Threading.CountdownEvent.CurrentCount" />.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is already set.</exception>
    /// <returns>
    /// <see langword="true" /> if the signal caused the count to reach zero and the event was set; otherwise, <see langword="false" />.</returns>
    public bool Signal()
    {
      this.ThrowIfDisposed();
      int num = this._currentCount > 0 ? Interlocked.Decrement(ref this._currentCount) : throw new InvalidOperationException(SR.CountdownEvent_Decrement_BelowZero);
      if (num == 0)
      {
        this._event.Set();
        return true;
      }
      if (num < 0)
        throw new InvalidOperationException(SR.CountdownEvent_Decrement_BelowZero);
      return false;
    }

    /// <summary>Registers multiple signals with the <see cref="T:System.Threading.CountdownEvent" />, decrementing the value of <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> by the specified amount.</summary>
    /// <param name="signalCount">The number of signals to register.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="signalCount" /> is less than 1.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is already set. -or- Or <paramref name="signalCount" /> is greater than <see cref="P:System.Threading.CountdownEvent.CurrentCount" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the signals caused the count to reach zero and the event was set; otherwise, <see langword="false" />.</returns>
    public bool Signal(int signalCount)
    {
      if (signalCount <= 0)
        throw new ArgumentOutOfRangeException(nameof (signalCount));
      this.ThrowIfDisposed();
      SpinWait spinWait = new SpinWait();
      int currentCount;
      while (true)
      {
        currentCount = this._currentCount;
        if (currentCount >= signalCount)
        {
          if (Interlocked.CompareExchange(ref this._currentCount, currentCount - signalCount, currentCount) != currentCount)
            spinWait.SpinOnce(-1);
          else
            goto label_7;
        }
        else
          break;
      }
      throw new InvalidOperationException(SR.CountdownEvent_Decrement_BelowZero);
label_7:
      if (currentCount != signalCount)
        return false;
      this._event.Set();
      return true;
    }

    /// <summary>Increments the <see cref="T:System.Threading.CountdownEvent" />'s current count by one.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is already set.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is equal to or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    public void AddCount() => this.AddCount(1);

    /// <summary>Attempts to increment <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> by one.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is equal to <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the increment succeeded; otherwise, false. If <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is already at zero, this method will return <see langword="false" />.</returns>
    public bool TryAddCount() => this.TryAddCount(1);

    /// <summary>Increments the <see cref="T:System.Threading.CountdownEvent" />'s current count by a specified value.</summary>
    /// <param name="signalCount">The value by which to increase <see cref="P:System.Threading.CountdownEvent.CurrentCount" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="signalCount" /> is less than or equal to 0.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is already set.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is equal to or greater than <see cref="F:System.Int32.MaxValue" /> after count is incremented by <paramref name="signalCount" />.</exception>
    public void AddCount(int signalCount)
    {
      if (!this.TryAddCount(signalCount))
        throw new InvalidOperationException(SR.CountdownEvent_Increment_AlreadyZero);
    }

    /// <summary>Attempts to increment <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> by a specified value.</summary>
    /// <param name="signalCount">The value by which to increase <see cref="P:System.Threading.CountdownEvent.CurrentCount" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="signalCount" /> is less than or equal to 0.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> + <paramref name="signalCount" /> is equal to or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the increment succeeded; otherwise, false. If <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> is already at zero this will return <see langword="false" />.</returns>
    public bool TryAddCount(int signalCount)
    {
      if (signalCount <= 0)
        throw new ArgumentOutOfRangeException(nameof (signalCount));
      this.ThrowIfDisposed();
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int currentCount = this._currentCount;
        if (currentCount > 0)
        {
          if (currentCount <= int.MaxValue - signalCount)
          {
            if (Interlocked.CompareExchange(ref this._currentCount, currentCount + signalCount, currentCount) != currentCount)
              spinWait.SpinOnce(-1);
            else
              goto label_9;
          }
          else
            goto label_6;
        }
        else
          break;
      }
      return false;
label_6:
      throw new InvalidOperationException(SR.CountdownEvent_Increment_AlreadyMax);
label_9:
      return true;
    }

    /// <summary>Resets the <see cref="P:System.Threading.CountdownEvent.CurrentCount" /> to the value of <see cref="P:System.Threading.CountdownEvent.InitialCount" />.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    public void Reset() => this.Reset(this._initialCount);

    /// <summary>Resets the <see cref="P:System.Threading.CountdownEvent.InitialCount" /> property to a specified value.</summary>
    /// <param name="count">The number of signals required to set the <see cref="T:System.Threading.CountdownEvent" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="count" /> is less than 0.</exception>
    public void Reset(int count)
    {
      this.ThrowIfDisposed();
      this._currentCount = count >= 0 ? count : throw new ArgumentOutOfRangeException(nameof (count));
      this._initialCount = count;
      if (count == 0)
        this._event.Set();
      else
        this._event.Reset();
    }

    /// <summary>Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Wait() => this.Wait(-1, CancellationToken.None);

    /// <summary>Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. -or- The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Wait(CancellationToken cancellationToken) => this.Wait(-1, cancellationToken);

    /// <summary>Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the timeout.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? this.Wait((int) totalMilliseconds, CancellationToken.None) : throw new ArgumentOutOfRangeException(nameof (timeout));
    }

    /// <summary>Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the timeout, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. -or- The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? this.Wait((int) totalMilliseconds, cancellationToken) : throw new ArgumentOutOfRangeException(nameof (timeout));
    }

    /// <summary>Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using a 32-bit signed integer to measure the timeout.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(int millisecondsTimeout) => this.Wait(millisecondsTimeout, CancellationToken.None);

    /// <summary>Blocks the current thread until the <see cref="T:System.Threading.CountdownEvent" /> is set, using a 32-bit signed integer to measure the timeout, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. -or- The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Threading.CountdownEvent" /> was set; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout));
      this.ThrowIfDisposed();
      cancellationToken.ThrowIfCancellationRequested();
      bool flag = this._event.IsSet;
      if (!flag)
        flag = this._event.Wait(millisecondsTimeout, cancellationToken);
      return flag;
    }

    private void ThrowIfDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (CountdownEvent));
    }
  }
}
