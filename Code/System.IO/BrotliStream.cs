// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.BrotliStream
// Assembly: System.IO.Compression.Brotli, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 531A6BB9-061C-413B-90D3-3B694AB08A91
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.Brotli.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\netstandard.xml

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Provides methods and properties used to compress and decompress streams by using the Brotli data format specification.</summary>
  public sealed class BrotliStream : Stream
  {
    private BrotliEncoder _encoder;
    private BrotliDecoder _decoder;
    private int _bufferOffset;
    private int _bufferCount;

    #nullable disable
    private Stream _stream;
    private byte[] _buffer;
    private readonly bool _leaveOpen;
    private readonly CompressionMode _mode;
    private int _activeAsyncOperation;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.BrotliStream" /> class by using the specified stream and compression level.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    public BrotliStream(Stream stream, CompressionLevel compressionLevel)
      : this(stream, compressionLevel, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.BrotliStream" /> class by using the specified stream and compression level, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after disposing the <see cref="T:System.IO.Compression.BrotliStream" /> object; otherwise, <see langword="false" />.</param>
    public BrotliStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
      : this(stream, CompressionMode.Compress, leaveOpen)
    {
      this._encoder.SetQuality(BrotliUtils.GetQualityFromCompressionLevel(compressionLevel));
    }

    /// <summary>Writes compressed bytes to the underlying stream from the specified byte array.</summary>
    /// <param name="buffer">The buffer containing the data to compress.</param>
    /// <param name="offset">The byte offset in <paramref name="array" /> from which the bytes will be read.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="T:System.ObjectDisposedException">The write operation cannot be performed because the stream is closed.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      this.WriteCore(new ReadOnlySpan<byte>(buffer, offset, count));
    }

    public override void WriteByte(byte value) => this.WriteCore(MemoryMarshal.CreateReadOnlySpan<byte>(ref value, 1));

    /// <summary>Writes a sequence of bytes to the current Brotli stream from a read-only byte span and advances the current position within this Brotli stream by the number of bytes written.</summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the current Brotli stream.</param>
    public override void Write(ReadOnlySpan<byte> buffer) => this.WriteCore(buffer);


    #nullable disable
    internal void WriteCore(ReadOnlySpan<byte> buffer, bool isFinalBlock = false)
    {
      if (this._mode != CompressionMode.Compress)
        throw new InvalidOperationException(SR.BrotliStream_Decompress_UnsupportedOperation);
      this.EnsureNotDisposed();
      OperationStatus operationStatus = OperationStatus.DestinationTooSmall;
      Span<byte> destination = new Span<byte>(this._buffer);
      while (operationStatus == OperationStatus.DestinationTooSmall)
      {
        int bytesConsumed;
        int bytesWritten;
        operationStatus = this._encoder.Compress(buffer, destination, out bytesConsumed, out bytesWritten, isFinalBlock);
        if (operationStatus == OperationStatus.InvalidData)
          throw new InvalidOperationException(SR.BrotliStream_Compress_InvalidData);
        if (bytesWritten > 0)
          this._stream.Write((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
        if (bytesConsumed > 0)
          buffer = buffer.Slice(bytesConsumed);
      }
    }


    #nullable enable
    /// <summary>Begins an asynchronous write operation. (Consider using the <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="buffer">The buffer from which data will be written.</param>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="asyncCallback">An optional asynchronous callback, to be called when the write operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.IO.IOException">The method tried to write asynchronously past the end of the stream, or a disk error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.IO.Compression.BrotliStream" /> implementation does not support the write operation.</exception>
    /// <exception cref="T:System.InvalidOperationException">The write operation cannot be performed because the stream is closed.</exception>
    /// <returns>An object that represents the asynchronous write operation, which could still be pending.</returns>
    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? asyncCallback,
      object? asyncState)
    {
      return TaskToApm.Begin(this.WriteAsync(buffer, offset, count, CancellationToken.None), asyncCallback, asyncState);
    }

    /// <summary>Handles the end of an asynchronous write operation. (Consider using the <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="asyncResult">The object that represents the asynchronous call.</param>
    /// <exception cref="T:System.InvalidOperationException">The underlying stream is closed or <see langword="null" />.</exception>
    public override void EndWrite(IAsyncResult asyncResult) => TaskToApm.End(asyncResult);

    /// <summary>Asynchronously writes compressed bytes to the underlying Brotli stream from the specified byte array.</summary>
    /// <param name="buffer">The buffer that contains the data to compress.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the Brotli stream.</param>
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
      return this.WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    /// <summary>Asynchronously writes compressed bytes to the underlying Brotli stream from the specified byte memory range.</summary>
    /// <param name="buffer">The memory region to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this._mode != CompressionMode.Compress)
        throw new InvalidOperationException(SR.BrotliStream_Decompress_UnsupportedOperation);
      this.EnsureNoActiveAsyncOperation();
      this.EnsureNotDisposed();
      return !cancellationToken.IsCancellationRequested ? this.WriteAsyncMemoryCore(buffer, cancellationToken) : ValueTask.FromCanceled(cancellationToken);
    }


    #nullable disable
    private async ValueTask WriteAsyncMemoryCore(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken,
      bool isFinalBlock = false)
    {
      this.AsyncOperationStarting();
      try
      {
        OperationStatus lastResult = OperationStatus.DestinationTooSmall;
        while (lastResult == OperationStatus.DestinationTooSmall)
        {
          Memory<byte> destination = new Memory<byte>(this._buffer);
          int bytesConsumed = 0;
          int bytesWritten = 0;
          lastResult = this._encoder.Compress(buffer, destination, out bytesConsumed, out bytesWritten, isFinalBlock);
          if (lastResult == OperationStatus.InvalidData)
            throw new InvalidOperationException(SR.BrotliStream_Compress_InvalidData);
          if (bytesConsumed > 0)
            buffer = buffer.Slice(bytesConsumed);
          if (bytesWritten > 0)
            await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, bytesWritten), cancellationToken).ConfigureAwait(false);
        }
      }
      finally
      {
        this.AsyncOperationCompleting();
      }
    }

    /// <summary>The current implementation of this method has no functionality.</summary>
    /// <exception cref="T:System.IO.InvalidDataException">The encoder ran into invalid data.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is disposed.</exception>
    public override void Flush()
    {
      this.EnsureNotDisposed();
      if (this._mode != CompressionMode.Compress || this._encoder._state == null || this._encoder._state.IsClosed)
        return;
      OperationStatus operationStatus = OperationStatus.DestinationTooSmall;
      Span<byte> destination = new Span<byte>(this._buffer);
      while (operationStatus == OperationStatus.DestinationTooSmall)
      {
        int bytesWritten;
        operationStatus = this._encoder.Flush(destination, out bytesWritten);
        if (operationStatus == OperationStatus.InvalidData)
          throw new InvalidDataException(SR.BrotliStream_Compress_InvalidData);
        if (bytesWritten > 0)
          this._stream.Write((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
      }
      this._stream.Flush();
    }


    #nullable enable
    /// <summary>Asynchronously clears all buffers for this Brotli stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      this.EnsureNoActiveAsyncOperation();
      this.EnsureNotDisposed();
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      return this._mode == CompressionMode.Compress ? this.FlushAsyncCore(cancellationToken) : Task.CompletedTask;
    }


    #nullable disable
    private async Task FlushAsyncCore(CancellationToken cancellationToken)
    {
      this.AsyncOperationStarting();
      try
      {
        if (this._encoder._state == null || this._encoder._state.IsClosed)
          return;
        OperationStatus lastResult = OperationStatus.DestinationTooSmall;
        while (lastResult == OperationStatus.DestinationTooSmall)
        {
          Memory<byte> destination = new Memory<byte>(this._buffer);
          int bytesWritten = 0;
          lastResult = this._encoder.Flush(destination, out bytesWritten);
          if (lastResult == OperationStatus.InvalidData)
            throw new InvalidDataException(SR.BrotliStream_Compress_InvalidData);
          if (bytesWritten > 0)
            await this._stream.WriteAsync((ReadOnlyMemory<byte>) destination.Slice(0, bytesWritten), cancellationToken).ConfigureAwait(false);
        }
        await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
      }
      finally
      {
        this.AsyncOperationCompleting();
      }
    }


    #nullable enable
    /// <summary>Reads a number of decompressed bytes into the specified byte array.</summary>
    /// <param name="buffer">The array used to store decompressed bytes.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which the read bytes will be placed.</param>
    /// <param name="count">The maximum number of decompressed bytes to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.IO.Compression.CompressionMode" /> value was <see langword="Compress" /> when the object was created, or there is already an active asynchronous operation on this stream.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="buffer" /> length minus the index starting point is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The data is in an invalid format.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The underlying stream is null or closed.</exception>
    /// <returns>The number of bytes that were decompressed into the byte array. If the end of the stream has been reached, zero or the number of bytes read is returned.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      return this.Read(new Span<byte>(buffer, offset, count));
    }

    public override int ReadByte()
    {
      byte reference = 0;
      return this.Read(MemoryMarshal.CreateSpan<byte>(ref reference, 1)) == 0 ? -1 : (int) reference;
    }

    /// <summary>Reads a sequence of bytes from the current Brotli stream to a byte span and advances the position within the Brotli stream by the number of bytes read.</summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public override int Read(Span<byte> buffer)
    {
      if (this._mode != CompressionMode.Decompress)
        throw new InvalidOperationException(SR.BrotliStream_Compress_UnsupportedOperation);
      this.EnsureNotDisposed();
      int bytesWritten;
      while (!this.TryDecompress(buffer, out bytesWritten))
      {
        int num = this._stream.Read(this._buffer, this._bufferCount, this._buffer.Length - this._bufferCount);
        if (num > 0)
        {
          this._bufferCount += num;
          if (this._bufferCount > this._buffer.Length)
            BrotliStream.ThrowInvalidStream();
        }
        else
          break;
      }
      return bytesWritten;
    }

    /// <summary>Begins an asynchronous read operation. (Consider using the <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="buffer">The buffer from which data will be read.</param>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin reading data from the stream.</param>
    /// <param name="count">To maximum number of bytes to read.</param>
    /// <param name="asyncCallback">An optional asynchronous callback, to be called when the read operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.IO.IOException">The method tried to read asynchronously past the end of the stream, or a disk error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.IO.Compression.BrotliStream" /> implementation does not support the read operation.</exception>
    /// <exception cref="T:System.InvalidOperationException">This call cannot be completed.</exception>
    /// <returns>An object that represents the asynchronous read operation, which could still be pending.</returns>
    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? asyncCallback,
      object? asyncState)
    {
      return TaskToApm.Begin((Task) this.ReadAsync(buffer, offset, count, CancellationToken.None), asyncCallback, asyncState);
    }

    /// <summary>Waits for the pending asynchronous read to complete. (Consider using the <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Compression.BrotliStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.InvalidOperationException">The end operation cannot be performed because the stream is closed.</exception>
    /// <returns>The number of bytes read from the stream, between 0 (zero) and the number of bytes you requested. <see cref="T:System.IO.Compression.BrotliStream" /> returns 0 only at the end of the stream; otherwise, it blocks until at least one byte is available.</returns>
    public override int EndRead(IAsyncResult asyncResult) => TaskToApm.End<int>(asyncResult);

    /// <summary>Asynchronously reads a sequence of bytes from the current Brotli stream, writes them to a byte array starting at a specified index, advances the position within the Brotli stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the Brotli stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the total number of bytes read into the <paramref name="buffer" />. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the Brotli stream has been reached.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      return this.ReadAsync(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current Brotli stream, writes them to a byte memory range, advances the position within the Brotli stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the total number of bytes read into the buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the Brotli stream has been reached.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this._mode != CompressionMode.Decompress)
        throw new InvalidOperationException(SR.BrotliStream_Compress_UnsupportedOperation);
      this.EnsureNoActiveAsyncOperation();
      this.EnsureNotDisposed();
      return cancellationToken.IsCancellationRequested ? ValueTask.FromCanceled<int>(cancellationToken) : Core(buffer, cancellationToken);


      #nullable disable
      async ValueTask<int> Core(Memory<byte> buffer, CancellationToken cancellationToken)
      {
        this.AsyncOperationStarting();
        int num1;
        try
        {
          int bytesWritten;
          while (!this.TryDecompress(buffer.Span, out bytesWritten))
          {
            int num2 = await this._stream.ReadAsync(this._buffer.AsMemory<byte>(this._bufferCount), cancellationToken).ConfigureAwait(false);
            if (num2 > 0)
            {
              this._bufferCount += num2;
              if (this._bufferCount > this._buffer.Length)
                BrotliStream.ThrowInvalidStream();
            }
            else
              break;
          }
          num1 = bytesWritten;
        }
        finally
        {
          this.AsyncOperationCompleting();
        }
        return num1;
      }
    }

    private bool TryDecompress(Span<byte> destination, out int bytesWritten)
    {
      int bytesConsumed;
      OperationStatus operationStatus = this._decoder.Decompress(new ReadOnlySpan<byte>(this._buffer, this._bufferOffset, this._bufferCount), destination, out bytesConsumed, out bytesWritten);
      if (operationStatus == OperationStatus.InvalidData)
        throw new InvalidOperationException(SR.BrotliStream_Decompress_InvalidData);
      if (bytesConsumed != 0)
      {
        this._bufferOffset += bytesConsumed;
        this._bufferCount -= bytesConsumed;
      }
      if (bytesWritten != 0 || operationStatus == OperationStatus.Done || destination.IsEmpty && this._bufferCount != 0)
        return true;
      if (this._bufferCount != 0 && this._bufferOffset != 0)
        new ReadOnlySpan<byte>(this._buffer, this._bufferOffset, this._bufferCount).CopyTo((Span<byte>) this._buffer);
      this._bufferOffset = 0;
      return false;
    }

    private static void ThrowInvalidStream() => throw new InvalidDataException(SR.BrotliStream_Decompress_InvalidStream);


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.BrotliStream" /> class by using the specified stream and compression mode.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    public BrotliStream(Stream stream, CompressionMode mode)
      : this(stream, mode, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.BrotliStream" /> class by using the specified stream and compression mode, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.Compression.BrotliStream" /> object is disposed; otherwise, <see langword="false" />.</param>
    public BrotliStream(Stream stream, CompressionMode mode, bool leaveOpen)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (mode != CompressionMode.Decompress)
      {
        if (mode != CompressionMode.Compress)
          throw new ArgumentException(SR.ArgumentOutOfRange_Enum, nameof (mode));
        if (!stream.CanWrite)
          throw new ArgumentException(SR.Stream_FalseCanWrite, nameof (stream));
      }
      else if (!stream.CanRead)
        throw new ArgumentException(SR.Stream_FalseCanRead, nameof (stream));
      this._mode = mode;
      this._stream = stream;
      this._leaveOpen = leaveOpen;
      this._buffer = ArrayPool<byte>.Shared.Rent(65520);
    }

    private void EnsureNotDisposed()
    {
      if (this._stream == null)
        throw new ObjectDisposedException(this.GetType().Name, SR.ObjectDisposed_StreamClosed);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this._stream == null)
          return;
        if (this._mode == CompressionMode.Compress)
          this.WriteCore(ReadOnlySpan<byte>.Empty, true);
        if (this._leaveOpen)
          return;
        this._stream.Dispose();
      }
      finally
      {
        this.ReleaseStateForDispose();
        base.Dispose(disposing);
      }
    }

    /// <summary>Asynchronously releases the unmanaged resources used by the <see cref="T:System.IO.Compression.BrotliStream" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override async ValueTask DisposeAsync()
    {
      try
      {
        if (this._stream == null)
          return;
        ConfiguredValueTaskAwaitable valueTaskAwaitable;
        if (this._mode == CompressionMode.Compress)
        {
          valueTaskAwaitable = this.WriteAsyncMemoryCore(ReadOnlyMemory<byte>.Empty, CancellationToken.None, true).ConfigureAwait(false);
          await valueTaskAwaitable;
        }
        if (this._leaveOpen)
          return;
        valueTaskAwaitable = this._stream.DisposeAsync().ConfigureAwait(false);
        await valueTaskAwaitable;
      }
      finally
      {
        this.ReleaseStateForDispose();
      }
    }

    private void ReleaseStateForDispose()
    {
      this._stream = (Stream) null;
      this._encoder.Dispose();
      this._decoder.Dispose();
      byte[] buffer = this._buffer;
      if (buffer == null)
        return;
      this._buffer = (byte[]) null;
      if (this.AsyncOperationIsActive)
        return;
      ArrayPool<byte>.Shared.Return(buffer);
    }

    /// <summary>Gets a reference to the underlying stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The underlying stream is closed.</exception>
    /// <returns>A stream object that represents the underlying stream.</returns>
    public Stream BaseStream => this._stream;

    /// <summary>Gets a value indicating whether the stream supports reading while decompressing a file.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Compression.CompressionMode" /> value is <see langword="Decompress," /> and the underlying stream supports reading and is not closed; otherwise, <see langword="false" />.</returns>
    public override bool CanRead => this._mode == CompressionMode.Decompress && this._stream != null && this._stream.CanRead;

    /// <summary>Gets a value indicating whether the stream supports writing.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Compression.CompressionMode" /> value is <see langword="Compress" />, and the underlying stream supports writing and is not closed; otherwise, <see langword="false" />.</returns>
    public override bool CanWrite => this._mode == CompressionMode.Compress && this._stream != null && this._stream.CanWrite;

    /// <summary>Gets a value indicating whether the stream supports seeking.</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public override bool CanSeek => false;

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="offset">The location in the stream.</param>
    /// <param name="origin">One of the <see cref="T:System.IO.SeekOrigin" /> values.</param>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    /// <returns>A long value.</returns>
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    /// <returns>A long value.</returns>
    public override long Length => throw new NotSupportedException();

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    /// <returns>A long value.</returns>
    public override long Position
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The length of the stream.</param>
    public override void SetLength(long value) => throw new NotSupportedException();

    private bool AsyncOperationIsActive => this._activeAsyncOperation != 0;

    private void EnsureNoActiveAsyncOperation()
    {
      if (!this.AsyncOperationIsActive)
        return;
      BrotliStream.ThrowInvalidBeginCall();
    }

    private void AsyncOperationStarting()
    {
      if (Interlocked.Exchange(ref this._activeAsyncOperation, 1) == 0)
        return;
      BrotliStream.ThrowInvalidBeginCall();
    }

    private void AsyncOperationCompleting() => Volatile.Write(ref this._activeAsyncOperation, 0);

    private static void ThrowInvalidBeginCall() => throw new InvalidOperationException(SR.InvalidBeginCall);
  }
}
