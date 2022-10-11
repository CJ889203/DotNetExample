// Decompiled with JetBrains decompiler
// Type: System.HashCode
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Combines the hash code for multiple values into a single hash code.</summary>
  public struct HashCode
  {
    private static readonly uint s_seed = HashCode.GenerateGlobalSeed();
    private uint _v1;
    private uint _v2;
    private uint _v3;
    private uint _v4;
    private uint _queue1;
    private uint _queue2;
    private uint _queue3;
    private uint _length;

    private static unsafe uint GenerateGlobalSeed()
    {
      uint globalSeed;
      Interop.GetRandomBytes((byte*) &globalSeed, 4);
      return globalSeed;
    }

    /// <summary>Diffuses the hash code returned by the specified value.</summary>
    /// <param name="value1">The value to add to the hash code.</param>
    /// <typeparam name="T1">The type of the value to add the hash code.</typeparam>
    /// <returns>The hash code that represents the single value.</returns>
    public static int Combine<T1>(T1 value1)
    {
      uint queuedValue = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      return (int) HashCode.MixFinal(HashCode.QueueRound(HashCode.MixEmptyState() + 4U, queuedValue));
    }

    /// <summary>Combines two values into a hash code.</summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <typeparam name="T1">The type of the first value to combine into the hash code.</typeparam>
    /// <typeparam name="T2">The type of the second value to combine into the hash code.</typeparam>
    /// <returns>The hash code that represents the two values.</returns>
    public static int Combine<T1, T2>(T1 value1, T2 value2)
    {
      uint queuedValue1 = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      uint queuedValue2 = (object) value2 != null ? (uint) value2.GetHashCode() : 0U;
      return (int) HashCode.MixFinal(HashCode.QueueRound(HashCode.QueueRound(HashCode.MixEmptyState() + 8U, queuedValue1), queuedValue2));
    }

    /// <summary>Combines three values into a hash code.</summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <param name="value3">The third value to combine into the hash code.</param>
    /// <typeparam name="T1">The type of the first value to combine into the hash code.</typeparam>
    /// <typeparam name="T2">The type of the second value to combine into the hash code.</typeparam>
    /// <typeparam name="T3">The type of the third value to combine into the hash code.</typeparam>
    /// <returns>The hash code that represents the three values.</returns>
    public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
      uint queuedValue1 = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      uint queuedValue2 = (object) value2 != null ? (uint) value2.GetHashCode() : 0U;
      uint queuedValue3 = (object) value3 != null ? (uint) value3.GetHashCode() : 0U;
      return (int) HashCode.MixFinal(HashCode.QueueRound(HashCode.QueueRound(HashCode.QueueRound(HashCode.MixEmptyState() + 12U, queuedValue1), queuedValue2), queuedValue3));
    }

    /// <summary>Combines four values into a hash code.</summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <param name="value3">The third value to combine into the hash code.</param>
    /// <param name="value4">The fourth value to combine into the hash code.</param>
    /// <typeparam name="T1">The type of the first value to combine into the hash code.</typeparam>
    /// <typeparam name="T2">The type of the second value to combine into the hash code.</typeparam>
    /// <typeparam name="T3">The type of the third value to combine into the hash code.</typeparam>
    /// <typeparam name="T4">The type of the fourth value to combine into the hash code.</typeparam>
    /// <returns>The hash code that represents the four values.</returns>
    public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
      uint input1 = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      uint input2 = (object) value2 != null ? (uint) value2.GetHashCode() : 0U;
      uint input3 = (object) value3 != null ? (uint) value3.GetHashCode() : 0U;
      uint input4 = (object) value4 != null ? (uint) value4.GetHashCode() : 0U;
      uint v1_1;
      uint v2;
      uint v3;
      uint v4;
      HashCode.Initialize(out v1_1, out v2, out v3, out v4);
      uint v1_2 = HashCode.Round(v1_1, input1);
      v2 = HashCode.Round(v2, input2);
      v3 = HashCode.Round(v3, input3);
      v4 = HashCode.Round(v4, input4);
      return (int) HashCode.MixFinal(HashCode.MixState(v1_2, v2, v3, v4) + 16U);
    }

    /// <summary>Combines five values into a hash code.</summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <param name="value3">The third value to combine into the hash code.</param>
    /// <param name="value4">The fourth value to combine into the hash code.</param>
    /// <param name="value5">The fifth value to combine into the hash code.</param>
    /// <typeparam name="T1">The type of the first value to combine into the hash code.</typeparam>
    /// <typeparam name="T2">The type of the second value to combine into the hash code.</typeparam>
    /// <typeparam name="T3">The type of the third value to combine into the hash code.</typeparam>
    /// <typeparam name="T4">The type of the fourth value to combine into the hash code.</typeparam>
    /// <typeparam name="T5">The type of the fifth value to combine into the hash code.</typeparam>
    /// <returns>The hash code that represents the five values.</returns>
    public static int Combine<T1, T2, T3, T4, T5>(
      T1 value1,
      T2 value2,
      T3 value3,
      T4 value4,
      T5 value5)
    {
      uint input1 = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      uint input2 = (object) value2 != null ? (uint) value2.GetHashCode() : 0U;
      uint input3 = (object) value3 != null ? (uint) value3.GetHashCode() : 0U;
      uint input4 = (object) value4 != null ? (uint) value4.GetHashCode() : 0U;
      uint queuedValue = (object) value5 != null ? (uint) value5.GetHashCode() : 0U;
      uint v1_1;
      uint v2;
      uint v3;
      uint v4;
      HashCode.Initialize(out v1_1, out v2, out v3, out v4);
      uint v1_2 = HashCode.Round(v1_1, input1);
      v2 = HashCode.Round(v2, input2);
      v3 = HashCode.Round(v3, input3);
      v4 = HashCode.Round(v4, input4);
      return (int) HashCode.MixFinal(HashCode.QueueRound(HashCode.MixState(v1_2, v2, v3, v4) + 20U, queuedValue));
    }

    /// <summary>Combines six values into a hash code.</summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <param name="value3">The third value to combine into the hash code.</param>
    /// <param name="value4">The fourth value to combine into the hash code.</param>
    /// <param name="value5">The fifth value to combine into the hash code.</param>
    /// <param name="value6">The sixth value to combine into the hash code.</param>
    /// <typeparam name="T1">The type of the first value to combine into the hash code.</typeparam>
    /// <typeparam name="T2">The type of the second value to combine into the hash code.</typeparam>
    /// <typeparam name="T3">The type of the third value to combine into the hash code.</typeparam>
    /// <typeparam name="T4">The type of the fourth value to combine into the hash code.</typeparam>
    /// <typeparam name="T5">The type of the fifth value to combine into the hash code.</typeparam>
    /// <typeparam name="T6">The type of the sixth value to combine into the hash code.</typeparam>
    /// <returns>The hash code that represents the six values.</returns>
    public static int Combine<T1, T2, T3, T4, T5, T6>(
      T1 value1,
      T2 value2,
      T3 value3,
      T4 value4,
      T5 value5,
      T6 value6)
    {
      uint input1 = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      uint input2 = (object) value2 != null ? (uint) value2.GetHashCode() : 0U;
      uint input3 = (object) value3 != null ? (uint) value3.GetHashCode() : 0U;
      uint input4 = (object) value4 != null ? (uint) value4.GetHashCode() : 0U;
      uint queuedValue1 = (object) value5 != null ? (uint) value5.GetHashCode() : 0U;
      uint queuedValue2 = (object) value6 != null ? (uint) value6.GetHashCode() : 0U;
      uint v1_1;
      uint v2;
      uint v3;
      uint v4;
      HashCode.Initialize(out v1_1, out v2, out v3, out v4);
      uint v1_2 = HashCode.Round(v1_1, input1);
      v2 = HashCode.Round(v2, input2);
      v3 = HashCode.Round(v3, input3);
      v4 = HashCode.Round(v4, input4);
      return (int) HashCode.MixFinal(HashCode.QueueRound(HashCode.QueueRound(HashCode.MixState(v1_2, v2, v3, v4) + 24U, queuedValue1), queuedValue2));
    }

    /// <summary>Combines seven values into a hash code.</summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <param name="value3">The third value to combine into the hash code.</param>
    /// <param name="value4">The fourth value to combine into the hash code.</param>
    /// <param name="value5">The fifth value to combine into the hash code.</param>
    /// <param name="value6">The sixth value to combine into the hash code.</param>
    /// <param name="value7">The seventh value to combine into the hash code.</param>
    /// <typeparam name="T1">The type of the first value to combine into the hash code.</typeparam>
    /// <typeparam name="T2">The type of the second value to combine into the hash code.</typeparam>
    /// <typeparam name="T3">The type of the third value to combine into the hash code.</typeparam>
    /// <typeparam name="T4">The type of the fourth value to combine into the hash code.</typeparam>
    /// <typeparam name="T5">The type of the fifth value to combine into the hash code.</typeparam>
    /// <typeparam name="T6">The type of the sixth value to combine into the hash code.</typeparam>
    /// <typeparam name="T7">The type of the seventh value to combine into the hash code.</typeparam>
    /// <returns>The hash code that represents the seven values.</returns>
    public static int Combine<T1, T2, T3, T4, T5, T6, T7>(
      T1 value1,
      T2 value2,
      T3 value3,
      T4 value4,
      T5 value5,
      T6 value6,
      T7 value7)
    {
      uint input1 = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      uint input2 = (object) value2 != null ? (uint) value2.GetHashCode() : 0U;
      uint input3 = (object) value3 != null ? (uint) value3.GetHashCode() : 0U;
      uint input4 = (object) value4 != null ? (uint) value4.GetHashCode() : 0U;
      uint queuedValue1 = (object) value5 != null ? (uint) value5.GetHashCode() : 0U;
      uint queuedValue2 = (object) value6 != null ? (uint) value6.GetHashCode() : 0U;
      uint queuedValue3 = (object) value7 != null ? (uint) value7.GetHashCode() : 0U;
      uint v1_1;
      uint v2;
      uint v3;
      uint v4;
      HashCode.Initialize(out v1_1, out v2, out v3, out v4);
      uint v1_2 = HashCode.Round(v1_1, input1);
      v2 = HashCode.Round(v2, input2);
      v3 = HashCode.Round(v3, input3);
      v4 = HashCode.Round(v4, input4);
      return (int) HashCode.MixFinal(HashCode.QueueRound(HashCode.QueueRound(HashCode.QueueRound(HashCode.MixState(v1_2, v2, v3, v4) + 28U, queuedValue1), queuedValue2), queuedValue3));
    }

    /// <summary>Combines eight values into a hash code.</summary>
    /// <param name="value1">The first value to combine into the hash code.</param>
    /// <param name="value2">The second value to combine into the hash code.</param>
    /// <param name="value3">The third value to combine into the hash code.</param>
    /// <param name="value4">The fourth value to combine into the hash code.</param>
    /// <param name="value5">The fifth value to combine into the hash code.</param>
    /// <param name="value6">The sixth value to combine into the hash code.</param>
    /// <param name="value7">The seventh value to combine into the hash code.</param>
    /// <param name="value8">The eighth value to combine into the hash code.</param>
    /// <typeparam name="T1">The type of the first value to combine into the hash code.</typeparam>
    /// <typeparam name="T2">The type of the second value to combine into the hash code.</typeparam>
    /// <typeparam name="T3">The type of the third value to combine into the hash code.</typeparam>
    /// <typeparam name="T4">The type of the fourth value to combine into the hash code.</typeparam>
    /// <typeparam name="T5">The type of the fifth value to combine into the hash code.</typeparam>
    /// <typeparam name="T6">The type of the sixth value to combine into the hash code.</typeparam>
    /// <typeparam name="T7">The type of the seventh value to combine into the hash code.</typeparam>
    /// <typeparam name="T8">The type of the eighth value to combine into the hash code.</typeparam>
    /// <returns>The hash code that represents the eight values.</returns>
    public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(
      T1 value1,
      T2 value2,
      T3 value3,
      T4 value4,
      T5 value5,
      T6 value6,
      T7 value7,
      T8 value8)
    {
      uint input1 = (object) value1 != null ? (uint) value1.GetHashCode() : 0U;
      uint input2 = (object) value2 != null ? (uint) value2.GetHashCode() : 0U;
      uint input3 = (object) value3 != null ? (uint) value3.GetHashCode() : 0U;
      uint input4 = (object) value4 != null ? (uint) value4.GetHashCode() : 0U;
      uint input5 = (object) value5 != null ? (uint) value5.GetHashCode() : 0U;
      uint input6 = (object) value6 != null ? (uint) value6.GetHashCode() : 0U;
      uint input7 = (object) value7 != null ? (uint) value7.GetHashCode() : 0U;
      uint input8 = (object) value8 != null ? (uint) value8.GetHashCode() : 0U;
      uint v1_1;
      uint v2;
      uint v3;
      uint v4;
      HashCode.Initialize(out v1_1, out v2, out v3, out v4);
      uint hash = HashCode.Round(v1_1, input1);
      v2 = HashCode.Round(v2, input2);
      v3 = HashCode.Round(v3, input3);
      v4 = HashCode.Round(v4, input4);
      uint v1_2 = HashCode.Round(hash, input5);
      v2 = HashCode.Round(v2, input6);
      v3 = HashCode.Round(v3, input7);
      v4 = HashCode.Round(v4, input8);
      return (int) HashCode.MixFinal(HashCode.MixState(v1_2, v2, v3, v4) + 32U);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Initialize(out uint v1, out uint v2, out uint v3, out uint v4)
    {
      v1 = (uint) ((int) HashCode.s_seed - 1640531535 - 2048144777);
      v2 = HashCode.s_seed + 2246822519U;
      v3 = HashCode.s_seed;
      v4 = HashCode.s_seed - 2654435761U;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Round(uint hash, uint input) => BitOperations.RotateLeft(hash + input * 2246822519U, 13) * 2654435761U;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint QueueRound(uint hash, uint queuedValue) => BitOperations.RotateLeft(hash + queuedValue * 3266489917U, 17) * 668265263U;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint MixState(uint v1, uint v2, uint v3, uint v4) => BitOperations.RotateLeft(v1, 1) + BitOperations.RotateLeft(v2, 7) + BitOperations.RotateLeft(v3, 12) + BitOperations.RotateLeft(v4, 18);

    private static uint MixEmptyState() => HashCode.s_seed + 374761393U;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint MixFinal(uint hash)
    {
      hash ^= hash >> 15;
      hash *= 2246822519U;
      hash ^= hash >> 13;
      hash *= 3266489917U;
      hash ^= hash >> 16;
      return hash;
    }


    #nullable enable
    /// <summary>Adds a single value to the hash code.</summary>
    /// <param name="value">The value to add to the hash code.</param>
    /// <typeparam name="T">The type of the value to add to the hash code.</typeparam>
    public void Add<T>(T value) => this.Add((object) value != null ? value.GetHashCode() : 0);

    /// <summary>Adds a single value to the hash code, specifying the type that provides the hash code function.</summary>
    /// <param name="value">The value to add to the hash code.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to use to calculate the hash code.
    /// This value can be a null reference (Nothing in Visual Basic), which will use the default equality comparer for <typeparamref name="T" />.</param>
    /// <typeparam name="T">The type of the value to add to the hash code.</typeparam>
    public void Add<T>(T value, IEqualityComparer<T>? comparer) => this.Add((object) value == null ? 0 : (comparer != null ? comparer.GetHashCode(value) : value.GetHashCode()));

    /// <summary>Adds a span of bytes to the hash code.</summary>
    /// <param name="value">The span to add.</param>
    public void AddBytes(ReadOnlySpan<byte> value)
    {
      ref byte local1 = ref MemoryMarshal.GetReference<byte>(value);
      // ISSUE: variable of a reference type
      byte& local2;
      for (local2 = ref Unsafe.Add<byte>(ref local1, value.Length); Unsafe.ByteOffset<byte>(ref local1, ref local2) >= new IntPtr(4); local1 = ref Unsafe.Add<byte>(ref local1, 4))
        this.Add(Unsafe.ReadUnaligned<int>(ref local1));
      for (; Unsafe.IsAddressLessThan<byte>(ref local1, ref local2); local1 = ref Unsafe.Add<byte>(ref local1, 1))
        this.Add((int) local1);
    }

    private void Add(int value)
    {
      uint input = (uint) value;
      uint num = this._length++;
      switch (num % 4U)
      {
        case 0:
          this._queue1 = input;
          break;
        case 1:
          this._queue2 = input;
          break;
        case 2:
          this._queue3 = input;
          break;
        default:
          if (num == 3U)
            HashCode.Initialize(out this._v1, out this._v2, out this._v3, out this._v4);
          this._v1 = HashCode.Round(this._v1, this._queue1);
          this._v2 = HashCode.Round(this._v2, this._queue2);
          this._v3 = HashCode.Round(this._v3, this._queue3);
          this._v4 = HashCode.Round(this._v4, input);
          break;
      }
    }

    /// <summary>Calculates the final hash code after consecutive <see cref="Overload:System.HashCode.Add" /> invocations.</summary>
    /// <returns>The calculated hash code.</returns>
    public int ToHashCode()
    {
      uint length = this._length;
      uint num = length % 4U;
      uint hash = (length < 4U ? HashCode.MixEmptyState() : HashCode.MixState(this._v1, this._v2, this._v3, this._v4)) + length * 4U;
      if (num > 0U)
      {
        hash = HashCode.QueueRound(hash, this._queue1);
        if (num > 1U)
        {
          hash = HashCode.QueueRound(hash, this._queue2);
          if (num > 2U)
            hash = HashCode.QueueRound(hash, this._queue3);
        }
      }
      return (int) HashCode.MixFinal(hash);
    }

    /// <summary>This method is not supported and should not be called.</summary>
    /// <exception cref="T:System.NotSupportedException">Always thrown when this method is called.</exception>
    /// <returns>This method will always throw a <see cref="T:System.NotSupportedException" />.</returns>
    [Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => throw new NotSupportedException(SR.HashCode_HashCodeNotSupported);

    /// <summary>This method is not supported and should not be called.</summary>
    /// <param name="obj">Ignored.</param>
    /// <exception cref="T:System.NotSupportedException">Always thrown when this method is called.</exception>
    /// <returns>This method will always throw a <see cref="T:System.NotSupportedException" />.</returns>
    [Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj) => throw new NotSupportedException(SR.HashCode_EqualityNotSupported);
  }
}
