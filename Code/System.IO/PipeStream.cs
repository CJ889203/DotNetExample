// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeStream
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.xml

using Microsoft.Win32.SafeHandles;
using System.Buffers;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;


#nullable enable
namespace System.IO.Pipes
{
  /// <summary>Exposes a <see cref="T:System.IO.Stream" /> object around a pipe, which supports both anonymous and named pipes.</summary>
  public abstract class PipeStream : Stream
  {

    #nullable disable
    private SafePipeHandle _handle;
    private bool _canRead;
    private bool _canWrite;
    private bool _isAsync;
    private bool _isCurrentUserOnly;
    private bool _isMessageComplete;
    private bool _isFromExistingHandle;
    private bool _isHandleExposed;
    private PipeTransmissionMode _readMode;
    private PipeTransmissionMode _transmissionMode;
    private PipeDirection _pipeDirection;
    private uint _outBufferSize;
    private PipeState _state;
    internal ThreadPoolBoundHandle _threadPoolBinding;
    private PipeStream.ReadWriteValueTaskSource _reusableReadValueTaskSource;
    private PipeStream.ReadWriteValueTaskSource _reusableWriteValueTaskSource;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.PipeStream" /> class using the specified <see cref="T:System.IO.Pipes.PipeDirection" /> value and buffer size.</summary>
    /// <param name="direction">One of the <see cref="T:System.IO.Pipes.PipeDirection" /> values that indicates the direction of the pipe object.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than or equal to 0 that indicates the buffer size.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="bufferSize" /> is less than 0.</exception>
    protected PipeStream(PipeDirection direction, int bufferSize)
    {
      if (direction < PipeDirection.In || direction > PipeDirection.InOut)
        throw new ArgumentOutOfRangeException(nameof (direction), SR.ArgumentOutOfRange_DirectionModeInOutOrInOut);
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), SR.ArgumentOutOfRange_NeedNonNegNum);
      this.Init(direction, PipeTransmissionMode.Byte, (uint) bufferSize);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Pipes.PipeStream" /> class using the specified <see cref="T:System.IO.Pipes.PipeDirection" />, <see cref="T:System.IO.Pipes.PipeTransmissionMode" />, and buffer size.</summary>
    /// <param name="direction">One of the <see cref="T:System.IO.Pipes.PipeDirection" /> values that indicates the direction of the pipe object.</param>
    /// <param name="transmissionMode">One of the <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> values that indicates the transmission mode of the pipe object.</param>
    /// <param name="outBufferSize">A positive <see cref="T:System.Int32" /> value greater than or equal to 0 that indicates the buffer size.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="direction" /> is not a valid <see cref="T:System.IO.Pipes.PipeDirection" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="transmissionMode" /> is not a valid <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="bufferSize" /> is less than 0.</exception>
    protected PipeStream(
      PipeDirection direction,
      PipeTransmissionMode transmissionMode,
      int outBufferSize)
    {
      if (direction < PipeDirection.In || direction > PipeDirection.InOut)
        throw new ArgumentOutOfRangeException(nameof (direction), SR.ArgumentOutOfRange_DirectionModeInOutOrInOut);
      if (transmissionMode < PipeTransmissionMode.Byte || transmissionMode > PipeTransmissionMode.Message)
        throw new ArgumentOutOfRangeException(nameof (transmissionMode), SR.ArgumentOutOfRange_TransmissionModeByteOrMsg);
      if (outBufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (outBufferSize), SR.ArgumentOutOfRange_NeedNonNegNum);
      this.Init(direction, transmissionMode, (uint) outBufferSize);
    }

    private void Init(
      PipeDirection direction,
      PipeTransmissionMode transmissionMode,
      uint outBufferSize)
    {
      this._readMode = transmissionMode;
      this._transmissionMode = transmissionMode;
      this._pipeDirection = direction;
      if ((this._pipeDirection & PipeDirection.In) != (PipeDirection) 0)
        this._canRead = true;
      if ((this._pipeDirection & PipeDirection.Out) != (PipeDirection) 0)
        this._canWrite = true;
      this._outBufferSize = outBufferSize;
      this._isMessageComplete = true;
      this._state = PipeState.WaitingToConnect;
    }


    #nullable enable
    /// <summary>Initializes a <see cref="T:System.IO.Pipes.PipeStream" /> object from the specified <see cref="T:Microsoft.Win32.SafeHandles.SafePipeHandle" /> object.</summary>
    /// <param name="handle">The <see cref="T:Microsoft.Win32.SafeHandles.SafePipeHandle" /> object of the pipe to initialize.</param>
    /// <param name="isExposed">
    /// <see langword="true" /> to expose the handle; otherwise, <see langword="false" />.</param>
    /// <param name="isAsync">
    /// <see langword="true" /> to indicate that the handle was opened asynchronously; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.IO.IOException">A handle cannot be bound to the pipe.</exception>
    protected void InitializeHandle(SafePipeHandle? handle, bool isExposed, bool isAsync)
    {
      if (isAsync && handle != null)
        this.InitializeAsyncHandle(handle);
      this._handle = handle;
      this._isAsync = isAsync;
      this._isHandleExposed = isExposed;
      this._isFromExistingHandle = isExposed;
    }

