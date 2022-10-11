// Decompiled with JetBrains decompiler
// Type: System.Threading.ThreadPoolBoundHandle
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Overlapped.xml

using System.Runtime.InteropServices;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents an I/O handle that is bound to the system thread pool and enables low-level components to receive notifications for asynchronous I/O operations.</summary>
  public sealed class ThreadPoolBoundHandle : IDisposable
  {

    #nullable disable
    private readonly SafeHandle _handle;
    private bool _isDisposed;

    private ThreadPoolBoundHandle(SafeHandle handle) => this._handle = handle;


    #nullable enable
    /// <summary>Gets the bound operating system handle.</summary>
    /// <returns>An object that holds the bound operating system handle.</returns>
    public SafeHandle Handle => this._handle;

    /// <summary>Returns a <see cref="T:System.Threading.ThreadPoolBoundHandle" /> for the specified handle, which is bound to the system thread pool.</summary>
    /// <param name="handle">An object that holds the operating system handle. The handle must have been opened for overlapped I/O in unmanaged code.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="handle" /> has been disposed.
    /// 
    /// -or-
    /// 
    /// <paramref name="handle" /> does not refer to a valid I/O handle.
    /// 
    /// -or-
    /// 
    /// <paramref name="handle" /> refers to a handle that has not been opened for overlapped I/O.
    /// 
    /// -or-
    /// 
    /// <paramref name="handle" /> refers to a handle that has already been bound.</exception>
    /// <returns>A <see cref="T:System.Threading.ThreadPoolBoundHandle" /> for <paramref name="handle" />, which is bound to the system thread pool.</returns>
    public static ThreadPoolBoundHandle BindHandle(SafeHandle handle)
    {
      if (handle == null)
        throw new ArgumentNullException(nameof (handle));
      if (handle.IsClosed || handle.IsInvalid)
        throw new ArgumentException(SR.Argument_InvalidHandle, nameof (handle));
      return ThreadPoolBoundHandle.BindHandleCore(handle);
    }

    /// <summary>Returns an unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure, specifying a delegate that is invoked when the asynchronous I/O operation is complete, a user-provided object that supplies context, and managed objects that serve as buffers.</summary>
    /// <param name="callback">A delegate that represents the callback method to invoke when the asynchronous I/O operation completes.</param>
    /// <param name="state">A user-provided object that distinguishes this <see cref="T:System.Threading.NativeOverlapped" /> instance from other <see cref="T:System.Threading.NativeOverlapped" /> instances.</param>
    /// <param name="pinData">An object or array of objects that represent the input or output buffer for the operation, or <see langword="null" />. Each object represents a buffer, such an array of bytes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This method was called after the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> object was disposed.</exception>
    /// <returns>An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</returns>
    [CLSCompliant(false)]
    public unsafe NativeOverlapped* AllocateNativeOverlapped(
      IOCompletionCallback callback,
      object? state,
      object? pinData)
    {
      return this.AllocateNativeOverlapped(callback, state, pinData, true);
    }

    /// <summary>Returns an unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure, specifying a delegate that is invoked when the asynchronous I/O operation is complete, a user-provided object providing context, and managed objects that serve as buffers.</summary>
    /// <param name="callback">An <see cref="T:System.Threading.IOCompletionCallback" /> delegate that represents the callback method invoked when the asynchronous I/O operation completes.</param>
    /// <param name="state">A user-provided object that distinguishes this <see cref="T:System.Threading.NativeOverlapped" /> from other <see cref="T:System.Threading.NativeOverlapped" /> instances. Can be <see langword="null" />.</param>
    /// <param name="pinData">An object or array of objects representing the input or output buffer for the operation. Each object represents a buffer, for example an array of bytes.  Can be <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This method was called after the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> was disposed.</exception>
    /// <returns>An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</returns>
    [CLSCompliant(false)]
    public unsafe NativeOverlapped* UnsafeAllocateNativeOverlapped(
      IOCompletionCallback callback,
      object? state,
      object? pinData)
    {
      return this.AllocateNativeOverlapped(callback, state, pinData, false);
    }


    #nullable disable
    private unsafe NativeOverlapped* AllocateNativeOverlapped(
      IOCompletionCallback callback,
      object state,
      object pinData,
      bool flowExecutionContext)
    {
      if (callback == null)
        throw new ArgumentNullException(nameof (callback));
      this.EnsureNotDisposed();
      return new ThreadPoolBoundHandleOverlapped(callback, state, pinData, (PreAllocatedOverlapped) null, flowExecutionContext)
      {
        _boundHandle = this
      }._nativeOverlapped;
    }


    #nullable enable
    /// <summary>Returns an unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure using the callback state and buffers associated with the specified <see cref="T:System.Threading.PreAllocatedOverlapped" /> object.</summary>
    /// <param name="preAllocated">An object from which to create the <see cref="T:System.Threading.NativeOverlapped" /> pointer.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="preAllocated" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="preAllocated" /> is currently in use for another I/O operation.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This method was called after the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> was disposed.
    /// 
    /// -or-
    /// 
    /// This method was called after <paramref name="preAllocated" /> was disposed.</exception>
    /// <returns>An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</returns>
    [CLSCompliant(false)]
    public unsafe NativeOverlapped* AllocateNativeOverlapped(
      PreAllocatedOverlapped preAllocated)
    {
      if (preAllocated == null)
        throw new ArgumentNullException(nameof (preAllocated));
      this.EnsureNotDisposed();
      preAllocated.AddRef();
      try
      {
        ThreadPoolBoundHandleOverlapped overlapped = preAllocated._overlapped;
        overlapped._boundHandle = overlapped._boundHandle == null ? this : throw new ArgumentException(SR.Argument_PreAllocatedAlreadyAllocated, nameof (preAllocated));
        return overlapped._nativeOverlapped;
      }
      catch
      {
        preAllocated.Release();
        throw;
      }
    }

    /// <summary>Frees the memory associated with a <see cref="T:System.Threading.NativeOverlapped" /> structure allocated by the <see cref="Overload:System.Threading.ThreadPoolBoundHandle.AllocateNativeOverlapped" /> method.</summary>
    /// <param name="overlapped">An unmanaged pointer to the <see cref="T:System.Threading.NativeOverlapped" /> structure to be freed.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="overlapped" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This method was called after the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> object was disposed.</exception>
    [CLSCompliant(false)]
    public unsafe void FreeNativeOverlapped(NativeOverlapped* overlapped)
    {
      ThreadPoolBoundHandleOverlapped handleOverlapped = (IntPtr) overlapped != IntPtr.Zero ? ThreadPoolBoundHandle.GetOverlappedWrapper(overlapped) : throw new ArgumentNullException(nameof (overlapped));
      if (handleOverlapped._boundHandle != this)
        throw new ArgumentException(SR.Argument_NativeOverlappedWrongBoundHandle, nameof (overlapped));
      if (handleOverlapped._preAllocated != null)
        handleOverlapped._preAllocated.Release();
      else
        Overlapped.Free(overlapped);
    }

    /// <summary>Returns the user-provided object that was specified when the <see cref="T:System.Threading.NativeOverlapped" /> instance was allocated by calling the <see cref="M:System.Threading.ThreadPoolBoundHandle.AllocateNativeOverlapped(System.Threading.IOCompletionCallback,System.Object,System.Object)" /> method.</summary>
    /// <param name="overlapped">An unmanaged pointer to the <see cref="T:System.Threading.NativeOverlapped" /> structure from which to return the associated user-provided object.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="overlapped" /> is <see langword="null" />.</exception>
    /// <returns>A user-provided object that distinguishes this <see cref="T:System.Threading.NativeOverlapped" /> instance from other <see cref="T:System.Threading.NativeOverlapped" /> instances, or <see langword="null" /> if one was not specified when the instance was allocated by calling the <see cref="Overload:System.Threading.ThreadPoolBoundHandle.AllocateNativeOverlapped" /> method.</returns>
    [CLSCompliant(false)]
    public static unsafe object? GetNativeOverlappedState(NativeOverlapped* overlapped) => (IntPtr) overlapped != IntPtr.Zero ? ThreadPoolBoundHandle.GetOverlappedWrapper(overlapped)._userState : throw new ArgumentNullException(nameof (overlapped));


    #nullable disable
    private static unsafe ThreadPoolBoundHandleOverlapped GetOverlappedWrapper(
      NativeOverlapped* overlapped)
    {
      try
      {
        return (ThreadPoolBoundHandleOverlapped) Overlapped.Unpack(overlapped);
      }
      catch (NullReferenceException ex)
      {
        throw new ArgumentException(SR.Argument_NativeOverlappedAlreadyFree, nameof (overlapped), (Exception) ex);
      }
    }

    /// <summary>Releases all unmanaged resources used by the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> instance.</summary>
    public void Dispose() => this._isDisposed = true;

    private void EnsureNotDisposed()
    {
      if (this._isDisposed)
        throw new ObjectDisposedException(this.GetType().ToString());
    }

    private static ThreadPoolBoundHandle BindHandleCore(SafeHandle handle)
    {
      try
      {
        ThreadPool.BindHandle(handle);
      }
      catch (Exception ex)
      {
        if (ex.HResult == -2147024890)
          throw new ArgumentException(SR.Argument_InvalidHandle, nameof (handle));
        if (ex.HResult == -2147024809)
          throw new ArgumentException(SR.Argument_AlreadyBoundOrSyncHandle, nameof (handle));
        throw;
      }
      return new ThreadPoolBoundHandle(handle);
    }
  }
}
