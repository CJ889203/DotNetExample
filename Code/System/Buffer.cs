// Decompiled with JetBrains decompiler
// Type: System.Buffer
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Manipulates arrays of primitive types.</summary>
  public static class Buffer
  {

    #nullable disable
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static unsafe void _ZeroMemory(ref byte b, [NativeInteger] UIntPtr byteLength)
    {
      fixed (byte* b1 = &b)
        Buffer.__ZeroMemory((void*) b1, byteLength);
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern unsafe void __ZeroMemory(void* b, [NativeInteger] UIntPtr byteLength);

    internal static void BulkMoveWithWriteBarrier(
      ref byte destination,
      ref byte source,
      [NativeInteger] UIntPtr byteCount)
    {
      if (byteCount <= new UIntPtr(16384))
        Buffer.__BulkMoveWithWriteBarrier(ref destination, ref source, byteCount);
      else
        Buffer._BulkMoveWithWriteBarrier(ref destination, ref source, byteCount);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void _BulkMoveWithWriteBarrier(
      ref byte destination,
      ref byte source,
      [NativeInteger] UIntPtr byteCount)
    {
      if (Unsafe.AreSame<byte>(ref source, ref destination))
        return;
      if ((UIntPtr) Unsafe.ByteOffset<byte>(ref source, ref destination) >= byteCount)
      {
        do
        {
          byteCount -= new UIntPtr(16384);
          Buffer.__BulkMoveWithWriteBarrier(ref destination, ref source, new UIntPtr(16384));
          ref destination = ref Unsafe.AddByteOffset<byte>(ref destination, new UIntPtr(16384));
          ref source = ref Unsafe.AddByteOffset<byte>(ref source, new UIntPtr(16384));
        }
        while (byteCount > new UIntPtr(16384));
      }
      else
      {
        do
        {
          byteCount -= new UIntPtr(16384);
          Buffer.__BulkMoveWithWriteBarrier(ref Unsafe.AddByteOffset<byte>(ref destination, byteCount), ref Unsafe.AddByteOffset<byte>(ref source, byteCount), new UIntPtr(16384));
        }
        while (byteCount > new UIntPtr(16384));
      }
      Buffer.__BulkMoveWithWriteBarrier(ref destination, ref source, byteCount);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void __BulkMoveWithWriteBarrier(
      ref byte destination,
      ref byte source,
      [NativeInteger] UIntPtr byteCount);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern unsafe void __Memmove(byte* dest, byte* src, [NativeInteger] UIntPtr len);

    internal static unsafe void Memcpy(byte* dest, byte* src, int len) => Buffer.Memmove(ref *dest, ref *src, (UIntPtr) (uint) len);

    internal static unsafe void Memcpy(
      byte* pDest,
      int destIndex,
      byte[] src,
      int srcIndex,
      int len)
    {
      Buffer.Memmove(ref pDest[(uint) destIndex], ref Unsafe.Add<byte>(ref MemoryMarshal.GetArrayDataReference<byte>(src), (IntPtr) (uint) srcIndex), (UIntPtr) (uint) len);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Memmove<T>(ref T destination, ref T source, [NativeInteger] UIntPtr elementCount)
    {
      if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        Buffer.Memmove(ref Unsafe.As<T, byte>(ref destination), ref Unsafe.As<T, byte>(ref source), elementCount * (UIntPtr) Unsafe.SizeOf<T>());
      else
        Buffer.BulkMoveWithWriteBarrier(ref Unsafe.As<T, byte>(ref destination), ref Unsafe.As<T, byte>(ref source), elementCount * (UIntPtr) Unsafe.SizeOf<T>());
    }


    #nullable enable
    /// <summary>Copies a specified number of bytes from a source array starting at a particular offset to a destination array starting at a particular offset.</summary>
    /// <param name="src">The source buffer.</param>
    /// <param name="srcOffset">The zero-based byte offset into <paramref name="src" />.</param>
    /// <param name="dst">The destination buffer.</param>
    /// <param name="dstOffset">The zero-based byte offset into <paramref name="dst" />.</param>
    /// <param name="count">The number of bytes to copy.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="src" /> or <paramref name="dst" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="src" /> or <paramref name="dst" /> is not an array of primitives.
    /// 
    /// -or-
    /// 
    /// The number of bytes in <paramref name="src" /> is less than <paramref name="srcOffset" /> plus <paramref name="count" />.
    /// 
    /// -or-
    /// 
    /// The number of bytes in <paramref name="dst" /> is less than <paramref name="dstOffset" /> plus <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="srcOffset" />, <paramref name="dstOffset" />, or <paramref name="count" /> is less than 0.</exception>
    public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
    {
      if (src == null)
        throw new ArgumentNullException(nameof (src));
      if (dst == null)
        throw new ArgumentNullException(nameof (dst));
      UIntPtr nativeLength = src.NativeLength;
      if (src.GetType() != typeof (byte[]))
      {
        if (!src.GetCorElementTypeOfElementType().IsPrimitiveType())
          throw new ArgumentException(SR.Arg_MustBePrimArray, nameof (src));
        nativeLength *= (UIntPtr) src.GetElementSize();
      }
      UIntPtr num = nativeLength;
      if (src != dst)
      {
        num = dst.NativeLength;
        if (dst.GetType() != typeof (byte[]))
        {
          if (!dst.GetCorElementTypeOfElementType().IsPrimitiveType())
            throw new ArgumentException(SR.Arg_MustBePrimArray, nameof (dst));
          num *= (UIntPtr) dst.GetElementSize();
        }
      }
      if (srcOffset < 0)
        throw new ArgumentOutOfRangeException(nameof (srcOffset), SR.ArgumentOutOfRange_MustBeNonNegInt32);
      if (dstOffset < 0)
        throw new ArgumentOutOfRangeException(nameof (dstOffset), SR.ArgumentOutOfRange_MustBeNonNegInt32);
      UIntPtr len = count >= 0 ? (UIntPtr) count : throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_MustBeNonNegInt32);
      UIntPtr byteOffset1 = (UIntPtr) srcOffset;
      UIntPtr byteOffset2 = (UIntPtr) dstOffset;
      if (nativeLength < byteOffset1 + len || num < byteOffset2 + len)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      Buffer.Memmove(ref Unsafe.AddByteOffset<byte>(ref MemoryMarshal.GetArrayDataReference(dst), byteOffset2), ref Unsafe.AddByteOffset<byte>(ref MemoryMarshal.GetArrayDataReference(src), byteOffset1), len);
    }

    /// <summary>Returns the number of bytes in the specified array.</summary>
    /// <param name="array">An array.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="array" /> is not a primitive.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
    /// <returns>The number of bytes in the array.</returns>
    public static int ByteLength(Array array)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (!array.GetCorElementTypeOfElementType().IsPrimitiveType())
        throw new ArgumentException(SR.Arg_MustBePrimArray, nameof (array));
      return checked ((int) unchecked (array.NativeLength * (UIntPtr) array.GetElementSize()));
    }

    /// <summary>Retrieves the byte at the specified location in the specified array.</summary>
    /// <param name="array">An array.</param>
    /// <param name="index">A location in the array.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="array" /> is not a primitive.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is negative or greater than the length of <paramref name="array" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
    /// <returns>The byte at the specified location in the array.</returns>
    public static byte GetByte(Array array, int index)
    {
      if ((uint) index >= (uint) Buffer.ByteLength(array))
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return Unsafe.Add<byte>(ref MemoryMarshal.GetArrayDataReference(array), index);
    }

    /// <summary>Assigns a specified value to a byte at a particular location in a specified array.</summary>
    /// <param name="array">An array.</param>
    /// <param name="index">A location in the array.</param>
    /// <param name="value">A value to assign.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="array" /> is not a primitive.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is negative or greater than the length of <paramref name="array" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
    public static void SetByte(Array array, int index, byte value)
    {
      if ((uint) index >= (uint) Buffer.ByteLength(array))
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      Unsafe.Add<byte>(ref MemoryMarshal.GetArrayDataReference(array), index) = value;
    }


    #nullable disable
    internal static unsafe void ZeroMemory(byte* dest, [NativeInteger] UIntPtr len) => SpanHelpers.ClearWithoutReferences(ref *dest, len);


    #nullable enable
    /// <summary>Copies a number of bytes specified as a long integer value from one address in memory to another.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="source">The address of the bytes to copy.</param>
    /// <param name="destination">The target address.</param>
    /// <param name="destinationSizeInBytes">The number of bytes available in the destination memory block.</param>
    /// <param name="sourceBytesToCopy">The number of bytes to copy.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="sourceBytesToCopy" /> is greater than <paramref name="destinationSizeInBytes" />.</exception>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void MemoryCopy(
      void* source,
      void* destination,
      long destinationSizeInBytes,
      long sourceBytesToCopy)
    {
      if (sourceBytesToCopy > destinationSizeInBytes)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sourceBytesToCopy);
      Buffer.Memmove(ref *(byte*) destination, ref *(byte*) source, checked ((UIntPtr) (ulong) sourceBytesToCopy));
    }

    /// <summary>Copies a number of bytes specified as an unsigned long integer value from one address in memory to another.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="source">The address of the bytes to copy.</param>
    /// <param name="destination">The target address.</param>
    /// <param name="destinationSizeInBytes">The number of bytes available in the destination memory block.</param>
    /// <param name="sourceBytesToCopy">The number of bytes to copy.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="sourceBytesToCopy" /> is greater than <paramref name="destinationSizeInBytes" />.</exception>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void MemoryCopy(
      void* source,
      void* destination,
      ulong destinationSizeInBytes,
      ulong sourceBytesToCopy)
    {
      if (sourceBytesToCopy > destinationSizeInBytes)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sourceBytesToCopy);
      Buffer.Memmove(ref *(byte*) destination, ref *(byte*) source, checked ((UIntPtr) sourceBytesToCopy));
    }


    #nullable disable
    internal static void Memmove(ref byte dest, ref byte src, [NativeInteger] UIntPtr len)
    {
      if ((UIntPtr) Unsafe.ByteOffset<byte>(ref src, ref dest) >= len && (UIntPtr) Unsafe.ByteOffset<byte>(ref dest, ref src) >= len)
      {
        ref byte local1 = ref Unsafe.Add<byte>(ref src, (IntPtr) len);
        ref byte local2 = ref Unsafe.Add<byte>(ref dest, (IntPtr) len);
        if (len > new UIntPtr(16))
        {
          if (len > new UIntPtr(64))
            goto label_16;
label_3:
          Unsafe.As<byte, Buffer.Block16>(ref dest) = Unsafe.As<byte, Buffer.Block16>(ref src);
          if (len > new UIntPtr(32))
          {
            Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref dest, 16)) = Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref src, 16));
            if (len > new UIntPtr(48))
              Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref dest, 32)) = Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref src, 32));
          }
          Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref local2, -16)) = Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref local1, -16));
          return;
