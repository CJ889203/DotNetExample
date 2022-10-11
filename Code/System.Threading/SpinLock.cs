// Decompiled with JetBrains decompiler
// Type: System.Threading.SpinLock
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading
{
  /// <summary>Provides a mutual exclusion lock primitive where a thread trying to acquire the lock waits in a loop repeatedly checking until the lock becomes available.</summary>
  [DebuggerTypeProxy(typeof (SpinLock.SystemThreading_SpinLockDebugView))]
  [DebuggerDisplay("IsHeld = {IsHeld}")]
  public struct SpinLock
  {
    private volatile int _owner;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CompareExchange(
      ref int location,
      int value,
      int comparand,
      ref bool success)
    {
      int num = Interlocked.CompareExchange(ref location, value, comparand);
      success = num == comparand;
      return num;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.SpinLock" /> structure with the option to track thread IDs to improve debugging.</summary>
    /// <param name="enableThreadOwnerTracking">Whether to capture and use thread IDs for debugging purposes.</param>
    public SpinLock(bool enableThreadOwnerTracking)
    {
      this._owner = 0;
      if (enableThreadOwnerTracking)
        return;
      this._owner |= int.MinValue;
    }

    /// <summary>Acquires the lock in a reliable manner, such that even if an exception occurs within the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the lock was acquired.</summary>
    /// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="lockTaken" /> argument must be initialized to false prior to calling Enter.</exception>
    /// <exception cref="T:System.Threading.LockRecursionException">Thread ownership tracking is enabled, and the current thread has already acquired this lock.</exception>
    public void Enter(ref bool lockTaken)
    {
      int owner = this._owner;
      if (!lockTaken && (owner & -2147483647) == int.MinValue && SpinLock.CompareExchange(ref this._owner, owner | 1, owner, ref lockTaken) == owner)
        return;
      this.ContinueTryEnter(-1, ref lockTaken);
    }

    /// <summary>Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the lock was acquired.</summary>
    /// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.</exception>
    /// <exception cref="T:System.Threading.LockRecursionException">Thread ownership tracking is enabled, and the current thread has already acquired this lock.</exception>
    public void TryEnter(ref bool lockTaken)
    {
      int owner = this._owner;
      if ((owner & int.MinValue) == 0 | lockTaken)
        this.ContinueTryEnter(0, ref lockTaken);
      else if ((owner & 1) != 0)
        lockTaken = false;
      else
        SpinLock.CompareExchange(ref this._owner, owner | 1, owner, ref lockTaken);
    }

    /// <summary>Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the lock was acquired.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.</exception>
    /// <exception cref="T:System.Threading.LockRecursionException">Thread ownership tracking is enabled, and the current thread has already acquired this lock.</exception>
    public void TryEnter(TimeSpan timeout, ref bool lockTaken)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), (object) timeout, SR.SpinLock_TryEnter_ArgumentOutOfRange);
      this.TryEnter((int) timeout.TotalMilliseconds, ref lockTaken);
    }

    /// <summary>Attempts to acquire the lock in a reliable manner, such that even if an exception occurs within the method call, <paramref name="lockTaken" /> can be examined reliably to determine whether the lock was acquired.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <param name="lockTaken">True if the lock is acquired; otherwise, false. <paramref name="lockTaken" /> must be initialized to false prior to calling this method.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="lockTaken" /> argument must be initialized to false prior to calling TryEnter.</exception>
    /// <exception cref="T:System.Threading.LockRecursionException">Thread ownership tracking is enabled, and the current thread has already acquired this lock.</exception>
    public void TryEnter(int millisecondsTimeout, ref bool lockTaken)
    {
      int owner = this._owner;
      if (!(millisecondsTimeout < -1 | lockTaken) && (owner & -2147483647) == int.MinValue && SpinLock.CompareExchange(ref this._owner, owner | 1, owner, ref lockTaken) == owner)
        return;
      this.ContinueTryEnter(millisecondsTimeout, ref lockTaken);
    }

    private void ContinueTryEnter(int millisecondsTimeout, ref bool lockTaken)
    {
      if (lockTaken)
      {
        lockTaken = false;
        throw new ArgumentException(SR.SpinLock_TryReliableEnter_ArgumentException);
      }
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), (object) millisecondsTimeout, SR.SpinLock_TryEnter_ArgumentOutOfRange);
      uint startTime = 0;
      if (millisecondsTimeout != -1 && millisecondsTimeout != 0)
        startTime = TimeoutHelper.GetTime();
      if (this.IsThreadOwnerTrackingEnabled)
      {
        this.ContinueTryEnterWithThreadTracking(millisecondsTimeout, startTime, ref lockTaken);
      }
      else
      {
        int num = int.MaxValue;
        int owner1 = this._owner;
        if ((owner1 & 1) == 0)
        {
          if (SpinLock.CompareExchange(ref this._owner, owner1 | 1, owner1, ref lockTaken) == owner1 || millisecondsTimeout == 0)
            return;
        }
        else
        {
          if (millisecondsTimeout == 0)
            return;
          if ((owner1 & 2147483646) != 2147483646)
            num = (Interlocked.Add(ref this._owner, 2) & 2147483646) >> 1;
        }
        SpinWait spinWait = new SpinWait();
        if (num > Environment.ProcessorCount)
          spinWait.Count = 10;
        do
        {
          spinWait.SpinOnce(40);
          int owner2 = this._owner;
          if ((owner2 & 1) == 0 && SpinLock.CompareExchange(ref this._owner, (owner2 & 2147483646) == 0 ? owner2 | 1 : owner2 - 2 | 1, owner2, ref lockTaken) == owner2)
            return;
        }
        while (spinWait.Count % 10 != 0 || millisecondsTimeout == -1 || TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout) > 0);
        this.DecrementWaiters();
      }
    }

    private void DecrementWaiters()
    {
      SpinWait spinWait = new SpinWait();
      while (true)
      {
        int owner = this._owner;
        if ((owner & 2147483646) != 0 && Interlocked.CompareExchange(ref this._owner, owner - 2, owner) != owner)
          spinWait.SpinOnce();
        else
          break;
      }
    }

    private void ContinueTryEnterWithThreadTracking(
      int millisecondsTimeout,
      uint startTime,
      ref bool lockTaken)
    {
      int currentManagedThreadId = Environment.CurrentManagedThreadId;
      if (this._owner == currentManagedThreadId)
        throw new LockRecursionException(SR.SpinLock_TryEnter_LockRecursionException);
      SpinWait spinWait = new SpinWait();
      do
      {
        spinWait.SpinOnce();
        if (this._owner == 0 && SpinLock.CompareExchange(ref this._owner, currentManagedThreadId, 0, ref lockTaken) == 0)
          return;
        switch (millisecondsTimeout)
        {
          case -1:
            continue;
          case 0:
            goto label_4;
          default:
            continue;
        }
      }
      while (!spinWait.NextSpinWillYield || TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout) > 0);
      goto label_8;
