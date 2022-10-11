// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.ZLibStream
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Provides methods and properties used to compress and decompress streams by using the zlib data format specification.</summary>
  public sealed class ZLibStream : Stream
  {

    #nullable disable
    private DeflateStream _deflateStream;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZLibStream" /> class by using the specified stream and compression mode.</summary>
    /// <param name="stream">The stream to which compressed data is written or from which decompressed data is read.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    public ZLibStream(Stream stream, CompressionMode mode)
      : this(stream, mode, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZLibStream" /> class by using the specified stream, compression mode, and whether to leave the <paramref name="stream" /> open.</summary>
    /// <param name="stream">The stream to which compressed data is written or from which decompressed data is read.</param>
    /// <param name="mode">One of the enumeration values that indicates whether to compress or decompress the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream object open after disposing the <see cref="T:System.IO.Compression.ZLibStream" /> object; otherwise, <see langword="false" />.</param>
    public ZLibStream(Stream stream, CompressionMode mode, bool leaveOpen) => this._deflateStream = new DeflateStream(stream, mode, leaveOpen, 15);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZLibStream" /> class by using the specified stream and compression level.</summary>
    /// <param name="stream">The stream to which compressed data is written.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    public ZLibStream(Stream stream, CompressionLevel compressionLevel)
      : this(stream, compressionLevel, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZLibStream" /> class by using the specified stream, compression level, and whether to leave the <paramref name="stream" /> open.</summary>
    /// <param name="stream">The stream to which compressed data is written.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression efficiency when compressing the stream.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream object open after disposing the <see cref="T:System.IO.Compression.ZLibStream" /> object; otherwise, <see langword="false" />.</param>
    public ZLibStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen) => this._deflateStream = new DeflateStream(stream, compressionLevel, leaveOpen, 15);

    /// <summary>Gets a value indicating whether the stream supports reading.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports reading; <see langword="false" /> otherwise.</returns>
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
    /// <see langword="true" /> if the stream supports writing; <see langword="false" /> otherwise.</returns>
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
    /// <see langword="true" /> if the stream supports seeking; <see langword="false" /> otherwise.</returns>
    public override bool CanSeek => false;

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <returns>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</returns>
    public override long Length => throw new NotSupportedException();

    /// <summary>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <returns>This property is not supported and always throws a <see cref="T:System.NotSupportedException" />.</returns>
    public override long Position
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    /// <summary>Flushes the internal buffers.</summary>
    public override void Flush()
    {
      this.ThrowIfClosed();
      this._deflateStream.Flush();
    }

    /// <summary>Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      this.ThrowIfClosed();
      return this._deflateStream.FlushAsync(cancellationToken);
    }

    /// <summary>This method is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="offset">Not supported.</param>
    /// <param name="origin">Not supported.</param>
    /// <returns>This method is not supported and always throws a <see cref="T:System.NotSupportedException" />.</returns>
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    /// <summary>This method is not supported and always throws a <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="value">Not supported.</param>
    public override void SetLength(long value) => throw new NotSupportedException();

    /// <summary>Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.</summary>
    /// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
    public override int ReadByte()
    {
      this.ThrowIfClosed();
      return this._deflateStream.ReadByte();
    }

    /// <summary>Begins an asynchronous read operation.</summary>
    /// <param name="buffer">The byte array to read the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin reading data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="asyncCallback">An optional asynchronous callback, to be called when the read operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="offset" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="offset" /> and <paramref name="count" /> were out of bounds for the array.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is greater than the number of elements from <paramref name="offset" /> to the end of <paramref name="buffer" />.</exception>
    /// <returns>An object that represents the asynchronous read operation, which could still be pending.</returns>
    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? asyncCallback,
      object? asyncState)
    {
      this.ThrowIfClosed();
      return this._deflateStream.BeginRead(buffer, offset, count, asyncCallback, asyncState);
    }

    /// <summary>Waits for the pending asynchronous read to complete.</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
    /// <returns>The number of bytes that were read into the byte array.</returns>
    public override int EndRead(IAsyncResult asyncResult) => this._deflateStream.EndRead(asyncResult);

    /// <summary>Reads a number of decompressed bytes into the specified byte array.</summary>
    /// <param name="buffer">The byte array to read the data into.</param>
    /// <param name="offset">The byte offset in array at which to begin reading data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <returns>The number of bytes that were read into the byte array.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      this.ThrowIfClosed();
      return this._deflateStream.Read(buffer, offset, count);
    }

    /// <summary>Reads a number of decompressed bytes into the specified byte span.</summary>
    /// <param name="buffer">The span to read the data into.</param>
    /// <returns>The number of bytes that were read into the byte span.</returns>
    public override int Read(Span<byte> buffer)
    {
      this.ThrowIfClosed();
      return this._deflateStream.ReadCore(buffer);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The byte array to read the data into.</param>
    /// <param name="offset">The byte offset in array at which to begin reading data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous completion of the operation.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      this.ThrowIfClosed();
      return this._deflateStream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The byte span to read the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous completion of the operation.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      this.ThrowIfClosed();
      return this._deflateStream.ReadAsyncMemory(buffer, cancellationToken);
    }

    /// <summary>Begins an asynchronous write operation.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> to begin writing from.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="asyncCallback">An optional asynchronous callback, to be called when the write operation is complete.</param>
    /// <param name="asyncState">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="offset" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="offset" /> and <paramref name="count" /> were out of bounds for the array.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is greater than the number of elements from <paramref name="offset" /> to the end of <paramref name="buffer" />.</exception>
    /// <returns>An object that represents the asynchronous write operation, which could still be pending.</returns>
    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? asyncCallback,
      object? asyncState)
    {
      this.ThrowIfClosed();
      return this._deflateStream.BeginWrite(buffer, offset, count, asyncCallback, asyncState);
    }

    /// <summary>Ends an asynchronous write operation.</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
    public override void EndWrite(IAsyncResult asyncResult) => this._deflateStream.EndWrite(asyncResult);

    /// <summary>Writes compressed bytes to the underlying stream from the specified byte array.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The byte offset in buffer to begin writing from.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    public override void Write(byte[] buffer, int offset, int count)
    {
      this.ThrowIfClosed();
      this._deflateStream.Write(buffer, offset, count);
    }

    /// <summary>Writes compressed bytes to the underlying stream from the specified byte span.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    public override void Write(ReadOnlySpan<byte> buffer)
    {
      this.ThrowIfClosed();
      this._deflateStream.WriteCore(buffer);
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The byte offset in buffer to begin writing from.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous completion of the operation.</returns>
    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      this.ThrowIfClosed();
      return this._deflateStream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous completion of the operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      this.ThrowIfClosed();
      return this._deflateStream.WriteAsyncMemory(buffer, cancellationToken);
    }

    /// <summary>Writes a byte to the current position in the stream and advances the position within the stream by one byte.</summary>
    /// <param name="value">The byte to write to the stream.</param>
    public override void WriteByte(byte value)
    {
      this.ThrowIfClosed();
      this._deflateStream.WriteByte(value);
    }

    /// <summary>Reads the bytes from the current stream and writes them to another stream, using the specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is not a positive number.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public override void CopyTo(Stream destination, int bufferSize)
    {
      this.ThrowIfClosed();
      this._deflateStream.CopyTo(destination, bufferSize);
    }

    /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer size and cancellation token.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is not a positive number.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading or writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.InvalidOperationException">Only one asynchronous reader or writer is allowed at a time.</exception>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public override Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      this.ThrowIfClosed();
      return this._deflateStream.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing)
          this._deflateStream?.Dispose();
        this._deflateStream = (DeflateStream) null;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    /// <summary>Asynchronously releases all resources used by the stream.</summary>
    /// <returns>A task that represents the completion of the disposal operation.</returns>
    public override ValueTask DisposeAsync()
    {
      DeflateStream deflateStream = this._deflateStream;
      if (deflateStream == null)
        return new ValueTask();
      this._deflateStream = (DeflateStream) null;
      return deflateStream.DisposeAsync();
    }

    /// <summary>Gets a reference to the underlying stream.</summary>
    public Stream BaseStream => this._deflateStream?.BaseStream;

    private void ThrowIfClosed()
    {
      if (this._deflateStream != null)
        return;
      ZLibStream.ThrowClosedException();
    }

    [DoesNotReturn]
    private static void ThrowClosedException() => throw new ObjectDisposedException(nameof (ZLibStream), SR.ObjectDisposed_StreamClosed);
  }
}
