// Decompiled with JetBrains decompiler
// Type: System.ValueTuple`8
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
  /// <summary>Represents an n-value tuple, where n is 8 or greater.</summary>
  /// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
  /// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
  /// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
  /// <typeparam name="T4">The type of the value tuple's fourth element.</typeparam>
  /// <typeparam name="T5">The type of the value tuple's fifth element.</typeparam>
  /// <typeparam name="T6">The type of the value tuple's sixth element.</typeparam>
  /// <typeparam name="T7">The type of the value tuple's seventh element.</typeparam>
  /// <typeparam name="TRest">Any generic value tuple instance that defines the types of the tuple's remaining elements.</typeparam>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  [StructLayout(LayoutKind.Auto)]
  public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : 
    IEquatable<(T1, T2, T3, T4, T5, T6, T7, TRest)>,
    IStructuralEquatable,
    IStructuralComparable,
    IComparable,
    IComparable<(T1, T2, T3, T4, T5, T6, T7, TRest)>,
    IValueTupleInternal,
    ITuple
    where TRest : struct
  {
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`8" /> instance's first element.</summary>
    public T1 Item1;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`8" /> instance's second element.</summary>
    public T2 Item2;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`8" /> instance's third element.</summary>
    public T3 Item3;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`8" /> instance's fourth element.</summary>
    public T4 Item4;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`8" /> instance's fifth element.</summary>
    public T5 Item5;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`8" /> instance's sixth element.</summary>
    public T6 Item6;
    /// <summary>Gets the value of the current <see cref="T:System.ValueTuple`8" /> instance's seventh element.</summary>
    public T7 Item7;
    /// <summary>Gets the current <see cref="T:System.ValueTuple`8" /> instance's remaining elements.</summary>
    public TRest Rest;

    /// <summary>Initializes a new <see cref="T:System.ValueTuple`8" /> instance.</summary>
    /// <param name="item1">The value tuple's first element.</param>
    /// <param name="item2">The value tuple's second element.</param>
    /// <param name="item3">The value tuple's third element.</param>
    /// <param name="item4">The value tuple's fourth element.</param>
    /// <param name="item5">The value tuple's fifth element.</param>
    /// <param name="item6">The value tuple's sixth element.</param>
    /// <param name="item7">The value tuple's seventh element.</param>
    /// <param name="rest">An instance of any value tuple type that contains the values of the value's tuple's remaining elements.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="rest" /> is not a generic value tuple type.</exception>
    public ValueTuple(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5,
      T6 item6,
      T7 item7,
      TRest rest)
    {
      if (!((ValueType) rest is IValueTupleInternal))
        throw new ArgumentException(SR.ArgumentException_ValueTupleLastArgumentNotAValueTuple);
      this.Item1 = item1;
      this.Item2 = item2;
      this.Item3 = item3;
      this.Item4 = item4;
      this.Item5 = item5;
      this.Item6 = item6;
      this.Item7 = item7;
      this.Rest = rest;
    }

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`8" /> instance is equal to a specified object.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is (T1, T2, T3, T4, T5, T6, T7, TRest) other && this.Equals(other);

    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`8" /> instance is equal to a specified <see cref="T:System.ValueTuple`8" /> instance.</summary>
    /// <param name="other">The value tuple to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
    public bool Equals((T1, T2, T3, T4, T5, T6, T7, TRest) other) => EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4) && EqualityComparer<T5>.Default.Equals(this.Item5, other.Item5) && EqualityComparer<T6>.Default.Equals(this.Item6, other.Item6) && EqualityComparer<T7>.Default.Equals(this.Item7, other.Item7) && EqualityComparer<TRest>.Default.Equals(this.Rest, other.Rest);


    #nullable disable
    /// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`8" /> instance is equal to a specified object based on a specified comparison method.</summary>
    /// <param name="other">The object to compare with this instance.</param>
    /// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified objects; otherwise, <see langword="false" />.</returns>
    bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) => other is (T1, T2, T3, T4, T5, T6, T7, TRest) valueTuple && comparer.Equals((object) this.Item1, (object) valueTuple.Item1) && comparer.Equals((object) this.Item2, (object) valueTuple.Item2) && comparer.Equals((object) this.Item3, (object) valueTuple.Item3) && comparer.Equals((object) this.Item5, (object) valueTuple.Item5) && comparer.Equals((object) this.Item6, (object) valueTuple.Item6) && comparer.Equals((object) this.Item7, (object) valueTuple.Item7) && comparer.Equals((object) this.Rest, (object) valueTuple.Rest);

    /// <summary>Compares the current <see cref="T:System.ValueTuple`8" /> object to a specified object and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="other">An object to compare with the current instance.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="other" /> is not a <see cref="T:System.ValueTuple`8" /> object.</exception>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="obj" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    int IComparable.CompareTo(object other)
    {
      if (other != null)
      {
        if (other is (T1, T2, T3, T4, T5, T6, T7, TRest) other1)
          return this.CompareTo(other1);
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      }
      return 1;
    }


    #nullable enable
    /// <summary>Compares the current <see cref="T:System.ValueTuple`8" /> instance to a specified <see cref="T:System.ValueTuple`8" /> instance.</summary>
    /// <param name="other">The tuple to compare with this instance.</param>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    public int CompareTo((T1, T2, T3, T4, T5, T6, T7, TRest) other)
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
      if (num4 != 0)
        return num4;
      int num5 = Comparer<T5>.Default.Compare(this.Item5, other.Item5);
      if (num5 != 0)
        return num5;
      int num6 = Comparer<T6>.Default.Compare(this.Item6, other.Item6);
      if (num6 != 0)
        return num6;
      int num7 = Comparer<T7>.Default.Compare(this.Item7, other.Item7);
      return num7 != 0 ? num7 : Comparer<TRest>.Default.Compare(this.Rest, other.Rest);
    }


    #nullable disable
    /// <summary>Compares the current <see cref="T:System.ValueTuple`8" /> instance to a specified object by using a specified comparer and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <param name="comparer">An object that provides custom rules for comparison.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="other" /> is not a <see cref="T:System.ValueTuple`8" /> object.</exception>
    /// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance precedes <paramref name="other" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="other" /> have the same position in the sort order.</description></item><item><term> A positive integer</term><description> This instance follows <paramref name="other" />.</description></item></list></returns>
    int IStructuralComparable.CompareTo(object other, IComparer comparer)
    {
      if (other != null)
      {
        if (other is (T1, T2, T3, T4, T5, T6, T7, TRest) valueTuple)
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
          if (num4 != 0)
            return num4;
          int num5 = comparer.Compare((object) this.Item5, (object) valueTuple.Item5);
          if (num5 != 0)
            return num5;
          int num6 = comparer.Compare((object) this.Item6, (object) valueTuple.Item6);
          if (num6 != 0)
            return num6;
          int num7 = comparer.Compare((object) this.Item7, (object) valueTuple.Item7);
          return num7 != 0 ? num7 : comparer.Compare((object) this.Rest, (object) valueTuple.Rest);
        }
        ThrowHelper.ThrowArgumentException_TupleIncorrectType((object) this);
      }
      return 1;
    }

    /// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`8" /> instance.</summary>
    /// <returns>The hash code for the current <see cref="T:System.ValueTuple`8" /> instance.</returns>
    public override int GetHashCode()
    {
      if (!((ValueType) this.Rest is IValueTupleInternal))
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
            goto label_5;
          }
          else
            local1 = ref local2;
        }
        num1 = local1.GetHashCode();
