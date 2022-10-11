// Decompiled with JetBrains decompiler
// Type: System.UIntPtr
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>A platform-specific type that is used to represent a pointer or a handle.</summary>
  [CLSCompliant(false)]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct UIntPtr : 
    IEquatable<UIntPtr>,
    IComparable,
    IComparable<UIntPtr>,
    ISpanFormattable,
    IFormattable,
    ISerializable,
    IBinaryInteger<UIntPtr>,
    IBinaryNumber<UIntPtr>,
    IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>,
    INumber<UIntPtr>,
    IAdditionOperators<UIntPtr, UIntPtr, UIntPtr>,
    IAdditiveIdentity<UIntPtr, UIntPtr>,
    IComparisonOperators<UIntPtr, UIntPtr>,
    IEqualityOperators<UIntPtr, UIntPtr>,
    IDecrementOperators<UIntPtr>,
    IDivisionOperators<UIntPtr, UIntPtr, UIntPtr>,
    IIncrementOperators<UIntPtr>,
    IModulusOperators<UIntPtr, UIntPtr, UIntPtr>,
    IMultiplicativeIdentity<UIntPtr, UIntPtr>,
    IMultiplyOperators<UIntPtr, UIntPtr, UIntPtr>,
    ISpanParseable<UIntPtr>,
    IParseable<UIntPtr>,
    ISubtractionOperators<UIntPtr, UIntPtr, UIntPtr>,
    IUnaryNegationOperators<UIntPtr, UIntPtr>,
    IUnaryPlusOperators<UIntPtr, UIntPtr>,
    IShiftOperators<UIntPtr, UIntPtr>,
    IMinMaxValue<UIntPtr>,
    IUnsignedNumber<UIntPtr>
  {

    #nullable disable
    private readonly unsafe void* _value;
    /// <summary>A read-only field that represents a pointer or handle that has been initialized to zero.</summary>
    [Intrinsic]
    public static readonly UIntPtr Zero;

    /// <summary>Initializes a new instance of the <see cref="T:System.UIntPtr" /> structure using the specified 32-bit pointer or handle.</summary>
    /// <param name="value">A pointer or handle contained in a 32-bit unsigned integer.</param>
    [NonVersionable]
    public unsafe UIntPtr(uint value) => this._value = (void*) value;

    /// <summary>Initializes a new instance of <see cref="T:System.UIntPtr" /> using the specified 64-bit pointer or handle.</summary>
    /// <param name="value">A pointer or handle contained in a 64-bit unsigned integer.</param>
    /// <exception cref="T:System.OverflowException">On a 32-bit platform, <paramref name="value" /> is too large to represent as an <see cref="T:System.UIntPtr" />.</exception>
    [NonVersionable]
    public unsafe UIntPtr(ulong value) => this._value = (void*) value;


    #nullable enable
    /// <summary>Initializes a new instance of <see cref="T:System.UIntPtr" /> using the specified pointer to an unspecified type.</summary>
    /// <param name="value">A pointer to an unspecified type.</param>
    [NonVersionable]
    public unsafe UIntPtr(void* value) => this._value = value;


    #nullable disable
    private unsafe UIntPtr(SerializationInfo info, StreamingContext context)
    {
      ulong uint64 = info.GetUInt64("value");
      if (UIntPtr.Size != 4)
        ;
      this._value = (void*) uint64;
    }

    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize the current <see cref="T:System.UIntPtr" /> object.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to populate with data.</param>
    /// <param name="context">The destination for this serialization. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      info.AddValue("value", this.ToUInt64());
    }


    #nullable enable
    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.UIntPtr" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override unsafe bool Equals([NotNullWhen(true)] object? obj) => obj is UIntPtr num && this._value == num._value;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override unsafe int GetHashCode()
    {
      ulong num = (ulong) this._value;
      return (int) num ^ (int) (num >> 32);
    }

    /// <summary>Converts the value of this instance to a 32-bit unsigned integer.</summary>
    /// <exception cref="T:System.OverflowException">On a 64-bit platform, the value of this instance is too large to represent as a 32-bit unsigned integer.</exception>
    /// <returns>A 32-bit unsigned integer equal to the value of this instance.</returns>
    [NonVersionable]
    public unsafe uint ToUInt32() => checked ((uint) (UIntPtr) this._value);

    /// <summary>Converts the value of this instance to a 64-bit unsigned integer.</summary>
    /// <returns>A 64-bit unsigned integer equal to the value of this instance.</returns>
    [NonVersionable]
    public unsafe ulong ToUInt64() => (ulong) this._value;

    /// <summary>Converts the value of a 32-bit unsigned integer to an <see cref="T:System.UIntPtr" />.</summary>
    /// <param name="value">A 32-bit unsigned integer.</param>
    /// <returns>A new instance of <see cref="T:System.UIntPtr" /> initialized to <paramref name="value" />.</returns>
    [NonVersionable]
    public static explicit operator UIntPtr(uint value) => new UIntPtr(value);

    /// <summary>Converts the value of a 64-bit unsigned integer to an <see cref="T:System.UIntPtr" />.</summary>
    /// <param name="value">A 64-bit unsigned integer.</param>
    /// <exception cref="T:System.OverflowException">On a 32-bit platform, <paramref name="value" /> is too large to represent as an <see cref="T:System.UIntPtr" />.</exception>
    /// <returns>A new instance of <see cref="T:System.UIntPtr" /> initialized to <paramref name="value" />.</returns>
    [NonVersionable]
    public static explicit operator UIntPtr(ulong value) => new UIntPtr(value);

    /// <summary>Converts the specified pointer to an unspecified type to an <see cref="T:System.UIntPtr" />.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">A pointer to an unspecified type.</param>
    /// <returns>A new instance of <see cref="T:System.UIntPtr" /> initialized to <paramref name="value" />.</returns>
    [NonVersionable]
    public static unsafe explicit operator UIntPtr(void* value) => new UIntPtr(value);

    /// <summary>Converts the value of the specified <see cref="T:System.UIntPtr" /> to a pointer to an unspecified type.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The pointer or handle to convert.</param>
    /// <returns>The contents of <paramref name="value" />.</returns>
    [NonVersionable]
    public static unsafe explicit operator void*(UIntPtr value) => value._value;

    /// <summary>Converts the value of the specified <see cref="T:System.UIntPtr" /> to a 32-bit unsigned integer.</summary>
    /// <param name="value">The pointer or handle to convert.</param>
    /// <exception cref="T:System.OverflowException">On a 64-bit platform, the value of <paramref name="value" /> is too large to represent as a 32-bit unsigned integer.</exception>
    /// <returns>The contents of <paramref name="value" />.</returns>
    [NonVersionable]
    public static unsafe explicit operator uint(UIntPtr value) => checked ((uint) (UIntPtr) value._value);

    /// <summary>Converts the value of the specified <see cref="T:System.UIntPtr" /> to a 64-bit unsigned integer.</summary>
    /// <param name="value">The pointer or handle to convert.</param>
    /// <returns>The contents of <paramref name="value" />.</returns>
    [NonVersionable]
    public static unsafe explicit operator ulong(UIntPtr value) => (ulong) value._value;

    /// <summary>Determines whether two specified instances of <see cref="T:System.UIntPtr" /> are equal.</summary>
    /// <param name="value1">The first pointer or handle to compare.</param>
    /// <param name="value2">The second pointer or handle to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value1" /> equals <paramref name="value2" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static unsafe bool operator ==(UIntPtr value1, UIntPtr value2) => value1._value == value2._value;

    /// <summary>Determines whether two specified instances of <see cref="T:System.UIntPtr" /> are not equal.</summary>
    /// <param name="value1">The first pointer or handle to compare.</param>
    /// <param name="value2">The second pointer or handle to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value1" /> does not equal <paramref name="value2" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static unsafe bool operator !=(UIntPtr value1, UIntPtr value2) => value1._value != value2._value;

    /// <summary>Adds an offset to the value of an unsigned pointer.</summary>
    /// <param name="pointer">The unsigned pointer to add the offset to.</param>
    /// <param name="offset">The offset to add.</param>
    /// <returns>A new unsigned pointer that reflects the addition of <paramref name="offset" /> to <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static UIntPtr Add(UIntPtr pointer, int offset) => pointer + offset;

    /// <summary>Adds an offset to the value of an unsigned pointer.</summary>
    /// <param name="pointer">The unsigned pointer to add the offset to.</param>
    /// <param name="offset">The offset to add.</param>
    /// <returns>A new unsigned pointer that reflects the addition of <paramref name="offset" /> to <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static unsafe UIntPtr operator +(UIntPtr pointer, int offset) => (UIntPtr) pointer._value + (UIntPtr) offset;

    /// <summary>Subtracts an offset from the value of an unsigned pointer.</summary>
    /// <param name="pointer">The unsigned pointer to subtract the offset from.</param>
    /// <param name="offset">The offset to subtract.</param>
    /// <returns>A new unsigned pointer that reflects the subtraction of <paramref name="offset" /> from <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static UIntPtr Subtract(UIntPtr pointer, int offset) => pointer - offset;

    /// <summary>Subtracts an offset from the value of an unsigned pointer.</summary>
    /// <param name="pointer">The unsigned pointer to subtract the offset from.</param>
    /// <param name="offset">The offset to subtract.</param>
    /// <returns>A new unsigned pointer that reflects the subtraction of <paramref name="offset" /> from <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static unsafe UIntPtr operator -(UIntPtr pointer, int offset) => (UIntPtr) pointer._value - (UIntPtr) offset;

    /// <summary>Gets the size of this instance.</summary>
    /// <returns>The size of a pointer or handle on this platform, measured in bytes. The value of this property is 4 on a 32-bit platform, and 8 on a 64-bit platform.</returns>
    public static int Size
    {
      [NonVersionable] get => 8;
    }

    /// <summary>Converts the value of this instance to a pointer to an unspecified type.</summary>
    /// <returns>A pointer to <see cref="T:System.Void" />; that is, a pointer to memory containing data of an unspecified type.</returns>
    [NonVersionable]
    public unsafe void* ToPointer() => this._value;

    /// <summary>Represents the largest possible value of <see cref="T:System.UIntPtr" />.</summary>
    public static UIntPtr MaxValue
    {
      [NonVersionable] get => (UIntPtr) ulong.MaxValue;
    }

    /// <summary>Represents the smallest possible value of <see cref="T:System.UIntPtr" />.</summary>
    public static UIntPtr MinValue
    {
      [NonVersionable] get => (UIntPtr) 0UL;
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="obj" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="obj" /> in the sort order.</description></item></list></returns>
    public unsafe int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is UIntPtr num))
        throw new ArgumentException(SR.Arg_MustBeUIntPtr);
      if ((UIntPtr) this._value < num)
        return -1;
      return (UIntPtr) this._value > num ? 1 : 0;
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="value">An unsigned native integer to compare.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
    public unsafe int CompareTo(UIntPtr value) => ((ulong) this._value).CompareTo((ulong) value);

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public unsafe bool Equals(UIntPtr other) => (IntPtr) this._value == (IntPtr) other;

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance.</returns>
    public override unsafe string ToString() => ((ulong) this._value).ToString();

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid or not supported.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public unsafe string ToString(string? format) => ((ulong) this._value).ToString(format);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public unsafe string ToString(IFormatProvider? provider) => ((ulong) this._value).ToString(provider);

    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use.
    /// -or-
    /// A <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid or not supported.</exception>
    /// <returns>The value of the current instance in the specified format.</returns>
    public unsafe string ToString(string? format, IFormatProvider? provider) => ((ulong) this._value).ToString(format, provider);

    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public unsafe bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      return ((ulong) this._value).TryFormat(destination, out charsWritten, format, provider);
    }

    /// <summary>Converts the string representation of a number to its unsigned native integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />.</exception>
    /// <returns>An unsigned native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static UIntPtr Parse(string s) => (UIntPtr) ulong.Parse(s);

    /// <summary>Converts the string representation of a number in a specified style to its unsigned native integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value or <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />.</exception>
    /// <returns>An unsigned native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static UIntPtr Parse(string s, NumberStyles style) => (UIntPtr) ulong.Parse(s, style);

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its unsigned native integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />.</exception>
    /// <returns>An unsigned native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static UIntPtr Parse(string s, IFormatProvider? provider) => (UIntPtr) ulong.Parse(s, provider);

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its unsigned native integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value or <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />.</exception>
    /// <returns>An unsigned native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static UIntPtr Parse(string s, NumberStyles style, IFormatProvider? provider) => (UIntPtr) ulong.Parse(s, style, provider);

    /// <summary>Converts the read-only span of characters representation of a number in optionally specified style and optionally specified culture-specific format to its unsigned native integer equivalent.</summary>
    /// <param name="s">A read-only span of characters containing a number to convert.</param>
    /// <param name="style">An optional bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. The default value is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information about <paramref name="s" />. The default value is <see langword="default" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value or <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />.</exception>
    /// <returns>An unsigned native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static UIntPtr Parse(
      ReadOnlySpan<char> s,
      NumberStyles style = NumberStyles.Integer,
      IFormatProvider? provider = null)
    {
      return (UIntPtr) ulong.Parse(s, style, provider);
    }

    /// <summary>Converts the string representation of a number to its unsigned native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the unsigned native integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or empty, is not of the correct format, or represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out UIntPtr result)
    {
      Unsafe.SkipInit<UIntPtr>(out result);
      return ulong.TryParse(s, out Unsafe.As<UIntPtr, ulong>(ref result));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its unsigned native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the unsigned native integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or empty, is not of the correct format, or represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      NumberStyles style,
      IFormatProvider? provider,
      out UIntPtr result)
    {
      Unsafe.SkipInit<UIntPtr>(out result);
      return ulong.TryParse(s, style, provider, out Unsafe.As<UIntPtr, ulong>(ref result));
    }

    /// <summary>Converts the read-only span of characters representation of a number to its unsigned native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A read-only span of characters containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the unsigned native integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty, is not of the correct format, or represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out UIntPtr result)
    {
      Unsafe.SkipInit<UIntPtr>(out result);
      return ulong.TryParse(s, out Unsafe.As<UIntPtr, ulong>(ref result));
    }

    /// <summary>Converts the read-only span of characters representation of a number in a specified style and culture-specific format to its unsigned native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A read-only span of characters containing a number to convert. The span is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the unsigned native integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty, is not of the correct format, or represents a number less than <see cref="P:System.UIntPtr.MinValue" /> or greater than <see cref="P:System.UIntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out UIntPtr result)
    {
      Unsafe.SkipInit<UIntPtr>(out result);
      return ulong.TryParse(s, style, provider, out Unsafe.As<UIntPtr, ulong>(ref result));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IAdditionOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIAdditionOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_Addition(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left + right;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    UIntPtr IAdditiveIdentity<UIntPtr, UIntPtr>.System\u002EIAdditiveIdentity\u003Cnuint\u002Cnuint\u003E\u002EAdditiveIdentity
    {
      [return: NativeInteger] get => UIntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBinaryInteger<UIntPtr>.System\u002EIBinaryInteger\u003Cnuint\u003E\u002ELeadingZeroCount(
      [NativeInteger] UIntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (UIntPtr) BitOperations.LeadingZeroCount((ulong) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBinaryInteger<UIntPtr>.System\u002EIBinaryInteger\u003Cnuint\u003E\u002EPopCount(
      [NativeInteger] UIntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (UIntPtr) BitOperations.PopCount((ulong) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBinaryInteger<UIntPtr>.System\u002EIBinaryInteger\u003Cnuint\u003E\u002ERotateLeft(
      [NativeInteger] UIntPtr value,
      int rotateAmount)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (UIntPtr) BitOperations.RotateLeft((ulong) value, rotateAmount);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBinaryInteger<UIntPtr>.System\u002EIBinaryInteger\u003Cnuint\u003E\u002ERotateRight(
      [NativeInteger] UIntPtr value,
      int rotateAmount)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (UIntPtr) BitOperations.RotateRight((ulong) value, rotateAmount);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBinaryInteger<UIntPtr>.System\u002EIBinaryInteger\u003Cnuint\u003E\u002ETrailingZeroCount(
      [NativeInteger] UIntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (UIntPtr) BitOperations.TrailingZeroCount((ulong) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<UIntPtr>.System\u002EIBinaryNumber\u003Cnuint\u003E\u002EIsPow2(
      [NativeInteger] UIntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return BitOperations.IsPow2((ulong) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBinaryNumber<UIntPtr>.System\u002EIBinaryNumber\u003Cnuint\u003E\u002ELog2(
      [NativeInteger] UIntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (UIntPtr) BitOperations.Log2((ulong) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIBitwiseOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_BitwiseAnd(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left & right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIBitwiseOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_BitwiseOr(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left | right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIBitwiseOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_ExclusiveOr(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left ^ right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IBitwiseOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIBitwiseOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_OnesComplement(
      [NativeInteger] UIntPtr value)
    {
      return (UIntPtr) ~(IntPtr) value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<UIntPtr, UIntPtr>.System\u002EIComparisonOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_LessThan(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<UIntPtr, UIntPtr>.System\u002EIComparisonOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_LessThanOrEqual(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<UIntPtr, UIntPtr>.System\u002EIComparisonOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_GreaterThan(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<UIntPtr, UIntPtr>.System\u002EIComparisonOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_GreaterThanOrEqual(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IDecrementOperators<UIntPtr>.System\u002EIDecrementOperators\u003Cnuint\u003E\u002Eop_Decrement(
      [NativeInteger] UIntPtr value)
    {
      return --value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IDivisionOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIDivisionOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_Division(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<UIntPtr, UIntPtr>.System\u002EIEqualityOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_Equality(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return (IntPtr) left == (IntPtr) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<UIntPtr, UIntPtr>.System\u002EIEqualityOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_Inequality(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return (IntPtr) left != (IntPtr) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IIncrementOperators<UIntPtr>.System\u002EIIncrementOperators\u003Cnuint\u003E\u002Eop_Increment(
      [NativeInteger] UIntPtr value)
    {
      return ++value;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    UIntPtr IMinMaxValue<UIntPtr>.System\u002EIMinMaxValue\u003Cnuint\u003E\u002EMinValue
    {
      [return: NativeInteger] get => UIntPtr.MinValue;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    UIntPtr IMinMaxValue<UIntPtr>.System\u002EIMinMaxValue\u003Cnuint\u003E\u002EMaxValue
    {
      [return: NativeInteger] get => UIntPtr.MaxValue;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IModulusOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIModulusOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_Modulus(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left % right;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    UIntPtr IMultiplicativeIdentity<UIntPtr, UIntPtr>.System\u002EIMultiplicativeIdentity\u003Cnuint\u002Cnuint\u003E\u002EMultiplicativeIdentity
    {
      [return: NativeInteger] get => new UIntPtr(1);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IMultiplyOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EIMultiplyOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_Multiply(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left * right;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EOne
    {
      [return: NativeInteger] get => new UIntPtr(1);
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EZero
    {
      [return: NativeInteger] get => UIntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EAbs(
      [NativeInteger] UIntPtr value)
    {
      return value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EClamp(
      [NativeInteger] UIntPtr value,
      [NativeInteger] UIntPtr min,
      [NativeInteger] UIntPtr max)
    {
      return Math.Clamp(value, min, max);
    }


    #nullable disable
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002ECreate<TOther>(
      TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (UIntPtr) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (UIntPtr) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return checked ((UIntPtr) (ulong) (Decimal) (object) value);
      if (typeof (TOther) == typeof (double))
        return checked ((UIntPtr) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return checked ((UIntPtr) (uint) (short) (object) value);
      if (typeof (TOther) == typeof (int))
        return checked ((UIntPtr) (uint) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((UIntPtr) (ulong) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((UIntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return checked ((UIntPtr) (uint) (sbyte) (object) value);
      if (typeof (TOther) == typeof (float))
        return checked ((UIntPtr) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (UIntPtr) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (UIntPtr) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return checked ((UIntPtr) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return UIntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002ECreateSaturating<TOther>(
      TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (UIntPtr) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (UIntPtr) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > (Decimal) (ulong) UIntPtr.MaxValue)
          return UIntPtr.MaxValue;
        return !(num < 0M) ? (UIntPtr) (ulong) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) (IntPtr) UIntPtr.MaxValue)
          return UIntPtr.MaxValue;
        return num >= 0.0 ? (UIntPtr) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        return num >= (short) 0 ? (UIntPtr) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        return num >= 0 ? (UIntPtr) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (UIntPtr.Size != 4)
          ;
        return num >= 0L ? (UIntPtr) (ulong) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        return num >= IntPtr.Zero ? (UIntPtr) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        return num >= (sbyte) 0 ? (UIntPtr) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) (IntPtr) UIntPtr.MaxValue)
          return UIntPtr.MaxValue;
        return (double) num >= 0.0 ? (UIntPtr) num : UIntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (ushort))
        return (UIntPtr) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (UIntPtr) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) UIntPtr.MaxValue ? (UIntPtr) num : UIntPtr.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
        return (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return UIntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002ECreateTruncating<TOther>(
      TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (UIntPtr) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (UIntPtr) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (UIntPtr) (ulong) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (UIntPtr) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (UIntPtr) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (UIntPtr) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (UIntPtr) (ulong) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (UIntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (UIntPtr) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (UIntPtr) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (UIntPtr) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (UIntPtr) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (UIntPtr) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return UIntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger(new bool[] {true, true})]
    (UIntPtr Quotient, UIntPtr Remainder) INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EDivRem(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return Math.DivRem(left, right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EMax(
      [NativeInteger] UIntPtr x,
      [NativeInteger] UIntPtr y)
    {
      return Math.Max(x, y);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EMin(
      [NativeInteger] UIntPtr x,
      [NativeInteger] UIntPtr y)
    {
      return Math.Min(x, y);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EParse(
      string s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return UIntPtr.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002EParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return UIntPtr.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002ESign(
      [NativeInteger] UIntPtr value)
    {
      return (IntPtr) value == IntPtr.Zero ? UIntPtr.Zero : new UIntPtr(1);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002ETryCreate<TOther>(
      TOther value,
      [NativeInteger] out UIntPtr result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (UIntPtr) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (UIntPtr) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < 0M || num > (Decimal) (ulong) UIntPtr.MaxValue)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < 0.0 || num > (double) (IntPtr) UIntPtr.MaxValue)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num < (short) 0)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < 0)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < 0L || UIntPtr.Size == 4)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < IntPtr.Zero)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        if (num < (sbyte) 0)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < 0.0 || (double) num > (double) (IntPtr) UIntPtr.MaxValue)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (UIntPtr) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (UIntPtr) (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) UIntPtr.MaxValue)
        {
          result = UIntPtr.Zero;
          return false;
        }
        result = (UIntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        result = (UIntPtr) (object) value;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = UIntPtr.Zero;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002ETryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      [NativeInteger] out UIntPtr result)
    {
      return UIntPtr.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<UIntPtr>.System\u002EINumber\u003Cnuint\u003E\u002ETryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      [NativeInteger] out UIntPtr result)
    {
      return UIntPtr.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IParseable<UIntPtr>.System\u002EIParseable\u003Cnuint\u003E\u002EParse(
      string s,
      IFormatProvider provider)
    {
      return UIntPtr.Parse(s, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<UIntPtr>.System\u002EIParseable\u003Cnuint\u003E\u002ETryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      [NativeInteger] out UIntPtr result)
    {
      return UIntPtr.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IShiftOperators<UIntPtr, UIntPtr>.System\u002EIShiftOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_LeftShift(
      [NativeInteger] UIntPtr value,
      int shiftAmount)
    {
      return value << shiftAmount;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IShiftOperators<UIntPtr, UIntPtr>.System\u002EIShiftOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_RightShift(
      [NativeInteger] UIntPtr value,
      int shiftAmount)
    {
      return value >> shiftAmount;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr ISpanParseable<UIntPtr>.System\u002EISpanParseable\u003Cnuint\u003E\u002EParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return UIntPtr.Parse(s, NumberStyles.Integer, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<UIntPtr>.System\u002EISpanParseable\u003Cnuint\u003E\u002ETryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      [NativeInteger] out UIntPtr result)
    {
      return UIntPtr.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr ISubtractionOperators<UIntPtr, UIntPtr, UIntPtr>.System\u002EISubtractionOperators\u003Cnuint\u002Cnuint\u002Cnuint\u003E\u002Eop_Subtraction(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IUnaryNegationOperators<UIntPtr, UIntPtr>.System\u002EIUnaryNegationOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_UnaryNegation(
      [NativeInteger] UIntPtr value)
    {
      return UIntPtr.Zero - value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    UIntPtr IUnaryPlusOperators<UIntPtr, UIntPtr>.System\u002EIUnaryPlusOperators\u003Cnuint\u002Cnuint\u003E\u002Eop_UnaryPlus(
      [NativeInteger] UIntPtr value)
    {
      return value;
    }
  }
}
