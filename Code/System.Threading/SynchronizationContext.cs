// Decompiled with JetBrains decompiler
// Type: System.Threading.SynchronizationContext
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Provides the basic functionality for propagating a synchronization context in various synchronization models.</summary>
  public class SynchronizationContext
  {
    private bool _requireWaitNotification;


    #nullable disable
    private static int InvokeWaitMethodHelper(
      SynchronizationContext syncContext,
      IntPtr[] waitHandles,
      bool waitAll,
      int millisecondsTimeout)
    {
      return syncContext.Wait(waitHandles, waitAll, millisecondsTimeout);
    }


    #nullable enable
    /// <summary>Gets the synchronization context for the current thread.</summary>
    /// <returns>A <see cref="T:System.Threading.SynchronizationContext" /> object representing the current synchronization context.</returns>
    public static SynchronizationContext? Current => Thread.CurrentThread._synchronizationContext;

    /// <summary>Sets notification that wait notification is required and prepares the callback method so it can be called more reliably when a wait occurs.</summary>
    protected void SetWaitNotificationRequired() => this._requireWaitNotification = true;

    /// <summary>Determines if wait notification is required.</summary>
    /// <returns>
    /// <see langword="true" /> if wait notification is required; otherwise, <see langword="false" />.</returns>
    public bool IsWaitNotificationRequired() => this._requireWaitNotification;

    /// <summary>When overridden in a derived class, dispatches a synchronous message to a synchronization context.</summary>
    /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
    /// <param name="state">The object passed to the delegate.</param>
    /// <exception cref="T:System.NotSupportedException">The method was called in a Windows Store app. The implementation of <see cref="T:System.Threading.SynchronizationContext" /> for Windows Store apps does not support the <see cref="M:System.Threading.SynchronizationContext.Send(System.Threading.SendOrPostCallback,System.Object)" /> method.</exception>
    public virtual void Send(SendOrPostCallback d, object? state) => d(state);

    /// <summary>When overridden in a derived class, dispatches an asynchronous message to a synchronization context.</summary>
    /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
    /// <param name="state">The object passed to the delegate.</param>
    public virtual void Post(SendOrPostCallback d, object? state) => ThreadPool.QueueUserWorkItem<(SendOrPostCallback, object)>((Action<(SendOrPostCallback, object)>) (s => s.d(s.state)), (d, state), false);

    /// <summary>When overridden in a derived class, responds to the notification that an operation has started.</summary>
    public virtual void OperationStarted()
    {
    }

    /// <summary>When overridden in a derived class, responds to the notification that an operation has completed.</summary>
    public virtual void OperationCompleted()
    {
    }

    /// <summary>Waits for any or all the elements in the specified array to receive a signal.</summary>
    /// <param name="waitHandles">An array of type <see cref="T:System.IntPtr" /> that contains the native operating system handles.</param>
    /// <param name="waitAll">
    /// <see langword="true" /> to wait for all handles; <see langword="false" /> to wait for any handle.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="waitHandles" /> is null.</exception>
    /// <returns>The array index of the object that satisfied the wait.</returns>
    [CLSCompliant(false)]
    public virtual int Wait(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout) => SynchronizationContext.WaitHelper(waitHandles, waitAll, millisecondsTimeout);

    /// <summary>Helper function that waits for any or all the elements in the specified array to receive a signal.</summary>
    /// <param name="waitHandles">An array of type <see cref="T:System.IntPtr" /> that contains the native operating system handles.</param>
    /// <param name="waitAll">
    /// <see langword="true" /> to wait for all handles;  <see langword="false" /> to wait for any handle.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <returns>The array index of the object that satisfied the wait.</returns>
    [CLSCompliant(false)]
    protected static int WaitHelper(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
    {
      if (waitHandles == null)
        throw new ArgumentNullException(nameof (waitHandles));
      return WaitHandle.WaitMultipleIgnoringSyncContext((Span<IntPtr>) waitHandles, waitAll, millisecondsTimeout);
    }

    /// <summary>Sets the current synchronization context.</summary>
    /// <param name="syncContext">The <see cref="T:System.Threading.SynchronizationContext" /> object to be set.</param>
    public static void SetSynchronizationContext(SynchronizationContext? syncContext) => Thread.CurrentThread._synchronizationContext = syncContext;

    /// <summary>When overridden in a derived class, creates a copy of the synchronization context.</summary>
    /// <returns>A new <see cref="T:System.Threading.SynchronizationContext" /> object.</returns>
    public virtual SynchronizationContext CreateCopy() => new SynchronizationContext();
  }
}