label_5:
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
            goto label_9;
          }
          else
            local3 = ref local4;
        }
        num2 = local3.GetHashCode();
label_9:
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
            goto label_13;
          }
          else
            local5 = ref local6;
        }
        num3 = local5.GetHashCode();
label_13:
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
            goto label_17;
          }
          else
            local7 = ref local8;
        }
        num4 = local7.GetHashCode();
label_17:
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
            goto label_21;
          }
          else
            local9 = ref local10;
        }
        num5 = local9.GetHashCode();
label_21:
        // ISSUE: explicit reference operation
        ref T6 local11 = @this.Item6;
        int num6;
        if ((object) default (T6) == null)
        {
          T6 obj = local11;
          ref T6 local12 = ref obj;
          if ((object) obj == null)
          {
            num6 = 0;
            goto label_25;
          }
          else
            local11 = ref local12;
        }
        num6 = local11.GetHashCode();
label_25:
        // ISSUE: explicit reference operation
        ref T7 local13 = @this.Item7;
        int num7;
        if ((object) default (T7) == null)
        {
          T7 obj = local13;
          ref T7 local14 = ref obj;
          if ((object) obj == null)
          {
            num7 = 0;
            goto label_29;
          }
          else
            local13 = ref local14;
        }
        num7 = local13.GetHashCode();
