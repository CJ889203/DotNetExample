// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.GZipStream
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Provides methods and properties used to compress and decompress streams by using the GZip data format specification.</summary>
  public class GZipStream : Stream
  {

    #nullable disable
    private DeflateStream _deflateStream;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.GZipStream" /> class by using the specified stream and compression mode.</summary>
    /// <param name="stream">The stream the compressed or decompressed data is written to.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mode" /> is not a valid <see cref="T:System.IO.Compression.CompressionMode" /> enumeration value.
    /// 
    /// -or-
    /// 
    /// <see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Compress" /> and <see cref="P:System.IO.Stream.CanWrite" /> is <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// <see cref="T:System.IO.Compression.CompressionMode" /> is <see cref="F:System.IO.Compression.CompressionMode.Decompress" /> and <see cref="P:System.IO.Stream.CanRead" /> is <see langword="false" />.</exception>
    public GZipStream(Stream stream, CompressionMode mode)
      : this(stream, mode, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.GZipStream" /> class by using the specified stream and compression mode, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after disposing the <see cref="T:System.IO.Compression.GZipStream" /> object; otherwise, <see langword="false" />.</param>
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
    public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) => this._deflateStream = new DeflateStream(stream, mode, leaveOpen, 31);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.GZipStream" /> class by using the specified stream and compression level.</summary>
    /// <param name="stream">The stream to compress.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The stream does not support write operations such as compression. (The <see cref="P:System.IO.Stream.CanWrite" /> property on the stream object is <see langword="false" />.)</exception>
    public GZipStream(Stream stream, CompressionLevel compressionLevel)
      : this(stream, compressionLevel, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.GZipStream" /> class by using the specified stream and compression level, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to write the compressed data to.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream object open after disposing the <see cref="T:System.IO.Compression.GZipStream" /> object; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The stream does not support write operations such as compression. (The <see cref="P:System.IO.Stream.CanWrite" /> property on the stream object is <see langword="false" />.)</exception>
    public GZipStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen) => this._deflateStream = new DeflateStream(stream, compressionLevel, leaveOpen, 31);

    /// <summary>Gets a value indicating whether the stream supports reading while decompressing a file.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Compression.CompressionMode" /> value is <see langword="Decompress," /> and the underlying stream supports reading and is not closed; otherwise, <see langword="false" />.</returns>
    public override bool CanRead
    {
      get
      {
        DeflateStream deflateStream = this._deflateStream;
        return deflateStream != null && deflateStream.CanRead;
      }
    }

    /// <summary>Gets a value indicating whether the stream supports writing.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.Compression.CompressionMode" /> value is <see langword="Compress" />, and the underlying stream supports writing and is not closed; otherwise, <see langword="false" />.</returns>
    public override bool CanWrite
    {
      get
      {
        DeflateStream deflateStream = this._deflateStream;
        return deflateStream != null && deflateStream.CanWrite;
      }
    }

    /// <summary>Gets a value indicating whether the stream supports seeking.</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.</returns>
    public override bool CanSeek
    {
      get
      {
        DeflateStream deflateStream = this._deflateStream;
        return deflateStream != null && deflateStream.CanSeek;
      }
    }

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

    /// <summary>Flushes the internal buffers.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The underlying stream is closed.</exception>
    public override void Flush()
    {
      this.CheckDeflateStream();
      this._deflateStream.Flush();
    }

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="offset">The location in the stream.</param>
    /// <param name="origin">One of the <see cref="T:System.IO.SeekOrigin" /> values.</param>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    /// <returns>A long value.</returns>
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException(SR.NotSupported);

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">The length of the stream.</param>
    /// <exception cref="T:System.NotSupportedException">This property is not supported on this stream.</exception>
    public override void SetLength(long value) => throw new NotSupportedException(SR.NotSupported);

    /// <summary>Reads a byte from the GZip stream and advances the position within the stream by one byte, or returns -1 if at the end of the GZip stream.</summary>
    /// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
    public override int ReadByte()
    {
      this.CheckDeflateStream();
      return this._deflateStream.ReadByte();
    }

    /// <summary>Begins an asynchronous read operation. (Consider using the <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="buffer">The byte array to read the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin reading data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="asyncCallback">An optional asynchronous callback, to be called when the read operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.IO.IOException">The method tried to  read asynchronously past the end of the stream, or a disk error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.IO.Compression.GZipStream" /> implementation does not support the read operation.</exception>
    /// <exception cref="T:System.InvalidOperationException">A read operation cannot be performed because the stream is closed.</exception>
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
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Compression.GZipStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.InvalidOperationException">The end operation cannot be performed because the stream is closed.</exception>
    /// <returns>The number of bytes read from the stream, between 0 (zero) and the number of bytes you requested. <see cref="T:System.IO.Compression.GZipStream" /> returns 0 only at the end of the stream; otherwise, it blocks until at least one byte is available.</returns>
    public override int EndRead(IAsyncResult asyncResult) => this._deflateStream.EndRead(asyncResult);

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
    /// <returns>The number of bytes that were decompressed into the byte array. If the end of the stream has been reached, zero or the number of bytes read is returned.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      this.CheckDeflateStream();
      return this._deflateStream.Read(buffer, offset, count);
    }

    /// <summary>Reads a sequence of bytes from the current GZip stream into a byte span and advances the position within the GZip stream by the number of bytes read.</summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public override int Read(Span<byte> buffer)
    {
      if (this.GetType() != typeof (GZipStream))
        return base.Read(buffer);
      this.CheckDeflateStream();
      return this._deflateStream.ReadCore(buffer);
    }

    /// <summary>Begins an asynchronous write operation. (Consider using the <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> method instead.)</summary>
    /// <param name="buffer">The buffer containing data to write to the current stream.</param>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="asyncCallback">An optional asynchronous callback to be called when the write operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.InvalidOperationException">The underlying stream is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The underlying stream is closed.</exception>
    /// <returns>An  object that represents the asynchronous write operation, which could still be pending.</returns>
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
    /// <exception cref="T:System.InvalidOperationException">The underlying stream is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The underlying stream is closed.</exception>
    public override void EndWrite(IAsyncResult asyncResult) => this._deflateStream.EndWrite(asyncResult);

    /// <summary>Writes compressed bytes to the underlying GZip stream from the specified byte array.</summary>
    /// <param name="buffer" />
    /// <param name="offset">The byte offset in <paramref name="array" /> from which the bytes will be read.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="T:System.ObjectDisposedException">The write operation cannot be performed because the stream is closed.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      this.CheckDeflateStream();
      this._deflateStream.Write(buffer, offset, count);
    }

    /// <summary>Writes a sequence of bytes to the current GZip stream from a read-only byte span and advances the current position within this GZip stream by the number of bytes written.</summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the current GZip stream.</param>
    public override void Write(ReadOnlySpan<byte> buffer)
    {
      if (this.GetType() != typeof (GZipStream))
      {
        base.Write(buffer);
      }
      else
      {
        this.CheckDeflateStream();
        this._deflateStream.WriteCore(buffer);
      }
    }

    /// <summary>Reads the bytes from the current GZip stream and writes them to another stream, using a specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current GZip stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
    public override void CopyTo(Stream destination, int bufferSize)
    {
      this.CheckDeflateStream();
      this._deflateStream.CopyTo(destination, bufferSize);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Compression.GZipStream" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing && this._deflateStream != null)
          this._deflateStream.Dispose();
        this._deflateStream = (DeflateStream) null;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    /// <summary>Asynchronously releases the unmanaged resources used by the <see cref="T:System.IO.Compression.GZipStream" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override ValueTask DisposeAsync()
    {
      if (this.GetType() != typeof (GZipStream))
        return base.DisposeAsync();
      DeflateStream deflateStream = this._deflateStream;
      if (deflateStream == null)
        return new ValueTask();
      this._deflateStream = (DeflateStream) null;
      return deflateStream.DisposeAsync();
    }

    /// <summary>Gets a reference to the underlying stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The underlying stream is closed.</exception>
    /// <returns>A stream object that represents the underlying stream.</returns>
    public Stream BaseStream => this._deflateStream?.BaseStream;

    /// <summary>Asynchronously reads a sequence of bytes from the current GZip stream into a byte array, advances the position within the GZip stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin writing data from the GZip stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <param name="buffer" />
    /// <returns>A task that represents the asynchronous read operation, which wraps the total number of bytes read into the <paramref name="array" />. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the GZip stream has been reached.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      this.CheckDeflateStream();
      return this._deflateStream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current GZip stream into a byte memory region, advances the position within the GZip stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the total number of bytes read into the buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the GZip stream has been reached.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (GZipStream))
        return base.ReadAsync(buffer, cancellationToken);
      this.CheckDeflateStream();
      return this._deflateStream.ReadAsyncMemory(buffer, cancellationToken);
    }

    /// <summary>Asynchronously writes compressed bytes to the underlying GZip stream from the specified byte array.</summary>
    /// <param name="offset">The zero-based byte offset in <paramref name="array" /> from which to begin copying bytes to the GZip stream.</param>
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
      this.CheckDeflateStream();
      return this._deflateStream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously writes compressed bytes to the underlying GZip stream from the specified read-only byte memory region.</summary>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (GZipStream))
        return base.WriteAsync(buffer, cancellationToken);
      this.CheckDeflateStream();
      return this._deflateStream.WriteAsyncMemory(buffer, cancellationToken);
    }

    /// <summary>Asynchronously clears all buffers for this GZip stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      this.CheckDeflateStream();
      return this._deflateStream.FlushAsync(cancellationToken);
    }

    /// <summary>Asynchronously reads the bytes from the current GZip stream and writes them to another stream, using a specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current GZip stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public override Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      this.CheckDeflateStream();
      return this._deflateStream.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    private void CheckDeflateStream()
    {
      if (this._deflateStream != null)
        return;
      GZipStream.ThrowStreamClosedException();
    }

    private static void ThrowStreamClosedException() => throw new ObjectDisposedException(nameof (GZipStream), SR.ObjectDisposed_StreamClosed);
  }
}
