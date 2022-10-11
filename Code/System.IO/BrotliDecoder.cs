// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.BrotliDecoder
// Assembly: System.IO.Compression.Brotli, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 531A6BB9-061C-413B-90D3-3B694AB08A91
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.Brotli.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\netstandard.xml

using Microsoft.Win32.SafeHandles;
using System.Buffers;
using System.Runtime.InteropServices;

namespace System.IO.Compression
{
  /// <summary>Provides non-allocating, performant Brotli decompression methods. The methods decompress in a single pass without using a <see cref="T:System.IO.Compression.BrotliStream" /> instance.</summary>
  public struct BrotliDecoder : IDisposable
  {
    private SafeBrotliDecoderHandle _state;
    private bool _disposed;

    internal void InitializeDecoder()
    {
      this._state = Interop.Brotli.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
      if (this._state.IsInvalid)
        throw new IOException(SR.BrotliDecoder_Create);
    }

    internal void EnsureInitialized()
    {
      this.EnsureNotDisposed();
      if (this._state != null)
        return;
      this.InitializeDecoder();
    }

    /// <summary>Releases all resources used by the current Brotli decoder instance.</summary>
    public void Dispose()
    {
      this._disposed = true;
      this._state?.Dispose();
    }

    private void EnsureNotDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (BrotliDecoder), SR.BrotliDecoder_Disposed);
    }

    /// <summary>Decompresses data that was compressed using the Brotli algorithm.</summary>
    /// <param name="source">A buffer containing the compressed data.</param>
    /// <param name="destination">When this method returns, a byte span containing the decompressed data.</param>
    /// <param name="bytesConsumed">The total number of bytes that were read from <paramref name="source" />.</param>
    /// <param name="bytesWritten">The total number of bytes that were written in the <paramref name="destination" />.</param>
    /// <returns>One of the enumeration values that indicates the status of the decompression operation.</returns>
    public unsafe OperationStatus Decompress(
      ReadOnlySpan<byte> source,
      Span<byte> destination,
      out int bytesConsumed,
      out int bytesWritten)
    {
      this.EnsureInitialized();
      bytesConsumed = 0;
      bytesWritten = 0;
      if (Interop.Brotli.BrotliDecoderIsFinished(this._state) != Interop.BOOL.FALSE)
        return OperationStatus.Done;
      UIntPtr length1 = (UIntPtr) destination.Length;
      UIntPtr length2 = (UIntPtr) source.Length;
      while ((int) length1 > 0)
      {
        fixed (byte* numPtr1 = &MemoryMarshal.GetReference<byte>(source))
          fixed (byte* numPtr2 = &MemoryMarshal.GetReference<byte>(destination))
          {
            int num = Interop.Brotli.BrotliDecoderDecompressStream(this._state, ref length2, &numPtr1, ref length1, &numPtr2, out UIntPtr _);
            if (num == 0)
              return OperationStatus.InvalidData;
            bytesConsumed += source.Length - (int) length2;
            bytesWritten += destination.Length - (int) length1;
            switch (num - 1)
            {
              case 0:
                return OperationStatus.Done;
              case 2:
                return OperationStatus.DestinationTooSmall;
              default:
                source = source.Slice(source.Length - (int) length2);
                destination = destination.Slice(destination.Length - (int) length1);
                if (num == 2 && source.Length == 0)
                  return OperationStatus.NeedMoreData;
                // ISSUE: __unpin statement
                __unpin(numPtr2);
                // ISSUE: __unpin statement
                __unpin(numPtr1);
                continue;
            }
          }
      }
      return OperationStatus.DestinationTooSmall;
    }

    /// <summary>Attempts to decompress data that was compressed with the Brotli algorithm.</summary>
    /// <param name="source">A buffer containing the compressed data.</param>
    /// <param name="destination">When this method returns, a byte span containing the decompressed data.</param>
    /// <param name="bytesWritten">The total number of bytes that were written in the <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> on success; <see langword="false" /> otherwise.</returns>
    public static unsafe bool TryDecompress(
      ReadOnlySpan<byte> source,
      Span<byte> destination,
      out int bytesWritten)
    {
      fixed (byte* inBytes = &MemoryMarshal.GetReference<byte>(source))
        fixed (byte* outBytes = &MemoryMarshal.GetReference<byte>(destination))
        {
          UIntPtr length = (UIntPtr) destination.Length;
          bool flag = Interop.Brotli.BrotliDecoderDecompress((UIntPtr) source.Length, inBytes, &length, outBytes) != 0;
          bytesWritten = (int) length;
          return flag;
        }
    }
  }
}