label_29:
        return HashCode.Combine<int, int, int, int, int, int, int>(num1, num2, num3, num4, num5, num6, num7);
      }
      int length = ((ITuple) this.Rest).Length;
      int hashCode = this.Rest.GetHashCode();
      if (length >= 8)
        return hashCode;
      switch (8 - length)
      {
        case 1:
          // ISSUE: explicit reference operation
          ref T7 local15 = @this.Item7;
          int num8;
          if ((object) default (T7) == null)
          {
            T7 obj = local15;
            ref T7 local16 = ref obj;
            if ((object) obj == null)
            {
              num8 = 0;
              goto label_38;
            }
            else
              local15 = ref local16;
          }
          num8 = local15.GetHashCode();
label_38:
          int num9 = hashCode;
          return HashCode.Combine<int, int>(num8, num9);
        case 2:
          // ISSUE: explicit reference operation
          ref T6 local17 = @this.Item6;
          int num10;
          if ((object) default (T6) == null)
          {
            T6 obj = local17;
            ref T6 local18 = ref obj;
            if ((object) obj == null)
            {
              num10 = 0;
              goto label_43;
            }
            else
              local17 = ref local18;
          }
          num10 = local17.GetHashCode();
label_43:
          // ISSUE: explicit reference operation
          ref T7 local19 = @this.Item7;
          int num11;
          if ((object) default (T7) == null)
          {
            T7 obj = local19;
            ref T7 local20 = ref obj;
            if ((object) obj == null)
            {
              num11 = 0;
              goto label_47;
            }
            else
              local19 = ref local20;
          }
          num11 = local19.GetHashCode();
label_47:
          int num12 = hashCode;
          return HashCode.Combine<int, int, int>(num10, num11, num12);
        case 3:
          // ISSUE: explicit reference operation
          ref T5 local21 = @this.Item5;
          int num13;
          if ((object) default (T5) == null)
          {
            T5 obj = local21;
            ref T5 local22 = ref obj;
            if ((object) obj == null)
            {
              num13 = 0;
              goto label_52;
            }
            else
              local21 = ref local22;
          }
          num13 = local21.GetHashCode();
label_52:
          // ISSUE: explicit reference operation
          ref T6 local23 = @this.Item6;
          int num14;
          if ((object) default (T6) == null)
          {
            T6 obj = local23;
            ref T6 local24 = ref obj;
            if ((object) obj == null)
            {
              num14 = 0;
              goto label_56;
            }
            else
              local23 = ref local24;
          }
          num14 = local23.GetHashCode();
label_56:
          // ISSUE: explicit reference operation
          ref T7 local25 = @this.Item7;
          int num15;
          if ((object) default (T7) == null)
          {
            T7 obj = local25;
            ref T7 local26 = ref obj;
            if ((object) obj == null)
            {
              num15 = 0;
              goto label_60;
            }
            else
              local25 = ref local26;
          }
          num15 = local25.GetHashCode();
label_60:
          int num16 = hashCode;
          return HashCode.Combine<int, int, int, int>(num13, num14, num15, num16);
        case 4:
          // ISSUE: explicit reference operation
          ref T4 local27 = @this.Item4;
          int num17;
          if ((object) default (T4) == null)
          {
            T4 obj = local27;
            ref T4 local28 = ref obj;
            if ((object) obj == null)
            {
              num17 = 0;
              goto label_65;
            }
            else
              local27 = ref local28;
          }
          num17 = local27.GetHashCode();
label_65:
          // ISSUE: explicit reference operation
          ref T5 local29 = @this.Item5;
          int num18;
          if ((object) default (T5) == null)
          {
            T5 obj = local29;
            ref T5 local30 = ref obj;
            if ((object) obj == null)
            {
              num18 = 0;
              goto label_69;
            }
            else
              local29 = ref local30;
          }
          num18 = local29.GetHashCode();
label_69:
          // ISSUE: explicit reference operation
          ref T6 local31 = @this.Item6;
          int num19;
          if ((object) default (T6) == null)
          {
            T6 obj = local31;
            ref T6 local32 = ref obj;
            if ((object) obj == null)
            {
              num19 = 0;
              goto label_73;
            }
            else
              local31 = ref local32;
          }
          num19 = local31.GetHashCode();
label_73:
          // ISSUE: explicit reference operation
          ref T7 local33 = @this.Item7;
          int num20;
          if ((object) default (T7) == null)
          {
            T7 obj = local33;
            ref T7 local34 = ref obj;
            if ((object) obj == null)
            {
              num20 = 0;
              goto label_77;
            }
            else
              local33 = ref local34;
          }
          num20 = local33.GetHashCode();
label_77:
          int num21 = hashCode;
          return HashCode.Combine<int, int, int, int, int>(num17, num18, num19, num20, num21);
        case 5:
          // ISSUE: explicit reference operation
          ref T3 local35 = @this.Item3;
          int num22;
          if ((object) default (T3) == null)
          {
            T3 obj = local35;
            ref T3 local36 = ref obj;
            if ((object) obj == null)
            {
              num22 = 0;
              goto label_82;
            }
            else
              local35 = ref local36;
          }
          num22 = local35.GetHashCode();
label_82:
          // ISSUE: explicit reference operation
          ref T4 local37 = @this.Item4;
          int num23;
          if ((object) default (T4) == null)
          {
            T4 obj = local37;
            ref T4 local38 = ref obj;
            if ((object) obj == null)
            {
              num23 = 0;
              goto label_86;
            }
            else
              local37 = ref local38;
          }
          num23 = local37.GetHashCode();
label_86:
          // ISSUE: explicit reference operation
          ref T5 local39 = @this.Item5;
          int num24;
          if ((object) default (T5) == null)
          {
            T5 obj = local39;
            ref T5 local40 = ref obj;
            if ((object) obj == null)
            {
              num24 = 0;
              goto label_90;
            }
            else
              local39 = ref local40;
          }
          num24 = local39.GetHashCode();
label_90:
          // ISSUE: explicit reference operation
          ref T6 local41 = @this.Item6;
          int num25;
          if ((object) default (T6) == null)
          {
            T6 obj = local41;
            ref T6 local42 = ref obj;
            if ((object) obj == null)
            {
              num25 = 0;
              goto label_94;
            }
            else
              local41 = ref local42;
          }
          num25 = local41.GetHashCode();
label_94:
          // ISSUE: explicit reference operation
          ref T7 local43 = @this.Item7;
          int num26;
          if ((object) default (T7) == null)
          {
            T7 obj = local43;
            ref T7 local44 = ref obj;
            if ((object) obj == null)
            {
              num26 = 0;
              goto label_98;
            }
            else
              local43 = ref local44;
          }
          num26 = local43.GetHashCode();
label_98:
          int num27 = hashCode;
          return HashCode.Combine<int, int, int, int, int, int>(num22, num23, num24, num25, num26, num27);
        case 6:
          // ISSUE: explicit reference operation
          ref T2 local45 = @this.Item2;
          int num28;
          if ((object) default (T2) == null)
          {
            T2 obj = local45;
            ref T2 local46 = ref obj;
            if ((object) obj == null)
            {
              num28 = 0;
              goto label_103;
            }
            else
              local45 = ref local46;
          }
          num28 = local45.GetHashCode();
label_103:
          // ISSUE: explicit reference operation
          ref T3 local47 = @this.Item3;
          int num29;
          if ((object) default (T3) == null)
          {
            T3 obj = local47;
            ref T3 local48 = ref obj;
            if ((object) obj == null)
            {
              num29 = 0;
              goto label_107;
            }
            else
              local47 = ref local48;
          }
          num29 = local47.GetHashCode();
label_107:
          // ISSUE: explicit reference operation
          ref T4 local49 = @this.Item4;
          int num30;
          if ((object) default (T4) == null)
          {
            T4 obj = local49;
            ref T4 local50 = ref obj;
            if ((object) obj == null)
            {
              num30 = 0;
              goto label_111;
            }
            else
              local49 = ref local50;
          }
          num30 = local49.GetHashCode();
label_111:
          // ISSUE: explicit reference operation
          ref T5 local51 = @this.Item5;
          int num31;
          if ((object) default (T5) == null)
          {
            T5 obj = local51;
            ref T5 local52 = ref obj;
            if ((object) obj == null)
            {
              num31 = 0;
              goto label_115;
            }
            else
              local51 = ref local52;
          }
          num31 = local51.GetHashCode();
label_115:
          // ISSUE: explicit reference operation
          ref T6 local53 = @this.Item6;
          int num32;
          if ((object) default (T6) == null)
          {
            T6 obj = local53;
            ref T6 local54 = ref obj;
            if ((object) obj == null)
            {
              num32 = 0;
              goto label_119;
            }
            else
              local53 = ref local54;
          }
          num32 = local53.GetHashCode();
label_119:
          // ISSUE: explicit reference operation
          ref T7 local55 = @this.Item7;
          int num33;
          if ((object) default (T7) == null)
          {
            T7 obj = local55;
            ref T7 local56 = ref obj;
            if ((object) obj == null)
            {
              num33 = 0;
              goto label_123;
            }
            else
              local55 = ref local56;
          }
          num33 = local55.GetHashCode();
label_123:
          int num34 = hashCode;
          return HashCode.Combine<int, int, int, int, int, int, int>(num28, num29, num30, num31, num32, num33, num34);
        case 7:
        case 8:
          // ISSUE: explicit reference operation
          ref T1 local57 = @this.Item1;
          int num35;
          if ((object) default (T1) == null)
          {
            T1 obj = local57;
            ref T1 local58 = ref obj;
            if ((object) obj == null)
            {
              num35 = 0;
              goto label_128;
            }
            else
              local57 = ref local58;
          }
          num35 = local57.GetHashCode();
label_128:
          // ISSUE: explicit reference operation
          ref T2 local59 = @this.Item2;
          int num36;
          if ((object) default (T2) == null)
          {
            T2 obj = local59;
            ref T2 local60 = ref obj;
            if ((object) obj == null)
            {
              num36 = 0;
              goto label_132;
            }
            else
              local59 = ref local60;
          }
          num36 = local59.GetHashCode();
label_132:
          // ISSUE: explicit reference operation
          ref T3 local61 = @this.Item3;
          int num37;
          if ((object) default (T3) == null)
          {
            T3 obj = local61;
            ref T3 local62 = ref obj;
            if ((object) obj == null)
            {
              num37 = 0;
              goto label_136;
            }
            else
              local61 = ref local62;
          }
          num37 = local61.GetHashCode();
label_136:
          // ISSUE: explicit reference operation
          ref T4 local63 = @this.Item4;
          int num38;
          if ((object) default (T4) == null)
          {
            T4 obj = local63;
            ref T4 local64 = ref obj;
            if ((object) obj == null)
            {
              num38 = 0;
              goto label_140;
            }
            else
              local63 = ref local64;
          }
          num38 = local63.GetHashCode();
label_140:
          // ISSUE: explicit reference operation
          ref T5 local65 = @this.Item5;
          int num39;
          if ((object) default (T5) == null)
          {
            T5 obj = local65;
            ref T5 local66 = ref obj;
            if ((object) obj == null)
            {
              num39 = 0;
              goto label_144;
            }
            else
              local65 = ref local66;
          }
          num39 = local65.GetHashCode();
label_144:
          // ISSUE: explicit reference operation
          ref T6 local67 = @this.Item6;
          int num40;
          if ((object) default (T6) == null)
          {
            T6 obj = local67;
            ref T6 local68 = ref obj;
            if ((object) obj == null)
            {
              num40 = 0;
              goto label_148;
            }
            else
              local67 = ref local68;
          }
          num40 = local67.GetHashCode();
label_148:
          // ISSUE: explicit reference operation
          ref T7 local69 = @this.Item7;
          int num41;
          if ((object) default (T7) == null)
          {
            T7 obj = local69;
            ref T7 local70 = ref obj;
            if ((object) obj == null)
            {
              num41 = 0;
              goto label_152;
            }
            else
              local69 = ref local70;
          }
          num41 = local69.GetHashCode();
label_152:
          int num42 = hashCode;
          return HashCode.Combine<int, int, int, int, int, int, int, int>(num35, num36, num37, num38, num39, num40, num41, num42);
        default:
          return -1;
      }
    }

    /// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`8" /> instance by using a specified computation method.</summary>
    /// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" /> method calculates the hash code of the current <see cref="T:System.ValueTuple`8" /> instance.</param>
    /// <returns>A 32-bit signed integer hash code.</returns>
    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) => this.GetHashCodeCore(comparer);

    private int GetHashCodeCore(IEqualityComparer comparer)
    {
      if (!(this.Rest is IValueTupleInternal rest))
        return HashCode.Combine<int, int, int, int, int, int, int>(comparer.GetHashCode((object) this.Item1), comparer.GetHashCode((object) this.Item2), comparer.GetHashCode((object) this.Item3), comparer.GetHashCode((object) this.Item4), comparer.GetHashCode((object) this.Item5), comparer.GetHashCode((object) this.Item6), comparer.GetHashCode((object) this.Item7));
      int length = rest.Length;
      int hashCode = rest.GetHashCode(comparer);
      if (length >= 8)
        return hashCode;
      switch (8 - length)
      {
        case 1:
          return HashCode.Combine<int, int>(comparer.GetHashCode((object) this.Item7), hashCode);
        case 2:
          return HashCode.Combine<int, int, int>(comparer.GetHashCode((object) this.Item6), comparer.GetHashCode((object) this.Item7), hashCode);
        case 3:
          return HashCode.Combine<int, int, int, int>(comparer.GetHashCode((object) this.Item5), comparer.GetHashCode((object) this.Item6), comparer.GetHashCode((object) this.Item7), hashCode);
        case 4:
          return HashCode.Combine<int, int, int, int, int>(comparer.GetHashCode((object) this.Item4), comparer.GetHashCode((object) this.Item5), comparer.GetHashCode((object) this.Item6), comparer.GetHashCode((object) this.Item7), hashCode);
        case 5:
          return HashCode.Combine<int, int, int, int, int, int>(comparer.GetHashCode((object) this.Item3), comparer.GetHashCode((object) this.Item4), comparer.GetHashCode((object) this.Item5), comparer.GetHashCode((object) this.Item6), comparer.GetHashCode((object) this.Item7), hashCode);
        case 6:
          return HashCode.Combine<int, int, int, int, int, int, int>(comparer.GetHashCode((object) this.Item2), comparer.GetHashCode((object) this.Item3), comparer.GetHashCode((object) this.Item4), comparer.GetHashCode((object) this.Item5), comparer.GetHashCode((object) this.Item6), comparer.GetHashCode((object) this.Item7), hashCode);
        case 7:
        case 8:
          return HashCode.Combine<int, int, int, int, int, int, int, int>(comparer.GetHashCode((object) this.Item1), comparer.GetHashCode((object) this.Item2), comparer.GetHashCode((object) this.Item3), comparer.GetHashCode((object) this.Item4), comparer.GetHashCode((object) this.Item5), comparer.GetHashCode((object) this.Item6), comparer.GetHashCode((object) this.Item7), hashCode);
        default:
          return -1;
      }
    }

    int IValueTupleInternal.GetHashCode(IEqualityComparer comparer) => this.GetHashCodeCore(comparer);


    #nullable enable
    /// <summary>Returns a string that represents the value of this <see cref="T:System.ValueTuple`8" /> instance.</summary>
    /// <returns>The string representation of this <see cref="T:System.ValueTuple`8" /> instance.</returns>
    public override string ToString()
    {
      if ((ValueType) this.Rest is IValueTupleInternal)
      {
        string[] strArray = new string[16];
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
            goto label_5;
          }
          else
            local1 = ref local2;
        }
        str1 = local1.ToString();
