// Decompiled with JetBrains decompiler
// Type: System.IO.MemoryStream
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Creates a stream whose backing store is memory.</summary>
  public class MemoryStream : Stream
  {

    #nullable disable
    private byte[] _buffer;
    private readonly int _origin;
    private int _position;
    private int _length;
    private int _capacity;
    private bool _expandable;
    private bool _writable;
    private readonly bool _exposable;
    private bool _isOpen;
    private Task<int> _lastReadTask;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.MemoryStream" /> class with an expandable capacity initialized to zero.</summary>
    public MemoryStream()
      : this(0)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.MemoryStream" /> class with an expandable capacity initialized as specified.</summary>
    /// <param name="capacity">The initial size of the internal array in bytes.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is negative.</exception>
    public MemoryStream(int capacity)
    {
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_NegativeCapacity);
      this._buffer = capacity != 0 ? new byte[capacity] : Array.Empty<byte>();
      this._capacity = capacity;
      this._expandable = true;
      this._writable = true;
      this._exposable = true;
      this._isOpen = true;
    }


    #nullable enable
    /// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified byte array.</summary>
    /// <param name="buffer">The array of unsigned bytes from which to create the current stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    public MemoryStream(byte[] buffer)
      : this(buffer, true)
    {
    }

    /// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified byte array with the <see cref="P:System.IO.MemoryStream.CanWrite" /> property set as specified.</summary>
    /// <param name="buffer">The array of unsigned bytes from which to create this stream.</param>
    /// <param name="writable">The setting of the <see cref="P:System.IO.MemoryStream.CanWrite" /> property, which determines whether the stream supports writing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    public MemoryStream(byte[] buffer, bool writable)
    {
      this._buffer = buffer != null ? buffer : throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      this._length = this._capacity = buffer.Length;
      this._writable = writable;
      this._isOpen = true;
    }

    /// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified region (index) of a byte array.</summary>
    /// <param name="buffer">The array of unsigned bytes from which to create this stream.</param>
    /// <param name="index">The index into <paramref name="buffer" /> at which the stream begins.</param>
    /// <param name="count">The length of the stream in bytes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    public MemoryStream(byte[] buffer, int index, int count)
      : this(buffer, index, count, true, false)
    {
    }

    /// <summary>Initializes a new non-resizable instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified region of a byte array, with the <see cref="P:System.IO.MemoryStream.CanWrite" /> property set as specified.</summary>
    /// <param name="buffer">The array of unsigned bytes from which to create this stream.</param>
    /// <param name="index">The index in <paramref name="buffer" /> at which the stream begins.</param>
    /// <param name="count">The length of the stream in bytes.</param>
    /// <param name="writable">The setting of the <see cref="P:System.IO.MemoryStream.CanWrite" /> property, which determines whether the stream supports writing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> are negative.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    public MemoryStream(byte[] buffer, int index, int count, bool writable)
      : this(buffer, index, count, writable, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.MemoryStream" /> class based on the specified region of a byte array, with the <see cref="P:System.IO.MemoryStream.CanWrite" /> property set as specified, and the ability to call <see cref="M:System.IO.MemoryStream.GetBuffer" /> set as specified.</summary>
    /// <param name="buffer">The array of unsigned bytes from which to create this stream.</param>
    /// <param name="index">The index into <paramref name="buffer" /> at which the stream begins.</param>
    /// <param name="count">The length of the stream in bytes.</param>
    /// <param name="writable">The setting of the <see cref="P:System.IO.MemoryStream.CanWrite" /> property, which determines whether the stream supports writing.</param>
    /// <param name="publiclyVisible">
    /// <see langword="true" /> to enable <see cref="M:System.IO.MemoryStream.GetBuffer" />, which returns the unsigned byte array from which the stream was created; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      this._buffer = buffer;
      this._origin = this._position = index;
      this._length = this._capacity = index + count;
      this._writable = writable;
      this._exposable = publiclyVisible;
      this._isOpen = true;
    }

    /// <summary>Gets a value indicating whether the current stream supports reading.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream is open.</returns>
    public override bool CanRead => this._isOpen;

    /// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream is open.</returns>
    public override bool CanSeek => this._isOpen;

    /// <summary>Gets a value indicating whether the current stream supports writing.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports writing; otherwise, <see langword="false" />.</returns>
    public override bool CanWrite => this._writable;

    private void EnsureNotClosed()
    {
      if (this._isOpen)
        return;
      ThrowHelper.ThrowObjectDisposedException_StreamClosed((string) null);
    }

    private void EnsureWriteable()
    {
      if (this.CanWrite)
        return;
      ThrowHelper.ThrowNotSupportedException_UnwritableStream();
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.MemoryStream" /> class and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        this._isOpen = false;
        this._writable = false;
        this._expandable = false;
        this._lastReadTask = (Task<int>) null;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    private bool EnsureCapacity(int value)
    {
      if (value < 0)
        throw new IOException(SR.IO_StreamTooLong);
      if (value <= this._capacity)
        return false;
      int num = Math.Max(value, 256);
      if (num < this._capacity * 2)
        num = this._capacity * 2;
      if ((long) (uint) (this._capacity * 2) > (long) Array.MaxLength)
        num = Math.Max(value, Array.MaxLength);
      this.Capacity = num;
      return true;
    }

    /// <summary>Overrides the <see cref="M:System.IO.Stream.Flush" /> method so that no action is performed.</summary>
    public override void Flush()
    {
    }

    /// <summary>Asynchronously clears all buffers for this stream, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
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

    /// <summary>Returns the array of unsigned bytes from which this stream was created.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">The <see langword="MemoryStream" /> instance was not created with a publicly visible buffer.</exception>
    /// <returns>The byte array from which this stream was created, or the underlying array if a byte array was not provided to the <see cref="T:System.IO.MemoryStream" /> constructor during construction of the current instance.</returns>
    public virtual byte[] GetBuffer()
    {
      if (!this._exposable)
        throw new UnauthorizedAccessException(SR.UnauthorizedAccess_MemStreamBuffer);
      return this._buffer;
    }

    /// <summary>Returns the array of unsigned bytes from which this stream was created. The return value indicates whether the conversion succeeded.</summary>
    /// <param name="buffer">When this method returns <see langword="true" />, the byte array segment from which this stream was created; when this method returns <see langword="false" />, this parameter is set to <see langword="default" />.</param>
    /// <returns>
    /// <see langword="true" /> if the buffer is exposable; otherwise, <see langword="false" />.</returns>
    public virtual bool TryGetBuffer(out ArraySegment<byte> buffer)
    {
      if (!this._exposable)
      {
        buffer = new ArraySegment<byte>();
        return false;
      }
      buffer = new ArraySegment<byte>(this._buffer, this._origin, this._length - this._origin);
      return true;
    }


    #nullable disable
    internal byte[] InternalGetBuffer() => this._buffer;

    internal int InternalGetPosition() => this._position;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ReadOnlySpan<byte> InternalReadSpan(int count)
    {
      this.EnsureNotClosed();
      int position = this._position;
      int num = position + count;
      if ((uint) num > (uint) this._length)
      {
        this._position = this._length;
        ThrowHelper.ThrowEndOfFileException();
      }
      ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(this._buffer, position, count);
      this._position = num;
      return readOnlySpan;
    }

    internal int InternalEmulateRead(int count)
    {
      this.EnsureNotClosed();
      int num = this._length - this._position;
      if (num > count)
        num = count;
      if (num < 0)
        num = 0;
      this._position += num;
      return num;
    }

    /// <summary>Gets or sets the number of bytes allocated for this stream.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">A capacity is set that is negative or less than the current length of the stream.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <see langword="set" /> is invoked on a stream whose capacity cannot be modified.</exception>
    /// <returns>The length of the usable portion of the buffer for the stream.</returns>
    public virtual int Capacity
    {
      get
      {
        this.EnsureNotClosed();
        return this._capacity - this._origin;
      }
      set
      {
        if ((long) value < this.Length)
          throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_SmallCapacity);
        this.EnsureNotClosed();
        if (!this._expandable && value != this.Capacity)
          throw new NotSupportedException(SR.NotSupported_MemStreamNotExpandable);
        if (!this._expandable || value == this._capacity)
          return;
        if (value > 0)
        {
          byte[] dst = new byte[value];
          if (this._length > 0)
            Buffer.BlockCopy((Array) this._buffer, 0, (Array) dst, 0, this._length);
          this._buffer = dst;
        }
        else
          this._buffer = Array.Empty<byte>();
        this._capacity = value;
      }
    }

    /// <summary>Gets the length of the stream in bytes.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>The length of the stream in bytes.</returns>
    public override long Length
    {
      get
      {
        this.EnsureNotClosed();
        return (long) (this._length - this._origin);
      }
    }

    /// <summary>Gets or sets the current position within the stream.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The position is set to a negative value or a value greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>The current position within the stream.</returns>
    public override long Position
    {
      get
      {
        this.EnsureNotClosed();
        return (long) (this._position - this._origin);
      }
      set
      {
        if (value < 0L)
          throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_NeedNonNegNum);
        this.EnsureNotClosed();
        if (value > (long) int.MaxValue)
          throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_StreamLength);
        this._position = this._origin + (int) value;
      }
    }


    #nullable enable
    /// <summary>Reads a block of bytes from the current stream and writes the data to a buffer.</summary>
    /// <param name="buffer">When this method returns, contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the characters read from the current stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing data from the current stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="offset" /> subtracted from the buffer length is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed.</exception>
    /// <returns>The total number of bytes written into the buffer. This can be less than the number of bytes requested if that number of bytes are not currently available, or zero if the end of the stream is reached before any bytes are read.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      this.EnsureNotClosed();
      int count1 = this._length - this._position;
      if (count1 > count)
        count1 = count;
      if (count1 <= 0)
        return 0;
      if (count1 <= 8)
      {
        int num = count1;
        while (--num >= 0)
          buffer[offset + num] = this._buffer[this._position + num];
      }
      else
        Buffer.BlockCopy((Array) this._buffer, this._position, (Array) buffer, offset, count1);
      this._position += count1;
      return count1;
    }

    /// <summary>Reads a sequence of bytes from the current memory stream and advances the position within the memory stream by the number of bytes read.</summary>
    /// <param name="buffer" />
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the memory stream has been reached.</returns>
    public override int Read(Span<byte> buffer)
    {
      if (this.GetType() != typeof (MemoryStream))
        return base.Read(buffer);
      this.EnsureNotClosed();
      int length = Math.Min(this._length - this._position, buffer.Length);
      if (length <= 0)
        return 0;
      new Span<byte>(this._buffer, this._position, length).CopyTo(buffer);
      this._position += length;
      return length;
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
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
      catch (OperationCanceledException ex)
      {
        return Task.FromCanceled<int>(ex);
      }
      catch (Exception ex)
      {
        return Task.FromException<int>(ex);
      }
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current memory stream, writes the sequence into <paramref name="destination" />, advances the position within the memory stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <param name="buffer" />
    /// <returns>A task that represents the asynchronous read operation. The value of its <see cref="P:System.Threading.Tasks.ValueTask`1.Result" /> property contains the total number of bytes read into the <paramref name="destination" />. The result value can be less than the number of bytes allocated in <paramref name="destination" /> if that many bytes are not currently available, or it can be 0 (zero) if the end of the memory stream has been reached.</returns>
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
      catch (OperationCanceledException ex)
      {
        return new ValueTask<int>(Task.FromCanceled<int>(ex));
      }
      catch (Exception ex)
      {
        return ValueTask.FromException<int>(ex);
      }
    }

    /// <summary>Reads a byte from the current stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed.</exception>
    /// <returns>The byte cast to a <see cref="T:System.Int32" />, or -1 if the end of the stream has been reached.</returns>
    public override int ReadByte()
    {
      this.EnsureNotClosed();
      return this._position >= this._length ? -1 : (int) this._buffer[this._position++];
    }

    /// <summary>Reads the bytes from the current memory stream and writes them to another stream, using a specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current memory stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is not a positive number.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the underlying memory stream or the <paramref name="destination" /> stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The underlying memory stream is unreadable.
    /// 
    /// -or-
    /// 
    /// The <paramref name="destination" /> stream is unwritable.</exception>
    public override void CopyTo(Stream destination, int bufferSize)
    {
      if (this.GetType() != typeof (MemoryStream))
      {
        base.CopyTo(destination, bufferSize);
      }
      else
      {
        Stream.ValidateCopyToArguments(destination, bufferSize);
        this.EnsureNotClosed();
        int position = this._position;
        int count = this.InternalEmulateRead(this._length - position);
        if (count <= 0)
          return;
        destination.Write(this._buffer, position, count);
      }
    }

    /// <summary>Asynchronously reads all the bytes from the current stream and writes them to another stream, using a specified buffer size and cancellation token.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="buffersize" /> is negative or zero.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public override Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      this.EnsureNotClosed();
      if (this.GetType() != typeof (MemoryStream))
        return base.CopyToAsync(destination, bufferSize, cancellationToken);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      int position = this._position;
      int count = this.InternalEmulateRead(this._length - this._position);
      if (count == 0)
        return Task.CompletedTask;
      if (!(destination is MemoryStream memoryStream))
        return destination.WriteAsync(this._buffer, position, count, cancellationToken);
      try
      {
        memoryStream.Write(this._buffer, position, count);
        return Task.CompletedTask;
      }
      catch (Exception ex)
      {
        return Task.FromException(ex);
      }
    }

    /// <summary>Sets the position within the current stream to the specified value.</summary>
    /// <param name="offset">The new position within the stream. This is relative to the <paramref name="loc" /> parameter, and can be positive or negative.</param>
    /// <param name="loc">A value of type <see cref="T:System.IO.SeekOrigin" />, which acts as the seek reference point.</param>
    /// <exception cref="T:System.IO.IOException">Seeking is attempted before the beginning of the stream.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">There is an invalid <see cref="T:System.IO.SeekOrigin" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="offset" /> caused an arithmetic overflow.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed.</exception>
    /// <returns>The new position within the stream, calculated by combining the initial reference point and the offset.</returns>
    public override long Seek(long offset, SeekOrigin loc)
    {
      this.EnsureNotClosed();
      if (offset > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_StreamLength);
      switch (loc)
      {
        case SeekOrigin.Begin:
          int num1 = this._origin + (int) offset;
          if (offset < 0L || num1 < this._origin)
            throw new IOException(SR.IO_SeekBeforeBegin);
          this._position = num1;
          break;
        case SeekOrigin.Current:
          int num2 = this._position + (int) offset;
          if ((long) this._position + offset < (long) this._origin || num2 < this._origin)
            throw new IOException(SR.IO_SeekBeforeBegin);
          this._position = num2;
          break;
        case SeekOrigin.End:
          int num3 = this._length + (int) offset;
          if ((long) this._length + offset < (long) this._origin || num3 < this._origin)
            throw new IOException(SR.IO_SeekBeforeBegin);
          this._position = num3;
          break;
        default:
          throw new ArgumentException(SR.Argument_InvalidSeekOrigin);
      }
      return (long) this._position;
    }

    /// <summary>Sets the length of the current stream to the specified value.</summary>
    /// <param name="value">The value at which to set the length.</param>
    /// <exception cref="T:System.NotSupportedException">The current stream is not resizable and <paramref name="value" /> is larger than the current capacity.
    /// 
    /// -or-
    /// 
    /// The current stream does not support writing.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is negative or is greater than the maximum length of the <see cref="T:System.IO.MemoryStream" />, where the maximum length is(<see cref="F:System.Int32.MaxValue" /> - origin), and origin is the index into the underlying buffer at which the stream starts.</exception>
    public override void SetLength(long value)
    {
      if (value < 0L || value > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_StreamLength);
      this.EnsureWriteable();
      if (value > (long) (int.MaxValue - this._origin))
        throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_StreamLength);
      int num = this._origin + (int) value;
      if (!this.EnsureCapacity(num) && num > this._length)
        Array.Clear((Array) this._buffer, this._length, num - this._length);
      this._length = num;
      if (this._position <= num)
        return;
      this._position = num;
    }

    /// <summary>Writes the stream contents to a byte array, regardless of the <see cref="P:System.IO.MemoryStream.Position" /> property.</summary>
    /// <returns>A new byte array.</returns>
    public virtual byte[] ToArray()
    {
      int length = this._length - this._origin;
      if (length == 0)
        return Array.Empty<byte>();
      byte[] destination = GC.AllocateUninitializedArray<byte>(length);
      this._buffer.AsSpan<byte>(this._origin, length).CopyTo((Span<byte>) destination);
      return destination;
    }

    /// <summary>Writes a block of bytes to the current stream using data read from a buffer.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing. For additional information see <see cref="P:System.IO.Stream.CanWrite" />.
    /// 
    /// -or-
    /// 
    /// The current position is closer than <paramref name="count" /> bytes to the end of the stream, and the capacity cannot be modified.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="offset" /> subtracted from the buffer length is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> are negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current stream instance is closed.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      this.EnsureNotClosed();
      this.EnsureWriteable();
      int num1 = this._position + count;
      if (num1 < 0)
        throw new IOException(SR.IO_StreamTooLong);
      if (num1 > this._length)
      {
        bool flag = this._position > this._length;
        if (num1 > this._capacity && this.EnsureCapacity(num1))
          flag = false;
        if (flag)
          Array.Clear((Array) this._buffer, this._length, num1 - this._length);
        this._length = num1;
      }
      if (count <= 8 && buffer != this._buffer)
      {
        int num2 = count;
        while (--num2 >= 0)
          this._buffer[this._position + num2] = buffer[offset + num2];
      }
      else
        Buffer.BlockCopy((Array) buffer, offset, (Array) this._buffer, this._position, count);
      this._position = num1;
    }

    /// <summary>Writes the sequence of bytes contained in <paramref name="source" /> into the current memory stream and advances the current position within this memory stream by the number of bytes written.</summary>
    /// <param name="buffer" />
    public override void Write(ReadOnlySpan<byte> buffer)
    {
      if (this.GetType() != typeof (MemoryStream))
      {
        base.Write(buffer);
      }
      else
      {
        this.EnsureNotClosed();
        this.EnsureWriteable();
        int num = this._position + buffer.Length;
        if (num < 0)
          throw new IOException(SR.IO_StreamTooLong);
        if (num > this._length)
        {
          bool flag = this._position > this._length;
          if (num > this._capacity && this.EnsureCapacity(num))
            flag = false;
          if (flag)
            Array.Clear((Array) this._buffer, this._length, num - this._length);
          this._length = num;
        }
        buffer.CopyTo(new Span<byte>(this._buffer, this._position, buffer.Length));
        this._position = num;
      }
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous write operation.</exception>
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
      catch (OperationCanceledException ex)
      {
        return Task.FromCanceled(ex);
      }
      catch (Exception ex)
      {
        return Task.FromException(ex);
      }
    }

    /// <summary>Asynchronously writes the sequence of bytes contained in <paramref name="source" /> into the current memory stream, advances the current position within this memory stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <param name="buffer" />
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
      catch (OperationCanceledException ex)
      {
        return new ValueTask(Task.FromCanceled(ex));
      }
      catch (Exception ex)
      {
        return ValueTask.FromException(ex);
      }
    }

    /// <summary>Writes a byte to the current stream at the current position.</summary>
    /// <param name="value">The byte to write.</param>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing. For additional information see <see cref="P:System.IO.Stream.CanWrite" />.
    /// 
    /// -or-
    /// 
    /// The current position is at the end of the stream, and the capacity cannot be modified.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current stream is closed.</exception>
    public override void WriteByte(byte value)
    {
      this.EnsureNotClosed();
      this.EnsureWriteable();
      if (this._position >= this._length)
      {
        int num = this._position + 1;
        bool flag = this._position > this._length;
        if (num >= this._capacity && this.EnsureCapacity(num))
          flag = false;
        if (flag)
          Array.Clear((Array) this._buffer, this._length, this._position - this._length);
        this._length = num;
      }
      this._buffer[this._position++] = value;
    }

    /// <summary>Writes the entire contents of this memory stream to another stream.</summary>
    /// <param name="stream">The stream to write this memory stream to.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current or target stream is closed.</exception>
    public virtual void WriteTo(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream), SR.ArgumentNull_Stream);
      this.EnsureNotClosed();
      stream.Write(this._buffer, this._origin, this._length - this._origin);
    }
  }
}
