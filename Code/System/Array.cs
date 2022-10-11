// Decompiled with JetBrains decompiler
// Type: System.Array
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Provides methods for creating, manipulating, searching, and sorting arrays, thereby serving as the base class for all arrays in the common language runtime.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public abstract class Array : 
    ICloneable,
    IList,
    ICollection,
    IEnumerable,
    IStructuralComparable,
    IStructuralEquatable
  {
    /// <summary>Creates a one-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and length, with zero-based indexing.</summary>
    /// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="length">The size of the <see cref="T:System.Array" /> to create.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="elementType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="elementType" /> is an open generic type.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="length" /> is less than zero.</exception>
    /// <returns>A new one-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length, using zero-based indexing.</returns>
    public static unsafe Array CreateInstance(Type elementType, int length)
    {
      if ((object) elementType == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.elementType);
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      RuntimeType underlyingSystemType = elementType.UnderlyingSystemType as RuntimeType;
      if ((Type) underlyingSystemType == (Type) null)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_MustBeType, ExceptionArgument.elementType);
      return Array.InternalCreate((void*) underlyingSystemType.TypeHandle.Value, 1, &length, (int*) null);
    }

    /// <summary>Creates a two-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing.</summary>
    /// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="length1">The size of the first dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="length2">The size of the second dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="elementType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="elementType" /> is an open generic type.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="length1" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="length2" /> is less than zero.</exception>
    /// <returns>A new two-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
    public static unsafe Array CreateInstance(Type elementType, int length1, int length2)
    {
      if ((object) elementType == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.elementType);
      if (length1 < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length1, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (length2 < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length2, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      RuntimeType underlyingSystemType = elementType.UnderlyingSystemType as RuntimeType;
      if ((Type) underlyingSystemType == (Type) null)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_MustBeType, ExceptionArgument.elementType);
      int* pLengths = stackalloc int[2];
      pLengths[0] = length1;
      pLengths[1] = length2;
      return Array.InternalCreate((void*) underlyingSystemType.TypeHandle.Value, 2, pLengths, (int*) null);
    }

    /// <summary>Creates a three-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing.</summary>
    /// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="length1">The size of the first dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="length2">The size of the second dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="length3">The size of the third dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="elementType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="elementType" /> is an open generic type.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="length1" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="length2" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="length3" /> is less than zero.</exception>
    /// <returns>A new three-dimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
    public static unsafe Array CreateInstance(
      Type elementType,
      int length1,
      int length2,
      int length3)
    {
      if ((object) elementType == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.elementType);
      if (length1 < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length1, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (length2 < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length2, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (length3 < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length3, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      RuntimeType underlyingSystemType = elementType.UnderlyingSystemType as RuntimeType;
      if ((Type) underlyingSystemType == (Type) null)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_MustBeType, ExceptionArgument.elementType);
      int* pLengths = stackalloc int[3];
      pLengths[0] = length1;
      pLengths[1] = length2;
      pLengths[2] = length3;
      return Array.InternalCreate((void*) underlyingSystemType.TypeHandle.Value, 3, pLengths, (int*) null);
    }

    /// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing. The dimension lengths are specified in an array of 32-bit integers.</summary>
    /// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="lengths">An array of 32-bit integers that represent the size of each dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="elementType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="lengths" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="lengths" /> array contains less than one element.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="elementType" /> is an open generic type.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero.</exception>
    /// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
    public static unsafe Array CreateInstance(Type elementType, params int[] lengths)
    {
      if ((object) elementType == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.elementType);
      if (lengths == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.lengths);
      if (lengths.Length == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NeedAtLeast1Rank);
      RuntimeType underlyingSystemType = elementType.UnderlyingSystemType as RuntimeType;
      if ((Type) underlyingSystemType == (Type) null)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_MustBeType, ExceptionArgument.elementType);
      for (int paramNumber = 0; paramNumber < lengths.Length; ++paramNumber)
      {
        if (lengths[paramNumber] < 0)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.lengths, paramNumber, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      }
      fixed (int* pLengths = &lengths[0])
        return Array.InternalCreate((void*) underlyingSystemType.TypeHandle.Value, lengths.Length, pLengths, (int*) null);
    }

    /// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with the specified lower bounds.</summary>
    /// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="lengths">A one-dimensional array that contains the size of each dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="lowerBounds">A one-dimensional array that contains the lower bound (starting index) of each dimension of the <see cref="T:System.Array" /> to create.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="elementType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="lengths" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="lowerBounds" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="lengths" /> array contains less than one element.
    /// 
    /// -or-
    /// 
    /// The <paramref name="lengths" /> and <paramref name="lowerBounds" /> arrays do not contain the same number of elements.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="elementType" /> is an open generic type.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// Any value in <paramref name="lowerBounds" /> is very large, such that the sum of a dimension's lower bound and length is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length and lower bound for each dimension.</returns>
    public static unsafe Array CreateInstance(
      Type elementType,
      int[] lengths,
      int[] lowerBounds)
    {
      if (elementType == (Type) null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.elementType);
      if (lengths == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.lengths);
      if (lowerBounds == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.lowerBounds);
      if (lengths.Length != lowerBounds.Length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RanksAndBounds);
      if (lengths.Length == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NeedAtLeast1Rank);
      RuntimeType underlyingSystemType = elementType.UnderlyingSystemType as RuntimeType;
      if ((Type) underlyingSystemType == (Type) null)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_MustBeType, ExceptionArgument.elementType);
      for (int paramNumber = 0; paramNumber < lengths.Length; ++paramNumber)
      {
        if (lengths[paramNumber] < 0)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.lengths, paramNumber, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      }
      fixed (int* pLengths = &lengths[0])
        fixed (int* pLowerBounds = &lowerBounds[0])
          return Array.InternalCreate((void*) underlyingSystemType.TypeHandle.Value, lengths.Length, pLengths, pLowerBounds);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern unsafe Array InternalCreate(
      void* elementType,
      int rank,
      int* pLengths,
      int* pLowerBounds);


    #nullable enable
    /// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the first element and pastes them into another <see cref="T:System.Array" /> starting at the first element. The length is specified as a 32-bit integer.</summary>
    /// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
    /// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
    /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="sourceArray" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
    /// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="length" /> is greater than the number of elements in <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is greater than the number of elements in <paramref name="destinationArray" />.</exception>
    public static unsafe void Copy(Array sourceArray, Array destinationArray, int length)
    {
      if (sourceArray == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.sourceArray);
      if (destinationArray == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.destinationArray);
      MethodTable* methodTable = RuntimeHelpers.GetMethodTable((object) sourceArray);
      if (methodTable == RuntimeHelpers.GetMethodTable((object) destinationArray) && !methodTable->IsMultiDimensionalArray && (UIntPtr) (uint) length <= sourceArray.NativeLength && (UIntPtr) (uint) length <= destinationArray.NativeLength)
      {
        UIntPtr num = (UIntPtr) (uint) length * (UIntPtr) methodTable->ComponentSize;
        ref byte local1 = ref Unsafe.As<RawArrayData>((object) sourceArray).Data;
        ref byte local2 = ref Unsafe.As<RawArrayData>((object) destinationArray).Data;
        if (methodTable->ContainsGCPointers)
          Buffer.BulkMoveWithWriteBarrier(ref local2, ref local1, num);
        else
          Buffer.Memmove(ref local2, ref local1, num);
      }
      else
        Array.Copy(sourceArray, sourceArray.GetLowerBound(0), destinationArray, destinationArray.GetLowerBound(0), length, false);
    }

    /// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index. The length and the indexes are specified as 32-bit integers.</summary>
    /// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
    /// <param name="sourceIndex">A 32-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
    /// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
    /// <param name="destinationIndex">A 32-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
    /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="sourceArray" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
    /// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="sourceIndex" /> is less than the lower bound of the first dimension of <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationIndex" /> is less than the lower bound of the first dimension of <paramref name="destinationArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
    public static unsafe void Copy(
      Array sourceArray,
      int sourceIndex,
      Array destinationArray,
      int destinationIndex,
      int length)
    {
      if (sourceArray != null && destinationArray != null)
      {
        MethodTable* methodTable = RuntimeHelpers.GetMethodTable((object) sourceArray);
        if (methodTable == RuntimeHelpers.GetMethodTable((object) destinationArray) && !methodTable->IsMultiDimensionalArray && length >= 0 && sourceIndex >= 0 && destinationIndex >= 0 && (UIntPtr) (uint) (sourceIndex + length) <= sourceArray.NativeLength && (UIntPtr) (uint) (destinationIndex + length) <= destinationArray.NativeLength)
        {
          UIntPtr componentSize = (UIntPtr) methodTable->ComponentSize;
          UIntPtr num = (UIntPtr) (uint) length * componentSize;
          ref byte local1 = ref Unsafe.AddByteOffset<byte>(ref Unsafe.As<RawArrayData>((object) sourceArray).Data, (UIntPtr) (uint) sourceIndex * componentSize);
          ref byte local2 = ref Unsafe.AddByteOffset<byte>(ref Unsafe.As<RawArrayData>((object) destinationArray).Data, (UIntPtr) (uint) destinationIndex * componentSize);
          if (methodTable->ContainsGCPointers)
          {
            Buffer.BulkMoveWithWriteBarrier(ref local2, ref local1, num);
            return;
          }
          Buffer.Memmove(ref local2, ref local1, num);
          return;
        }
      }
      Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length, false);
    }


    #nullable disable
    private static unsafe void Copy(
      Array sourceArray,
      int sourceIndex,
      Array destinationArray,
      int destinationIndex,
      int length,
      bool reliable)
    {
      if (sourceArray == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.sourceArray);
      if (destinationArray == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.destinationArray);
      if (sourceArray.GetType() != destinationArray.GetType() && sourceArray.Rank != destinationArray.Rank)
        throw new RankException(SR.Rank_MustMatch);
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length), SR.ArgumentOutOfRange_NeedNonNegNum);
      int lowerBound1 = sourceArray.GetLowerBound(0);
      if (sourceIndex < lowerBound1 || sourceIndex - lowerBound1 < 0)
        throw new ArgumentOutOfRangeException(nameof (sourceIndex), SR.ArgumentOutOfRange_ArrayLB);
      sourceIndex -= lowerBound1;
      int lowerBound2 = destinationArray.GetLowerBound(0);
      if (destinationIndex < lowerBound2 || destinationIndex - lowerBound2 < 0)
        throw new ArgumentOutOfRangeException(nameof (destinationIndex), SR.ArgumentOutOfRange_ArrayLB);
      destinationIndex -= lowerBound2;
      if ((UIntPtr) (uint) (sourceIndex + length) > sourceArray.NativeLength)
        throw new ArgumentException(SR.Arg_LongerThanSrcArray, nameof (sourceArray));
      if ((UIntPtr) (uint) (destinationIndex + length) > destinationArray.NativeLength)
        throw new ArgumentException(SR.Arg_LongerThanDestArray, nameof (destinationArray));
      if (sourceArray.GetType() == destinationArray.GetType() || Array.IsSimpleCopy(sourceArray, destinationArray))
      {
        MethodTable* methodTable = RuntimeHelpers.GetMethodTable((object) sourceArray);
        UIntPtr componentSize = (UIntPtr) methodTable->ComponentSize;
        UIntPtr num = (UIntPtr) (uint) length * componentSize;
        ref byte local1 = ref Unsafe.AddByteOffset<byte>(ref MemoryMarshal.GetArrayDataReference(sourceArray), (UIntPtr) (uint) sourceIndex * componentSize);
        ref byte local2 = ref Unsafe.AddByteOffset<byte>(ref MemoryMarshal.GetArrayDataReference(destinationArray), (UIntPtr) (uint) destinationIndex * componentSize);
        if (methodTable->ContainsGCPointers)
          Buffer.BulkMoveWithWriteBarrier(ref local2, ref local1, num);
        else
          Buffer.Memmove(ref local2, ref local1, num);
      }
      else
      {
        if (reliable)
          throw new ArrayTypeMismatchException(SR.ArrayTypeMismatch_ConstrainedCopy);
        Array.CopySlow(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool IsSimpleCopy(Array sourceArray, Array destinationArray);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void CopySlow(
      Array sourceArray,
      int sourceIndex,
      Array destinationArray,
      int destinationIndex,
      int length);


    #nullable enable
    /// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index.  Guarantees that all changes are undone if the copy does not succeed completely.</summary>
    /// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
    /// <param name="sourceIndex">A 32-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
    /// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
    /// <param name="destinationIndex">A 32-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
    /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="sourceArray" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">The <paramref name="sourceArray" /> type is neither the same as nor derived from the <paramref name="destinationArray" /> type.</exception>
    /// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="sourceIndex" /> is less than the lower bound of the first dimension of <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationIndex" /> is less than the lower bound of the first dimension of <paramref name="destinationArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
    public static void ConstrainedCopy(
      Array sourceArray,
      int sourceIndex,
      Array destinationArray,
      int destinationIndex,
      int length)
    {
      Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length, true);
    }

    /// <summary>Clears the contents of an array.</summary>
    /// <param name="array">The array to clear.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    public static unsafe void Clear(Array array)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      MethodTable* methodTable = RuntimeHelpers.GetMethodTable((object) array);
      UIntPtr byteLength = (UIntPtr) methodTable->ComponentSize * array.NativeLength;
      ref byte local = ref MemoryMarshal.GetArrayDataReference(array);
      if (!methodTable->ContainsGCPointers)
        SpanHelpers.ClearWithoutReferences(ref local, byteLength);
      else
        SpanHelpers.ClearWithReferences(ref Unsafe.As<byte, IntPtr>(ref local), byteLength / (UIntPtr) sizeof (IntPtr));
    }

    /// <summary>Sets a range of elements in an array to the default value of each element type.</summary>
    /// <param name="array">The array whose elements need to be cleared.</param>
    /// <param name="index">The starting index of the range of elements to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// The sum of <paramref name="index" /> and <paramref name="length" /> is greater than the size of <paramref name="array" />.</exception>
    public static unsafe void Clear(Array array, int index, int length)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      ref byte local1 = ref Unsafe.As<RawArrayData>((object) array).Data;
      int num1 = 0;
      MethodTable* methodTable = RuntimeHelpers.GetMethodTable((object) array);
      if (methodTable->IsMultiDimensionalArray)
      {
        int dimensionalArrayRank = methodTable->MultiDimensionalArrayRank;
        num1 = Unsafe.Add<int>(ref Unsafe.As<byte, int>(ref local1), dimensionalArrayRank);
        local1 = ref Unsafe.Add<byte>(ref local1, 8 * dimensionalArrayRank);
      }
      int num2 = index - num1;
      if (index < num1 || num2 < 0 || length < 0 || (UIntPtr) (uint) (num2 + length) > array.NativeLength)
        ThrowHelper.ThrowIndexOutOfRangeException();
      UIntPtr componentSize = (UIntPtr) methodTable->ComponentSize;
      ref byte local2 = ref Unsafe.AddByteOffset<byte>(ref local1, (UIntPtr) (uint) num2 * componentSize);
      UIntPtr byteLength = (UIntPtr) (uint) length * componentSize;
      if (methodTable->ContainsGCPointers)
        SpanHelpers.ClearWithReferences(ref Unsafe.As<byte, IntPtr>(ref local2), byteLength / (UIntPtr) (uint) sizeof (IntPtr));
      else
        SpanHelpers.ClearWithoutReferences(ref local2, byteLength);
    }


    #nullable disable
    [return: NativeInteger]
    private unsafe IntPtr GetFlattenedIndex(ReadOnlySpan<int> indices)
    {
      if (RuntimeHelpers.GetMethodTable((object) this)->IsMultiDimensionalArray)
      {
        ref int local = ref RuntimeHelpers.GetMultiDimensionalArrayBounds(this);
        IntPtr flattenedIndex = IntPtr.Zero;
        for (int index = 0; index < indices.Length; ++index)
        {
          int num1 = indices[index] - Unsafe.Add<int>(ref local, indices.Length + index);
          int num2 = Unsafe.Add<int>(ref local, index);
          if ((uint) num1 >= (uint) num2)
            ThrowHelper.ThrowIndexOutOfRangeException();
          flattenedIndex = (IntPtr) num2 * flattenedIndex + (IntPtr) num1;
        }
        return flattenedIndex;
      }
      int flattenedIndex1 = indices[0];
      if ((uint) flattenedIndex1 >= (uint) this.LongLength)
        ThrowHelper.ThrowIndexOutOfRangeException();
      return (IntPtr) flattenedIndex1;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern object InternalGetValue([NativeInteger] IntPtr flattenedIndex);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalSetValue(object value, [NativeInteger] IntPtr flattenedIndex);

    /// <summary>Gets the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</summary>
    /// <exception cref="T:System.OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
    /// <returns>The total number of elements in all the dimensions of the <see cref="T:System.Array" />; zero if there are no elements in the array.</returns>
    public int Length => checked ((int) Unsafe.As<RawArrayData>((object) this).Length);

    [NativeInteger]
    internal UIntPtr NativeLength
    {
      [return: NativeInteger] get => (UIntPtr) Unsafe.As<RawArrayData>((object) this).Length;
    }

    /// <summary>Gets a 64-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</summary>
    /// <returns>A 64-bit integer that represents the total number of elements in all the dimensions of the <see cref="T:System.Array" />.</returns>
    public long LongLength => (long) (ulong) this.NativeLength;

    /// <summary>Gets the rank (number of dimensions) of the <see cref="T:System.Array" />. For example, a one-dimensional array returns 1, a two-dimensional array returns 2, and so on.</summary>
    /// <returns>The rank (number of dimensions) of the <see cref="T:System.Array" />.</returns>
    public int Rank
    {
      get
      {
        int dimensionalArrayRank = RuntimeHelpers.GetMultiDimensionalArrayRank(this);
        return dimensionalArrayRank == 0 ? 1 : dimensionalArrayRank;
      }
    }

    /// <summary>Gets a 32-bit integer that represents the number of elements in the specified dimension of the <see cref="T:System.Array" />.</summary>
    /// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose length needs to be determined.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///        <paramref name="dimension" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
    /// <returns>A 32-bit integer that represents the number of elements in the specified dimension.</returns>
    public int GetLength(int dimension)
    {
      int dimensionalArrayRank = RuntimeHelpers.GetMultiDimensionalArrayRank(this);
      if (dimensionalArrayRank == 0 && dimension == 0)
        return this.Length;
      if ((uint) dimension >= (uint) dimensionalArrayRank)
        throw new IndexOutOfRangeException(SR.IndexOutOfRange_ArrayRankIndex);
      return Unsafe.Add<int>(ref RuntimeHelpers.GetMultiDimensionalArrayBounds(this), dimension);
    }

    /// <summary>Gets the index of the last element of the specified dimension in the array.</summary>
    /// <param name="dimension">A zero-based dimension of the array whose upper bound needs to be determined.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///        <paramref name="dimension" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
    /// <returns>The index of the last element of the specified dimension in the array, or -1 if the specified dimension is empty.</returns>
    public int GetUpperBound(int dimension)
    {
      int dimensionalArrayRank = RuntimeHelpers.GetMultiDimensionalArrayRank(this);
      if (dimensionalArrayRank == 0 && dimension == 0)
        return this.Length - 1;
      if ((uint) dimension >= (uint) dimensionalArrayRank)
        throw new IndexOutOfRangeException(SR.IndexOutOfRange_ArrayRankIndex);
      ref int local = ref RuntimeHelpers.GetMultiDimensionalArrayBounds(this);
      return Unsafe.Add<int>(ref local, dimension) + Unsafe.Add<int>(ref local, dimensionalArrayRank + dimension) - 1;
    }

    /// <summary>Gets the index of the first element of the specified dimension in the array.</summary>
    /// <param name="dimension">A zero-based dimension of the array whose starting index needs to be determined.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///        <paramref name="dimension" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
    /// <returns>The index of the first element of the specified dimension in the array.</returns>
    public int GetLowerBound(int dimension)
    {
      int dimensionalArrayRank = RuntimeHelpers.GetMultiDimensionalArrayRank(this);
      if (dimensionalArrayRank == 0 && dimension == 0)
        return 0;
      if ((uint) dimension >= (uint) dimensionalArrayRank)
        throw new IndexOutOfRangeException(SR.IndexOutOfRange_ArrayRankIndex);
      return Unsafe.Add<int>(ref RuntimeHelpers.GetMultiDimensionalArrayBounds(this), dimensionalArrayRank + dimension);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern CorElementType GetCorElementTypeOfElementType();

    private unsafe bool IsValueOfElementType(object value) => (IntPtr) RuntimeHelpers.GetMethodTable((object) this)->ElementType == (IntPtr) (void*) RuntimeHelpers.GetMethodTable(value);

    /// <summary>Initializes every element of the value-type <see cref="T:System.Array" /> by calling the parameterless constructor of the value type.</summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Initialize();

    private protected Array()
    {
    }


    #nullable enable
    /// <summary>Returns a read-only wrapper for the specified array.</summary>
    /// <param name="array">The one-dimensional, zero-based array to wrap in a read-only <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wrapper.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <returns>A read-only <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wrapper for the specified array.</returns>
    public static ReadOnlyCollection<T> AsReadOnly<T>(T[] array)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return new ReadOnlyCollection<T>((IList<T>) array);
    }

    /// <summary>Changes the number of elements of a one-dimensional array to the specified new size.</summary>
    /// <param name="array">The one-dimensional, zero-based array to resize, or <see langword="null" /> to create a new array with the specified size.</param>
    /// <param name="newSize">The size of the new array.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="newSize" /> is less than zero.</exception>
    public static void Resize<T>([NotNull] ref T[] array, int newSize)
    {
      if (newSize < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.newSize, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      T[] array1 = array;
      if (array1 == null)
      {
        array = new T[newSize];
      }
      else
      {
        if (array1.Length == newSize)
          return;
        T[] array2 = new T[newSize];
        Buffer.Memmove<T>(ref MemoryMarshal.GetArrayDataReference<T>(array2), ref MemoryMarshal.GetArrayDataReference<T>(array1), (UIntPtr) (uint) Math.Min(newSize, array1.Length));
        array = array2;
      }
    }

    /// <summary>Creates a multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> and dimension lengths, with zero-based indexing. The dimension lengths are specified in an array of 64-bit integers.</summary>
    /// <param name="elementType">The <see cref="T:System.Type" /> of the <see cref="T:System.Array" /> to create.</param>
    /// <param name="lengths">An array of 64-bit integers that represent the size of each dimension of the <see cref="T:System.Array" /> to create. Each integer in the array must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="elementType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="lengths" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="elementType" /> is not a valid <see cref="T:System.Type" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="lengths" /> array contains less than one element.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="elementType" /> is not supported. For example, <see cref="T:System.Void" /> is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="elementType" /> is an open generic type.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Any value in <paramref name="lengths" /> is less than zero or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A new multidimensional <see cref="T:System.Array" /> of the specified <see cref="T:System.Type" /> with the specified length for each dimension, using zero-based indexing.</returns>
    public static Array CreateInstance(Type elementType, params long[] lengths)
    {
      if (lengths == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.lengths);
      if (lengths.Length == 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NeedAtLeast1Rank);
      int[] numArray = new int[lengths.Length];
      for (int index = 0; index < lengths.Length; ++index)
      {
        long length = lengths[index];
        int num = (int) length;
        if (length != (long) num)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.len, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
        numArray[index] = num;
      }
      return Array.CreateInstance(elementType, numArray);
    }

    /// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the first element and pastes them into another <see cref="T:System.Array" /> starting at the first element. The length is specified as a 64-bit integer.</summary>
    /// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
    /// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
    /// <param name="length">A 64-bit integer that represents the number of elements to copy. The integer must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="sourceArray" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
    /// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="length" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="length" /> is greater than the number of elements in <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is greater than the number of elements in <paramref name="destinationArray" />.</exception>
    public static void Copy(Array sourceArray, Array destinationArray, long length)
    {
      int length1 = (int) length;
      if (length != (long) length1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      Array.Copy(sourceArray, destinationArray, length1);
    }

    /// <summary>Copies a range of elements from an <see cref="T:System.Array" /> starting at the specified source index and pastes them to another <see cref="T:System.Array" /> starting at the specified destination index. The length and the indexes are specified as 64-bit integers.</summary>
    /// <param name="sourceArray">The <see cref="T:System.Array" /> that contains the data to copy.</param>
    /// <param name="sourceIndex">A 64-bit integer that represents the index in the <paramref name="sourceArray" /> at which copying begins.</param>
    /// <param name="destinationArray">The <see cref="T:System.Array" /> that receives the data.</param>
    /// <param name="destinationIndex">A 64-bit integer that represents the index in the <paramref name="destinationArray" /> at which storing begins.</param>
    /// <param name="length">A 64-bit integer that represents the number of elements to copy. The integer must be between zero and <see cref="F:System.Int32.MaxValue" />, inclusive.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="sourceArray" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> have different ranks.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">
    /// <paramref name="sourceArray" /> and <paramref name="destinationArray" /> are of incompatible types.</exception>
    /// <exception cref="T:System.InvalidCastException">At least one element in <paramref name="sourceArray" /> cannot be cast to the type of <paramref name="destinationArray" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="sourceIndex" /> is outside the range of valid indexes for the <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="destinationIndex" /> is outside the range of valid indexes for the <paramref name="destinationArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than 0 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="length" /> is greater than the number of elements from <paramref name="sourceIndex" /> to the end of <paramref name="sourceArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is greater than the number of elements from <paramref name="destinationIndex" /> to the end of <paramref name="destinationArray" />.</exception>
    public static void Copy(
      Array sourceArray,
      long sourceIndex,
      Array destinationArray,
      long destinationIndex,
      long length)
    {
      int sourceIndex1 = (int) sourceIndex;
      int destinationIndex1 = (int) destinationIndex;
      int length1 = (int) length;
      if (sourceIndex != (long) sourceIndex1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sourceIndex, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (destinationIndex != (long) destinationIndex1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.destinationIndex, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (length != (long) length1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      Array.Copy(sourceArray, sourceIndex1, destinationArray, destinationIndex1, length1);
    }

    /// <summary>Gets the value at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 32-bit integers.</summary>
    /// <param name="indices">A one-dimensional array of 32-bit integers that represent the indexes specifying the position of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="indices" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the multidimensional <see cref="T:System.Array" />.</returns>
    public object? GetValue(params int[] indices)
    {
      if (indices == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.indices);
      if (this.Rank != indices.Length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankIndices);
      return this.InternalGetValue(this.GetFlattenedIndex(new ReadOnlySpan<int>(indices)));
    }

    /// <summary>Gets the value at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 32-bit integer.</summary>
    /// <param name="index">A 32-bit integer that represents the position of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the one-dimensional <see cref="T:System.Array" />.</returns>
    public unsafe object? GetValue(int index)
    {
      if (this.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_Need1DArray);
      return this.InternalGetValue(this.GetFlattenedIndex(new ReadOnlySpan<int>((void*) &index, 1)));
    }

    /// <summary>Gets the value at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
    /// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the two-dimensional <see cref="T:System.Array" />.</returns>
    public unsafe object? GetValue(int index1, int index2)
    {
      if (this.Rank != 2)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_Need2DArray);
      int* pointer = stackalloc int[2];
      pointer[0] = index1;
      pointer[1] = index2;
      return this.InternalGetValue(this.GetFlattenedIndex((ReadOnlySpan<int>) new Span<int>((void*) pointer, 2)));
    }

    /// <summary>Gets the value at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
    /// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <param name="index3">A 32-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the three-dimensional <see cref="T:System.Array" />.</returns>
    public unsafe object? GetValue(int index1, int index2, int index3)
    {
      if (this.Rank != 3)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_Need3DArray);
      int* pointer = stackalloc int[3];
      pointer[0] = index1;
      pointer[1] = index2;
      pointer[2] = index3;
      return this.InternalGetValue(this.GetFlattenedIndex((ReadOnlySpan<int>) new Span<int>((void*) pointer, 3)));
    }

    /// <summary>Sets a value to the element at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 32-bit integer.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="index">A 32-bit integer that represents the position of the <see cref="T:System.Array" /> element to set.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
    public unsafe void SetValue(object? value, int index)
    {
      if (this.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_Need1DArray);
      this.InternalSetValue(value, this.GetFlattenedIndex(new ReadOnlySpan<int>((void*) &index, 1)));
    }

    /// <summary>Sets a value to the element at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    public unsafe void SetValue(object? value, int index1, int index2)
    {
      if (this.Rank != 2)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_Need2DArray);
      object obj = value;
      int* pointer = stackalloc int[2];
      pointer[0] = index1;
      pointer[1] = index2;
      Span<int> indices = new Span<int>((void*) pointer, 2);
      this.InternalSetValue(obj, this.GetFlattenedIndex((ReadOnlySpan<int>) indices));
    }

    /// <summary>Sets a value to the element at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 32-bit integers.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="index1">A 32-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <param name="index2">A 32-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <param name="index3">A 32-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    public unsafe void SetValue(object? value, int index1, int index2, int index3)
    {
      if (this.Rank != 3)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_Need3DArray);
      object obj = value;
      int* pointer = stackalloc int[3];
      pointer[0] = index1;
      pointer[1] = index2;
      pointer[2] = index3;
      Span<int> indices = new Span<int>((void*) pointer, 3);
      this.InternalSetValue(obj, this.GetFlattenedIndex((ReadOnlySpan<int>) indices));
    }

    /// <summary>Sets a value to the element at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 32-bit integers.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="indices">A one-dimensional array of 32-bit integers that represent the indexes specifying the position of the element to set.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="indices" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    public void SetValue(object? value, params int[] indices)
    {
      if (indices == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.indices);
      if (this.Rank != indices.Length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankIndices);
      this.InternalSetValue(value, this.GetFlattenedIndex(new ReadOnlySpan<int>(indices)));
    }

    /// <summary>Gets the value at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 64-bit integer.</summary>
    /// <param name="index">A 64-bit integer that represents the position of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the one-dimensional <see cref="T:System.Array" />.</returns>
    public object? GetValue(long index)
    {
      int index1 = (int) index;
      if (index != (long) index1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      return this.GetValue(index1);
    }

    /// <summary>Gets the value at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
    /// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the two-dimensional <see cref="T:System.Array" />.</returns>
    public object? GetValue(long index1, long index2)
    {
      int index1_1 = (int) index1;
      int index2_1 = (int) index2;
      if (index1 != (long) index1_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index1, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (index2 != (long) index2_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index2, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      return this.GetValue(index1_1, index2_1);
    }

    /// <summary>Gets the value at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
    /// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <param name="index3">A 64-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the three-dimensional <see cref="T:System.Array" />.</returns>
    public object? GetValue(long index1, long index2, long index3)
    {
      int index1_1 = (int) index1;
      int index2_1 = (int) index2;
      int index3_1 = (int) index3;
      if (index1 != (long) index1_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index1, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (index2 != (long) index2_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index2, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (index3 != (long) index3_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index3, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      return this.GetValue(index1_1, index2_1, index3_1);
    }

    /// <summary>Gets the value at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 64-bit integers.</summary>
    /// <param name="indices">A one-dimensional array of 64-bit integers that represent the indexes specifying the position of the <see cref="T:System.Array" /> element to get.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="indices" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    /// <returns>The value at the specified position in the multidimensional <see cref="T:System.Array" />.</returns>
    public object? GetValue(params long[] indices)
    {
      if (indices == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.indices);
      if (this.Rank != indices.Length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankIndices);
      int[] numArray = new int[indices.Length];
      for (int index1 = 0; index1 < indices.Length; ++index1)
      {
        long index2 = indices[index1];
        int num = (int) index2;
        if (index2 != (long) num)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
        numArray[index1] = num;
      }
      return this.GetValue(numArray);
    }

    /// <summary>Sets a value to the element at the specified position in the one-dimensional <see cref="T:System.Array" />. The index is specified as a 64-bit integer.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="index">A 64-bit integer that represents the position of the <see cref="T:System.Array" /> element to set.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is outside the range of valid indexes for the current <see cref="T:System.Array" />.</exception>
    public void SetValue(object? value, long index)
    {
      int index1 = (int) index;
      if (index != (long) index1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      this.SetValue(value, index1);
    }

    /// <summary>Sets a value to the element at the specified position in the two-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly two dimensions.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Either <paramref name="index1" /> or <paramref name="index2" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    public void SetValue(object? value, long index1, long index2)
    {
      int index1_1 = (int) index1;
      int index2_1 = (int) index2;
      if (index1 != (long) index1_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index1, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (index2 != (long) index2_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index2, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      this.SetValue(value, index1_1, index2_1);
    }

    /// <summary>Sets a value to the element at the specified position in the three-dimensional <see cref="T:System.Array" />. The indexes are specified as 64-bit integers.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="index1">A 64-bit integer that represents the first-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <param name="index2">A 64-bit integer that represents the second-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <param name="index3">A 64-bit integer that represents the third-dimension index of the <see cref="T:System.Array" /> element to set.</param>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly three dimensions.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index1" /> or <paramref name="index2" /> or <paramref name="index3" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    public void SetValue(object? value, long index1, long index2, long index3)
    {
      int index1_1 = (int) index1;
      int index2_1 = (int) index2;
      int index3_1 = (int) index3;
      if (index1 != (long) index1_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index1, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (index2 != (long) index2_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index2, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      if (index3 != (long) index3_1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index3, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      this.SetValue(value, index1_1, index2_1, index3_1);
    }

    /// <summary>Sets a value to the element at the specified position in the multidimensional <see cref="T:System.Array" />. The indexes are specified as an array of 64-bit integers.</summary>
    /// <param name="value">The new value for the specified element.</param>
    /// <param name="indices">A one-dimensional array of 64-bit integers that represent the indexes specifying the position of the element to set.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="indices" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The number of dimensions in the current <see cref="T:System.Array" /> is not equal to the number of elements in <paramref name="indices" />.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> cannot be cast to the element type of the current <see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Any element in <paramref name="indices" /> is outside the range of valid indexes for the corresponding dimension of the current <see cref="T:System.Array" />.</exception>
    public void SetValue(object? value, params long[] indices)
    {
      if (indices == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.indices);
      if (this.Rank != indices.Length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankIndices);
      int[] numArray = new int[indices.Length];
      for (int index1 = 0; index1 < indices.Length; ++index1)
      {
        long index2 = indices[index1];
        int num = (int) index2;
        if (index2 != (long) num)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
        numArray[index1] = num;
      }
      this.SetValue(value, numArray);
    }

    private static int GetMedian(int low, int hi) => low + (hi - low >> 1);

    /// <summary>Gets a 64-bit integer that represents the number of elements in the specified dimension of the <see cref="T:System.Array" />.</summary>
    /// <param name="dimension">A zero-based dimension of the <see cref="T:System.Array" /> whose length needs to be determined.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///        <paramref name="dimension" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="dimension" /> is equal to or greater than <see cref="P:System.Array.Rank" />.</exception>
    /// <returns>A 64-bit integer that represents the number of elements in the specified dimension.</returns>
    public long GetLongLength(int dimension) => (long) this.GetLength(dimension);

    /// <summary>Gets the number of elements contained in the <see cref="T:System.Array" />.</summary>
    /// <returns>The number of elements contained in the collection.</returns>
    int ICollection.Count => this.Length;

    /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Array" />.</summary>
    /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Array" />.</returns>
    public object SyncRoot => (object) this;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Array" /> is read-only.</summary>
    /// <returns>This property is always <see langword="false" /> for all arrays.</returns>
    public bool IsReadOnly => false;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Array" /> has a fixed size.</summary>
    /// <returns>This property is always <see langword="true" /> for all arrays.</returns>
    public bool IsFixedSize => true;

    /// <summary>Gets a value indicating whether access to the <see cref="T:System.Array" /> is synchronized (thread safe).</summary>
    /// <returns>This property is always <see langword="false" /> for all arrays.</returns>
    public bool IsSynchronized => false;

    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The index of the element to get or set.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ICollection.Count" />.</exception>
    /// <exception cref="T:System.ArgumentException">The current <see cref="T:System.Array" /> does not have exactly one dimension.</exception>
    /// <returns>The element at the specified index.</returns>
    object? IList.this[int index]
    {
      get => this.GetValue(index);
      set => this.SetValue(value, index);
    }


    #nullable disable
    /// <summary>Calling this method always throws a <see cref="T:System.NotSupportedException" /> exception.</summary>
    /// <param name="value">The object to be added to the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    /// <returns>Adding a value to an array is not supported. No value is returned.</returns>
    int IList.Add(object value)
    {
      ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_FixedSizeCollection);
      return 0;
    }

    /// <summary>Determines whether an element is in the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="value">The object to locate in the current list. The element to locate can be <see langword="null" /> for reference types.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
    bool IList.Contains(object value) => Array.IndexOf(this, value) >= this.GetLowerBound(0);

    /// <summary>Removes all items from the <see cref="T:System.Collections.IList" />.</summary>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.</exception>
    void IList.Clear() => Array.Clear(this);

    /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="value">The object to locate in the current list.</param>
    /// <returns>The index of value if found in the list; otherwise, -1.</returns>
    int IList.IndexOf(object value) => Array.IndexOf(this, value);

    /// <summary>Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.</summary>
    /// <param name="index">The index at which <paramref name="value" /> should be inserted.</param>
    /// <param name="value">The object to insert.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    /// <exception cref="T:System.NullReferenceException">
    /// <paramref name="value" /> is null reference in the <see cref="T:System.Collections.IList" />.</exception>
    void IList.Insert(int index, object value) => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_FixedSizeCollection);

    /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    void IList.Remove(object value) => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_FixedSizeCollection);

    /// <summary>Removes the <see cref="T:System.Collections.IList" /> item at the specified index.</summary>
    /// <param name="index">The index of the element to remove.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList" /> is read-only.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Collections.IList" /> has a fixed size.</exception>
    void IList.RemoveAt(int index) => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_FixedSizeCollection);


    #nullable enable
    /// <summary>Creates a shallow copy of the <see cref="T:System.Array" />.</summary>
    /// <returns>A shallow copy of the <see cref="T:System.Array" />.</returns>
    [Intrinsic]
    public object Clone() => this.MemberwiseClone();


    #nullable disable
    /// <summary>Determines whether the current collection object precedes, occurs in the same position as, or follows another object in the sort order.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that compares the current object and <paramref name="other" />.</param>
    /// <returns>An integer that indicates the relationship of the current collection object to other, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Description</description></listheader><item><term> -1</term><description> The current instance precedes <paramref name="other" />.</description></item><item><term> 0</term><description> The current instance and <paramref name="other" /> are equal.</description></item><item><term> 1</term><description> The current instance follows <paramref name="other" />.</description></item></list></returns>
    int IStructuralComparable.CompareTo(object other, IComparer comparer)
    {
      if (other == null)
        return 1;
      if (!(other is Array array) || this.Length != array.Length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.ArgumentException_OtherNotArrayOfCorrectLength, ExceptionArgument.other);
      int index = 0;
      int num;
      for (num = 0; index < array.Length && num == 0; ++index)
      {
        object x = this.GetValue(index);
        object y = array.GetValue(index);
        num = comparer.Compare(x, y);
      }
      return num;
    }

    /// <summary>Determines whether an object is equal to the current instance.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that determines whether the current instance and <paramref name="other" /> are equal.</param>
    /// <returns>
    /// <see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
    bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (!(other is Array array) || array.Length != this.Length)
        return false;
      for (int index = 0; index < array.Length; ++index)
      {
        object x = this.GetValue(index);
        object y = array.GetValue(index);
        if (!comparer.Equals(x, y))
          return false;
      }
      return true;
    }

    /// <summary>Returns a hash code for the current instance.</summary>
    /// <param name="comparer">An object that computes the hash code of the current object.</param>
    /// <returns>The hash code for the current instance.</returns>
    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
    {
      if (comparer == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparer);
      HashCode hashCode = new HashCode();
      for (int index = this.Length >= 8 ? this.Length - 8 : 0; index < this.Length; ++index)
        hashCode.Add<int>(comparer.GetHashCode(this.GetValue(index)));
      return hashCode.ToHashCode();
    }


    #nullable enable
    /// <summary>Searches an entire one-dimensional sorted array for a specific element, using the <see cref="T:System.IComparable" /> interface implemented by each element of the array and by the specified object.</summary>
    /// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch(Array array, object? value)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.BinarySearch(array, array.GetLowerBound(0), array.Length, value, (IComparer) null);
    }

    /// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the <see cref="T:System.IComparable" /> interface implemented by each element of the array and by the specified value.</summary>
    /// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch(Array array, int index, int length, object? value) => Array.BinarySearch(array, index, length, value, (IComparer) null);

    /// <summary>Searches an entire one-dimensional sorted array for a value using the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
    /// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch(Array array, object? value, IComparer? comparer)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.BinarySearch(array, array.GetLowerBound(0), array.Length, value, comparer);
    }

    /// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the specified <see cref="T:System.Collections.IComparer" /> interface.</summary>
    /// <param name="array">The sorted one-dimensional <see cref="T:System.Array" /> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, <paramref name="value" /> does not implement the <see cref="T:System.IComparable" /> interface, and the search encounters an element that does not implement the <see cref="T:System.IComparable" /> interface.</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch(
      Array array,
      int index,
      int length,
      object? value,
      IComparer? comparer)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int lowerBound = array.GetLowerBound(0);
      if (index < lowerBound)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      if (array.Length - (index - lowerBound) < length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (array.Rank != 1)
        ThrowHelper.ThrowRankException(ExceptionResource.Rank_MultiDimNotSupported);
      if (comparer == null)
        comparer = (IComparer) Comparer.Default;
      int low = index;
      int hi = index + length - 1;
      if (array is object[] objArray)
      {
        while (low <= hi)
        {
          int median = Array.GetMedian(low, hi);
          int num;
          try
          {
            num = comparer.Compare(objArray[median], value);
          }
          catch (Exception ex)
          {
            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_IComparerFailed, ex);
            return 0;
          }
          if (num == 0)
            return median;
          if (num < 0)
            low = median + 1;
          else
            hi = median - 1;
        }
        return ~low;
      }
      if (comparer == Comparer.Default)
      {
        CorElementType typeOfElementType = array.GetCorElementTypeOfElementType();
        if (typeOfElementType.IsPrimitiveType())
        {
          if (value == null)
            return ~index;
          if (array.IsValueOfElementType(value))
          {
            int adjustedIndex = index - lowerBound;
            int num = -1;
            switch (typeOfElementType)
            {
              case CorElementType.ELEMENT_TYPE_BOOLEAN:
              case CorElementType.ELEMENT_TYPE_U1:
                num = GenericBinarySearch<byte>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_CHAR:
              case CorElementType.ELEMENT_TYPE_U2:
                num = GenericBinarySearch<ushort>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_I1:
                num = GenericBinarySearch<sbyte>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_I2:
                num = GenericBinarySearch<short>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_I4:
                num = GenericBinarySearch<int>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_U4:
                num = GenericBinarySearch<uint>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_I8:
              case CorElementType.ELEMENT_TYPE_I:
                num = GenericBinarySearch<long>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_U8:
              case CorElementType.ELEMENT_TYPE_U:
                num = GenericBinarySearch<ulong>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_R4:
                num = GenericBinarySearch<float>(array, adjustedIndex, length, value);
                break;
              case CorElementType.ELEMENT_TYPE_R8:
                num = GenericBinarySearch<double>(array, adjustedIndex, length, value);
                break;
            }
            return num < 0 ? ~(index + ~num) : index + num;
          }
        }
      }
      while (low <= hi)
      {
        int median = Array.GetMedian(low, hi);
        int num;
        try
        {
          num = comparer.Compare(array.GetValue(median), value);
        }
        catch (Exception ex)
        {
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_IComparerFailed, ex);
          return 0;
        }
        if (num == 0)
          return median;
        if (num < 0)
          low = median + 1;
        else
          hi = median - 1;
      }
      return ~low;


      #nullable disable
      static int GenericBinarySearch<T>(Array array, int adjustedIndex, int length, object value) where T : struct, IComparable<T> => Array.UnsafeArrayAsSpan<T>(array, adjustedIndex, length).BinarySearch<T, T>(Unsafe.As<byte, T>(ref value.GetRawData()));
    }


    #nullable enable
    /// <summary>Searches an entire one-dimensional sorted array for a specific element, using the <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:System.Array" /> and by the specified object.</summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch<T>(T[] array, T value)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.BinarySearch<T>(array, 0, array.Length, value, (IComparer<T>) null);
    }

    /// <summary>Searches an entire one-dimensional sorted array for a value using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> implementation of each element.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch<T>(T[] array, T value, IComparer<T>? comparer)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.BinarySearch<T>(array, 0, array.Length, value, comparer);
    }

    /// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the <see cref="T:System.IComparable`1" /> generic interface implemented by each element of the <see cref="T:System.Array" /> and by the specified value.</summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch<T>(T[] array, int index, int length, T value) => Array.BinarySearch<T>(array, index, length, value, (IComparer<T>) null);

    /// <summary>Searches a range of elements in a one-dimensional sorted array for a value, using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
    /// <param name="array">The sorted one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The object to search for.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> implementation of each element.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="value" /> is of a type that is not compatible with the elements of <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and <paramref name="T" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    /// <returns>The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if <paramref name="value" /> is found; otherwise, a negative number. If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in <paramref name="array" />, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value" />. If <paramref name="value" /> is not found and <paramref name="value" /> is greater than all elements in <paramref name="array" />, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array" />, the return value can be incorrect and a negative number could be returned, even if <paramref name="value" /> is present in <paramref name="array" />.</returns>
    public static int BinarySearch<T>(
      T[] array,
      int index,
      int length,
      T value,
      IComparer<T>? comparer)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      if (array.Length - index < length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      return ArraySortHelper<T>.Default.BinarySearch(array, index, length, value, comparer);
    }

    /// <summary>Converts an array of one type to an array of another type.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to convert to a target type.</param>
    /// <param name="converter">A <see cref="T:System.Converter`2" /> that converts each element from one type to another type.</param>
    /// <typeparam name="TInput">The type of the elements of the source array.</typeparam>
    /// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="converter" /> is <see langword="null" />.</exception>
    /// <returns>An array of the target type containing the converted elements from the source array.</returns>
    public static TOutput[] ConvertAll<TInput, TOutput>(
      TInput[] array,
      Converter<TInput, TOutput> converter)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (converter == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
      TOutput[] outputArray = new TOutput[array.Length];
      for (int index = 0; index < array.Length; ++index)
        outputArray[index] = converter(array[index]);
      return outputArray;
    }

    /// <summary>Copies all the elements of the current one-dimensional array to the specified one-dimensional array starting at the specified destination array index. The index is specified as a 32-bit integer.</summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the current array.</param>
    /// <param name="index">A 32-bit integer that represents the index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than the lower bound of <paramref name="array" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// The number of elements in the source array is greater than the available number of elements from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
    /// <exception cref="T:System.RankException">The source array is multidimensional.</exception>
    /// <exception cref="T:System.InvalidCastException">At least one element in the source <see cref="T:System.Array" /> cannot be cast to the type of destination <paramref name="array" />.</exception>
    public void CopyTo(Array array, int index)
    {
      if (array != null && array.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
      Array.Copy(this, this.GetLowerBound(0), array, index, this.Length);
    }

    /// <summary>Copies all the elements of the current one-dimensional array to the specified one-dimensional array starting at the specified destination array index. The index is specified as a 64-bit integer.</summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the current array.</param>
    /// <param name="index">A 64-bit integer that represents the index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// The number of elements in the source array is greater than the available number of elements from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
    /// <exception cref="T:System.ArrayTypeMismatchException">The type of the source <see cref="T:System.Array" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
    /// <exception cref="T:System.RankException">The source <see cref="T:System.Array" /> is multidimensional.</exception>
    /// <exception cref="T:System.InvalidCastException">At least one element in the source <see cref="T:System.Array" /> cannot be cast to the type of destination <paramref name="array" />.</exception>
    public void CopyTo(Array array, long index)
    {
      int index1 = (int) index;
      if (index != (long) index1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported);
      this.CopyTo(array, index1);
    }

    /// <summary>Returns an empty array.</summary>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <returns>An empty array.</returns>
    public static T[] Empty<T>() => Array.EmptyArray<T>.Value;

    /// <summary>Determines whether the specified array contains elements that match the conditions defined by the specified predicate.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the elements to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="array" /> contains one or more elements that match the conditions defined by the specified predicate; otherwise, <see langword="false" />.</returns>
    public static bool Exists<T>(T[] array, Predicate<T> match) => Array.FindIndex<T>(array, match) != -1;

    /// <summary>Assigns the given <paramref name="value" /> of type <typeparamref name="T" /> to each element of the specified <paramref name="array" />.</summary>
    /// <param name="array">The array to be filled.</param>
    /// <param name="value">The value to assign to each array element.</param>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public static void Fill<T>(T[] array, T value)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (!typeof (T).IsValueType && array.GetType() != typeof (T[]))
      {
        for (int index = 0; index < array.Length; ++index)
          array[index] = value;
      }
      else
        new Span<T>(array).Fill(value);
    }

    /// <summary>Assigns the given <paramref name="value" /> of type <typeparamref name="T" /> to the elements of the specified <paramref name="array" /> which are
    /// within the range of <paramref name="startIndex" /> (inclusive) and the next <paramref name="count" /> number of indices.</summary>
    /// <param name="array">The <see cref="T:System.Array" /> to be filled.</param>
    /// <param name="value">The new value for the elements in the specified range.</param>
    /// <param name="startIndex">A 32-bit integer that represents the index in the <see cref="T:System.Array" /> at which filling begins.</param>
    /// <param name="count">The number of elements to copy.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    public static void Fill<T>(T[] array, T value, int startIndex, int count)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if ((uint) startIndex > (uint) array.Length)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      if ((uint) count > (uint) (array.Length - startIndex))
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      if (!typeof (T).IsValueType && array.GetType() != typeof (T[]))
      {
        for (int index = startIndex; index < startIndex + count; ++index)
          array[index] = value;
      }
      else
        new Span<T>(ref Unsafe.Add<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), (IntPtr) (uint) startIndex), count).Fill(value);
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional, zero-based array to search.</param>
    /// <param name="match">The predicate that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
    public static T? Find<T>(T[] array, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = 0; index < array.Length; ++index)
      {
        if (match(array[index]))
          return array[index];
      }
      return default (T);
    }

    /// <summary>Retrieves all the elements that match the conditions defined by the specified predicate.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the elements to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Array" /> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="T:System.Array" />.</returns>
    public static T[] FindAll<T>(T[] array, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      List<T> objList = new List<T>();
      for (int index = 0; index < array.Length; ++index)
      {
        if (match(array[index]))
          objList.Add(array[index]);
      }
      return objList.ToArray();
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public static int FindIndex<T>(T[] array, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.FindIndex<T>(array, 0, array.Length, match);
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the specified index to the last element.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public static int FindIndex<T>(T[] array, int startIndex, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.FindIndex<T>(array, startIndex, array.Length - startIndex, match);
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Array" /> that starts at the specified index and contains the specified number of elements.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public static int FindIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (startIndex < 0 || startIndex > array.Length)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      if (count < 0 || startIndex > array.Length - count)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      int num = startIndex + count;
      for (int index = startIndex; index < num; ++index)
      {
        if (match(array[index]))
          return index;
      }
      return -1;
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
    public static T? FindLast<T>(T[] array, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = array.Length - 1; index >= 0; --index)
      {
        if (match(array[index]))
          return array[index];
      }
      return default (T);
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public static int FindLastIndex<T>(T[] array, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.FindLastIndex<T>(array, array.Length - 1, array.Length, match);
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public static int FindLastIndex<T>(T[] array, int startIndex, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.FindLastIndex<T>(array, startIndex, startIndex + 1, match);
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> that defines the conditions of the element to search for.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public static int FindLastIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      if (array.Length == 0)
      {
        if (startIndex != -1)
          ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      }
      else if (startIndex < 0 || startIndex >= array.Length)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      if (count < 0 || startIndex - count + 1 < 0)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      int num = startIndex - count;
      for (int lastIndex = startIndex; lastIndex > num; --lastIndex)
      {
        if (match(array[lastIndex]))
          return lastIndex;
      }
      return -1;
    }

    /// <summary>Performs the specified action on each element of the specified array.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> on whose elements the action is to be performed.</param>
    /// <param name="action">The <see cref="T:System.Action`1" /> to perform on each element of <paramref name="array" />.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="action" /> is <see langword="null" />.</exception>
    public static void ForEach<T>(T[] array, Action<T> action)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (action == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.action);
      for (int index = 0; index < array.Length; ++index)
        action(array[index]);
    }

    /// <summary>Searches for the specified object and returns the index of its first occurrence in a one-dimensional array.</summary>
    /// <param name="array">The one-dimensional array to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <returns>The index of the first occurrence of <paramref name="value" /> in <paramref name="array" />, if found; otherwise, the lower bound of the array minus 1.</returns>
    public static int IndexOf(Array array, object? value)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.IndexOf(array, value, array.GetLowerBound(0), array.Length);
    }

    /// <summary>Searches for the specified object in a range of elements of a one-dimensional array, and returns the index of its first occurrence. The range extends from a specified index to the end of the array.</summary>
    /// <param name="array">The one-dimensional array to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <returns>The index of the first occurrence of <paramref name="value" />, if it's found, within the range of elements in <paramref name="array" /> that extends from <paramref name="startIndex" /> to the last element; otherwise, the lower bound of the array minus 1.</returns>
    public static int IndexOf(Array array, object? value, int startIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int lowerBound = array.GetLowerBound(0);
      return Array.IndexOf(array, value, startIndex, array.Length - startIndex + lowerBound);
    }

    /// <summary>Searches for the specified object in a range of elements of a one-dimensional array, and returns the index of ifs first occurrence. The range extends from a specified index for a specified number of elements.</summary>
    /// <param name="array">The one-dimensional array to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <param name="count">The number of elements to search.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <returns>The index of the first occurrence of <paramref name="value" />, if it's found in the <paramref name="array" /> from index <paramref name="startIndex" /> to <paramref name="startIndex" /> + <paramref name="count" /> - 1; otherwise, the lower bound of the array minus 1.</returns>
    public static int IndexOf(Array array, object? value, int startIndex, int count)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (array.Rank != 1)
        ThrowHelper.ThrowRankException(ExceptionResource.Rank_MultiDimNotSupported);
      int lowerBound = array.GetLowerBound(0);
      if (startIndex < lowerBound || startIndex > array.Length + lowerBound)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      if (count < 0 || count > array.Length - startIndex + lowerBound)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      int num1 = startIndex + count;
      if (array is object[] objArray)
      {
        if (value == null)
        {
          for (int index = startIndex; index < num1; ++index)
          {
            if (objArray[index] == null)
              return index;
          }
        }
        else
        {
          for (int index = startIndex; index < num1; ++index)
          {
            object obj = objArray[index];
            if (obj != null && obj.Equals(value))
              return index;
          }
        }
        return -1;
      }
      CorElementType typeOfElementType = array.GetCorElementTypeOfElementType();
      if (typeOfElementType.IsPrimitiveType())
      {
        if (value == null)
          return lowerBound - 1;
        if (array.IsValueOfElementType(value))
        {
          int adjustedIndex = startIndex - lowerBound;
          int num2 = -1;
          switch (typeOfElementType)
          {
            case CorElementType.ELEMENT_TYPE_BOOLEAN:
            case CorElementType.ELEMENT_TYPE_I1:
            case CorElementType.ELEMENT_TYPE_U1:
              num2 = GenericIndexOf<byte>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_CHAR:
            case CorElementType.ELEMENT_TYPE_I2:
            case CorElementType.ELEMENT_TYPE_U2:
              num2 = GenericIndexOf<char>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_I4:
            case CorElementType.ELEMENT_TYPE_U4:
              num2 = GenericIndexOf<int>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_I8:
            case CorElementType.ELEMENT_TYPE_U8:
            case CorElementType.ELEMENT_TYPE_I:
            case CorElementType.ELEMENT_TYPE_U:
              num2 = GenericIndexOf<long>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_R4:
              num2 = GenericIndexOf<float>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_R8:
              num2 = GenericIndexOf<double>(array, value, adjustedIndex, count);
              break;
          }
          return (num2 >= 0 ? startIndex : lowerBound) + num2;
        }
      }
      for (int index = startIndex; index < num1; ++index)
      {
        object obj = array.GetValue(index);
        if (obj == null)
        {
          if (value == null)
            return index;
        }
        else if (obj.Equals(value))
          return index;
      }
      return lowerBound - 1;


      #nullable disable
      static int GenericIndexOf<T>(Array array, object value, int adjustedIndex, int length) where T : struct, IEquatable<T> => Array.UnsafeArrayAsSpan<T>(array, adjustedIndex, length).IndexOf<T>(Unsafe.As<byte, T>(ref value.GetRawData()));
    }


    #nullable enable
    /// <summary>Searches for the specified object and returns the index of its first occurrence in a one-dimensional array.</summary>
    /// <param name="array">The one-dimensional, zero-based array to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> in the entire <paramref name="array" />, if found; otherwise, -1.</returns>
    public static int IndexOf<T>(T[] array, T value)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.IndexOf<T>(array, value, 0, array.Length);
    }

    /// <summary>Searches for the specified object in a range of elements of a one dimensional array, and returns the index of its first occurrence. The range extends from a specified index to the end of the array.</summary>
    /// <param name="array">The one-dimensional, zero-based array to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from <paramref name="startIndex" /> to the last element, if found; otherwise, -1.</returns>
    public static int IndexOf<T>(T[] array, T value, int startIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.IndexOf<T>(array, value, startIndex, array.Length - startIndex);
    }

    /// <summary>Searches for the specified object in a range of elements of a one-dimensional array, and returns the index of its first occurrence. The range extends from a specified index for a specified number of elements.</summary>
    /// <param name="array">The one-dimensional, zero-based array to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The zero-based starting index of the search. 0 (zero) is valid in an empty array.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that starts at <paramref name="startIndex" /> and contains the number of elements specified in <paramref name="count" />, if found; otherwise, -1.</returns>
    public static int IndexOf<T>(T[] array, T value, int startIndex, int count)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if ((uint) startIndex > (uint) array.Length)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      if ((uint) count > (uint) (array.Length - startIndex))
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      if (RuntimeHelpers.IsBitwiseEquatable<T>())
      {
        if (Unsafe.SizeOf<T>() == 1)
        {
          int num = SpanHelpers.IndexOf(ref Unsafe.Add<byte>(ref MemoryMarshal.GetArrayDataReference<byte>(Unsafe.As<byte[]>((object) array)), startIndex), Unsafe.As<T, byte>(ref value), count);
          return (num >= 0 ? startIndex : 0) + num;
        }
        if (Unsafe.SizeOf<T>() == 2)
        {
          int num = SpanHelpers.IndexOf(ref Unsafe.Add<char>(ref MemoryMarshal.GetArrayDataReference<char>(Unsafe.As<char[]>((object) array)), startIndex), Unsafe.As<T, char>(ref value), count);
          return (num >= 0 ? startIndex : 0) + num;
        }
        if (Unsafe.SizeOf<T>() == 4)
        {
          int num = SpanHelpers.IndexOf<int>(ref Unsafe.Add<int>(ref MemoryMarshal.GetArrayDataReference<int>(Unsafe.As<int[]>((object) array)), startIndex), Unsafe.As<T, int>(ref value), count);
          return (num >= 0 ? startIndex : 0) + num;
        }
        if (Unsafe.SizeOf<T>() == 8)
        {
          int num = SpanHelpers.IndexOf<long>(ref Unsafe.Add<long>(ref MemoryMarshal.GetArrayDataReference<long>(Unsafe.As<long[]>((object) array)), startIndex), Unsafe.As<T, long>(ref value), count);
          return (num >= 0 ? startIndex : 0) + num;
        }
      }
      return EqualityComparer<T>.Default.IndexOf(array, value, startIndex, count);
    }

    /// <summary>Searches for the specified object and returns the index of the last occurrence within the entire one-dimensional <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <returns>The index of the last occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, the lower bound of the array minus 1.</returns>
    public static int LastIndexOf(Array array, object? value)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int lowerBound = array.GetLowerBound(0);
      return Array.LastIndexOf(array, value, array.Length - 1 + lowerBound, array.Length);
    }

    /// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The starting index of the backward search.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <returns>The index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, the lower bound of the array minus 1.</returns>
    public static int LastIndexOf(Array array, object? value, int startIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int lowerBound = array.GetLowerBound(0);
      return Array.LastIndexOf(array, value, startIndex, startIndex + 1 - lowerBound);
    }

    /// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the one-dimensional <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <returns>The index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that contains the number of elements specified in <paramref name="count" /> and ends at <paramref name="startIndex" />, if found; otherwise, the lower bound of the array minus 1.</returns>
    public static int LastIndexOf(Array array, object? value, int startIndex, int count)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int lowerBound = array.GetLowerBound(0);
      if (array.Length == 0)
        return lowerBound - 1;
      if (startIndex < lowerBound || startIndex >= array.Length + lowerBound)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      if (count < 0)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      if (count > startIndex - lowerBound + 1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.endIndex, ExceptionResource.ArgumentOutOfRange_EndIndexStartIndex);
      if (array.Rank != 1)
        ThrowHelper.ThrowRankException(ExceptionResource.Rank_MultiDimNotSupported);
      int num1 = startIndex - count + 1;
      if (array is object[] objArray)
      {
        if (value == null)
        {
          for (int index = startIndex; index >= num1; --index)
          {
            if (objArray[index] == null)
              return index;
          }
        }
        else
        {
          for (int index = startIndex; index >= num1; --index)
          {
            object obj = objArray[index];
            if (obj != null && obj.Equals(value))
              return index;
          }
        }
        return -1;
      }
      CorElementType typeOfElementType = array.GetCorElementTypeOfElementType();
      if (typeOfElementType.IsPrimitiveType())
      {
        if (value == null)
          return lowerBound - 1;
        if (array.IsValueOfElementType(value))
        {
          int adjustedIndex = num1 - lowerBound;
          int num2 = -1;
          switch (typeOfElementType)
          {
            case CorElementType.ELEMENT_TYPE_BOOLEAN:
            case CorElementType.ELEMENT_TYPE_I1:
            case CorElementType.ELEMENT_TYPE_U1:
              num2 = GenericLastIndexOf<byte>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_CHAR:
            case CorElementType.ELEMENT_TYPE_I2:
            case CorElementType.ELEMENT_TYPE_U2:
              num2 = GenericLastIndexOf<char>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_I4:
            case CorElementType.ELEMENT_TYPE_U4:
              num2 = GenericLastIndexOf<int>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_I8:
            case CorElementType.ELEMENT_TYPE_U8:
            case CorElementType.ELEMENT_TYPE_I:
            case CorElementType.ELEMENT_TYPE_U:
              num2 = GenericLastIndexOf<long>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_R4:
              num2 = GenericLastIndexOf<float>(array, value, adjustedIndex, count);
              break;
            case CorElementType.ELEMENT_TYPE_R8:
              num2 = GenericLastIndexOf<double>(array, value, adjustedIndex, count);
              break;
          }
          return (num2 >= 0 ? num1 : lowerBound) + num2;
        }
      }
      for (int index = startIndex; index >= num1; --index)
      {
        object obj = array.GetValue(index);
        if (obj == null)
        {
          if (value == null)
            return index;
        }
        else if (obj.Equals(value))
          return index;
      }
      return lowerBound - 1;


      #nullable disable
      static int GenericLastIndexOf<T>(Array array, object value, int adjustedIndex, int length) where T : struct, IEquatable<T> => Array.UnsafeArrayAsSpan<T>(array, adjustedIndex, length).LastIndexOf<T>(Unsafe.As<byte, T>(ref value.GetRawData()));
    }


    #nullable enable
    /// <summary>Searches for the specified object and returns the index of the last occurrence within the entire <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the entire <paramref name="array" />, if found; otherwise, -1.</returns>
    public static int LastIndexOf<T>(T[] array, T value)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.LastIndexOf<T>(array, value, array.Length - 1, array.Length);
    }

    /// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that extends from the first element to the specified index.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that extends from the first element to <paramref name="startIndex" />, if found; otherwise, -1.</returns>
    public static int LastIndexOf<T>(T[] array, T value, int startIndex)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      return Array.LastIndexOf<T>(array, value, startIndex, array.Length == 0 ? 0 : startIndex + 1);
    }

    /// <summary>Searches for the specified object and returns the index of the last occurrence within the range of elements in the <see cref="T:System.Array" /> that contains the specified number of elements and ends at the specified index.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to search.</param>
    /// <param name="value">The object to locate in <paramref name="array" />.</param>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in <paramref name="array" />.</exception>
    /// <returns>The zero-based index of the last occurrence of <paramref name="value" /> within the range of elements in <paramref name="array" /> that contains the number of elements specified in <paramref name="count" /> and ends at <paramref name="startIndex" />, if found; otherwise, -1.</returns>
    public static int LastIndexOf<T>(T[] array, T value, int startIndex, int count)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (array.Length == 0)
      {
        if (startIndex != -1 && startIndex != 0)
          ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
        if (count != 0)
          ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
        return -1;
      }
      if ((uint) startIndex >= (uint) array.Length)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_Index();
      if (count < 0 || startIndex - count + 1 < 0)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      if (RuntimeHelpers.IsBitwiseEquatable<T>())
      {
        if (Unsafe.SizeOf<T>() == 1)
        {
          int elementOffset = startIndex - count + 1;
          int num = SpanHelpers.LastIndexOf(ref Unsafe.Add<byte>(ref MemoryMarshal.GetArrayDataReference<byte>(Unsafe.As<byte[]>((object) array)), elementOffset), Unsafe.As<T, byte>(ref value), count);
          return (num >= 0 ? elementOffset : 0) + num;
        }
        if (Unsafe.SizeOf<T>() == 2)
        {
          int elementOffset = startIndex - count + 1;
          int num = SpanHelpers.LastIndexOf(ref Unsafe.Add<char>(ref MemoryMarshal.GetArrayDataReference<char>(Unsafe.As<char[]>((object) array)), elementOffset), Unsafe.As<T, char>(ref value), count);
          return (num >= 0 ? elementOffset : 0) + num;
        }
        if (Unsafe.SizeOf<T>() == 4)
        {
          int elementOffset = startIndex - count + 1;
          int num = SpanHelpers.LastIndexOf<int>(ref Unsafe.Add<int>(ref MemoryMarshal.GetArrayDataReference<int>(Unsafe.As<int[]>((object) array)), elementOffset), Unsafe.As<T, int>(ref value), count);
          return (num >= 0 ? elementOffset : 0) + num;
        }
        if (Unsafe.SizeOf<T>() == 8)
        {
          int elementOffset = startIndex - count + 1;
          int num = SpanHelpers.LastIndexOf<long>(ref Unsafe.Add<long>(ref MemoryMarshal.GetArrayDataReference<long>(Unsafe.As<long[]>((object) array)), elementOffset), Unsafe.As<T, long>(ref value), count);
          return (num >= 0 ? elementOffset : 0) + num;
        }
      }
      return EqualityComparer<T>.Default.LastIndexOf(array, value, startIndex, count);
    }

    /// <summary>Reverses the sequence of the elements in the entire one-dimensional <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to reverse.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    public static void Reverse(Array array)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      Array.Reverse(array, array.GetLowerBound(0), array.Length);
    }

    /// <summary>Reverses the sequence of a subset of the elements in the one-dimensional <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to reverse.</param>
    /// <param name="index">The starting index of the section to reverse.</param>
    /// <param name="length">The number of elements in the section to reverse.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
    public static void Reverse(Array array, int index, int length)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      int lowerBound = array.GetLowerBound(0);
      if (index < lowerBound)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      if (array.Length - (index - lowerBound) < length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (array.Rank != 1)
        ThrowHelper.ThrowRankException(ExceptionResource.Rank_MultiDimNotSupported);
      if (length <= 1)
        return;
      int adjustedIndex = index - lowerBound;
      switch (array.GetCorElementTypeOfElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
        case CorElementType.ELEMENT_TYPE_I1:
        case CorElementType.ELEMENT_TYPE_U1:
          Array.UnsafeArrayAsSpan<byte>(array, adjustedIndex, length).Reverse<byte>();
          break;
        case CorElementType.ELEMENT_TYPE_CHAR:
        case CorElementType.ELEMENT_TYPE_I2:
        case CorElementType.ELEMENT_TYPE_U2:
          Array.UnsafeArrayAsSpan<short>(array, adjustedIndex, length).Reverse<short>();
          break;
        case CorElementType.ELEMENT_TYPE_I4:
        case CorElementType.ELEMENT_TYPE_U4:
        case CorElementType.ELEMENT_TYPE_R4:
          Array.UnsafeArrayAsSpan<int>(array, adjustedIndex, length).Reverse<int>();
          break;
        case CorElementType.ELEMENT_TYPE_I8:
        case CorElementType.ELEMENT_TYPE_U8:
        case CorElementType.ELEMENT_TYPE_R8:
        case CorElementType.ELEMENT_TYPE_I:
        case CorElementType.ELEMENT_TYPE_U:
          Array.UnsafeArrayAsSpan<long>(array, adjustedIndex, length).Reverse<long>();
          break;
        case CorElementType.ELEMENT_TYPE_ARRAY:
        case CorElementType.ELEMENT_TYPE_OBJECT:
        case CorElementType.ELEMENT_TYPE_SZARRAY:
          Array.UnsafeArrayAsSpan<object>(array, adjustedIndex, length).Reverse<object>();
          break;
        default:
          int index1 = index;
          for (int index2 = index + length - 1; index1 < index2; --index2)
          {
            object obj = array.GetValue(index1);
            array.SetValue(array.GetValue(index2), index1);
            array.SetValue(obj, index2);
            ++index1;
          }
          break;
      }
    }

    /// <summary>Reverses the sequence of the elements in the one-dimensional generic array.</summary>
    /// <param name="array">The one-dimensional array of elements to reverse.</param>
    /// <typeparam name="T">The type of the elements in <paramref name="array" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    public static void Reverse<T>(T[] array)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      Array.Reverse<T>(array, 0, array.Length);
    }

    /// <summary>Reverses the sequence of a subset of the elements in the one-dimensional generic array.</summary>
    /// <param name="array">The one-dimensional array of elements to reverse.</param>
    /// <param name="index">The starting index of the section to reverse.</param>
    /// <param name="length">The number of elements in the section to reverse.</param>
    /// <typeparam name="T">The type of the elements in <paramref name="array" />.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
    public static void Reverse<T>(T[] array, int index, int length)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      if (array.Length - index < length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (length <= 1)
        return;
      ref T local1 = ref Unsafe.Add<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), index);
      ref T local2 = ref Unsafe.Add<T>(ref Unsafe.Add<T>(ref local1, length), -1);
      do
      {
        T obj = local1;
        local1 = local2;
        local2 = obj;
        local1 = ref Unsafe.Add<T>(ref local1, 1);
        local2 = ref Unsafe.Add<T>(ref local2, -1);
      }
      while (Unsafe.IsAddressLessThan<T>(ref local1, ref local2));
    }

    /// <summary>Sorts the elements in an entire one-dimensional <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each element of the <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    public static void Sort(Array array)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      Array.Sort(array, (Array) null, array.GetLowerBound(0), array.Length, (IComparer) null);
    }

    /// <summary>Sorts a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each key.</summary>
    /// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    public static void Sort(Array keys, Array? items)
    {
      if (keys == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.keys);
      Array.Sort(keys, items, keys.GetLowerBound(0), keys.Length, (IComparer) null);
    }

    /// <summary>Sorts the elements in a range of elements in a one-dimensional <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each element of the <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    public static void Sort(Array array, int index, int length) => Array.Sort(array, (Array) null, index, length, (IComparer) null);

    /// <summary>Sorts a range of elements in a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable" /> implementation of each key.</summary>
    /// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    public static void Sort(Array keys, Array? items, int index, int length) => Array.Sort(keys, items, index, length, (IComparer) null);

    /// <summary>Sorts the elements in a one-dimensional <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
    /// <param name="array">The one-dimensional array to sort.</param>
    /// <param name="comparer">The implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    /// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    public static void Sort(Array array, IComparer? comparer)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      Array.Sort(array, (Array) null, array.GetLowerBound(0), array.Length, comparer);
    }

    /// <summary>Sorts a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
    /// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    public static void Sort(Array keys, Array? items, IComparer? comparer)
    {
      if (keys == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.keys);
      Array.Sort(keys, items, keys.GetLowerBound(0), keys.Length, comparer);
    }

    /// <summary>Sorts the elements in a range of elements in a one-dimensional <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    public static void Sort(Array array, int index, int length, IComparer? comparer) => Array.Sort(array, (Array) null, index, length, comparer);

    /// <summary>Sorts a range of elements in a pair of one-dimensional <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.IComparer" />.</summary>
    /// <param name="keys">The one-dimensional <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional <see cref="T:System.Array" /> that contains the items that correspond to each of the keys in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to sort only the <paramref name="keys" /><see cref="T:System.Array" />.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.IComparer" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the <see cref="T:System.IComparable" /> implementation of each element.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.RankException">The <paramref name="keys" /><see cref="T:System.Array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// The <paramref name="items" /><see cref="T:System.Array" /> is multidimensional.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable" /> interface.</exception>
    public static void Sort(Array keys, Array? items, int index, int length, IComparer? comparer)
    {
      if (keys == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.keys);
      if (keys.Rank != 1 || items != null && items.Rank != 1)
        ThrowHelper.ThrowRankException(ExceptionResource.Rank_MultiDimNotSupported);
      int lowerBound = keys.GetLowerBound(0);
      if (items != null && lowerBound != items.GetLowerBound(0))
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_LowerBoundsMustMatch);
      if (index < lowerBound)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      if (keys.Length - (index - lowerBound) < length || items != null && index - lowerBound > items.Length - length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (length <= 1)
        return;
      if (comparer == null)
        comparer = (IComparer) Comparer.Default;
      if (keys is object[] keys1)
      {
        object[] items1 = items as object[];
        if (items == null || items1 != null)
        {
          new Array.SorterObjectArray(keys1, items1, comparer).Sort(index, length);
          return;
        }
      }
      if (comparer == Comparer.Default)
      {
        CorElementType typeOfElementType = keys.GetCorElementTypeOfElementType();
        if (items == null || items.GetCorElementTypeOfElementType() == typeOfElementType)
        {
          int adjustedIndex = index - lowerBound;
          switch (typeOfElementType)
          {
            case CorElementType.ELEMENT_TYPE_BOOLEAN:
            case CorElementType.ELEMENT_TYPE_U1:
              GenericSort<byte>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_CHAR:
            case CorElementType.ELEMENT_TYPE_U2:
              GenericSort<ushort>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_I1:
              GenericSort<sbyte>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_I2:
              GenericSort<short>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_I4:
              GenericSort<int>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_U4:
              GenericSort<uint>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_I8:
            case CorElementType.ELEMENT_TYPE_I:
              GenericSort<long>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_U8:
            case CorElementType.ELEMENT_TYPE_U:
              GenericSort<ulong>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_R4:
              GenericSort<float>(keys, items, adjustedIndex, length);
              return;
            case CorElementType.ELEMENT_TYPE_R8:
              GenericSort<double>(keys, items, adjustedIndex, length);
              return;
          }
        }
      }
      new Array.SorterGenericArray(keys, items, comparer).Sort(index, length);


      #nullable disable
      static void GenericSort<T>(Array keys, Array items, int adjustedIndex, int length) where T : struct
      {
        Span<T> span = Array.UnsafeArrayAsSpan<T>(keys, adjustedIndex, length);
        if (items != null)
          span.Sort<T, T>(Array.UnsafeArrayAsSpan<T>(items, adjustedIndex, length));
        else
          span.Sort<T>();
      }
    }


    #nullable enable
    /// <summary>Sorts the elements in an entire <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each element of the <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    public static void Sort<T>(T[] array)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (array.Length <= 1)
        return;
      ArraySortHelper<T>.Default.Sort(new Span<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), array.Length), (IComparer<T>) null);
    }

    /// <summary>Sorts a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each key.</summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
    /// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
    /// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    public static void Sort<TKey, TValue>(TKey[] keys, TValue[]? items)
    {
      if (keys == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.keys);
      Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, (IComparer<TKey>) null);
    }

    /// <summary>Sorts the elements in a range of elements in an <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each element of the <see cref="T:System.Array" />.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    public static void Sort<T>(T[] array, int index, int length) => Array.Sort<T>(array, index, length, (IComparer<T>) null);

    /// <summary>Sorts a range of elements in a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the <see cref="T:System.IComparable`1" /> generic interface implementation of each key.</summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
    /// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">One or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    public static void Sort<TKey, TValue>(TKey[] keys, TValue[]? items, int index, int length) => Array.Sort<TKey, TValue>(keys, items, index, length, (IComparer<TKey>) null);

    /// <summary>Sorts the elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
    /// <param name="array">The one-dimensional, zero-base <see cref="T:System.Array" /> to sort.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    /// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    public static void Sort<T>(T[] array, IComparer<T>? comparer)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      Array.Sort<T>(array, 0, array.Length, comparer);
    }

    /// <summary>Sorts a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
    /// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
    /// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    public static void Sort<TKey, TValue>(TKey[] keys, TValue[]? items, IComparer<TKey>? comparer)
    {
      if (keys == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.keys);
      Array.Sort<TKey, TValue>(keys, items, 0, keys.Length, comparer);
    }

    /// <summary>Sorts the elements in a range of elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in <paramref name="array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    public static void Sort<T>(T[] array, int index, int length, IComparer<T>? comparer)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      if (array.Length - index < length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (length <= 1)
        return;
      ArraySortHelper<T>.Default.Sort(new Span<T>(ref Unsafe.Add<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), index), length), comparer);
    }

    /// <summary>Sorts a range of elements in a pair of <see cref="T:System.Array" /> objects (one contains the keys and the other contains the corresponding items) based on the keys in the first <see cref="T:System.Array" /> using the specified <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface.</summary>
    /// <param name="keys">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the keys to sort.</param>
    /// <param name="items">The one-dimensional, zero-based <see cref="T:System.Array" /> that contains the items that correspond to the keys in <paramref name="keys" />, or <see langword="null" /> to sort only <paramref name="keys" />.</param>
    /// <param name="index">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> generic interface implementation to use when comparing elements, or <see langword="null" /> to use the <see cref="T:System.IComparable`1" /> generic interface implementation of each element.</param>
    /// <typeparam name="TKey">The type of the elements of the key array.</typeparam>
    /// <typeparam name="TValue">The type of the elements of the items array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="keys" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than the lower bound of <paramref name="keys" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="length" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="items" /> is not <see langword="null" />, and the lower bound of <paramref name="keys" /> does not match the lower bound of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and the length of <paramref name="keys" /> is greater than the length of <paramref name="items" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="keys" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="items" /> is not <see langword="null" />, and <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the <paramref name="items" /><see cref="T:System.Array" />.
    /// 
    /// -or-
    /// 
    /// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and one or more elements in the <paramref name="keys" /><see cref="T:System.Array" /> do not implement the <see cref="T:System.IComparable`1" /> generic interface.</exception>
    public static void Sort<TKey, TValue>(
      TKey[] keys,
      TValue[]? items,
      int index,
      int length,
      IComparer<TKey>? comparer)
    {
      if (keys == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.keys);
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (length < 0)
        ThrowHelper.ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
      if (keys.Length - index < length || items != null && index > items.Length - length)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (length <= 1)
        return;
      if (items == null)
        Array.Sort<TKey>(keys, index, length, comparer);
      else
        ArraySortHelper<TKey, TValue>.Default.Sort(new Span<TKey>(ref Unsafe.Add<TKey>(ref MemoryMarshal.GetArrayDataReference<TKey>(keys), index), length), new Span<TValue>(ref Unsafe.Add<TValue>(ref MemoryMarshal.GetArrayDataReference<TValue>(items), index), length), comparer);
    }

    /// <summary>Sorts the elements in an <see cref="T:System.Array" /> using the specified <see cref="T:System.Comparison`1" />.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to sort.</param>
    /// <param name="comparison">The <see cref="T:System.Comparison`1" /> to use when comparing elements.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="comparison" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparison" /> caused an error during the sort. For example, <paramref name="comparison" /> might not return 0 when comparing an item with itself.</exception>
    public static void Sort<T>(T[] array, Comparison<T> comparison)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (comparison == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparison);
      ArraySortHelper<T>.Sort(new Span<T>(ref MemoryMarshal.GetArrayDataReference<T>(array), array.Length), comparison);
    }

    /// <summary>Determines whether every element in the array matches the conditions defined by the specified predicate.</summary>
    /// <param name="array">The one-dimensional, zero-based <see cref="T:System.Array" /> to check against the conditions.</param>
    /// <param name="match">The predicate that defines the conditions to check against the elements.</param>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="array" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if every element in <paramref name="array" /> matches the conditions defined by the specified predicate; otherwise, <see langword="false" />. If there are no elements in the array, the return value is <see langword="true" />.</returns>
    public static bool TrueForAll<T>(T[] array, Predicate<T> match)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = 0; index < array.Length; ++index)
      {
        if (!match(array[index]))
          return false;
      }
      return true;
    }

    /// <summary>Gets the maximum number of elements that may be contained in an array.</summary>
    /// <returns>The maximum count of elements allowed in any array.</returns>
    public static int MaxLength => 2147483591;


    #nullable disable
    private static Span<T> UnsafeArrayAsSpan<T>(Array array, int adjustedIndex, int length) => new Span<T>(ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(array)), array.Length).Slice(adjustedIndex, length);


    #nullable enable
    /// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Array" />.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Array" />.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) new ArrayEnumerator(this);


    #nullable disable
    private static class EmptyArray<T>
    {
      internal static readonly T[] Value = new T[0];
    }

    private readonly struct SorterObjectArray
    {
      private readonly object[] keys;
      private readonly object[] items;
      private readonly IComparer comparer;

      internal SorterObjectArray(object[] keys, object[] items, IComparer comparer)
      {
        this.keys = keys;
        this.items = items;
        this.comparer = comparer;
      }

      internal void SwapIfGreater(int a, int b)
      {
        if (a == b || this.comparer.Compare(this.keys[a], this.keys[b]) <= 0)
          return;
        object key = this.keys[a];
        this.keys[a] = this.keys[b];
        this.keys[b] = key;
        if (this.items == null)
          return;
        object obj = this.items[a];
        this.items[a] = this.items[b];
        this.items[b] = obj;
      }

      private void Swap(int i, int j)
      {
        object key = this.keys[i];
        this.keys[i] = this.keys[j];
        this.keys[j] = key;
        if (this.items == null)
          return;
        object obj = this.items[i];
        this.items[i] = this.items[j];
        this.items[j] = obj;
      }

      internal void Sort(int left, int length) => this.IntrospectiveSort(left, length);

      private void IntrospectiveSort(int left, int length)
      {
        if (length < 2)
          return;
        try
        {
          this.IntroSort(left, length + left - 1, 2 * (BitOperations.Log2((uint) length) + 1));
        }
        catch (IndexOutOfRangeException ex)
        {
          ThrowHelper.ThrowArgumentException_BadComparer((object) this.comparer);
        }
        catch (Exception ex)
        {
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_IComparerFailed, ex);
        }
      }

      private void IntroSort(int lo, int hi, int depthLimit)
      {
        int num1;
        for (; hi > lo; hi = num1 - 1)
        {
          int num2 = hi - lo + 1;
          if (num2 <= 16)
          {
            if (num2 == 2)
            {
              this.SwapIfGreater(lo, hi);
              break;
            }
            if (num2 == 3)
            {
              this.SwapIfGreater(lo, hi - 1);
              this.SwapIfGreater(lo, hi);
              this.SwapIfGreater(hi - 1, hi);
              break;
            }
            this.InsertionSort(lo, hi);
            break;
          }
          if (depthLimit == 0)
          {
            this.Heapsort(lo, hi);
            break;
          }
          --depthLimit;
          num1 = this.PickPivotAndPartition(lo, hi);
          this.IntroSort(num1 + 1, hi, depthLimit);
        }
      }

      private int PickPivotAndPartition(int lo, int hi)
      {
        int index = lo + (hi - lo) / 2;
        this.SwapIfGreater(lo, index);
        this.SwapIfGreater(lo, hi);
        this.SwapIfGreater(index, hi);
        object key = this.keys[index];
        this.Swap(index, hi - 1);
        int i = lo;
        int j = hi - 1;
        while (i < j)
        {
          do
            ;
          while (this.comparer.Compare(this.keys[++i], key) < 0);
          do
            ;
          while (this.comparer.Compare(key, this.keys[--j]) < 0);
          if (i < j)
            this.Swap(i, j);
          else
            break;
        }
        if (i != hi - 1)
          this.Swap(i, hi - 1);
        return i;
      }

      private void Heapsort(int lo, int hi)
      {
        int n = hi - lo + 1;
        for (int i = n / 2; i >= 1; --i)
          this.DownHeap(i, n, lo);
        for (int index = n; index > 1; --index)
        {
          this.Swap(lo, lo + index - 1);
          this.DownHeap(1, index - 1, lo);
        }
      }

      private void DownHeap(int i, int n, int lo)
      {
        object key = this.keys[lo + i - 1];
        object obj = this.items?[lo + i - 1];
        int num;
        for (; i <= n / 2; i = num)
        {
          num = 2 * i;
          if (num < n && this.comparer.Compare(this.keys[lo + num - 1], this.keys[lo + num]) < 0)
            ++num;
          if (this.comparer.Compare(key, this.keys[lo + num - 1]) < 0)
          {
            this.keys[lo + i - 1] = this.keys[lo + num - 1];
            if (this.items != null)
              this.items[lo + i - 1] = this.items[lo + num - 1];
          }
          else
            break;
        }
        this.keys[lo + i - 1] = key;
        if (this.items == null)
          return;
        this.items[lo + i - 1] = obj;
      }

      private void InsertionSort(int lo, int hi)
      {
        for (int index1 = lo; index1 < hi; ++index1)
        {
          int index2 = index1;
          object key = this.keys[index1 + 1];
          object obj = this.items?[index1 + 1];
          for (; index2 >= lo && this.comparer.Compare(key, this.keys[index2]) < 0; --index2)
          {
            this.keys[index2 + 1] = this.keys[index2];
            if (this.items != null)
              this.items[index2 + 1] = this.items[index2];
          }
          this.keys[index2 + 1] = key;
          if (this.items != null)
            this.items[index2 + 1] = obj;
        }
      }
    }

    private readonly struct SorterGenericArray
    {
      private readonly Array keys;
      private readonly Array items;
      private readonly IComparer comparer;

      internal SorterGenericArray(Array keys, Array items, IComparer comparer)
      {
        this.keys = keys;
        this.items = items;
        this.comparer = comparer;
      }

      internal void SwapIfGreater(int a, int b)
      {
        if (a == b || this.comparer.Compare(this.keys.GetValue(a), this.keys.GetValue(b)) <= 0)
          return;
        object obj1 = this.keys.GetValue(a);
        this.keys.SetValue(this.keys.GetValue(b), a);
        this.keys.SetValue(obj1, b);
        if (this.items == null)
          return;
        object obj2 = this.items.GetValue(a);
        this.items.SetValue(this.items.GetValue(b), a);
        this.items.SetValue(obj2, b);
      }

      private void Swap(int i, int j)
      {
        object obj1 = this.keys.GetValue(i);
        this.keys.SetValue(this.keys.GetValue(j), i);
        this.keys.SetValue(obj1, j);
        if (this.items == null)
          return;
        object obj2 = this.items.GetValue(i);
        this.items.SetValue(this.items.GetValue(j), i);
        this.items.SetValue(obj2, j);
      }

      internal void Sort(int left, int length) => this.IntrospectiveSort(left, length);

      private void IntrospectiveSort(int left, int length)
      {
        if (length < 2)
          return;
        try
        {
          this.IntroSort(left, length + left - 1, 2 * (BitOperations.Log2((uint) length) + 1));
        }
        catch (IndexOutOfRangeException ex)
        {
          ThrowHelper.ThrowArgumentException_BadComparer((object) this.comparer);
        }
        catch (Exception ex)
        {
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_IComparerFailed, ex);
        }
      }

      private void IntroSort(int lo, int hi, int depthLimit)
      {
        int num1;
        for (; hi > lo; hi = num1 - 1)
        {
          int num2 = hi - lo + 1;
          if (num2 <= 16)
          {
            if (num2 == 2)
            {
              this.SwapIfGreater(lo, hi);
              break;
            }
            if (num2 == 3)
            {
              this.SwapIfGreater(lo, hi - 1);
              this.SwapIfGreater(lo, hi);
              this.SwapIfGreater(hi - 1, hi);
              break;
            }
            this.InsertionSort(lo, hi);
            break;
          }
          if (depthLimit == 0)
          {
            this.Heapsort(lo, hi);
            break;
          }
          --depthLimit;
          num1 = this.PickPivotAndPartition(lo, hi);
          this.IntroSort(num1 + 1, hi, depthLimit);
        }
      }

      private int PickPivotAndPartition(int lo, int hi)
      {
        int num = lo + (hi - lo) / 2;
        this.SwapIfGreater(lo, num);
        this.SwapIfGreater(lo, hi);
        this.SwapIfGreater(num, hi);
        object obj = this.keys.GetValue(num);
        this.Swap(num, hi - 1);
        int i = lo;
        int j = hi - 1;
        while (i < j)
        {
          do
            ;
          while (this.comparer.Compare(this.keys.GetValue(++i), obj) < 0);
          do
            ;
          while (this.comparer.Compare(obj, this.keys.GetValue(--j)) < 0);
          if (i < j)
            this.Swap(i, j);
          else
            break;
        }
        if (i != hi - 1)
          this.Swap(i, hi - 1);
        return i;
      }

      private void Heapsort(int lo, int hi)
      {
        int n = hi - lo + 1;
        for (int i = n / 2; i >= 1; --i)
          this.DownHeap(i, n, lo);
        for (int index = n; index > 1; --index)
        {
          this.Swap(lo, lo + index - 1);
          this.DownHeap(1, index - 1, lo);
        }
      }

      private void DownHeap(int i, int n, int lo)
      {
        object x = this.keys.GetValue(lo + i - 1);
        object obj = this.items?.GetValue(lo + i - 1);
        int num;
        for (; i <= n / 2; i = num)
        {
          num = 2 * i;
          if (num < n && this.comparer.Compare(this.keys.GetValue(lo + num - 1), this.keys.GetValue(lo + num)) < 0)
            ++num;
          if (this.comparer.Compare(x, this.keys.GetValue(lo + num - 1)) < 0)
          {
            this.keys.SetValue(this.keys.GetValue(lo + num - 1), lo + i - 1);
            if (this.items != null)
              this.items.SetValue(this.items.GetValue(lo + num - 1), lo + i - 1);
          }
          else
            break;
        }
        this.keys.SetValue(x, lo + i - 1);
        if (this.items == null)
          return;
        this.items.SetValue(obj, lo + i - 1);
      }

      private void InsertionSort(int lo, int hi)
      {
        for (int index1 = lo; index1 < hi; ++index1)
        {
          int index2 = index1;
          object x = this.keys.GetValue(index1 + 1);
          object obj = this.items?.GetValue(index1 + 1);
          for (; index2 >= lo && this.comparer.Compare(x, this.keys.GetValue(index2)) < 0; --index2)
          {
            this.keys.SetValue(this.keys.GetValue(index2), index2 + 1);
            if (this.items != null)
              this.items.SetValue(this.items.GetValue(index2), index2 + 1);
          }
          this.keys.SetValue(x, index2 + 1);
          if (this.items != null)
            this.items.SetValue(obj, index2 + 1);
        }
      }
    }
  }
}
