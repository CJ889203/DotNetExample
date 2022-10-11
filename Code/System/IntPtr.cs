// Decompiled with JetBrains decompiler
// Type: System.IntPtr
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
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct IntPtr : 
    IEquatable<IntPtr>,
    IComparable,
    IComparable<IntPtr>,
    ISpanFormattable,
    IFormattable,
    ISerializable,
    IBinaryInteger<IntPtr>,
    IBinaryNumber<IntPtr>,
    IBitwiseOperators<IntPtr, IntPtr, IntPtr>,
    INumber<IntPtr>,
    IAdditionOperators<IntPtr, IntPtr, IntPtr>,
    IAdditiveIdentity<IntPtr, IntPtr>,
    IComparisonOperators<IntPtr, IntPtr>,
    IEqualityOperators<IntPtr, IntPtr>,
    IDecrementOperators<IntPtr>,
    IDivisionOperators<IntPtr, IntPtr, IntPtr>,
    IIncrementOperators<IntPtr>,
    IModulusOperators<IntPtr, IntPtr, IntPtr>,
    IMultiplicativeIdentity<IntPtr, IntPtr>,
    IMultiplyOperators<IntPtr, IntPtr, IntPtr>,
    ISpanParseable<IntPtr>,
    IParseable<IntPtr>,
    ISubtractionOperators<IntPtr, IntPtr, IntPtr>,
    IUnaryNegationOperators<IntPtr, IntPtr>,
    IUnaryPlusOperators<IntPtr, IntPtr>,
    IShiftOperators<IntPtr, IntPtr>,
    IMinMaxValue<IntPtr>,
    ISignedNumber<IntPtr>
  {

    #nullable disable
    private readonly unsafe void* _value;
    /// <summary>A read-only field that represents a pointer or handle that has been initialized to zero.</summary>
    [Intrinsic]
    public static readonly IntPtr Zero;

    /// <summary>Initializes a new instance of <see cref="T:System.IntPtr" /> using the specified 32-bit pointer or handle.</summary>
    /// <param name="value">A pointer or handle contained in a 32-bit signed integer.</param>
    [NonVersionable]
    public unsafe IntPtr(int value) => this._value = (void*) value;

    /// <summary>Initializes a new instance of <see cref="T:System.IntPtr" /> using the specified 64-bit pointer.</summary>
    /// <param name="value">A pointer or handle contained in a 64-bit signed integer.</param>
    /// <exception cref="T:System.OverflowException">On a 32-bit platform, <paramref name="value" /> is too large or too small to represent as an <see cref="T:System.IntPtr" />.</exception>
    [NonVersionable]
    public unsafe IntPtr(long value) => this._value = (void*) value;


    #nullable enable
    /// <summary>Initializes a new instance of <see cref="T:System.IntPtr" /> using the specified pointer to an unspecified type.</summary>
    /// <param name="value">A pointer to an unspecified type.</param>
    [CLSCompliant(false)]
    [NonVersionable]
    public unsafe IntPtr(void* value) => this._value = value;


    #nullable disable
    private unsafe IntPtr(SerializationInfo info, StreamingContext context)
    {
      long int64 = info.GetInt64("value");
      if (IntPtr.Size != 4)
        ;
      this._value = (void*) int64;
    }

    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize the current <see cref="T:System.IntPtr" /> object.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to populate with data.</param>
    /// <param name="context">The destination for this serialization. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      info.AddValue("value", this.ToInt64());
    }


    #nullable enable
    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.IntPtr" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override unsafe bool Equals([NotNullWhen(true)] object? obj) => obj is IntPtr num && this._value == num._value;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override unsafe int GetHashCode()
    {
      long num = (long) this._value;
      return (int) num ^ (int) (num >> 32);
    }

    /// <summary>Converts the value of this instance to a 32-bit signed integer.</summary>
    /// <exception cref="T:System.OverflowException">On a 64-bit platform, the value of this instance is too large or too small to represent as a 32-bit signed integer.</exception>
    /// <returns>A 32-bit signed integer equal to the value of this instance.</returns>
    [NonVersionable]
    public unsafe int ToInt32() => checked ((int) this._value);

    /// <summary>Converts the value of this instance to a 64-bit signed integer.</summary>
    /// <returns>A 64-bit signed integer equal to the value of this instance.</returns>
    [NonVersionable]
    public unsafe long ToInt64() => (long) this._value;

    /// <summary>Converts the value of a 32-bit signed integer to an <see cref="T:System.IntPtr" />.</summary>
    /// <param name="value">A 32-bit signed integer.</param>
    /// <returns>A new instance of <see cref="T:System.IntPtr" /> initialized to <paramref name="value" />.</returns>
    [NonVersionable]
    public static explicit operator IntPtr(int value) => new IntPtr(value);

    /// <summary>Converts the value of a 64-bit signed integer to an <see cref="T:System.IntPtr" />.</summary>
    /// <param name="value">A 64-bit signed integer.</param>
    /// <exception cref="T:System.OverflowException">On a 32-bit platform, <paramref name="value" /> is too large to represent as an <see cref="T:System.IntPtr" />.</exception>
    /// <returns>A new instance of <see cref="T:System.IntPtr" /> initialized to <paramref name="value" />.</returns>
    [NonVersionable]
    public static explicit operator IntPtr(long value) => new IntPtr(value);

    /// <summary>Converts the specified pointer to an unspecified type to an <see cref="T:System.IntPtr" />.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">A pointer to an unspecified type.</param>
    /// <returns>A new instance of <see cref="T:System.IntPtr" /> initialized to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static unsafe explicit operator IntPtr(void* value) => new IntPtr(value);

    /// <summary>Converts the value of the specified <see cref="T:System.IntPtr" /> to a pointer to an unspecified type.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The pointer or handle to convert.</param>
    /// <returns>The contents of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static unsafe explicit operator void*(IntPtr value) => value._value;

    /// <summary>Converts the value of the specified <see cref="T:System.IntPtr" /> to a 32-bit signed integer.</summary>
    /// <param name="value">The pointer or handle to convert.</param>
    /// <exception cref="T:System.OverflowException">On a 64-bit platform, the value of <paramref name="value" /> is too large to represent as a 32-bit signed integer.</exception>
    /// <returns>The contents of <paramref name="value" />.</returns>
    [NonVersionable]
    public static unsafe explicit operator int(IntPtr value) => checked ((int) value._value);

    /// <summary>Converts the value of the specified <see cref="T:System.IntPtr" /> to a 64-bit signed integer.</summary>
    /// <param name="value">The pointer or handle to convert.</param>
    /// <returns>The contents of <paramref name="value" />.</returns>
    [NonVersionable]
    public static unsafe explicit operator long(IntPtr value) => (long) value._value;

    /// <summary>Determines whether two specified instances of <see cref="T:System.IntPtr" /> are equal.</summary>
    /// <param name="value1">The first pointer or handle to compare.</param>
    /// <param name="value2">The second pointer or handle to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value1" /> equals <paramref name="value2" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static unsafe bool operator ==(IntPtr value1, IntPtr value2) => value1._value == value2._value;

    /// <summary>Determines whether two specified instances of <see cref="T:System.IntPtr" /> are not equal.</summary>
    /// <param name="value1">The first pointer or handle to compare.</param>
    /// <param name="value2">The second pointer or handle to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value1" /> does not equal <paramref name="value2" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static unsafe bool operator !=(IntPtr value1, IntPtr value2) => value1._value != value2._value;

    /// <summary>Adds an offset to the value of a pointer.</summary>
    /// <param name="pointer">The pointer to add the offset to.</param>
    /// <param name="offset">The offset to add.</param>
    /// <returns>A new pointer that reflects the addition of <paramref name="offset" /> to <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static IntPtr Add(IntPtr pointer, int offset) => pointer + offset;

    /// <summary>Adds an offset to the value of a pointer.</summary>
    /// <param name="pointer">The pointer to add the offset to.</param>
    /// <param name="offset">The offset to add.</param>
    /// <returns>A new pointer that reflects the addition of <paramref name="offset" /> to <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static unsafe IntPtr operator +(IntPtr pointer, int offset) => (IntPtr) pointer._value + (IntPtr) offset;

    /// <summary>Subtracts an offset from the value of a pointer.</summary>
    /// <param name="pointer">The pointer to subtract the offset from.</param>
    /// <param name="offset">The offset to subtract.</param>
    /// <returns>A new pointer that reflects the subtraction of <paramref name="offset" /> from <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static IntPtr Subtract(IntPtr pointer, int offset) => pointer - offset;

    /// <summary>Subtracts an offset from the value of a pointer.</summary>
    /// <param name="pointer">The pointer to subtract the offset from.</param>
    /// <param name="offset">The offset to subtract.</param>
    /// <returns>A new pointer that reflects the subtraction of <paramref name="offset" /> from <paramref name="pointer" />.</returns>
    [NonVersionable]
    public static unsafe IntPtr operator -(IntPtr pointer, int offset) => (IntPtr) pointer._value - (IntPtr) offset;

    /// <summary>Gets the size of this instance.</summary>
    /// <returns>The size of a pointer or handle in this process, measured in bytes. The value of this property is 4 in a 32-bit process, and 8 in a 64-bit process. You can define the process type by setting the <see langword="/platform" /> switch when you compile your code with the C# and Visual Basic compilers.</returns>
    public static int Size
    {
      [NonVersionable] get => 8;
    }

    /// <summary>Converts the value of this instance to a pointer to an unspecified type.</summary>
    /// <returns>A pointer to <see cref="T:System.Void" />; that is, a pointer to memory containing data of an unspecified type.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public unsafe void* ToPointer() => this._value;

    /// <summary>Represents the largest possible value of <see cref="T:System.IntPtr" />.</summary>
    public static IntPtr MaxValue
    {
      [NonVersionable] get => (IntPtr) long.MaxValue;
    }

    /// <summary>Represents the smallest possible value of <see cref="T:System.IntPtr" />.</summary>
    public static IntPtr MinValue
    {
      [NonVersionable] get => (IntPtr) long.MinValue;
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="obj" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="obj" /> in the sort order.</description></item></list></returns>
    public unsafe int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is IntPtr num))
        throw new ArgumentException(SR.Arg_MustBeIntPtr);
      if ((IntPtr) this._value < num)
        return -1;
      return (IntPtr) this._value > num ? 1 : 0;
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="value">A signed native integer to compare.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
    public unsafe int CompareTo(IntPtr value) => ((long) this._value).CompareTo((long) value);

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to <paramref name="other" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public unsafe bool Equals(IntPtr other) => (long) this._value == (long) other;

    /// <summary>Converts the numeric value of the current <see cref="T:System.IntPtr" /> object to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance.</returns>
    public override unsafe string ToString() => ((long) this._value).ToString();

    /// <summary>Converts the numeric value of the current <see cref="T:System.IntPtr" /> object to its equivalent string representation.</summary>
    /// <param name="format">A format specification that governs how the current <see cref="T:System.IntPtr" /> object is converted.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid or not supported.</exception>
    /// <returns>The string representation of the value of the current <see cref="T:System.IntPtr" /> object.</returns>
    public unsafe string ToString(string? format) => ((long) this._value).ToString(format);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public unsafe string ToString(IFormatProvider? provider) => ((long) this._value).ToString(provider);

    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use.
    /// -or-
    /// A <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The value of the current instance in the specified format.</returns>
    public unsafe string ToString(string? format, IFormatProvider? provider) => ((long) this._value).ToString(format, provider);

    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <param name="destination">The span where this instance's value formatted as a span of characters should be written.</param>
    /// <param name="charsWritten">When this method returns, contains the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">The characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public unsafe bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      return ((long) this._value).TryFormat(destination, out charsWritten, format, provider);
    }

    /// <summary>Converts the string representation of a number to its signed native integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />.</exception>
    /// <returns>A signed native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static IntPtr Parse(string s) => (IntPtr) long.Parse(s);

    /// <summary>Converts the string representation of a number in a specified style to its signed native integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value or <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />.</exception>
    /// <returns>A signed native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static IntPtr Parse(string s, NumberStyles style) => (IntPtr) long.Parse(s, style);

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its signed native integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />.</exception>
    /// <returns>A signed native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static IntPtr Parse(string s, IFormatProvider? provider) => (IntPtr) long.Parse(s, provider);

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its signed native integer equivalent.</summary>
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
    /// <paramref name="s" /> represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />.</exception>
    /// <returns>A signed native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static IntPtr Parse(string s, NumberStyles style, IFormatProvider? provider) => (IntPtr) long.Parse(s, style, provider);

    /// <summary>Converts the read-only span of characters representation of a number in a specified style and culture-specific format to its signed native integer equivalent.</summary>
    /// <param name="s">A read-only span of characters containing a number to convert.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value or <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />.</exception>
    /// <returns>A signed native integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static IntPtr Parse(
      ReadOnlySpan<char> s,
      NumberStyles style = NumberStyles.Integer,
      IFormatProvider? provider = null)
    {
      return (IntPtr) long.Parse(s, style, provider);
    }

    /// <summary>Converts the string representation of a number to its signed native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the signed native integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or empty, is not of the correct format, or represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out IntPtr result)
    {
      Unsafe.SkipInit<IntPtr>(out result);
      return long.TryParse(s, out Unsafe.As<IntPtr, long>(ref result));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its signed native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the signed native integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or empty, is not of the correct format, or represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      NumberStyles style,
      IFormatProvider? provider,
      out IntPtr result)
    {
      Unsafe.SkipInit<IntPtr>(out result);
      return long.TryParse(s, style, provider, out Unsafe.As<IntPtr, long>(ref result));
    }

    /// <summary>Converts the read-only span of characters representation of a number to its signed native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A read-only span of characters containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the signed native integer equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty, is not of the correct format, or represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out IntPtr result)
    {
      Unsafe.SkipInit<IntPtr>(out result);
      return long.TryParse(s, out Unsafe.As<IntPtr, long>(ref result));
    }

    /// <summary>Converts the read-only span of characters representation of a number in a specified style and culture-specific format to its signed native integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A read-only span of characters containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the signed native integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty, is not of the correct format, or represents a number less than <see cref="P:System.IntPtr.MinValue" /> or greater than <see cref="P:System.IntPtr.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out IntPtr result)
    {
      Unsafe.SkipInit<IntPtr>(out result);
      return long.TryParse(s, style, provider, out Unsafe.As<IntPtr, long>(ref result));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IAdditionOperators<IntPtr, IntPtr, IntPtr>.System\u002EIAdditionOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_Addition(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left + right;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    IntPtr IAdditiveIdentity<IntPtr, IntPtr>.System\u002EIAdditiveIdentity\u003Cnint\u002Cnint\u003E\u002EAdditiveIdentity
    {
      [return: NativeInteger] get => IntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBinaryInteger<IntPtr>.System\u002EIBinaryInteger\u003Cnint\u003E\u002ELeadingZeroCount(
      [NativeInteger] IntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (IntPtr) BitOperations.LeadingZeroCount((ulong) (long) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBinaryInteger<IntPtr>.System\u002EIBinaryInteger\u003Cnint\u003E\u002EPopCount(
      [NativeInteger] IntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (IntPtr) BitOperations.PopCount((ulong) (long) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBinaryInteger<IntPtr>.System\u002EIBinaryInteger\u003Cnint\u003E\u002ERotateLeft(
      [NativeInteger] IntPtr value,
      int rotateAmount)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (IntPtr) (long) BitOperations.RotateLeft((ulong) (long) value, rotateAmount);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBinaryInteger<IntPtr>.System\u002EIBinaryInteger\u003Cnint\u003E\u002ERotateRight(
      [NativeInteger] IntPtr value,
      int rotateAmount)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (IntPtr) (long) BitOperations.RotateRight((ulong) (long) value, rotateAmount);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBinaryInteger<IntPtr>.System\u002EIBinaryInteger\u003Cnint\u003E\u002ETrailingZeroCount(
      [NativeInteger] IntPtr value)
    {
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (IntPtr) BitOperations.TrailingZeroCount((ulong) (long) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<IntPtr>.System\u002EIBinaryNumber\u003Cnint\u003E\u002EIsPow2(
      [NativeInteger] IntPtr value)
    {
      return BitOperations.IsPow2(value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBinaryNumber<IntPtr>.System\u002EIBinaryNumber\u003Cnint\u003E\u002ELog2(
      [NativeInteger] IntPtr value)
    {
      if (value < IntPtr.Zero)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      int num = Environment.Is64BitProcess ? 1 : 0;
      return (IntPtr) BitOperations.Log2((ulong) (long) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.System\u002EIBitwiseOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_BitwiseAnd(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left & right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.System\u002EIBitwiseOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_BitwiseOr(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left | right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.System\u002EIBitwiseOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_ExclusiveOr(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left ^ right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IBitwiseOperators<IntPtr, IntPtr, IntPtr>.System\u002EIBitwiseOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_OnesComplement(
      [NativeInteger] IntPtr value)
    {
      return ~value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<IntPtr, IntPtr>.System\u002EIComparisonOperators\u003Cnint\u002Cnint\u003E\u002Eop_LessThan(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<IntPtr, IntPtr>.System\u002EIComparisonOperators\u003Cnint\u002Cnint\u003E\u002Eop_LessThanOrEqual(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<IntPtr, IntPtr>.System\u002EIComparisonOperators\u003Cnint\u002Cnint\u003E\u002Eop_GreaterThan(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<IntPtr, IntPtr>.System\u002EIComparisonOperators\u003Cnint\u002Cnint\u003E\u002Eop_GreaterThanOrEqual(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IDecrementOperators<IntPtr>.System\u002EIDecrementOperators\u003Cnint\u003E\u002Eop_Decrement(
      [NativeInteger] IntPtr value)
    {
      return --value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IDivisionOperators<IntPtr, IntPtr, IntPtr>.System\u002EIDivisionOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_Division(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<IntPtr, IntPtr>.System\u002EIEqualityOperators\u003Cnint\u002Cnint\u003E\u002Eop_Equality(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left == right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<IntPtr, IntPtr>.System\u002EIEqualityOperators\u003Cnint\u002Cnint\u003E\u002Eop_Inequality(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left != right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IIncrementOperators<IntPtr>.System\u002EIIncrementOperators\u003Cnint\u003E\u002Eop_Increment(
      [NativeInteger] IntPtr value)
    {
      return ++value;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    IntPtr IMinMaxValue<IntPtr>.System\u002EIMinMaxValue\u003Cnint\u003E\u002EMinValue
    {
      [return: NativeInteger] get => IntPtr.MinValue;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    IntPtr IMinMaxValue<IntPtr>.System\u002EIMinMaxValue\u003Cnint\u003E\u002EMaxValue
    {
      [return: NativeInteger] get => IntPtr.MaxValue;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IModulusOperators<IntPtr, IntPtr, IntPtr>.System\u002EIModulusOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_Modulus(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left % right;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    IntPtr IMultiplicativeIdentity<IntPtr, IntPtr>.System\u002EIMultiplicativeIdentity\u003Cnint\u002Cnint\u003E\u002EMultiplicativeIdentity
    {
      [return: NativeInteger] get => new IntPtr(1);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IMultiplyOperators<IntPtr, IntPtr, IntPtr>.System\u002EIMultiplyOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_Multiply(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left * right;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EOne
    {
      [return: NativeInteger] get => new IntPtr(1);
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EZero
    {
      [return: NativeInteger] get => IntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EAbs(
      [NativeInteger] IntPtr value)
    {
      return Math.Abs(value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EClamp(
      [NativeInteger] IntPtr value,
      [NativeInteger] IntPtr min,
      [NativeInteger] IntPtr max)
    {
      return Math.Clamp(value, min, max);
    }


    #nullable disable
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002ECreate<TOther>(
      TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (IntPtr) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (IntPtr) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return checked ((IntPtr) (long) (Decimal) (object) value);
      if (typeof (TOther) == typeof (double))
        return checked ((IntPtr) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return (IntPtr) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (IntPtr) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return checked ((IntPtr) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (IntPtr) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return checked ((IntPtr) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (IntPtr) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return checked ((IntPtr) (uint) (object) value);
      if (typeof (TOther) == typeof (ulong))
        return checked ((IntPtr) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((IntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return IntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002ECreateSaturating<TOther>(
      TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (IntPtr) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (IntPtr) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > (Decimal) (long) IntPtr.MaxValue)
          return IntPtr.MaxValue;
        return !(num < (Decimal) (long) IntPtr.MinValue) ? (IntPtr) (long) num : IntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) IntPtr.MaxValue)
          return IntPtr.MaxValue;
        return num >= (double) IntPtr.MinValue ? (IntPtr) num : IntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (short))
        return (IntPtr) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (IntPtr) (int) (object) value;
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) IntPtr.MaxValue)
          return IntPtr.MaxValue;
        return num >= (long) IntPtr.MinValue ? (IntPtr) num : IntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (IntPtr))
        return (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (IntPtr) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) IntPtr.MaxValue)
          return IntPtr.MaxValue;
        return (double) num >= (double) IntPtr.MinValue ? (IntPtr) num : IntPtr.MinValue;
      }
      if (typeof (TOther) == typeof (ushort))
        return (IntPtr) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        return (long) num <= (long) IntPtr.MaxValue ? (IntPtr) num : IntPtr.MaxValue;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) IntPtr.MaxValue ? (IntPtr) (long) num : IntPtr.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= (UIntPtr) IntPtr.MaxValue ? (IntPtr) num : IntPtr.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return IntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002ECreateTruncating<TOther>(
      TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (IntPtr) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (IntPtr) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (IntPtr) (long) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (IntPtr) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (IntPtr) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (IntPtr) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (IntPtr) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (IntPtr) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (IntPtr) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (IntPtr) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (IntPtr) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (IntPtr) (long) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (IntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return IntPtr.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger(new bool[] {true, true})]
    (IntPtr Quotient, IntPtr Remainder) INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EDivRem(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return Math.DivRem(left, right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EMax(
      [NativeInteger] IntPtr x,
      [NativeInteger] IntPtr y)
    {
      return Math.Max(x, y);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EMin(
      [NativeInteger] IntPtr x,
      [NativeInteger] IntPtr y)
    {
      return Math.Min(x, y);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EParse(
      string s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return IntPtr.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002EParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return IntPtr.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002ESign(
      [NativeInteger] IntPtr value)
    {
      return (IntPtr) Math.Sign(value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002ETryCreate<TOther>(
      TOther value,
      [NativeInteger] out IntPtr result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (IntPtr) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (IntPtr) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < (Decimal) (long) IntPtr.MinValue || num > (Decimal) (long) IntPtr.MaxValue)
        {
          result = IntPtr.Zero;
          return false;
        }
        result = (IntPtr) (long) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < (double) IntPtr.MinValue || num > (double) IntPtr.MaxValue)
        {
          result = IntPtr.Zero;
          return false;
        }
        result = (IntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (IntPtr) (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        result = (IntPtr) (int) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < (long) IntPtr.MinValue || num > (long) IntPtr.MaxValue)
        {
          result = IntPtr.Zero;
          return false;
        }
        result = (IntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        result = (IntPtr) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (IntPtr) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < (double) IntPtr.MinValue || (double) num > (double) IntPtr.MaxValue)
        {
          result = IntPtr.Zero;
          return false;
        }
        result = (IntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (IntPtr) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        if ((long) num > (long) IntPtr.MaxValue)
        {
          result = IntPtr.Zero;
          return false;
        }
        result = (IntPtr) num;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) IntPtr.MaxValue)
        {
          result = IntPtr.Zero;
          return false;
        }
        result = (IntPtr) (long) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > (UIntPtr) IntPtr.MaxValue)
        {
          result = IntPtr.Zero;
          return false;
        }
        result = (IntPtr) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = IntPtr.Zero;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002ETryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      [NativeInteger] out IntPtr result)
    {
      return IntPtr.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<IntPtr>.System\u002EINumber\u003Cnint\u003E\u002ETryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      [NativeInteger] out IntPtr result)
    {
      return IntPtr.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IParseable<IntPtr>.System\u002EIParseable\u003Cnint\u003E\u002EParse(
      string s,
      IFormatProvider provider)
    {
      return IntPtr.Parse(s, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<IntPtr>.System\u002EIParseable\u003Cnint\u003E\u002ETryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      [NativeInteger] out IntPtr result)
    {
      return IntPtr.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IShiftOperators<IntPtr, IntPtr>.System\u002EIShiftOperators\u003Cnint\u002Cnint\u003E\u002Eop_LeftShift(
      [NativeInteger] IntPtr value,
      int shiftAmount)
    {
      return value << shiftAmount;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IShiftOperators<IntPtr, IntPtr>.System\u002EIShiftOperators\u003Cnint\u002Cnint\u003E\u002Eop_RightShift(
      [NativeInteger] IntPtr value,
      int shiftAmount)
    {
      return value >> shiftAmount;
    }

    [NativeInteger]
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    IntPtr ISignedNumber<IntPtr>.System\u002EISignedNumber\u003Cnint\u003E\u002ENegativeOne
    {
      [return: NativeInteger] get => new IntPtr(-1);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr ISpanParseable<IntPtr>.System\u002EISpanParseable\u003Cnint\u003E\u002EParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return IntPtr.Parse(s, NumberStyles.Integer, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<IntPtr>.System\u002EISpanParseable\u003Cnint\u003E\u002ETryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      [NativeInteger] out IntPtr result)
    {
      return IntPtr.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr ISubtractionOperators<IntPtr, IntPtr, IntPtr>.System\u002EISubtractionOperators\u003Cnint\u002Cnint\u002Cnint\u003E\u002Eop_Subtraction(
      [NativeInteger] IntPtr left,
      [NativeInteger] IntPtr right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IUnaryNegationOperators<IntPtr, IntPtr>.System\u002EIUnaryNegationOperators\u003Cnint\u002Cnint\u003E\u002Eop_UnaryNegation(
      [NativeInteger] IntPtr value)
    {
      return -value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [return: NativeInteger]
    IntPtr IUnaryPlusOperators<IntPtr, IntPtr>.System\u002EIUnaryPlusOperators\u003Cnint\u002Cnint\u003E\u002Eop_UnaryPlus(
      [NativeInteger] IntPtr value)
    {
      return value;
    }
  }
}
