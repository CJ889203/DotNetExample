﻿// Decompiled with JetBrains decompiler
// Type: System.ValueTuple
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Provides static methods for creating value tuples.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct ValueTuple : 
    IEquatable<ValueTuple>,
    IStructuralEquatable,
    IStructuralComparable,
    IComparable,
    IComparable<ValueTuple>,
    IValueTupleInternal,
    ITuple
  {
    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple" /> instance is equal to a specified object.</summary>
    /// <param name="obj">The object to compare to the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.ValueTuple" /> instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ValueTuple;

    /// <summary>Determines whether two <see cref="T:System.ValueTuple" /> instances are equal. This method always returns <see langword="true" />.</summary>
    /// <param name="other">The value tuple to compare with the current instance.</param>
    /// <returns>This method always returns <see langword="true" />.</returns>
    public bool Equals(ValueTuple other) => true;


    #nullable disable
    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple" /> instance is equal to a specified object based on a specified comparison method.</summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) => other is ValueTuple;

    /// <summary>Compares this <see cref="T:System.ValueTuple" /> instance with a specified object and returns an indication of their relative values.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="other" /> is not a <see cref="T:System.ValueTuple" /> instance.</exception>
    /// <returns>0 if <paramref name="other" /> is a <see cref="T:System.ValueTuple" /> instance; otherwise, 1 if <paramref name="other" /> is <see langword="null" />.</returns>
    int IComparable.CompareTo(object other)
    {
      if (other == null)
        return 1;
      if (!(other is ValueTuple))
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      return 0;
    }

    /// <summary>Compares the current <see cref="T:System.ValueTuple" /> instance to a specified <see cref="T:System.ValueTuple" /> instance.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="other" /> is not a <see cref="T:System.ValueTuple" /> instance.</exception>
    /// <returns>This method always returns 0.</returns>
    public int CompareTo(ValueTuple other) => 0;

    /// <summary>Compares the current <see cref="T:System.ValueTuple" /> instance to a specified object.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that provides custom rules for comparison. This parameter is ignored.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="other" /> is not a <see cref="T:System.ValueTuple" /> instance.</exception>
    /// <returns>Returns 0 if <paramref name="other" /> is a <see cref="T:System.ValueTuple" /> instance and 1 if <paramref name="other" /> is <see langword="null" />.</returns>
    int IStructuralComparable.CompareTo(object other, IComparer comparer)
    {
      if (other == null)
        return 1;
      if (!(other is ValueTuple))
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      return 0;
    }

    /// <summary>Returns the hash code for the current <see cref="T:System.ValueTuple" /> instance.</summary>
    /// <returns>This method always return 0.</returns>
    public override int GetHashCode() => 0;

    /// <summary>Returns the hash code for this <see cref="T:System.ValueTuple" /> instance.</summary>
    /// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" /> method computes the hash code. This parameter is ignored.</param>
    /// <returns>The hash code for this <see cref="T:System.ValueTuple" /> instance.</returns>
    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) => 0;

    int IValueTupleInternal.GetHashCode(IEqualityComparer comparer) => 0;


    #nullable enable
    /// <summary>Returns the string representation of this <see cref="T:System.ValueTuple" /> instance.</summary>
    /// <returns>This method always returns "()".</returns>
    public override string ToString() => "()";


    #nullable disable
    string IValueTupleInternal.ToStringEnd() => ")";

    /// <summary>Gets the length of this <see langword="ValueTuple" /> instance, which is always 0. There are no elements in a <see langword="ValueTuple" />.</summary>
    /// <returns>0, the number of elements in this <see langword="ValueTuple" /> instance.</returns>
    int ITuple.Length => 0;


    #nullable enable
    /// <summary>Returns an <see cref="T:System.IndexOutOfRangeException" />. There are no elements in a <see langword="ValueTuple" />.</summary>
    /// <param name="index">There is no acceptable value for <paramref name="index" />.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">There is no acceptable value for <paramref name="index" />.</exception>
    /// <returns>An <see cref="T:System.IndexOutOfRangeException" />.</returns>
    object? ITuple.this[int index] => throw new IndexOutOfRangeException();

    /// <summary>Creates a new value tuple with zero components.</summary>
    /// <returns>A new value tuple with no components.</returns>
    public static ValueTuple Create() => new ValueTuple();

    /// <summary>Creates a new value tuple with 1 component (a singleton).</summary>
    /// <param name="item1">The value of the value tuple's only component.</param>
    /// <typeparam name="T1">The type of the value tuple's only component.</typeparam>
    /// <returns>A value tuple with 1 component.</returns>
    public static ValueTuple<T1> Create<T1>(T1 item1) => (item1);

    /// <summary>Creates a new value tuple with 2 components (a pair).</summary>
    /// <param name="item1">The value of the value tuple's first component.</param>
    /// <param name="item2">The value of the value tuple's second component.</param>
    /// <typeparam name="T1">The type of the value tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the value tuple's second component.</typeparam>
    /// <returns>A value tuple with 2 components.</returns>
    public static (T1, T2) Create<T1, T2>(T1 item1, T2 item2) => (item1, item2);

    /// <summary>Creates a new value tuple with 3 components (a triple).</summary>
    /// <param name="item1">The value of the value tuple's first component.</param>
    /// <param name="item2">The value of the value tuple's second component.</param>
    /// <param name="item3">The value of the value tuple's third component.</param>
    /// <typeparam name="T1">The type of the value tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the value tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the value tuple's third component.</typeparam>
    /// <returns>A value tuple with 3 components.</returns>
    public static (T1, T2, T3) Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) => (item1, item2, item3);

    /// <summary>Creates a new value tuple with 4 components (a quadruple).</summary>
    /// <param name="item1">The value of the value tuple's first component.</param>
    /// <param name="item2">The value of the value tuple's second component.</param>
    /// <param name="item3">The value of the value tuple's third component.</param>
    /// <param name="item4">The value of the value tuple's fourth component.</param>
    /// <typeparam name="T1">The type of the value tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the value tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the value tuple's third component.</typeparam>
    /// <typeparam name="T4">The type of the value tuple's fourth component.</typeparam>
    /// <returns>A value tuple with 4 components.</returns>
    public static (T1, T2, T3, T4) Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) => (item1, item2, item3, item4);

    /// <summary>Creates a new value tuple with 5 components (a quintuple).</summary>
    /// <param name="item1">The value of the value tuple's first component.</param>
    /// <param name="item2">The value of the value tuple's second component.</param>
    /// <param name="item3">The value of the value tuple's third component.</param>
    /// <param name="item4">The value of the value tuple's fourth component.</param>
    /// <param name="item5">The value of the value tuple's fifth component.</param>
    /// <typeparam name="T1">The type of the value tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the value tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the value tuple's third component.</typeparam>
    /// <typeparam name="T4">The type of the value tuple's fourth component.</typeparam>
    /// <typeparam name="T5">The type of the value tuple's fifth component.</typeparam>
    /// <returns>A value tuple with 5 components.</returns>
    public static (T1, T2, T3, T4, T5) Create<T1, T2, T3, T4, T5>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5)
    {
      return (item1, item2, item3, item4, item5);
    }

    /// <summary>Creates a new value tuple with 6 components (a sexuple).</summary>
    /// <param name="item1">The value of the value tuple's first component.</param>
    /// <param name="item2">The value of the value tuple's second component.</param>
    /// <param name="item3">The value of the value tuple's third component.</param>
    /// <param name="item4">The value of the value tuple's fourth component.</param>
    /// <param name="item5">The value of the value tuple's fifth component.</param>
    /// <param name="item6">The value of the value tuple's sixth component.</param>
    /// <typeparam name="T1">The type of the value tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the value tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the value tuple's third component.</typeparam>
    /// <typeparam name="T4">The type of the value tuple's fourth component.</typeparam>
    /// <typeparam name="T5">The type of the value tuple's fifth component.</typeparam>
    /// <typeparam name="T6">The type of the value tuple's sixth component.</typeparam>
    /// <returns>A value tuple with 6 components.</returns>
    public static (T1, T2, T3, T4, T5, T6) Create<T1, T2, T3, T4, T5, T6>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5,
      T6 item6)
    {
      return (item1, item2, item3, item4, item5, item6);
    }

    /// <summary>Creates a new value tuple with 7 components (a septuple).</summary>
    /// <param name="item1">The value of the value tuple's first component.</param>
    /// <param name="item2">The value of the value tuple's second component.</param>
    /// <param name="item3">The value of the value tuple's third component.</param>
    /// <param name="item4">The value of the value tuple's fourth component.</param>
    /// <param name="item5">The value of the value tuple's fifth component.</param>
    /// <param name="item6">The value of the value tuple's sixth component.</param>
    /// <param name="item7">The value of the value tuple's seventh component.</param>
    /// <typeparam name="T1">The type of the value tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the value tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the value tuple's third component.</typeparam>
    /// <typeparam name="T4">The type of the value tuple's fourth component.</typeparam>
    /// <typeparam name="T5">The type of the value tuple's fifth component.</typeparam>
    /// <typeparam name="T6">The type of the value tuple's sixth component.</typeparam>
    /// <typeparam name="T7">The type of the value tuple's seventh component.</typeparam>
    /// <returns>A value tuple with 7 components.</returns>
    public static (T1, T2, T3, T4, T5, T6, T7) Create<T1, T2, T3, T4, T5, T6, T7>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5,
      T6 item6,
      T7 item7)
    {
      return (item1, item2, item3, item4, item5, item6, item7);
    }

    /// <summary>Creates a new value tuple with 8 components (an octuple).</summary>
    /// <param name="item1">The value of the value tuple's first component.</param>
    /// <param name="item2">The value of the value tuple's second component.</param>
    /// <param name="item3">The value of the value tuple's third component.</param>
    /// <param name="item4">The value of the value tuple's fourth component.</param>
    /// <param name="item5">The value of the value tuple's fifth component.</param>
    /// <param name="item6">The value of the value tuple's sixth component.</param>
    /// <param name="item7">The value of the value tuple's seventh component.</param>
    /// <param name="item8">The value of the value tuple's eighth component.</param>
    /// <typeparam name="T1">The type of the value tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the value tuple's second component.</typeparam>
    /// <typeparam name="T3">The type of the value tuple's third component.</typeparam>
    /// <typeparam name="T4">The type of the value tuple's fourth component.</typeparam>
    /// <typeparam name="T5">The type of the value tuple's fifth component.</typeparam>
    /// <typeparam name="T6">The type of the value tuple's sixth component.</typeparam>
    /// <typeparam name="T7">The type of the value tuple's seventh component.</typeparam>
    /// <typeparam name="T8">The type of the value tuple's eighth component.</typeparam>
    /// <returns>A value tuple with 8 components.</returns>
    public static (T1, T2, T3, T4, T5, T6, T7, T8) Create<T1, T2, T3, T4, T5, T6, T7, T8>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5,
      T6 item6,
      T7 item7,
      T8 item8)
    {
      return (item1, item2, item3, item4, item5, item6, item7, ValueTuple.Create<T8>(item8));
    }
  }
}
