// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadState
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml

namespace System.Threading
{
  /// <summary>Specifies the execution states of a <see cref="T:System.Threading.Thread" />.</summary>
  [Flags]
  public enum ThreadState
  {
    /// <summary>The thread has been started and not yet stopped.</summary>
    Running = 0,
    /// <summary>The thread is being requested to stop. This is for internal use only.</summary>
    StopRequested = 1,
    /// <summary>The thread is being requested to suspend.</summary>
    SuspendRequested = 2,
    /// <summary>The thread is being executed as a background thread, as opposed to a foreground thread. This state is controlled by setting the <see cref="P:System.Threading.Thread.IsBackground" /> property.</summary>
    Background = 4,
    /// <summary>The <see cref="M:System.Threading.Thread.Start" /> method has not been invoked on the thread.</summary>
    Unstarted = 8,
    /// <summary>The thread has stopped.</summary>
    Stopped = 16, // 0x00000010
    /// <summary>The thread is blocked. This could be the result of calling <see cref="M:System.Threading.Thread.Sleep(System.Int32)" /> or <see cref="M:System.Threading.Thread.Join" />, of requesting a lock - for example, by calling <see cref="M:System.Threading.Monitor.Enter(System.Object)" /> or <see cref="M:System.Threading.Monitor.Wait(System.Object,System.Int32,System.Boolean)" /> - or of waiting on a thread synchronization object such as <see cref="T:System.Threading.ManualResetEvent" />.</summary>
    WaitSleepJoin = 32, // 0x00000020
    /// <summary>The thread has been suspended.</summary>
    Suspended = 64, // 0x00000040
    /// <summary>The <see cref="M:System.Threading.Thread.Abort(System.Object)" /> method has been invoked on the thread, but the thread has not yet received the pending <see cref="T:System.Threading.ThreadAbortException" /> that will attempt to terminate it.</summary>
    AbortRequested = 128, // 0x00000080
    /// <summary>The thread state includes <see cref="F:System.Threading.ThreadState.AbortRequested" /> and the thread is now dead, but its state has not yet changed to <see cref="F:System.Threading.ThreadState.Stopped" />.</summary>
    Aborted = 256, // 0x00000100
  }
}