label_5:
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
            goto label_9;
          }
          else
            local3 = ref local4;
        }
        str2 = local3.ToString();
label_9:
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
            goto label_13;
          }
          else
            local5 = ref local6;
        }
        str3 = local5.ToString();
label_13:
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
            goto label_17;
          }
          else
            local7 = ref local8;
        }
        str4 = local7.ToString();
label_17:
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
            goto label_21;
          }
          else
            local9 = ref local10;
        }
        str5 = local9.ToString();
label_21:
        strArray[9] = str5;
        strArray[10] = ", ";
        // ISSUE: explicit reference operation
        ref T6 local11 = @this.Item6;
        string str6;
        if ((object) default (T6) == null)
        {
          T6 obj = local11;
          ref T6 local12 = ref obj;
          if ((object) obj == null)
          {
            str6 = (string) null;
            goto label_25;
          }
          else
            local11 = ref local12;
        }
        str6 = local11.ToString();
label_25:
        strArray[11] = str6;
        strArray[12] = ", ";
        // ISSUE: explicit reference operation
        ref T7 local13 = @this.Item7;
        string str7;
        if ((object) default (T7) == null)
        {
          T7 obj = local13;
          ref T7 local14 = ref obj;
          if ((object) obj == null)
          {
            str7 = (string) null;
            goto label_29;
          }
          else
            local13 = ref local14;
        }
        str7 = local13.ToString();