label_4:
      return;
label_8:;
    }

    /// <summary>Releases the lock.</summary>
    /// <exception cref="T:System.Threading.SynchronizationLockException">Thread ownership tracking is enabled, and the current thread is not the owner of this lock.</exception>
    public void Exit()
    {
      if ((this._owner & int.MinValue) == 0)
        this.ExitSlowPath(true);
      else
        Interlocked.Decrement(ref this._owner);
    }

    /// <summary>Releases the lock.</summary>
    /// <param name="useMemoryBarrier">A Boolean value that indicates whether a memory fence should be issued in order to immediately publish the exit operation to other threads.</param>
    /// <exception cref="T:System.Threading.SynchronizationLockException">Thread ownership tracking is enabled, and the current thread is not the owner of this lock.</exception>
    public void Exit(bool useMemoryBarrier)
    {
      int owner = this._owner;
      if ((owner & int.MinValue) != 0 & !useMemoryBarrier)
        this._owner = owner & -2;
      else
        this.ExitSlowPath(useMemoryBarrier);
    }

    private void ExitSlowPath(bool useMemoryBarrier)
    {
      bool flag = (this._owner & int.MinValue) == 0;
      if (flag && !this.IsHeldByCurrentThread)
        throw new SynchronizationLockException(SR.SpinLock_Exit_SynchronizationLockException);
      if (useMemoryBarrier)
      {
        if (flag)
          Interlocked.Exchange(ref this._owner, 0);
        else
          Interlocked.Decrement(ref this._owner);
      }
      else if (flag)
        this._owner = 0;
      else
        this._owner &= -2;
    }

    /// <summary>Gets whether the lock is currently held by any thread.</summary>
    /// <returns>true if the lock is currently held by any thread; otherwise false.</returns>
    public bool IsHeld => this.IsThreadOwnerTrackingEnabled ? this._owner != 0 : (this._owner & 1) != 0;

    /// <summary>Gets whether the lock is held by the current thread.</summary>
    /// <exception cref="T:System.InvalidOperationException">Thread ownership tracking is disabled.</exception>
    /// <returns>true if the lock is held by the current thread; otherwise false.</returns>
    public bool IsHeldByCurrentThread
    {
      get
      {
        if (!this.IsThreadOwnerTrackingEnabled)
          throw new InvalidOperationException(SR.SpinLock_IsHeldByCurrentThread);
        return (this._owner & int.MaxValue) == Environment.CurrentManagedThreadId;
      }
    }

    /// <summary>Gets whether thread ownership tracking is enabled for this instance.</summary>
    /// <returns>true if thread ownership tracking is enabled for this instance; otherwise false.</returns>
    public bool IsThreadOwnerTrackingEnabled => (this._owner & int.MinValue) == 0;

    internal sealed class SystemThreading_SpinLockDebugView
    {
      private SpinLock _spinLock;

      public SystemThreading_SpinLockDebugView(SpinLock spinLock) => this._spinLock = spinLock;

      public bool? IsHeldByCurrentThread
      {
        get
        {
          try
          {
            return new bool?(this._spinLock.IsHeldByCurrentThread);
          }
          catch (InvalidOperationException ex)
          {
            return new bool?();
          }
        }
      }

      public int? OwnerThreadID => this._spinLock.IsThreadOwnerTrackingEnabled ? new int?(this._spinLock._owner) : new int?();

      public bool IsHeld => this._spinLock.IsHeld;
    }
  }
}
