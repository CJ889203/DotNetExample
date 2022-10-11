// Decompiled with JetBrains decompiler
// Type: System.IO.UnmanagedMemoryStream
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Provides access to unmanaged blocks of memory from managed code.</summary>
  public class UnmanagedMemoryStream : Stream
  {

    #nullable disable
    private SafeBuffer _buffer;
    private unsafe byte* _mem;
    private long _length;
    private long _capacity;
    private long _position;
    private long _offset;
    private FileAccess _access;
    private bool _isOpen;
    private Task<int> _lastReadTask;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class.</summary>
    /// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
    protected UnmanagedMemoryStream()
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class in a safe buffer with a specified offset and length.</summary>
    /// <param name="buffer">The buffer to contain the unmanaged memory stream.</param>
    /// <param name="offset">The byte position in the buffer at which to start the unmanaged memory stream.</param>
    /// <param name="length">The length of the unmanaged memory stream.</param>
    public UnmanagedMemoryStream(SafeBuffer buffer, long offset, long length) => this.Initialize(buffer, offset, length, FileAccess.Read);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class in a safe buffer with a specified offset, length, and file access.</summary>
    /// <param name="buffer">The buffer to contain the unmanaged memory stream.</param>
    /// <param name="offset">The byte position in the buffer at which to start the unmanaged memory stream.</param>
    /// <param name="length">The length of the unmanaged memory stream.</param>
    /// <param name="access">The mode of file access to the unmanaged memory stream.</param>
    public UnmanagedMemoryStream(SafeBuffer buffer, long offset, long length, FileAccess access) => this.Initialize(buffer, offset, length, access);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class in a safe buffer with a specified offset, length, and file access.</summary>
    /// <param name="buffer">The buffer to contain the unmanaged memory stream.</param>
    /// <param name="offset">The byte position in the buffer at which to start the unmanaged memory stream.</param>
    /// <param name="length">The length of the unmanaged memory stream.</param>
    /// <param name="access">The mode of file access to the unmanaged memory stream.</param>
    protected unsafe void Initialize(
      SafeBuffer buffer,
      long offset,
      long length,
      FileAccess access)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0L)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (length < 0L)
        throw new ArgumentOutOfRangeException(nameof (length), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.ByteLength < (ulong) (offset + length))
        throw new ArgumentException(SR.Argument_InvalidSafeBufferOffLen);
      if (access < FileAccess.Read || access > FileAccess.ReadWrite)
        throw new ArgumentOutOfRangeException(nameof (access));
      if (this._isOpen)
        throw new InvalidOperationException(SR.InvalidOperation_CalledTwice);
      byte* pointer = (byte*) null;
      try
      {
        buffer.AcquirePointer(ref pointer);
        if (pointer + offset + length < pointer)
          throw new ArgumentException(SR.ArgumentOutOfRange_UnmanagedMemStreamWrapAround);
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          buffer.ReleasePointer();
      }
      this._offset = offset;
      this._buffer = buffer;
      this._length = length;
      this._capacity = length;
      this._access = access;
      this._isOpen = true;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class using the specified location and memory length.</summary>
    /// <param name="pointer">A pointer to an unmanaged memory location.</param>
    /// <param name="length">The length of the memory to use.</param>
    /// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="pointer" /> value is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="length" /> value is less than zero.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="length" /> is large enough to cause an overflow.</exception>
    [CLSCompliant(false)]
    public unsafe UnmanagedMemoryStream(byte* pointer, long length) => this.Initialize(pointer, length, length, FileAccess.Read);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class using the specified location, memory length, total amount of memory, and file access values.</summary>
    /// <param name="pointer">A pointer to an unmanaged memory location.</param>
    /// <param name="length">The length of the memory to use.</param>
    /// <param name="capacity">The total amount of memory assigned to the stream.</param>
    /// <param name="access">One of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="pointer" /> value is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="length" /> value is less than zero.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="capacity" /> value is less than zero.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="length" /> value is greater than the <paramref name="capacity" /> value.</exception>
    [CLSCompliant(false)]
    public unsafe UnmanagedMemoryStream(
      byte* pointer,
      long length,
      long capacity,
      FileAccess access)
    {
      this.Initialize(pointer, length, capacity, access);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.UnmanagedMemoryStream" /> class by using a pointer to an unmanaged memory location.</summary>
    /// <param name="pointer">A pointer to an unmanaged memory location.</param>
    /// <param name="length">The length of the memory to use.</param>
    /// <param name="capacity">The total amount of memory assigned to the stream.</param>
    /// <param name="access">One of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <exception cref="T:System.Security.SecurityException">The user does not have the required permission.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="pointer" /> value is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="length" /> value is less than zero.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="capacity" /> value is less than zero.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="length" /> value is large enough to cause an overflow.</exception>
    [CLSCompliant(false)]
    protected unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access)
    {
      if ((IntPtr) pointer == IntPtr.Zero)
        throw new ArgumentNullException(nameof (pointer));
      if (length < 0L || capacity < 0L)
        throw new ArgumentOutOfRangeException(length < 0L ? nameof (length) : nameof (capacity), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (length > capacity)
        throw new ArgumentOutOfRangeException(nameof (length), SR.ArgumentOutOfRange_LengthGreaterThanCapacity);
      if ((UIntPtr) ((ulong) pointer + (ulong) capacity) < (UIntPtr) pointer)
        throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_UnmanagedMemStreamWrapAround);
      if (access < FileAccess.Read || access > FileAccess.ReadWrite)
        throw new ArgumentOutOfRangeException(nameof (access), SR.ArgumentOutOfRange_Enum);
      if (this._isOpen)
        throw new InvalidOperationException(SR.InvalidOperation_CalledTwice);
      this._mem = pointer;
      this._offset = 0L;
      this._length = length;
      this._capacity = capacity;
      this._access = access;
      this._isOpen = true;
    }

    /// <summary>Gets a value indicating whether a stream supports reading.</summary>
    /// <returns>
    /// <see langword="false" /> if the object was created by a constructor with an <paramref name="access" /> parameter that did not include reading the stream and if the stream is closed; otherwise, <see langword="true" />.</returns>
    public override bool CanRead => this._isOpen && (this._access & FileAccess.Read) != 0;

    /// <summary>Gets a value indicating whether a stream supports seeking.</summary>
    /// <returns>
    /// <see langword="false" /> if the stream is closed; otherwise, <see langword="true" />.</returns>
    public override bool CanSeek => this._isOpen;

    /// <summary>Gets a value indicating whether a stream supports writing.</summary>
    /// <returns>
    /// <see langword="false" /> if the object was created by a constructor with an <paramref name="access" /> parameter value that supports writing or was created by a constructor that had no parameters, or if the stream is closed; otherwise, <see langword="true" />.</returns>
    public override bool CanWrite => this._isOpen && (this._access & FileAccess.Write) != 0;

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.UnmanagedMemoryStream" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override unsafe void Dispose(bool disposing)
    {
      this._isOpen = false;
      this._mem = (byte*) null;
      base.Dispose(disposing);
    }

    private void EnsureNotClosed()
    {
      if (this._isOpen)
        return;
      ThrowHelper.ThrowObjectDisposedException_StreamClosed((string) null);
    }

    private void EnsureReadable()
    {
      if (this.CanRead)
        return;
      ThrowHelper.ThrowNotSupportedException_UnreadableStream();
    }

    private void EnsureWriteable()
    {
      if (this.CanWrite)
        return;
      ThrowHelper.ThrowNotSupportedException_UnwritableStream();
    }

    /// <summary>Overrides the <see cref="M:System.IO.Stream.Flush" /> method so that no action is performed.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public override void Flush() => this.EnsureNotClosed();

    /// <summary>Overrides the <see cref="M:System.IO.Stream.FlushAsync(System.Threading.CancellationToken)" /> method so that the operation is cancelled if specified, but no other action is performed.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
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

    /// <summary>Gets the length of the data in a stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>The length of the data in the stream.</returns>
    public override long Length
    {
      get
      {
        this.EnsureNotClosed();
        return Interlocked.Read(ref this._length);
      }
    }

    /// <summary>Gets the stream length (size) or the total amount of memory assigned to a stream (capacity).</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>The size or capacity of the stream.</returns>
    public long Capacity
    {
      get
      {
        this.EnsureNotClosed();
        return this._capacity;
      }
    }

    /// <summary>Gets or sets the current position in a stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The position is set to a value that is less than zero, or the position is larger than <see cref="F:System.Int32.MaxValue" /> or results in overflow when added to the current pointer.</exception>
    /// <returns>The current position in the stream.</returns>
    public override long Position
    {
      get
      {
        if (!this.CanSeek)
          ThrowHelper.ThrowObjectDisposedException_StreamClosed((string) null);
        return Interlocked.Read(ref this._position);
      }
      set
      {
        if (value < 0L)
          throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_NeedNonNegNum);
        if (!this.CanSeek)
          ThrowHelper.ThrowObjectDisposedException_StreamClosed((string) null);
        Interlocked.Exchange(ref this._position, value);
      }
    }

    /// <summary>Gets or sets a byte pointer to a stream based on the current position in the stream.</summary>
    /// <exception cref="T:System.IndexOutOfRangeException">The current position is larger than the capacity of the stream.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The position is being set is not a valid position in the current stream.</exception>
    /// <exception cref="T:System.IO.IOException">The pointer is being set to a lower value than the starting position of the stream.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream was initialized for use with a <see cref="T:System.Runtime.InteropServices.SafeBuffer" />. The <see cref="P:System.IO.UnmanagedMemoryStream.PositionPointer" /> property is valid only for streams that are initialized with a <see cref="T:System.Byte" /> pointer.</exception>
    /// <returns>A byte pointer.</returns>
    [CLSCompliant(false)]
    public unsafe byte* PositionPointer
    {
      get
      {
        if (this._buffer != null)
          throw new NotSupportedException(SR.NotSupported_UmsSafeBuffer);
        this.EnsureNotClosed();
        long num = Interlocked.Read(ref this._position);
        if (num > this._capacity)
          throw new IndexOutOfRangeException(SR.IndexOutOfRange_UMSPosition);
        return this._mem + num;
      }
      set
      {
        if (this._buffer != null)
          throw new NotSupportedException(SR.NotSupported_UmsSafeBuffer);
        this.EnsureNotClosed();
        if (value < this._mem)
          throw new IOException(SR.IO_SeekBeforeBegin);
        long num = (long) value - (long) this._mem;
        if (num < 0L)
          throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_UnmanagedMemStreamLength);
        Interlocked.Exchange(ref this._position, num);
      }
    }

    /// <summary>Reads the specified number of bytes into the specified array.</summary>
    /// <param name="buffer">When this method returns, contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source. This parameter is passed uninitialized.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
    /// <param name="count">The maximum number of bytes to read from the current stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying memory does not support reading.
    /// 
    /// -or-
    /// 
    ///  The <see cref="P:System.IO.UnmanagedMemoryStream.CanRead" /> property is set to <see langword="false" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is set to <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> parameter is less than zero.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="count" /> parameter is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">The length of the buffer array minus the <paramref name="offset" /> parameter is less than the <paramref name="count" /> parameter.</exception>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      return this.ReadCore(new Span<byte>(buffer, offset, count));
    }

    /// <summary>Reads all the bytes of this unmanaged memory stream into the specified span of bytes.</summary>
    /// <param name="buffer" />
    /// <returns>The total number of bytes read into the destination.</returns>
    public override int Read(Span<byte> buffer) => this.GetType() == typeof (UnmanagedMemoryStream) ? this.ReadCore(buffer) : base.Read(buffer);


    #nullable disable
    internal unsafe int ReadCore(Span<byte> buffer)
    {
      this.EnsureNotClosed();
      this.EnsureReadable();
      long index = Interlocked.Read(ref this._position);
      long num = Math.Min(Interlocked.Read(ref this._length) - index, (long) buffer.Length);
      if (num <= 0L)
        return 0;
      int len = (int) num;
      if (len < 0)
        return 0;
      if (this._buffer != null)
      {
        byte* pointer = (byte*) null;
        try
        {
          this._buffer.AcquirePointer(ref pointer);
          Buffer.Memmove(ref MemoryMarshal.GetReference<byte>(buffer), ref (pointer + index)[this._offset], (UIntPtr) len);
        }
        finally
        {
          if ((IntPtr) pointer != IntPtr.Zero)
            this._buffer.ReleasePointer();
        }
      }
      else
        Buffer.Memmove(ref MemoryMarshal.GetReference<byte>(buffer), ref this._mem[index], (UIntPtr) len);
      Interlocked.Exchange(ref this._position, index + num);
      return len;
    }


    #nullable enable
    /// <summary>Asynchronously reads the specified number of bytes into the specified array.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled<int>(cancellationToken);
      try
      {
        int result = this.Read(buffer, offset, count);
        Task<int> lastReadTask = this._lastReadTask;
        return lastReadTask == null || lastReadTask.Result != result ? (this._lastReadTask = Task.FromResult<int>(result)) : lastReadTask;
      }
      catch (Exception ex)
      {
        return Task.FromException<int>(ex);
      }
    }

    /// <summary>Asynchronously reads the unmanaged memory stream bytes into the memory region.</summary>
    /// <param name="buffer">When the asynchronous method finishes, this memory region contains all the bytes read from the unmanaged memory stream.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, and wraps the total number of bytes read into the buffer.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled<int>(cancellationToken);
      try
      {
        ArraySegment<byte> segment;
        return new ValueTask<int>(MemoryMarshal.TryGetArray<byte>((ReadOnlyMemory<byte>) buffer, out segment) ? this.Read(segment.Array, segment.Offset, segment.Count) : this.Read(buffer.Span));
      }
      catch (Exception ex)
      {
        return ValueTask.FromException<int>(ex);
      }
    }

    /// <summary>Reads a byte from a stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying memory does not support reading.
    /// 
    /// -or-
    /// 
    ///  The current position is at the end of the stream.</exception>
    /// <returns>The unsigned byte cast to an <see cref="T:System.Int32" /> object, or -1 if at the end of the stream.</returns>
    public override unsafe int ReadByte()
    {
      this.EnsureNotClosed();
      this.EnsureReadable();
      long index = Interlocked.Read(ref this._position);
      long num1 = Interlocked.Read(ref this._length);
      if (index >= num1)
        return -1;
      Interlocked.Exchange(ref this._position, index + 1L);
      int num2;
      if (this._buffer != null)
      {
        byte* pointer = (byte*) null;
        try
        {
          this._buffer.AcquirePointer(ref pointer);
          num2 = (int) (pointer + index)[this._offset];
        }
        finally
        {
          if ((IntPtr) pointer != IntPtr.Zero)
            this._buffer.ReleasePointer();
        }
      }
      else
        num2 = (int) this._mem[index];
      return num2;
    }

    /// <summary>Sets the current position of the current stream to the given value.</summary>
    /// <param name="offset">The point relative to <c>origin</c> to begin seeking from.</param>
    /// <param name="loc">Specifies the beginning, the end, or the current position as a reference point for <c>origin</c>, using a value of type <see cref="T:System.IO.SeekOrigin" />.</param>
    /// <exception cref="T:System.IO.IOException">An attempt was made to seek before the beginning of the stream.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> value is larger than the maximum size of the stream.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="loc" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>The new position in the stream.</returns>
    public override long Seek(long offset, SeekOrigin loc)
    {
      this.EnsureNotClosed();
      switch (loc)
      {
        case SeekOrigin.Begin:
          if (offset < 0L)
            throw new IOException(SR.IO_SeekBeforeBegin);
          Interlocked.Exchange(ref this._position, offset);
          break;
        case SeekOrigin.Current:
          long num1 = Interlocked.Read(ref this._position);
          if (offset + num1 < 0L)
            throw new IOException(SR.IO_SeekBeforeBegin);
          Interlocked.Exchange(ref this._position, offset + num1);
          break;
        case SeekOrigin.End:
          long num2 = Interlocked.Read(ref this._length);
          if (num2 + offset < 0L)
            throw new IOException(SR.IO_SeekBeforeBegin);
          Interlocked.Exchange(ref this._position, num2 + offset);
          break;
        default:
          throw new ArgumentException(SR.Argument_InvalidSeekOrigin);
      }
      return Interlocked.Read(ref this._position);
    }

    /// <summary>Sets the length of a stream to a specified value.</summary>
    /// <param name="value">The length of the stream.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error has occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying memory does not support writing.
    /// 
    /// -or-
    /// 
    ///  An attempt is made to write to the stream and the <see cref="P:System.IO.UnmanagedMemoryStream.CanWrite" /> property is <see langword="false" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The specified <paramref name="value" /> exceeds the capacity of the stream.
    /// 
    /// -or-
    /// 
    ///  The specified <paramref name="value" /> is negative.</exception>
    public override unsafe void SetLength(long value)
    {
      if (value < 0L)
        throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (this._buffer != null)
        throw new NotSupportedException(SR.NotSupported_UmsSafeBuffer);
      this.EnsureNotClosed();
      this.EnsureWriteable();
      if (value > this._capacity)
        throw new IOException(SR.IO_FixedCapacity);
      long num1 = Interlocked.Read(ref this._position);
      long num2 = Interlocked.Read(ref this._length);
      if (value > num2)
        Buffer.ZeroMemory(this._mem + num2, (UIntPtr) (ulong) (value - num2));
      Interlocked.Exchange(ref this._length, value);
      if (num1 <= value)
        return;
      Interlocked.Exchange(ref this._position, value);
    }

    /// <summary>Writes a block of bytes to the current stream using data from a buffer.</summary>
    /// <param name="buffer">The byte array from which to copy bytes to the current stream.</param>
    /// <param name="offset">The offset in the buffer at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The number of bytes to write to the current stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying memory does not support writing.
    /// 
    /// -or-
    /// 
    ///  An attempt is made to write to the stream and the <see cref="P:System.IO.UnmanagedMemoryStream.CanWrite" /> property is <see langword="false" />.
    /// 
    /// -or-
    /// 
    ///  The <paramref name="count" /> value is greater than the capacity of the stream.
    /// 
    /// -or-
    /// 
    ///  The position is at the end of the stream capacity.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">One of the specified parameters is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="offset" /> parameter minus the length of the <paramref name="buffer" /> parameter is less than the <paramref name="count" /> parameter.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is <see langword="null" />.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      this.WriteCore(new ReadOnlySpan<byte>(buffer, offset, count));
    }

    /// <summary>Writes a block of bytes to the current unmanaged memory stream using data from the provided span of bytes.</summary>
    /// <param name="buffer" />
    public override void Write(ReadOnlySpan<byte> buffer)
    {
      if (this.GetType() == typeof (UnmanagedMemoryStream))
        this.WriteCore(buffer);
      else
        base.Write(buffer);
    }


    #nullable disable
    internal unsafe void WriteCore(ReadOnlySpan<byte> buffer)
    {
      this.EnsureNotClosed();
      this.EnsureWriteable();
      long index = Interlocked.Read(ref this._position);
      long num1 = Interlocked.Read(ref this._length);
      long num2 = index + (long) buffer.Length;
      if (num2 < 0L)
        throw new IOException(SR.IO_StreamTooLong);
      if (num2 > this._capacity)
        throw new NotSupportedException(SR.IO_FixedCapacity);
      if (this._buffer == null)
      {
        if (index > num1)
          Buffer.ZeroMemory(this._mem + num1, (UIntPtr) (ulong) (index - num1));
        if (num2 > num1)
          Interlocked.Exchange(ref this._length, num2);
      }
      if (this._buffer != null)
      {
        if (this._capacity - index < (long) buffer.Length)
          throw new ArgumentException(SR.Arg_BufferTooSmall);
        byte* pointer = (byte*) null;
        try
        {
          this._buffer.AcquirePointer(ref pointer);
          Buffer.Memmove(ref (pointer + index)[this._offset], ref MemoryMarshal.GetReference<byte>(buffer), (UIntPtr) buffer.Length);
        }
        finally
        {
          if ((IntPtr) pointer != IntPtr.Zero)
            this._buffer.ReleasePointer();
        }
      }
      else
        Buffer.Memmove(ref this._mem[index], ref MemoryMarshal.GetReference<byte>(buffer), (UIntPtr) buffer.Length);
      Interlocked.Exchange(ref this._position, num2);
    }


    #nullable enable
    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      try
      {
        this.Write(buffer, offset, count);
        return Task.CompletedTask;
      }
      catch (Exception ex)
      {
        return Task.FromException(ex);
      }
    }

    /// <summary>Asynchronously writes a span of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled(cancellationToken);
      try
      {
        ArraySegment<byte> segment;
        if (MemoryMarshal.TryGetArray<byte>(buffer, out segment))
          this.Write(segment.Array, segment.Offset, segment.Count);
        else
          this.Write(buffer.Span);
        return new ValueTask();
      }
      catch (Exception ex)
      {
        return ValueTask.FromException(ex);
      }
    }

    /// <summary>Writes a byte to the current position in the file stream.</summary>
    /// <param name="value">A byte value written to the stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying memory does not support writing.
    /// 
    /// -or-
    /// 
    ///  An attempt is made to write to the stream and the <see cref="P:System.IO.UnmanagedMemoryStream.CanWrite" /> property is <see langword="false" />.
    /// 
    /// -or-
    /// 
    ///  The current position is at the end of the capacity of the stream.</exception>
    /// <exception cref="T:System.IO.IOException">The supplied <paramref name="value" /> causes the stream exceed its maximum capacity.</exception>
    public override unsafe void WriteByte(byte value)
    {
      this.EnsureNotClosed();
      this.EnsureWriteable();
      long index = Interlocked.Read(ref this._position);
      long num1 = Interlocked.Read(ref this._length);
      long num2 = index + 1L;
      if (index >= num1)
      {
        if (num2 < 0L)
          throw new IOException(SR.IO_StreamTooLong);
        if (num2 > this._capacity)
          throw new NotSupportedException(SR.IO_FixedCapacity);
        if (this._buffer == null)
        {
          if (index > num1)
            Buffer.ZeroMemory(this._mem + num1, (UIntPtr) (ulong) (index - num1));
          Interlocked.Exchange(ref this._length, num2);
        }
      }
      if (this._buffer != null)
      {
        byte* pointer = (byte*) null;
        try
        {
          this._buffer.AcquirePointer(ref pointer);
          (pointer + index)[this._offset] = value;
        }
        finally
        {
          if ((IntPtr) pointer != IntPtr.Zero)
            this._buffer.ReleasePointer();
        }
      }
      else
        this._mem[index] = value;
      Interlocked.Exchange(ref this._position, num2);
    }
  }
}