label_29:
        strArray[13] = str7;
        strArray[14] = ", ";
        strArray[15] = ((IValueTupleInternal) this.Rest).ToStringEnd();
        return string.Concat(strArray);
      }
      string[] strArray1 = new string[17];
      strArray1[0] = "(";
      // ISSUE: explicit reference operation
      ref T1 local15 = @this.Item1;
      string str8;
      if ((object) default (T1) == null)
      {
        T1 obj = local15;
        ref T1 local16 = ref obj;
        if ((object) obj == null)
        {
          str8 = (string) null;
          goto label_34;
        }
        else
          local15 = ref local16;
      }
      str8 = local15.ToString();
label_34:
      strArray1[1] = str8;
      strArray1[2] = ", ";
      // ISSUE: explicit reference operation
      ref T2 local17 = @this.Item2;
      string str9;
      if ((object) default (T2) == null)
      {
        T2 obj = local17;
        ref T2 local18 = ref obj;
        if ((object) obj == null)
        {
          str9 = (string) null;
          goto label_38;
        }
        else
          local17 = ref local18;
      }
      str9 = local17.ToString();
label_38:
      strArray1[3] = str9;
      strArray1[4] = ", ";
      // ISSUE: explicit reference operation
      ref T3 local19 = @this.Item3;
      string str10;
      if ((object) default (T3) == null)
      {
        T3 obj = local19;
        ref T3 local20 = ref obj;
        if ((object) obj == null)
        {
          str10 = (string) null;
          goto label_42;
        }
        else
          local19 = ref local20;
      }
      str10 = local19.ToString();
