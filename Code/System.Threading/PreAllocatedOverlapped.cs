// Decompiled with JetBrains decompiler
// Type: System.Threading.PreAllocatedOverlapped
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Overlapped.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Represents pre-allocated state for native overlapped I/O operations.</summary>
  public sealed class PreAllocatedOverlapped : IDisposable, IDeferredDisposable
  {

    #nullable disable
    internal readonly ThreadPoolBoundHandleOverlapped _overlapped;
    private DeferredDisposableLifetime<PreAllocatedOverlapped> _lifetime;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.PreAllocatedOverlapped" /> class and specifies a delegate to invoke when each asynchronous I/O operation is complete, a user-provided object that provides context, and managed objects that serve as buffers.</summary>
    /// <param name="callback">A delegate that represents the callback method to invoke when each asynchronous I/O operation completes.</param>
    /// <param name="state">A user-supplied object that distinguishes the <see cref="T:System.Threading.NativeOverlapped" /> instance produced from this object from other <see cref="T:System.Threading.NativeOverlapped" /> instances. Its value can be <see langword="null" />.</param>
    /// <param name="pinData">An object or array of objects that represent the input or output buffer for the operations. Each object represents a buffer, such as an array of bytes. Its value can be <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This method was called after the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> was disposed.</exception>
    [CLSCompliant(false)]
    public PreAllocatedOverlapped(IOCompletionCallback callback, object? state, object? pinData)
      : this(callback, state, pinData, true)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.PreAllocatedOverlapped" /> class, specifying a delegate that is invoked when each asynchronous I/O operation is complete, a user-provided object providing context, and managed objects that serve as buffers.</summary>
    /// <param name="callback">An <see cref="T:System.Threading.IOCompletionCallback" /> delegate that represents the callback method invoked when each asynchronous I/O operation completes.</param>
    /// <param name="state">A user-provided object that distinguishes <see cref="T:System.Threading.NativeOverlapped" /> instance produced from this object from other <see cref="T:System.Threading.NativeOverlapped" /> instances. Can be <see langword="null" />.</param>
    /// <param name="pinData">An object or array of objects representing the input or output buffer for the operations. Each object represents a buffer, for example an array of bytes.  Can be <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="callback" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This method was called after the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> was disposed.</exception>
    /// <returns>The new <see cref="T:System.Threading.PreAllocatedOverlapped" /> instance.</returns>
    [CLSCompliant(false)]
    public static PreAllocatedOverlapped UnsafeCreate(
      IOCompletionCallback callback,
      object? state,
      object? pinData)
    {
      return new PreAllocatedOverlapped(callback, state, pinData, false);
    }


    #nullable disable
    private PreAllocatedOverlapped(
      IOCompletionCallback callback,
      object state,
      object pinData,
      bool flowExecutionContext)
    {
      if (callback == null)
        throw new ArgumentNullException(nameof (callback));
      this._overlapped = new ThreadPoolBoundHandleOverlapped(callback, state, pinData, this, flowExecutionContext);
    }

    internal bool AddRef() => this._lifetime.AddRef();

    internal void Release() => this._lifetime.Release(this);

    /// <summary>Frees the resources associated with this <see cref="T:System.Threading.PreAllocatedOverlapped" /> instance.</summary>
    public void Dispose()
    {
      this._lifetime.Dispose(this);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Frees unmanaged resources before the current instance is reclaimed by garbage collection.</summary>
    ~PreAllocatedOverlapped() => this.Dispose();

    unsafe void IDeferredDisposable.OnFinalRelease(bool disposed)
    {
      if (this._overlapped == null)
        return;
      if (disposed)
      {
        Overlapped.Free(this._overlapped._nativeOverlapped);
      }
      else
      {
        this._overlapped._boundHandle = (ThreadPoolBoundHandle) null;
        this._overlapped._completed = false;
        *this._overlapped._nativeOverlapped = new NativeOverlapped();
      }
    }

    internal bool IsUserObject(byte[] buffer) => this._overlapped.IsUserObject(buffer);
  }
}
