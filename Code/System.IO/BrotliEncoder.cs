// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.BrotliEncoder
// Assembly: System.IO.Compression.Brotli, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 531A6BB9-061C-413B-90D3-3B694AB08A91
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.Brotli.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\netstandard.xml

using Microsoft.Win32.SafeHandles;
using System.Buffers;
using System.Runtime.InteropServices;

namespace System.IO.Compression
{
  /// <summary>Provides methods and static methods to encode and decode data in a streamless, non-allocating, and performant manner using the Brotli data format specification.</summary>
  public struct BrotliEncoder : IDisposable
  {
    internal SafeBrotliEncoderHandle _state;
    private bool _disposed;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.BrotliEncoder" /> structure using the specified quality and window.</summary>
    /// <param name="quality">A number representing quality of the Brotli compression. 0 is the minimum (no compression), 11 is the maximum.</param>
    /// <param name="window">A number representing the encoder window bits. The minimum value is 10, and the maximum value is 24.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="quality" /> is not between the minimum value of 0 and the maximum value of 11.
    /// 
    /// -or-
    /// 
    /// <paramref name="window" /> is not between the minimum value of 10 and the maximum value of 24.</exception>
    /// <exception cref="T:System.IO.IOException">Failed to create the <see cref="T:System.IO.Compression.BrotliEncoder" /> instance.</exception>
    public BrotliEncoder(int quality, int window)
    {
      this._disposed = false;
      this._state = Interop.Brotli.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
      if (this._state.IsInvalid)
        throw new IOException(SR.BrotliEncoder_Create);
      this.SetQuality(quality);
      this.SetWindow(window);
    }

    internal void InitializeEncoder()
    {
      this.EnsureNotDisposed();
      this._state = Interop.Brotli.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
      if (this._state.IsInvalid)
        throw new IOException(SR.BrotliEncoder_Create);
    }

    internal void EnsureInitialized()
    {
      this.EnsureNotDisposed();
      if (this._state != null)
        return;
      this.InitializeEncoder();
    }

    /// <summary>Frees and disposes unmanaged resources.</summary>
    public void Dispose()
    {
      this._disposed = true;
      this._state?.Dispose();
    }

