// Decompiled with JetBrains decompiler
// Type: System.Threading.RegisteredWaitHandle
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.ThreadPool.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents a handle that has been registered when calling <see cref="M:System.Threading.ThreadPool.RegisterWaitForSingleObject(System.Threading.WaitHandle,System.Threading.WaitOrTimerCallback,System.Object,System.UInt32,System.Boolean)" />. This class cannot be inherited.</summary>
  [UnsupportedOSPlatform("browser")]
  public sealed class RegisteredWaitHandle : MarshalByRefObject
  {
    private IntPtr _nativeRegisteredWaitHandle = RegisteredWaitHandle.InvalidHandleValue;
    private bool _releaseHandle;

    #nullable disable
    private static AutoResetEvent s_cachedEvent;
    private static readonly LowLevelLock s_callbackLock = new LowLevelLock();
    private int _numRequestedCallbacks;
    private bool _signalAfterCallbacksComplete;
    private bool _unregisterCalled;
    private bool _unregistered;
    private AutoResetEvent _callbacksComplete;
    private AutoResetEvent _removed;

    private static bool IsValidHandle(IntPtr handle) => handle != RegisteredWaitHandle.InvalidHandleValue && handle != IntPtr.Zero;

    internal void SetNativeRegisteredWaitHandle(IntPtr nativeRegisteredWaitHandle) => this._nativeRegisteredWaitHandle = nativeRegisteredWaitHandle;

    internal void OnBeforeRegister()
    {
      if (ThreadPool.UsePortableThreadPool)
        GC.SuppressFinalize((object) this);
      else
        this.Handle.DangerousAddRef(ref this._releaseHandle);
    }


    #nullable enable
    /// <summary>Cancels a registered wait operation issued by the <see cref="M:System.Threading.ThreadPool.RegisterWaitForSingleObject(System.Threading.WaitHandle,System.Threading.WaitOrTimerCallback,System.Object,System.UInt32,System.Boolean)" /> method.</summary>
    /// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to be signaled.</param>
    /// <returns>
    /// <see langword="true" /> if the function succeeds; otherwise, <see langword="false" />.</returns>
    public bool Unregister(WaitHandle waitObject)
    {
      if (ThreadPool.UsePortableThreadPool)
        return this.UnregisterPortable(waitObject);
      RegisteredWaitHandle.s_callbackLock.Acquire();
      try
      {
        if (!RegisteredWaitHandle.IsValidHandle(this._nativeRegisteredWaitHandle) || !RegisteredWaitHandle.UnregisterWaitNative(this._nativeRegisteredWaitHandle, (SafeHandle) waitObject?.SafeWaitHandle))
          return false;
        this._nativeRegisteredWaitHandle = RegisteredWaitHandle.InvalidHandleValue;
        if (this._releaseHandle)
        {
          this.Handle.DangerousRelease();
          this._releaseHandle = false;
        }
      }
      finally
      {
        RegisteredWaitHandle.s_callbackLock.Release();
      }
      GC.SuppressFinalize((object) this);
      return true;
    }

    ~RegisteredWaitHandle()
    {
      if (ThreadPool.UsePortableThreadPool)
        return;
      RegisteredWaitHandle.s_callbackLock.Acquire();
      try
      {
        if (!RegisteredWaitHandle.IsValidHandle(this._nativeRegisteredWaitHandle))
          return;
        RegisteredWaitHandle.WaitHandleCleanupNative(this._nativeRegisteredWaitHandle);
        this._nativeRegisteredWaitHandle = RegisteredWaitHandle.InvalidHandleValue;
        if (!this._releaseHandle)
          return;
        this.Handle.DangerousRelease();
        this._releaseHandle = false;
      }
      finally
      {
        RegisteredWaitHandle.s_callbackLock.Release();
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void WaitHandleCleanupNative(IntPtr handle);


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool UnregisterWaitNative(IntPtr handle, SafeHandle waitObject);

    internal RegisteredWaitHandle(
      WaitHandle waitHandle,
      _ThreadPoolWaitOrTimerCallback callbackHelper,
      int millisecondsTimeout,
      bool repeating)
    {
      this.Handle = waitHandle.SafeWaitHandle;
      this.Callback = callbackHelper;
      this.TimeoutDurationMs = millisecondsTimeout;
      this.Repeating = repeating;
      if (this.IsInfiniteTimeout)
        return;
      this.RestartTimeout();
    }

    private static AutoResetEvent RentEvent() => Interlocked.Exchange<AutoResetEvent>(ref RegisteredWaitHandle.s_cachedEvent, (AutoResetEvent) null) ?? new AutoResetEvent(false);

    private static void ReturnEvent(AutoResetEvent resetEvent)
    {
      if (Interlocked.CompareExchange<AutoResetEvent>(ref RegisteredWaitHandle.s_cachedEvent, resetEvent, (AutoResetEvent) null) == null)
        return;
      resetEvent.Dispose();
    }


    #nullable enable
    internal _ThreadPoolWaitOrTimerCallback Callback { get; }

    internal SafeWaitHandle Handle { get; }

    internal int TimeoutTimeMs { get; private set; }

    internal int TimeoutDurationMs { get; }

    internal bool IsInfiniteTimeout => this.TimeoutDurationMs == -1;

    internal void RestartTimeout() => this.TimeoutTimeMs = Environment.TickCount + this.TimeoutDurationMs;

    internal bool Repeating { get; }

    private SafeWaitHandle? UserUnregisterWaitHandle { get; set; }

    private IntPtr UserUnregisterWaitHandleValue { get; set; }

    private static IntPtr InvalidHandleValue => new IntPtr(-1);

    internal bool IsBlocking => this.UserUnregisterWaitHandleValue == RegisteredWaitHandle.InvalidHandleValue;

    internal PortableThreadPool.WaitThread? WaitThread { get; set; }


    #nullable disable
    private bool UnregisterPortable(WaitHandle waitObject)
    {
      RegisteredWaitHandle.s_callbackLock.Acquire();
      bool success = false;
      try
      {
        if (this._unregisterCalled)
          return false;
        this.UserUnregisterWaitHandle = waitObject?.SafeWaitHandle;
        this.UserUnregisterWaitHandle?.DangerousAddRef(ref success);
        SafeWaitHandle unregisterWaitHandle = this.UserUnregisterWaitHandle;
        this.UserUnregisterWaitHandleValue = unregisterWaitHandle != null ? unregisterWaitHandle.DangerousGetHandle() : IntPtr.Zero;
        if (this._unregistered)
        {
          this.SignalUserWaitHandle();
          return true;
        }
        if (this.IsBlocking)
          this._callbacksComplete = RegisteredWaitHandle.RentEvent();
        else
          this._removed = RegisteredWaitHandle.RentEvent();
      }
      catch (Exception ex)
      {
        if (this._removed != null)
        {
          RegisteredWaitHandle.ReturnEvent(this._removed);
          this._removed = (AutoResetEvent) null;
        }
        else if (this._callbacksComplete != null)
        {
          RegisteredWaitHandle.ReturnEvent(this._callbacksComplete);
          this._callbacksComplete = (AutoResetEvent) null;
        }
        this.UserUnregisterWaitHandleValue = IntPtr.Zero;
        if (success)
          this.UserUnregisterWaitHandle?.DangerousRelease();
        this.UserUnregisterWaitHandle = (SafeWaitHandle) null;
        throw;
      }
      finally
      {
        this._unregisterCalled = true;
        RegisteredWaitHandle.s_callbackLock.Release();
      }
      this.WaitThread.UnregisterWait(this);
      return true;
    }

    private void SignalUserWaitHandle()
    {
      SafeWaitHandle unregisterWaitHandle = this.UserUnregisterWaitHandle;
      IntPtr unregisterWaitHandleValue = this.UserUnregisterWaitHandleValue;
      try
      {
        if (!(unregisterWaitHandleValue != IntPtr.Zero) || !(unregisterWaitHandleValue != RegisteredWaitHandle.InvalidHandleValue))
          return;
        EventWaitHandle.Set(unregisterWaitHandle);
      }
      finally
      {
        unregisterWaitHandle?.DangerousRelease();
        this._callbacksComplete?.Set();
        this._unregistered = true;
      }
    }

    internal void PerformCallback(bool timedOut)
    {
      _ThreadPoolWaitOrTimerCallback.PerformWaitOrTimerCallback(this.Callback, timedOut);
      this.CompleteCallbackRequest();
    }

    internal void RequestCallback()
    {
      RegisteredWaitHandle.s_callbackLock.Acquire();
      try
      {
        ++this._numRequestedCallbacks;
      }
      finally
      {
        RegisteredWaitHandle.s_callbackLock.Release();
      }
    }

    internal void OnRemoveWait()
    {
      RegisteredWaitHandle.s_callbackLock.Acquire();
      try
      {
        this._removed?.Set();
        if (this._numRequestedCallbacks == 0)
          this.SignalUserWaitHandle();
        else
          this._signalAfterCallbacksComplete = true;
      }
      finally
      {
        RegisteredWaitHandle.s_callbackLock.Release();
      }
    }

    private void CompleteCallbackRequest()
    {
      RegisteredWaitHandle.s_callbackLock.Acquire();
      try
      {
        --this._numRequestedCallbacks;
        if (this._numRequestedCallbacks != 0 || !this._signalAfterCallbacksComplete)
          return;
        this.SignalUserWaitHandle();
      }
      finally
      {
        RegisteredWaitHandle.s_callbackLock.Release();
      }
    }

    internal void WaitForCallbacks()
    {
      this._callbacksComplete.WaitOne();
      RegisteredWaitHandle.ReturnEvent(this._callbacksComplete);
      this._callbacksComplete = (AutoResetEvent) null;
    }

    internal void WaitForRemoval()
    {
      this._removed.WaitOne();
      RegisteredWaitHandle.ReturnEvent(this._removed);
      this._removed = (AutoResetEvent) null;
    }
  }
}
