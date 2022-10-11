// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadPool
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.ThreadPool.xml

using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides a pool of threads that can be used to execute tasks, post work items, process asynchronous I/O, wait on behalf of other threads, and process timers.</summary>
  public static class ThreadPool
  {
    internal static readonly bool UsePortableThreadPool = ThreadPool.InitializeConfigAndDetermineUsePortableThreadPool();
    private static readonly bool IsWorkerTrackingEnabledInConfig = ThreadPool.GetEnableWorkerTracking();

    #nullable disable
    internal static readonly ThreadPoolWorkQueue s_workQueue = new ThreadPoolWorkQueue();
    internal static readonly Action<object> s_invokeAsyncStateMachineBox = (Action<object>) (state =>
    {
      if (!(state is IAsyncStateMachineBox asyncStateMachineBox2))
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.state);
      else
        asyncStateMachineBox2.MoveNext();
    });

    internal static bool SupportsTimeSensitiveWorkItems => ThreadPool.UsePortableThreadPool;

    private static unsafe bool InitializeConfigAndDetermineUsePortableThreadPool()
    {
      bool portableThreadPool = false;
      int configVariableIndex = 0;
      while (true)
      {
        uint configValue;
        bool isBoolean;
        char* appContextConfigName;
        int configUint32Value = ThreadPool.GetNextConfigUInt32Value(configVariableIndex, out configValue, out isBoolean, out appContextConfigName);
        if (configUint32Value >= 0)
        {
          configVariableIndex = configUint32Value;
          if ((IntPtr) appContextConfigName == IntPtr.Zero)
          {
            portableThreadPool = true;
          }
          else
          {
            string str = new string(appContextConfigName);
            if (isBoolean)
              AppContext.SetSwitch(str, configValue > 0U);
            else
              AppContext.SetData(str, (object) configValue);
          }
        }
        else
          break;
      }
      return portableThreadPool;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetNextConfigUInt32Value(
      int configVariableIndex,
      out uint configValue,
      out bool isBoolean,
      out char* appContextConfigName);

    private static bool GetEnableWorkerTracking() => !ThreadPool.UsePortableThreadPool ? ThreadPool.GetEnableWorkerTrackingNative() : AppContextConfigHelper.GetBooleanConfig("System.Threading.ThreadPool.EnableWorkerTracking", false);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanSetMinIOCompletionThreads(int ioCompletionThreads);

    internal static void SetMinIOCompletionThreads(int ioCompletionThreads) => ThreadPool.SetMinThreadsNative(1, ioCompletionThreads);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanSetMaxIOCompletionThreads(int ioCompletionThreads);

    internal static void SetMaxIOCompletionThreads(int ioCompletionThreads) => ThreadPool.SetMaxThreadsNative(1, ioCompletionThreads);

    /// <summary>Sets the number of requests to the thread pool that can be active concurrently. All requests above that number remain queued until thread pool threads become available.</summary>
    /// <param name="workerThreads">The maximum number of worker threads in the thread pool.</param>
    /// <param name="completionPortThreads">The maximum number of asynchronous I/O threads in the thread pool.</param>
    /// <returns>
    /// <see langword="true" /> if the change is successful; otherwise, <see langword="false" />.</returns>
    public static bool SetMaxThreads(int workerThreads, int completionPortThreads)
    {
      if (ThreadPool.UsePortableThreadPool)
        return PortableThreadPool.ThreadPoolInstance.SetMaxThreads(workerThreads, completionPortThreads);
      return workerThreads >= 0 && completionPortThreads >= 0 && ThreadPool.SetMaxThreadsNative(workerThreads, completionPortThreads);
    }


    #nullable enable
    /// <summary>Retrieves the number of requests to the thread pool that can be active concurrently. All requests above that number remain queued until thread pool threads become available.</summary>
    /// <param name="workerThreads">The maximum number of worker threads in the thread pool.</param>
    /// <param name="completionPortThreads">The maximum number of asynchronous I/O threads in the thread pool.</param>
    public static void GetMaxThreads(out int workerThreads, out int completionPortThreads)
    {
      ThreadPool.GetMaxThreadsNative(out workerThreads, out completionPortThreads);
      if (!ThreadPool.UsePortableThreadPool)
        return;
      workerThreads = PortableThreadPool.ThreadPoolInstance.GetMaxThreads();
    }

    /// <summary>Sets the minimum number of threads the thread pool creates on demand, as new requests are made, before switching to an algorithm for managing thread creation and destruction.</summary>
    /// <param name="workerThreads">The minimum number of worker threads that the thread pool creates on demand.</param>
    /// <param name="completionPortThreads">The minimum number of asynchronous I/O threads that the thread pool creates on demand.</param>
    /// <returns>
    /// <see langword="true" /> if the change is successful; otherwise, <see langword="false" />.</returns>
    public static bool SetMinThreads(int workerThreads, int completionPortThreads)
    {
      if (ThreadPool.UsePortableThreadPool)
        return PortableThreadPool.ThreadPoolInstance.SetMinThreads(workerThreads, completionPortThreads);
      return workerThreads >= 0 && completionPortThreads >= 0 && ThreadPool.SetMinThreadsNative(workerThreads, completionPortThreads);
    }

    /// <summary>Retrieves the minimum number of threads the thread pool creates on demand, as new requests are made, before switching to an algorithm for managing thread creation and destruction.</summary>
    /// <param name="workerThreads">When this method returns, contains the minimum number of worker threads that the thread pool creates on demand.</param>
    /// <param name="completionPortThreads">When this method returns, contains the minimum number of asynchronous I/O threads that the thread pool creates on demand.</param>
    public static void GetMinThreads(out int workerThreads, out int completionPortThreads)
    {
      ThreadPool.GetMinThreadsNative(out workerThreads, out completionPortThreads);
      if (!ThreadPool.UsePortableThreadPool)
        return;
      workerThreads = PortableThreadPool.ThreadPoolInstance.GetMinThreads();
    }

    /// <summary>Retrieves the difference between the maximum number of thread pool threads returned by the <see cref="M:System.Threading.ThreadPool.GetMaxThreads(System.Int32@,System.Int32@)" /> method, and the number currently active.</summary>
    /// <param name="workerThreads">The number of available worker threads.</param>
    /// <param name="completionPortThreads">The number of available asynchronous I/O threads.</param>
    public static void GetAvailableThreads(out int workerThreads, out int completionPortThreads)
    {
      ThreadPool.GetAvailableThreadsNative(out workerThreads, out completionPortThreads);
      if (!ThreadPool.UsePortableThreadPool)
        return;
      workerThreads = PortableThreadPool.ThreadPoolInstance.GetAvailableThreads();
    }

    /// <summary>Gets the number of thread pool threads that currently exist.</summary>
    /// <returns>The number of thread pool threads that currently exist.</returns>
    public static int ThreadCount => (ThreadPool.UsePortableThreadPool ? PortableThreadPool.ThreadPoolInstance.ThreadCount : 0) + ThreadPool.GetThreadCount();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetThreadCount();

    /// <summary>Gets the number of work items that have been processed so far.</summary>
    /// <returns>The number of work items that have been processed so far.</returns>
    public static long CompletedWorkItemCount
    {
      get
      {
        long completedWorkItemCount = ThreadPool.GetCompletedWorkItemCount();
        if (ThreadPool.UsePortableThreadPool)
          completedWorkItemCount += PortableThreadPool.ThreadPoolInstance.CompletedWorkItemCount;
        return completedWorkItemCount;
      }
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern long GetCompletedWorkItemCount();

    private static long PendingUnmanagedWorkItemCount => !ThreadPool.UsePortableThreadPool ? ThreadPool.GetPendingUnmanagedWorkItemCount() : 0L;

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern long GetPendingUnmanagedWorkItemCount();


    #nullable disable
    private static RegisteredWaitHandle RegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object state,
      uint millisecondsTimeOutInterval,
      bool executeOnlyOnce,
      bool flowExecutionContext)
    {
      if (waitObject == null)
        throw new ArgumentNullException(nameof (waitObject));
      if (callBack == null)
        throw new ArgumentNullException(nameof (callBack));
      RegisteredWaitHandle registeredWaitHandle = new RegisteredWaitHandle(waitObject, new _ThreadPoolWaitOrTimerCallback(callBack, state, flowExecutionContext), (int) millisecondsTimeOutInterval, !executeOnlyOnce);
      registeredWaitHandle.OnBeforeRegister();
      if (ThreadPool.UsePortableThreadPool)
      {
        PortableThreadPool.ThreadPoolInstance.RegisterWaitHandle(registeredWaitHandle);
      }
      else
      {
        IntPtr nativeRegisteredWaitHandle = ThreadPool.RegisterWaitForSingleObjectNative(waitObject, (object) registeredWaitHandle.Callback, (uint) registeredWaitHandle.TimeoutDurationMs, !registeredWaitHandle.Repeating, registeredWaitHandle);
        registeredWaitHandle.SetNativeRegisteredWaitHandle(nativeRegisteredWaitHandle);
      }
      return registeredWaitHandle;
    }

    internal static void UnsafeQueueWaitCompletion(
      CompleteWaitThreadPoolWorkItem completeWaitWorkItem)
    {
      ThreadPool.QueueWaitCompletionNative(completeWaitWorkItem);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void QueueWaitCompletionNative(
      CompleteWaitThreadPoolWorkItem completeWaitWorkItem);

    internal static void RequestWorkerThread()
    {
      if (ThreadPool.UsePortableThreadPool)
      {
        PortableThreadPool.ThreadPoolInstance.RequestWorker();
      }
      else
      {
        int num = (int) ThreadPool.RequestWorkerThreadNative();
      }
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern Interop.BOOL RequestWorkerThreadNative();

    private static void EnsureGateThreadRunning() => PortableThreadPool.EnsureGateThreadRunning();

    internal static bool PerformRuntimeSpecificGateActivities(int cpuUtilization) => ThreadPool.PerformRuntimeSpecificGateActivitiesNative(cpuUtilization) != 0;

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern Interop.BOOL PerformRuntimeSpecificGateActivitiesNative(
      int cpuUtilization);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern unsafe bool PostQueuedCompletionStatus(NativeOverlapped* overlapped);


    #nullable enable
    /// <summary>Queues an overlapped I/O operation for execution.</summary>
    /// <param name="overlapped">The <see cref="T:System.Threading.NativeOverlapped" /> structure to queue.</param>
    /// <returns>
    /// <see langword="true" /> if the operation was successfully queued to an I/O completion port; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    [SupportedOSPlatform("windows")]
    public static unsafe bool UnsafeQueueNativeOverlapped(NativeOverlapped* overlapped) => ThreadPool.PostQueuedCompletionStatus(overlapped);

    private static void UnsafeQueueUnmanagedWorkItem(IntPtr callback, IntPtr state) => ThreadPool.UnsafeQueueTimeSensitiveWorkItemInternal((IThreadPoolWorkItem) new UnmanagedThreadPoolWorkItem(callback, state));

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SetMinThreadsNative(int workerThreads, int completionPortThreads);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SetMaxThreadsNative(int workerThreads, int completionPortThreads);


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetMinThreadsNative(
      out int workerThreads,
      out int completionPortThreads);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetMaxThreadsNative(
      out int workerThreads,
      out int completionPortThreads);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetAvailableThreadsNative(
      out int workerThreads,
      out int completionPortThreads);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool NotifyWorkItemComplete(
      object threadLocalCompletionCountObject,
      int currentTimeMs)
    {
      return ThreadPool.UsePortableThreadPool ? PortableThreadPool.ThreadPoolInstance.NotifyWorkItemComplete(threadLocalCompletionCountObject, currentTimeMs) : ThreadPool.NotifyWorkItemCompleteNative();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool NotifyWorkItemCompleteNative();

    internal static void ReportThreadStatus(bool isWorking)
    {
      if (ThreadPool.UsePortableThreadPool)
        PortableThreadPool.ThreadPoolInstance.ReportThreadStatus(isWorking);
      else
        ThreadPool.ReportThreadStatusNative(isWorking);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ReportThreadStatusNative(bool isWorking);

    internal static void NotifyWorkItemProgress()
    {
      if (ThreadPool.UsePortableThreadPool)
        PortableThreadPool.ThreadPoolInstance.NotifyWorkItemProgress();
      else
        ThreadPool.NotifyWorkItemProgressNative();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void NotifyWorkItemProgressNative();

    internal static bool NotifyThreadBlocked() => ThreadPool.UsePortableThreadPool && PortableThreadPool.ThreadPoolInstance.NotifyThreadBlocked();

    internal static void NotifyThreadUnblocked() => PortableThreadPool.ThreadPoolInstance.NotifyThreadUnblocked();

    internal static object GetOrCreateThreadLocalCompletionCountObject() => !ThreadPool.UsePortableThreadPool ? (object) null : PortableThreadPool.ThreadPoolInstance.GetOrCreateThreadLocalCompletionCountObject();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool GetEnableWorkerTrackingNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr RegisterWaitForSingleObjectNative(
      WaitHandle waitHandle,
      object state,
      uint timeOutInterval,
      bool executeOnlyOnce,
      RegisteredWaitHandle registeredWaitHandle);

    /// <summary>Binds an operating system handle to the <see cref="T:System.Threading.ThreadPool" />.</summary>
    /// <param name="osHandle">An <see cref="T:System.IntPtr" /> that holds the handle. The handle must have been opened for overlapped I/O on the unmanaged side.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>
    /// <see langword="true" /> if the handle is bound; otherwise, <see langword="false" />.</returns>
    [Obsolete("ThreadPool.BindHandle(IntPtr) has been deprecated. Use ThreadPool.BindHandle(SafeHandle) instead.")]
    [SupportedOSPlatform("windows")]
    public static bool BindHandle(IntPtr osHandle) => ThreadPool.BindIOCompletionCallbackNative(osHandle);


    #nullable enable
    /// <summary>Binds an operating system handle to the <see cref="T:System.Threading.ThreadPool" />.</summary>
    /// <param name="osHandle">A <see cref="T:System.Runtime.InteropServices.SafeHandle" /> that holds the operating system handle. The handle must have been opened for overlapped I/O on the unmanaged side.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="osHandle" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the handle is bound; otherwise, <see langword="false" />.</returns>
    [SupportedOSPlatform("windows")]
    public static bool BindHandle(SafeHandle osHandle)
    {
      if (osHandle == null)
        throw new ArgumentNullException(nameof (osHandle));
      bool success = false;
      try
      {
        osHandle.DangerousAddRef(ref success);
        return ThreadPool.BindIOCompletionCallbackNative(osHandle.DangerousGetHandle());
      }
      finally
      {
        if (success)
          osHandle.DangerousRelease();
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool BindIOCompletionCallbackNative(IntPtr fileHandle);

    internal static bool EnableWorkerTracking => ThreadPool.IsWorkerTrackingEnabledInConfig && EventSource.IsSupported;

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 32-bit unsigned integer for the time-out in milliseconds.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object passed to the delegate.</param>
    /// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that can be used to cancel the registered wait operation.</returns>
    [CLSCompliant(false)]
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle RegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      uint millisecondsTimeOutInterval,
      bool executeOnlyOnce)
    {
      if (millisecondsTimeOutInterval > (uint) int.MaxValue && millisecondsTimeOutInterval != uint.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_LessEqualToIntegerMaxVal);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, millisecondsTimeOutInterval, executeOnlyOnce, true);
    }

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 32-bit unsigned integer for the time-out in milliseconds. This method does not propagate the calling stack to the worker thread.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object that is passed to the delegate.</param>
    /// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
    [CLSCompliant(false)]
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      uint millisecondsTimeOutInterval,
      bool executeOnlyOnce)
    {
      if (millisecondsTimeOutInterval > (uint) int.MaxValue && millisecondsTimeOutInterval != uint.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, millisecondsTimeOutInterval, executeOnlyOnce, false);
    }

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 32-bit signed integer for the time-out in milliseconds.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object that is passed to the delegate.</param>
    /// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that encapsulates the native handle.</returns>
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle RegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      int millisecondsTimeOutInterval,
      bool executeOnlyOnce)
    {
      if (millisecondsTimeOutInterval < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint) millisecondsTimeOutInterval, executeOnlyOnce, true);
    }

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, using a 32-bit signed integer for the time-out in milliseconds. This method does not propagate the calling stack to the worker thread.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object that is passed to the delegate.</param>
    /// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      int millisecondsTimeOutInterval,
      bool executeOnlyOnce)
    {
      if (millisecondsTimeOutInterval < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint) millisecondsTimeOutInterval, executeOnlyOnce, false);
    }

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 64-bit signed integer for the time-out in milliseconds.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object passed to the delegate.</param>
    /// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that encapsulates the native handle.</returns>
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle RegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      long millisecondsTimeOutInterval,
      bool executeOnlyOnce)
    {
      if (millisecondsTimeOutInterval < -1L)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (millisecondsTimeOutInterval > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_LessEqualToIntegerMaxVal);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint) millisecondsTimeOutInterval, executeOnlyOnce, true);
    }

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 64-bit signed integer for the time-out in milliseconds. This method does not propagate the calling stack to the worker thread.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object that is passed to the delegate.</param>
    /// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      long millisecondsTimeOutInterval,
      bool executeOnlyOnce)
    {
      if (millisecondsTimeOutInterval < -1L)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (millisecondsTimeOutInterval > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeOutInterval), SR.ArgumentOutOfRange_LessEqualToIntegerMaxVal);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint) millisecondsTimeOutInterval, executeOnlyOnce, false);
    }

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a <see cref="T:System.TimeSpan" /> value for the time-out.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object passed to the delegate.</param>
    /// <param name="timeout">The time-out represented by a <see cref="T:System.TimeSpan" />. If <paramref name="timeout" /> is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="timeout" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="timeout" /> parameter is less than -1.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="timeout" /> parameter is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that encapsulates the native handle.</returns>
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle RegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      TimeSpan timeout,
      bool executeOnlyOnce)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L)
        throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_LessEqualToIntegerMaxVal);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint) totalMilliseconds, executeOnlyOnce, true);
    }

    /// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a <see cref="T:System.TimeSpan" /> value for the time-out. This method does not propagate the calling stack to the worker thread.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
    /// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
    /// <param name="state">The object that is passed to the delegate.</param>
    /// <param name="timeout">The time-out represented by a <see cref="T:System.TimeSpan" />. If <paramref name="timeout" /> is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="timeout" /> is -1, the function's time-out interval never elapses.</param>
    /// <param name="executeOnlyOnce">
    /// <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="timeout" /> parameter is less than -1.</exception>
    /// <exception cref="T:System.NotSupportedException">The <paramref name="timeout" /> parameter is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
    [UnsupportedOSPlatform("browser")]
    public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(
      WaitHandle waitObject,
      WaitOrTimerCallback callBack,
      object? state,
      TimeSpan timeout,
      bool executeOnlyOnce)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L)
        throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), SR.ArgumentOutOfRange_LessEqualToIntegerMaxVal);
      return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint) totalMilliseconds, executeOnlyOnce, false);
    }

    /// <summary>Queues a method for execution. The method executes when a thread pool thread becomes available.</summary>
    /// <param name="callBack">A <see cref="T:System.Threading.WaitCallback" /> that represents the method to be executed.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callBack" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The common language runtime (CLR) is hosted, and the host does not support this action.</exception>
    /// <returns>
    /// <see langword="true" /> if the method is successfully queued; <see cref="T:System.NotSupportedException" /> is thrown if the work item could not be queued.</returns>
    public static bool QueueUserWorkItem(WaitCallback callBack) => ThreadPool.QueueUserWorkItem(callBack, (object) null);

    /// <summary>Queues a method for execution, and specifies an object containing data to be used by the method. The method executes when a thread pool thread becomes available.</summary>
    /// <param name="callBack">A <see cref="T:System.Threading.WaitCallback" /> representing the method to execute.</param>
    /// <param name="state">An object containing data to be used by the method.</param>
    /// <exception cref="T:System.NotSupportedException">The common language runtime (CLR) is hosted, and the host does not support this action.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callBack" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the method is successfully queued; <see cref="T:System.NotSupportedException" /> is thrown if the work item could not be queued.</returns>
    public static bool QueueUserWorkItem(WaitCallback callBack, object? state)
    {
      if (callBack == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.callBack);
      ExecutionContext context = ExecutionContext.Capture();
      object callback = context == null || context.IsDefault ? (object) new QueueUserWorkItemCallbackDefaultContext(callBack, state) : (object) new QueueUserWorkItemCallback(callBack, state, context);
      ThreadPool.s_workQueue.Enqueue(callback, true);
      return true;
    }

    /// <summary>Queues a method specified by an <see cref="T:System.Action`1" /> delegate for execution, and provides data to be used by the method. The method executes when a thread pool thread becomes available.</summary>
    /// <param name="callBack">An <see cref="T:System.Action`1" /> representing the method to execute.</param>
    /// <param name="state">An object containing data to be used by the method.</param>
    /// <param name="preferLocal">
    /// <see langword="true" /> to prefer queueing the work item in a queue close to the current thread; <see langword="false" /> to prefer queueing the work item to the thread pool's shared queue.</param>
    /// <typeparam name="TState">The type of elements of <paramref name="state" />.</typeparam>
    /// <returns>
    /// <see langword="true" /> if the method is successfully queued; <see cref="T:System.NotSupportedException" /> is thrown if the work item could not be queued.</returns>
    public static bool QueueUserWorkItem<TState>(
      Action<TState> callBack,
      TState state,
      bool preferLocal)
    {
      if (callBack == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.callBack);
      ExecutionContext context = ExecutionContext.Capture();
      object callback = context == null || context.IsDefault ? (object) new QueueUserWorkItemCallbackDefaultContext<TState>(callBack, state) : (object) new QueueUserWorkItemCallback<TState>(callBack, state, context);
      ThreadPool.s_workQueue.Enqueue(callback, !preferLocal);
      return true;
    }

    /// <summary>Queues a method specified by an <see cref="T:System.Action`1" /> delegate for execution, and specifies an object containing data to be used by the method. The method executes when a thread pool thread becomes available.</summary>
    /// <param name="callBack">A delegate representing the method to execute.</param>
    /// <param name="state">An object containing data to be used by the method.</param>
    /// <param name="preferLocal">
    /// <see langword="true" /> to prefer queueing the work item in a queue close to the current thread; <see langword="false" /> to prefer queueing the work item to the thread pool's shared queue.</param>
    /// <typeparam name="TState">The type of elements of <paramref name="state" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The work item could not be queued.</exception>
    /// <returns>
    /// <see langword="true" /> if the method is successfully queued; <see cref="T:System.NotSupportedException" /> is thrown if the work item could not be queued.</returns>
    public static bool UnsafeQueueUserWorkItem<TState>(
      Action<TState> callBack,
      TState state,
      bool preferLocal)
    {
      if (callBack == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.callBack);
      if (callBack == ThreadPool.s_invokeAsyncStateMachineBox)
      {
        if (!((object) state is IAsyncStateMachineBox))
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.state);
        ThreadPool.UnsafeQueueUserWorkItemInternal((object) state, preferLocal);
        return true;
      }
      ThreadPool.s_workQueue.Enqueue((object) new QueueUserWorkItemCallbackDefaultContext<TState>(callBack, state), !preferLocal);
      return true;
    }

    /// <summary>Queues the specified delegate to the thread pool, but does not propagate the calling stack to the worker thread.</summary>
    /// <param name="callBack">A <see cref="T:System.Threading.WaitCallback" /> that represents the delegate to invoke when a thread in the thread pool picks up the work item.</param>
    /// <param name="state">The object that is passed to the delegate when serviced from the thread pool.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.ApplicationException">An out-of-memory condition was encountered.</exception>
    /// <exception cref="T:System.OutOfMemoryException">The work item could not be queued.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callBack" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the method succeeds; <see cref="T:System.OutOfMemoryException" /> is thrown if the work item could not be queued.</returns>
    public static bool UnsafeQueueUserWorkItem(WaitCallback callBack, object? state)
    {
      if (callBack == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.callBack);
      object callback = (object) new QueueUserWorkItemCallbackDefaultContext(callBack, state);
      ThreadPool.s_workQueue.Enqueue(callback, true);
      return true;
    }

    /// <summary>Queues the specified work item object to the thread pool.</summary>
    /// <param name="callBack">The work item to invoke when a thread in the thread pool picks up the work item.</param>
    /// <param name="preferLocal">
    /// <see langword="true" /> to prefer queueing the work item in a queue close to the current thread; <see langword="false" /> to prefer queueing the work item to the thread pool's shared queue.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The work item is a <see cref="T:System.Threading.Tasks.Task" />.</exception>
    /// <exception cref="T:System.OutOfMemoryException">The work item could not be queued.</exception>
    /// <returns>
    /// <see langword="true" /> if the method succeeds; <see cref="T:System.OutOfMemoryException" /> is thrown if the work item could not be queued.</returns>
    public static bool UnsafeQueueUserWorkItem(IThreadPoolWorkItem callBack, bool preferLocal)
    {
      if (callBack == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.callBack);
      if (callBack is Task)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.callBack);
      ThreadPool.UnsafeQueueUserWorkItemInternal((object) callBack, preferLocal);
      return true;
    }


    #nullable disable
    internal static void UnsafeQueueUserWorkItemInternal(object callBack, bool preferLocal) => ThreadPool.s_workQueue.Enqueue(callBack, !preferLocal);

    internal static void UnsafeQueueTimeSensitiveWorkItemInternal(
      IThreadPoolWorkItem timeSensitiveWorkItem)
    {
      ThreadPool.s_workQueue.EnqueueTimeSensitiveWorkItem(timeSensitiveWorkItem);
    }

    internal static bool TryPopCustomWorkItem(object workItem) => ThreadPoolWorkQueue.LocalFindAndPop(workItem);

    internal static IEnumerable<object> GetQueuedWorkItems()
    {
      if (ThreadPool.SupportsTimeSensitiveWorkItems)
      {
        foreach (object timeSensitiveWork in ThreadPool.s_workQueue.timeSensitiveWorkQueue)
          yield return timeSensitiveWork;
      }
      foreach (object workItem in ThreadPool.s_workQueue.workItems)
        yield return workItem;
      ThreadPoolWorkQueue.WorkStealingQueue[] workStealingQueueArray = ThreadPoolWorkQueue.WorkStealingQueueList.Queues;
      for (int index = 0; index < workStealingQueueArray.Length; ++index)
      {
        ThreadPoolWorkQueue.WorkStealingQueue workStealingQueue = workStealingQueueArray[index];
        if (workStealingQueue != null && workStealingQueue.m_array != null)
        {
          object[] items = workStealingQueue.m_array;
          for (int i = 0; i < items.Length; ++i)
          {
            object obj = items[i];
            if (obj != null)
              yield return obj;
          }
          items = (object[]) null;
        }
      }
      workStealingQueueArray = (ThreadPoolWorkQueue.WorkStealingQueue[]) null;
    }

    internal static IEnumerable<object> GetLocallyQueuedWorkItems()
    {
      ThreadPoolWorkQueue.WorkStealingQueue workStealingQueue = ThreadPoolWorkQueueThreadLocals.threadLocals?.workStealingQueue;
      if (workStealingQueue != null && workStealingQueue.m_array != null)
      {
        object[] items = workStealingQueue.m_array;
        for (int i = 0; i < items.Length; ++i)
        {
          object obj = items[i];
          if (obj != null)
            yield return obj;
        }
        items = (object[]) null;
      }
    }

    internal static IEnumerable<object> GetGloballyQueuedWorkItems()
    {
      if (ThreadPool.SupportsTimeSensitiveWorkItems)
      {
        foreach (object timeSensitiveWork in ThreadPool.s_workQueue.timeSensitiveWorkQueue)
          yield return timeSensitiveWork;
      }
      foreach (object workItem in ThreadPool.s_workQueue.workItems)
        yield return workItem;
    }

    private static object[] ToObjectArray(IEnumerable<object> workitems)
    {
      int length = 0;
      foreach (object workitem in workitems)
        ++length;
      object[] objectArray = new object[length];
      int index = 0;
      foreach (object workitem in workitems)
      {
        if (index < objectArray.Length)
          objectArray[index] = workitem;
        ++index;
      }
      return objectArray;
    }

    internal static object[] GetQueuedWorkItemsForDebugger() => ThreadPool.ToObjectArray(ThreadPool.GetQueuedWorkItems());

    internal static object[] GetGloballyQueuedWorkItemsForDebugger() => ThreadPool.ToObjectArray(ThreadPool.GetGloballyQueuedWorkItems());

    internal static object[] GetLocallyQueuedWorkItemsForDebugger() => ThreadPool.ToObjectArray(ThreadPool.GetLocallyQueuedWorkItems());

    /// <summary>Gets the number of work items that are currently queued to be processed.</summary>
    /// <returns>The number of work items that are currently queued to be processed.</returns>
    public static long PendingWorkItemCount => ThreadPoolWorkQueue.LocalCount + ThreadPool.s_workQueue.GlobalCount + ThreadPool.PendingUnmanagedWorkItemCount;
  }
}
