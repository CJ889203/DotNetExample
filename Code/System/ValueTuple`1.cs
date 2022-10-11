// Decompiled with JetBrains decompiler
// Type: System.ValueTuple`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Represents a value tuple with a single component.</summary>
  /// <typeparam name="T1">The type of the value tuple's only element.</typeparam>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public struct ValueTuple<T1> : 
    IEquatable<ValueTuple<T1>>,
    IStructuralEquatable,
    IStructuralComparable,
    IComparable,
    IComparable<ValueTuple<T1>>,
    IValueTupleInternal,
    ITuple
  {
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`1" /> instance's first element.</summary>
    public T1 Item1;

    /// <summary>Initializes a new <see cref="T:System.ValueTuple`1" /> instance.</summary>
    /// <param name="item1">The value tuple's first element.</param>
    public ValueTuple(T1 item1) => this.Item1 = item1;

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`1" /> instance is equal to a specified object.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ValueTuple<T1> other && this.Equals(other);

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`1" /> instance is equal to a specified <see cref="T:System.ValueTuple`1" /> instance.</summary>
    /// <param name="other">The value tuple to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
    public bool Equals(ValueTuple<T1> other) => EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1);


    #nullable disable
    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`1" /> instance is equal to a specified object based on a specified comparison method.</summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) => other is ValueTuple<T1> valueTuple && comparer.Equals((object) this.Item1, (object) valueTuple.Item1);

    /// <summary>Compares the current <see cref="T:System.ValueTuple`1" /> instance to a specified object by using a specified comparer and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="obj" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    int IComparable.CompareTo(object other)
    {
      if (other != null)
      {
        if (other is ValueTuple<T1> valueTuple)
          return Comparer<T1>.Default.Compare(this.Item1, valueTuple.Item1);
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      }
      return 1;
    }


    #nullable enable
    /// <summary>Compares the current <see cref="T:System.ValueTuple`1" /> instance to a specified <see cref="T:System.ValueTuple`1" /> instance.</summary>
    /// <param name="other">The tuple to compare with this instance.</param>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    public int CompareTo(ValueTuple<T1> other) => Comparer<T1>.Default.Compare(this.Item1, other.Item1);


    #nullable disable
    /// <summary>Compares the current <see cref="T:System.ValueTuple`1" /> instance to a specified object by using a specified comparer and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that provides custom rules for comparison.</param>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    int IStructuralComparable.CompareTo(object other, IComparer comparer)
    {
      if (other != null)
      {
        if (other is ValueTuple<T1> valueTuple)
          return comparer.Compare((object) this.Item1, (object) valueTuple.Item1);
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      }
      return 1;
    }

    /// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`1" /> instance.</summary>
    /// <returns>The hash code for the current <see cref="T:System.ValueTuple`1" /> instance.</returns>
    public override int GetHashCode()
    {
      // ISSUE: explicit reference operation
      ref T1 local1 = @this.Item1;
      if ((object) default (T1) == null)
      {
        T1 obj = local1;
        ref T1 local2 = ref obj;
        if ((object) obj == null)
          return 0;
        local1 = ref local2;
      }
      return local1.GetHashCode();
    }

    /// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`1" /> instance by using a specified computation method.</summary>
    /// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" /> method calculates the hash code of the current <see cref="T:System.ValueTuple`1" /> instance.</param>
    /// <returns>A 32-bit signed integer hash code.</returns>
    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) => comparer.GetHashCode((object) this.Item1);

    int IValueTupleInternal.GetHashCode(IEqualityComparer comparer) => comparer.GetHashCode((object) this.Item1);


    #nullable enable
    /// <summary>Returns a string that represents the value of this <see cref="T:System.ValueTuple`1" /> instance.</summary>
    /// <returns>The string representation of this <see cref="T:System.ValueTuple`1" /> instance.</returns>
    public override string ToString()
    {
      // ISSUE: explicit reference operation
      ref T1 local1 = @this.Item1;
      string str;
      if ((object) default (T1) == null)
      {
        T1 obj = local1;
        ref T1 local2 = ref obj;
        if ((object) obj == null)
        {
          str = (string) null;
          goto label_4;
        }
        else
          local1 = ref local2;
      }
      str = local1.ToString();
label_4:
      return "(" + str + ")";
    }


    #nullable disable
    string IValueTupleInternal.ToStringEnd()
    {
      // ISSUE: explicit reference operation
      ref T1 local1 = @this.Item1;
      string str;
      if ((object) default (T1) == null)
      {
        T1 obj = local1;
        ref T1 local2 = ref obj;
        if ((object) obj == null)
        {
          str = (string) null;
          goto label_4;
        }
        else
          local1 = ref local2;
      }
      str = local1.ToString();
label_4:
      return str + ")";
    }

    /// <summary>Gets the number of elements in the <see langword="ValueTuple" />.</summary>
    /// <returns>1, the number of elements in a <see cref="T:System.ValueTuple`1" /> object.</returns>
    int ITuple.Length => 1;


    #nullable enable
    /// <summary>Gets the value of the <see langword="ValueTuple" /> element.</summary>
    /// <param name="index">The index of the <see langword="ValueTuple" /> element. <paramref name="index" /> must be 0.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index" /> is less than 0 or greater than 0.</exception>
    /// <returns>The value of the <see langword="ValueTuple" /> element.</returns>
    object? ITuple.this[int index]
    {
      get
      {
        if (index != 0)
          throw new IndexOutOfRangeException();
        return (object) this.Item1;
      }
    }
  }
}
