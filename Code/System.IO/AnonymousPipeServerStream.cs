// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.AnonymousPipeServerStream
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Exposes a stream around an anonymous pipe, which supports both synchronous and asynchronous read and write operations.</summary>
  public sealed class AnonymousPipeServerStream : PipeStream
  {

    #nullable disable
    private SafePipeHandle _clientHandle;
    private bool _clientHandleExposed;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> class.</summary>
    public AnonymousPipeServerStream()
      : this(PipeDirection.Out, HandleInheritability.None, 0)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> class with the specified pipe direction.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.
    /// 
    /// Anonymous pipes can only be in one direction, so <paramref name="direction" /> cannot be set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</param>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="direction" /> is set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</exception>
    public AnonymousPipeServerStream(PipeDirection direction)
      : this(direction, HandleInheritability.None, 0)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> class with the specified pipe direction and inheritability mode.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.
    /// 
    /// Anonymous pipes can only be in one direction, so <paramref name="direction" /> cannot be set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</param>
    /// <param name="inheritability">One of the enumeration values that determines whether the underlying handle can be inherited by child processes. Must be set to either <see cref="F:System.IO.HandleInheritability.None" /> or <see cref="F:System.IO.HandleInheritability.Inheritable" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="inheritability" /> is not set to either <see cref="F:System.IO.HandleInheritability.None" /> or <see cref="F:System.IO.HandleInheritability.Inheritable" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="direction" /> is set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</exception>
    public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability)
      : this(direction, inheritability, 0)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> class from the specified pipe handles.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.
    /// 
    /// Anonymous pipes can only be in one direction, so <paramref name="direction" /> cannot be set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</param>
    /// <param name="serverSafePipeHandle">A safe handle for the pipe that this <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> object will encapsulate.</param>
    /// <param name="clientSafePipeHandle">A safe handle for the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="serverSafePipeHandle" /> or <paramref name="clientSafePipeHandle" /> is an invalid handle.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="serverSafePipeHandle" /> or <paramref name="clientSafePipeHandle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="direction" /> is set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, has occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    public AnonymousPipeServerStream(
      PipeDirection direction,
      SafePipeHandle serverSafePipeHandle,
      SafePipeHandle clientSafePipeHandle)
      : base(direction, 0)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.NotSupported_AnonymousPipeUnidirectional);
      if (serverSafePipeHandle == null)
        throw new ArgumentNullException(nameof (serverSafePipeHandle));
      if (clientSafePipeHandle == null)
        throw new ArgumentNullException(nameof (clientSafePipeHandle));
      if (serverSafePipeHandle.IsInvalid)
        throw new ArgumentException(SR.Argument_InvalidHandle, nameof (serverSafePipeHandle));
      if (clientSafePipeHandle.IsInvalid)
        throw new ArgumentException(SR.Argument_InvalidHandle, nameof (clientSafePipeHandle));
      this.ValidateHandleIsPipe(serverSafePipeHandle);
      this.ValidateHandleIsPipe(clientSafePipeHandle);
      this.InitializeHandle(serverSafePipeHandle, true, false);
      this._clientHandle = clientSafePipeHandle;
      this._clientHandleExposed = true;
      this.State = PipeState.Connected;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> class with the specified pipe direction, inheritability mode, and buffer size.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.
    /// 
    /// Anonymous pipes can only be in one direction, so <paramref name="direction" /> cannot be set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</param>
    /// <param name="inheritability">One of the enumeration values that determines whether the underlying handle can be inherited by child processes. Must be set to either <see cref="F:System.IO.HandleInheritability.None" /> or <see cref="F:System.IO.HandleInheritability.Inheritable" />.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than or equal to 0.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="inheritability" /> is not set to either <see cref="F:System.IO.HandleInheritability.None" /> or <see cref="F:System.IO.HandleInheritability.Inheritable" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="bufferSize" /> is less than 0.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="direction" /> is set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</exception>
    public AnonymousPipeServerStream(
      PipeDirection direction,
      HandleInheritability inheritability,
      int bufferSize)
      : base(direction, bufferSize)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.NotSupported_AnonymousPipeUnidirectional);
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException(nameof (inheritability), SR.ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable);
      this.Create(direction, inheritability, bufferSize);
    }

    /// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> instance is reclaimed by garbage collection.</summary>
    ~AnonymousPipeServerStream() => this.Dispose(false);

    /// <summary>Gets the connected <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object's handle as a string.</summary>
    /// <returns>A string that represents the connected <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object's handle.</returns>
    public string GetClientHandleAsString()
    {
      this._clientHandleExposed = true;
      GC.SuppressFinalize((object) this._clientHandle);
      return this._clientHandle.DangerousGetHandle().ToString();
    }

    /// <summary>Gets the safe handle for the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object that is currently connected to the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> object.</summary>
    /// <returns>A handle for the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object that is currently connected to the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> object.</returns>
    public SafePipeHandle ClientSafePipeHandle
    {
      get
      {
        this._clientHandleExposed = true;
        return this._clientHandle;
      }
    }

    /// <summary>Closes the local copy of the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object's handle.</summary>
    public void DisposeLocalCopyOfClientHandle()
    {
      if (this._clientHandle == null || this._clientHandle.IsClosed)
        return;
      this._clientHandle.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this._clientHandleExposed || this._clientHandle == null || this._clientHandle.IsClosed)
          return;
        this._clientHandle.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    /// <summary>Gets the pipe transmission mode that is supported by the current pipe.</summary>
    /// <returns>The <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> that is supported by the current pipe.</returns>
    public override PipeTransmissionMode TransmissionMode => PipeTransmissionMode.Byte;

    /// <summary>Sets the reading mode for the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> object. For anonymous pipes, transmission mode must be <see cref="F:System.IO.Pipes.PipeTransmissionMode.Byte" />.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The transmission mode is not valid. For anonymous pipes, only <see cref="F:System.IO.Pipes.PipeTransmissionMode.Byte" /> is supported.</exception>
    /// <exception cref="T:System.NotSupportedException">The property is set to <see cref="F:System.IO.Pipes.PipeTransmissionMode.Message" />, which is not supported for anonymous pipes.</exception>
    /// <exception cref="T:System.IO.IOException">The connection is broken or another I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <returns>The reading mode for the <see cref="T:System.IO.Pipes.AnonymousPipeServerStream" /> object.</returns>
    public override PipeTransmissionMode ReadMode
    {
      set
      {
        this.CheckPipePropertyOperations();
        if (value < PipeTransmissionMode.Byte || value > PipeTransmissionMode.Message)
          throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_TransmissionModeByteOrMsg);
        if (value == PipeTransmissionMode.Message)
          throw new NotSupportedException(SR.NotSupported_AnonymousPipeMessagesNotSupported);
      }
    }


    #nullable disable
    internal AnonymousPipeServerStream(
      PipeDirection direction,
      HandleInheritability inheritability,
      int bufferSize,
      PipeSecurity pipeSecurity)
      : base(direction, bufferSize)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.NotSupported_AnonymousPipeUnidirectional);
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException(nameof (inheritability), SR.ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable);
      this.Create(direction, inheritability, bufferSize, pipeSecurity);
    }

    private void Create(
      PipeDirection direction,
      HandleInheritability inheritability,
      int bufferSize)
    {
      this.Create(direction, inheritability, bufferSize, (PipeSecurity) null);
    }

    private void Create(
      PipeDirection direction,
      HandleInheritability inheritability,
      int bufferSize,
      PipeSecurity pipeSecurity)
    {
      GCHandle pinningHandle = new GCHandle();
      SafePipeHandle hSourceHandle;
      bool flag;
      try
      {
        Interop.Kernel32.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(inheritability, pipeSecurity, ref pinningHandle);
        flag = direction != PipeDirection.In ? Interop.Kernel32.CreatePipe(out this._clientHandle, out hSourceHandle, ref secAttrs, bufferSize) : Interop.Kernel32.CreatePipe(out hSourceHandle, out this._clientHandle, ref secAttrs, bufferSize);
      }
      finally
      {
        if (pinningHandle.IsAllocated)
          pinningHandle.Free();
      }
      if (!flag)
        throw Win32Marshal.GetExceptionForLastWin32Error();
      SafePipeHandle lpTargetHandle;
      if (!Interop.Kernel32.DuplicateHandle(Interop.Kernel32.GetCurrentProcess(), (SafeHandle) hSourceHandle, Interop.Kernel32.GetCurrentProcess(), out lpTargetHandle, 0U, false, 2U))
        throw Win32Marshal.GetExceptionForLastWin32Error();
      hSourceHandle.Dispose();
      this.InitializeHandle(lpTargetHandle, false, false);
      this.State = PipeState.Connected;
    }
  }
}
