// Decompiled with JetBrains decompiler
// Type: System.Tuple`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace System
{
  /// <summary>Represents a 1-tuple, or singleton.</summary>
  /// <typeparam name="T1">The type of the tuple's only component.</typeparam>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class Tuple<T1> : 
    IStructuralEquatable,
    IStructuralComparable,
    IComparable,
    ITupleInternal,
    ITuple
  {

    #nullable disable
    private readonly T1 m_Item1;


    #nullable enable
    /// <summary>Gets the value of the <see cref="T:System.Tuple`1" /> object's single component.</summary>
    /// <returns>The value of the current <see cref="T:System.Tuple`1" /> object's single component.</returns>
    public T1 Item1 => this.m_Item1;

    /// <summary>Initializes a new instance of the <see cref="T:System.Tuple`1" /> class.</summary>
    /// <param name="item1">The value of the tuple's only component.</param>
    public Tuple(T1 item1) => this.m_Item1 = item1;

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.Tuple`1" /> object is equal to a specified object.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => this.Equals(obj, (IEqualityComparer) EqualityComparer<object>.Default);


    #nullable disable
    /// <summary>Returns a value that indicates whether the current <see cref="T:System.Tuple`1" /> object is equal to a specified object based on a specified comparison method.</summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    bool IStructuralEquatable.Equals([NotNullWhen(true)] object other, IEqualityComparer comparer) => this.Equals(other, comparer);

    private bool Equals([NotNullWhen(true)] object other, IEqualityComparer comparer) => other != null && other is Tuple<T1> tuple && comparer.Equals((object) this.m_Item1, (object) tuple.m_Item1);

    /// <summary>Compares the current <see cref="T:System.Tuple`1" /> object to a specified object, and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="obj">An object to compare with the current instance.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="obj" /> is not a <see cref="T:System.Tuple`1" /> object.</exception>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="obj" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="obj" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="obj" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="obj" />.</description></item></list></returns>
    int IComparable.CompareTo(object obj) => this.CompareTo(obj, (IComparer) Comparer<object>.Default);

    /// <summary>Compares the current <see cref="T:System.Tuple`1" /> object to a specified object by using a specified comparer, and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="other">An object to compare with the current instance.</param>
    /// <param name="comparer">An object that provides custom rules for comparison.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="other" /> is not a <see cref="T:System.Tuple`1" /> object.</exception>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    int IStructuralComparable.CompareTo(object other, IComparer comparer) => this.CompareTo(other, comparer);

    private int CompareTo(object other, IComparer comparer)
    {
      if (other == null)
        return 1;
      if (!(other is Tuple<T1> tuple))
        throw new ArgumentException(SR.Format(SR.ArgumentException_TupleIncorrectType, (object) this.GetType()), nameof (other));
      return comparer.Compare((object) this.m_Item1, (object) tuple.m_Item1);
    }

    /// <summary>Returns the hash code for the current <see cref="T:System.Tuple`1" /> object.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => this.GetHashCode((IEqualityComparer) EqualityComparer<object>.Default);

    /// <summary>Calculates the hash code for the current <see cref="T:System.Tuple`1" /> object by using a specified computation method.</summary>
    /// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" /> method calculates the hash code of the current <see cref="T:System.Tuple`1" /> object.</param>
    /// <returns>A 32-bit signed integer hash code.</returns>
    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) => this.GetHashCode(comparer);

    int ITupleInternal.GetHashCode(IEqualityComparer comparer) => this.GetHashCode(comparer);

    private int GetHashCode(IEqualityComparer comparer) => comparer.GetHashCode((object) this.m_Item1);


    #nullable enable
    /// <summary>Returns a string that represents the value of this <see cref="T:System.Tuple`1" /> instance.</summary>
    /// <returns>The string representation of this <see cref="T:System.Tuple`1" /> object.</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append('(');
      return this.ToString(sb);
    }


    #nullable disable
    string ITupleInternal.ToString(StringBuilder sb) => this.ToString(sb);

    private string ToString(StringBuilder sb)
    {
      sb.Append((object) this.m_Item1);
      sb.Append(')');
      return sb.ToString();
    }

    /// <summary>Gets the number of elements in the <see langword="Tuple" />.</summary>
    /// <returns>1, the number of elements in a <see cref="T:System.Tuple`1" /> object.</returns>
    int ITuple.Length => 1;


    #nullable enable
    /// <summary>Gets the value of the <see langword="Tuple" /> element.</summary>
    /// <param name="index">The index of the <see langword="Tuple" /> element. <paramref name="index" /> must be 0.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index" /> is less than 0 or greater than 0.</exception>
    /// <returns>The value of the <see langword="Tuple" /> element.</returns>
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
