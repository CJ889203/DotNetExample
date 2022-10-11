// Decompiled with JetBrains decompiler
// Type: System.IO.BufferedStream
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Adds a buffering layer to read and write operations on another stream. This class cannot be inherited.</summary>
  public sealed class BufferedStream : Stream
  {

    #nullable disable
    private Stream _stream;
    private byte[] _buffer;
    private readonly int _bufferSize;
    private int _readPos;
    private int _readLen;
    private int _writePos;
    private Task<int> _lastSyncCompletedReadTask;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BufferedStream" /> class with a default buffer size of 4096 bytes.</summary>
    /// <param name="stream">The current stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    public BufferedStream(Stream stream)
      : this(stream, 4096)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BufferedStream" /> class with the specified buffer size.</summary>
    /// <param name="stream">The current stream.</param>
    /// <param name="bufferSize">The buffer size in bytes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    public BufferedStream(Stream stream, int bufferSize)
    {
      if (stream == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.stream);
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), SR.Format(SR.ArgumentOutOfRange_MustBePositive, (object) nameof (bufferSize)));
      this._stream = stream;
      this._bufferSize = bufferSize;
      if (this._stream.CanRead || this._stream.CanWrite)
        return;
      ThrowHelper.ThrowObjectDisposedException_StreamClosed((string) null);
    }

    private void EnsureNotClosed()
    {
      if (this._stream != null)
        return;
      ThrowHelper.ThrowObjectDisposedException_StreamClosed((string) null);
    }

    private void EnsureCanSeek()
    {
      if (this._stream.CanSeek)
        return;
      ThrowHelper.ThrowNotSupportedException_UnseekableStream();
    }

    private void EnsureCanRead()
    {
      if (this._stream.CanRead)
        return;
      ThrowHelper.ThrowNotSupportedException_UnreadableStream();
    }

    private void EnsureCanWrite()
    {
      if (this._stream.CanWrite)
        return;
      ThrowHelper.ThrowNotSupportedException_UnwritableStream();
    }

    private void EnsureShadowBufferAllocated()
    {
      if (this._buffer.Length != this._bufferSize || this._bufferSize >= 81920)
        return;
      byte[] dst = new byte[Math.Min(this._bufferSize + this._bufferSize, 81920)];
      Buffer.BlockCopy((Array) this._buffer, 0, (Array) dst, 0, this._writePos);
      this._buffer = dst;
    }

    private void EnsureBufferAllocated()
    {
      if (this._buffer != null)
        return;
      this._buffer = new byte[this._bufferSize];
    }

    /// <summary>Gets the underlying <see cref="T:System.IO.Stream" /> instance for this buffered stream.</summary>
    /// <returns>The underlying stream instance.</returns>
    public Stream UnderlyingStream => this._stream;

    /// <summary>Gets the buffer size in bytes for this buffered stream.</summary>
    /// <returns>An integer representing the buffer size in bytes.</returns>
    public int BufferSize => this._bufferSize;

    /// <summary>Gets a value indicating whether the current stream supports reading.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports reading; <see langword="false" /> if the stream is closed or was opened with write-only access.</returns>
    public override bool CanRead => this._stream != null && this._stream.CanRead;

    /// <summary>Gets a value indicating whether the current stream supports writing.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports writing; <see langword="false" /> if the stream is closed or was opened with read-only access.</returns>
    public override bool CanWrite => this._stream != null && this._stream.CanWrite;

    /// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports seeking; <see langword="false" /> if the stream is closed or if the stream was constructed from an operating system handle such as a pipe or output to the console.</returns>
    public override bool CanSeek => this._stream != null && this._stream.CanSeek;

    /// <summary>Gets the stream length in bytes.</summary>
    /// <exception cref="T:System.IO.IOException">The underlying stream is <see langword="null" /> or closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The stream length in bytes.</returns>
    public override long Length
    {
      get
      {
        this.EnsureNotClosed();
        if (this._writePos > 0)
          this.FlushWrite();
        return this._stream.Length;
      }
    }

    /// <summary>Gets the position within the current stream.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value passed to <see cref="M:System.IO.BufferedStream.Seek(System.Int64,System.IO.SeekOrigin)" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream being closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The position within the current stream.</returns>
    public override long Position
    {
      get
      {
        this.EnsureNotClosed();
        this.EnsureCanSeek();
        return this._stream.Position + (long) (this._readPos - this._readLen + this._writePos);
      }
      set
      {
        if (value < 0L)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        this.Seek(value, SeekOrigin.Begin);
      }
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing)
          return;
        if (this._stream == null)
          return;
        try
        {
          this.Flush();
        }
        finally
        {
          this._stream.Dispose();
        }
      }
      finally
      {
        this._stream = (Stream) null;
        this._buffer = (byte[]) null;
        this._writePos = 0;
        base.Dispose(disposing);
      }
    }

    /// <summary>Asynchronously releases the unmanaged resources used by the buffered stream.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override async ValueTask DisposeAsync()
    {
      BufferedStream bufferedStream = this;
      try
      {
        if (bufferedStream._stream == null)
          return;
        try
        {
          await bufferedStream.FlushAsync().ConfigureAwait(false);
        }
        finally
        {
          await bufferedStream._stream.DisposeAsync().ConfigureAwait(false);
        }
      }
      finally
      {
        bufferedStream._stream = (Stream) null;
        bufferedStream._buffer = (byte[]) null;
        bufferedStream._writePos = 0;
      }
    }

    /// <summary>Clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.IO.IOException">The data source or repository is not open.</exception>
    public override void Flush()
    {
      this.EnsureNotClosed();
      if (this._writePos > 0)
        this.FlushWrite();
      else if (this._readPos < this._readLen)
      {
        if (this._stream.CanSeek)
          this.FlushRead();
        if (!this._stream.CanWrite)
          return;
        this._stream.Flush();
      }
      else
      {
        if (this._stream.CanWrite)
          this._stream.Flush();
        this._writePos = this._readPos = this._readLen = 0;
      }
    }

    /// <summary>Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      if (cancellationToken.IsCancellationRequested)
        return (Task) Task.FromCanceled<int>(cancellationToken);
      this.EnsureNotClosed();
      return this.FlushAsyncInternal(cancellationToken);
    }


    #nullable disable
    private async Task FlushAsyncInternal(CancellationToken cancellationToken)
    {
      BufferedStream bufferedStream = this;
      await bufferedStream.EnsureAsyncActiveSemaphoreInitialized().WaitAsync(cancellationToken).ConfigureAwait(false);
      try
      {
        if (bufferedStream._writePos > 0)
          await bufferedStream.FlushWriteAsync(cancellationToken).ConfigureAwait(false);
        else if (bufferedStream._readPos < bufferedStream._readLen)
        {
          if (bufferedStream._stream.CanSeek)
            bufferedStream.FlushRead();
          if (!bufferedStream._stream.CanWrite)
            return;
          await bufferedStream._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
          if (!bufferedStream._stream.CanWrite)
            return;
          await bufferedStream._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
      }
      finally
      {
        bufferedStream._asyncActiveSemaphore.Release();
      }
    }

    private void FlushRead()
    {
      if (this._readPos - this._readLen != 0)
        this._stream.Seek((long) (this._readPos - this._readLen), SeekOrigin.Current);
      this._readPos = 0;
      this._readLen = 0;
    }

    private void ClearReadBufferBeforeWrite()
    {
      if (this._readPos == this._readLen)
      {
        this._readPos = this._readLen = 0;
      }
      else
      {
        if (!this._stream.CanSeek)
          throw new NotSupportedException(SR.NotSupported_CannotWriteToBufferedStreamIfReadBufferCannotBeFlushed);
        this.FlushRead();
      }
    }

    private void FlushWrite()
    {
      this._stream.Write(this._buffer, 0, this._writePos);
      this._writePos = 0;
      this._stream.Flush();
    }

    private async ValueTask FlushWriteAsync(CancellationToken cancellationToken)
    {
      await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, this._writePos), cancellationToken).ConfigureAwait(false);
      this._writePos = 0;
      await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    private int ReadFromBuffer(byte[] buffer, int offset, int count)
    {
      int count1 = this._readLen - this._readPos;
      if (count1 == 0)
        return 0;
      if (count1 > count)
        count1 = count;
      Buffer.BlockCopy((Array) this._buffer, this._readPos, (Array) buffer, offset, count1);
      this._readPos += count1;
      return count1;
    }

    private int ReadFromBuffer(Span<byte> destination)
    {
      int length = Math.Min(this._readLen - this._readPos, destination.Length);
      if (length > 0)
      {
        new ReadOnlySpan<byte>(this._buffer, this._readPos, length).CopyTo(destination);
        this._readPos += length;
      }
      return length;
    }

    private int ReadFromBuffer(byte[] buffer, int offset, int count, out Exception error)
    {
      try
      {
        error = (Exception) null;
        return this.ReadFromBuffer(buffer, offset, count);
      }
      catch (Exception ex)
      {
        error = ex;
        return 0;
      }
    }


    #nullable enable
    /// <summary>Copies bytes from the current buffered stream to an array.</summary>
    /// <param name="buffer" />
    /// <param name="offset">The byte offset in the buffer at which to begin reading bytes.</param>
    /// <param name="count">The number of bytes to be read.</param>
    /// <exception cref="T:System.ArgumentException">Length of <paramref name="array" /> minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is not open or is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The total number of bytes read into <paramref name="array" />. This can be less than the number of bytes requested if that many bytes are not currently available, or 0 if the end of the stream has been reached before any data can be read.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      this.EnsureNotClosed();
      this.EnsureCanRead();
      int num1 = this.ReadFromBuffer(buffer, offset, count);
      if (num1 == count)
        return num1;
      int num2 = num1;
      if (num1 > 0)
      {
        count -= num1;
        offset += num1;
      }
      this._readPos = this._readLen = 0;
      if (this._writePos > 0)
        this.FlushWrite();
      if (count >= this._bufferSize)
        return this._stream.Read(buffer, offset, count) + num2;
      this.EnsureBufferAllocated();
      this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
      return this.ReadFromBuffer(buffer, offset, count) + num2;
    }

    /// <summary>Copies bytes from the current buffered stream to a byte span and advances the position within the buffered stream by the number of bytes read.</summary>
    /// <param name="destination">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public override int Read(Span<byte> destination)
    {
      this.EnsureNotClosed();
      this.EnsureCanRead();
      int start = this.ReadFromBuffer(destination);
      if (start == destination.Length)
        return start;
      if (start > 0)
        destination = destination.Slice(start);
      this._readPos = this._readLen = 0;
      if (this._writePos > 0)
        this.FlushWrite();
      if (destination.Length >= this._bufferSize)
        return this._stream.Read(destination) + start;
      this.EnsureBufferAllocated();
      this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
      return this.ReadFromBuffer(destination) + start;
    }


    #nullable disable
    private Task<int> LastSyncCompletedReadTask(int val)
    {
      Task<int> completedReadTask = this._lastSyncCompletedReadTask;
      if (completedReadTask != null && completedReadTask.Result == val)
        return completedReadTask;
      Task<int> task = Task.FromResult<int>(val);
      this._lastSyncCompletedReadTask = task;
      return task;
    }


    #nullable enable
    /// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
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
      this.EnsureNotClosed();
      this.EnsureCanRead();
      int num = 0;
      SemaphoreSlim semaphoreSlim = this.EnsureAsyncActiveSemaphoreInitialized();
      Task semaphoreLockTask = semaphoreSlim.WaitAsync(cancellationToken);
      if (semaphoreLockTask.IsCompletedSuccessfully)
      {
        bool flag = true;
        try
        {
          Exception error;
          num = this.ReadFromBuffer(buffer, offset, count, out error);
          flag = num == count || error != null;
          if (flag)
            return error == null ? this.LastSyncCompletedReadTask(num) : Task.FromException<int>(error);
        }
        finally
        {
          if (flag)
            semaphoreSlim.Release();
        }
      }
      return this.ReadFromUnderlyingStreamAsync(new Memory<byte>(buffer, offset + num, count - num), cancellationToken, num, semaphoreLockTask).AsTask();
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current buffered stream and advances the position within the buffered stream by the number of bytes read.</summary>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of its <see cref="P:System.Threading.Tasks.ValueTask`1.Result" /> property contains the total number of bytes read into the buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled<int>(cancellationToken);
      this.EnsureNotClosed();
      this.EnsureCanRead();
      int num = 0;
      SemaphoreSlim semaphoreSlim = this.EnsureAsyncActiveSemaphoreInitialized();
      Task semaphoreLockTask = semaphoreSlim.WaitAsync(cancellationToken);
      if (semaphoreLockTask.IsCompletedSuccessfully)
      {
        bool flag = true;
        try
        {
          num = this.ReadFromBuffer(buffer.Span);
          flag = num == buffer.Length;
          if (flag)
            return new ValueTask<int>(num);
        }
        finally
        {
          if (flag)
            semaphoreSlim.Release();
        }
      }
      return this.ReadFromUnderlyingStreamAsync(buffer.Slice(num), cancellationToken, num, semaphoreLockTask);
    }


    #nullable disable
    private async ValueTask<int> ReadFromUnderlyingStreamAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken,
      int bytesAlreadySatisfied,
      Task semaphoreLockTask)
    {
      BufferedStream bufferedStream = this;
      await semaphoreLockTask.ConfigureAwait(false);
      try
      {
        int start = bufferedStream.ReadFromBuffer(buffer.Span);
        if (start == buffer.Length)
          return bytesAlreadySatisfied + start;
        if (start > 0)
        {
          buffer = buffer.Slice(start);
          bytesAlreadySatisfied += start;
        }
        bufferedStream._readPos = bufferedStream._readLen = 0;
        if (bufferedStream._writePos > 0)
          await bufferedStream.FlushWriteAsync(cancellationToken).ConfigureAwait(false);
        if (buffer.Length >= bufferedStream._bufferSize)
          return await bufferedStream._stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false) + bytesAlreadySatisfied;
        bufferedStream.EnsureBufferAllocated();
        int num = await bufferedStream._stream.ReadAsync(new Memory<byte>(bufferedStream._buffer, 0, bufferedStream._bufferSize), cancellationToken).ConfigureAwait(false);
        bufferedStream._readLen = num;
        return bytesAlreadySatisfied + bufferedStream.ReadFromBuffer(buffer.Span);
      }
      finally
      {
        bufferedStream._asyncActiveSemaphore.Release();
      }
    }


    #nullable enable
    /// <summary>Begins an asynchronous read operation. (Consider using <see cref="M:System.IO.BufferedStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
    /// <param name="buffer">The buffer to read the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data read from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">Attempted an asynchronous read past the end of the stream.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support the read operation.</exception>
    /// <returns>An object that represents the asynchronous read, which could still be pending.</returns>
    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      return TaskToApm.Begin((Task) this.ReadAsync(buffer, offset, count, CancellationToken.None), callback, state);
    }

    /// <summary>Waits for the pending asynchronous read operation to complete. (Consider using <see cref="M:System.IO.BufferedStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request to wait for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">This <see cref="T:System.IAsyncResult" /> object was not created by calling <see cref="M:System.IO.BufferedStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> on this class.</exception>
    /// <returns>The number of bytes read from the stream, between 0 (zero) and the number of bytes you requested. Streams only return 0 only at the end of the stream, otherwise, they should block until at least 1 byte is available.</returns>
    public override int EndRead(IAsyncResult asyncResult) => TaskToApm.End<int>(asyncResult);

    /// <summary>Reads a byte from the underlying stream and returns the byte cast to an <see langword="int" />, or returns -1 if reading from the end of the stream.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream being closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The byte cast to an <see langword="int" />, or -1 if reading from the end of the stream.</returns>
    public override int ReadByte() => this._readPos == this._readLen ? this.ReadByteSlow() : (int) this._buffer[this._readPos++];

    private int ReadByteSlow()
    {
      this.EnsureNotClosed();
      this.EnsureCanRead();
      if (this._writePos > 0)
        this.FlushWrite();
      this.EnsureBufferAllocated();
      this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
      this._readPos = 0;
      return this._readLen == 0 ? -1 : (int) this._buffer[this._readPos++];
    }


    #nullable disable
    private void WriteToBuffer(byte[] buffer, ref int offset, ref int count)
    {
      int count1 = Math.Min(this._bufferSize - this._writePos, count);
      if (count1 <= 0)
        return;
      this.EnsureBufferAllocated();
      Buffer.BlockCopy((Array) buffer, offset, (Array) this._buffer, this._writePos, count1);
      this._writePos += count1;
      count -= count1;
      offset += count1;
    }

    private int WriteToBuffer(ReadOnlySpan<byte> buffer)
    {
      int length = Math.Min(this._bufferSize - this._writePos, buffer.Length);
      if (length > 0)
      {
        this.EnsureBufferAllocated();
        buffer.Slice(0, length).CopyTo(new Span<byte>(this._buffer, this._writePos, length));
        this._writePos += length;
      }
      return length;
    }


    #nullable enable
    /// <summary>Copies bytes to the buffered stream and advances the current position within the buffered stream by the number of bytes written.</summary>
    /// <param name="buffer" />
    /// <param name="offset">The offset in the buffer at which to begin copying bytes to the current buffered stream.</param>
    /// <param name="count">The number of bytes to be written to the current buffered stream.</param>
    /// <exception cref="T:System.ArgumentException">Length of <paramref name="array" /> minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed or <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      this.EnsureNotClosed();
      this.EnsureCanWrite();
      if (this._writePos == 0)
        this.ClearReadBufferBeforeWrite();
      int count1 = checked (this._writePos + count);
      if (checked ((long) (uint) count1 + (long) count) < (long) checked (this._bufferSize + this._bufferSize))
      {
        this.WriteToBuffer(buffer, ref offset, ref count);
        if (this._writePos < this._bufferSize)
          return;
        this._stream.Write(this._buffer, 0, this._writePos);
        this._writePos = 0;
        this.WriteToBuffer(buffer, ref offset, ref count);
      }
      else
      {
        if (this._writePos > 0)
        {
          if (count1 <= this._bufferSize + this._bufferSize && count1 <= 81920)
          {
            this.EnsureShadowBufferAllocated();
            Buffer.BlockCopy((Array) buffer, offset, (Array) this._buffer, this._writePos, count);
            this._stream.Write(this._buffer, 0, count1);
            this._writePos = 0;
            return;
          }
          this._stream.Write(this._buffer, 0, this._writePos);
          this._writePos = 0;
        }
        this._stream.Write(buffer, offset, count);
      }
    }

    /// <summary>Writes a sequence of bytes to the current buffered stream and advances the current position within this buffered stream by the number of bytes written.</summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the current buffered stream.</param>
    public override void Write(ReadOnlySpan<byte> buffer)
    {
      this.EnsureNotClosed();
      this.EnsureCanWrite();
      if (this._writePos == 0)
        this.ClearReadBufferBeforeWrite();
      int count = checked (this._writePos + buffer.Length);
      if (checked ((long) (uint) count + (long) buffer.Length) < (long) checked (this._bufferSize + this._bufferSize))
      {
        int buffer1 = this.WriteToBuffer(buffer);
        if (this._writePos < this._bufferSize)
          return;
        buffer = buffer.Slice(buffer1);
        this._stream.Write(this._buffer, 0, this._writePos);
        this._writePos = 0;
        this.WriteToBuffer(buffer);
      }
      else
      {
        if (this._writePos > 0)
        {
          if (count <= this._bufferSize + this._bufferSize && count <= 81920)
          {
            this.EnsureShadowBufferAllocated();
            buffer.CopyTo(new Span<byte>(this._buffer, this._writePos, buffer.Length));
            this._stream.Write(this._buffer, 0, count);
            this._writePos = 0;
            return;
          }
          this._stream.Write(this._buffer, 0, this._writePos);
          this._writePos = 0;
        }
        this._stream.Write(buffer);
      }
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
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
      return this.WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current buffered stream, advances the current position within this buffered stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled(cancellationToken);
      this.EnsureNotClosed();
      this.EnsureCanWrite();
      SemaphoreSlim semaphoreSlim = this.EnsureAsyncActiveSemaphoreInitialized();
      Task semaphoreLockTask = semaphoreSlim.WaitAsync(cancellationToken);
      if (semaphoreLockTask.IsCompletedSuccessfully)
      {
        bool flag = true;
        try
        {
          if (this._writePos == 0)
            this.ClearReadBufferBeforeWrite();
          flag = buffer.Length < this._bufferSize - this._writePos;
          if (flag)
          {
            this.WriteToBuffer(buffer.Span);
            return new ValueTask();
          }
        }
        finally
        {
          if (flag)
            semaphoreSlim.Release();
        }
      }
      return this.WriteToUnderlyingStreamAsync(buffer, cancellationToken, semaphoreLockTask);
    }


    #nullable disable
    private async ValueTask WriteToUnderlyingStreamAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken,
      Task semaphoreLockTask)
    {
      BufferedStream bufferedStream = this;
      await semaphoreLockTask.ConfigureAwait(false);
      try
      {
        if (bufferedStream._writePos == 0)
          bufferedStream.ClearReadBufferBeforeWrite();
        int length = checked (bufferedStream._writePos + buffer.Length);
        if (checked (length + buffer.Length) < checked (bufferedStream._bufferSize + bufferedStream._bufferSize))
        {
          buffer = buffer.Slice(bufferedStream.WriteToBuffer(buffer.Span));
          if (bufferedStream._writePos < bufferedStream._bufferSize)
            return;
          await bufferedStream._stream.WriteAsync(new ReadOnlyMemory<byte>(bufferedStream._buffer, 0, bufferedStream._writePos), cancellationToken).ConfigureAwait(false);
          bufferedStream._writePos = 0;
          bufferedStream.WriteToBuffer(buffer.Span);
        }
        else
        {
          ValueTask valueTask;
          if (bufferedStream._writePos > 0)
          {
            if (length <= bufferedStream._bufferSize + bufferedStream._bufferSize && length <= 81920)
            {
              bufferedStream.EnsureShadowBufferAllocated();
              buffer.Span.CopyTo(new Span<byte>(bufferedStream._buffer, bufferedStream._writePos, buffer.Length));
              await bufferedStream._stream.WriteAsync(new ReadOnlyMemory<byte>(bufferedStream._buffer, 0, length), cancellationToken).ConfigureAwait(false);
              bufferedStream._writePos = 0;
              return;
            }
            valueTask = bufferedStream._stream.WriteAsync(new ReadOnlyMemory<byte>(bufferedStream._buffer, 0, bufferedStream._writePos), cancellationToken);
            await valueTask.ConfigureAwait(false);
            bufferedStream._writePos = 0;
          }
          valueTask = bufferedStream._stream.WriteAsync(buffer, cancellationToken);
          await valueTask.ConfigureAwait(false);
        }
      }
      finally
      {
        bufferedStream._asyncActiveSemaphore.Release();
      }
    }


    #nullable enable
    /// <summary>Begins an asynchronous write operation. (Consider using <see cref="M:System.IO.BufferedStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
    /// <param name="buffer">The buffer containing data to write to the current stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="callback">The method to be called when the asynchronous write operation is completed.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="buffer" /> length minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <returns>An object that references the asynchronous write which could still be pending.</returns>
    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      return TaskToApm.Begin(this.WriteAsync(buffer, offset, count, CancellationToken.None), callback, state);
    }

    /// <summary>Ends an asynchronous write operation and blocks until the I/O operation is complete. (Consider using <see cref="M:System.IO.BufferedStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
    /// <param name="asyncResult">The pending asynchronous request.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">This <see cref="T:System.IAsyncResult" /> object was not created by calling <see cref="M:System.IO.BufferedStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> on this class.</exception>
    public override void EndWrite(IAsyncResult asyncResult) => TaskToApm.End(asyncResult);

    /// <summary>Writes a byte to the current position in the buffered stream.</summary>
    /// <param name="value">A byte to write to the stream.</param>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public override void WriteByte(byte value)
    {
      if (this._writePos > 0 && this._writePos < this._bufferSize - 1)
        this._buffer[this._writePos++] = value;
      else
        this.WriteByteSlow(value);
    }

    private void WriteByteSlow(byte value)
    {
      this.EnsureNotClosed();
      if (this._writePos == 0)
      {
        this.EnsureCanWrite();
        this.ClearReadBufferBeforeWrite();
        this.EnsureBufferAllocated();
      }
      if (this._writePos >= this._bufferSize - 1)
        this.FlushWrite();
      this._buffer[this._writePos++] = value;
    }

    /// <summary>Sets the position within the current buffered stream.</summary>
    /// <param name="offset">A byte offset relative to <paramref name="origin" />.</param>
    /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point from which to obtain the new position.</param>
    /// <exception cref="T:System.IO.IOException">The stream is not open or is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The new position within the current buffered stream.</returns>
    public override long Seek(long offset, SeekOrigin origin)
    {
      this.EnsureNotClosed();
      this.EnsureCanSeek();
      if (this._writePos > 0)
      {
        this.FlushWrite();
        return this._stream.Seek(offset, origin);
      }
      if (this._readLen - this._readPos > 0 && origin == SeekOrigin.Current)
        offset -= (long) (this._readLen - this._readPos);
      long position = this.Position;
      long num1 = this._stream.Seek(offset, origin);
      long num2 = num1 - (position - (long) this._readPos);
      if (0L <= num2 && num2 < (long) this._readLen)
      {
        this._readPos = (int) num2;
        this._stream.Seek((long) (this._readLen - this._readPos), SeekOrigin.Current);
      }
      else
        this._readPos = this._readLen = 0;
      return num1;
    }

    /// <summary>Sets the length of the buffered stream.</summary>
    /// <param name="value">An integer indicating the desired length of the current buffered stream in bytes.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is not open or is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public override void SetLength(long value)
    {
      if (value < 0L)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      this.EnsureNotClosed();
      this.EnsureCanSeek();
      this.EnsureCanWrite();
      this.Flush();
      this._stream.SetLength(value);
    }

    /// <summary>Reads the bytes from the current buffered stream and writes them to another stream.</summary>
    /// <param name="destination">The stream to which the contents of the current buffered stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative or zero.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading.
    /// 
    /// -or-
    /// 
    /// <paramref name="destination" /> does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination" /> was closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)" /> method was called.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public override void CopyTo(Stream destination, int bufferSize)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      this.EnsureNotClosed();
      this.EnsureCanRead();
      int count = this._readLen - this._readPos;
      if (count > 0)
      {
        destination.Write(this._buffer, this._readPos, count);
        this._readPos = this._readLen = 0;
      }
      else if (this._writePos > 0)
        this.FlushWrite();
      this._stream.CopyTo(destination, bufferSize);
    }

    /// <summary>Asynchronously reads the bytes from the current buffered stream and writes them to another stream, using a specified buffer size and cancellation token.</summary>
    /// <param name="destination">The stream to which the contents of the current buffered stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default sizer is 81920.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public override Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      this.EnsureNotClosed();
      this.EnsureCanRead();
      return !cancellationToken.IsCancellationRequested ? this.CopyToAsyncCore(destination, bufferSize, cancellationToken) : (Task) Task.FromCanceled<int>(cancellationToken);
    }


    #nullable disable
    private async Task CopyToAsyncCore(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      BufferedStream bufferedStream = this;
      await bufferedStream.EnsureAsyncActiveSemaphoreInitialized().WaitAsync(cancellationToken).ConfigureAwait(false);
      try
      {
        int length = bufferedStream._readLen - bufferedStream._readPos;
        if (length > 0)
        {
          await destination.WriteAsync(new ReadOnlyMemory<byte>(bufferedStream._buffer, bufferedStream._readPos, length), cancellationToken).ConfigureAwait(false);
          bufferedStream._readPos = bufferedStream._readLen = 0;
        }
        else if (bufferedStream._writePos > 0)
          await bufferedStream.FlushWriteAsync(cancellationToken).ConfigureAwait(false);
        await bufferedStream._stream.CopyToAsync(destination, bufferSize, cancellationToken).ConfigureAwait(false);
      }
      finally
      {
        bufferedStream._asyncActiveSemaphore.Release();
      }
    }
  }
}