label_42:
      strArray1[5] = str10;
      strArray1[6] = ", ";
      // ISSUE: explicit reference operation
      ref T4 local21 = @this.Item4;
      string str11;
      if ((object) default (T4) == null)
      {
        T4 obj = local21;
        ref T4 local22 = ref obj;
        if ((object) obj == null)
        {
          str11 = (string) null;
          goto label_46;
        }
        else
          local21 = ref local22;
      }
      str11 = local21.ToString();
label_46:
      strArray1[7] = str11;
      strArray1[8] = ", ";
      // ISSUE: explicit reference operation
      ref T5 local23 = @this.Item5;
      string str12;
      if ((object) default (T5) == null)
      {
        T5 obj = local23;
        ref T5 local24 = ref obj;
        if ((object) obj == null)
        {
          str12 = (string) null;
          goto label_50;
        }
        else
          local23 = ref local24;
      }
      str12 = local23.ToString();
label_50:
      strArray1[9] = str12;
      strArray1[10] = ", ";
      // ISSUE: explicit reference operation
      ref T6 local25 = @this.Item6;
      string str13;
      if ((object) default (T6) == null)
      {
        T6 obj = local25;
        ref T6 local26 = ref obj;
        if ((object) obj == null)
        {
          str13 = (string) null;
          goto label_54;
        }
        else
          local25 = ref local26;
      }
      str13 = local25.ToString();
