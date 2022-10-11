// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.AnonymousPipeClientStream
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

using Microsoft.Win32.SafeHandles;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Exposes the client side of an anonymous pipe stream, which supports both synchronous and asynchronous read and write operations.</summary>
  public sealed class AnonymousPipeClientStream : PipeStream
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> class with the specified string representation of the pipe handle.</summary>
    /// <param name="pipeHandleAsString">A string that represents the pipe handle.</param>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="pipeHandleAsString" /> is not a valid pipe handle.</exception>
    public AnonymousPipeClientStream(string pipeHandleAsString)
      : this(PipeDirection.In, pipeHandleAsString)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> class with the specified pipe direction and a string representation of the pipe handle.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.
    /// 
    /// Anonymous pipes can only be in one direction, so <paramref name="direction" /> cannot be set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</param>
    /// <param name="pipeHandleAsString">A string that represents the pipe handle.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="pipeHandleAsString" /> is an invalid handle.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="pipeHandleAsString" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="direction" /> is set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</exception>
    public AnonymousPipeClientStream(PipeDirection direction, string pipeHandleAsString)
      : base(direction, 0)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.NotSupported_AnonymousPipeUnidirectional);
      if (pipeHandleAsString == null)
        throw new ArgumentNullException(nameof (pipeHandleAsString));
      long result = 0;
      if (!long.TryParse(pipeHandleAsString, out result))
        throw new ArgumentException(SR.Argument_InvalidHandle, nameof (pipeHandleAsString));
      SafePipeHandle safePipeHandle = new SafePipeHandle((IntPtr) result, true);
      if (safePipeHandle.IsInvalid)
        throw new ArgumentException(SR.Argument_InvalidHandle, nameof (pipeHandleAsString));
      this.Init(direction, safePipeHandle);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> class from the specified handle.</summary>
    /// <param name="direction">One of the enumeration values that determines the direction of the pipe.
    /// 
    /// Anonymous pipes can only be in one direction, so <paramref name="direction" /> cannot be set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</param>
    /// <param name="safePipeHandle">A safe handle for the pipe that this <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object will encapsulate.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="safePipeHandle" /> is not a valid handle.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="safePipeHandle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="direction" /> is set to <see cref="F:System.IO.Pipes.PipeDirection.InOut" />.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, has occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    public AnonymousPipeClientStream(PipeDirection direction, SafePipeHandle safePipeHandle)
      : base(direction, 0)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.NotSupported_AnonymousPipeUnidirectional);
      if (safePipeHandle == null)
        throw new ArgumentNullException(nameof (safePipeHandle));
      if (safePipeHandle.IsInvalid)
        throw new ArgumentException(SR.Argument_InvalidHandle, nameof (safePipeHandle));
      this.Init(direction, safePipeHandle);
    }


    #nullable disable
    private void Init(PipeDirection direction, SafePipeHandle safePipeHandle)
    {
      this.ValidateHandleIsPipe(safePipeHandle);
      this.InitializeHandle(safePipeHandle, true, false);
      this.State = PipeState.Connected;
    }

    /// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> instance is reclaimed by garbage collection.</summary>
    ~AnonymousPipeClientStream() => this.Dispose(false);

    /// <summary>Gets the pipe transmission mode supported by the current pipe.</summary>
    /// <returns>The <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> supported by the current pipe.</returns>
    public override PipeTransmissionMode TransmissionMode => PipeTransmissionMode.Byte;

    /// <summary>Sets the reading mode for the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The transmission mode is not valid. For anonymous pipes, only <see cref="F:System.IO.Pipes.PipeTransmissionMode.Byte" /> is supported.</exception>
    /// <exception cref="T:System.NotSupportedException">The transmission mode is <see cref="F:System.IO.Pipes.PipeTransmissionMode.Message" />.</exception>
    /// <exception cref="T:System.IO.IOException">The connection is broken or another I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <returns>The <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> for the <see cref="T:System.IO.Pipes.AnonymousPipeClientStream" /> object.</returns>
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
  }
}
