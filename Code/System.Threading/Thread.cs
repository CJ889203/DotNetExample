// Decompiled with JetBrains decompiler
// Type: System.Threading.Thread
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Principal;


#nullable enable
namespace System.Threading
{
  /// <summary>Creates and controls a thread, sets its priority, and gets its status.</summary>
  public sealed class Thread : CriticalFinalizerObject
  {

    #nullable disable
    internal ExecutionContext _executionContext;
    internal SynchronizationContext _synchronizationContext;
    private string _name;
    private Thread.StartHelper _startHelper;
    private IntPtr _DONT_USE_InternalThread;
    private int _priority;
    private int _managedThreadId;
    private bool _mayNeedResetForThreadPool;
    private static readonly bool s_isProcessorNumberReallyFast = ProcessorIdCache.ProcessorNumberSpeedCheck();
    private static AsyncLocal<IPrincipal> s_asyncLocalPrincipal;
    [ThreadStatic]
    private static Thread t_currentThread;

    private Thread()
    {
    }

    /// <summary>Gets a unique identifier for the current managed thread.</summary>
    /// <returns>An integer that represents a unique identifier for this managed thread.</returns>
    public extern int ManagedThreadId { [Intrinsic, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal ThreadHandle GetNativeHandle()
    {
      IntPtr useInternalThread = this._DONT_USE_InternalThread;
      return !(useInternalThread == IntPtr.Zero) ? new ThreadHandle(useInternalThread) : throw new ArgumentException((string) null, SR.Argument_InvalidHandle);
    }

    private unsafe void StartCore()
    {
      lock (this)
      {
        string name = this._name;
        IntPtr num;
        if (name == null)
        {
          num = IntPtr.Zero;
        }
        else
        {
          fixed (char* chPtr = &name.GetPinnableReference())
            num = (IntPtr) chPtr;
        }
        char* chPtr1 = (char*) num;
        ThreadHandle nativeHandle = this.GetNativeHandle();
        Thread.StartHelper startHelper = this._startHelper;
        int stackSize = startHelper != null ? startHelper._maxStackSize : 0;
        int priority = this._priority;
        char* pThreadName = chPtr1;
        Thread.StartInternal(nativeHandle, stackSize, priority, pThreadName);
        // ISSUE: fixed variable is out of scope
        // ISSUE: __unpin statement
        __unpin(chPtr);
      }
    }

    [DllImport("QCall")]
    private static extern unsafe void StartInternal(
      ThreadHandle t,
      int stackSize,
      int priority,
      char* pThreadName);

    private void StartCallback()
    {
      Thread.StartHelper startHelper = this._startHelper;
      this._startHelper = (Thread.StartHelper) null;
      startHelper.Run();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr InternalGetCurrentThread();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SleepInternal(int millisecondsTimeout);

    [DllImport("QCall")]
    internal static extern void UninterruptibleSleep0();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SpinWaitInternal(int iterations);

    /// <summary>Causes a thread to wait the number of times defined by the <paramref name="iterations" /> parameter.</summary>
    /// <param name="iterations">A 32-bit signed integer that defines how long a thread is to wait.</param>
    public static void SpinWait(int iterations) => Thread.SpinWaitInternal(iterations);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern Interop.BOOL YieldInternal();

    /// <summary>Causes the calling thread to yield execution to another thread that is ready to run on the current processor. The operating system selects the thread to yield to.</summary>
    /// <returns>
    /// <see langword="true" /> if the operating system switched execution to another thread; otherwise, <see langword="false" />.</returns>
    public static bool Yield() => Thread.YieldInternal() != 0;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Thread InitializeCurrentThread() => Thread.t_currentThread = Thread.GetCurrentThreadNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Thread GetCurrentThreadNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Initialize();

    /// <summary>Ensures that resources are freed and other cleanup operations are performed when the garbage collector reclaims the <see cref="T:System.Threading.Thread" /> object.</summary>
    ~Thread() => this.InternalFinalize();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalFinalize();

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void InformThreadNameChange(ThreadHandle t, string name, int len);

    /// <summary>Gets a value indicating the execution status of the current thread.</summary>
    /// <returns>
    /// <see langword="true" /> if this thread has been started and has not terminated normally or aborted; otherwise, <see langword="false" />.</returns>
    public extern bool IsAlive { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>Gets or sets a value indicating whether or not a thread is a background thread.</summary>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread is dead.</exception>
    /// <returns>
    /// <see langword="true" /> if this thread is or is to become a background thread; otherwise, <see langword="false" />.</returns>
    public bool IsBackground
    {
      get => this.IsBackgroundNative();
      set
      {
        this.SetBackgroundNative(value);
        if (value)
          return;
        this._mayNeedResetForThreadPool = true;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool IsBackgroundNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetBackgroundNative(bool isBackground);

    /// <summary>Gets a value indicating whether or not a thread belongs to the managed thread pool.</summary>
    /// <returns>
    /// <see langword="true" /> if this thread belongs to the managed thread pool; otherwise, <see langword="false" />.</returns>
    public extern bool IsThreadPoolThread { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] internal set; }

    /// <summary>Gets or sets a value indicating the scheduling priority of a thread.</summary>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has reached a final state, such as <see cref="F:System.Threading.ThreadState.Aborted" />.</exception>
    /// <exception cref="T:System.ArgumentException">The value specified for a set operation is not a valid <see cref="T:System.Threading.ThreadPriority" /> value.</exception>
    /// <returns>One of the <see cref="T:System.Threading.ThreadPriority" /> values. The default value is <see cref="F:System.Threading.ThreadPriority.Normal" />.</returns>
    public ThreadPriority Priority
    {
      get => (ThreadPriority) this.GetPriorityNative();
      set
      {
        this.SetPriorityNative((int) value);
        if (value == ThreadPriority.Normal)
          return;
        this._mayNeedResetForThreadPool = true;
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetPriorityNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetPriorityNative(int priority);

    [DllImport("QCall")]
    private static extern ulong GetCurrentOSThreadId();

    /// <summary>Gets a value containing the states of the current thread.</summary>
    /// <returns>One of the <see cref="T:System.Threading.ThreadState" /> values indicating the state of the current thread. The initial value is <see cref="F:System.Threading.ThreadState.Unstarted" />.</returns>
    public ThreadState ThreadState => (ThreadState) this.GetThreadStateNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetThreadStateNative();

    /// <summary>Returns an <see cref="T:System.Threading.ApartmentState" /> value indicating the apartment state.</summary>
    /// <returns>One of the <see cref="T:System.Threading.ApartmentState" /> values indicating the apartment state of the managed thread. The default is <see cref="F:System.Threading.ApartmentState.Unknown" />.</returns>
    public ApartmentState GetApartmentState() => (ApartmentState) this.GetApartmentStateNative();

    private bool SetApartmentStateUnchecked(ApartmentState state, bool throwOnError)
    {
      ApartmentState p1 = (ApartmentState) this.SetApartmentStateNative((int) state);
      if (state == ApartmentState.Unknown && p1 == ApartmentState.MTA || p1 == state)
        return true;
      if (throwOnError)
        throw new InvalidOperationException(SR.Format(SR.Thread_ApartmentState_ChangeFailed, (object) p1));
      return false;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int GetApartmentStateNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int SetApartmentStateNative(int state);

    /// <summary>Turns off automatic cleanup of runtime callable wrappers (RCW) for the current thread.</summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void DisableComObjectEagerCleanup();

    /// <summary>Interrupts a thread that is in the <see cref="F:System.Threading.ThreadState.WaitSleepJoin" /> thread state.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the appropriate <see cref="T:System.Security.Permissions.SecurityPermission" />.</exception>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Interrupt();

    /// <summary>Blocks the calling thread until the thread represented by this instance terminates or the specified time elapses, while continuing to perform standard COM and SendMessage pumping.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the thread to terminate.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout" /> is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> in milliseconds.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has not been started.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="millisecondsTimeout" /> is less than -1 (Timeout.Infinite).</exception>
    /// <exception cref="T:System.Threading.ThreadInterruptedException">The thread was interrupted while waiting.</exception>
    /// <returns>
    /// <see langword="true" /> if the thread has terminated; <see langword="false" /> if the thread has not terminated after the amount of time specified by the <paramref name="millisecondsTimeout" /> parameter has elapsed.</returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool Join(int millisecondsTimeout);

    internal static extern int OptimalMaxSpinWaitsPerSpinIteration { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetCurrentProcessorNumber();

    /// <summary>Gets an ID used to indicate on which processor the current thread is executing.</summary>
    /// <returns>An integer representing the cached processor ID.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetCurrentProcessorId() => Thread.s_isProcessorNumberReallyFast ? Thread.GetCurrentProcessorNumber() : ProcessorIdCache.GetCurrentProcessorId();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void ResetThreadPoolThread()
    {
      if (!ThreadPool.UsePortableThreadPool || !this._mayNeedResetForThreadPool)
        return;
      this.ResetThreadPoolThreadSlow();
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Thread" /> class.</summary>
    /// <param name="start">A <see cref="T:System.Threading.ThreadStart" /> delegate that represents the methods to be invoked when this thread begins executing.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="start" /> parameter is <see langword="null" />.</exception>
    public Thread(ThreadStart start)
    {
      this._startHelper = start != null ? new Thread.StartHelper((Delegate) start) : throw new ArgumentNullException(nameof (start));
      this.Initialize();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Thread" /> class, specifying the maximum stack size for the thread.</summary>
    /// <param name="start">A <see cref="T:System.Threading.ThreadStart" /> delegate that represents the methods to be invoked when this thread begins executing.</param>
    /// <param name="maxStackSize">The maximum stack size, in bytes, to be used by the thread, or 0 to use the default maximum stack size specified in the header for the executable.
    /// 
    /// Important   For partially trusted code, <paramref name="maxStackSize" /> is ignored if it is greater than the default stack size. No exception is thrown.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="start" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="maxStackSize" /> is less than zero.</exception>
    public Thread(ThreadStart start, int maxStackSize)
    {
      if (start == null)
        throw new ArgumentNullException(nameof (start));
      this._startHelper = maxStackSize >= 0 ? new Thread.StartHelper((Delegate) start)
      {
        _maxStackSize = maxStackSize
      } : throw new ArgumentOutOfRangeException(nameof (maxStackSize), SR.ArgumentOutOfRange_NeedNonNegNum);
      this.Initialize();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Thread" /> class, specifying a delegate that allows an object to be passed to the thread when the thread is started.</summary>
    /// <param name="start">A delegate that represents the methods to be invoked when this thread begins executing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="start" /> is <see langword="null" />.</exception>
    public Thread(ParameterizedThreadStart start)
    {
      this._startHelper = start != null ? new Thread.StartHelper((Delegate) start) : throw new ArgumentNullException(nameof (start));
      this.Initialize();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Thread" /> class, specifying a delegate that allows an object to be passed to the thread when the thread is started and specifying the maximum stack size for the thread.</summary>
    /// <param name="start">A <see cref="T:System.Threading.ParameterizedThreadStart" /> delegate that represents the methods to be invoked when this thread begins executing.</param>
    /// <param name="maxStackSize">The maximum stack size, in bytes, to be used by the thread, or 0 to use the default maximum stack size specified in the header for the executable.
    /// 
    /// Important   For partially trusted code, <paramref name="maxStackSize" /> is ignored if it is greater than the default stack size. No exception is thrown.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="start" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="maxStackSize" /> is less than zero.</exception>
    public Thread(ParameterizedThreadStart start, int maxStackSize)
    {
      if (start == null)
        throw new ArgumentNullException(nameof (start));
      this._startHelper = maxStackSize >= 0 ? new Thread.StartHelper((Delegate) start)
      {
        _maxStackSize = maxStackSize
      } : throw new ArgumentOutOfRangeException(nameof (maxStackSize), SR.ArgumentOutOfRange_NeedNonNegNum);
      this.Initialize();
    }

    [UnsupportedOSPlatformGuard("browser")]
    internal static bool IsThreadStartSupported => true;

    /// <summary>Causes the operating system to change the state of the current instance to <see cref="F:System.Threading.ThreadState.Running" />, and optionally supplies an object containing data to be used by the method the thread executes.</summary>
    /// <param name="parameter">An object that contains data to be used by the method the thread executes.</param>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has already been started.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is not enough memory available to start this thread.</exception>
    /// <exception cref="T:System.InvalidOperationException">This thread was created using a <see cref="T:System.Threading.ThreadStart" /> delegate instead of a <see cref="T:System.Threading.ParameterizedThreadStart" /> delegate.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Start(object? parameter) => this.Start(parameter, true);

    /// <summary>Causes the operating system to change the state of the current instance to <see cref="F:System.Threading.ThreadState.Running" />, and optionally supplies an object containing data to be used by the method the thread executes.</summary>
    /// <param name="parameter">An object that contains data to be used by the method the thread executes.</param>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has already been started.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is not enough memory available to start this thread.</exception>
    /// <exception cref="T:System.InvalidOperationException">This thread was created using a <see cref="T:System.Threading.ThreadStart" /> delegate instead of a <see cref="T:System.Threading.ParameterizedThreadStart" /> delegate.</exception>
    [UnsupportedOSPlatform("browser")]
    public void UnsafeStart(object? parameter) => this.Start(parameter, false);


    #nullable disable
    private void Start(object parameter, bool captureContext)
    {
      Thread.StartHelper startHelper = this._startHelper;
      if (startHelper != null)
      {
        if (startHelper._start is ThreadStart)
          throw new InvalidOperationException(SR.InvalidOperation_ThreadWrongThreadStart);
        startHelper._startArg = parameter;
        startHelper._executionContext = captureContext ? ExecutionContext.Capture() : (ExecutionContext) null;
      }
      this.StartCore();
    }

    /// <summary>Causes the operating system to change the state of the current instance to <see cref="F:System.Threading.ThreadState.Running" />.</summary>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has already been started.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is not enough memory available to start this thread.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Start() => this.Start(true);

    /// <summary>Causes the operating system to change the state of the current instance to <see cref="F:System.Threading.ThreadState.Running" />.</summary>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has already been started.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is not enough memory available to start this thread.</exception>
    [UnsupportedOSPlatform("browser")]
    public void UnsafeStart() => this.Start(false);

    private void Start(bool captureContext)
    {
      Thread.StartHelper startHelper = this._startHelper;
      if (startHelper != null)
      {
        startHelper._startArg = (object) null;
        startHelper._executionContext = captureContext ? ExecutionContext.Capture() : (ExecutionContext) null;
      }
      this.StartCore();
    }

    private void RequireCurrentThread()
    {
      if (this != Thread.CurrentThread)
        throw new InvalidOperationException(SR.Thread_Operation_RequiresCurrentThread);
    }

    private void SetCultureOnUnstartedThread(CultureInfo value, bool uiCulture)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      Thread.StartHelper startHelper = this._startHelper;
      if ((this.ThreadState & ThreadState.Unstarted) == ThreadState.Running)
        throw new InvalidOperationException(SR.Thread_Operation_RequiresCurrentThread);
      if (uiCulture)
        startHelper._uiCulture = value;
      else
        startHelper._culture = value;
    }

    private void ThreadNameChanged(string value) => Thread.InformThreadNameChange(this.GetNativeHandle(), value, value != null ? value.Length : 0);


    #nullable enable
    /// <summary>Gets or sets the culture for the current thread.</summary>
    /// <exception cref="T:System.ArgumentNullException">The property is set to <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">.NET Core and .NET 5+ only: Reading or writing the culture of a thread from another thread is not supported.</exception>
    /// <returns>An object that represents the culture for the current thread.</returns>
    public CultureInfo CurrentCulture
    {
      get
      {
        this.RequireCurrentThread();
        return CultureInfo.CurrentCulture;
      }
      set
      {
        if (this != Thread.CurrentThread)
          this.SetCultureOnUnstartedThread(value, false);
        else
          CultureInfo.CurrentCulture = value;
      }
    }

    /// <summary>Gets or sets the current culture used by the Resource Manager to look up culture-specific resources at run time.</summary>
    /// <exception cref="T:System.ArgumentNullException">The property is set to <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The property is set to a culture name that cannot be used to locate a resource file. Resource filenames must include only letters, numbers, hyphens or underscores.</exception>
    /// <exception cref="T:System.InvalidOperationException">.NET Core and .NET 5+ only: Reading or writing the culture of a thread from another thread is not supported.</exception>
    /// <returns>An object that represents the current culture.</returns>
    public CultureInfo CurrentUICulture
    {
      get
      {
        this.RequireCurrentThread();
        return CultureInfo.CurrentUICulture;
      }
      set
      {
        if (this != Thread.CurrentThread)
          this.SetCultureOnUnstartedThread(value, true);
        else
          CultureInfo.CurrentUICulture = value;
      }
    }

    /// <summary>Gets or sets the thread's current principal (for role-based security).</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the permission required to set the principal.</exception>
    /// <returns>An <see cref="T:System.Security.Principal.IPrincipal" /> value representing the security context.</returns>
    public static IPrincipal? CurrentPrincipal
    {
      get
      {
        IPrincipal threadPrincipal = Thread.s_asyncLocalPrincipal?.Value;
        if (threadPrincipal == null)
          Thread.CurrentPrincipal = threadPrincipal = AppDomain.CurrentDomain.GetThreadPrincipal();
        return threadPrincipal;
      }
      set
      {
        if (Thread.s_asyncLocalPrincipal == null)
        {
          if (value == null)
            return;
          Interlocked.CompareExchange<AsyncLocal<IPrincipal>>(ref Thread.s_asyncLocalPrincipal, new AsyncLocal<IPrincipal>(), (AsyncLocal<IPrincipal>) null);
        }
        Thread.s_asyncLocalPrincipal.Value = value;
      }
    }

    /// <summary>Gets the currently running thread.</summary>
    /// <returns>A <see cref="T:System.Threading.Thread" /> that is the representation of the currently running thread.</returns>
    public static Thread CurrentThread
    {
      [Intrinsic] get => Thread.t_currentThread ?? Thread.InitializeCurrentThread();
    }

    /// <summary>Suspends the current thread for the specified number of milliseconds.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds for which the thread is suspended. If the value of the <paramref name="millisecondsTimeout" /> argument is zero, the thread relinquishes the remainder of its time slice to any thread of equal priority that is ready to run. If there are no other threads of equal priority that are ready to run, execution of the current thread is not suspended.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The time-out value is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Sleep(int millisecondsTimeout)
    {
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), (object) millisecondsTimeout, SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      Thread.SleepInternal(millisecondsTimeout);
    }

    internal static ulong CurrentOSThreadId => Thread.GetCurrentOSThreadId();

    /// <summary>Gets an <see cref="T:System.Threading.ExecutionContext" /> object that contains information about the various contexts of the current thread.</summary>
    /// <returns>An <see cref="T:System.Threading.ExecutionContext" /> object that consolidates context information for the current thread.</returns>
    public ExecutionContext? ExecutionContext => ExecutionContext.Capture();

    /// <summary>Gets or sets the name of the thread.</summary>
    /// <exception cref="T:System.InvalidOperationException">A set operation was requested, but the <see langword="Name" /> property has already been set.</exception>
    /// <returns>A string containing the name of the thread, or <see langword="null" /> if no name was set.</returns>
    public string? Name
    {
      get => this._name;
      set
      {
        lock (this)
        {
          if (!(this._name != value))
            return;
          this._name = value;
          this.ThreadNameChanged(value);
          this._mayNeedResetForThreadPool = true;
        }
      }
    }

    internal void SetThreadPoolWorkerThreadName()
    {
      lock (this)
      {
        this._name = ".NET ThreadPool Worker";
        this.ThreadNameChanged(".NET ThreadPool Worker");
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ResetThreadPoolThreadSlow()
    {
      this._mayNeedResetForThreadPool = false;
      if (this._name != ".NET ThreadPool Worker")
        this.SetThreadPoolWorkerThreadName();
      if (!this.IsBackground)
        this.IsBackground = true;
      if (this.Priority == ThreadPriority.Normal)
        return;
      this.Priority = ThreadPriority.Normal;
    }

    /// <summary>Raises a <see cref="T:System.Threading.ThreadAbortException" /> in the thread on which it is invoked, to begin the process of terminating the thread. Calling this method usually terminates the thread.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread that is being aborted is currently suspended.</exception>
    [Obsolete("Thread.Abort is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public void Abort() => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ThreadAbort);

    /// <summary>Raises a <see cref="T:System.Threading.ThreadAbortException" /> in the thread on which it is invoked, to begin the process of terminating the thread while also providing exception information about the thread termination. Calling this method usually terminates the thread.</summary>
    /// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted.</param>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread that is being aborted is currently suspended.</exception>
    [Obsolete("Thread.Abort is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public void Abort(object? stateInfo) => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ThreadAbort);

    /// <summary>Cancels an <see cref="M:System.Threading.Thread.Abort(System.Object)" /> requested for the current thread.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">
    /// <see langword="Abort" /> was not invoked on the current thread.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required security permission for the current thread.</exception>
    [Obsolete("Thread.Abort is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public static void ResetAbort() => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ThreadAbort);

    /// <summary>Either suspends the thread, or if the thread is already suspended, has no effect.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has not been started or is dead.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the appropriate <see cref="T:System.Security.Permissions.SecurityPermission" />.</exception>
    [Obsolete("Thread.Suspend has been deprecated. Use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.")]
    public void Suspend() => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ThreadSuspend);

    /// <summary>Resumes a thread that has been suspended.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has not been started, is dead, or is not in the suspended state.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the appropriate <see cref="T:System.Security.Permissions.SecurityPermission" />.</exception>
    [Obsolete("Thread.Resume has been deprecated. Use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.")]
    public void Resume() => throw new PlatformNotSupportedException(SR.PlatformNotSupported_ThreadSuspend);

    /// <summary>Notifies a host that execution is about to enter a region of code in which the effects of a thread abort or unhandled exception might jeopardize other tasks in the application domain.</summary>
    public static void BeginCriticalRegion()
    {
    }

    /// <summary>Notifies a host that execution is about to enter a region of code in which the effects of a thread abort or unhandled exception are limited to the current task.</summary>
    public static void EndCriticalRegion()
    {
    }

    /// <summary>Notifies a host that managed code is about to execute instructions that depend on the identity of the current physical operating system thread.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void BeginThreadAffinity()
    {
    }

    /// <summary>Notifies a host that managed code has finished executing instructions that depend on the identity of the current physical operating system thread.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public static void EndThreadAffinity()
    {
    }

    /// <summary>Allocates an unnamed data slot on all the threads. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute" /> attribute instead.</summary>
    /// <returns>The allocated named data slot on all threads.</returns>
    public static LocalDataStoreSlot AllocateDataSlot() => Thread.LocalDataStore.AllocateSlot();

    /// <summary>Allocates a named data slot on all threads. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute" /> attribute instead.</summary>
    /// <param name="name">The name of the data slot to be allocated.</param>
    /// <exception cref="T:System.ArgumentException">A named data slot with the specified name already exists.</exception>
    /// <returns>The allocated named data slot on all threads.</returns>
    public static LocalDataStoreSlot AllocateNamedDataSlot(string name) => Thread.LocalDataStore.AllocateNamedSlot(name);

    /// <summary>Looks up a named data slot. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute" /> attribute instead.</summary>
    /// <param name="name">The name of the local data slot.</param>
    /// <returns>A <see cref="T:System.LocalDataStoreSlot" /> allocated for this thread.</returns>
    public static LocalDataStoreSlot GetNamedDataSlot(string name) => Thread.LocalDataStore.GetNamedSlot(name);

    /// <summary>Eliminates the association between a name and a slot, for all threads in the process. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute" /> attribute instead.</summary>
    /// <param name="name">The name of the data slot to be freed.</param>
    public static void FreeNamedDataSlot(string name) => Thread.LocalDataStore.FreeNamedSlot(name);

    /// <summary>Retrieves the value from the specified slot on the current thread, within the current thread's current domain. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute" /> attribute instead.</summary>
    /// <param name="slot">The <see cref="T:System.LocalDataStoreSlot" /> from which to get the value.</param>
    /// <returns>The retrieved value.</returns>
    public static object? GetData(LocalDataStoreSlot slot) => Thread.LocalDataStore.GetData(slot);

    /// <summary>Sets the data in the specified slot on the currently running thread, for that thread's current domain. For better performance, use fields marked with the <see cref="T:System.ThreadStaticAttribute" /> attribute instead.</summary>
    /// <param name="slot">The <see cref="T:System.LocalDataStoreSlot" /> in which to set the value.</param>
    /// <param name="data">The value to be set.</param>
    public static void SetData(LocalDataStoreSlot slot, object? data) => Thread.LocalDataStore.SetData(slot, data);

    /// <summary>Gets or sets the apartment state of this thread.</summary>
    /// <exception cref="T:System.ArgumentException">An attempt is made to set this property to a state that is not a valid apartment state (a state other than single-threaded apartment (<see langword="STA" />) or multithreaded apartment (<see langword="MTA" />)).</exception>
    /// <returns>One of the <see cref="T:System.Threading.ApartmentState" /> values. The initial value is <see langword="Unknown" />.</returns>
    [Obsolete("The ApartmentState property has been deprecated. Use GetApartmentState, SetApartmentState or TrySetApartmentState.")]
    public ApartmentState ApartmentState
    {
      get => this.GetApartmentState();
      set => this.TrySetApartmentState(value);
    }

    /// <summary>Sets the apartment state of a thread before it is started.</summary>
    /// <param name="state">The new apartment state.</param>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases on macOS and Linux.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="state" /> is not a valid apartment state.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread has already been started.</exception>
    /// <exception cref="T:System.InvalidOperationException">The apartment state has already been initialized.</exception>
    [SupportedOSPlatform("windows")]
    public void SetApartmentState(ApartmentState state) => this.SetApartmentState(state, true);

    /// <summary>Sets the apartment state of a thread before it is started.</summary>
    /// <param name="state">The new apartment state.</param>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: In all cases on macOS and Linux.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="state" /> is not a valid apartment state.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The thread was started and has terminated, or the call is not being made from the thread's context while the thread is running.</exception>
    /// <returns>
    /// <see langword="true" /> if the apartment state is set; otherwise, <see langword="false" />.</returns>
    public bool TrySetApartmentState(ApartmentState state) => this.SetApartmentState(state, false);

    private bool SetApartmentState(ApartmentState state, bool throwOnError)
    {
      switch (state)
      {
        case ApartmentState.STA:
        case ApartmentState.MTA:
        case ApartmentState.Unknown:
          return this.SetApartmentStateUnchecked(state, throwOnError);
        default:
          throw new ArgumentOutOfRangeException(nameof (state), SR.ArgumentOutOfRange_Enum);
      }
    }

    /// <summary>Returns a <see cref="T:System.Threading.CompressedStack" /> object that can be used to capture the stack for the current thread.</summary>
    /// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
    [Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public CompressedStack GetCompressedStack() => throw new InvalidOperationException(SR.Thread_GetSetCompressedStack_NotSupported);

    /// <summary>Applies a captured <see cref="T:System.Threading.CompressedStack" /> to the current thread.</summary>
    /// <param name="stack">The <see cref="T:System.Threading.CompressedStack" /> object to be applied to the current thread.</param>
    /// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
    [Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public void SetCompressedStack(CompressedStack stack) => throw new InvalidOperationException(SR.Thread_GetSetCompressedStack_NotSupported);

    /// <summary>Returns the current domain in which the current thread is running.</summary>
    /// <returns>An <see cref="T:System.AppDomain" /> representing the current application domain of the running thread.</returns>
    public static AppDomain GetDomain() => AppDomain.CurrentDomain;

    /// <summary>Returns a unique application domain identifier.</summary>
    /// <returns>A 32-bit signed integer uniquely identifying the application domain.</returns>
    public static int GetDomainID() => 1;

    /// <summary>Returns a hash code for the current thread.</summary>
    /// <returns>An integer hash code value.</returns>
    public override int GetHashCode() => this.ManagedThreadId;

    /// <summary>Blocks the calling thread until the thread represented by this instance terminates, while continuing to perform standard COM and <see langword="SendMessage" /> pumping.</summary>
    /// <exception cref="T:System.Threading.ThreadStateException">The caller attempted to join a thread that is in the <see cref="F:System.Threading.ThreadState.Unstarted" /> state.</exception>
    /// <exception cref="T:System.Threading.ThreadInterruptedException">The thread is interrupted while waiting.</exception>
    public void Join() => this.Join(-1);

    /// <summary>Blocks the calling thread until the thread represented by this instance terminates or the specified time elapses, while continuing to perform standard COM and SendMessage pumping.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> set to the amount of time to wait for the thread to terminate.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> in milliseconds, or is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds.</exception>
    /// <exception cref="T:System.Threading.ThreadStateException">The caller attempted to join a thread that is in the <see cref="F:System.Threading.ThreadState.Unstarted" /> state.</exception>
    /// <returns>
    /// <see langword="true" /> if the thread terminated; <see langword="false" /> if the thread has not terminated after the amount of time specified by the <paramref name="timeout" /> parameter has elapsed.</returns>
    public bool Join(TimeSpan timeout) => this.Join(WaitHandle.ToTimeoutMilliseconds(timeout));

    /// <summary>Synchronizes memory access as follows: The processor executing the current thread cannot reorder instructions in such a way that memory accesses prior to the call to <see cref="M:System.Threading.Thread.MemoryBarrier" /> execute after memory accesses that follow the call to <see cref="M:System.Threading.Thread.MemoryBarrier" />.</summary>
    public static void MemoryBarrier() => Interlocked.MemoryBarrier();

    /// <summary>Suspends the current thread for the specified amount of time.</summary>
    /// <param name="timeout">The amount of time for which the thread is suspended. If the value of the <paramref name="timeout" /> argument is <see cref="F:System.TimeSpan.Zero" />, the thread relinquishes the remainder of its time slice to any thread of equal priority that is ready to run. If there are no other threads of equal priority that are ready to run, execution of the current thread is not suspended.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> in milliseconds, or is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds.</exception>
    public static void Sleep(TimeSpan timeout) => Thread.Sleep(WaitHandle.ToTimeoutMilliseconds(timeout));

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    public static byte VolatileRead(ref byte address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    public static double VolatileRead(ref double address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    public static short VolatileRead(ref short address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    public static int VolatileRead(ref int address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    public static long VolatileRead(ref long address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    public static IntPtr VolatileRead(ref IntPtr address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    [return: NotNullIfNotNull("address")]
    public static object? VolatileRead([NotNullIfNotNull("address")] ref object? address) => Volatile.Read<object>(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public static sbyte VolatileRead(ref sbyte address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    public static float VolatileRead(ref float address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public static ushort VolatileRead(ref ushort address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public static uint VolatileRead(ref uint address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public static ulong VolatileRead(ref ulong address) => Volatile.Read(ref address);

    /// <summary>Reads the value of a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="address">The field to be read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    public static UIntPtr VolatileRead(ref UIntPtr address) => Volatile.Read(ref address);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite(ref byte address, byte value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite(ref double address, double value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite(ref short address, short value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite(ref int address, int value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite(ref long address, long value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite(ref IntPtr address, IntPtr value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite([NotNullIfNotNull("value")] ref object? address, object? value) => Volatile.Write<object>(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    [CLSCompliant(false)]
    public static void VolatileWrite(ref sbyte address, sbyte value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    public static void VolatileWrite(ref float address, float value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    [CLSCompliant(false)]
    public static void VolatileWrite(ref ushort address, ushort value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    [CLSCompliant(false)]
    public static void VolatileWrite(ref uint address, uint value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    [CLSCompliant(false)]
    public static void VolatileWrite(ref ulong address, ulong value) => Volatile.Write(ref address, value);

    /// <summary>Writes a value to a field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="address">The field to which the value is to be written.</param>
    /// <param name="value">The value to be written.</param>
    [CLSCompliant(false)]
    public static void VolatileWrite(ref UIntPtr address, UIntPtr value) => Volatile.Write(ref address, value);


    #nullable disable
    private sealed class StartHelper
    {
      internal int _maxStackSize;
      internal Delegate _start;
      internal object _startArg;
      internal CultureInfo _culture;
      internal CultureInfo _uiCulture;
      internal ExecutionContext _executionContext;
      internal static readonly ContextCallback s_threadStartContextCallback = new ContextCallback(Thread.StartHelper.Callback);

      internal StartHelper(Delegate start) => this._start = start;

      private static void Callback(object state) => ((Thread.StartHelper) state).RunWorker();

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      internal void Run()
      {
        if (this._executionContext != null && !this._executionContext.IsDefault)
          ExecutionContext.RunInternal(this._executionContext, Thread.StartHelper.s_threadStartContextCallback, (object) this);
        else
          this.RunWorker();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private void RunWorker()
      {
        this.InitializeCulture();
        Delegate start = this._start;
        this._start = (Delegate) null;
        if (start is ThreadStart threadStart)
        {
          threadStart();
        }
        else
        {
          ParameterizedThreadStart parameterizedThreadStart = (ParameterizedThreadStart) start;
          object startArg = this._startArg;
          this._startArg = (object) null;
          parameterizedThreadStart(startArg);
        }
      }

      private void InitializeCulture()
      {
        if (this._culture != null)
        {
          CultureInfo.CurrentCulture = this._culture;
          this._culture = (CultureInfo) null;
        }
        if (this._uiCulture == null)
          return;
        CultureInfo.CurrentUICulture = this._uiCulture;
        this._uiCulture = (CultureInfo) null;
      }
    }

    private static class LocalDataStore
    {
      private static Dictionary<string, LocalDataStoreSlot> s_nameToSlotMap;

      public static LocalDataStoreSlot AllocateSlot() => new LocalDataStoreSlot(new ThreadLocal<object>());

      private static Dictionary<string, LocalDataStoreSlot> EnsureNameToSlotMap()
      {
        Dictionary<string, LocalDataStoreSlot> nameToSlotMap = Thread.LocalDataStore.s_nameToSlotMap;
        if (nameToSlotMap != null)
          return nameToSlotMap;
        Dictionary<string, LocalDataStoreSlot> dictionary = new Dictionary<string, LocalDataStoreSlot>();
        return Interlocked.CompareExchange<Dictionary<string, LocalDataStoreSlot>>(ref Thread.LocalDataStore.s_nameToSlotMap, dictionary, (Dictionary<string, LocalDataStoreSlot>) null) ?? dictionary;
      }

      public static LocalDataStoreSlot AllocateNamedSlot(string name)
      {
        LocalDataStoreSlot localDataStoreSlot = Thread.LocalDataStore.AllocateSlot();
        Dictionary<string, LocalDataStoreSlot> slotMap = Thread.LocalDataStore.EnsureNameToSlotMap();
        lock (slotMap)
          slotMap.Add(name, localDataStoreSlot);
        return localDataStoreSlot;
      }

      public static LocalDataStoreSlot GetNamedSlot(string name)
      {
        Dictionary<string, LocalDataStoreSlot> slotMap = Thread.LocalDataStore.EnsureNameToSlotMap();
        lock (slotMap)
        {
          LocalDataStoreSlot namedSlot;
          if (!slotMap.TryGetValue(name, out namedSlot))
          {
            namedSlot = Thread.LocalDataStore.AllocateSlot();
            slotMap[name] = namedSlot;
          }
          return namedSlot;
        }
      }

      public static void FreeNamedSlot(string name)
      {
        Dictionary<string, LocalDataStoreSlot> slotMap = Thread.LocalDataStore.EnsureNameToSlotMap();
        lock (slotMap)
          slotMap.Remove(name);
      }

      private static ThreadLocal<object> GetThreadLocal(LocalDataStoreSlot slot) => slot != null ? slot.Data : throw new ArgumentNullException(nameof (slot));

      public static object GetData(LocalDataStoreSlot slot) => Thread.LocalDataStore.GetThreadLocal(slot).Value;

      public static void SetData(LocalDataStoreSlot slot, object value) => Thread.LocalDataStore.GetThreadLocal(slot).Value = value;
    }
  }
}
