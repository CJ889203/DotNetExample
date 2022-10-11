// Decompiled with JetBrains decompiler
// Type: System.SByte
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
  /// <summary>Represents an 8-bit signed integer.</summary>
  [CLSCompliant(false)]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct SByte : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<sbyte>,
    IEquatable<sbyte>,
    IBinaryInteger<sbyte>,
    IBinaryNumber<sbyte>,
    IBitwiseOperators<sbyte, sbyte, sbyte>,
    INumber<sbyte>,
    IAdditionOperators<sbyte, sbyte, sbyte>,
    IAdditiveIdentity<sbyte, sbyte>,
    IComparisonOperators<sbyte, sbyte>,
    IEqualityOperators<sbyte, sbyte>,
    IDecrementOperators<sbyte>,
    IDivisionOperators<sbyte, sbyte, sbyte>,
    IIncrementOperators<sbyte>,
    IModulusOperators<sbyte, sbyte, sbyte>,
    IMultiplicativeIdentity<sbyte, sbyte>,
    IMultiplyOperators<sbyte, sbyte, sbyte>,
    ISpanParseable<sbyte>,
    IParseable<sbyte>,
    ISubtractionOperators<sbyte, sbyte, sbyte>,
    IUnaryNegationOperators<sbyte, sbyte>,
    IUnaryPlusOperators<sbyte, sbyte>,
    IShiftOperators<sbyte, sbyte>,
    IMinMaxValue<sbyte>,
    ISignedNumber<sbyte>
  {
    private readonly sbyte m_value;
    /// <summary>Represents the largest possible value of <see cref="T:System.SByte" />. This field is constant.</summary>
    public const sbyte MaxValue = 127;
    /// <summary>Represents the smallest possible value of <see cref="T:System.SByte" />. This field is constant.</summary>
    public const sbyte MinValue = -128;

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="obj">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="obj" /> is not an <see cref="T:System.SByte" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="obj" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="obj" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="obj" />, or <paramref name="obj" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? obj)
    {
      if (obj == null)
        return 1;
      if (!(obj is sbyte num))
        throw new ArgumentException(SR.Arg_MustBeSByte);
      return (int) this - (int) num;
    }

    /// <summary>Compares this instance to a specified 8-bit signed integer and returns an indication of their relative values.</summary>
    /// <param name="value">An 8-bit signed integer to compare.</param>
    /// <returns>A signed integer that indicates the relative order of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(sbyte value) => (int) this - (int) value;

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.SByte" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is sbyte num && (int) this == (int) num;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.SByte" /> value.</summary>
    /// <param name="obj">An <see cref="T:System.SByte" /> value to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(sbyte obj) => (int) this == (int) obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this;

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance, consisting of a negative sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
    public override string ToString() => Number.Int32ToDecStr((int) this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => this.ToString(format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance, as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatInt32((int) this, 0, (string) null, provider);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatInt32((int) this, (int) byte.MaxValue, format, provider);

    /// <summary>Tries to format the value of the current 8-bit signed integer instance into the provided span of characters.</summary>
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
      return Number.TryFormatInt32((int) this, (int) byte.MaxValue, format, provider, destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its 8-bit signed integer equivalent.</summary>
    /// <param name="s">A string that represents a number to convert. The string is interpreted using the <see cref="F:System.Globalization.NumberStyles.Integer" /> style.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not consist of an optional sign followed by a sequence of digits (zero through nine).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to the number contained in the <paramref name="s" /> parameter.</returns>
    public static sbyte Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return sbyte.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its 8-bit signed integer equivalent.</summary>
    /// <param name="s">A string that contains a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in a format that is compliant with <paramref name="style" />.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" /> values.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to the number specified in <paramref name="s" />.</returns>
    public static sbyte Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return sbyte.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its 8-bit signed integer equivalent.</summary>
    /// <param name="s">A string that represents a number to convert. The string is interpreted using the <see cref="F:System.Globalization.NumberStyles.Integer" /> style.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to the number specified in <paramref name="s" />.</returns>
    public static sbyte Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return sbyte.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number that is in a specified style and culture-specific format to its 8-bit signed equivalent.</summary>
    /// <param name="s">A string that contains the number to convert. The string is interpreted by using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is not a combination of <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> and <see cref="F:System.Globalization.NumberStyles.HexNumber" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in a format that is compliant with <paramref name="style" />.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="s" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>An 8-bit signed byte value that is equivalent to the number specified in the <paramref name="s" /> parameter.</returns>
    public static sbyte Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return sbyte.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number that is in a specified style and culture-specific format to its 8-bit signed equivalent.</summary>
    /// <param name="s">A span containing the characters representing the number to convert. The span is interpreted by using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <returns>An 8-bit signed byte value that is equivalent to the number specified in the <paramref name="s" /> parameter.</returns>
    public static sbyte Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return sbyte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
    }


    #nullable disable
    private static sbyte Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
    {
      int result;
      Number.ParsingStatus int32 = Number.TryParseInt32(s, style, info, out result);
      if (int32 != Number.ParsingStatus.OK)
        Number.ThrowOverflowOrFormatException(int32, TypeCode.SByte);
      if ((uint) (result - (int) sbyte.MinValue - ((int) (style & NumberStyles.AllowHexSpecifier) >> 2)) > (uint) byte.MaxValue)
        Number.ThrowOverflowException(TypeCode.SByte);
      return (sbyte) result;
    }


    #nullable enable
    /// <summary>Tries to convert the string representation of a number to its <see cref="T:System.SByte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="result">When this method returns, contains the 8-bit signed integer value that is equivalent to the number contained in <paramref name="s" /> if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in the correct format, or represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out sbyte result)
    {
      if (s != null)
        return sbyte.TryParse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
      result = (sbyte) 0;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number to its <see cref="T:System.SByte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="result">When this method returns, contains the 8-bit signed integer value that is equivalent to the number contained in <paramref name="s" /> if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in the correct format, or represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out sbyte result) => sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);

    /// <summary>Tries to convert the string representation of a number in a specified style and culture-specific format to its <see cref="T:System.SByte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string representing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 8-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
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
      out sbyte result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return sbyte.TryParse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result);
      result = (sbyte) 0;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number in a specified style and culture-specific format to its <see cref="T:System.SByte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 8-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out sbyte result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return sbyte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
    }


    #nullable disable
    private static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      NumberFormatInfo info,
      out sbyte result)
    {
      int result1;
      if (Number.TryParseInt32(s, style, info, out result1) != Number.ParsingStatus.OK || (uint) (result1 - (int) sbyte.MinValue - ((int) (style & NumberStyles.AllowHexSpecifier) >> 2)) > (uint) byte.MaxValue)
      {
        result = (sbyte) 0;
        return false;
      }
      result = (sbyte) result1;
      return true;
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.SByte" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.SByte" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.SByte;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is unused.</param>
    /// <returns>
    /// <see langword="true" /> if the value of the current instance is not zero; otherwise, <see langword="false" />.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToChar(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Char" />.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, unchanged.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => this;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is unused.</param>
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
    int IConvertible.ToInt32(IFormatProvider provider) => (int) this;

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
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Double" />.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is unused.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Decimal" />.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(this);

    /// <summary>This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>None. This conversion is not supported.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (SByte), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The <see cref="T:System.Type" /> to which to convert this <see cref="T:System.SByte" /> value.</param>
    /// <param name="provider">A <see cref="T:System.IFormatProvider" /> implementation that provides information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to an object of type <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IAdditionOperators<sbyte, sbyte, sbyte>.op_Addition(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left + (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IAdditiveIdentity<sbyte, sbyte>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBinaryInteger<sbyte>.LeadingZeroCount(sbyte value) => (sbyte) (BitOperations.LeadingZeroCount((uint) (byte) value) - 24);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBinaryInteger<sbyte>.PopCount(sbyte value) => (sbyte) BitOperations.PopCount((uint) (byte) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBinaryInteger<sbyte>.RotateLeft(sbyte value, int rotateAmount) => (sbyte) ((int) value << (rotateAmount & 7) | (int) (byte) value >> (8 - rotateAmount & 7));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBinaryInteger<sbyte>.RotateRight(sbyte value, int rotateAmount) => (sbyte) ((int) (byte) value >> (rotateAmount & 7) | (int) value << (8 - rotateAmount & 7));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBinaryInteger<sbyte>.TrailingZeroCount(sbyte value) => (sbyte) (BitOperations.TrailingZeroCount((int) value << 24) - 24);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<sbyte>.IsPow2(sbyte value) => BitOperations.IsPow2((int) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBinaryNumber<sbyte>.Log2(sbyte value)
    {
      if (value < (sbyte) 0)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      return (sbyte) BitOperations.Log2((uint) (byte) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.op_BitwiseAnd(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left & (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.op_BitwiseOr(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left | (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.op_ExclusiveOr(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left ^ (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IBitwiseOperators<sbyte, sbyte, sbyte>.op_OnesComplement(sbyte value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<sbyte, sbyte>.op_LessThan(sbyte left, sbyte right) => (int) left < (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<sbyte, sbyte>.op_LessThanOrEqual(
      sbyte left,
      sbyte right)
    {
      return (int) left <= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<sbyte, sbyte>.op_GreaterThan(
      sbyte left,
      sbyte right)
    {
      return (int) left > (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<sbyte, sbyte>.op_GreaterThanOrEqual(
      sbyte left,
      sbyte right)
    {
      return (int) left >= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IDecrementOperators<sbyte>.op_Decrement(sbyte value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IDivisionOperators<sbyte, sbyte, sbyte>.op_Division(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left / (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<sbyte, sbyte>.op_Equality(sbyte left, sbyte right) => (int) left == (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<sbyte, sbyte>.op_Inequality(sbyte left, sbyte right) => (int) left != (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IIncrementOperators<sbyte>.op_Increment(sbyte value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IMinMaxValue<sbyte>.MinValue => sbyte.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IMinMaxValue<sbyte>.MaxValue => sbyte.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IModulusOperators<sbyte, sbyte, sbyte>.op_Modulus(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left % (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IMultiplicativeIdentity<sbyte, sbyte>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IMultiplyOperators<sbyte, sbyte, sbyte>.op_Multiply(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left * (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Abs(sbyte value) => Math.Abs(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Clamp(sbyte value, sbyte min, sbyte max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    sbyte INumber<sbyte>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return checked ((sbyte) (byte) (object) value);
      if (typeof (TOther) == typeof (char))
        return checked ((sbyte) (char) (object) value);
      if (typeof (TOther) == typeof (Decimal))
        return (sbyte) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((sbyte) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return checked ((sbyte) (short) (object) value);
      if (typeof (TOther) == typeof (int))
        return checked ((sbyte) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((sbyte) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((sbyte) (IntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return checked ((sbyte) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return checked ((sbyte) (ushort) (object) value);
      if (typeof (TOther) == typeof (uint))
        return checked ((sbyte) (uint) (object) value);
      if (typeof (TOther) == typeof (ulong))
        return checked ((sbyte) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((sbyte) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    sbyte INumber<sbyte>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
      {
        byte num = (byte) (object) value;
        return num <= (byte) 127 ? (sbyte) num : sbyte.MaxValue;
      }
      if (typeof (TOther) == typeof (char))
      {
        char ch = (char) (object) value;
        return ch <= '\u007F' ? (sbyte) ch : sbyte.MaxValue;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 127M)
          return sbyte.MaxValue;
        return !(num < -128M) ? (sbyte) num : sbyte.MinValue;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) sbyte.MaxValue)
          return sbyte.MaxValue;
        return num >= (double) sbyte.MinValue ? (sbyte) num : sbyte.MinValue;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num > (short) sbyte.MaxValue)
          return sbyte.MaxValue;
        return num >= (short) sbyte.MinValue ? (sbyte) num : sbyte.MinValue;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num > (int) sbyte.MaxValue)
          return sbyte.MaxValue;
        return num >= (int) sbyte.MinValue ? (sbyte) num : sbyte.MinValue;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) sbyte.MaxValue)
          return sbyte.MaxValue;
        return num >= (long) sbyte.MinValue ? (sbyte) num : sbyte.MinValue;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num > new IntPtr(127))
          return sbyte.MaxValue;
        return num >= new IntPtr(-128) ? (sbyte) num : sbyte.MinValue;
      }
      if (typeof (TOther) == typeof (sbyte))
        return (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) sbyte.MaxValue)
          return sbyte.MaxValue;
        return (double) num >= (double) sbyte.MinValue ? (sbyte) num : sbyte.MinValue;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        ushort num = (ushort) (object) value;
        return num <= (ushort) sbyte.MaxValue ? (sbyte) num : sbyte.MaxValue;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        return num <= (uint) sbyte.MaxValue ? (sbyte) num : sbyte.MaxValue;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) sbyte.MaxValue ? (sbyte) num : sbyte.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= new UIntPtr(127) ? (sbyte) num : sbyte.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    sbyte INumber<sbyte>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (sbyte) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (sbyte) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (sbyte) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (sbyte) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (sbyte) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (sbyte) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (sbyte) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (sbyte) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (sbyte) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (sbyte) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (sbyte) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (sbyte) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (sbyte) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (sbyte Quotient, sbyte Remainder) INumber<sbyte>.DivRem(sbyte left, sbyte right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Max(sbyte x, sbyte y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Min(sbyte x, sbyte y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Parse(string s, NumberStyles style, IFormatProvider provider) => sbyte.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return sbyte.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte INumber<sbyte>.Sign(sbyte value) => (sbyte) Math.Sign(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<sbyte>.TryCreate<TOther>(TOther value, out sbyte result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        byte num = (byte) (object) value;
        if (num > (byte) 127)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        char ch = (char) (object) value;
        if (ch > '\u007F')
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) ch;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < -128M || num > 127M)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < (double) sbyte.MinValue || num > (double) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num < (short) sbyte.MinValue || num > (short) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < (int) sbyte.MinValue || num > (int) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < (long) sbyte.MinValue || num > (long) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < new IntPtr(-128) || num > new IntPtr(127))
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < (double) sbyte.MinValue || (double) num > (double) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        ushort num = (ushort) (object) value;
        if (num > (ushort) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        if (num > (uint) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) sbyte.MaxValue)
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > new UIntPtr(127))
        {
          result = (sbyte) 0;
          return false;
        }
        result = (sbyte) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = (sbyte) 0;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<sbyte>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out sbyte result)
    {
      return sbyte.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<sbyte>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out sbyte result)
    {
      return sbyte.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IParseable<sbyte>.Parse(string s, IFormatProvider provider) => sbyte.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<sbyte>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out sbyte result) => sbyte.TryParse(s, NumberStyles.Integer, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IShiftOperators<sbyte, sbyte>.op_LeftShift(sbyte value, int shiftAmount) => (sbyte) ((int) value << shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IShiftOperators<sbyte, sbyte>.op_RightShift(sbyte value, int shiftAmount) => (sbyte) ((int) value >> shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte ISignedNumber<sbyte>.NegativeOne => -1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte ISpanParseable<sbyte>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => sbyte.Parse(s, NumberStyles.Integer, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<sbyte>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out sbyte result)
    {
      return sbyte.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte ISubtractionOperators<sbyte, sbyte, sbyte>.op_Subtraction(
      sbyte left,
      sbyte right)
    {
      return (sbyte) ((int) left - (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IUnaryNegationOperators<sbyte, sbyte>.op_UnaryNegation(sbyte value) => -value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    sbyte IUnaryPlusOperators<sbyte, sbyte>.op_UnaryPlus(sbyte value) => value;
  }
}
