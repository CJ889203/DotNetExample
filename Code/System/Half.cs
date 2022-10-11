// Decompiled with JetBrains decompiler
// Type: System.Half
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Represents a half-precision floating-point number.</summary>
  public readonly struct Half : 
    IComparable,
    ISpanFormattable,
    IFormattable,
    IComparable<Half>,
    IEquatable<Half>,
    IBinaryFloatingPoint<Half>,
    IBinaryNumber<Half>,
    IBitwiseOperators<Half, Half, Half>,
    INumber<Half>,
    IAdditionOperators<Half, Half, Half>,
    IAdditiveIdentity<Half, Half>,
    IComparisonOperators<Half, Half>,
    IEqualityOperators<Half, Half>,
    IDecrementOperators<Half>,
    IDivisionOperators<Half, Half, Half>,
    IIncrementOperators<Half>,
    IModulusOperators<Half, Half, Half>,
    IMultiplicativeIdentity<Half, Half>,
    IMultiplyOperators<Half, Half, Half>,
    ISpanParseable<Half>,
    IParseable<Half>,
    ISubtractionOperators<Half, Half, Half>,
    IUnaryNegationOperators<Half, Half>,
    IUnaryPlusOperators<Half, Half>,
    IFloatingPoint<Half>,
    ISignedNumber<Half>,
    IMinMaxValue<Half>
  {
    private static readonly Half PositiveZero = new Half((ushort) 0);
    private static readonly Half NegativeZero = new Half((ushort) 32768);
    private readonly ushort _value;

    /// <summary>Represents the smallest positive <see cref="T:System.Half" /> value that is greater than zero.</summary>
    /// <returns>5.9604645E-08</returns>
    public static Half Epsilon => new Half((ushort) 1);

    /// <summary>Represents positive infinity.</summary>
    /// <returns>Infinity.</returns>
    public static Half PositiveInfinity => new Half((ushort) 31744);

    /// <summary>Represents negative infinity.</summary>
    /// <returns>-Infinity.</returns>
    public static Half NegativeInfinity => new Half((ushort) 64512);

    /// <summary>Represents not a number.</summary>
    /// <returns>NaN.</returns>
    public static Half NaN => new Half((ushort) 65024);

    /// <summary>Represents the smallest possible value of <see cref="T:System.Half" />.</summary>
    /// <returns>-65504</returns>
    public static Half MinValue => new Half((ushort) 64511);

    /// <summary>Represents the largest possible value of <see cref="T:System.Half" />.</summary>
    /// <returns>65504</returns>
    public static Half MaxValue => new Half((ushort) 31743);

    internal Half(ushort value) => this._value = value;

    private Half(bool sign, ushort exp, ushort sig) => this._value = (ushort) ((uint) (((sign ? 1 : 0) << 15) + ((int) exp << 10)) + (uint) sig);

    private sbyte Exponent => (sbyte) (((int) this._value & 31744) >> 10);

    private ushort Significand => (ushort) ((uint) this._value & 1023U);

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Half" /> value is less than another specified <see cref="T:System.Half" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <(Half left, Half right)
    {
      if (Half.IsNaN(left) || Half.IsNaN(right))
        return false;
      bool flag = Half.IsNegative(left);
      return flag != Half.IsNegative(right) ? flag && !Half.AreZero(left, right) : (int) left._value != (int) right._value && (int) left._value < (int) right._value ^ flag;
    }

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Half" /> value is greater than another specified <see cref="T:System.Half" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >(Half left, Half right) => right < left;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Half" /> value is less than or equal to another specified <see cref="T:System.Half" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <=(Half left, Half right)
    {
      if (Half.IsNaN(left) || Half.IsNaN(right))
        return false;
      bool flag = Half.IsNegative(left);
      return flag != Half.IsNegative(right) ? flag || Half.AreZero(left, right) : (int) left._value == (int) right._value || (int) left._value < (int) right._value ^ flag;
    }

    /// <summary>Returns a value that indicates whether <paramref name="left" /> is greater than or equal to <paramref name="right" />.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >=(Half left, Half right) => right <= left;

    /// <summary>Returns a value that indicates whether two specified <see cref="T:System.Half" /> values are equal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Half left, Half right)
    {
      if (Half.IsNaN(left) || Half.IsNaN(right))
        return false;
      return (int) left._value == (int) right._value || Half.AreZero(left, right);
    }

    /// <summary>Returns a value that indicates whether two specified <see cref="T:System.Half" /> values are not equal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Half left, Half right) => !(left == right);

    /// <summary>Determines whether the specified value is finite (zero, subnormal, or normal).</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if the specified value is finite (zero, subnormal or normal); otherwise, <see langword="false" />.</returns>
    public static bool IsFinite(Half value) => Half.StripSign(value) < 31744U;

    /// <summary>Returns a value indicating whether the specified number evaluates to positive infinity.</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> evaluates to <see cref="P:System.Half.PositiveInfinity" />; otherwise <see langword="false" />.</returns>
    public static bool IsInfinity(Half value) => Half.StripSign(value) == 31744U;

    /// <summary>Determines whether the specified value is not a number.</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> evaluates to not a number <see cref="P:System.Half.NaN" />; otherwise <see langword="false" />.</returns>
    public static bool IsNaN(Half value) => Half.StripSign(value) > 31744U;

    /// <summary>Determines whether the specified value is negative.</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if negative; <see langword="false" /> otherwise.</returns>
    public static bool IsNegative(Half value) => (short) value._value < (short) 0;

    /// <summary>Determines whether the specified value is negative infinity.</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> evaluates to <see cref="P:System.Half.NegativeInfinity" />; otherwise <see langword="false" />.</returns>
    public static bool IsNegativeInfinity(Half value) => value._value == (ushort) 64512;

    /// <summary>Determines whether the specified value is normal.</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is normal; <see langword="false" /> otherwise.</returns>
    public static bool IsNormal(Half value)
    {
      uint num = Half.StripSign(value);
      return num < 31744U && num != 0U && (num & 31744U) > 0U;
    }

    /// <summary>Determines whether the specified value is positive infinity.</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> evaluates to <see cref="P:System.Half.PositiveInfinity" />; otherwise <see langword="false" />.</returns>
    public static bool IsPositiveInfinity(Half value) => value._value == (ushort) 31744;

    /// <summary>Determines whether the specified value is subnormal.</summary>
    /// <param name="value">A <see cref="T:System.Half" /> floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is subnormal; <see langword="false" /><see langword="false" />.</returns>
    public static bool IsSubnormal(Half value)
    {
      uint num = Half.StripSign(value);
      return num < 31744U && num != 0U && ((int) num & 31744) == 0;
    }

    /// <summary>Converts the string representation of a number to its half-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.Half.MinValue" /> or greater than <see cref="P:System.Half.MaxValue" />.</exception>
    /// <returns>A half-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static Half Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseHalf((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.Half.MinValue" /> or greater than <see cref="P:System.Half.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    ///    -or-
    /// 
    ///    <see cref="T:System.Globalization.NumberStyles" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>A half-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static Half Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseHalf((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.Half.MinValue" /> or greater than <see cref="P:System.Half.MaxValue" />.</exception>
    /// <returns>A half-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static Half Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseHalf((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="P:System.Half.MinValue" /> or greater than <see cref="P:System.Half.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    ///   -or-
    /// 
    ///   <see cref="T:System.Globalization.NumberStyles" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>A half-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static Half Parse(string s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseHalf((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">Converts the string representation of a number to its half-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    ///   -or-
    /// 
    ///   <see cref="T:System.Globalization.NumberStyles" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>A half-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static Half Parse(
      ReadOnlySpan<char> s,
      NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands,
      IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return Number.ParseHalf(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number to its half-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="result">When this method returns, contains half-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or a default <see cref="T:System.Half" /> value if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" /> or is not a number in a valid format. If is a valid number less than <see cref="P:System.Half.MinValue" />, result is <see cref="P:System.Half.NegativeInfinity" />. If is a valid number greater than <see cref="P:System.Half.MaxValue" />, result is <see cref="P:System.Half.PositiveInfinity" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if the parse was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out Half result)
    {
      if (s != null)
        return Half.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) null, out result);
      result = new Half();
      return false;
    }

    /// <summary>Converts the span representation of a number to its half-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A read-only span that contains a number to convert.</param>
    /// <param name="result">When this method returns, contains half-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or a default <see cref="T:System.Half" /> value if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" /> or is not a number in a valid format. If is a valid number less than <see cref="P:System.Half.MinValue" />, result is <see cref="P:System.Half.NegativeInfinity" />. If is a valid number greater than <see cref="P:System.Half.MaxValue" />, result is <see cref="P:System.Half.PositiveInfinity" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully, <see langword="false" /> otherwise.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out Half result) => Half.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) null, out result);

    /// <summary>Converts the string representation of a number to its half-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains half-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or a default <see cref="T:System.Half" /> value if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" /> or is not a number in a valid format. If is a valid number less than <see cref="P:System.Half.MinValue" />, result is <see cref="P:System.Half.NegativeInfinity" />. If is a valid number greater than <see cref="P:System.Half.MaxValue" />, result is <see cref="P:System.Half.PositiveInfinity" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    ///   -or-
    /// 
    ///   <see cref="T:System.Globalization.NumberStyles" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>
    /// <see langword="true" /> if the parse was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      NumberStyles style,
      IFormatProvider? provider,
      out Half result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s != null)
        return Half.TryParse(s.AsSpan(), style, provider, out result);
      result = new Half();
      return false;
    }

    /// <summary>Converts the span representation of a number to its half-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A read-only span that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains half-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or a default <see cref="T:System.Half" /> value if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is not a number in a valid format. If <paramref name="s" /> is a valid number less than <see cref="P:System.Half.MinValue" />, result is <see cref="P:System.Half.NegativeInfinity" />. If <paramref name="s" /> is a valid number greater than <see cref="P:System.Half.MaxValue" />, result is <see cref="P:System.Half.PositiveInfinity" />. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    ///   -or-
    /// 
    ///   <see cref="T:System.Globalization.NumberStyles" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>
    /// <see langword="true" /> if the parse was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out Half result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return Number.TryParseHalf(s, style, NumberFormatInfo.GetInstance(provider), out result);
    }

    private static bool AreZero(Half left, Half right) => (ushort) (((int) left._value | (int) right._value) & -32769) == (ushort) 0;

    private static bool IsNaNOrZero(Half value) => ((int) value._value - 1 & -32769) >= 31744;

    private static uint StripSign(Half value) => (uint) (ushort) ((uint) value._value & 4294934527U);

    /// <summary>Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified object.</summary>
    /// <param name="obj">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="obj" /> is not of type <see cref="T:System.Half" />.</exception>
    /// <returns>
    /// A value less than zero if this instance is less than <paramref name="obj" />, or this instance is not a number (NaN) and <paramref name="obj" /> is a number.
    /// 
    /// -or-
    /// 
    /// Zero if this instance is equal to <paramref name="obj" />, or this instance and <paramref name="obj" /> are both not a number (NaN), <see cref="P:System.Half.PositiveInfinity" />, or <see cref="P:System.Half.NegativeInfinity" />.
    /// 
    /// -or-
    /// 
    /// A value greater than zero if this instance is greater than <paramref name="obj" />, or this instance is a number and <paramref name="obj" /> is not a number (NaN), or <paramref name="obj" /> is <see langword="null" />.</returns>
    public int CompareTo(object? obj)
    {
      if (obj is Half other)
        return this.CompareTo(other);
      if (obj != null)
        throw new ArgumentException(SR.Arg_MustBeHalf);
      return 1;
    }

    /// <summary>Compares this instance to a specified half-precision floating-point number and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified half-precision floating-point number.</summary>
    /// <param name="other">A half-precision floating-point number to compare.</param>
    /// <returns>A value less than zero if this is less than <paramref name="other" />, zero if this is equal to <paramref name="other" />, or a value greater than zero if this is greater than <paramref name="other" />.</returns>
    public int CompareTo(Half other)
    {
      if (this < other)
        return -1;
      if (this > other)
        return 1;
      if (this == other)
        return 0;
      if (!Half.IsNaN(this))
        return 1;
      return !Half.IsNaN(other) ? -1 : 0;
    }

    /// <summary>Returns a value that indicates whether this instance is equal to the specified <paramref name="obj" />.</summary>
    /// <param name="obj">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Half" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Half other && this.Equals(other);

    /// <summary>Compares this instance for equality with <paramref name="other" />.</summary>
    /// <param name="other">A half-precision floating point number to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to <paramref name="other" />; otherwise, <see langword="false" />.</returns>
    public bool Equals(Half other)
    {
      if ((int) this._value == (int) other._value || Half.AreZero(this, other))
        return true;
      return Half.IsNaN(this) && Half.IsNaN(other);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => Half.IsNaNOrZero(this) ? (int) this._value & 31744 : (int) this._value;

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance.</returns>
    public override string ToString() => Number.FormatHalf(this, (string) null, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatHalf(this, format, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatHalf(this, (string) null, NumberFormatInfo.GetInstance(provider));

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatHalf(this, format, NumberFormatInfo.GetInstance(provider));

    /// <summary>Tries to format the value of the current <see cref="System.Half" /> instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful, otherwise, <see langword="false" />.</returns>
    public bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      return Number.TryFormatHalf(this, format, NumberFormatInfo.GetInstance(provider), destination, out charsWritten);
    }

    /// <summary>An explicit operator to convert a <see cref="T:System.Single" /> value to a <see cref="T:System.Half" />.</summary>
    /// <param name="value">The single-precision floating point value to convert to <see cref="T:System.Half" />.</param>
    /// <returns>The <see cref="T:System.Half" /> representation of the specified single-precision floating point <paramref name="value" />.</returns>
    public static explicit operator Half(float value)
    {
      uint uint32Bits = BitConverter.SingleToUInt32Bits(value);
      bool sign = (uint32Bits & 2147483648U) >> 31 > 0U;
      int num1 = ((int) uint32Bits & 2139095040) >> 23;
      uint num2 = uint32Bits & 8388607U;
      if (num1 == (int) byte.MaxValue)
      {
        if (num2 != 0U)
          return Half.CreateHalfNaN(sign, (ulong) num2 << 41);
        return !sign ? Half.PositiveInfinity : Half.NegativeInfinity;
      }
      uint num3 = num2 >> 9 | (((int) num2 & 511) != 0 ? 1U : 0U);
      return (num1 | (int) num3) == 0 ? new Half(sign, (ushort) 0, (ushort) 0) : new Half(Half.RoundPackToHalf(sign, (short) (num1 - 113), (ushort) (num3 | 16384U)));
    }

    /// <summary>An explicit operator to convert a <see cref="T:System.Double" /> value to a <see cref="T:System.Half" />.</summary>
    /// <param name="value">The double-precision floating point value to convert to <see cref="T:System.Half" />.</param>
    /// <returns>The <see cref="T:System.Half" /> representation of the specified double-precision floating point <paramref name="value" />.</returns>
    public static explicit operator Half(double value)
    {
      ulong uint64Bits = BitConverter.DoubleToUInt64Bits(value);
      bool sign = (uint64Bits & 9223372036854775808UL) >> 63 > 0UL;
      int num1 = (int) ((uint64Bits & 9218868437227405312UL) >> 52);
      ulong l = uint64Bits & 4503599627370495UL;
      if (num1 == 2047)
      {
        if (l != 0UL)
          return Half.CreateHalfNaN(sign, l << 12);
        return !sign ? Half.PositiveInfinity : Half.NegativeInfinity;
      }
      uint num2 = (uint) Half.ShiftRightJam(l, 38);
      return (num1 | (int) num2) == 0 ? new Half(sign, (ushort) 0, (ushort) 0) : new Half(Half.RoundPackToHalf(sign, (short) (num1 - 1009), (ushort) (num2 | 16384U)));
    }

    /// <summary>An explicit operator to convert a <see cref="T:System.Half" /> value to a <see cref="T:System.Single" />.</summary>
    /// <param name="value">The half-precision floating point value to convert to <see cref="T:System.Single" />.</param>
    /// <returns>The <see cref="T:System.Single" /> representation of the specified half-precision floating point <paramref name="value" />.</returns>
    public static explicit operator float(Half value)
    {
      bool sign = Half.IsNegative(value);
      int num = (int) value.Exponent;
      uint significand = (uint) value.Significand;
      switch (num)
      {
        case 0:
          if (significand == 0U)
            return BitConverter.UInt32BitsToSingle(sign ? 2147483648U : 0U);
          int Exp;
          (Exp, significand) = Half.NormSubnormalF16Sig(significand);
          num = Exp - 1;
          break;
        case 31:
          if (significand != 0U)
            return Half.CreateSingleNaN(sign, (ulong) significand << 54);
          return !sign ? float.PositiveInfinity : float.NegativeInfinity;
      }
      return Half.CreateSingle(sign, (byte) (num + 112), significand << 13);
    }

    /// <summary>An explicit operator to convert a <see cref="T:System.Half" /> value to a <see cref="T:System.Double" />.</summary>
    /// <param name="value">The half-precision floating point value to convert to <see cref="T:System.Double" />.</param>
    /// <returns>The <see cref="T:System.Double" /> representation of the specified half-precision floating point <paramref name="value" />.</returns>
    public static explicit operator double(Half value)
    {
      bool sign = Half.IsNegative(value);
      int num = (int) value.Exponent;
      uint significand = (uint) value.Significand;
      switch (num)
      {
        case 0:
          if (significand == 0U)
            return BitConverter.UInt64BitsToDouble(sign ? 9223372036854775808UL : 0UL);
          int Exp;
          (Exp, significand) = Half.NormSubnormalF16Sig(significand);
          num = Exp - 1;
          break;
        case 31:
          if (significand != 0U)
            return Half.CreateDoubleNaN(sign, (ulong) significand << 54);
          return !sign ? double.PositiveInfinity : double.NegativeInfinity;
      }
      return Half.CreateDouble(sign, (ushort) (num + 1008), (ulong) significand << 42);
    }

    internal static Half Negate(Half value) => !Half.IsNaN(value) ? new Half((ushort) ((uint) value._value ^ 32768U)) : value;


    #nullable disable
    private static (int Exp, uint Sig) NormSubnormalF16Sig(uint sig)
    {
      int num = BitOperations.LeadingZeroCount(sig) - 16 - 5;
      return (1 - num, sig << num);
    }

    private static Half CreateHalfNaN(bool sign, ulong significand) => BitConverter.UInt16BitsToHalf((ushort) ((uint) ((sign ? 1 : 0) << 15 | 32256) | (uint) (significand >> 54)));

    private static ushort RoundPackToHalf(bool sign, short exp, ushort sig)
    {
      int num = (int) sig & 15;
      if ((uint) exp >= 29U)
      {
        if (exp < (short) 0)
        {
          sig = (ushort) Half.ShiftRightJam((uint) sig, (int) -exp);
          exp = (short) 0;
          num = (int) sig & 15;
        }
        else if (exp > (short) 29 || (int) sig + 8 >= 32768)
          return !sign ? (ushort) 31744 : (ushort) 64512;
      }
      sig = (ushort) ((int) sig + 8 >> 4);
      sig &= (ushort) ~(((num ^ 8) != 0 ? 0 : 1) & 1);
      if (sig == (ushort) 0)
        exp = (short) 0;
      return new Half(sign, (ushort) exp, sig)._value;
    }

    private static uint ShiftRightJam(uint i, int dist)
    {
      if (dist < 31)
        return i >> dist | ((int) i << -dist != 0 ? 1U : 0U);
      return i == 0U ? 0U : 1U;
    }

    private static ulong ShiftRightJam(ulong l, int dist)
    {
      if (dist < 63)
        return l >> dist | ((long) l << -dist != 0L ? 1UL : 0UL);
      return l == 0UL ? 0UL : 1UL;
    }

    private static float CreateSingleNaN(bool sign, ulong significand) => BitConverter.UInt32BitsToSingle((uint) ((sign ? 1 : 0) << 31 | 2143289344) | (uint) (significand >> 41));

    private static double CreateDoubleNaN(bool sign, ulong significand) => BitConverter.UInt64BitsToDouble((ulong) ((sign ? 1L : 0L) << 63 | 9221120237041090560L) | significand >> 12);

    private static float CreateSingle(bool sign, byte exp, uint sig) => BitConverter.UInt32BitsToSingle((uint) (((sign ? 1 : 0) << 31) + ((int) exp << 23)) + sig);

    private static double CreateDouble(bool sign, ushort exp, ulong sig) => BitConverter.UInt64BitsToDouble((ulong) (((sign ? 1L : 0L) << 63) + ((long) exp << 52)) + sig);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IAdditionOperators<Half, Half, Half>.op_Addition(
      Half left,
      Half right)
    {
      return (Half) ((float) left + (float) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IAdditiveIdentity<Half, Half>.AdditiveIdentity => Half.PositiveZero;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<Half>.IsPow2(Half value)
    {
      uint uint16Bits = (uint) BitConverter.HalfToUInt16Bits(value);
      uint num1 = uint16Bits >> 10 & 31U;
      uint num2 = uint16Bits & 1023U;
      return value > Half.PositiveZero && num1 != 0U && num1 != 31U && num2 == 0U;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IBinaryNumber<Half>.Log2(Half value) => (Half) MathF.Log2((float) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IBitwiseOperators<Half, Half, Half>.op_BitwiseAnd(
      Half left,
      Half right)
    {
      return BitConverter.UInt16BitsToHalf((ushort) ((uint) BitConverter.HalfToUInt16Bits(left) & (uint) BitConverter.HalfToUInt16Bits(right)));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IBitwiseOperators<Half, Half, Half>.op_BitwiseOr(
      Half left,
      Half right)
    {
      return BitConverter.UInt16BitsToHalf((ushort) ((uint) BitConverter.HalfToUInt16Bits(left) | (uint) BitConverter.HalfToUInt16Bits(right)));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IBitwiseOperators<Half, Half, Half>.op_ExclusiveOr(
      Half left,
      Half right)
    {
      return BitConverter.UInt16BitsToHalf((ushort) ((uint) BitConverter.HalfToUInt16Bits(left) ^ (uint) BitConverter.HalfToUInt16Bits(right)));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IBitwiseOperators<Half, Half, Half>.op_OnesComplement(
      Half value)
    {
      return BitConverter.UInt16BitsToHalf(~BitConverter.HalfToUInt16Bits(value));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Half, Half>.op_LessThan(
      Half left,
      Half right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Half, Half>.op_LessThanOrEqual(
      Half left,
      Half right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Half, Half>.op_GreaterThan(
      Half left,
      Half right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Half, Half>.op_GreaterThanOrEqual(
      Half left,
      Half right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IDecrementOperators<Half>.op_Decrement(Half value) => (Half) ((float) value - 1f);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<Half, Half>.op_Equality(Half left, Half right) => left == right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<Half, Half>.op_Inequality(
      Half left,
      Half right)
    {
      return left != right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IDivisionOperators<Half, Half, Half>.op_Division(
      Half left,
      Half right)
    {
      return (Half) ((float) left / (float) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.E => (Half) 2.7182817f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Epsilon => Half.Epsilon;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.NaN => Half.NaN;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.NegativeInfinity => Half.NegativeInfinity;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.NegativeZero => Half.NegativeZero;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Pi => (Half) 3.1415927f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.PositiveInfinity => Half.PositiveInfinity;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Tau => (Half) 6.2831855f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Acos(Half x) => (Half) MathF.Acos((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Acosh(Half x) => (Half) MathF.Acosh((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Asin(Half x) => (Half) MathF.Asin((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Asinh(Half x) => (Half) MathF.Asinh((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Atan(Half x) => (Half) MathF.Atan((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Atan2(Half y, Half x) => (Half) MathF.Atan2((float) y, (float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Atanh(Half x) => (Half) MathF.Atanh((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.BitIncrement(Half x)
    {
      ushort uint16Bits = BitConverter.HalfToUInt16Bits(x);
      return ((int) uint16Bits & 31744) >= 31744 ? (uint16Bits != (ushort) 64512 ? x : Half.MinValue) : (uint16Bits == (ushort) 32768 ? Half.Epsilon : BitConverter.UInt16BitsToHalf((ushort) ((int) uint16Bits + (uint16Bits < (ushort) 0 ? (int) ushort.MaxValue : 1))));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.BitDecrement(Half x)
    {
      ushort uint16Bits = BitConverter.HalfToUInt16Bits(x);
      return ((int) uint16Bits & 31744) >= 31744 ? (uint16Bits != (ushort) 31744 ? x : Half.MaxValue) : (uint16Bits == (ushort) 0 ? new Half((ushort) 32769) : BitConverter.UInt16BitsToHalf((ushort) ((int) uint16Bits + (uint16Bits < (ushort) 0 ? 1 : (int) ushort.MaxValue))));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Cbrt(Half x) => (Half) MathF.Cbrt((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Ceiling(Half x) => (Half) MathF.Ceiling((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.CopySign(Half x, Half y) => (Half) MathF.CopySign((float) x, (float) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Cos(Half x) => (Half) MathF.Cos((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Cosh(Half x) => (Half) MathF.Cosh((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Exp(Half x) => (Half) MathF.Exp((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Floor(Half x) => (Half) MathF.Floor((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.FusedMultiplyAdd(
      Half left,
      Half right,
      Half addend)
    {
      return (Half) MathF.FusedMultiplyAdd((float) left, (float) right, (float) addend);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.IEEERemainder(Half left, Half right) => (Half) MathF.IEEERemainder((float) left, (float) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TInteger IFloatingPoint<Half>.ILogB<TInteger>(Half x) => INumber<TInteger>.Create<int>(MathF.ILogB((float) x));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Log(Half x) => (Half) MathF.Log((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Log(Half x, Half newBase) => (Half) MathF.Log((float) x, (float) newBase);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Log2(Half x) => (Half) MathF.Log2((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Log10(Half x) => (Half) MathF.Log10((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.MaxMagnitude(Half x, Half y) => (Half) MathF.MaxMagnitude((float) x, (float) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.MinMagnitude(Half x, Half y) => (Half) MathF.MinMagnitude((float) x, (float) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Pow(Half x, Half y) => (Half) MathF.Pow((float) x, (float) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Round(Half x) => (Half) MathF.Round((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Round<TInteger>(Half x, TInteger digits) => (Half) MathF.Round((float) x, int.Create<TInteger>(digits));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Round(Half x, MidpointRounding mode) => (Half) MathF.Round((float) x, mode);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Round<TInteger>(
      Half x,
      TInteger digits,
      MidpointRounding mode)
    {
      return (Half) MathF.Round((float) x, int.Create<TInteger>(digits), mode);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.ScaleB<TInteger>(Half x, TInteger n) => (Half) MathF.ScaleB((float) x, int.Create<TInteger>(n));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Sin(Half x) => (Half) MathF.Sin((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Sinh(Half x) => (Half) MathF.Sinh((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Sqrt(Half x) => (Half) MathF.Sqrt((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Tan(Half x) => (Half) MathF.Tan((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Tanh(Half x) => (Half) MathF.Tanh((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IFloatingPoint<Half>.Truncate(Half x) => (Half) MathF.Truncate((float) x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsFinite(Half x) => Half.IsFinite(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsInfinity(Half x) => Half.IsInfinity(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsNaN(Half x) => Half.IsNaN(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsNegative(Half x) => Half.IsNegative(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsNegativeInfinity(Half x) => Half.IsNegativeInfinity(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsNormal(Half x) => Half.IsNormal(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsPositiveInfinity(Half x) => Half.IsPositiveInfinity(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<Half>.IsSubnormal(Half x) => Half.IsSubnormal(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IIncrementOperators<Half>.op_Increment(Half value) => (Half) ((float) value + 1f);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IMinMaxValue<Half>.MinValue => Half.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IMinMaxValue<Half>.MaxValue => Half.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IModulusOperators<Half, Half, Half>.op_Modulus(
      Half left,
      Half right)
    {
      return (Half) ((float) left % (float) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IMultiplicativeIdentity<Half, Half>.MultiplicativeIdentity => (Half) 1f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IMultiplyOperators<Half, Half, Half>.op_Multiply(
      Half left,
      Half right)
    {
      return (Half) ((float) left * (float) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.One => (Half) 1f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Zero => Half.PositiveZero;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Abs(Half value) => (Half) MathF.Abs((float) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Clamp(Half value, Half min, Half max) => (Half) Math.Clamp((float) value, (float) min, (float) max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Half INumber<Half>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (Half) (float) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (Half) (float) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (Half) (float) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (Half) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (Half) (float) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (Half) (float) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (Half) (float) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (Half) (float) (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (Half) (float) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (Half) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (Half) (float) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (Half) (float) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (Half) (float) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (Half) (float) (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return new Half();
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Half INumber<Half>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (Half) (float) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (Half) (float) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (Half) (float) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (Half) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (Half) (float) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (Half) (float) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (Half) (float) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (Half) (float) (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (Half) (float) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (Half) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (Half) (float) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (Half) (float) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (Half) (float) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (Half) (float) (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return new Half();
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Half INumber<Half>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (Half) (float) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (Half) (float) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (Half) (float) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (Half) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (Half) (float) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (Half) (float) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (Half) (float) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (Half) (float) (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (Half) (float) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (Half) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (Half) (float) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (Half) (float) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (Half) (float) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (Half) (float) (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return new Half();
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (Half Quotient, Half Remainder) INumber<Half>.DivRem(Half left, Half right) => ((Half) ((float) left / (float) right), (Half) ((float) left % (float) right));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Max(Half x, Half y) => (Half) MathF.Max((float) x, (float) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Min(Half x, Half y) => (Half) MathF.Min((float) x, (float) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Parse(string s, NumberStyles style, IFormatProvider provider) => Half.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return Half.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half INumber<Half>.Sign(Half value) => (Half) (float) MathF.Sign((float) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<Half>.TryCreate<TOther>(TOther value, out Half result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (Half) (float) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (Half) (float) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        result = (Half) (float) (Decimal) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        result = (Half) (double) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (Half) (float) (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        result = (Half) (float) (int) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        result = (Half) (float) (long) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        result = (Half) (float) (long) (IntPtr) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (Half) (float) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        result = (Half) (float) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (Half) (float) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (Half) (float) (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        result = (Half) (float) (ulong) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        result = (Half) (float) (ulong) (UIntPtr) (object) value;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = new Half();
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<Half>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out Half result)
    {
      return Half.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<Half>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out Half result)
    {
      return Half.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IParseable<Half>.Parse(string s, IFormatProvider provider) => Half.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<Half>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out Half result)
    {
      return Half.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half ISignedNumber<Half>.NegativeOne => (Half) -1f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half ISpanParseable<Half>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return Half.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<Half>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out Half result)
    {
      return Half.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half ISubtractionOperators<Half, Half, Half>.op_Subtraction(
      Half left,
      Half right)
    {
      return (Half) ((float) left - (float) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IUnaryNegationOperators<Half, Half>.op_UnaryNegation(
      Half value)
    {
      return (Half) -(float) value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Half IUnaryPlusOperators<Half, Half>.op_UnaryPlus(Half value) => value;
  }
}
