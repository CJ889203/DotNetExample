// Decompiled with JetBrains decompiler
// Type: System.ValueTuple`5
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Represents a value tuple with 5 components.</summary>
  /// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
  /// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
  /// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
  /// <typeparam name="T4">The type of the value tuple's fourth element.</typeparam>
  /// <typeparam name="T5">The type of the value tuple's fifth element.</typeparam>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  [StructLayout(LayoutKind.Auto)]
  public struct ValueTuple<T1, T2, T3, T4, T5> : 
    IEquatable<(T1, T2, T3, T4, T5)>,
    IStructuralEquatable,
    IStructuralComparable,
    IComparable,
    IComparable<(T1, T2, T3, T4, T5)>,
    IValueTupleInternal,
    ITuple
  {
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's first element.</summary>
    public T1 Item1;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's second element.</summary>
    public T2 Item2;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's third element.</summary>
    public T3 Item3;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's fourth element.</summary>
    public T4 Item4;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's fifth element.</summary>
    public T5 Item5;

    /// <summary>Initializes a new <see cref="T:System.ValueTuple`5" /> instance.</summary>
    /// <param name="item1">The value tuple's first element.</param>
    /// <param name="item2">The value tuple's second element.</param>
    /// <param name="item3">The value tuple's third element.</param>
    /// <param name="item4">The value tuple's fourth element.</param>
    /// <param name="item5">The value tuple's fifth element.</param>
    public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
    {
      this.Item1 = item1;
      this.Item2 = item2;
      this.Item3 = item3;
      this.Item4 = item4;
      this.Item5 = item5;
    }

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`5" /> instance is equal to a specified object.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is (T1, T2, T3, T4, T5) other && this.Equals(other);

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`5" /> instance is equal to a specified <see cref="T:System.ValueTuple`5" /> instance.</summary>
    /// <param name="other">The value tuple to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
    public bool Equals((T1, T2, T3, T4, T5) other) => EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4) && EqualityComparer<T5>.Default.Equals(this.Item5, other.Item5);


    #nullable disable
    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`5" /> instance is equal to a specified object based on a specified comparison method.</summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified objects; otherwise, <see langword="false" />.</returns>
    bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) => other is (T1, T2, T3, T4, T5) valueTuple && comparer.Equals((object) this.Item1, (object) valueTuple.Item1) && comparer.Equals((object) this.Item2, (object) valueTuple.Item2) && comparer.Equals((object) this.Item3, (object) valueTuple.Item3) && comparer.Equals((object) this.Item5, (object) valueTuple.Item5);

    /// <summary>Compares the current <see cref="T:System.ValueTuple`5" /> instance to a specified object by using a specified comparer and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="obj" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    int IComparable.CompareTo(object other)
    {
      if (other != null)
      {
        if (other is (T1, T2, T3, T4, T5) other1)
          return this.CompareTo(other1);
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      }
      return 1;
    }


    #nullable enable
    /// <summary>Compares the current <see cref="T:System.ValueTuple`5" /> instance to a specified <see cref="T:System.ValueTuple`5" /> instance.</summary>
    /// <param name="other">The tuple to compare with this instance.</param>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    public int CompareTo((T1, T2, T3, T4, T5) other)
    {
      int num1 = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
      if (num1 != 0)
        return num1;
      int num2 = Comparer<T2>.Default.Compare(this.Item2, other.Item2);
      if (num2 != 0)
        return num2;
      int num3 = Comparer<T3>.Default.Compare(this.Item3, other.Item3);
      if (num3 != 0)
        return num3;
      int num4 = Comparer<T4>.Default.Compare(this.Item4, other.Item4);
      return num4 != 0 ? num4 : Comparer<T5>.Default.Compare(this.Item5, other.Item5);
    }


    #nullable disable
    /// <summary>Compares the current <see cref="T:System.ValueTuple`5" /> instance to a specified object by using a specified comparer and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that provides custom rules for comparison.</param>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    int IStructuralComparable.CompareTo(object other, IComparer comparer)
    {
      if (other != null)
      {
        if (other is (T1, T2, T3, T4, T5) valueTuple)
        {
          int num1 = comparer.Compare((object) this.Item1, (object) valueTuple.Item1);
          if (num1 != 0)
            return num1;
          int num2 = comparer.Compare((object) this.Item2, (object) valueTuple.Item2);
          if (num2 != 0)
            return num2;
          int num3 = comparer.Compare((object) this.Item3, (object) valueTuple.Item3);
          if (num3 != 0)
            return num3;
          int num4 = comparer.Compare((object) this.Item4, (object) valueTuple.Item4);
          return num4 != 0 ? num4 : comparer.Compare((object) this.Item5, (object) valueTuple.Item5);
        }
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      }
      return 1;
    }

    /// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`5" /> instance.</summary>
    /// <returns>The hash code for the current <see cref="T:System.ValueTuple`5" /> instance.</returns>
    public override int GetHashCode()
    {
      // ISSUE: explicit reference operation
      ref T1 local1 = @this.Item1;
      int num1;
      if ((object) default (T1) == null)
      {
        T1 obj = local1;
        ref T1 local2 = ref obj;
        if ((object) obj == null)
        {
          num1 = 0;
          goto label_4;
        }
        else
          local1 = ref local2;
      }
      num1 = local1.GetHashCode();
label_4:
      // ISSUE: explicit reference operation
      ref T2 local3 = @this.Item2;
      int num2;
      if ((object) default (T2) == null)
      {
        T2 obj = local3;
        ref T2 local4 = ref obj;
        if ((object) obj == null)
        {
          num2 = 0;
          goto label_8;
        }
        else
          local3 = ref local4;
      }
      num2 = local3.GetHashCode();
label_8:
      // ISSUE: explicit reference operation
      ref T3 local5 = @this.Item3;
      int num3;
      if ((object) default (T3) == null)
      {
        T3 obj = local5;
        ref T3 local6 = ref obj;
        if ((object) obj == null)
        {
          num3 = 0;
          goto label_12;
        }
        else
          local5 = ref local6;
      }
      num3 = local5.GetHashCode();
label_12:
      // ISSUE: explicit reference operation
      ref T4 local7 = @this.Item4;
      int num4;
      if ((object) default (T4) == null)
      {
        T4 obj = local7;
        ref T4 local8 = ref obj;
        if ((object) obj == null)
        {
          num4 = 0;
          goto label_16;
        }
        else
          local7 = ref local8;
      }
      num4 = local7.GetHashCode();
label_16:
      // ISSUE: explicit reference operation
      ref T5 local9 = @this.Item5;
      int num5;
      if ((object) default (T5) == null)
      {
        T5 obj = local9;
        ref T5 local10 = ref obj;
        if ((object) obj == null)
        {
          num5 = 0;
          goto label_20;
        }
        else
          local9 = ref local10;
      }
      num5 = local9.GetHashCode();
label_20:
      return HashCode.Combine<int, int, int, int, int>(num1, num2, num3, num4, num5);
    }

    /// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`5" /> instance by using a specified computation method.</summary>
    /// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" /> method calculates the hash code of the current <see cref="T:System.ValueTuple`5" /> instance.</param>
    /// <returns>A 32-bit signed integer hash code.</returns>
    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) => this.GetHashCodeCore(comparer);

    private int GetHashCodeCore(IEqualityComparer comparer) => HashCode.Combine<int, int, int, int, int>(comparer.GetHashCode((object) this.Item1), comparer.GetHashCode((object) this.Item2), comparer.GetHashCode((object) this.Item3), comparer.GetHashCode((object) this.Item4), comparer.GetHashCode((object) this.Item5));

    int IValueTupleInternal.GetHashCode(IEqualityComparer comparer) => this.GetHashCodeCore(comparer);


    #nullable enable
    /// <summary>Returns a string that represents the value of this <see cref="T:System.ValueTuple`5" /> instance.</summary>
    /// <returns>The string representation of this <see cref="T:System.ValueTuple`5" /> instance.</returns>
    public override string ToString()
    {
      string[] strArray = new string[11];
      strArray[0] = "(";
      // ISSUE: explicit reference operation
      ref T1 local1 = @this.Item1;
      string str1;
      if ((object) default (T1) == null)
      {
        T1 obj = local1;
        ref T1 local2 = ref obj;
        if ((object) obj == null)
        {
          str1 = (string) null;
          goto label_4;
        }
        else
          local1 = ref local2;
      }
      str1 = local1.ToString();
label_4:
      strArray[1] = str1;
      strArray[2] = ", ";
      // ISSUE: explicit reference operation
      ref T2 local3 = @this.Item2;
      string str2;
      if ((object) default (T2) == null)
      {
        T2 obj = local3;
        ref T2 local4 = ref obj;
        if ((object) obj == null)
        {
          str2 = (string) null;
          goto label_8;
        }
        else
          local3 = ref local4;
      }
      str2 = local3.ToString();
label_8:
      strArray[3] = str2;
      strArray[4] = ", ";
      // ISSUE: explicit reference operation
      ref T3 local5 = @this.Item3;
      string str3;
      if ((object) default (T3) == null)
      {
        T3 obj = local5;
        ref T3 local6 = ref obj;
        if ((object) obj == null)
        {
          str3 = (string) null;
          goto label_12;
        }
        else
          local5 = ref local6;
      }
      str3 = local5.ToString();
label_12:
      strArray[5] = str3;
      strArray[6] = ", ";
      // ISSUE: explicit reference operation
      ref T4 local7 = @this.Item4;
      string str4;
      if ((object) default (T4) == null)
      {
        T4 obj = local7;
        ref T4 local8 = ref obj;
        if ((object) obj == null)
        {
          str4 = (string) null;
          goto label_16;
        }
        else
          local7 = ref local8;
      }
      str4 = local7.ToString();
label_16:
      strArray[7] = str4;
      strArray[8] = ", ";
      // ISSUE: explicit reference operation
      ref T5 local9 = @this.Item5;
      string str5;
      if ((object) default (T5) == null)
      {
        T5 obj = local9;
        ref T5 local10 = ref obj;
        if ((object) obj == null)
        {
          str5 = (string) null;
          goto label_20;
        }
        else
          local9 = ref local10;
      }
      str5 = local9.ToString();
label_20:
      strArray[9] = str5;
      strArray[10] = ")";
      return string.Concat(strArray);
    }


    #nullable disable
    string IValueTupleInternal.ToStringEnd()
    {
      string[] strArray = new string[10];
      // ISSUE: explicit reference operation
      ref T1 local1 = @this.Item1;
      string str1;
      if ((object) default (T1) == null)
      {
        T1 obj = local1;
        ref T1 local2 = ref obj;
        if ((object) obj == null)
        {
          str1 = (string) null;
          goto label_4;
        }
        else
          local1 = ref local2;
      }
      str1 = local1.ToString();
label_4:
      strArray[0] = str1;
      strArray[1] = ", ";
      // ISSUE: explicit reference operation
      ref T2 local3 = @this.Item2;
      string str2;
      if ((object) default (T2) == null)
      {
        T2 obj = local3;
        ref T2 local4 = ref obj;
        if ((object) obj == null)
        {
          str2 = (string) null;
          goto label_8;
        }
        else
          local3 = ref local4;
      }
      str2 = local3.ToString();
label_8:
      strArray[2] = str2;
      strArray[3] = ", ";
      // ISSUE: explicit reference operation
      ref T3 local5 = @this.Item3;
      string str3;
      if ((object) default (T3) == null)
      {
        T3 obj = local5;
        ref T3 local6 = ref obj;
        if ((object) obj == null)
        {
          str3 = (string) null;
          goto label_12;
        }
        else
          local5 = ref local6;
      }
      str3 = local5.ToString();
label_12:
      strArray[4] = str3;
      strArray[5] = ", ";
      // ISSUE: explicit reference operation
      ref T4 local7 = @this.Item4;
      string str4;
      if ((object) default (T4) == null)
      {
        T4 obj = local7;
        ref T4 local8 = ref obj;
        if ((object) obj == null)
        {
          str4 = (string) null;
          goto label_16;
        }
        else
          local7 = ref local8;
      }
      str4 = local7.ToString();
label_16:
      strArray[6] = str4;
      strArray[7] = ", ";
      // ISSUE: explicit reference operation
      ref T5 local9 = @this.Item5;
      string str5;
      if ((object) default (T5) == null)
      {
        T5 obj = local9;
        ref T5 local10 = ref obj;
        if ((object) obj == null)
        {
          str5 = (string) null;
          goto label_20;
        }
        else
          local9 = ref local10;
      }
      str5 = local9.ToString();
label_20:
      strArray[8] = str5;
      strArray[9] = ")";
      return string.Concat(strArray);
    }

    /// <summary>Gets the number of elements in the <see langword="ValueTuple" />.</summary>
    /// <returns>5, the number of elements in a <see cref="T:System.ValueTuple`5" /> object.</returns>
    int ITuple.Length => 5;


    #nullable enable
    /// <summary>Gets the value of the specified <see langword="ValueTuple" /> element.</summary>
    /// <param name="index">The index of the specified <see langword="ValueTuple" /> element. <paramref name="index" /> can range from 0 to 4.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index" /> is less than 0 or greater than 4.</exception>
    /// <returns>The value of the <see langword="ValueTuple" /> element at the specified position.</returns>
    object? ITuple.this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return (object) this.Item1;
          case 1:
            return (object) this.Item2;
          case 2:
            return (object) this.Item3;
          case 3:
            return (object) this.Item4;
          case 4:
            return (object) this.Item5;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }
  }
}