label_54:
      strArray1[11] = str13;
      strArray1[12] = ", ";
      // ISSUE: explicit reference operation
      ref T7 local27 = @this.Item7;
      string str14;
      if ((object) default (T7) == null)
      {
        T7 obj = local27;
        ref T7 local28 = ref obj;
        if ((object) obj == null)
        {
          str14 = (string) null;
          goto label_58;
        }
        else
          local27 = ref local28;
      }
      str14 = local27.ToString();
label_58:
      strArray1[13] = str14;
      strArray1[14] = ", ";
      strArray1[15] = this.Rest.ToString();
      strArray1[16] = ")";
      return string.Concat(strArray1);
    }


    #nullable disable
    string IValueTupleInternal.ToStringEnd()
    {
      if ((ValueType) this.Rest is IValueTupleInternal)
      {
        string[] strArray = new string[15];
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
            goto label_5;
          }
          else
            local1 = ref local2;
        }
        str1 = local1.ToString();
label_5:
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
            goto label_9;
          }
          else
            local3 = ref local4;
        }
        str2 = local3.ToString();
label_9:
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
            goto label_13;
          }
          else
            local5 = ref local6;
        }
        str3 = local5.ToString();
label_13:
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
            goto label_17;
          }
          else
            local7 = ref local8;
        }
        str4 = local7.ToString();
label_17:
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
            goto label_21;
          }
          else
            local9 = ref local10;
        }
        str5 = local9.ToString();
label_21:
        strArray[8] = str5;
        strArray[9] = ", ";
        // ISSUE: explicit reference operation
        ref T6 local11 = @this.Item6;
        string str6;
        if ((object) default (T6) == null)
        {
          T6 obj = local11;
          ref T6 local12 = ref obj;
          if ((object) obj == null)
          {
            str6 = (string) null;
            goto label_25;
          }
          else
            local11 = ref local12;
        }
        str6 = local11.ToString();
label_25:
        strArray[10] = str6;
        strArray[11] = ", ";
        // ISSUE: explicit reference operation
        ref T7 local13 = @this.Item7;
        string str7;
        if ((object) default (T7) == null)
        {
          T7 obj = local13;
          ref T7 local14 = ref obj;
          if ((object) obj == null)
          {
            str7 = (string) null;
            goto label_29;
          }
          else
            local13 = ref local14;
        }
        str7 = local13.ToString();
