// Decompiled with JetBrains decompiler
// Type: System.UInt16
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
  /// <summary>Represents a 16-bit unsigned integer.</summary>
  [CLSCompliant(false)]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct UInt16 : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<ushort>,
    IEquatable<ushort>,
    IBinaryInteger<ushort>,
    IBinaryNumber<ushort>,
    IBitwiseOperators<ushort, ushort, ushort>,
    INumber<ushort>,
    IAdditionOperators<ushort, ushort, ushort>,
    IAdditiveIdentity<ushort, ushort>,
    IComparisonOperators<ushort, ushort>,
    IEqualityOperators<ushort, ushort>,
    IDecrementOperators<ushort>,
    IDivisionOperators<ushort, ushort, ushort>,
    IIncrementOperators<ushort>,
    IModulusOperators<ushort, ushort, ushort>,
    IMultiplicativeIdentity<ushort, ushort>,
    IMultiplyOperators<ushort, ushort, ushort>,
    ISpanParseable<ushort>,
    IParseable<ushort>,
    ISubtractionOperators<ushort, ushort, ushort>,
    IUnaryNegationOperators<ushort, ushort>,
    IUnaryPlusOperators<ushort, ushort>,
    IShiftOperators<ushort, ushort>,
    IMinMaxValue<ushort>,
    IUnsignedNumber<ushort>
  {
    private readonly ushort m_value;
    /// <summary>Represents the largest possible value of <see cref="T:System.UInt16" />. This field is constant.</summary>
    public const ushort MaxValue = 65535;
    /// <summary>Represents the smallest possible value of <see cref="T:System.UInt16" />. This field is constant.</summary>
    public const ushort MinValue = 0;

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.UInt16" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (value is ushort num)
        return (int) this - (int) num;
      throw new ArgumentException(SR.Arg_MustBeUInt16);
    }

    /// <summary>Compares this instance to a specified 16-bit unsigned integer and returns an indication of their relative values.</summary>
    /// <param name="value">An unsigned integer to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(ushort value) => (int) this - (int) value;

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.UInt16" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ushort num && (int) this == (int) num;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.UInt16" /> value.</summary>
    /// <param name="obj">A 16-bit unsigned integer to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(ushort obj) => (int) this == (int) obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this;

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance, which consists of a sequence of digits ranging from 0 to 9, without a sign or leading zeros.</returns>
    public override string ToString() => Number.UInt32ToDecStr((uint) this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance, which consists of a sequence of digits ranging from 0 to 9, without a sign or leading zeros.</returns>
    public string ToString(IFormatProvider? provider) => Number.UInt32ToDecStr((uint) this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatUInt32((uint) this, format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance, as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatUInt32((uint) this, format, provider);

    /// <summary>Tries to format the value of the current unsigned short number instance into the provided span of characters.</summary>
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
      return Number.TryFormatUInt32((uint) this, format, provider, destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its 16-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static ushort Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return ushort.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its 16-bit unsigned integer equivalent.
    /// 
    /// This method is not CLS-compliant. The CLS-compliant alternative is <see cref="M:System.Int32.Parse(System.String,System.Globalization.NumberStyles)" />.</summary>
    /// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
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
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 16-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ushort Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return ushort.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its 16-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ushort Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return ushort.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 16-bit unsigned integer equivalent.</summary>
    /// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
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
    ///        <paramref name="s" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 16-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ushort Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return ushort.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 16-bit unsigned integer equivalent.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert. The span is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <returns>A 16-bit unsigned integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static ushort Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return ushort.Parse(s, style, NumberFormatInfo.GetInstance(provider));
    }


    #nullable disable
    private static ushort Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
    {
      uint result;
      Number.ParsingStatus uint32 = Number.TryParseUInt32(s, style, info, out result);
      if (uint32 != Number.ParsingStatus.OK)
        Number.ThrowOverflowOrFormatException(uint32, TypeCode.UInt16);
      if (result > (uint) ushort.MaxValue)
        Number.ThrowOverflowException(TypeCode.UInt16);
      return (ushort) result;
    }


    #nullable enable
    /// <summary>Tries to convert the string representation of a number to its 16-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that represents the number to convert.</param>
    /// <param name="result">When this method returns, contains the 16-bit unsigned integer value that is equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in the correct format. , or represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out ushort result)
    {
      if (s != null)
        return ushort.TryParse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
      result = (ushort) 0;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number to its 16-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="result">When this method returns, contains the 16-bit unsigned integer value that is equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in the correct format. , or represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out ushort result) => ushort.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);

    /// <summary>Tries to convert the string representation of a number in a specified style and culture-specific format to its 16-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that represents the number to convert. The string is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 16-bit unsigned integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
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
      out ushort result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return ushort.TryParse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result);
      result = (ushort) 0;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number in a specified style and culture-specific format to its 16-bit unsigned integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert. The span is interpreted by using the style specified by the <paramref name="style" /> parameter.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 16-bit unsigned integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out ushort result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return ushort.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
    }


    #nullable disable
    private static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      NumberFormatInfo info,
      out ushort result)
    {
      uint result1;
      if (Number.TryParseUInt32(s, style, info, out result1) != Number.ParsingStatus.OK || result1 > (uint) ushort.MaxValue)
      {
        result = (ushort) 0;
        return false;
      }
      result = (ushort) result1;
      return true;
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.UInt16" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.UInt16" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.UInt16;

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
    /// <returns>The current value of this instance, converted to an <see cref="T:System.SByte" />.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Byte" />.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value of this instance, converted to an <see cref="T:System.Int16" />.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value of this instance, unchanged.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => this;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of this instance, converted to an <see cref="T:System.Int32" />.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value of this instance, converted to a <see cref="T:System.UInt32" />.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value of this instance, converted to an <see cref="T:System.Int64" />.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value of this instance, converted to a <see cref="T:System.UInt64" />.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value pf this instance, converted to a <see cref="T:System.Single" />.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value of this instance, converted to a <see cref="T:System.Double" />.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The current value of this instance, converted to a <see cref="T:System.Decimal" />.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(this);

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (UInt16), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.UInt16" /> value.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies information about the format of the returned value.</param>
    /// <returns>The current value of this instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IAdditionOperators<ushort, ushort, ushort>.op_Addition(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left + (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IAdditiveIdentity<ushort, ushort>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBinaryInteger<ushort>.LeadingZeroCount(ushort value) => (ushort) (BitOperations.LeadingZeroCount((uint) value) - 16);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBinaryInteger<ushort>.PopCount(ushort value) => (ushort) BitOperations.PopCount((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBinaryInteger<ushort>.RotateLeft(ushort value, int rotateAmount) => (ushort) ((int) value << (rotateAmount & 15) | (int) value >> (16 - rotateAmount & 15));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBinaryInteger<ushort>.RotateRight(ushort value, int rotateAmount) => (ushort) ((int) value >> (rotateAmount & 15) | (int) value << (16 - rotateAmount & 15));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBinaryInteger<ushort>.TrailingZeroCount(ushort value) => (ushort) (BitOperations.TrailingZeroCount((int) value << 16) - 16);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<ushort>.IsPow2(ushort value) => BitOperations.IsPow2((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBinaryNumber<ushort>.Log2(ushort value) => (ushort) BitOperations.Log2((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBitwiseOperators<ushort, ushort, ushort>.op_BitwiseAnd(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left & (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBitwiseOperators<ushort, ushort, ushort>.op_BitwiseOr(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left | (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBitwiseOperators<ushort, ushort, ushort>.op_ExclusiveOr(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left ^ (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IBitwiseOperators<ushort, ushort, ushort>.op_OnesComplement(ushort value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ushort, ushort>.op_LessThan(
      ushort left,
      ushort right)
    {
      return (int) left < (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ushort, ushort>.op_LessThanOrEqual(
      ushort left,
      ushort right)
    {
      return (int) left <= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ushort, ushort>.op_GreaterThan(
      ushort left,
      ushort right)
    {
      return (int) left > (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<ushort, ushort>.op_GreaterThanOrEqual(
      ushort left,
      ushort right)
    {
      return (int) left >= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IDecrementOperators<ushort>.op_Decrement(ushort value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IDivisionOperators<ushort, ushort, ushort>.op_Division(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left / (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<ushort, ushort>.op_Equality(ushort left, ushort right) => (int) left == (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<ushort, ushort>.op_Inequality(
      ushort left,
      ushort right)
    {
      return (int) left != (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IIncrementOperators<ushort>.op_Increment(ushort value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IMinMaxValue<ushort>.MinValue => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IMinMaxValue<ushort>.MaxValue => ushort.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IModulusOperators<ushort, ushort, ushort>.op_Modulus(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left % (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IMultiplicativeIdentity<ushort, ushort>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IMultiplyOperators<ushort, ushort, ushort>.op_Multiply(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left * (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Abs(ushort value) => value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Clamp(ushort value, ushort min, ushort max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ushort INumber<ushort>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (ushort) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (ushort) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (ushort) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((ushort) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return checked ((ushort) (short) (object) value);
      if (typeof (TOther) == typeof (int))
        return checked ((ushort) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((ushort) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((ushort) (UIntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return checked ((ushort) (sbyte) (object) value);
      if (typeof (TOther) == typeof (float))
        return checked ((ushort) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return checked ((ushort) (uint) (object) value);
      if (typeof (TOther) == typeof (ulong))
        return checked ((ushort) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((ushort) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ushort INumber<ushort>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (ushort) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (ushort) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 65535M)
          return ushort.MaxValue;
        return !(num < 0M) ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) ushort.MaxValue)
          return ushort.MaxValue;
        return num >= 0.0 ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        return num >= (short) 0 ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num > (int) ushort.MaxValue)
          return ushort.MaxValue;
        return num >= 0 ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) ushort.MaxValue)
          return ushort.MaxValue;
        return num >= 0L ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num > new IntPtr(65535))
          return ushort.MaxValue;
        return num >= IntPtr.Zero ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        return num >= (sbyte) 0 ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) ushort.MaxValue)
          return ushort.MaxValue;
        return (double) num >= 0.0 ? (ushort) num : (ushort) 0;
      }
      if (typeof (TOther) == typeof (ushort))
        return (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        return num <= (uint) ushort.MaxValue ? (ushort) num : ushort.MaxValue;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) ushort.MaxValue ? (ushort) num : ushort.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= new UIntPtr(65535) ? (ushort) num : ushort.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ushort INumber<ushort>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (ushort) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (ushort) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (ushort) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (ushort) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (ushort) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (ushort) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (ushort) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (ushort) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (ushort) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (ushort) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (ushort) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (ushort) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (ushort) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (ushort Quotient, ushort Remainder) INumber<ushort>.DivRem(ushort left, ushort right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Max(ushort x, ushort y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Min(ushort x, ushort y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Parse(string s, NumberStyles style, IFormatProvider provider) => ushort.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return ushort.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort INumber<ushort>.Sign(ushort value) => value == (ushort) 0 ? (ushort) 0 : (ushort) 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<ushort>.TryCreate<TOther>(TOther value, out ushort result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (ushort) (short) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (ushort) (short) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < 0M || num > 65535M)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < 0.0 || num > (double) ushort.MaxValue)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num < (short) 0)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < 0 || num > (int) ushort.MaxValue)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < 0L || num > (long) ushort.MaxValue)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < IntPtr.Zero || num > new IntPtr(65535))
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        if (num < (sbyte) 0)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < 0.0 || (double) num > (double) ushort.MaxValue)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        if (num > (uint) ushort.MaxValue)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) ushort.MaxValue)
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > new UIntPtr(65535))
        {
          result = (ushort) 0;
          return false;
        }
        result = (ushort) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = (ushort) 0;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<ushort>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out ushort result)
    {
      return ushort.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<ushort>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out ushort result)
    {
      return ushort.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IParseable<ushort>.Parse(string s, IFormatProvider provider) => ushort.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<ushort>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out ushort result)
    {
      return ushort.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IShiftOperators<ushort, ushort>.op_LeftShift(
      ushort value,
      int shiftAmount)
    {
      return (ushort) ((uint) value << shiftAmount);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IShiftOperators<ushort, ushort>.op_RightShift(
      ushort value,
      int shiftAmount)
    {
      return (ushort) ((uint) value >> shiftAmount);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort ISpanParseable<ushort>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return ushort.Parse(s, NumberStyles.Integer, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<ushort>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out ushort result)
    {
      return ushort.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort ISubtractionOperators<ushort, ushort, ushort>.op_Subtraction(
      ushort left,
      ushort right)
    {
      return (ushort) ((uint) left - (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IUnaryNegationOperators<ushort, ushort>.op_UnaryNegation(
      ushort value)
    {
      return -value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    ushort IUnaryPlusOperators<ushort, ushort>.op_UnaryPlus(ushort value) => value;
  }
}
