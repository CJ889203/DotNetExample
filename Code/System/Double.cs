// Decompiled with JetBrains decompiler
// Type: System.Double
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
  /// <summary>Represents a double-precision floating-point number.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Double : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<double>,
    IEquatable<double>,
    IBinaryFloatingPoint<double>,
    IBinaryNumber<double>,
    IBitwiseOperators<double, double, double>,
    INumber<double>,
    IAdditionOperators<double, double, double>,
    IAdditiveIdentity<double, double>,
    IComparisonOperators<double, double>,
    IEqualityOperators<double, double>,
    IDecrementOperators<double>,
    IDivisionOperators<double, double, double>,
    IIncrementOperators<double>,
    IModulusOperators<double, double, double>,
    IMultiplicativeIdentity<double, double>,
    IMultiplyOperators<double, double, double>,
    ISpanParseable<double>,
    IParseable<double>,
    ISubtractionOperators<double, double, double>,
    IUnaryNegationOperators<double, double>,
    IUnaryPlusOperators<double, double>,
    IFloatingPoint<double>,
    ISignedNumber<double>,
    IMinMaxValue<double>
  {
    private readonly double m_value;
    /// <summary>Represents the smallest possible value of a <see cref="T:System.Double" />. This field is constant.</summary>
    public const double MinValue = -1.7976931348623157E+308;
    /// <summary>Represents the largest possible value of a <see cref="T:System.Double" />. This field is constant.</summary>
    public const double MaxValue = 1.7976931348623157E+308;
    /// <summary>Represents the smallest positive <see cref="T:System.Double" /> value that is greater than zero. This field is constant.</summary>
    public const double Epsilon = 5E-324;
    /// <summary>Represents negative infinity. This field is constant.</summary>
    public const double NegativeInfinity = -1.0 / 0.0;
    /// <summary>Represents positive infinity. This field is constant.</summary>
    public const double PositiveInfinity = 1.0 / 0.0;
    /// <summary>Represents a value that is not a number (<see langword="NaN" />). This field is constant.</summary>
    public const double NaN = 0.0 / 0.0;

    /// <summary>Determines whether the specified value is finite (zero, subnormal, or normal).</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if the value is finite (zero, subnormal or normal); <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFinite(double d) => (BitConverter.DoubleToInt64Bits(d) & long.MaxValue) < 9218868437227405312L;

    /// <summary>Returns a value indicating whether the specified number evaluates to negative or positive infinity.</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d" /> evaluates to <see cref="F:System.Double.PositiveInfinity" /> or <see cref="F:System.Double.NegativeInfinity" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInfinity(double d) => (BitConverter.DoubleToInt64Bits(d) & long.MaxValue) == 9218868437227405312L;

    /// <summary>Returns a value that indicates whether the specified value is not a number (<see cref="F:System.Double.NaN" />).</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d" /> evaluates to <see cref="F:System.Double.NaN" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(double d) => d != d;

    /// <summary>Determines whether the specified value is negative.</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if the value is negative; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegative(double d) => BitConverter.DoubleToInt64Bits(d) < 0L;

    /// <summary>Returns a value indicating whether the specified number evaluates to negative infinity.</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d" /> evaluates to <see cref="F:System.Double.NegativeInfinity" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNegativeInfinity(double d) => d == double.NegativeInfinity;

    /// <summary>Determines whether the specified value is normal.</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if the value is normal; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsNormal(double d)
    {
      long num = BitConverter.DoubleToInt64Bits(d) & long.MaxValue;
      return num < 9218868437227405312L && num != 0L && (num & 9218868437227405312L) != 0L;
    }

    /// <summary>Returns a value indicating whether the specified number evaluates to positive infinity.</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d" /> evaluates to <see cref="F:System.Double.PositiveInfinity" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPositiveInfinity(double d) => d == double.PositiveInfinity;

    /// <summary>Determines whether the specified value is subnormal.</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>
    /// <see langword="true" /> if the value is subnormal; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsSubnormal(double d)
    {
      long num = BitConverter.DoubleToInt64Bits(d) & long.MaxValue;
      return num < 9218868437227405312L && num != 0L && (num & 9218868437227405312L) == 0L;
    }

    internal static int ExtractExponentFromBits(ulong bits) => (int) (bits >> 52) & 2047;

    internal static ulong ExtractSignificandFromBits(ulong bits) => bits & 4503599627370495UL;

    /// <summary>Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified object.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.Double" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance is less than <paramref name="value" />, or this instance is not a number (<see cref="F:System.Double.NaN" />) and <paramref name="value" /> is a number.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />, or this instance and <paramref name="value" /> are both <see langword="Double.NaN" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" /></description></item><item><term> A positive integer</term><description> This instance is greater than <paramref name="value" />, OR this instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Double.NaN" />), OR <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is double d))
        throw new ArgumentException(SR.Arg_MustBeDouble);
      if (this < d)
        return -1;
      if (this > d)
        return 1;
      if (this == d)
        return 0;
      if (!double.IsNaN(this))
        return 1;
      return !double.IsNaN(d) ? -1 : 0;
    }

    /// <summary>Compares this instance to a specified double-precision floating-point number and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified double-precision floating-point number.</summary>
    /// <param name="value">A double-precision floating-point number to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />, or this instance is not a number (<see cref="F:System.Double.NaN" />) and <paramref name="value" /> is a number.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />, or both this instance and <paramref name="value" /> are not a number (<see cref="F:System.Double.NaN" />), <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or this instance is a number and <paramref name="value" /> is not a number (<see cref="F:System.Double.NaN" />).</description></item></list></returns>
    public int CompareTo(double value)
    {
      if (this < value)
        return -1;
      if (this > value)
        return 1;
      if (this == value)
        return 0;
      if (!double.IsNaN(this))
        return 1;
      return !double.IsNaN(value) ? -1 : 0;
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Double" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
      if (!(obj is double d))
        return false;
      if (d == this)
        return true;
      return double.IsNaN(d) && double.IsNaN(this);
    }

    /// <summary>Returns a value that indicates whether two specified <see cref="T:System.Double" /> values are equal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator ==(double left, double right) => left == right;

    /// <summary>Returns a value that indicates whether two specified <see cref="T:System.Double" /> values are not equal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator !=(double left, double right) => left != right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Double" /> value is less than another specified <see cref="T:System.Double" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator <(double left, double right) => left < right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Double" /> value is greater than another specified <see cref="T:System.Double" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator >(double left, double right) => left > right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Double" /> value is less than or equal to another specified <see cref="T:System.Double" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator <=(double left, double right) => left <= right;

    /// <summary>Returns a value that indicates whether a specified <see cref="T:System.Double" /> value is greater than or equal to another specified <see cref="T:System.Double" /> value.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool operator >=(double left, double right) => left >= right;

    /// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Double" /> object represent the same value.</summary>
    /// <param name="obj">A <see cref="T:System.Double" /> object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(double obj)
    {
      if (obj == this)
        return true;
      return double.IsNaN(obj) && double.IsNaN(this);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
      long num = Unsafe.As<double, long>(ref Unsafe.AsRef<double>(in this.m_value));
      if ((num - 1L & long.MaxValue) >= 9218868437227405312L)
        num &= 9218868437227405312L;
      return (int) num ^ (int) (num >> 32);
    }

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance.</returns>
    public override string ToString() => Number.FormatDouble(this, (string) null, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatDouble(this, format, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatDouble(this, (string) null, NumberFormatInfo.GetInstance(provider));

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatDouble(this, format, NumberFormatInfo.GetInstance(provider));

    /// <summary>Tries to format the value of the current double instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      return Number.TryFormatDouble(this, format, NumberFormatInfo.GetInstance(provider), destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its double-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static double Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDouble((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its double-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s" />. A typical value to specify is a combination of <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static double Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDouble((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its double-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static double Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDouble((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its double-precision floating-point number equivalent.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
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
    /// .NET Framework and .NET Core 2.2 and earlier versions only: <paramref name="s" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static double Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDouble((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts a character span that contains the string representation of a number in a specified style and culture-specific format to its double-precision floating-point number equivalent.</summary>
    /// <param name="s">A character span that contains the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s" />.  A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not represent a numeric value.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to the numeric value or symbol specified in <paramref name="s" />.</returns>
    public static double Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return Number.ParseDouble(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number to its double-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the double-precision floating-point number equivalent of the <paramref name="s" /> parameter, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" /> or is not a number in a valid format. It also fails on .NET Framework and .NET Core 2.2 and earlier versions if <paramref name="s" /> represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out double result)
    {
      if (s != null)
        return double.TryParse((ReadOnlySpan<char>) s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);
      result = 0.0;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its double-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A character span that contains the string representation of the number to convert.</param>
    /// <param name="result">When this method returns, contains the double-precision floating-point number equivalent of the numeric value or symbol contained in <paramref name="s" /> parameter, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or empty, or is not in a format compliant with <paramref name="style" />. The conversion also fails if <paramref name="style" /> is not a valid combination of <see cref="T:System.Globalization.NumberStyles" /> enumerated constants. If <paramref name="s" /> is a valid number less than <see cref="F:System.Double.MinValue" />, <paramref name="result" /> is <see cref="F:System.Double.NegativeInfinity" />. If <paramref name="s" /> is a valid number greater than <see cref="F:System.Double.MaxValue" />, <paramref name="result" /> is <see cref="F:System.Double.PositiveInfinity" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out double result) => double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its double-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains a double-precision floating-point number equivalent of the numeric value or symbol contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" /> or is not in a format compliant with <paramref name="style" />, or if <paramref name="style" /> is not a valid combination of <see cref="T:System.Globalization.NumberStyles" /> enumeration constants. It also fails on .NET Framework or .NET Core 2.2 and earlier versions if <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> includes the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      NumberStyles style,
      IFormatProvider? provider,
      out double result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s != null)
        return double.TryParse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result);
      result = 0.0;
      return false;
    }

    /// <summary>Converts a character span containing the string representation of a number in a specified style and culture-specific format to its double-precision floating-point number equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A read-only character span that contains the number to convert.</param>
    /// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Float" /> combined with <see cref="F:System.Globalization.NumberStyles.AllowThousands" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns and if the conversion succeeded, contains a double-precision floating-point number equivalent of the numeric value or symbol contained in <paramref name="s" />. Contains zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" />, an empty character span, or not a number in a format compliant with <paramref name="style" />. If <paramref name="s" /> is a valid number less than <see cref="F:System.Double.MinValue" />, <paramref name="result" /> is <see cref="F:System.Double.NegativeInfinity" />. If <paramref name="s" /> is a valid number greater than <see cref="F:System.Double.MaxValue" />, <paramref name="result" /> is <see cref="F:System.Double.PositiveInfinity" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out double result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return double.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
    }


    #nullable disable
    private static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      NumberFormatInfo info,
      out double result)
    {
      return Number.TryParseDouble(s, style, info, out result);
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Double" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.Double" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Double;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>
    /// <see langword="true" /> if the value of the current instance is not zero; otherwise, <see langword="false" />.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(this);

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Double), (object) "Char"));

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
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Single" />.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, unchanged.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => this;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Decimal" />.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(this);

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Double), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.Double" /> value.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies culture-specific information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IAdditionOperators<double, double, double>.op_Addition(
      double left,
      double right)
    {
      return left + right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IAdditiveIdentity<double, double>.AdditiveIdentity => 0.0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<double>.IsPow2(double value)
    {
      ulong uint64Bits = BitConverter.DoubleToUInt64Bits(value);
      uint num1 = (uint) (uint64Bits >> 52) & 2047U;
      ulong num2 = uint64Bits & 4503599627370495UL;
      return value > 0.0 && num1 != 0U && num1 != 2047U && num2 == 0UL;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IBinaryNumber<double>.Log2(double value) => Math.Log2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IBitwiseOperators<double, double, double>.op_BitwiseAnd(
      double left,
      double right)
    {
      return BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(left) & BitConverter.DoubleToUInt64Bits(right));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IBitwiseOperators<double, double, double>.op_BitwiseOr(
      double left,
      double right)
    {
      return BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(left) | BitConverter.DoubleToUInt64Bits(right));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IBitwiseOperators<double, double, double>.op_ExclusiveOr(
      double left,
      double right)
    {
      return BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(left) ^ BitConverter.DoubleToUInt64Bits(right));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IBitwiseOperators<double, double, double>.op_OnesComplement(double value) => BitConverter.UInt64BitsToDouble(~BitConverter.DoubleToUInt64Bits(value));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<double, double>.op_LessThan(
      double left,
      double right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<double, double>.op_LessThanOrEqual(
      double left,
      double right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<double, double>.op_GreaterThan(
      double left,
      double right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<double, double>.op_GreaterThanOrEqual(
      double left,
      double right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IDecrementOperators<double>.op_Decrement(double value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IDivisionOperators<double, double, double>.op_Division(
      double left,
      double right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<double, double>.op_Equality(double left, double right) => left == right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<double, double>.op_Inequality(
      double left,
      double right)
    {
      return left != right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.E => Math.E;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Epsilon => double.Epsilon;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.NaN => double.NaN;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.NegativeInfinity => double.NegativeInfinity;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.NegativeZero => -0.0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Pi => Math.PI;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.PositiveInfinity => double.PositiveInfinity;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Tau => 2.0 * Math.PI;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Acos(double x) => Math.Acos(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Acosh(double x) => Math.Acosh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Asin(double x) => Math.Asin(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Asinh(double x) => Math.Asinh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Atan(double x) => Math.Atan(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Atan2(double y, double x) => Math.Atan2(y, x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Atanh(double x) => Math.Atanh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.BitIncrement(double x) => Math.BitIncrement(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.BitDecrement(double x) => Math.BitDecrement(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Cbrt(double x) => Math.Cbrt(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Ceiling(double x) => Math.Ceiling(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.CopySign(double x, double y) => Math.CopySign(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Cos(double x) => Math.Cos(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Cosh(double x) => Math.Cosh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Exp(double x) => Math.Exp(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Floor(double x) => Math.Floor(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.FusedMultiplyAdd(
      double left,
      double right,
      double addend)
    {
      return Math.FusedMultiplyAdd(left, right, addend);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.IEEERemainder(double left, double right) => Math.IEEERemainder(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TInteger IFloatingPoint<double>.ILogB<TInteger>(double x) => INumber<TInteger>.Create<int>(Math.ILogB(x));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Log(double x) => Math.Log(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Log(double x, double newBase) => Math.Log(x, newBase);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Log2(double x) => Math.Log2(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Log10(double x) => Math.Log10(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.MaxMagnitude(double x, double y) => Math.MaxMagnitude(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.MinMagnitude(double x, double y) => Math.MinMagnitude(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Pow(double x, double y) => Math.Pow(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Round(double x) => Math.Round(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Round<TInteger>(double x, TInteger digits) => Math.Round(x, int.Create<TInteger>(digits));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Round(double x, MidpointRounding mode) => Math.Round(x, mode);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Round<TInteger>(
      double x,
      TInteger digits,
      MidpointRounding mode)
    {
      return Math.Round(x, int.Create<TInteger>(digits), mode);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.ScaleB<TInteger>(double x, TInteger n) => Math.ScaleB(x, int.Create<TInteger>(n));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Sin(double x) => Math.Sin(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Sinh(double x) => Math.Sinh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Sqrt(double x) => Math.Sqrt(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Tan(double x) => Math.Tan(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Tanh(double x) => Math.Tanh(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IFloatingPoint<double>.Truncate(double x) => Math.Truncate(x);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsFinite(double d) => double.IsFinite(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsInfinity(double d) => double.IsInfinity(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsNaN(double d) => double.IsNaN(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsNegative(double d) => double.IsNegative(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsNegativeInfinity(double d) => double.IsNegativeInfinity(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsNormal(double d) => double.IsNormal(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsPositiveInfinity(double d) => double.IsPositiveInfinity(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IFloatingPoint<double>.IsSubnormal(double d) => double.IsSubnormal(d);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IIncrementOperators<double>.op_Increment(double value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IMinMaxValue<double>.MinValue => double.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IMinMaxValue<double>.MaxValue => double.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IModulusOperators<double, double, double>.op_Modulus(
      double left,
      double right)
    {
      return left % right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IMultiplicativeIdentity<double, double>.MultiplicativeIdentity => 1.0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IMultiplyOperators<double, double, double>.op_Multiply(
      double left,
      double right)
    {
      return left * right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.One => 1.0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Zero => 0.0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Abs(double value) => Math.Abs(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Clamp(double value, double min, double max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    double INumber<double>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (double) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (double) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (double) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (double) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (double) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (double) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (double) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (double) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (double) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (double) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (double) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (double) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (double) (IntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0.0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    double INumber<double>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (double) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (double) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (double) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (double) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (double) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (double) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (double) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (double) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (double) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (double) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (double) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (double) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (double) (IntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0.0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    double INumber<double>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (double) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (double) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (double) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (double) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (double) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (double) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (double) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (double) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (double) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (double) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (double) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (double) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (double) (IntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0.0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (double Quotient, double Remainder) INumber<double>.DivRem(double left, double right) => (left / right, left % right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Max(double x, double y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Min(double x, double y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Parse(string s, NumberStyles style, IFormatProvider provider) => double.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return double.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double INumber<double>.Sign(double value) => (double) Math.Sign(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<double>.TryCreate<TOther>(TOther value, out double result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (double) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (double) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        result = (double) (Decimal) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        result = (double) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (double) (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        result = (double) (int) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        result = (double) (long) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        result = (double) (IntPtr) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (double) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        result = (double) (float) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (double) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (double) (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        result = (double) (ulong) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        result = (double) (IntPtr) (object) value;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = 0.0;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<double>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out double result)
    {
      return double.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<double>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out double result)
    {
      return double.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IParseable<double>.Parse(string s, IFormatProvider provider) => double.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<double>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out double result)
    {
      return double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double ISignedNumber<double>.NegativeOne => -1.0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double ISpanParseable<double>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return double.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<double>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out double result)
    {
      return double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double ISubtractionOperators<double, double, double>.op_Subtraction(
      double left,
      double right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IUnaryNegationOperators<double, double>.op_UnaryNegation(
      double value)
    {
      return -value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IUnaryPlusOperators<double, double>.op_UnaryPlus(double value) => value;
  }
}
