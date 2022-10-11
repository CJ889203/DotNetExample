// Decompiled with JetBrains decompiler
// Type: System.Threading.Overlapped
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Overlapped.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Provides a managed representation of a Win32 <c>OVERLAPPED</c> structure, including methods to transfer information from an <see cref="T:System.Threading.Overlapped" /> instance to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</summary>
  public class Overlapped
  {

    #nullable disable
    private OverlappedData _overlappedData;

    /// <summary>Initializes a new, empty instance of the <see cref="T:System.Threading.Overlapped" /> class.</summary>
    public Overlapped() => this._overlappedData = new OverlappedData(this);


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Overlapped" /> class with the specified file position, the handle to an event that is signaled when the I/O operation is complete, and an interface through which to return the results of the operation.</summary>
    /// <param name="offsetLo">The low word of the file position at which to start the transfer.</param>
    /// <param name="offsetHi">The high word of the file position at which to start the transfer.</param>
    /// <param name="hEvent">The handle to an event that is signaled when the I/O operation is complete.</param>
    /// <param name="ar">An object that implements the <see cref="T:System.IAsyncResult" /> interface and provides status information on the I/O operation.</param>
    public Overlapped(int offsetLo, int offsetHi, IntPtr hEvent, IAsyncResult? ar)
      : this()
    {
      this._overlappedData.OffsetLow = offsetLo;
      this._overlappedData.OffsetHigh = offsetHi;
      this._overlappedData.EventHandle = hEvent;
      this._overlappedData._asyncResult = ar;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Overlapped" /> class with the specified file position, the 32-bit integer handle to an event that is signaled when the I/O operation is complete, and an interface through which to return the results of the operation.</summary>
    /// <param name="offsetLo">The low word of the file position at which to start the transfer.</param>
    /// <param name="offsetHi">The high word of the file position at which to start the transfer.</param>
    /// <param name="hEvent">The handle to an event that is signaled when the I/O operation is complete.</param>
    /// <param name="ar">An object that implements the <see cref="T:System.IAsyncResult" /> interface and provides status information on the I/O operation.</param>
    [Obsolete("This constructor is not 64-bit compatible and has been deprecated. Use the constructor that accepts an IntPtr for the event handle instead.")]
    public Overlapped(int offsetLo, int offsetHi, int hEvent, IAsyncResult? ar)
      : this(offsetLo, offsetHi, new IntPtr(hEvent), ar)
    {
    }

    /// <summary>Gets or sets the object that provides status information on the I/O operation.</summary>
    /// <returns>An object that implements the <see cref="T:System.IAsyncResult" /> interface.</returns>
    public IAsyncResult? AsyncResult
    {
      get => this._overlappedData._asyncResult;
      set => this._overlappedData._asyncResult = value;
    }

    /// <summary>Gets or sets the low-order word of the file position at which to start the transfer. The file position is a byte offset from the start of the file.</summary>
    /// <returns>An <see cref="T:System.Int32" /> value representing the low word of the file position.</returns>
    public int OffsetLow
    {
      get => this._overlappedData.OffsetLow;
      set => this._overlappedData.OffsetLow = value;
    }

    /// <summary>Gets or sets the high-order word of the file position at which to start the transfer. The file position is a byte offset from the start of the file.</summary>
    /// <returns>An <see cref="T:System.Int32" /> value representing the high word of the file position.</returns>
    public int OffsetHigh
    {
      get => this._overlappedData.OffsetHigh;
      set => this._overlappedData.OffsetHigh = value;
    }

    /// <summary>Gets or sets the 32-bit integer handle to a synchronization event that is signaled when the I/O operation is complete.</summary>
    /// <returns>An <see cref="T:System.Int32" /> value representing the handle of the synchronization event.</returns>
    [Obsolete("Overlapped.EventHandle is not 64-bit compatible and has been deprecated. Use EventHandleIntPtr instead.")]
    public int EventHandle
    {
      get => this.EventHandleIntPtr.ToInt32();
      set => this.EventHandleIntPtr = new IntPtr(value);
    }

    /// <summary>Gets or sets the handle to the synchronization event that is signaled when the I/O operation is complete.</summary>
    /// <returns>An <see cref="T:System.IntPtr" /> representing the handle of the event.</returns>
    public IntPtr EventHandleIntPtr
    {
      get => this._overlappedData.EventHandle;
      set => this._overlappedData.EventHandle = value;
    }

    /// <summary>Packs the current instance into a <see cref="T:System.Threading.NativeOverlapped" /> structure, specifying the delegate to be invoked when the asynchronous I/O operation is complete.</summary>
    /// <param name="iocb">An <see cref="T:System.Threading.IOCompletionCallback" /> delegate that represents the callback method invoked when the asynchronous I/O operation completes.</param>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Threading.Overlapped" /> has already been packed.</exception>
    /// <returns>An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</returns>
    [Obsolete("This overload is not safe and has been deprecated. Use Pack(IOCompletionCallback?, object?) instead.")]
    [CLSCompliant(false)]
    public unsafe NativeOverlapped* Pack(IOCompletionCallback? iocb) => this.Pack(iocb, (object) null);

    /// <summary>Packs the current instance into a <see cref="T:System.Threading.NativeOverlapped" /> structure, specifying a delegate that is invoked when the asynchronous I/O operation is complete and a managed object that serves as a buffer.</summary>
    /// <param name="iocb">An <see cref="T:System.Threading.IOCompletionCallback" /> delegate that represents the callback method invoked when the asynchronous I/O operation completes.</param>
    /// <param name="userData">An object or array of objects representing the input or output buffer for the operation. Each object represents a buffer, for example an array of bytes.</param>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Threading.Overlapped" /> has already been packed.</exception>
    /// <returns>An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</returns>
    [CLSCompliant(false)]
    public unsafe NativeOverlapped* Pack(IOCompletionCallback? iocb, object? userData) => this._overlappedData.Pack(iocb, userData);

    /// <summary>Packs the current instance into a <see cref="T:System.Threading.NativeOverlapped" /> structure specifying the delegate to invoke when the asynchronous I/O operation is complete. Does not propagate the calling stack.</summary>
    /// <param name="iocb">An <see cref="T:System.Threading.IOCompletionCallback" /> delegate that represents the callback method invoked when the asynchronous I/O operation completes.</param>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Threading.Overlapped" /> has already been packed.</exception>
    /// <returns>An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</returns>
    [Obsolete("This overload is not safe and has been deprecated. Use UnsafePack(IOCompletionCallback?, object?) instead.")]
    [CLSCompliant(false)]
    public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback? iocb) => this.UnsafePack(iocb, (object) null);

    /// <summary>Packs the current instance into a <see cref="T:System.Threading.NativeOverlapped" /> structure, specifying the delegate to invoke when the asynchronous I/O operation is complete and the managed object that serves as a buffer. Does not propagate the calling stack.</summary>
    /// <param name="iocb">An <see cref="T:System.Threading.IOCompletionCallback" /> delegate that represents the callback method invoked when the asynchronous I/O operation completes.</param>
    /// <param name="userData">An object or array of objects representing the input or output buffer for the operation. Each object represents a buffer, for example an array of bytes.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Threading.Overlapped" /> is already packed.</exception>
    /// <returns>An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</returns>
    [CLSCompliant(false)]
    public unsafe NativeOverlapped* UnsafePack(
      IOCompletionCallback? iocb,
      object? userData)
    {
      return this._overlappedData.UnsafePack(iocb, userData);
    }

    /// <summary>Unpacks the specified unmanaged <see cref="T:System.Threading.NativeOverlapped" /> structure into a managed <see cref="T:System.Threading.Overlapped" /> object.</summary>
    /// <param name="nativeOverlappedPtr">An unmanaged pointer to a <see cref="T:System.Threading.NativeOverlapped" /> structure.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="nativeOverlappedPtr" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Threading.Overlapped" /> object containing the information unpacked from the native structure.</returns>
    [CLSCompliant(false)]
    public static unsafe Overlapped Unpack(NativeOverlapped* nativeOverlappedPtr) => (IntPtr) nativeOverlappedPtr != IntPtr.Zero ? OverlappedData.GetOverlappedFromNative(nativeOverlappedPtr)._overlapped : throw new ArgumentNullException(nameof (nativeOverlappedPtr));

    /// <summary>Frees the unmanaged memory associated with a native overlapped structure allocated by the <see cref="Overload:System.Threading.Overlapped.Pack" /> method.</summary>
    /// <param name="nativeOverlappedPtr">A pointer to the <see cref="T:System.Threading.NativeOverlapped" /> structure to be freed.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="nativeOverlappedPtr" /> is <see langword="null" />.</exception>
    [CLSCompliant(false)]
    public static unsafe void Free(NativeOverlapped* nativeOverlappedPtr)
    {
      if ((IntPtr) nativeOverlappedPtr == IntPtr.Zero)
        throw new ArgumentNullException(nameof (nativeOverlappedPtr));
      OverlappedData.GetOverlappedFromNative(nativeOverlappedPtr)._overlapped._overlappedData = (OverlappedData) null;
      OverlappedData.FreeNativeOverlapped(nativeOverlappedPtr);
    }


    #nullable disable
    internal bool IsUserObject(byte[] buffer) => this._overlappedData.IsUserObject(buffer);
  }
}
