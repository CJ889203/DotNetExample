// Decompiled with JetBrains decompiler
// Type: System.Threading.WaitHandle
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System.Threading
{
  /// <summary>Encapsulates operating system-specific objects that wait for exclusive access to shared resources.</summary>
  public abstract class WaitHandle : MarshalByRefObject, IDisposable
  {
    internal const int MaxWaitHandles = 64;
    /// <summary>Represents an invalid native operating system handle. This field is read-only.</summary>
    protected static readonly IntPtr InvalidHandle = new IntPtr(-1);

    #nullable disable
    private SafeWaitHandle _waitHandle;
    [ThreadStatic]
    private static SafeWaitHandle[] t_safeWaitHandlesForRent;
    internal const int WaitSuccess = 0;
    internal const int WaitAbandoned = 128;
    /// <summary>Indicates that a <see cref="M:System.Threading.WaitHandle.WaitAny(System.Threading.WaitHandle[],System.Int32,System.Boolean)" /> operation timed out before any of the wait handles were signaled. This field is constant.</summary>
    public const int WaitTimeout = 258;

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int WaitOneCore(IntPtr waitHandle, int millisecondsTimeout);

    internal static unsafe int WaitMultipleIgnoringSyncContext(
      Span<IntPtr> waitHandles,
      bool waitAll,
      int millisecondsTimeout)
    {
      fixed (IntPtr* waitHandles1 = &MemoryMarshal.GetReference<IntPtr>(waitHandles))
        return WaitHandle.WaitMultipleIgnoringSyncContext(waitHandles1, waitHandles.Length, waitAll, millisecondsTimeout);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern unsafe int WaitMultipleIgnoringSyncContext(
      IntPtr* waitHandles,
      int numHandles,
      bool waitAll,
      int millisecondsTimeout);

    private static int SignalAndWaitCore(
      IntPtr waitHandleToSignal,
      IntPtr waitHandleToWaitOn,
      int millisecondsTimeout)
    {
      int num = WaitHandle.SignalAndWaitNative(waitHandleToSignal, waitHandleToWaitOn, millisecondsTimeout);
      return num != 298 ? num : throw new InvalidOperationException(SR.Threading_WaitHandleTooManyPosts);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int SignalAndWaitNative(
      IntPtr waitHandleToSignal,
      IntPtr waitHandleToWaitOn,
      int millisecondsTimeout);

    /// <summary>Gets or sets the native operating system handle.</summary>
    /// <returns>An <see cref="T:System.IntPtr" /> representing the native operating system handle. The default is the value of the <see cref="F:System.Threading.WaitHandle.InvalidHandle" /> field.</returns>
    [Obsolete("WaitHandleHandle has been deprecated. Use the SafeWaitHandle property instead.")]
    public virtual IntPtr Handle
    {
      get => this._waitHandle != null ? this._waitHandle.DangerousGetHandle() : WaitHandle.InvalidHandle;
      set
      {
        if (value == WaitHandle.InvalidHandle)
        {
          if (this._waitHandle == null)
            return;
          this._waitHandle.SetHandleAsInvalid();
          this._waitHandle = (SafeWaitHandle) null;
        }
        else
          this._waitHandle = new SafeWaitHandle(value, true);
      }
    }


    #nullable enable
    /// <summary>Gets or sets the native operating system handle.</summary>
    /// <returns>A <see cref="T:Microsoft.Win32.SafeHandles.SafeWaitHandle" /> representing the native operating system handle.</returns>
    public SafeWaitHandle SafeWaitHandle
    {
      get => this._waitHandle ?? (this._waitHandle = new SafeWaitHandle(WaitHandle.InvalidHandle, false));
      [param: AllowNull] set => this._waitHandle = value;
    }

    internal static int ToTimeoutMilliseconds(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L)
        throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      return totalMilliseconds <= (long) int.MaxValue ? (int) totalMilliseconds : throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_LessEqualToIntegerMaxVal);
    }

    /// <summary>Releases all resources held by the current <see cref="T:System.Threading.WaitHandle" />.</summary>
    public virtual void Close() => this.Dispose();

    /// <summary>When overridden in a derived class, releases the unmanaged resources used by the <see cref="T:System.Threading.WaitHandle" />, and optionally releases the managed resources.</summary>
    /// <param name="explicitDisposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool explicitDisposing) => this._waitHandle?.Close();

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.WaitHandle" /> class.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.WaitHandle" /> receives a signal, using a 32-bit signed integer to specify the time interval in milliseconds.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> if the current instance receives a signal; otherwise, <see langword="false" />.</returns>
    public virtual bool WaitOne(int millisecondsTimeout) => millisecondsTimeout >= -1 ? this.WaitOneNoCheck(millisecondsTimeout) : throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);

    private bool WaitOneNoCheck(int millisecondsTimeout)
    {
      SafeWaitHandle safeWaitHandle = this._waitHandle ?? throw new ObjectDisposedException((string) null, SR.ObjectDisposed_Generic);
      bool success = false;
      try
      {
        safeWaitHandle.DangerousAddRef(ref success);
        SynchronizationContext current = SynchronizationContext.Current;
        int num;
        if (current != null && current.IsWaitNotificationRequired())
          num = current.Wait(new IntPtr[1]
          {
            safeWaitHandle.DangerousGetHandle()
          }, false, millisecondsTimeout);
        else
          num = WaitHandle.WaitOneCore(safeWaitHandle.DangerousGetHandle(), millisecondsTimeout);
        if (num == 128)
          throw new AbandonedMutexException();
        return num != 258;
      }
      finally
      {
        if (success)
          safeWaitHandle.DangerousRelease();
      }
    }


    #nullable disable
    private static SafeWaitHandle[] RentSafeWaitHandleArray(int capacity)
    {
      SafeWaitHandle[] safeWaitHandleArray = WaitHandle.t_safeWaitHandlesForRent;
      WaitHandle.t_safeWaitHandlesForRent = (SafeWaitHandle[]) null;
      int num = safeWaitHandleArray != null ? safeWaitHandleArray.Length : 0;
      if (num < capacity)
        safeWaitHandleArray = new SafeWaitHandle[Math.Max(capacity, Math.Min(64, 2 * num))];
      return safeWaitHandleArray;
    }

    private static void ReturnSafeWaitHandleArray(SafeWaitHandle[] safeWaitHandles) => WaitHandle.t_safeWaitHandlesForRent = safeWaitHandles;

    private static void ObtainSafeWaitHandles(
      ReadOnlySpan<WaitHandle> waitHandles,
      Span<SafeWaitHandle> safeWaitHandles,
      Span<IntPtr> unsafeWaitHandles)
    {
      bool success = true;
      SafeWaitHandle safeWaitHandle1 = (SafeWaitHandle) null;
      try
      {
        for (int index = 0; index < waitHandles.Length; ++index)
        {
          SafeWaitHandle safeWaitHandle2 = (waitHandles[index] ?? throw new ArgumentNullException("waitHandles[" + index.ToString() + "]", SR.ArgumentNull_ArrayElement))._waitHandle ?? throw new ObjectDisposedException((string) null, SR.ObjectDisposed_Generic);
          safeWaitHandle1 = safeWaitHandle2;
          success = false;
          safeWaitHandle2.DangerousAddRef(ref success);
          safeWaitHandles[index] = safeWaitHandle2;
          unsafeWaitHandles[index] = safeWaitHandle2.DangerousGetHandle();
        }
      }
      catch
      {
        for (int index = 0; index < waitHandles.Length; ++index)
        {
          SafeWaitHandle safeWaitHandle3 = safeWaitHandles[index];
          if (safeWaitHandle3 != null)
          {
            safeWaitHandle3.DangerousRelease();
            safeWaitHandles[index] = (SafeWaitHandle) null;
            if (safeWaitHandle3 == safeWaitHandle1)
            {
              safeWaitHandle1 = (SafeWaitHandle) null;
              success = true;
            }
          }
          else
            break;
        }
        if (!success)
          safeWaitHandle1.DangerousRelease();
        throw;
      }
    }

    private static int WaitMultiple(
      WaitHandle[] waitHandles,
      bool waitAll,
      int millisecondsTimeout)
    {
      if (waitHandles == null)
        throw new ArgumentNullException(nameof (waitHandles), SR.ArgumentNull_Waithandles);
      return WaitHandle.WaitMultiple(new ReadOnlySpan<WaitHandle>(waitHandles), waitAll, millisecondsTimeout);
    }

    private static unsafe int WaitMultiple(
      ReadOnlySpan<WaitHandle> waitHandles,
      bool waitAll,
      int millisecondsTimeout)
    {
      if (waitHandles.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyWaithandleArray, nameof (waitHandles));
      if (waitHandles.Length > 64)
        throw new NotSupportedException(SR.NotSupported_MaxWaitHandles);
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      SynchronizationContext current = SynchronizationContext.Current;
      bool flag = current != null && current.IsWaitNotificationRequired();
      SafeWaitHandle[] safeWaitHandles = WaitHandle.RentSafeWaitHandleArray(waitHandles.Length);
      try
      {
        int num1;
        if (flag)
        {
          IntPtr[] numArray = new IntPtr[waitHandles.Length];
          WaitHandle.ObtainSafeWaitHandles(waitHandles, (Span<SafeWaitHandle>) safeWaitHandles, (Span<IntPtr>) numArray);
          num1 = current.Wait(numArray, waitAll, millisecondsTimeout);
        }
        else
        {
          int length = waitHandles.Length;
          // ISSUE: untyped stack allocation
          Span<IntPtr> span = new Span<IntPtr>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length) * sizeof (IntPtr))), length);
          WaitHandle.ObtainSafeWaitHandles(waitHandles, (Span<SafeWaitHandle>) safeWaitHandles, span);
          num1 = WaitHandle.WaitMultipleIgnoringSyncContext(span, waitAll, millisecondsTimeout);
        }
        if (num1 < 128 || num1 >= 128 + waitHandles.Length)
          return num1;
        if (waitAll)
          throw new AbandonedMutexException();
        int num2 = num1 - 128;
        throw new AbandonedMutexException(num2, waitHandles[num2]);
      }
      finally
      {
        for (int index = 0; index < waitHandles.Length; ++index)
        {
          SafeWaitHandle safeWaitHandle = safeWaitHandles[index];
          if (safeWaitHandle != null)
          {
            safeWaitHandle.DangerousRelease();
            safeWaitHandles[index] = (SafeWaitHandle) null;
          }
        }
        WaitHandle.ReturnSafeWaitHandleArray(safeWaitHandles);
      }
    }

    private static unsafe int WaitAnyMultiple(
      ReadOnlySpan<SafeWaitHandle> safeWaitHandles,
      int millisecondsTimeout)
    {
      SynchronizationContext current = SynchronizationContext.Current;
      int num;
      if (current != null && current.IsWaitNotificationRequired())
      {
        IntPtr[] waitHandles = new IntPtr[safeWaitHandles.Length];
        for (int index = 0; index < safeWaitHandles.Length; ++index)
          waitHandles[index] = safeWaitHandles[index].DangerousGetHandle();
        num = current.Wait(waitHandles, false, millisecondsTimeout);
      }
      else
      {
        int length = safeWaitHandles.Length;
        // ISSUE: untyped stack allocation
        Span<IntPtr> waitHandles = new Span<IntPtr>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length) * sizeof (IntPtr))), length);
        for (int index = 0; index < safeWaitHandles.Length; ++index)
          waitHandles[index] = safeWaitHandles[index].DangerousGetHandle();
        num = WaitHandle.WaitMultipleIgnoringSyncContext(waitHandles, false, millisecondsTimeout);
      }
      return num;
    }

    private static bool SignalAndWait(
      WaitHandle toSignal,
      WaitHandle toWaitOn,
      int millisecondsTimeout)
    {
      if (toSignal == null)
        throw new ArgumentNullException(nameof (toSignal));
      if (toWaitOn == null)
        throw new ArgumentNullException(nameof (toWaitOn));
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      SafeWaitHandle waitHandle1 = toSignal._waitHandle;
      SafeWaitHandle waitHandle2 = toWaitOn._waitHandle;
      if (waitHandle1 == null || waitHandle2 == null)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_Generic);
      bool success1 = false;
      bool success2 = false;
      try
      {
        waitHandle1.DangerousAddRef(ref success1);
        waitHandle2.DangerousAddRef(ref success2);
        int num = WaitHandle.SignalAndWaitCore(waitHandle1.DangerousGetHandle(), waitHandle2.DangerousGetHandle(), millisecondsTimeout);
        if (num == 128)
          throw new AbandonedMutexException();
        return num != 258;
      }
      finally
      {
        if (success2)
          waitHandle2.DangerousRelease();
        if (success1)
          waitHandle1.DangerousRelease();
      }
    }

    /// <summary>Blocks the current thread until the current instance receives a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> if the current instance receives a signal; otherwise, <see langword="false" />.</returns>
    public virtual bool WaitOne(TimeSpan timeout) => this.WaitOneNoCheck(WaitHandle.ToTimeoutMilliseconds(timeout));

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.WaitHandle" /> receives a signal.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> if the current instance receives a signal. If the current instance is never signaled, <see cref="M:System.Threading.WaitHandle.WaitOne" /> never returns.</returns>
    public virtual bool WaitOne() => this.WaitOneNoCheck(-1);

    /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.WaitHandle" /> receives a signal, using a 32-bit signed integer to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> if the current instance receives a signal; otherwise, <see langword="false" />.</returns>
    public virtual bool WaitOne(int millisecondsTimeout, bool exitContext) => this.WaitOne(millisecondsTimeout);

    /// <summary>Blocks the current thread until the current instance receives a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> if the current instance receives a signal; otherwise, <see langword="false" />.</returns>
    public virtual bool WaitOne(TimeSpan timeout, bool exitContext) => this.WaitOneNoCheck(WaitHandle.ToTimeoutMilliseconds(timeout));


    #nullable enable
    /// <summary>Waits for all the elements in the specified array to receive a signal, using an <see cref="T:System.Int32" /> value to specify the time interval.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object (duplicates).</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="waitHandles" /> is an array with no elements.</exception>
    /// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates.
    /// 
    /// Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.ArgumentException" />, instead.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.
    /// 
    /// -or-
    /// 
    /// The current thread is in <see cref="F:System.Threading.ApartmentState.STA" /> state, and <paramref name="waitHandles" /> contains more than one element.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> when every element in <paramref name="waitHandles" /> has received a signal; otherwise, <see langword="false" />.</returns>
    public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout) => WaitHandle.WaitMultiple(waitHandles, true, millisecondsTimeout) != 258;

    /// <summary>Waits for all the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> value to specify the time interval.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds, to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="waitHandles" /> is an array with no elements.</exception>
    /// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates.
    /// 
    /// Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.ArgumentException" />, instead.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.
    /// 
    /// -or-
    /// 
    /// The current thread is in <see cref="F:System.Threading.ApartmentState.STA" /> state, and <paramref name="waitHandles" /> contains more than one element.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait terminated because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> when every element in <paramref name="waitHandles" /> has received a signal; otherwise, <see langword="false" />.</returns>
    public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout) => WaitHandle.WaitMultiple(waitHandles, true, WaitHandle.ToTimeoutMilliseconds(timeout)) != 258;

    /// <summary>Waits for all the elements in the specified array to receive a signal.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />. -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array are <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 2.0 or later.</exception>
    /// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates.
    /// 
    /// Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.ArgumentException" />, instead.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.
    /// 
    /// -or-
    /// 
    /// The current thread is in <see cref="F:System.Threading.ApartmentState.STA" /> state, and <paramref name="waitHandles" /> contains more than one element.</exception>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 1.0 or 1.1.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait terminated because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> when every element in <paramref name="waitHandles" /> has received a signal; otherwise the method never returns.</returns>
    public static bool WaitAll(WaitHandle[] waitHandles) => WaitHandle.WaitMultiple(waitHandles, true, -1) != 258;

    /// <summary>Waits for all the elements in the specified array to receive a signal, using an <see cref="T:System.Int32" /> value to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object (duplicates).</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 2.0 or later.</exception>
    /// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.
    /// 
    /// -or-
    /// 
    /// The current thread is in <see cref="F:System.Threading.ApartmentState.STA" /> state, and <paramref name="waitHandles" /> contains more than one element.</exception>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 1.0 or 1.1.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> when every element in <paramref name="waitHandles" /> has received a signal; otherwise, <see langword="false" />.</returns>
    public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext) => WaitHandle.WaitMultiple(waitHandles, true, millisecondsTimeout) != 258;

    /// <summary>Waits for all the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> value to specify the time interval, and specifying whether to exit the synchronization domain before the wait.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds, to wait indefinitely.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 2.0 or later.</exception>
    /// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.STAThreadAttribute" /> attribute is applied to the thread procedure for the current thread, and <paramref name="waitHandles" /> contains more than one element.</exception>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 1.0 or 1.1.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait terminated because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>
    /// <see langword="true" /> when every element in <paramref name="waitHandles" /> has received a signal; otherwise <see langword="false" />.</returns>
    public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext) => WaitHandle.WaitMultiple(waitHandles, true, WaitHandle.ToTimeoutMilliseconds(timeout)) != 258;

    /// <summary>Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to specify the time interval.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="waitHandles" /> is an array with no elements.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="millisecondsTimeout" /> has passed.</returns>
    public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout) => WaitHandle.WaitMultiple(waitHandles, false, millisecondsTimeout);


    #nullable disable
    internal static int WaitAny(
      ReadOnlySpan<SafeWaitHandle> safeWaitHandles,
      int millisecondsTimeout)
    {
      return WaitHandle.WaitAnyMultiple(safeWaitHandles, millisecondsTimeout);
    }


    #nullable enable
    /// <summary>Waits for any of the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="waitHandles" /> is an array with no elements.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="timeout" /> has passed.</returns>
    public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout) => WaitHandle.WaitMultiple(waitHandles, false, WaitHandle.ToTimeoutMilliseconds(timeout));

    /// <summary>Waits for any of the elements in the specified array to receive a signal.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.</exception>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 1.0 or 1.1.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 2.0 or later.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>The array index of the object that satisfied the wait.</returns>
    public static int WaitAny(WaitHandle[] waitHandles) => WaitHandle.WaitMultiple(waitHandles, false, -1);

    /// <summary>Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to specify the time interval, and specifying whether to exit the synchronization domain before the wait.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.</exception>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 1.0 or 1.1.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 2.0 or later.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="millisecondsTimeout" /> has passed.</returns>
    public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext) => WaitHandle.WaitMultiple(waitHandles, false, millisecondsTimeout);

    /// <summary>Waits for any of the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
    /// <param name="waitHandles">A <see langword="WaitHandle" /> array containing the objects for which the current instance will wait.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// One or more of the objects in the <paramref name="waitHandles" /> array is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.</exception>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 1.0 or 1.1.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 2.0 or later.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
    /// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="timeout" /> has passed.</returns>
    public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext) => WaitHandle.WaitMultiple(waitHandles, false, WaitHandle.ToTimeoutMilliseconds(timeout));

    /// <summary>Signals one <see cref="T:System.Threading.WaitHandle" /> and waits on another.</summary>
    /// <param name="toSignal">The <see cref="T:System.Threading.WaitHandle" /> to signal.</param>
    /// <param name="toWaitOn">The <see cref="T:System.Threading.WaitHandle" /> to wait on.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="toSignal" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="toWaitOn" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The method was called on a thread in <see cref="F:System.Threading.ApartmentState.STA" /> state.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="toSignal" /> is a semaphore, and it already has a full count.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <returns>
    /// <see langword="true" /> if both the signal and the wait complete successfully; if the wait does not complete, the method does not return.</returns>
    public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn) => WaitHandle.SignalAndWait(toSignal, toWaitOn, -1);

    /// <summary>Signals one <see cref="T:System.Threading.WaitHandle" /> and waits on another, specifying the time-out interval as a <see cref="T:System.TimeSpan" /> and specifying whether to exit the synchronization domain for the context before entering the wait.</summary>
    /// <param name="toSignal">The <see cref="T:System.Threading.WaitHandle" /> to signal.</param>
    /// <param name="toWaitOn">The <see cref="T:System.Threading.WaitHandle" /> to wait on.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the interval to wait. If the value is -1, the wait is infinite.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="toSignal" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="toWaitOn" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The method was called on a thread in <see cref="F:System.Threading.ApartmentState.STA" /> state.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="toSignal" /> is a semaphore, and it already has a full count.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="timeout" /> evaluates to a negative number of milliseconds other than -1.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <returns>
    /// <see langword="true" /> if both the signal and the wait completed successfully, or <see langword="false" /> if the signal completed but the wait timed out.</returns>
    public static bool SignalAndWait(
      WaitHandle toSignal,
      WaitHandle toWaitOn,
      TimeSpan timeout,
      bool exitContext)
    {
      return WaitHandle.SignalAndWait(toSignal, toWaitOn, WaitHandle.ToTimeoutMilliseconds(timeout));
    }

    /// <summary>Signals one <see cref="T:System.Threading.WaitHandle" /> and waits on another, specifying a time-out interval as a 32-bit signed integer and specifying whether to exit the synchronization domain for the context before entering the wait.</summary>
    /// <param name="toSignal">The <see cref="T:System.Threading.WaitHandle" /> to signal.</param>
    /// <param name="toWaitOn">The <see cref="T:System.Threading.WaitHandle" /> to wait on.</param>
    /// <param name="millisecondsTimeout">An integer that represents the interval to wait. If the value is <see cref="F:System.Threading.Timeout.Infinite" />, that is, -1, the wait is infinite.</param>
    /// <param name="exitContext">
    /// <see langword="true" /> to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="toSignal" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="toWaitOn" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The method is called on a thread in <see cref="F:System.Threading.ApartmentState.STA" /> state.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.WaitHandle" /> cannot be signaled because it would exceed its maximum count.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex.</exception>
    /// <returns>
    /// <see langword="true" /> if both the signal and the wait completed successfully, or <see langword="false" /> if the signal completed but the wait timed out.</returns>
    public static bool SignalAndWait(
      WaitHandle toSignal,
      WaitHandle toWaitOn,
      int millisecondsTimeout,
      bool exitContext)
    {
      return WaitHandle.SignalAndWait(toSignal, toWaitOn, millisecondsTimeout);
    }
  }
}