    /// <summary>Reads a byte from a pipe.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support read operations.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe is disconnected, waiting to connect, or the handle has not been set.</exception>
    /// <exception cref="T:System.IO.IOException">Any I/O error occurred.</exception>
    /// <returns>The byte, cast to <see cref="T:System.Int32" />, or -1 indicates the end of the stream (the pipe has been closed).</returns>
    public override unsafe int ReadByte()
    {
      byte num;
      return this.Read(new Span<byte>((void*) &num, 1)) <= 0 ? -1 : (int) num;
    }

    /// <summary>Writes a byte to the current stream.</summary>
    /// <param name="value">The byte to write to the stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support write operations.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe is disconnected, waiting to connect, or the handle has not been set.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    public override unsafe void WriteByte(byte value) => this.Write(new ReadOnlySpan<byte>((void*) &value, 1));

    /// <summary>Clears the buffer for the current stream and causes any buffered data to be written to the underlying device.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support write operations.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    public override void Flush() => this.CheckWriteOperations();

    /// <summary>Asynchronously clears the buffer for the current stream and causes any buffered data to be written to the underlying device.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represent the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      try
      {
        this.Flush();
        return Task.CompletedTask;
      }
      catch (Exception ex)
      {
        return Task.FromException(ex);
      }
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Pipes.PipeStream" /> class and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this._handle != null && !this._handle.IsClosed)
          this._handle.Dispose();
        this.DisposeCore(disposing);
      }
      finally
      {
        base.Dispose(disposing);
      }
      this._state = PipeState.Closed;
    }

    /// <summary>Gets or sets a value indicating whether a <see cref="T:System.IO.Pipes.PipeStream" /> object is connected.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Pipes.PipeStream" /> object is connected; otherwise, <see langword="false" />.</returns>
    public bool IsConnected
    {
      get => this.State == PipeState.Connected;
      protected set => this._state = value ? PipeState.Connected : PipeState.Disconnected;
    }

    /// <summary>Gets a value indicating whether a <see cref="T:System.IO.Pipes.PipeStream" /> object was opened asynchronously or synchronously.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Pipes.PipeStream" /> object was opened asynchronously; otherwise, <see langword="false" />.</returns>
    public bool IsAsync => this._isAsync;

    /// <summary>Gets a value indicating whether there is more data in the message returned from the most recent read operation.</summary>
    /// <exception cref="T:System.InvalidOperationException">The pipe is not connected.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set.
    /// 
    /// -or-
    /// 
    /// The pipe's <see cref="P:System.IO.Pipes.PipeStream.ReadMode" /> property value is not <see cref="F:System.IO.Pipes.PipeTransmissionMode.Message" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <returns>
    /// <see langword="true" /> if there are no more characters to read in the message; otherwise, <see langword="false" />.</returns>
    public bool IsMessageComplete
    {
      get
      {
        if (this._state == PipeState.WaitingToConnect)
          throw new InvalidOperationException(SR.InvalidOperation_PipeNotYetConnected);
        if (this._state == PipeState.Disconnected)
          throw new InvalidOperationException(SR.InvalidOperation_PipeDisconnected);
        if (this._handle == null)
          throw new InvalidOperationException(SR.InvalidOperation_PipeHandleNotSet);
        if (this._state == PipeState.Closed || this._handle != null && this._handle.IsClosed)
          throw Error.GetPipeNotOpen();
        if (this._readMode != PipeTransmissionMode.Message)
          throw new InvalidOperationException(SR.InvalidOperation_PipeReadModeNotMessage);
        return this._isMessageComplete;
      }
    }

    internal void UpdateMessageCompletion(bool completion) => this._isMessageComplete = completion || this._state == PipeState.Broken;

    /// <summary>Gets the safe handle for the local end of the pipe that the current <see cref="T:System.IO.Pipes.PipeStream" /> object encapsulates.</summary>
    /// <exception cref="T:System.InvalidOperationException">The pipe handle has not been set.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <returns>A <see cref="T:Microsoft.Win32.SafeHandles.SafePipeHandle" /> object for the pipe that is encapsulated by the current <see cref="T:System.IO.Pipes.PipeStream" /> object.</returns>
    public SafePipeHandle SafePipeHandle
    {
      get
      {
        if (this._handle == null)
          throw new InvalidOperationException(SR.InvalidOperation_PipeHandleNotSet);
        if (this._handle.IsClosed)
          throw Error.GetPipeNotOpen();
        this._isHandleExposed = true;
        return this._handle;
      }
    }

    internal SafePipeHandle? InternalHandle => this._handle;

    /// <summary>Gets a value indicating whether a handle to a <see cref="T:System.IO.Pipes.PipeStream" /> object is exposed.</summary>
    /// <returns>
    /// <see langword="true" /> if a handle to the <see cref="T:System.IO.Pipes.PipeStream" /> object is exposed; otherwise, <see langword="false" />.</returns>
    protected bool IsHandleExposed => this._isHandleExposed;

    /// <summary>Gets a value indicating whether the current stream supports read operations.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports read operations; otherwise, <see langword="false" />.</returns>
    public override bool CanRead => this._canRead;

    /// <summary>Gets a value indicating whether the current stream supports write operations.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports write operations; otherwise, <see langword="false" />.</returns>
    public override bool CanWrite => this._canWrite;

    /// <summary>Gets a value indicating whether the current stream supports seek operations.</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public override bool CanSeek => false;

    /// <summary>Gets the length of a stream, in bytes.</summary>
    /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
    /// <returns>0 in all cases.</returns>
    public override long Length => throw Error.GetSeekNotSupported();

    /// <summary>Gets or sets the current position of the current stream.</summary>
    /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
    /// <returns>0 in all cases.</returns>
    public override long Position
    {
      get => throw Error.GetSeekNotSupported();
      set => throw Error.GetSeekNotSupported();
    }

    /// <summary>Sets the length of the current stream to the specified value.</summary>
    /// <param name="value">The new length of the stream.</param>
    public override void SetLength(long value) => throw Error.GetSeekNotSupported();

    /// <summary>Sets the current position of the current stream to the specified value.</summary>
    /// <param name="offset">The point, relative to <paramref name="origin" />, to begin seeking from.</param>
    /// <param name="origin">Specifies the beginning, the end, or the current position as a reference point for <paramref name="offset" />, using a value of type <see cref="T:System.IO.SeekOrigin" />.</param>
    /// <returns>The new position in the stream.</returns>
    public override long Seek(long offset, SeekOrigin origin) => throw Error.GetSeekNotSupported();

    /// <summary>Verifies that the pipe is in a proper state for getting or setting properties.</summary>
    protected internal virtual void CheckPipePropertyOperations()
    {
      if (this._handle == null)
        throw new InvalidOperationException(SR.InvalidOperation_PipeHandleNotSet);
      if (this._state == PipeState.Closed || this._handle != null && this._handle.IsClosed)
        throw Error.GetPipeNotOpen();
    }

    /// <summary>Verifies that the pipe is in a connected state for read operations.</summary>
    protected internal void CheckReadOperations()
    {
      if (this._state == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.InvalidOperation_PipeNotYetConnected);
      if (this._state == PipeState.Disconnected)
        throw new InvalidOperationException(SR.InvalidOperation_PipeDisconnected);
      if (this._handle == null)
        throw new InvalidOperationException(SR.InvalidOperation_PipeHandleNotSet);
      if (this._state == PipeState.Closed || this._handle != null && this._handle.IsClosed)
        throw Error.GetPipeNotOpen();
    }

    /// <summary>Verifies that the pipe is in a connected state for write operations.</summary>
    protected internal void CheckWriteOperations()
    {
      if (this._state == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.InvalidOperation_PipeNotYetConnected);
      if (this._state == PipeState.Disconnected)
        throw new InvalidOperationException(SR.InvalidOperation_PipeDisconnected);
      if (this._handle == null)
        throw new InvalidOperationException(SR.InvalidOperation_PipeHandleNotSet);
      if (this._state == PipeState.Broken)
        throw new IOException(SR.IO_PipeBroken);
      if (this._state == PipeState.Closed || this._handle != null && this._handle.IsClosed)
        throw Error.GetPipeNotOpen();
    }

    internal PipeState State
    {
      get => this._state;
      set => this._state = value;
    }

    internal bool IsCurrentUserOnly
    {
      get => this._isCurrentUserOnly;
      set => this._isCurrentUserOnly = value;
    }

    /// <summary>Reads a block of bytes from a stream and writes the data to a specified buffer starting at a specified position for a specified length.</summary>
    /// <param name="buffer">When this method returns, contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
    /// <param name="offset">The byte offset in the <paramref name="buffer" /> array at which the bytes that are read will be placed.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="count" /> is greater than the number of bytes available in <paramref name="buffer" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support read operations.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe is disconnected, waiting to connect, or the handle has not been set.</exception>
    /// <exception cref="T:System.IO.IOException">Any I/O error occurred.</exception>
    /// <returns>The total number of bytes that are read into <paramref name="buffer" />. This might be less than the number of bytes requested if that number of bytes is not currently available, or 0 if the end of the stream is reached.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this._isAsync)
        return this.ReadAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (!this.CanRead)
        throw Error.GetReadNotSupported();
      this.CheckReadOperations();
      return this.ReadCore(new Span<byte>(buffer, offset, count));
    }

    /// <summary>Reads a sequence of bytes from the current stream, writes them to a byte array, and advances the position within the stream by the number of bytes read.</summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source.</param>
    /// <exception cref="T:System.ArgumentNullException">The number of bytes read was longer than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Cannot access a closed pipe.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe hasn't been connected yet.
    /// 
    /// -or-
    /// 
    /// The pipe is in a disconnected state.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set. (Did your <see cref="T:System.IO.Pipes.PipeStream" /> implementation call <see cref="M:System.IO.Pipes.PipeStream.InitializeHandle(Microsoft.Win32.SafeHandles.SafePipeHandle,System.Boolean,System.Boolean)" />?</exception>
    /// <returns>The total number of bytes read into the <paramref name="buffer" />. This can be less than the number of bytes allocated in <paramref name="buffer" /> if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public override int Read(Span<byte> buffer)
    {
      if (this._isAsync)
        return base.Read(buffer);
      if (!this.CanRead)
        throw Error.GetReadNotSupported();
      this.CheckReadOperations();
      return this.ReadCore(buffer);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream to a byte array starting at a specified position for a specified number of bytes, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Cannot access a closed pipe.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe hasn't been connected yet.
    /// 
    /// -or-
    /// 
    /// The pipe is in a disconnected state.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set. (Did your <see cref="T:System.IO.Pipes.PipeStream" /> implementation call <see cref="M:System.IO.Pipes.PipeStream.InitializeHandle(Microsoft.Win32.SafeHandles.SafePipeHandle,System.Boolean,System.Boolean)" />?</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of its <see cref="P:System.Threading.Tasks.Task`1.Result" /> property contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (!this.CanRead)
        throw Error.GetReadNotSupported();
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled<int>(cancellationToken);
      this.CheckReadOperations();
      if (!this._isAsync)
        return base.ReadAsync(buffer, offset, count, cancellationToken);
      if (count != 0)
        return this.ReadAsyncCore(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
      this.UpdateMessageCompletion(false);
      return Task.FromResult<int>(0);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream, writes them to a byte memory range, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Cannot access a closed pipe.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe hasn't been connected yet.
    /// 
    /// -or-
    /// 
    /// The pipe is in a disconnected state.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set. (Did your <see cref="T:System.IO.Pipes.PipeStream" /> implementation call <see cref="M:System.IO.Pipes.PipeStream.InitializeHandle(Microsoft.Win32.SafeHandles.SafePipeHandle,System.Boolean,System.Boolean)" />?</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of its <see cref="P:System.Threading.Tasks.ValueTask`1.Result" /> property contains the total number of bytes read into the buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (!this._isAsync)
        return base.ReadAsync(buffer, cancellationToken);
      if (!this.CanRead)
        throw Error.GetReadNotSupported();
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled<int>(cancellationToken);
      this.CheckReadOperations();
      if (buffer.Length != 0)
        return this.ReadAsyncCore(buffer, cancellationToken);
      this.UpdateMessageCompletion(false);
      return new ValueTask<int>(0);
    }

    /// <summary>Begins an asynchronous read operation.</summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin reading.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="callback">The method to call when the asynchronous read operation is completed.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="count" /> is greater than the number of bytes available in <paramref name="buffer" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support read operations.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe is disconnected, waiting to connect, or the handle has not been set.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    /// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous read.</returns>
    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      return this._isAsync ? TaskToApm.Begin((Task) this.ReadAsync(buffer, offset, count, CancellationToken.None), callback, state) : base.BeginRead(buffer, offset, count, callback, state);
    }

    /// <summary>Ends a pending asynchronous read request.</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Pipes.PipeStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
    /// <returns>The number of bytes that were read. A return value of 0 indicates the end of the stream (the pipe has been closed).</returns>
    public override int EndRead(IAsyncResult asyncResult) => this._isAsync ? TaskToApm.End<int>(asyncResult) : base.EndRead(asyncResult);

    /// <summary>Writes a block of bytes to the current stream using data from a buffer.</summary>
    /// <param name="buffer">The buffer that contains data to write to the pipe.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The maximum number of bytes to write to the current stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="count" /> is greater than the number of bytes available in <paramref name="buffer" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support write operations.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      if (this._isAsync)
      {
        this.WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
      }
      else
      {
        Stream.ValidateBufferArguments(buffer, offset, count);
        if (!this.CanWrite)
          throw Error.GetWriteNotSupported();
        this.CheckWriteOperations();
        this.WriteCore(new ReadOnlySpan<byte>(buffer, offset, count));
      }
    }

    /// <summary>Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the current stream.</param>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Cannot access a closed pipe.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe hasn't been connected yet.
    /// 
    /// -or-
    /// 
    /// The pipe is in a disconnected state.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set. (Did your <see cref="T:System.IO.Pipes.PipeStream" /> implementation call <see cref="M:System.IO.Pipes.PipeStream.InitializeHandle(Microsoft.Win32.SafeHandles.SafePipeHandle,System.Boolean,System.Boolean)" />?</exception>
    public override void Write(ReadOnlySpan<byte> buffer)
    {
      if (this._isAsync)
      {
        base.Write(buffer);
      }
      else
      {
        if (!this.CanWrite)
          throw Error.GetWriteNotSupported();
        this.CheckWriteOperations();
        this.WriteCore(buffer);
      }
    }

    /// <summary>Asynchronously writes a specified number of bytes from a byte array starting at a specified position, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> is negative.
    /// 
    /// -or-
    /// 
    /// The <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="buffer" />.Length - <paramref name="offset" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.NotSupportedException">Stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Cannot access a closed pipe.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe hasn't been connected yet.
    /// 
    /// -or-
    /// 
    /// The pipe is in a disconnected state.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set. (Did your <see cref="T:System.IO.Pipes.PipeStream" /> implementation call <see cref="M:System.IO.Pipes.PipeStream.InitializeHandle(Microsoft.Win32.SafeHandles.SafePipeHandle,System.Boolean,System.Boolean)" />?</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (!this.CanWrite)
        throw Error.GetWriteNotSupported();
      if (cancellationToken.IsCancellationRequested)
        return (Task) Task.FromCanceled<int>(cancellationToken);
      this.CheckWriteOperations();
      if (!this._isAsync)
        return base.WriteAsync(buffer, offset, count, cancellationToken);
      return count == 0 ? Task.CompletedTask : this.WriteAsyncCore(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.NotSupportedException">Stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Cannot access a closed pipe.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe hasn't been connected yet.
    /// 
    /// -or-
    /// 
    /// The pipe is in a disconnected state.
    /// 
    /// -or-
    /// 
    /// The pipe handle has not been set. (Did your <see cref="T:System.IO.Pipes.PipeStream" /> implementation call <see cref="M:System.IO.Pipes.PipeStream.InitializeHandle(Microsoft.Win32.SafeHandles.SafePipeHandle,System.Boolean,System.Boolean)" />?</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (!this._isAsync)
        return base.WriteAsync(buffer, cancellationToken);
      if (!this.CanWrite)
        throw Error.GetWriteNotSupported();
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled(cancellationToken);
      this.CheckWriteOperations();
      return buffer.Length == 0 ? new ValueTask() : this.WriteAsyncCore(buffer, cancellationToken);
    }

    /// <summary>Begins an asynchronous write operation.</summary>
    /// <param name="buffer">The buffer that contains the data to write to the current stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="callback">The method to call when the asynchronous write operation is completed.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="count" /> is greater than the number of bytes available in <paramref name="buffer" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support write operations.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe is disconnected, waiting to connect, or the handle has not been set.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    /// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous write operation.</returns>
    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      return this._isAsync ? TaskToApm.Begin(this.WriteAsync(buffer, offset, count, CancellationToken.None), callback, state) : base.BeginWrite(buffer, offset, count, callback, state);
    }

    /// <summary>Ends a pending asynchronous write request.</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Pipes.PipeStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
    public override void EndWrite(IAsyncResult asyncResult)
    {
      if (this._isAsync)
        TaskToApm.End(asyncResult);
      else
        base.EndWrite(asyncResult);
    }


    #nullable disable
    internal static string GetPipePath(string serverName, string pipeName)
    {
      string fullPath = Path.GetFullPath("\\\\" + serverName + "\\pipe\\" + pipeName);
      return !string.Equals(fullPath, "\\\\.\\pipe\\anonymous", StringComparison.OrdinalIgnoreCase) ? fullPath : throw new ArgumentOutOfRangeException(nameof (pipeName), SR.ArgumentOutOfRange_AnonymousReserved);
    }

    internal void ValidateHandleIsPipe(SafePipeHandle safePipeHandle)
    {
      if (Interop.Kernel32.GetFileType((SafeHandle) safePipeHandle) != 3)
        throw new IOException(SR.IO_InvalidPipeHandle);
    }

    private void InitializeAsyncHandle(SafePipeHandle handle) => this._threadPoolBinding = ThreadPoolBoundHandle.BindHandle((SafeHandle) handle);

    internal virtual void TryToReuse(PipeStream.PipeValueTaskSource source)
    {
      source._source.Reset();
      if (!(source is PipeStream.ReadWriteValueTaskSource writeValueTaskSource) || Interlocked.CompareExchange<PipeStream.ReadWriteValueTaskSource>(ref writeValueTaskSource._isWrite ? ref this._reusableWriteValueTaskSource : ref this._reusableReadValueTaskSource, writeValueTaskSource, (PipeStream.ReadWriteValueTaskSource) null) == null)
        return;
      source._preallocatedOverlapped.Dispose();
    }

    private void DisposeCore(bool disposing)
    {
      if (!disposing)
        return;
      this._threadPoolBinding?.Dispose();
      Interlocked.Exchange<PipeStream.ReadWriteValueTaskSource>(ref this._reusableReadValueTaskSource, (PipeStream.ReadWriteValueTaskSource) null)?.Dispose();
      Interlocked.Exchange<PipeStream.ReadWriteValueTaskSource>(ref this._reusableWriteValueTaskSource, (PipeStream.ReadWriteValueTaskSource) null)?.Dispose();
    }

    private unsafe int ReadCore(Span<byte> buffer)
    {
      if (buffer.Length == 0)
        return 0;
      fixed (byte* bytes = &MemoryMarshal.GetReference<byte>(buffer))
      {
        int numBytesRead = 0;
        if (Interop.Kernel32.ReadFile((SafeHandle) this._handle, bytes, buffer.Length, out numBytesRead, IntPtr.Zero) != 0)
        {
          this._isMessageComplete = true;
          return numBytesRead;
        }
        int lastPinvokeError = Marshal.GetLastPInvokeError();
        this._isMessageComplete = lastPinvokeError != 234;
        if (lastPinvokeError != 109 && lastPinvokeError != 233)
        {
          if (lastPinvokeError == 234)
            return numBytesRead;
          throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError, string.Empty);
        }
        this.State = PipeState.Broken;
        return 0;
      }
    }

    private unsafe ValueTask<int> ReadAsyncCore(
      Memory<byte> buffer,
      CancellationToken cancellationToken)
    {
      PipeStream.ReadWriteValueTaskSource source = Interlocked.Exchange<PipeStream.ReadWriteValueTaskSource>(ref this._reusableReadValueTaskSource, (PipeStream.ReadWriteValueTaskSource) null) ?? new PipeStream.ReadWriteValueTaskSource(this, false);
      try
      {
        source.PrepareForOperation((ReadOnlyMemory<byte>) buffer);
        if (Interop.Kernel32.ReadFile((SafeHandle) this._handle, (byte*) source._memoryHandle.Pointer, buffer.Length, IntPtr.Zero, source._overlapped) == 0)
        {
          int lastPinvokeError = Marshal.GetLastPInvokeError();
          switch (lastPinvokeError)
          {
            case 109:
            case 233:
              this.State = PipeState.Broken;
              source._overlapped->InternalLow = IntPtr.Zero;
              source.Dispose();
              this.UpdateMessageCompletion(true);
              return new ValueTask<int>(0);
            case 234:
              break;
            case 997:
              source.RegisterForCancellation(cancellationToken);
              break;
            default:
              source.Dispose();
              return ValueTask.FromException<int>(Win32Marshal.GetExceptionForWin32Error(lastPinvokeError));
          }
        }
      }
      catch
      {
        source.Dispose();
        throw;
      }
      source.FinishedScheduling();
      return new ValueTask<int>((IValueTaskSource<int>) source, source.Version);
    }

    private unsafe void WriteCore(ReadOnlySpan<byte> buffer)
    {
      if (buffer.Length == 0)
        return;
      fixed (byte* bytes = &MemoryMarshal.GetReference<byte>(buffer))
      {
        int numBytesWritten = 0;
        if (Interop.Kernel32.WriteFile((SafeHandle) this._handle, bytes, buffer.Length, out numBytesWritten, IntPtr.Zero) == 0)
          throw this.WinIOError(Marshal.GetLastPInvokeError());
      }
    }

    private unsafe ValueTask WriteAsyncCore(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken)
    {
      PipeStream.ReadWriteValueTaskSource source = Interlocked.Exchange<PipeStream.ReadWriteValueTaskSource>(ref this._reusableWriteValueTaskSource, (PipeStream.ReadWriteValueTaskSource) null) ?? new PipeStream.ReadWriteValueTaskSource(this, true);
      try
      {
        source.PrepareForOperation(buffer);
        if (Interop.Kernel32.WriteFile((SafeHandle) this._handle, (byte*) source._memoryHandle.Pointer, buffer.Length, IntPtr.Zero, source._overlapped) == 0)
        {
          int lastPinvokeError = Marshal.GetLastPInvokeError();
          if (lastPinvokeError == 997)
          {
            source.RegisterForCancellation(cancellationToken);
          }
          else
          {
            source.Dispose();
            return ValueTask.FromException(ExceptionDispatchInfo.SetCurrentStackTrace(this.WinIOError(lastPinvokeError)));
          }
        }
      }
      catch
      {
        source.Dispose();
        throw;
      }
      source.FinishedScheduling();
      return new ValueTask((IValueTaskSource) source, source.Version);
    }

    /// <summary>Waits for the other end of the pipe to read all sent bytes.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The pipe does not support write operations.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    [SupportedOSPlatform("windows")]
    public void WaitForPipeDrain()
    {
      this.CheckWriteOperations();
      if (!this.CanWrite)
        throw Error.GetWriteNotSupported();
      if (!Interop.Kernel32.FlushFileBuffers((SafeHandle) this._handle))
        throw this.WinIOError(Marshal.GetLastPInvokeError());
    }

    /// <summary>Gets the pipe transmission mode supported by the current pipe.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The pipe is closed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The handle has not been set.
    /// 
    /// -or-
    /// 
    /// The pipe is waiting to connect in an anonymous client/server operation or with a named client.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    /// <returns>One of the <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> values that indicates the transmission mode supported by the current pipe.</returns>
    public virtual unsafe PipeTransmissionMode TransmissionMode
    {
      get
      {
        this.CheckPipePropertyOperations();
        if (!this._isFromExistingHandle)
          return this._transmissionMode;
        uint num;
        if (!Interop.Kernel32.GetNamedPipeInfo(this._handle, &num, (uint*) null, (uint*) null, (uint*) null))
          throw this.WinIOError(Marshal.GetLastPInvokeError());
        return ((int) num & 4) != 0 ? PipeTransmissionMode.Message : PipeTransmissionMode.Byte;
      }
    }

    /// <summary>Gets the size, in bytes, of the inbound buffer for a pipe.</summary>
    /// <exception cref="T:System.NotSupportedException">The stream is unreadable.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe is waiting to connect.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    /// <returns>An integer value that represents the inbound buffer size, in bytes.</returns>
    public virtual unsafe int InBufferSize
    {
      get
      {
        this.CheckPipePropertyOperations();
        if (!this.CanRead)
          throw new NotSupportedException(SR.NotSupported_UnreadableStream);
        uint num;
        return Interop.Kernel32.GetNamedPipeInfo(this._handle, (uint*) null, (uint*) null, &num, (uint*) null) ? (int) num : throw this.WinIOError(Marshal.GetLastPInvokeError());
      }
    }

    /// <summary>Gets the size, in bytes, of the outbound buffer for a pipe.</summary>
    /// <exception cref="T:System.NotSupportedException">The stream is unwriteable.</exception>
    /// <exception cref="T:System.InvalidOperationException">The pipe is waiting to connect.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or another I/O error occurred.</exception>
    /// <returns>The outbound buffer size, in bytes.</returns>
    public virtual unsafe int OutBufferSize
    {
      get
      {
        this.CheckPipePropertyOperations();
        if (!this.CanWrite)
          throw new NotSupportedException(SR.NotSupported_UnwritableStream);
        uint outBufferSize;
        if (this._pipeDirection == PipeDirection.Out)
          outBufferSize = this._outBufferSize;
        else if (!Interop.Kernel32.GetNamedPipeInfo(this._handle, (uint*) null, &outBufferSize, (uint*) null, (uint*) null))
          throw this.WinIOError(Marshal.GetLastPInvokeError());
        return (int) outBufferSize;
      }
    }

    /// <summary>Gets or sets the reading mode for a <see cref="T:System.IO.Pipes.PipeStream" /> object.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The supplied value is not a valid <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> value.</exception>
    /// <exception cref="T:System.NotSupportedException">The supplied value is not a supported <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> value for this pipe stream.</exception>
    /// <exception cref="T:System.InvalidOperationException">The handle has not been set.
    /// 
    /// -or-
    /// 
    /// The pipe is waiting to connect with a named client.</exception>
    /// <exception cref="T:System.IO.IOException">The pipe is broken or an I/O error occurred with a named client.</exception>
    /// <returns>One of the <see cref="T:System.IO.Pipes.PipeTransmissionMode" /> values that indicates how the <see cref="T:System.IO.Pipes.PipeStream" /> object reads from the pipe.</returns>
    public virtual unsafe PipeTransmissionMode ReadMode
    {
      get
      {
        this.CheckPipePropertyOperations();
        if (this._isFromExistingHandle || this.IsHandleExposed)
          this.UpdateReadMode();
        return this._readMode;
      }
      set
      {
        this.CheckPipePropertyOperations();
        if (value < PipeTransmissionMode.Byte || value > PipeTransmissionMode.Message)
          throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_TransmissionModeByteOrMsg);
        if (!Interop.Kernel32.SetNamedPipeHandleState(this._handle, &((int) value << 1), IntPtr.Zero, IntPtr.Zero))
          throw this.WinIOError(Marshal.GetLastPInvokeError());
        this._readMode = value;
      }
    }

    internal static Interop.Kernel32.SECURITY_ATTRIBUTES GetSecAttrs(
      HandleInheritability inheritability)
    {
      return new Interop.Kernel32.SECURITY_ATTRIBUTES()
      {
        nLength = (uint) sizeof (Interop.Kernel32.SECURITY_ATTRIBUTES),
        bInheritHandle = (inheritability & HandleInheritability.Inheritable) != HandleInheritability.None ? Interop.BOOL.TRUE : Interop.BOOL.FALSE
      };
    }

    internal static unsafe Interop.Kernel32.SECURITY_ATTRIBUTES GetSecAttrs(
      HandleInheritability inheritability,
      PipeSecurity pipeSecurity,
      ref GCHandle pinningHandle)
    {
      Interop.Kernel32.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(inheritability);
      if (pipeSecurity != null)
      {
        byte[] descriptorBinaryForm = pipeSecurity.GetSecurityDescriptorBinaryForm();
        pinningHandle = GCHandle.Alloc((object) descriptorBinaryForm, GCHandleType.Pinned);
        fixed (byte* numPtr = descriptorBinaryForm)
          secAttrs.lpSecurityDescriptor = (IntPtr) (void*) numPtr;
      }
      return secAttrs;
    }

    private unsafe void UpdateReadMode()
    {
      uint num;
      if (!Interop.Kernel32.GetNamedPipeHandleStateW(this.SafePipeHandle, &num, (uint*) null, (uint*) null, (uint*) null, (char*) null, 0U))
        throw this.WinIOError(Marshal.GetLastPInvokeError());
      if (((int) num & 2) != 0)
        this._readMode = PipeTransmissionMode.Message;
      else
        this._readMode = PipeTransmissionMode.Byte;
    }

    internal Exception WinIOError(int errorCode)
    {
      switch (errorCode)
      {
        case 6:
          this._handle.SetHandleAsInvalid();
          this._state = PipeState.Broken;
          break;
        case 38:
          return Error.GetEndOfFile();
        case 109:
        case 232:
        case 233:
          this._state = PipeState.Broken;
          return (Exception) new IOException(SR.IO_PipeBroken, Win32Marshal.MakeHRFromErrorCode(errorCode));
      }
      return Win32Marshal.GetExceptionForWin32Error(errorCode);
    }

    internal abstract class PipeValueTaskSource : IValueTaskSource<int>, IValueTaskSource
    {
      internal static readonly IOCompletionCallback s_ioCallback = new IOCompletionCallback(PipeStream.PipeValueTaskSource.IOCallback);
      internal readonly PreAllocatedOverlapped _preallocatedOverlapped;
      internal readonly PipeStream _pipeStream;
      internal MemoryHandle _memoryHandle;
      internal ManualResetValueTaskSourceCore<int> _source;
      internal unsafe NativeOverlapped* _overlapped;
      internal CancellationTokenRegistration _cancellationRegistration;
      internal ulong _result;

      protected PipeValueTaskSource(PipeStream pipeStream)
      {
        this._pipeStream = pipeStream;
        this._source.RunContinuationsAsynchronously = true;
        this._preallocatedOverlapped = new PreAllocatedOverlapped(PipeStream.PipeValueTaskSource.s_ioCallback, (object) this, (object) null);
      }

      internal void Dispose()
      {
        this.ReleaseResources();
        this._preallocatedOverlapped.Dispose();
      }

      internal unsafe void PrepareForOperation(ReadOnlyMemory<byte> memory = default (ReadOnlyMemory<byte>))
      {
        this._result = 0UL;
        this._memoryHandle = memory.Pin();
        this._overlapped = this._pipeStream._threadPoolBinding.AllocateNativeOverlapped(this._preallocatedOverlapped);
      }

      public ValueTaskSourceStatus GetStatus(short token) => this._source.GetStatus(token);

      public void OnCompleted(
        Action<object> continuation,
        object state,
        short token,
        ValueTaskSourceOnCompletedFlags flags)
      {
        this._source.OnCompleted(continuation, state, token, flags);
      }

      void IValueTaskSource.GetResult(short token) => this.GetResult(token);

      public int GetResult(short token)
      {
        try
        {
          return this._source.GetResult(token);
        }
        finally
        {
          this._pipeStream.TryToReuse(this);
        }
      }

      internal short Version => this._source.Version;

      internal unsafe void RegisterForCancellation(CancellationToken cancellationToken)
      {
        if (!cancellationToken.CanBeCanceled)
          return;
        try
        {
          this._cancellationRegistration = cancellationToken.UnsafeRegister((Action<object, CancellationToken>) ((s, token) =>
          {
            PipeStream.PipeValueTaskSource pipeValueTaskSource = (PipeStream.PipeValueTaskSource) s;
            if (pipeValueTaskSource._pipeStream.SafePipeHandle.IsInvalid)
              return;
            try
            {
              Interop.Kernel32.CancelIoEx((SafeHandle) pipeValueTaskSource._pipeStream.SafePipeHandle, pipeValueTaskSource._overlapped);
            }
            catch (ObjectDisposedException ex)
            {
            }
          }), (object) this);
        }
        catch (OutOfMemoryException ex)
        {
        }
      }

      internal unsafe void ReleaseResources()
      {
        this._cancellationRegistration.Dispose();
        this._memoryHandle.Dispose();
        if ((IntPtr) this._overlapped == IntPtr.Zero)
          return;
        this._pipeStream._threadPoolBinding.FreeNativeOverlapped(this._overlapped);
        this._overlapped = (NativeOverlapped*) null;
      }

      internal void FinishedScheduling()
      {
        ulong errorCode = Interlocked.Exchange(ref this._result, 1UL);
        if (errorCode == 0UL)
          return;
        this.Complete((uint) errorCode, (uint) (errorCode >> 32) & (uint) int.MaxValue);
      }

      private static unsafe void IOCallback(
        uint errorCode,
        uint numBytes,
        NativeOverlapped* pOverlapped)
      {
        PipeStream.PipeValueTaskSource nativeOverlappedState = (PipeStream.PipeValueTaskSource) ThreadPoolBoundHandle.GetNativeOverlappedState(pOverlapped);
        if (Interlocked.Exchange(ref nativeOverlappedState._result, (ulong) (long.MinValue | (long) numBytes << 32) | (ulong) errorCode) == 0UL)
          return;
        nativeOverlappedState.Complete(errorCode, numBytes);
      }

      private void Complete(uint errorCode, uint numBytes)
      {
        this.ReleaseResources();
        this.CompleteCore(errorCode, numBytes);
      }

      private protected abstract void CompleteCore(uint errorCode, uint numBytes);
    }

    internal sealed class ReadWriteValueTaskSource : PipeStream.PipeValueTaskSource
    {
      internal readonly bool _isWrite;

      internal ReadWriteValueTaskSource(PipeStream stream, bool isWrite)
        : base(stream)
      {
        this._isWrite = isWrite;
      }

      private protected override void CompleteCore(uint errorCode, uint numBytes)
      {
        if (!this._isWrite)
        {
          bool completion = true;
          switch (errorCode)
          {
            case 109:
            case 232:
            case 233:
              errorCode = 0U;
              break;
            case 234:
              errorCode = 0U;
              completion = false;
              break;
          }
          this._pipeStream.UpdateMessageCompletion(completion);
        }
        switch (errorCode)
        {
          case 0:
            this._source.SetResult((int) numBytes);
            break;
          case 995:
            CancellationToken token = this._cancellationRegistration.Token;
            this._source.SetException(token.IsCancellationRequested ? (Exception) new OperationCanceledException(token) : (Exception) new OperationCanceledException());
            break;
          default:
            this._source.SetException(this._pipeStream.WinIOError((int) errorCode));
            break;
        }
      }
    }

    internal sealed class ConnectionValueTaskSource : PipeStream.PipeValueTaskSource
    {
      internal ConnectionValueTaskSource(NamedPipeServerStream server)
        : base((PipeStream) server)
      {
      }

      private protected override void CompleteCore(uint errorCode, uint numBytes)
      {
        switch (errorCode)
        {
          case 0:
          case 535:
            this._pipeStream.State = PipeState.Connected;
            this._source.SetResult((int) numBytes);
            break;
          case 995:
            CancellationToken token = this._cancellationRegistration.Token;
            this._source.SetException(!token.CanBeCanceled || token.IsCancellationRequested ? (Exception) new OperationCanceledException(token) : Error.GetOperationAborted());
            break;
          default:
            this._source.SetException(Win32Marshal.GetExceptionForWin32Error((int) errorCode));
            break;
        }
      }
    }
  }
}
