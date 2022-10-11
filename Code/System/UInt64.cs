// Decompiled with JetBrains decompiler
// Type: System.UInt64
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
  /// <summary>Represents a 64-bit unsigned integer.</summary>
  [CLSCompliant(false)]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct UInt64 : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<ulong>,
    IEquatable<ulong>,
    IBinaryInteger<ulong>,
    IBinaryNumber<ulong>,
    IBitwiseOperators<ulong, ulong, ulong>,
    INumber<ulong>,
    IAdditionOperators<ulong, ulong, ulong>,
    IAdditiveIdentity<ulong, ulong>,
    IComparisonOperators<ulong, ulong>,
    IEqualityOperators<ulong, ulong>,
    IDecrementOperators<ulong>,
    IDivisionOperators<ulong, ulong, ulong>,
    IIncrementOperators<ulong>,
    IModulusOperators<ulong, ulong, ulong>,
    IMultiplicativeIdentity<ulong, ulong>,
    IMultiplyOperators<ulong, ulong, ulong>,
    ISpanParseable<ulong>,
    IParseable<ulong>,
    ISubtractionOperators<ulong, ulong, ulong>,
    IUnaryNegationOperators<ulong, ulong>,
    IUnaryPlusOperators<ulong, ulong>,
    IShiftOperators<ulong, ulong>,
    IMinMaxValue<ulong>,
    IUnsignedNumber<ulong>
  {
    private readonly ulong m_value;
    /// <summary>Represents the largest possible value of <see cref="T:System.UInt64" />. This field is constant.</summary>
    public const ulong MaxValue = 18446744073709551615;
    /// <summary>Represents the smallest possible value of <see cref="T:System.UInt64" />. This field is constant.</summary>
    public const ulong MinValue = 0;

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.UInt64" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is ulong num))
        throw new ArgumentException(SR.Arg_MustBeUInt64);
      if (this < num)
        return -1;
      return this > num ? 1 : 0;
    }

    /// <summary>Compares this instance to a specified 64-bit unsigned integer and returns an indication of their relative values.</summary>
    /// <param name="value">An unsigned integer to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(ulong value)
    {
      if (this < value)
        return -1;
      return this > value ? 1 : 0;
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.UInt64" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ulong num && (long) this == (long) num;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.UInt64" /> value.</summary>
    /// <param name="obj">A <see cref="T:System.UInt64" /> value to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(ulong obj) => (long) this == (long) obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this ^ (int) (this >> 32);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance, consisting of a sequence of digits ranging from 0 to 9, without a sign or leading zeroes.</returns>
    public override string ToString() => Number.UInt64ToDecStr(this, -1);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance, consisting of a sequence of digits ranging from 0 to 9, without a sign or leading zeros.</returns>
    public string ToString(IFormatProvider? provider) => Number.UInt64ToDecStr(this, -1);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatUInt64(this, format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about this instance.</param>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatUInt64(this, format, provider);

    /// <summary>Tries to format the value of the current unsigned long number instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format of <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      return Number.TryFormatUInt64(this, format, provider, destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its 64-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static ulong Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt64((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its 64-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of the enumeration values that specifies the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in a format compliant with <paramref name="style" />.</exception>
    /// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 64-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ulong Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt64((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its 64-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in the correct style.</exception>
    /// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ulong Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt64((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 64-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not in a format compliant with <paramref name="style" />.</exception>
    /// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 64-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ulong Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt64((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 64-bit unsigned integer equivalent.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert. The span is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <returns>A 64-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ulong Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.ParseUInt64(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Tries to convert the string representation of a number to its 64-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <param name="result">When this method returns, contains the 64-bit unsigned integer value that is equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out ulong result)
    {
      if (s != null)
        return Number.TryParseUInt64IntegerStyle((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
      result = 0UL;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number to its 64-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert.</param>
    /// <param name="result">When this method returns, contains the 64-bit unsigned integer value that is equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out ulong result) => Number.TryParseUInt64IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;

    /// <summary>Tries to convert the string representation of a number in a specified style and culture-specific format to its 64-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 64-bit unsigned integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      NumberStyles style,
      IFormatProvider? provider,
      out ulong result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return Number.TryParseUInt64((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
      result = 0UL;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number in a specified style and culture-specific format to its 64-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert. The span is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 64-bit unsigned integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out ulong result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.TryParseUInt64(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.UInt64" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.UInt64" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.UInt64;


    #nullable disable
    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>
    /// <see langword="true" /> if the value of the current instance is not zero; otherwise, <see langword="false" />.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToChar(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Char" />.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(this);

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
    /// <returns>The value of the current instance, unchanged.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => this;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Single" />.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(this);

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
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (UInt64), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.UInt64" /> value.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IAdditionOperators<ulong, ulong, ulong>.op_Addition(
      ulong left,
      ulong right)
    {
      return left + right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IAdditiveIdentity<ulong, ulong>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBinaryInteger<ulong>.LeadingZeroCount(ulong value) => (ulong) BitOperations.LeadingZeroCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBinaryInteger<ulong>.PopCount(ulong value) => (ulong) BitOperations.PopCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBinaryInteger<ulong>.RotateLeft(ulong value, int rotateAmount) => BitOperations.RotateLeft(value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBinaryInteger<ulong>.RotateRight(ulong value, int rotateAmount) => BitOperations.RotateRight(value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBinaryInteger<ulong>.TrailingZeroCount(ulong value) => (ulong) BitOperations.TrailingZeroCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<ulong>.IsPow2(ulong value) => BitOperations.IsPow2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBinaryNumber<ulong>.Log2(ulong value) => (ulong) BitOperations.Log2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBitwiseOperators<ulong, ulong, ulong>.op_BitwiseAnd(
      ulong left,
      ulong right)
    {
      return left & right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBitwiseOperators<ulong, ulong, ulong>.op_BitwiseOr(
      ulong left,
      ulong right)
    {
      return left | right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBitwiseOperators<ulong, ulong, ulong>.op_ExclusiveOr(
      ulong left,
      ulong right)
    {
      return left ^ right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IBitwiseOperators<ulong, ulong, ulong>.op_OnesComplement(ulong value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ulong, ulong>.op_LessThan(ulong left, ulong right) => left < right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ulong, ulong>.op_LessThanOrEqual(
      ulong left,
      ulong right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ulong, ulong>.op_GreaterThan(
      ulong left,
      ulong right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ulong, ulong>.op_GreaterThanOrEqual(
      ulong left,
      ulong right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IDecrementOperators<ulong>.op_Decrement(ulong value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IDivisionOperators<ulong, ulong, ulong>.op_Division(
      ulong left,
      ulong right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<ulong, ulong>.op_Equality(ulong left, ulong right) => (long) left == (long) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<ulong, ulong>.op_Inequality(ulong left, ulong right) => (long) left != (long) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IIncrementOperators<ulong>.op_Increment(ulong value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IMinMaxValue<ulong>.MinValue => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IMinMaxValue<ulong>.MaxValue => ulong.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IModulusOperators<ulong, ulong, ulong>.op_Modulus(
      ulong left,
      ulong right)
    {
      return left % right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IMultiplicativeIdentity<ulong, ulong>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IMultiplyOperators<ulong, ulong, ulong>.op_Multiply(
      ulong left,
      ulong right)
    {
      return left * right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Abs(ulong value) => value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Clamp(ulong value, ulong min, ulong max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ulong INumber<ulong>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (ulong) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (ulong) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (ulong) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((ulong) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return checked ((ulong) (short) (object) value);
      if (typeof (TOther) == typeof (int))
        return checked ((ulong) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((ulong) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((ulong) (UIntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return checked ((ulong) (sbyte) (object) value);
      if (typeof (TOther) == typeof (float))
        return checked ((ulong) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (ulong) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (ulong) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ulong INumber<ulong>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (ulong) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (ulong) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 18446744073709551615M)
          return ulong.MaxValue;
        return !(num < 0M) ? (ulong) num : 0UL;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > 1.8446744073709552E+19)
          return ulong.MaxValue;
        return num >= 0.0 ? (ulong) num : 0UL;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        return num >= (short) 0 ? (ulong) num : 0UL;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        return num >= 0 ? (ulong) num : 0UL;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        return num >= 0L ? (ulong) num : 0UL;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        return num >= IntPtr.Zero ? (ulong) (long) num : 0UL;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        return num >= (sbyte) 0 ? (ulong) num : 0UL;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > 1.8446744073709552E+19)
          return ulong.MaxValue;
        return (double) num >= 0.0 ? (ulong) num : 0UL;
      }
      if (typeof (TOther) == typeof (ushort))
        return (ulong) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (ulong) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ulong INumber<ulong>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (ulong) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (ulong) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (ulong) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (ulong) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (ulong) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (ulong) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (ulong) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (ulong) (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (ulong) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (ulong) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (ulong) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (ulong) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (ulong Quotient, ulong Remainder) INumber<ulong>.DivRem(ulong left, ulong right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Max(ulong x, ulong y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Min(ulong x, ulong y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Parse(string s, NumberStyles style, IFormatProvider provider) => ulong.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return ulong.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong INumber<ulong>.Sign(ulong value) => value == 0UL ? 0UL : 1UL;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<ulong>.TryCreate<TOther>(TOther value, out ulong result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (ulong) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (ulong) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < 0M || num > 18446744073709551615M)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < 0.0 || num > 1.8446744073709552E+19)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num < (short) 0)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < 0)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < 0L)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < IntPtr.Zero)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) (long) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        if (num < (sbyte) 0)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < 0.0 || (double) num > 1.8446744073709552E+19)
        {
          result = 0UL;
          return false;
        }
        result = (ulong) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (ulong) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (ulong) (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        result = (ulong) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        result = (ulong) (UIntPtr) (object) value;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = 0UL;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<ulong>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out ulong result)
    {
      return ulong.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<ulong>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out ulong result)
    {
      return ulong.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IParseable<ulong>.Parse(string s, IFormatProvider provider) => ulong.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<ulong>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out ulong result) => ulong.TryParse(s, NumberStyles.Integer, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IShiftOperators<ulong, ulong>.op_LeftShift(ulong value, int shiftAmount) => value << shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IShiftOperators<ulong, ulong>.op_RightShift(ulong value, int shiftAmount) => value >> shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong ISpanParseable<ulong>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => ulong.Parse(s, NumberStyles.Integer, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<ulong>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out ulong result)
    {
      return ulong.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong ISubtractionOperators<ulong, ulong, ulong>.op_Subtraction(
      ulong left,
      ulong right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IUnaryNegationOperators<ulong, ulong>.op_UnaryNegation(ulong value) => 0UL - value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ulong IUnaryPlusOperators<ulong, ulong>.op_UnaryPlus(ulong value) => value;
  }
}
