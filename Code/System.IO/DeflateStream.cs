// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.DeflateStream
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Provides methods and properties for compressing and decompressing streams by using the Deflate algorithm.</summary>
  public class DeflateStream : Stream
  {

    #nullable disable
    private Stream _stream;
    private CompressionMode _mode;
    private bool _leaveOpen;
    private Inflater _inflater;
    private Deflater _deflater;
    private byte[] _buffer;
    private int _activeAsyncOperation;
    private bool _wroteBytes;

    internal DeflateStream(Stream stream, CompressionMode mode, long uncompressedSize)
      : this(stream, mode, false, -15, uncompressedSize)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.DeflateStream" /> class by using the specified stream and compression mode.</summary>
    /// <param name="stream">The stream to compress or decompress.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mode" /> is not a valid <see cref="T:System.IO.Compression.CompressionMode" /> value.
    /// 
    /// -or-
    /// 
    /// <see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Compress" /> and <see cref="P:System.IO.Stream.CanWrite" /> is <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// <see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Decompress" /> and <see cref="P:System.IO.Stream.CanRead" /> is <see langword="false" />.</exception>
    public DeflateStream(Stream stream, CompressionMode mode)
      : this(stream, mode, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.DeflateStream" /> class by using the specified stream and compression mode, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to compress or decompress.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after disposing the <see cref="T:System.IO.Compression.DeflateStream" /> object; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mode" /> is not a valid <see cref="T:System.IO.Compression.CompressionMode" /> value.
    /// 
    /// -or-
    /// 
    /// <see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Compress" /> and <see cref="P:System.IO.Stream.CanWrite" /> is <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// <see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Decompress" /> and <see cref="P:System.IO.Stream.CanRead" /> is <see langword="false" />.</exception>
    public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen)
      : this(stream, mode, leaveOpen, -15)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.DeflateStream" /> class by using the specified stream and compression level.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The stream does not support write operations such as compression. (The <see cref="P:System.IO.Stream.CanWrite" /> property on the stream object is <see langword="false" />.)</exception>
    public DeflateStream(Stream stream, CompressionLevel compressionLevel)
      : this(stream, compressionLevel, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.DeflateStream" /> class by using the specified stream and compression level, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream object open after disposing the <see cref="T:System.IO.Compression.DeflateStream" /> object; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The stream does not support write operations such as compression. (The <see cref="P:System.IO.Stream.CanWrite" /> property on the stream object is <see langword="false" />.)</exception>
    public DeflateStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
      : this(stream, compressionLevel, leaveOpen, -15)
    {
    }


    #nullable disable
    internal DeflateStream(
      Stream stream,
      CompressionMode mode,
      bool leaveOpen,
      int windowBits,
      long uncompressedSize = -1)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (mode != CompressionMode.Decompress)
      {
        if (mode != CompressionMode.Compress)
          throw new ArgumentException(SR.ArgumentOutOfRange_Enum, nameof (mode));
        this.InitializeDeflater(stream, leaveOpen, windowBits, CompressionLevel.Optimal);
      }
      else
      {
        if (!stream.CanRead)
          throw new ArgumentException(SR.NotSupported_UnreadableStream, nameof (stream));
        this._inflater = new Inflater(windowBits, uncompressedSize);
        this._stream = stream;
        this._mode = CompressionMode.Decompress;
        this._leaveOpen = leaveOpen;
      }
    }

    internal DeflateStream(
      Stream stream,
      CompressionLevel compressionLevel,
      bool leaveOpen,
      int windowBits)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      this.InitializeDeflater(stream, leaveOpen, windowBits, compressionLevel);
    }

    [MemberNotNull("_stream")]
    internal void InitializeDeflater(
      Stream stream,
      bool leaveOpen,
      int windowBits,
      CompressionLevel compressionLevel)
    {
      if (!stream.CanWrite)
        throw new ArgumentException(SR.NotSupported_UnwritableStream, nameof (stream));
      this._deflater = new Deflater(compressionLevel, windowBits);
      this._stream = stream;
      this._mode = CompressionMode.Compress;
      this._leaveOpen = leaveOpen;
      this.InitializeBuffer();
    }

    [MemberNotNull("_buffer")]
    private void InitializeBuffer() => this._buffer = ArrayPool<byte>.Shared.Rent(8192);

    [MemberNotNull("_buffer")]
    private void EnsureBufferInitialized()
    {
      if (this._buffer != null)
        return;
      this.InitializeBuffer();
    }


    #nullable enable
    /// <summary>Gets a reference to the underlying stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The underlying stream is closed.</exception>
    /// <returns>A stream object that represents the underlying stream.</returns>
    public Stream BaseStream => this._stream;

    /// <summary>Gets a value indicating whether the stream supports reading while decompressing a file.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Compression.CompressionMode" /> value is <see langword="Decompress" />, and the underlying stream is opened and supports reading; otherwise, <see langword="false" />.</returns>
    public override bool CanRead => this._stream != null && this._mode == CompressionMode.Decompress && this._stream.CanRead;

    /// <summary>Gets a value indicating whether the stream supports writing.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Compression.CompressionMode" /> value is <see langword="Compress" />, and the underlying stream supports writing and is not closed; otherwise, <see langword="false" />.</returns>
    public override bool CanWrite => this._stream != null && this._mode == CompressionMode.Compress && this._stream.CanWrite;

    /// <summary>Gets a value indicating whether the stream supports seeking.</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public override bool CanSeek => false;

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    /// <returns>A long value.</returns>
    public override long Length => throw new NotSupportedException(SR.NotSupported);

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    /// <returns>A long value.</returns>
    public override long Position
    {
      get => throw new NotSupportedException(SR.NotSupported);
      set => throw new NotSupportedException(SR.NotSupported);
    }

    /// <summary>The current implementation of this method has no functionality.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public override void Flush()
    {
      this.EnsureNotDisposed();
      if (this._mode != CompressionMode.Compress)
        return;
      this.FlushBuffers();
    }

    /// <summary>Asynchronously clears all buffers for this Deflate stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      this.EnsureNoActiveAsyncOperation();
      this.EnsureNotDisposed();
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      return this._mode == CompressionMode.Compress ? Core(cancellationToken) : Task.CompletedTask;


      #nullable disable
      async Task Core(CancellationToken cancellationToken)
      {
        this.AsyncOperationStarting();
        try
        {
          ValueTask valueTask = this.WriteDeflaterOutputAsync(cancellationToken);
          await valueTask.ConfigureAwait(false);
          bool flushSuccessful;
          do
          {
            int bytesRead;
            flushSuccessful = this._deflater.Flush(this._buffer, out bytesRead);
            if (flushSuccessful)
            {
              valueTask = this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, bytesRead), cancellationToken);
              await valueTask.ConfigureAwait(false);
            }
          }
          while (flushSuccessful);
          await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
          this.AsyncOperationCompleting();
        }
      }
    }

    /// <summary>This operation is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="offset">The location in the stream.</param>
    /// <param name="origin">One of the <see cref="T:System.IO.SeekOrigin" /> values.</param>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    /// <returns>A long value.</returns>
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException(SR.NotSupported);

    /// <summary>This operation is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The length of the stream.</param>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    public override void SetLength(long value) => throw new NotSupportedException(SR.NotSupported);

    /// <summary>Reads a byte from the Deflate stream and advances the position within the stream by one byte, or returns -1 if at the end of the Deflate stream.</summary>
    /// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
    public override int ReadByte()
    {
      this.EnsureDecompressionMode();
      this.EnsureNotDisposed();
      byte b;
      return !this._inflater.Inflate(out b) ? base.ReadByte() : (int) b;
    }


    #nullable enable
    /// <summary>Reads a number of decompressed bytes into the specified byte array.</summary>
    /// <param name="buffer" />
    /// <param name="offset">The byte offset in <paramref name="array" /> at which the read bytes will be placed.</param>
    /// <param name="count">The maximum number of decompressed bytes to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.IO.Compression.CompressionMode" /> value was <see langword="Compress" /> when the object was created.
    /// 
    /// -or-
    /// 
    ///  The underlying stream does not support reading.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="count" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="array" /> length minus the index starting point is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The data is in an invalid format.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>The number of bytes that were read into the byte array.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      return this.ReadCore(new Span<byte>(buffer, offset, count));
    }

    /// <summary>Reads a sequence of bytes from the current Deflate stream into a byte span and advances the position within the Deflate stream by the number of bytes read.</summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public override int Read(Span<byte> buffer) => this.GetType() != typeof (DeflateStream) ? base.Read(buffer) : this.ReadCore(buffer);


    #nullable disable
    internal int ReadCore(Span<byte> buffer)
    {
      this.EnsureDecompressionMode();
      this.EnsureNotDisposed();
      this.EnsureBufferInitialized();
      int num;
      do
      {
        num = this._inflater.Inflate(buffer);
        if (num == 0 && !this.InflatorIsFinished)
        {
          if (this._inflater.NeedsInput())
          {
            int count = this._stream.Read(this._buffer, 0, this._buffer.Length);
            if (count > 0)
            {
              if (count > this._buffer.Length)
                DeflateStream.ThrowGenericInvalidData();
              else
                this._inflater.SetInput(this._buffer, 0, count);
            }
            else
              break;
          }
        }
        else
          break;
      }
      while (!buffer.IsEmpty);
      return num;
    }

    private bool InflatorIsFinished
    {
      get
      {
        if (!this._inflater.Finished())
          return false;
        return !this._inflater.IsGzipStream() || !this._inflater.NeedsInput();
      }
    }

    private void EnsureNotDisposed()
    {
      if (this._stream != null)
        return;
      ThrowStreamClosedException();

      static void ThrowStreamClosedException() => throw new ObjectDisposedException(nameof (DeflateStream), SR.ObjectDisposed_StreamClosed);
    }

    private void EnsureDecompressionMode()
    {
      if (this._mode == CompressionMode.Decompress)
        return;
      ThrowCannotReadFromDeflateStreamException();

      static void ThrowCannotReadFromDeflateStreamException() => throw new InvalidOperationException(SR.CannotReadFromDeflateStream);
    }

    private void EnsureCompressionMode()
    {
      if (this._mode == CompressionMode.Compress)
        return;
      ThrowCannotWriteToDeflateStreamException();

      static void ThrowCannotWriteToDeflateStreamException() => throw new InvalidOperationException(SR.CannotWriteToDeflateStream);
    }

    private static void ThrowGenericInvalidData() => throw new InvalidDataException(SR.GenericInvalidData);


    #nullable enable
    /// <summary>Begins an asynchronous read operation. (Consider using the <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="buffer">The byte array to read the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin reading data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="asyncCallback">An optional asynchronous callback, to be called when the read operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.IO.IOException">The method tried to read asynchronously past the end of the stream, or a disk error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.IO.Compression.DeflateStream" /> implementation does not support the read operation.</exception>
    /// <exception cref="T:System.InvalidOperationException">This call cannot be completed.</exception>
    /// <returns>An  object that represents the asynchronous read operation, which could still be pending.</returns>
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
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Compression.DeflateStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.SystemException">An exception was thrown during a call to <see cref="M:System.Threading.WaitHandle.WaitOne" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The end call is invalid because asynchronous read operations for this stream are not yet complete.
    /// 
    /// -or-
    /// 
    /// The stream is <see langword="null" />.</exception>
    /// <returns>The number of bytes read from the stream, between 0 (zero) and the number of bytes you requested. <see cref="T:System.IO.Compression.DeflateStream" /> returns 0 only at the end of the stream; otherwise, it blocks until at least one byte is available.</returns>
    public override int EndRead(IAsyncResult asyncResult)
    {
      this.EnsureDecompressionMode();
      this.EnsureNotDisposed();
      return TaskToApm.End<int>(asyncResult);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current Deflate stream, writes them to a byte array, advances the position within the Deflate stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin writing data from the Deflate stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <param name="buffer" />
    /// <returns>A task that represents the asynchronous read operation, which wraps the total number of bytes read into the <paramref name="array" />. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the Deflate stream has been reached.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      return this.ReadAsyncMemory(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current Deflate stream, writes them to a byte memory range, advances the position within the Deflate stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the total number of bytes read into the buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the Deflate stream has been reached.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return this.GetType() != typeof (DeflateStream) ? base.ReadAsync(buffer, cancellationToken) : this.ReadAsyncMemory(buffer, cancellationToken);
    }


    #nullable disable
    internal ValueTask<int> ReadAsyncMemory(
      Memory<byte> buffer,
      CancellationToken cancellationToken)
    {
      this.EnsureDecompressionMode();
      this.EnsureNoActiveAsyncOperation();
      this.EnsureNotDisposed();
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled<int>(cancellationToken);
      this.EnsureBufferInitialized();
      return Core(buffer, cancellationToken);

      async ValueTask<int> Core(Memory<byte> buffer, CancellationToken cancellationToken)
      {
        this.AsyncOperationStarting();
        int num;
        try
        {
          int bytesRead;
          do
          {
            bytesRead = this._inflater.Inflate(buffer.Span);
            if (bytesRead == 0 && !this.InflatorIsFinished)
            {
              if (this._inflater.NeedsInput())
              {
                int count = await this._stream.ReadAsync(new Memory<byte>(this._buffer, 0, this._buffer.Length), cancellationToken).ConfigureAwait(false);
                if (count > 0)
                {
                  if (count > this._buffer.Length)
                    DeflateStream.ThrowGenericInvalidData();
                  else
                    this._inflater.SetInput(this._buffer, 0, count);
                }
                else
                  break;
              }
            }
            else
              break;
          }
          while (!buffer.IsEmpty);
          num = bytesRead;
        }
        finally
        {
          this.AsyncOperationCompleting();
        }
        return num;
      }
    }


    #nullable enable
    /// <summary>Writes compressed bytes to the underlying stream from the specified byte array.</summary>
    /// <param name="buffer" />
    /// <param name="offset">The byte offset in <paramref name="array" /> from which the bytes will be read.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    public override void Write(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      this.WriteCore(new ReadOnlySpan<byte>(buffer, offset, count));
    }

    /// <summary>Writes a sequence of bytes to the current Deflate stream and advances the current position within this Deflate stream by the number of bytes written.</summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the current Deflate stream.</param>
    public override void Write(ReadOnlySpan<byte> buffer)
    {
      if (this.GetType() != typeof (DeflateStream))
        base.Write(buffer);
      else
        this.WriteCore(buffer);
    }


    #nullable disable
    internal unsafe void WriteCore(ReadOnlySpan<byte> buffer)
    {
      this.EnsureCompressionMode();
      this.EnsureNotDisposed();
      this.WriteDeflaterOutput();
      fixed (byte* inputBufferPtr = &MemoryMarshal.GetReference<byte>(buffer))
      {
        this._deflater.SetInput(inputBufferPtr, buffer.Length);
        this.WriteDeflaterOutput();
        this._wroteBytes = true;
      }
    }

    private void WriteDeflaterOutput()
    {
      while (!this._deflater.NeedsInput())
      {
        int deflateOutput = this._deflater.GetDeflateOutput(this._buffer);
        if (deflateOutput > 0)
          this._stream.Write(this._buffer, 0, deflateOutput);
      }
    }

    private void FlushBuffers()
    {
      if (this._wroteBytes)
      {
        this.WriteDeflaterOutput();
        bool flag;
        do
        {
          int bytesRead;
          flag = this._deflater.Flush(this._buffer, out bytesRead);
          if (flag)
            this._stream.Write(this._buffer, 0, bytesRead);
        }
        while (flag);
      }
      this._stream.Flush();
    }

    private void PurgeBuffers(bool disposing)
    {
      if (!disposing || this._stream == null || this._mode != CompressionMode.Compress)
        return;
      if (this._wroteBytes)
      {
        this.WriteDeflaterOutput();
        bool flag;
        do
        {
          int bytesRead;
          flag = this._deflater.Finish(this._buffer, out bytesRead);
          if (bytesRead > 0)
            this._stream.Write(this._buffer, 0, bytesRead);
        }
        while (!flag);
      }
      else
      {
        do
          ;
        while (!this._deflater.Finish(this._buffer, out int _));
      }
    }

    private async ValueTask PurgeBuffersAsync()
    {
      if (this._stream == null || this._mode != CompressionMode.Compress)
        return;
      if (this._wroteBytes)
      {
        ConfiguredValueTaskAwaitable valueTaskAwaitable = this.WriteDeflaterOutputAsync(new CancellationToken()).ConfigureAwait(false);
        await valueTaskAwaitable;
        bool finished;
        do
        {
          int bytesRead;
          finished = this._deflater.Finish(this._buffer, out bytesRead);
          if (bytesRead > 0)
          {
            valueTaskAwaitable = this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, bytesRead)).ConfigureAwait(false);
            await valueTaskAwaitable;
          }
        }
        while (!finished);
      }
      else
      {
        do
          ;
        while (!this._deflater.Finish(this._buffer, out int _));
      }
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Compression.DeflateStream" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        this.PurgeBuffers(disposing);
      }
      finally
      {
        try
        {
          if (disposing)
          {
            if (!this._leaveOpen)
              this._stream?.Dispose();
          }
        }
        finally
        {
          this._stream = (Stream) null;
          try
          {
            this._deflater?.Dispose();
            this._inflater?.Dispose();
          }
          finally
          {
            this._deflater = (Deflater) null;
            this._inflater = (Inflater) null;
            byte[] buffer = this._buffer;
            if (buffer != null)
            {
              this._buffer = (byte[]) null;
              if (!this.AsyncOperationIsActive)
                ArrayPool<byte>.Shared.Return(buffer);
            }
            base.Dispose(disposing);
          }
        }
      }
    }

    /// <summary>Asynchronously releases the unmanaged resources used by the <see cref="T:System.IO.Compression.DeflateStream" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override ValueTask DisposeAsync()
    {
      return !(this.GetType() == typeof (DeflateStream)) ? base.DisposeAsync() : Core();

      async ValueTask Core()
      {
        try
        {
          await this.PurgeBuffersAsync().ConfigureAwait(false);
        }
        finally
        {
          Stream stream = this._stream;
          this._stream = (Stream) null;
          try
          {
            if (!this._leaveOpen)
            {
              if (stream != null)
                await stream.DisposeAsync().ConfigureAwait(false);
            }
          }
          finally
          {
            try
            {
              this._deflater?.Dispose();
              this._inflater?.Dispose();
            }
            finally
            {
              this._deflater = (Deflater) null;
              this._inflater = (Inflater) null;
              byte[] buffer = this._buffer;
              if (buffer != null)
              {
                this._buffer = (byte[]) null;
                if (!this.AsyncOperationIsActive)
                  ArrayPool<byte>.Shared.Return(buffer);
              }
            }
          }
        }
      }
    }


    #nullable enable
    /// <summary>Begins an asynchronous write operation. (Consider using the <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> to begin writing from.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="asyncCallback">An optional asynchronous callback, to be called when the write operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.IO.IOException">The method tried to write asynchronously past the end of the stream, or a disk error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.IO.Compression.DeflateStream" /> implementation does not support the write operation.</exception>
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

    /// <summary>Ends an asynchronous write operation. (Consider using the <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Compression.DeflateStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.Exception">An exception was thrown during a call to <see cref="M:System.Threading.WaitHandle.WaitOne" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The end write call is invalid.</exception>
    public override void EndWrite(IAsyncResult asyncResult)
    {
      this.EnsureCompressionMode();
      this.EnsureNotDisposed();
      TaskToApm.End(asyncResult);
    }

    /// <summary>Asynchronously writes compressed bytes to the underlying Deflate stream from the specified byte array.</summary>
    /// <param name="offset">The zero-based byte offset in <paramref name="array" /> from which to begin copying bytes to the Deflate stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <param name="buffer" />
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      return this.WriteAsyncMemory(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
    }

    /// <summary>Asynchronously writes compressed bytes to the underlying Deflate stream from the specified read-only memory region.</summary>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken)
    {
      return this.GetType() != typeof (DeflateStream) ? base.WriteAsync(buffer, cancellationToken) : this.WriteAsyncMemory(buffer, cancellationToken);
    }


    #nullable disable
    internal ValueTask WriteAsyncMemory(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken)
    {
      this.EnsureCompressionMode();
      this.EnsureNoActiveAsyncOperation();
      this.EnsureNotDisposed();
      return !cancellationToken.IsCancellationRequested ? Core(buffer, cancellationToken) : ValueTask.FromCanceled(cancellationToken);

      async ValueTask Core(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken)
      {
        this.AsyncOperationStarting();
        try
        {
          ValueTask valueTask = this.WriteDeflaterOutputAsync(cancellationToken);
          await valueTask.ConfigureAwait(false);
          this._deflater.SetInput(buffer);
          valueTask = this.WriteDeflaterOutputAsync(cancellationToken);
          await valueTask.ConfigureAwait(false);
          this._wroteBytes = true;
        }
        finally
        {
          this.AsyncOperationCompleting();
        }
      }
    }

    private async ValueTask WriteDeflaterOutputAsync(CancellationToken cancellationToken)
    {
      while (!this._deflater.NeedsInput())
      {
        int deflateOutput = this._deflater.GetDeflateOutput(this._buffer);
        if (deflateOutput > 0)
          await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, deflateOutput), cancellationToken).ConfigureAwait(false);
      }
    }


    #nullable enable
    /// <summary>Reads the bytes from the current Deflate stream and writes them to another stream, using a specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current Deflate stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
    public override void CopyTo(Stream destination, int bufferSize)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      this.EnsureNotDisposed();
      if (!this.CanRead)
        throw new NotSupportedException();
      new DeflateStream.CopyToStream(this, destination, bufferSize).CopyFromSourceToDestination();
    }

    /// <summary>Asynchronously reads the bytes from the current Deflate stream and writes them to another stream, using a specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current Deflate stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public override Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      this.EnsureNotDisposed();
      if (!this.CanRead)
        throw new NotSupportedException();
      this.EnsureNoActiveAsyncOperation();
      return cancellationToken.IsCancellationRequested ? (Task) Task.FromCanceled<int>(cancellationToken) : new DeflateStream.CopyToStream(this, destination, bufferSize, cancellationToken).CopyFromSourceToDestinationAsync();
    }

    private bool AsyncOperationIsActive => this._activeAsyncOperation != 0;

    private void EnsureNoActiveAsyncOperation()
    {
      if (!this.AsyncOperationIsActive)
        return;
      DeflateStream.ThrowInvalidBeginCall();
    }

    private void AsyncOperationStarting()
    {
      if (Interlocked.Exchange(ref this._activeAsyncOperation, 1) == 0)
        return;
      DeflateStream.ThrowInvalidBeginCall();
    }

    private void AsyncOperationCompleting() => Volatile.Write(ref this._activeAsyncOperation, 0);

    private static void ThrowInvalidBeginCall() => throw new InvalidOperationException(SR.InvalidBeginCall);


    #nullable disable
    private sealed class CopyToStream : Stream
    {
      private readonly DeflateStream _deflateStream;
      private readonly Stream _destination;
      private readonly CancellationToken _cancellationToken;
      private byte[] _arrayPoolBuffer;

      public CopyToStream(DeflateStream deflateStream, Stream destination, int bufferSize)
        : this(deflateStream, destination, bufferSize, CancellationToken.None)
      {
      }

      public CopyToStream(
        DeflateStream deflateStream,
        Stream destination,
        int bufferSize,
        CancellationToken cancellationToken)
      {
        this._deflateStream = deflateStream;
        this._destination = destination;
        this._cancellationToken = cancellationToken;
        this._arrayPoolBuffer = ArrayPool<byte>.Shared.Rent(bufferSize);
      }

      public async Task CopyFromSourceToDestinationAsync()
      {
        DeflateStream.CopyToStream destination = this;
        destination._deflateStream.AsyncOperationStarting();
        try
        {
          while (!destination._deflateStream._inflater.Finished())
          {
            int length = destination._deflateStream._inflater.Inflate(destination._arrayPoolBuffer, 0, destination._arrayPoolBuffer.Length);
            if (length > 0)
              await destination._destination.WriteAsync(new ReadOnlyMemory<byte>(destination._arrayPoolBuffer, 0, length), destination._cancellationToken).ConfigureAwait(false);
            else if (destination._deflateStream._inflater.NeedsInput())
              break;
          }
          await destination._deflateStream._stream.CopyToAsync((Stream) destination, destination._arrayPoolBuffer.Length, destination._cancellationToken).ConfigureAwait(false);
        }
        finally
        {
          destination._deflateStream.AsyncOperationCompleting();
          ArrayPool<byte>.Shared.Return(destination._arrayPoolBuffer);
          destination._arrayPoolBuffer = (byte[]) null;
        }
      }

      public void CopyFromSourceToDestination()
      {
        try
        {
          while (!this._deflateStream._inflater.Finished())
          {
            int count = this._deflateStream._inflater.Inflate(this._arrayPoolBuffer, 0, this._arrayPoolBuffer.Length);
            if (count > 0)
              this._destination.Write(this._arrayPoolBuffer, 0, count);
            else if (this._deflateStream._inflater.NeedsInput())
              break;
          }
          this._deflateStream._stream.CopyTo((Stream) this, this._arrayPoolBuffer.Length);
        }
        finally
        {
          ArrayPool<byte>.Shared.Return(this._arrayPoolBuffer);
          this._arrayPoolBuffer = (byte[]) null;
        }
      }

      public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        this._deflateStream.EnsureNotDisposed();
        if (count <= 0)
          return Task.CompletedTask;
        return count > buffer.Length - offset ? Task.FromException((Exception) new InvalidDataException(SR.GenericInvalidData)) : this.WriteAsyncCore((ReadOnlyMemory<byte>) buffer.AsMemory<byte>(offset, count), cancellationToken).AsTask();
      }

      public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default (CancellationToken))
      {
        this._deflateStream.EnsureNotDisposed();
        return this.WriteAsyncCore(buffer, cancellationToken);
      }

      private async ValueTask WriteAsyncCore(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken)
      {
        this._deflateStream._inflater.SetInput(buffer);
        while (!this._deflateStream._inflater.Finished())
        {
          int length = this._deflateStream._inflater.Inflate(new Span<byte>(this._arrayPoolBuffer));
          if (length > 0)
            await this._destination.WriteAsync(new ReadOnlyMemory<byte>(this._arrayPoolBuffer, 0, length), cancellationToken).ConfigureAwait(false);
          else if (this._deflateStream._inflater.NeedsInput())
            break;
        }
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
        this._deflateStream.EnsureNotDisposed();
        if (count <= 0)
          return;
        if (count > buffer.Length - offset)
          throw new InvalidDataException(SR.GenericInvalidData);
        this._deflateStream._inflater.SetInput(buffer, offset, count);
        while (!this._deflateStream._inflater.Finished())
        {
          int count1 = this._deflateStream._inflater.Inflate(new Span<byte>(this._arrayPoolBuffer));
          if (count1 > 0)
            this._destination.Write(this._arrayPoolBuffer, 0, count1);
          else if (this._deflateStream._inflater.NeedsInput())
            break;
        }
      }

      public override bool CanWrite => true;

      public override void Flush()
      {
      }

      public override bool CanRead => false;

      public override bool CanSeek => false;

      public override long Length => throw new NotSupportedException();

      public override long Position
      {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
      }

      public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

      public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

      public override void SetLength(long value) => throw new NotSupportedException();
    }
  }
}