label_29:
        strArray[12] = str7;
        strArray[13] = ", ";
        strArray[14] = ((IValueTupleInternal) this.Rest).ToStringEnd();
        return string.Concat(strArray);
      }
      string[] strArray1 = new string[16];
      // ISSUE: explicit reference operation
      ref T1 local15 = @this.Item1;
      string str8;
      if ((object) default (T1) == null)
      {
        T1 obj = local15;
        ref T1 local16 = ref obj;
        if ((object) obj == null)
        {
          str8 = (string) null;
          goto label_34;
        }
        else
          local15 = ref local16;
      }
      str8 = local15.ToString();
label_34:
      strArray1[0] = str8;
      strArray1[1] = ", ";
      // ISSUE: explicit reference operation
      ref T2 local17 = @this.Item2;
      string str9;
      if ((object) default (T2) == null)
      {
        T2 obj = local17;
        ref T2 local18 = ref obj;
        if ((object) obj == null)
        {
          str9 = (string) null;
          goto label_38;
        }
        else
          local17 = ref local18;
      }
      str9 = local17.ToString();
label_38:
      strArray1[2] = str9;
      strArray1[3] = ", ";
      // ISSUE: explicit reference operation
      ref T3 local19 = @this.Item3;
      string str10;
      if ((object) default (T3) == null)
      {
        T3 obj = local19;
        ref T3 local20 = ref obj;
        if ((object) obj == null)
        {
          str10 = (string) null;
          goto label_42;
        }
        else
          local19 = ref local20;
      }
      str10 = local19.ToString();
label_42:
      strArray1[4] = str10;
      strArray1[5] = ", ";
      // ISSUE: explicit reference operation
      ref T4 local21 = @this.Item4;
      string str11;
      if ((object) default (T4) == null)
      {
        T4 obj = local21;
        ref T4 local22 = ref obj;
        if ((object) obj == null)
        {
          str11 = (string) null;
          goto label_46;
        }
        else
          local21 = ref local22;
      }
      str11 = local21.ToString();
label_46:
      strArray1[6] = str11;
      strArray1[7] = ", ";
      // ISSUE: explicit reference operation
      ref T5 local23 = @this.Item5;
      string str12;
      if ((object) default (T5) == null)
      {
        T5 obj = local23;
        ref T5 local24 = ref obj;
        if ((object) obj == null)
        {
          str12 = (string) null;
          goto label_50;
        }
        else
          local23 = ref local24;
      }
      str12 = local23.ToString();
label_50:
      strArray1[8] = str12;
      strArray1[9] = ", ";
      // ISSUE: explicit reference operation
      ref T6 local25 = @this.Item6;
      string str13;
      if ((object) default (T6) == null)
      {
        T6 obj = local25;
        ref T6 local26 = ref obj;
        if ((object) obj == null)
        {
          str13 = (string) null;
          goto label_54;
        }
        else
          local25 = ref local26;
      }
      str13 = local25.ToString();
label_54:
      strArray1[10] = str13;
      strArray1[11] = ", ";
      // ISSUE: explicit reference operation
      ref T7 local27 = @this.Item7;
      string str14;
      if ((object) default (T7) == null)
      {
        T7 obj = local27;
        ref T7 local28 = ref obj;
        if ((object) obj == null)
        {
          str14 = (string) null;
          goto label_58;
        }
        else
          local27 = ref local28;
      }
      str14 = local27.ToString();
label_58:
      strArray1[12] = str14;
      strArray1[13] = ", ";
      strArray1[14] = this.Rest.ToString();
      strArray1[15] = ")";
      return string.Concat(strArray1);
    }

    /// <summary>Gets the number of elements in the <see langword="ValueTuple" />.</summary>
    /// <returns>The number of elements in the <see langword="ValueTuple" />.</returns>
    int ITuple.Length => !((ValueType) this.Rest is IValueTupleInternal) ? 8 : 7 + ((ITuple) this.Rest).Length;


    #nullable enable
    /// <summary>Gets the value of the specified <see langword="ValueTuple" /> element.</summary>
    /// <param name="index">The value of the specified <see langword="ValueTuple" /> element. <paramref name="index" /> can range from 0 for <see langword="Item1" /> to one less than the number of elements in the <see langword="ValueTuple" />.</param>
    /// <exception cref="T:System.IndexOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> is greater than or equal to <see cref="P:System.ValueTuple`8.System#Runtime#CompilerServices#ITuple#Length" />.</exception>
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
          case 5:
            return (object) this.Item6;
          case 6:
            return (object) this.Item7;
          default:
            if ((ValueType) this.Rest is IValueTupleInternal)
              return ((ITuple) this.Rest)[index - 7];
            if (index == 7)
              return (object) this.Rest;
            throw new IndexOutOfRangeException();
        }
      }
    }
  }
}
