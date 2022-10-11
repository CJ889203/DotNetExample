// Decompiled with JetBrains decompiler
// Type: System.Threading.Monitor
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides a mechanism that synchronizes access to objects.</summary>
  public static class Monitor
  {
    /// <summary>Acquires an exclusive lock on the specified object.</summary>
    /// <param name="obj">The object on which to acquire the monitor lock.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Enter(object obj);

    /// <summary>Acquires an exclusive lock on the specified object, and atomically sets a value that indicates whether the lock was taken.</summary>
    /// <param name="obj">The object on which to wait.</param>
    /// <param name="lockTaken">The result of the attempt to acquire the lock, passed by reference. The input must be <see langword="false" />. The output is <see langword="true" /> if the lock is acquired; otherwise, the output is <see langword="false" />. The output is set even if an exception occurs during the attempt to acquire the lock.
    /// 
    /// Note   If no exception occurs, the output of this method is always <see langword="true" />.</param>
    /// <exception cref="T:System.ArgumentException">The input to <paramref name="lockTaken" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    public static void Enter(object obj, ref bool lockTaken)
    {
      if (lockTaken)
        Monitor.ThrowLockTakenException();
      Monitor.ReliableEnter(obj, ref lockTaken);
    }

    [DoesNotReturn]
    private static void ThrowLockTakenException() => throw new ArgumentException(SR.Argument_MustBeFalse, "lockTaken");


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ReliableEnter(object obj, ref bool lockTaken);


    #nullable enable
    /// <summary>Releases an exclusive lock on the specified object.</summary>
    /// <param name="obj">The object on which to release the lock.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The current thread does not own the lock for the specified object.</exception>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Exit(object obj);

    /// <summary>Attempts to acquire an exclusive lock on the specified object.</summary>
    /// <param name="obj">The object on which to acquire the lock.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread acquires the lock; otherwise, <see langword="false" />.</returns>
    public static bool TryEnter(object obj)
    {
      bool lockTaken = false;
      Monitor.TryEnter(obj, 0, ref lockTaken);
      return lockTaken;
    }

    /// <summary>Attempts to acquire an exclusive lock on the specified object, and atomically sets a value that indicates whether the lock was taken.</summary>
    /// <param name="obj">The object on which to acquire the lock.</param>
    /// <param name="lockTaken">The result of the attempt to acquire the lock, passed by reference. The input must be <see langword="false" />. The output is <see langword="true" /> if the lock is acquired; otherwise, the output is <see langword="false" />. The output is set even if an exception occurs during the attempt to acquire the lock.</param>
    /// <exception cref="T:System.ArgumentException">The input to <paramref name="lockTaken" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    public static void TryEnter(object obj, ref bool lockTaken)
    {
      if (lockTaken)
        Monitor.ThrowLockTakenException();
      Monitor.ReliableEnterTimeout(obj, 0, ref lockTaken);
    }

    /// <summary>Attempts, for the specified number of milliseconds, to acquire an exclusive lock on the specified object.</summary>
    /// <param name="obj">The object on which to acquire the lock.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is negative, and not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread acquires the lock; otherwise, <see langword="false" />.</returns>
    public static bool TryEnter(object obj, int millisecondsTimeout)
    {
      bool lockTaken = false;
      Monitor.TryEnter(obj, millisecondsTimeout, ref lockTaken);
      return lockTaken;
    }

    /// <summary>Attempts, for the specified number of milliseconds, to acquire an exclusive lock on the specified object, and atomically sets a value that indicates whether the lock was taken.</summary>
    /// <param name="obj">The object on which to acquire the lock.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
    /// <param name="lockTaken">The result of the attempt to acquire the lock, passed by reference. The input must be <see langword="false" />. The output is <see langword="true" /> if the lock is acquired; otherwise, the output is <see langword="false" />. The output is set even if an exception occurs during the attempt to acquire the lock.</param>
    /// <exception cref="T:System.ArgumentException">The input to <paramref name="lockTaken" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is negative, and not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    public static void TryEnter(object obj, int millisecondsTimeout, ref bool lockTaken)
    {
      if (lockTaken)
        Monitor.ThrowLockTakenException();
      Monitor.ReliableEnterTimeout(obj, millisecondsTimeout, ref lockTaken);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ReliableEnterTimeout(object obj, int timeout, ref bool lockTaken);


    #nullable enable
    /// <summary>Determines whether the current thread holds the lock on the specified object.</summary>
    /// <param name="obj">The object to test.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="obj" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread holds the lock on <paramref name="obj" />; otherwise, <see langword="false" />.</returns>
    public static bool IsEntered(object obj) => obj != null ? Monitor.IsEnteredNative(obj) : throw new ArgumentNullException(nameof (obj));


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool IsEnteredNative(object obj);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool ObjWait(int millisecondsTimeout, object obj);


    #nullable enable
    /// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue.</summary>
    /// <param name="obj">The object on which to wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait before the thread enters the ready queue.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object.</exception>
    /// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes <see langword="Wait" /> is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="millisecondsTimeout" /> parameter is negative, and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the lock was reacquired before the specified time elapsed; <see langword="false" /> if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
    [UnsupportedOSPlatform("browser")]
    public static bool Wait(object obj, int millisecondsTimeout)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      return millisecondsTimeout >= -1 ? Monitor.ObjWait(millisecondsTimeout, obj) : throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ObjPulse(object obj);


    #nullable enable
    /// <summary>Notifies a thread in the waiting queue of a change in the locked object's state.</summary>
    /// <param name="obj">The object a thread is waiting for.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object.</exception>
    public static void Pulse(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      Monitor.ObjPulse(obj);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ObjPulseAll(object obj);


    #nullable enable
    /// <summary>Notifies all waiting threads of a change in the object's state.</summary>
    /// <param name="obj">The object that sends the pulse.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object.</exception>
    public static void PulseAll(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      Monitor.ObjPulseAll(obj);
    }

    /// <summary>Gets the number of times there was contention when trying to take the monitor's lock.</summary>
    /// <returns>The number of times there was contention when trying to take the monitor's lock.</returns>
    public static long LockContentionCount => Monitor.GetLockContentionCount();

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern long GetLockContentionCount();

    /// <summary>Attempts, for the specified amount of time, to acquire an exclusive lock on the specified object.</summary>
    /// <param name="obj">The object on which to acquire the lock.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> representing the amount of time to wait for the lock. A value of -1 millisecond specifies an infinite wait.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> in milliseconds is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1 millisecond), or is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread acquires the lock; otherwise, <see langword="false" />.</returns>
    public static bool TryEnter(object obj, TimeSpan timeout) => Monitor.TryEnter(obj, WaitHandle.ToTimeoutMilliseconds(timeout));

    /// <summary>Attempts, for the specified amount of time, to acquire an exclusive lock on the specified object, and atomically sets a value that indicates whether the lock was taken.</summary>
    /// <param name="obj">The object on which to acquire the lock.</param>
    /// <param name="timeout">The amount of time to wait for the lock. A value of -1 millisecond specifies an infinite wait.</param>
    /// <param name="lockTaken">The result of the attempt to acquire the lock, passed by reference. The input must be <see langword="false" />. The output is <see langword="true" /> if the lock is acquired; otherwise, the output is <see langword="false" />. The output is set even if an exception occurs during the attempt to acquire the lock.</param>
    /// <exception cref="T:System.ArgumentException">The input to <paramref name="lockTaken" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> in milliseconds is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1 millisecond), or is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    public static void TryEnter(object obj, TimeSpan timeout, ref bool lockTaken) => Monitor.TryEnter(obj, WaitHandle.ToTimeoutMilliseconds(timeout), ref lockTaken);

    /// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue.</summary>
    /// <param name="obj">The object on which to wait.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> representing the amount of time to wait before the thread enters the ready queue.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object.</exception>
    /// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes <see langword="Wait" /> is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="timeout" /> parameter in milliseconds is negative and does not represent <see cref="F:System.Threading.Timeout.Infinite" /> (-1 millisecond), or is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the lock was reacquired before the specified time elapsed; <see langword="false" /> if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
    [UnsupportedOSPlatform("browser")]
    public static bool Wait(object obj, TimeSpan timeout) => Monitor.Wait(obj, WaitHandle.ToTimeoutMilliseconds(timeout));

    /// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock.</summary>
    /// <param name="obj">The object on which to wait.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The calling thread does not own the lock for the specified object.</exception>
    /// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes <see langword="Wait" /> is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method.</exception>
    /// <returns>
    /// <see langword="true" /> if the call returned because the caller reacquired the lock for the specified object. This method does not return if the lock is not reacquired.</returns>
    [UnsupportedOSPlatform("browser")]
    public static bool Wait(object obj) => Monitor.Wait(obj, -1);

    /// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue. This method also specifies whether the synchronization domain for the context (if in a synchronized context) is exited before the wait and reacquired afterward.</summary>
    /// <param name="obj">The object on which to wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait before the thread enters the ready queue.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit and reacquire the synchronization domain for the context (if in a synchronized context) before the wait; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">
    /// <see langword="Wait" /> is not invoked from within a synchronized block of code.</exception>
    /// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes <see langword="Wait" /> is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="millisecondsTimeout" /> parameter is negative, and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the lock was reacquired before the specified time elapsed; <see langword="false" /> if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
    [UnsupportedOSPlatform("browser")]
    public static bool Wait(object obj, int millisecondsTimeout, bool exitContext) => Monitor.Wait(obj, millisecondsTimeout);

    /// <summary>Releases the lock on an object and blocks the current thread until it reacquires the lock. If the specified time-out interval elapses, the thread enters the ready queue. Optionally exits the synchronization domain for the synchronized context before the wait and reacquires the domain afterward.</summary>
    /// <param name="obj">The object on which to wait.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> representing the amount of time to wait before the thread enters the ready queue.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit and reacquire the synchronization domain for the context (if in a synchronized context) before the wait; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.SynchronizationLockException">
    /// <see langword="Wait" /> is not invoked from within a synchronized block of code.</exception>
    /// <exception cref="T:System.Threading.ThreadInterruptedException">The thread that invokes Wait is later interrupted from the waiting state. This happens when another thread calls this thread's <see cref="M:System.Threading.Thread.Interrupt" /> method.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="timeout" /> parameter is negative and does not represent <see cref="F:System.Threading.Timeout.Infinite" /> (-1 millisecond), or is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the lock was reacquired before the specified time elapsed; <see langword="false" /> if the lock was reacquired after the specified time elapsed. The method does not return until the lock is reacquired.</returns>
    [UnsupportedOSPlatform("browser")]
    public static bool Wait(object obj, TimeSpan timeout, bool exitContext) => Monitor.Wait(obj, WaitHandle.ToTimeoutMilliseconds(timeout));
  }
}
