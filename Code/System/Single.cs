// Decompiled with JetBrains decompiler
// Type: System.Single
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Represents a single-precision floating-point number.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Single : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<float>,
    IEquatable<float>,
    IBinaryFloatingPoint<float>,
    IBinaryNumber<float>,
    IBitwiseOperators<float, float, float>,
    INumber<float>,
    IAdditionOperators<float, float, float>,
    IAdditiveIdentity<float, float>,
    IComparisonOperators<float, float>,
    IEqualityOperators<float, float>,
    IDecrementOperators<float>,
    IDivisionOperators<float, float, float>,
    IIncrementOperators<float>,
    IModulusOperators<float, float, float>,
    IMultiplicativeIdentity<float, float>,
    IMultiplyOperators<float, float, float>,
    ISpanParseable<float>,
    IParseable<float>,
    ISubtractionOperators<float, float, float>,
    IUnaryNegationOperators<float, float>,
    IUnaryPlusOperators<float, float>,
    IFloatingPoint<float>,
    ISignedNumber<float>,
    IMinMaxValue<float>
  {
    private readonly float m_value;
    /// <summary>Represents the smallest possible value of <see cref="T:System.Single" />. This field is constant.</summary>
    public const float MinValue = -3.4028235E+38f;
    /// <summary>Represents the smallest positive <see cref="T:System.Single" /> value that is greater than zero. This field is constant.</summary>
    public const float Epsilon = 1E-45f;
    /// <summary>Represents the largest possible value of <see cref="T:System.Single" />. This field is constant.</summary>
    public const float MaxValue = 3.4028235E+38f;
    /// <summary>Represents positive infinity. This field is constant.</summary>
    public const float PositiveInfinity = 1.0f / 0.0f;
    /// <summary>Represents negative infinity. This field is constant.</summary>
    public const float NegativeInfinity = -1.0f / 0.0f;
    /// <summary>Represents not a number (<see langword="NaN" />). This field is constant.</summary>
    public const float NaN = 0.0f / 0.0f;

    /// <summary>Determines whether the specified value is finite (zero, subnormal or normal).</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if the specified value is finite (zero, subnormal or normal); otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFinite(float f) => (BitConverter.SingleToInt32Bits(f) & int.MaxValue) < 2139095040;

    /// <summary>Returns a value indicating whether the specified number evaluates to negative or positive infinity.</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="f" /> evaluates to <see cref="F:System.Single.PositiveInfinity" /> or <see cref="F:System.Single.NegativeInfinity" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInfinity(float f) => (BitConverter.SingleToInt32Bits(f) & int.MaxValue) == 2139095040;

    /// <summary>Returns a value that indicates whether the specified value is not a number (<see cref="F:System.Single.NaN" />).</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="f" /> evaluates to not a number (<see cref="F:System.Single.NaN" />); otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(float f) => (double) f != (double) f;

    /// <summary>Determines whether the specified value is negative.</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if negative, <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegative(float f) => BitConverter.SingleToInt32Bits(f) < 0;

    /// <summary>Returns a value indicating whether the specified number evaluates to negative infinity.</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="f" /> evaluates to <see cref="F:System.Single.NegativeInfinity" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegativeInfinity(float f) => (double) f == double.NegativeInfinity;

    /// <summary>Determines whether the specified value is normal.</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="f" /> is normal; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsNormal(float f)
    {
      int num = BitConverter.SingleToInt32Bits(f) & int.MaxValue;
      return num < 2139095040 && num != 0 && (num & 2139095040) != 0;
    }

    /// <summary>Returns a value indicating whether the specified number evaluates to positive infinity.</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="f" /> evaluates to <see cref="F:System.Single.PositiveInfinity" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPositiveInfinity(float f) => (double) f == double.PositiveInfinity;

    /// <summary>Determines whether the specified value is subnormal.</summary>
    /// <param name="f">A single-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="f" /> is subnormal; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsSubnormal(float f)
    {
      int num = BitConverter.SingleToInt32Bits(f) & int.MaxValue;
      return num < 2139095040 && num != 0 && (num & 2139095040) == 0;
    }

    internal static int ExtractExponentFromBits(uint bits) => (int) (bits >> 23) & (int) byte.MaxValue;

    internal static uint ExtractSignificandFromBits(uint bits) => bits & 8388607U;

    /// <summary>Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified object.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.Single" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />, or this instance is not a number (<see cref="F:System.Single.NaN" />) and <paramref name="value" /> is a number.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />, or this instance and value are both not a number (<see cref="F:System.Single.NaN" />), <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, OR this instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Single.NaN" />), OR <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is float f))
        throw new ArgumentException(SR.Arg_MustBeSingle);
      if ((double) this < (double) f)
        return -1;
      if ((double) this > (double) f)
        return 1;
      if ((double) this == (double) f)
        return 0;
      if (!float.IsNaN(this))
        return 1;
      return !float.IsNaN(f) ? -1 : 0;
    }

    /// <summary>Compares this instance to a specified single-precision floating-point number and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified single-precision floating-point number.</summary>
    /// <param name="value">A single-precision floating-point number to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />, or this instance is not a number (<see cref="F:System.Single.NaN" />) and <paramref name="value" /> is a number.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />, or both this instance and <paramref name="value" /> are not a number (<see cref="F:System.Single.NaN" />), <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or this instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Single.NaN" />).</description></item></list></returns>
    public int CompareTo(float value)
    {
      if ((double) this < (double) value)
        return -1;
      if ((double) this > (double) value)
        return 1;
      if ((double) this == (double) value)
        return 0;
      if (!float.IsNaN(this))
        return 1;
      return !float.IsNaN(value) ? -1 : 0;
    }

    /// <summary>Returns a value that indicates whether two specified <see cref="T:System.Single" /> values are equal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator ==(float left, float right) => (double) left == (double) right;

    /// <summary>Returns a value that indicates whether two specified <see cref="T:System.Single" /> values are not equal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator !=(float left, float right) => (double) left != (double) right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is less than another specified <see cref="T:System.Single" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator <(float left, float right) => (double) left < (double) right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is greater than another specified <see cref="T:System.Single" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator >(float left, float right) => (double) left > (double) right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is less than or equal to another specified <see cref="T:System.Single" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator <=(float left, float right) => (double) left <= (double) right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Single" /> value is greater than or equal to another specified <see cref="T:System.Single" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator >=(float left, float right) => (double) left >= (double) right;

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Single" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
      if (!(obj is float f))
        return false;
      if ((double) f == (double) this)
        return true;
      return float.IsNaN(f) && float.IsNaN(this);
    }

    /// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Single" /> object represent the same value.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(float obj)
    {
      if ((double) obj == (double) this)
        return true;
      return float.IsNaN(obj) && float.IsNaN(this);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
      int hashCode = Unsafe.As<float, int>(ref Unsafe.AsRef<float>(in this.m_value));
      if ((hashCode - 1 & int.MaxValue) >= 2139095040)
        hashCode &= 2139095040;
      return hashCode;
    }

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance.</returns>
    public override string ToString() => Number.FormatSingle(this, (string) null, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatSingle(this, (string) null, NumberFormatInfo.GetInstance(provider));

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatSingle(this, format, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatSingle(this, format, NumberFormatInfo.GetInstance(provider));

    /// <summary>Tries to format the value of the current float number instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the charactes that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      return Number.TryFormatSingle(this, format, NumberFormatInfo.GetInstance(provider), destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>A single-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static float Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseSingle((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>A single-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static float Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseSingle((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>A single-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static float Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseSingle((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a numeric value.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <exception cref="T:System.OverflowException">
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>A single-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static float Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseSingle((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts a character span that contains the string representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent.</summary>
    /// <param name="s">A character span that contains the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s" />.  A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a numeric value.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>A single-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static float Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return Number.ParseSingle(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number to its single-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string representing a number to convert.</param>
    /// <param name="result">When this method returns, contains single-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" /> or is not a number in a valid format. It also fails on .NET Framework and .NET Core 2.2 and earlier versions if <paramref name="s" /> represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out float result)
    {
      if (s != null)
        return float.TryParse((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);
      result = 0.0f;
      return false;
    }

    /// <summary>Converts the string representation of a number in a character span to its single-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">&gt;A character span that contains the string representation of the number to convert.</param>
    /// <param name="result">When this method returns, contains the single-precision floating-point number equivalent of the <paramref name="s" /> parameter, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or empty or is not a number in a valid format. If <paramref name="s" /> is a valid number less than <see cref="F:System.Single.MinValue" />, <paramref name="result" /> is <see cref="F:System.Single.NegativeInfinity" />. If <paramref name="s" /> is a valid number greater than <see cref="F:System.Single.MaxValue" />, <paramref name="result" /> is <see cref="F:System.Single.PositiveInfinity" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out float result) => float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string representing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the single-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or if <paramref name="style" /> is not a valid combination of <see cref="T:System.Globalization.NumberStyles" /> enumeration constants. It also fails on .NET Framework or .NET Core 2.2 and earlier versions if <paramref name="s" /> represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      NumberStyles style,
      IFormatProvider? provider,
      out float result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s != null)
        return float.TryParse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result);
      result = 0.0f;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its single-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A read-only character span that contains the number to convert. The span is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the single-precision floating-point number equivalent to the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />, or if <paramref name="style" /> is not a valid combination of <see cref="T:System.Globalization.NumberStyles" /> enumerated constants. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out float result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return float.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
    }


    #nullable disable
    private static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      NumberFormatInfo info,
      out float result)
    {
      return Number.TryParseSingle(s, style, info, out result);
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Single" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.Single" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Single;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>
    /// <see langword="true" /> if the value of the current instance is not zero; otherwise, <see langword="false" />.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(this);

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Single), (object) "Char"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to an <see cref="T:System.SByte" />.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Byte" />.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to an <see cref="T:System.Int16" />.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.UInt16" />.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to an <see cref="T:System.Int32" />.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.UInt32" />.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to an <see cref="T:System.Int64" />.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.UInt64" />.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, unchanged.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => this;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Double" />.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Decimal" />.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(this);

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Single), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.Single" /> value.</param>
    /// <param name="provider">An object that supplies information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IAdditionOperators<float, float, float>.op_Addition(
      float left,
      float right)
    {
      return left + right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IAdditiveIdentity<float, float>.AdditiveIdentity => 0.0f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<float>.IsPow2(float value)
    {
      uint uint32Bits = BitConverter.SingleToUInt32Bits(value);
      uint num1 = uint32Bits >> 23 & (uint) byte.MaxValue;
      uint num2 = uint32Bits & 8388607U;
      return (double) value > 0.0 && num1 != 0U && num1 != (uint) byte.MaxValue && num2 == 0U;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IBinaryNumber<float>.Log2(float value) => MathF.Log2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IBitwiseOperators<float, float, float>.op_BitwiseAnd(
      float left,
      float right)
    {
      return BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(left) & BitConverter.SingleToUInt32Bits(right));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IBitwiseOperators<float, float, float>.op_BitwiseOr(
      float left,
      float right)
    {
      return BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(left) | BitConverter.SingleToUInt32Bits(right));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IBitwiseOperators<float, float, float>.op_ExclusiveOr(
      float left,
      float right)
    {
      return BitConverter.UInt32BitsToSingle(BitConverter.SingleToUInt32Bits(left) ^ BitConverter.SingleToUInt32Bits(right));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IBitwiseOperators<float, float, float>.op_OnesComplement(float value) => BitConverter.UInt32BitsToSingle(~BitConverter.SingleToUInt32Bits(value));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<float, float>.op_LessThan(float left, float right) => (double) left < (double) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<float, float>.op_LessThanOrEqual(
      float left,
      float right)
    {
      return (double) left <= (double) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<float, float>.op_GreaterThan(
      float left,
      float right)
    {
      return (double) left > (double) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<float, float>.op_GreaterThanOrEqual(
      float left,
      float right)
    {
      return (double) left >= (double) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IDecrementOperators<float>.op_Decrement(float value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IDivisionOperators<float, float, float>.op_Division(
      float left,
      float right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<float, float>.op_Equality(float left, float right) => (double) left == (double) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<float, float>.op_Inequality(float left, float right) => (double) left != (double) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.E => 2.7182817f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Epsilon => float.Epsilon;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.NaN => float.NaN;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.NegativeInfinity => float.NegativeInfinity;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.NegativeZero => -0.0f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Pi => 3.1415927f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.PositiveInfinity => float.PositiveInfinity;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Tau => 6.2831855f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Acos(float x) => MathF.Acos(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Acosh(float x) => MathF.Acosh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Asin(float x) => MathF.Asin(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Asinh(float x) => MathF.Asinh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Atan(float x) => MathF.Atan(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Atan2(float y, float x) => MathF.Atan2(y, x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Atanh(float x) => MathF.Atanh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.BitIncrement(float x) => MathF.BitIncrement(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.BitDecrement(float x) => MathF.BitDecrement(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Cbrt(float x) => MathF.Cbrt(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Ceiling(float x) => MathF.Ceiling(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.CopySign(float x, float y) => MathF.CopySign(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Cos(float x) => MathF.Cos(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Cosh(float x) => MathF.Cosh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Exp(float x) => MathF.Exp(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Floor(float x) => MathF.Floor(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.FusedMultiplyAdd(
      float left,
      float right,
      float addend)
    {
      return MathF.FusedMultiplyAdd(left, right, addend);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.IEEERemainder(float left, float right) => MathF.IEEERemainder(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TInteger IFloatingPoint<float>.ILogB<TInteger>(float x) => INumber<TInteger>.Create<int>(MathF.ILogB(x));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Log(float x) => MathF.Log(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Log(float x, float newBase) => MathF.Log(x, newBase);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Log2(float x) => MathF.Log2(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Log10(float x) => MathF.Log10(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.MaxMagnitude(float x, float y) => MathF.MaxMagnitude(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.MinMagnitude(float x, float y) => MathF.MinMagnitude(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Pow(float x, float y) => MathF.Pow(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Round(float x) => MathF.Round(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Round<TInteger>(float x, TInteger digits) => MathF.Round(x, int.Create<TInteger>(digits));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Round(float x, MidpointRounding mode) => MathF.Round(x, mode);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Round<TInteger>(
      float x,
      TInteger digits,
      MidpointRounding mode)
    {
      return MathF.Round(x, int.Create<TInteger>(digits), mode);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.ScaleB<TInteger>(float x, TInteger n) => MathF.ScaleB(x, int.Create<TInteger>(n));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Sin(float x) => MathF.Sin(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Sinh(float x) => MathF.Sinh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Sqrt(float x) => MathF.Sqrt(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Tan(float x) => MathF.Tan(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Tanh(float x) => MathF.Tanh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IFloatingPoint<float>.Truncate(float x) => MathF.Truncate(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsFinite(float x) => float.IsFinite(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsInfinity(float x) => float.IsInfinity(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsNaN(float x) => float.IsNaN(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsNegative(float x) => float.IsNegative(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsNegativeInfinity(float x) => float.IsNegativeInfinity(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsNormal(float x) => float.IsNormal(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsPositiveInfinity(float x) => float.IsPositiveInfinity(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<float>.IsSubnormal(float x) => float.IsSubnormal(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IIncrementOperators<float>.op_Increment(float value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IMinMaxValue<float>.MinValue => float.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IMinMaxValue<float>.MaxValue => float.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IModulusOperators<float, float, float>.op_Modulus(
      float left,
      float right)
    {
      return left % right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IMultiplicativeIdentity<float, float>.MultiplicativeIdentity => 1f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IMultiplyOperators<float, float, float>.op_Multiply(
      float left,
      float right)
    {
      return left * right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.One => 1f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Zero => 0.0f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Abs(float value) => MathF.Abs(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Clamp(float value, float min, float max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    float INumber<float>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (float) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (float) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (float) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (float) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (float) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (float) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (float) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (float) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (float) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (float) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (float) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (float) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (float) (IntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0.0f;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    float INumber<float>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (float) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (float) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (float) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (float) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (float) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (float) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (float) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (float) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (float) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (float) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (float) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (float) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (float) (IntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0.0f;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    float INumber<float>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (float) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (float) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (float) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (float) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (float) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (float) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (float) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (float) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (float) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (float) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (float) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (float) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (float) (IntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0.0f;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (float Quotient, float Remainder) INumber<float>.DivRem(float left, float right) => (left / right, left % right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Max(float x, float y) => MathF.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Min(float x, float y) => MathF.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Parse(string s, NumberStyles style, IFormatProvider provider) => float.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return float.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float INumber<float>.Sign(float value) => (float) MathF.Sign(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<float>.TryCreate<TOther>(TOther value, out float result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (float) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (float) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        result = (float) (Decimal) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        result = (float) (double) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (float) (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        result = (float) (int) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        result = (float) (long) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        result = (float) (IntPtr) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (float) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        result = (float) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (float) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (float) (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        result = (float) (ulong) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        result = (float) (IntPtr) (object) value;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = 0.0f;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<float>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out float result)
    {
      return float.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<float>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out float result)
    {
      return float.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IParseable<float>.Parse(string s, IFormatProvider provider) => float.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<float>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out float result) => float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float ISignedNumber<float>.NegativeOne => -1f;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float ISpanParseable<float>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => float.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<float>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out float result)
    {
      return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float ISubtractionOperators<float, float, float>.op_Subtraction(
      float left,
      float right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IUnaryNegationOperators<float, float>.op_UnaryNegation(float value) => -value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    float IUnaryPlusOperators<float, float>.op_UnaryPlus(float value) => value;
  }
}