label_16:
          if (len <= new UIntPtr(2048))
          {
            UIntPtr num = len >> 6;
            do
            {
              Unsafe.As<byte, Buffer.Block64>(ref dest) = Unsafe.As<byte, Buffer.Block64>(ref src);
              ref dest = ref Unsafe.Add<byte>(ref dest, 64);
              ref src = ref Unsafe.Add<byte>(ref src, 64);
              --num;
            }
            while ((IntPtr) num != IntPtr.Zero);
            len %= new UIntPtr(64);
            if (len <= new UIntPtr(16))
            {
              Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref local2, -16)) = Unsafe.As<byte, Buffer.Block16>(ref Unsafe.Add<byte>(ref local1, -16));
              return;
            }
            goto label_3;
          }
        }
        else
        {
          if (((IntPtr) len & new IntPtr(24)) != IntPtr.Zero)
          {
            Unsafe.As<byte, long>(ref dest) = Unsafe.As<byte, long>(ref src);
            Unsafe.As<byte, long>(ref Unsafe.Add<byte>(ref local2, -8)) = Unsafe.As<byte, long>(ref Unsafe.Add<byte>(ref local1, -8));
            return;
          }
          if (((IntPtr) len & new IntPtr(4)) != IntPtr.Zero)
          {
            Unsafe.As<byte, int>(ref dest) = Unsafe.As<byte, int>(ref src);
            Unsafe.As<byte, int>(ref Unsafe.Add<byte>(ref local2, -4)) = Unsafe.As<byte, int>(ref Unsafe.Add<byte>(ref local1, -4));
            return;
          }
          if ((IntPtr) len == IntPtr.Zero)
            return;
          dest = src;
          if (((IntPtr) len & new IntPtr(2)) == IntPtr.Zero)
            return;
          Unsafe.As<byte, short>(ref Unsafe.Add<byte>(ref local2, -2)) = Unsafe.As<byte, short>(ref Unsafe.Add<byte>(ref local1, -2));
          return;
        }
      }
      else if (Unsafe.AreSame<byte>(ref dest, ref src))
        return;
      Buffer._Memmove(ref dest, ref src, len);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static unsafe void _Memmove(ref byte dest, ref byte src, [NativeInteger] UIntPtr len)
    {
      fixed (byte* dest1 = &dest)
        fixed (byte* src1 = &src)
          Buffer.__Memmove(dest1, src1, len);
    }

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    private struct Block16
    {
    }

    [StructLayout(LayoutKind.Sequential, Size = 64)]
    private struct Block64
    {
    }
  }
}