    private void EnsureNotDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (BrotliEncoder), SR.BrotliEncoder_Disposed);
    }

    internal void SetQuality(int quality)
    {
      this.EnsureNotDisposed();
      if (this._state == null || this._state.IsInvalid || this._state.IsClosed)
        this.InitializeEncoder();
      if (quality < 0 || quality > 11)
        throw new ArgumentOutOfRangeException(nameof (quality), SR.Format(SR.BrotliEncoder_Quality, (object) quality, (object) 0, (object) 11));
      if (Interop.Brotli.BrotliEncoderSetParameter(this._state, BrotliEncoderParameter.Quality, (uint) quality) == Interop.BOOL.FALSE)
        throw new InvalidOperationException(SR.Format(SR.BrotliEncoder_InvalidSetParameter, (object) "Quality"));
    }

    internal void SetWindow(int window)
    {
      this.EnsureNotDisposed();
      if (this._state == null || this._state.IsInvalid || this._state.IsClosed)
        this.InitializeEncoder();
      if (window < 10 || window > 24)
        throw new ArgumentOutOfRangeException(nameof (window), SR.Format(SR.BrotliEncoder_Window, (object) window, (object) 10, (object) 24));
      if (Interop.Brotli.BrotliEncoderSetParameter(this._state, BrotliEncoderParameter.LGWin, (uint) window) == Interop.BOOL.FALSE)
        throw new InvalidOperationException(SR.Format(SR.BrotliEncoder_InvalidSetParameter, (object) "Window"));
    }

    /// <summary>Gets the maximum expected compressed length for the provided input size.</summary>
    /// <param name="inputSize">The input size to get the maximum expected compressed length from. Must be greater or equal than 0 and less or equal than <see cref="F:System.Int32.MaxValue" /> - 515.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="inputSize" /> is less than 0, the minimum allowed input size, or greater than <see cref="F:System.Int32.MaxValue" /> - 515, the maximum allowed input size.</exception>
    /// <returns>A number representing the maximum compressed length for the provided input size.</returns>
    public static int GetMaxCompressedLength(int inputSize)
    {
      if (inputSize < 0 || inputSize > 2147483132)
        throw new ArgumentOutOfRangeException(nameof (inputSize));
      if (inputSize == 0)
        return 1;
      int num = 2 + 4 * (inputSize >> 24) + ((inputSize & 16777215) > 1048576 ? 4 : 3) + 1;
      return inputSize + num;
    }

    internal OperationStatus Flush(Memory<byte> destination, out int bytesWritten) => this.Flush(destination.Span, out bytesWritten);

    /// <summary>Compresses an empty read-only span of bytes into its destination, which ensures that output is produced for all the processed input. An actual flush is performed when the source is depleted and there is enough space in the destination for the remaining data.</summary>
    /// <param name="destination">When this method returns, a span of bytes where the compressed data will be stored.</param>
    /// <param name="bytesWritten">When this method returns, the total number of bytes that were written to <paramref name="destination" />.</param>
    /// <returns>One of the enumeration values that describes the status with which the operation finished.</returns>
    public OperationStatus Flush(Span<byte> destination, out int bytesWritten) => this.Compress(ReadOnlySpan<byte>.Empty, destination, out int _, out bytesWritten, BrotliEncoderOperation.Flush);

    internal OperationStatus Compress(
      ReadOnlyMemory<byte> source,
      Memory<byte> destination,
      out int bytesConsumed,
      out int bytesWritten,
      bool isFinalBlock)
    {
      return this.Compress(source.Span, destination.Span, out bytesConsumed, out bytesWritten, isFinalBlock);
    }

    /// <summary>Compresses a read-only byte span into a destination span.</summary>
    /// <param name="source">A read-only span of bytes containing the source data to compress.</param>
    /// <param name="destination">When this method returns, a byte span where the compressed is stored.</param>
    /// <param name="bytesConsumed">When this method returns, the total number of bytes that were read from <paramref name="source" />.</param>
    /// <param name="bytesWritten">When this method returns, the total number of bytes that were written to <paramref name="destination" />.</param>
    /// <param name="isFinalBlock">
    /// <see langword="true" /> to finalize the internal stream, which prevents adding more input data when this method returns; <see langword="false" /> to allow the encoder to postpone the production of output until it has processed enough input.</param>
    /// <returns>One of the enumeration values that describes the status with which the span-based operation finished.</returns>
    public OperationStatus Compress(
      ReadOnlySpan<byte> source,
      Span<byte> destination,
      out int bytesConsumed,
      out int bytesWritten,
      bool isFinalBlock)
    {
      return this.Compress(source, destination, out bytesConsumed, out bytesWritten, isFinalBlock ? BrotliEncoderOperation.Finish : BrotliEncoderOperation.Process);
    }

    internal unsafe OperationStatus Compress(
      ReadOnlySpan<byte> source,
      Span<byte> destination,
      out int bytesConsumed,
      out int bytesWritten,
      BrotliEncoderOperation operation)
    {
      this.EnsureInitialized();
      bytesWritten = 0;
      bytesConsumed = 0;
      UIntPtr length1 = (UIntPtr) destination.Length;
      UIntPtr length2 = (UIntPtr) source.Length;
      while ((int) length1 > 0)
      {
        fixed (byte* numPtr1 = &MemoryMarshal.GetReference<byte>(source))
          fixed (byte* numPtr2 = &MemoryMarshal.GetReference<byte>(destination))
          {
            if (Interop.Brotli.BrotliEncoderCompressStream(this._state, operation, ref length2, &numPtr1, ref length1, &numPtr2, out UIntPtr _) == Interop.BOOL.FALSE)
              return OperationStatus.InvalidData;
            bytesConsumed += source.Length - (int) length2;
            bytesWritten += destination.Length - (int) length1;
            if ((int) length1 == destination.Length && Interop.Brotli.BrotliEncoderHasMoreOutput(this._state) == Interop.BOOL.FALSE && (IntPtr) length2 == IntPtr.Zero)
              return OperationStatus.Done;
            source = source.Slice(source.Length - (int) length2);
            destination = destination.Slice(destination.Length - (int) length1);
          }
      }
      return OperationStatus.DestinationTooSmall;
    }

    /// <summary>Tries to compress a source byte span into a destination span.</summary>
    /// <param name="source">A read-only span of bytes containing the source data to compress.</param>
    /// <param name="destination">When this method returns, a span of bytes where the compressed data is stored.</param>
    /// <param name="bytesWritten">When this method returns, the total number of bytes that were written to <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the compression operation was successful; <see langword="false" /> otherwise.</returns>
    public static bool TryCompress(
      ReadOnlySpan<byte> source,
      Span<byte> destination,
      out int bytesWritten)
    {
      return BrotliEncoder.TryCompress(source, destination, out bytesWritten, 11, 22);
    }

    /// <summary>Tries to compress a source byte span into a destination byte span, using the provided compression quality leven and encoder window bits.</summary>
    /// <param name="source">A read-only span of bytes containing the source data to compress.</param>
    /// <param name="destination">When this method returns, a span of bytes where the compressed data is stored.</param>
    /// <param name="bytesWritten">When this method returns, the total number of bytes that were written to <paramref name="destination" />.</param>
    /// <param name="quality">A number representing quality of the Brotli compression. 0 is the minimum (no compression), 11 is the maximum.</param>
    /// <param name="window">A number representing the encoder window bits. The minimum value is 10, and the maximum value is 24.</param>
    /// <returns>
    /// <see langword="true" /> if the compression operation was successful; <see langword="false" /> otherwise.</returns>
    public static unsafe bool TryCompress(
      ReadOnlySpan<byte> source,
      Span<byte> destination,
      out int bytesWritten,
      int quality,
      int window)
    {
      if (quality < 0 || quality > 11)
        throw new ArgumentOutOfRangeException(nameof (quality), SR.Format(SR.BrotliEncoder_Quality, (object) quality, (object) 0, (object) 11));
      if (window < 10 || window > 24)
        throw new ArgumentOutOfRangeException(nameof (window), SR.Format(SR.BrotliEncoder_Window, (object) window, (object) 10, (object) 24));
      fixed (byte* inBytes = &MemoryMarshal.GetReference<byte>(source))
        fixed (byte* outBytes = &MemoryMarshal.GetReference<byte>(destination))
        {
          UIntPtr length = (UIntPtr) destination.Length;
          bool flag = Interop.Brotli.BrotliEncoderCompress(quality, window, 0, (UIntPtr) source.Length, inBytes, &length, outBytes) != 0;
          bytesWritten = (int) length;
          return flag;
        }
    }
  }
}
