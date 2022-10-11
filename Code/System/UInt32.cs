// Decompiled with JetBrains decompiler
// Type: System.UInt32
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
  /// <summary>Represents a 32-bit unsigned integer.</summary>
  [CLSCompliant(false)]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct UInt32 : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<uint>,
    IEquatable<uint>,
    IBinaryInteger<uint>,
    IBinaryNumber<uint>,
    IBitwiseOperators<uint, uint, uint>,
    INumber<uint>,
    IAdditionOperators<uint, uint, uint>,
    IAdditiveIdentity<uint, uint>,
    IComparisonOperators<uint, uint>,
    IEqualityOperators<uint, uint>,
    IDecrementOperators<uint>,
    IDivisionOperators<uint, uint, uint>,
    IIncrementOperators<uint>,
    IModulusOperators<uint, uint, uint>,
    IMultiplicativeIdentity<uint, uint>,
    IMultiplyOperators<uint, uint, uint>,
    ISpanParseable<uint>,
    IParseable<uint>,
    ISubtractionOperators<uint, uint, uint>,
    IUnaryNegationOperators<uint, uint>,
    IUnaryPlusOperators<uint, uint>,
    IShiftOperators<uint, uint>,
    IMinMaxValue<uint>,
    IUnsignedNumber<uint>
  {
    private readonly uint m_value;
    /// <summary>Represents the largest possible value of <see cref="T:System.UInt32" />. This field is constant.</summary>
    public const uint MaxValue = 4294967295;
    /// <summary>Represents the smallest possible value of <see cref="T:System.UInt32" />. This field is constant.</summary>
    public const uint MinValue = 0;

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.UInt32" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is uint num))
        throw new ArgumentException(SR.Arg_MustBeUInt32);
      if (this < num)
        return -1;
      return this > num ? 1 : 0;
    }

    /// <summary>Compares this instance to a specified 32-bit unsigned integer and returns an indication of their relative values.</summary>
    /// <param name="value">An unsigned integer to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(uint value)
    {
      if (this < value)
        return -1;
      return this > value ? 1 : 0;
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.UInt32" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is uint num && (int) this == (int) num;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.UInt32" />.</summary>
    /// <param name="obj">A value to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(uint obj) => (int) this == (int) obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this;

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance, consisting of a sequence of digits ranging from 0 to 9, without a sign or leading zeroes.</returns>
    public override string ToString() => Number.UInt32ToDecStr(this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance, which consists of a sequence of digits ranging from 0 to 9, without a sign or leading zeros.</returns>
    public string ToString(IFormatProvider? provider) => Number.UInt32ToDecStr(this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatUInt32(this, format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about this instance.</param>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatUInt32(this, format, provider);

    /// <summary>Tries to format the value of the current unsigned integer number instance into the provided span of characters.</summary>
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
      return Number.TryFormatUInt32(this, format, provider, destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its 32-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string representing the number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The <paramref name="s" /> parameter is not of the correct format.</exception>
    /// <exception cref="T:System.OverflowException">The <paramref name="s" /> parameter represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static uint Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt32((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its 32-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string representing the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of the enumeration values that specify the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in a format compliant with <paramref name="style" />.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="s" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 32-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static uint Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt32((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its 32-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct style.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static uint Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt32((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 32-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string representing the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in a format compliant with <paramref name="style" />.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="s" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 32-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static uint Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseUInt32((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 32-bit unsigned integer equivalent.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert. The span is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <returns>A 32-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static uint Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Tries to convert the string representation of a number to its 32-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <param name="result">When this method returns, contains the 32-bit unsigned integer value that is equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out uint result)
    {
      if (s != null)
        return Number.TryParseUInt32IntegerStyle((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
      result = 0U;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number to its 32-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert.</param>
    /// <param name="result">When this method returns, contains the 32-bit unsigned integer value that is equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out uint result) => Number.TryParseUInt32IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;

    /// <summary>Tries to convert the string representation of a number in a specified style and culture-specific format to its 32-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 32-bit unsigned integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
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
      out uint result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return Number.TryParseUInt32((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
      result = 0U;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number in a specified style and culture-specific format to its 32-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert. The span is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 32-bit unsigned integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out uint result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.UInt32" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.UInt32" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.UInt32;


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
    /// <returns>The value of the current instance, unchanged.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => this;

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
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (UInt32), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.UInt32" /> value.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies culture-specific information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IAdditionOperators<uint, uint, uint>.op_Addition(uint left, uint right) => left + right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IAdditiveIdentity<uint, uint>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBinaryInteger<uint>.LeadingZeroCount(uint value) => (uint) BitOperations.LeadingZeroCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBinaryInteger<uint>.PopCount(uint value) => (uint) BitOperations.PopCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBinaryInteger<uint>.RotateLeft(uint value, int rotateAmount) => BitOperations.RotateLeft(value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBinaryInteger<uint>.RotateRight(uint value, int rotateAmount) => BitOperations.RotateRight(value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBinaryInteger<uint>.TrailingZeroCount(uint value) => (uint) BitOperations.TrailingZeroCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<uint>.IsPow2(uint value) => BitOperations.IsPow2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBinaryNumber<uint>.Log2(uint value) => (uint) BitOperations.Log2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBitwiseOperators<uint, uint, uint>.op_BitwiseAnd(uint left, uint right) => left & right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBitwiseOperators<uint, uint, uint>.op_BitwiseOr(uint left, uint right) => left | right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBitwiseOperators<uint, uint, uint>.op_ExclusiveOr(uint left, uint right) => left ^ right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IBitwiseOperators<uint, uint, uint>.op_OnesComplement(uint value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<uint, uint>.op_LessThan(uint left, uint right) => left < right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<uint, uint>.op_LessThanOrEqual(
      uint left,
      uint right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<uint, uint>.op_GreaterThan(uint left, uint right) => left > right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<uint, uint>.op_GreaterThanOrEqual(
      uint left,
      uint right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IDecrementOperators<uint>.op_Decrement(uint value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IDivisionOperators<uint, uint, uint>.op_Division(uint left, uint right) => left / right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<uint, uint>.op_Equality(uint left, uint right) => (int) left == (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<uint, uint>.op_Inequality(uint left, uint right) => (int) left != (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IIncrementOperators<uint>.op_Increment(uint value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IMinMaxValue<uint>.MinValue => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IMinMaxValue<uint>.MaxValue => uint.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IModulusOperators<uint, uint, uint>.op_Modulus(uint left, uint right) => left % right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IMultiplicativeIdentity<uint, uint>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IMultiplyOperators<uint, uint, uint>.op_Multiply(uint left, uint right) => left * right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Abs(uint value) => value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Clamp(uint value, uint min, uint max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    uint INumber<uint>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (uint) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (uint) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (uint) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((uint) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return checked ((uint) (short) (object) value);
      if (typeof (TOther) == typeof (int))
        return checked ((uint) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((uint) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((uint) (UIntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return checked ((uint) (sbyte) (object) value);
      if (typeof (TOther) == typeof (float))
        return checked ((uint) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (uint) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return checked ((uint) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((uint) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    uint INumber<uint>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (uint) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (uint) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 4294967295M)
          return uint.MaxValue;
        return !(num < 0M) ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) uint.MaxValue)
          return uint.MaxValue;
        return num >= 0.0 ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        return num >= (short) 0 ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        return num >= 0 ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) uint.MaxValue)
          return uint.MaxValue;
        return num >= 0L ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if ((long) num > (long) uint.MaxValue)
          return uint.MaxValue;
        return num >= IntPtr.Zero ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        return num >= (sbyte) 0 ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > 4294967296.0)
          return uint.MaxValue;
        return (double) num >= 0.0 ? (uint) num : 0U;
      }
      if (typeof (TOther) == typeof (ushort))
        return (uint) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) uint.MaxValue ? (uint) num : uint.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= new UIntPtr(18446744073709551615) ? (uint) num : uint.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    uint INumber<uint>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (uint) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (uint) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (uint) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (uint) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (uint) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (uint) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (uint) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (uint) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (uint) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (uint) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (uint) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (uint) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (uint) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (uint Quotient, uint Remainder) INumber<uint>.DivRem(uint left, uint right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Max(uint x, uint y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Min(uint x, uint y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Parse(string s, NumberStyles style, IFormatProvider provider) => uint.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return uint.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint INumber<uint>.Sign(uint value) => value != 0U ? 1U : 0U;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<uint>.TryCreate<TOther>(TOther value, out uint result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (uint) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (uint) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < 0M || num > 4294967295M)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < 0.0 || num > (double) uint.MaxValue)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num < (short) 0)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < 0)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < 0L || num > (long) uint.MaxValue)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < IntPtr.Zero || (long) num > (long) uint.MaxValue)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        if (num < (sbyte) 0)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < 0.0 || (double) num > 4294967296.0)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (uint) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) uint.MaxValue)
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > new UIntPtr(18446744073709551615))
        {
          result = 0U;
          return false;
        }
        result = (uint) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = 0U;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<uint>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out uint result)
    {
      return uint.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<uint>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out uint result)
    {
      return uint.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IParseable<uint>.Parse(string s, IFormatProvider provider) => uint.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<uint>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out uint result) => uint.TryParse(s, NumberStyles.Integer, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IShiftOperators<uint, uint>.op_LeftShift(uint value, int shiftAmount) => value << shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IShiftOperators<uint, uint>.op_RightShift(uint value, int shiftAmount) => value >> shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint ISpanParseable<uint>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => uint.Parse(s, NumberStyles.Integer, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<uint>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out uint result)
    {
      return uint.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint ISubtractionOperators<uint, uint, uint>.op_Subtraction(
      uint left,
      uint right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IUnaryNegationOperators<uint, uint>.op_UnaryNegation(uint value) => (uint) -(int) value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    uint IUnaryPlusOperators<uint, uint>.op_UnaryPlus(uint value) => value;
  }
}
