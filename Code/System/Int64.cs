// Decompiled with JetBrains decompiler
// Type: System.Int64
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
  /// <summary>Represents a 64-bit signed integer.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Int64 : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<long>,
    IEquatable<long>,
    IBinaryInteger<long>,
    IBinaryNumber<long>,
    IBitwiseOperators<long, long, long>,
    INumber<long>,
    IAdditionOperators<long, long, long>,
    IAdditiveIdentity<long, long>,
    IComparisonOperators<long, long>,
    IEqualityOperators<long, long>,
    IDecrementOperators<long>,
    IDivisionOperators<long, long, long>,
    IIncrementOperators<long>,
    IModulusOperators<long, long, long>,
    IMultiplicativeIdentity<long, long>,
    IMultiplyOperators<long, long, long>,
    ISpanParseable<long>,
    IParseable<long>,
    ISubtractionOperators<long, long, long>,
    IUnaryNegationOperators<long, long>,
    IUnaryPlusOperators<long, long>,
    IShiftOperators<long, long>,
    IMinMaxValue<long>,
    ISignedNumber<long>
  {
    private readonly long m_value;
    /// <summary>Represents the largest possible value of an <see cref="T:System.Int64" />. This field is constant.</summary>
    public const long MaxValue = 9223372036854775807;
    /// <summary>Represents the smallest possible value of an <see cref="T:System.Int64" />. This field is constant.</summary>
    public const long MinValue = -9223372036854775808;

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not an <see cref="T:System.Int64" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is long num))
        throw new ArgumentException(SR.Arg_MustBeInt64);
      if (this < num)
        return -1;
      return this > num ? 1 : 0;
    }

    /// <summary>Compares this instance to a specified 64-bit signed integer and returns an indication of their relative values.</summary>
    /// <param name="value">An integer to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(long value)
    {
      if (this < value)
        return -1;
      return this > value ? 1 : 0;
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of an <see cref="T:System.Int64" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is long num && this == num;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.Int64" /> value.</summary>
    /// <param name="obj">An <see cref="T:System.Int64" /> value to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(long obj) => this == obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this ^ (int) (this >> 32);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance, consisting of a minus sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
    public override string ToString() => Number.Int64ToDecStr(this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatInt64(this, (string) null, provider);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid or not supported.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatInt64(this, format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about this instance.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid or not supported.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatInt64(this, format, provider);

    /// <summary>Tries to format the value of the current long number instance into the provided span of characters.</summary>
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
      return Number.TryFormatInt64(this, format, provider, destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its 64-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static long Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt64((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its 64-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
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
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> supports fractional digits but <paramref name="s" /> includes non-zero fractional digits.</exception>
    /// <returns>A 64-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static long Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt64((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its 64-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static long Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt64((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 64-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />.</param>
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
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> supports fractional digits, but <paramref name="s" /> includes non-zero fractional digits.</exception>
    /// <returns>A 64-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static long Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt64((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 64-bit signed integer equivalent.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <returns>A 64-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static long Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.ParseInt64(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number to its 64-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the 64-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out long result)
    {
      if (s != null)
        return Number.TryParseInt64IntegerStyle((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
      result = 0L;
      return false;
    }

    /// <summary>Converts the span representation of a number to its 64-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="result">When this method returns, contains the 64-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out long result) => Number.TryParseInt64IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 64-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 64-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
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
      out long result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return Number.TryParseInt64((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
      result = 0L;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 64-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert. The span is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 64-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out long result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.TryParseInt64(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Int64" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.Int64" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Int64;


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
    /// <returns>The value of the current instance, unchanged.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => this;

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
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Int64), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.Int64" /> value.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that provides information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IAdditionOperators<long, long, long>.op_Addition(long left, long right) => left + right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IAdditiveIdentity<long, long>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBinaryInteger<long>.LeadingZeroCount(long value) => (long) BitOperations.LeadingZeroCount((ulong) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBinaryInteger<long>.PopCount(long value) => (long) BitOperations.PopCount((ulong) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBinaryInteger<long>.RotateLeft(long value, int rotateAmount) => (long) BitOperations.RotateLeft((ulong) value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBinaryInteger<long>.RotateRight(long value, int rotateAmount) => (long) BitOperations.RotateRight((ulong) value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBinaryInteger<long>.TrailingZeroCount(long value) => (long) BitOperations.TrailingZeroCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<long>.IsPow2(long value) => BitOperations.IsPow2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBinaryNumber<long>.Log2(long value)
    {
      if (value < 0L)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      return (long) BitOperations.Log2((ulong) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBitwiseOperators<long, long, long>.op_BitwiseAnd(long left, long right) => left & right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBitwiseOperators<long, long, long>.op_BitwiseOr(long left, long right) => left | right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBitwiseOperators<long, long, long>.op_ExclusiveOr(long left, long right) => left ^ right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IBitwiseOperators<long, long, long>.op_OnesComplement(long value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<long, long>.op_LessThan(long left, long right) => left < right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<long, long>.op_LessThanOrEqual(
      long left,
      long right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<long, long>.op_GreaterThan(long left, long right) => left > right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<long, long>.op_GreaterThanOrEqual(
      long left,
      long right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IDecrementOperators<long>.op_Decrement(long value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IDivisionOperators<long, long, long>.op_Division(long left, long right) => left / right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<long, long>.op_Equality(long left, long right) => left == right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<long, long>.op_Inequality(long left, long right) => left != right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IIncrementOperators<long>.op_Increment(long value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IMinMaxValue<long>.MinValue => long.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IMinMaxValue<long>.MaxValue => long.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IModulusOperators<long, long, long>.op_Modulus(long left, long right) => left % right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IMultiplicativeIdentity<long, long>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IMultiplyOperators<long, long, long>.op_Multiply(long left, long right) => left * right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Abs(long value) => Math.Abs(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Clamp(long value, long min, long max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long INumber<long>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (long) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (long) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (long) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((long) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return (long) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (long) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (long) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return checked ((long) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (long) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (long) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return checked ((long) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((long) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long INumber<long>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (long) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (long) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 9223372036854775807M)
          return long.MaxValue;
        return !(num < -9223372036854775808M) ? (long) num : long.MinValue;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) long.MaxValue)
          return long.MaxValue;
        return num >= (double) long.MinValue ? (long) num : long.MinValue;
      }
      if (typeof (TOther) == typeof (short))
        return (long) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (long) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (long) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) long.MaxValue)
          return long.MaxValue;
        return (double) num >= (double) long.MinValue ? (long) num : long.MinValue;
      }
      if (typeof (TOther) == typeof (ushort))
        return (long) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (long) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) long.MaxValue ? (long) num : long.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return (ulong) num <= (ulong) long.MaxValue ? (long) (ulong) num : long.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    long INumber<long>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (long) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (long) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (long) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (long) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (long) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (long) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (long) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (long) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (long) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (long) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (long) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (long) (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (long Quotient, long Remainder) INumber<long>.DivRem(long left, long right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Max(long x, long y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Min(long x, long y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Parse(string s, NumberStyles style, IFormatProvider provider) => long.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return long.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long INumber<long>.Sign(long value) => (long) Math.Sign(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<long>.TryCreate<TOther>(TOther value, out long result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (long) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (long) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < -9223372036854775808M || num > 9223372036854775807M)
        {
          result = 0L;
          return false;
        }
        result = (long) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < (double) long.MinValue || num > (double) long.MaxValue)
        {
          result = 0L;
          return false;
        }
        result = (long) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (long) (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        result = (long) (int) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        result = (long) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        result = (long) (IntPtr) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (long) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < (double) long.MinValue || (double) num > (double) long.MaxValue)
        {
          result = 0L;
          return false;
        }
        result = (long) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (long) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (long) (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) long.MaxValue)
        {
          result = 0L;
          return false;
        }
        result = (long) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if ((ulong) num > (ulong) long.MaxValue)
        {
          result = 0L;
          return false;
        }
        result = (long) (ulong) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = 0L;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<long>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out long result)
    {
      return long.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<long>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out long result)
    {
      return long.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IParseable<long>.Parse(string s, IFormatProvider provider) => long.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<long>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out long result) => long.TryParse(s, NumberStyles.Integer, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IShiftOperators<long, long>.op_LeftShift(long value, int shiftAmount) => value << shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IShiftOperators<long, long>.op_RightShift(long value, int shiftAmount) => value >> shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long ISignedNumber<long>.NegativeOne => -1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long ISpanParseable<long>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => long.Parse(s, NumberStyles.Integer, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<long>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out long result)
    {
      return long.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long ISubtractionOperators<long, long, long>.op_Subtraction(
      long left,
      long right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IUnaryNegationOperators<long, long>.op_UnaryNegation(long value) => -value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    long IUnaryPlusOperators<long, long>.op_UnaryPlus(long value) => value;
  }
}
