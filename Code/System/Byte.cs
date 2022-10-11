// Decompiled with JetBrains decompiler
// Type: System.Byte
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
  /// <summary>Represents an 8-bit unsigned integer.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Byte : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<byte>,
    IEquatable<byte>,
    IBinaryInteger<byte>,
    IBinaryNumber<byte>,
    IBitwiseOperators<byte, byte, byte>,
    INumber<byte>,
    IAdditionOperators<byte, byte, byte>,
    IAdditiveIdentity<byte, byte>,
    IComparisonOperators<byte, byte>,
    IEqualityOperators<byte, byte>,
    IDecrementOperators<byte>,
    IDivisionOperators<byte, byte, byte>,
    IIncrementOperators<byte>,
    IModulusOperators<byte, byte, byte>,
    IMultiplicativeIdentity<byte, byte>,
    IMultiplyOperators<byte, byte, byte>,
    ISpanParseable<byte>,
    IParseable<byte>,
    ISubtractionOperators<byte, byte, byte>,
    IUnaryNegationOperators<byte, byte>,
    IUnaryPlusOperators<byte, byte>,
    IShiftOperators<byte, byte>,
    IMinMaxValue<byte>,
    IUnsignedNumber<byte>
  {
    private readonly byte m_value;
    /// <summary>Represents the largest possible value of a <see cref="T:System.Byte" />. This field is constant.</summary>
    public const byte MaxValue = 255;
    /// <summary>Represents the smallest possible value of a <see cref="T:System.Byte" />. This field is constant.</summary>
    public const byte MinValue = 0;

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.Byte" />.</exception>
    /// <returns>A signed integer that indicates the relative order of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is byte num))
        throw new ArgumentException(SR.Arg_MustBeByte);
      return (int) this - (int) num;
    }

    /// <summary>Compares this instance to a specified 8-bit unsigned integer and returns an indication of their relative values.</summary>
    /// <param name="value">An 8-bit unsigned integer to compare.</param>
    /// <returns>A signed integer that indicates the relative order of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(byte value) => (int) this - (int) value;

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Byte" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is byte num && (int) this == (int) num;

    /// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Byte" /> object represent the same value.</summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(byte obj) => (int) this == (int) obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A hash code for the current <see cref="T:System.Byte" />.</returns>
    public override int GetHashCode() => (int) this;

    /// <summary>Converts the string representation of a number to its <see cref="T:System.Byte" /> equivalent.</summary>
    /// <param name="s">A string that contains a number to convert. The string is interpreted using the <see cref="F:System.Globalization.NumberStyles.Integer" /> style.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not of the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>A byte value that is equivalent to the number contained in <paramref name="s" />.</returns>
    public static byte Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return byte.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its <see cref="T:System.Byte" /> equivalent.</summary>
    /// <param name="s">A string that contains a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not of the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.
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
    /// <returns>A byte value that is equivalent to the number contained in <paramref name="s" />.</returns>
    public static byte Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return byte.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its <see cref="T:System.Byte" /> equivalent.</summary>
    /// <param name="s">A string that contains a number to convert. The string is interpreted using the <see cref="F:System.Globalization.NumberStyles.Integer" /> style.</param>
    /// <param name="provider">An object that supplies culture-specific parsing information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not of the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>A byte value that is equivalent to the number contained in <paramref name="s" />.</returns>
    public static byte Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return byte.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its <see cref="T:System.Byte" /> equivalent.</summary>
    /// <param name="s">A string that contains a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not of the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.
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
    /// <returns>A byte value that is equivalent to the number contained in <paramref name="s" />.</returns>
    public static byte Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return byte.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its <see cref="T:System.Byte" /> equivalent.</summary>
    /// <param name="s">A span containing the characters representing the value to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <returns>A byte value that is equivalent to the number contained in <paramref name="s" />.</returns>
    public static byte Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return byte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
    }


    #nullable disable
    private static byte Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
    {
      uint result;
      Number.ParsingStatus uint32 = Number.TryParseUInt32(s, style, info, out result);
      if (uint32 != Number.ParsingStatus.OK)
        Number.ThrowOverflowOrFormatException(uint32, TypeCode.Byte);
      if (result > (uint) byte.MaxValue)
        Number.ThrowOverflowException(TypeCode.Byte);
      return (byte) result;
    }


    #nullable enable
    /// <summary>Tries to convert the string representation of a number to its <see cref="T:System.Byte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string that contains a number to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.Byte" /> value equivalent to the number contained in <paramref name="s" /> if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out byte result)
    {
      if (s != null)
        return byte.TryParse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
      result = (byte) 0;
      return false;
    }

    /// <summary>Tries to convert the span representation of a number to its <see cref="T:System.Byte" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.Byte" /> value equivalent to the number contained in <paramref name="s" /> if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out byte result) => byte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its <see cref="T:System.Byte" /> equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <param name="result">When this method returns, contains the 8-bit unsigned integer value equivalent to the number contained in <paramref name="s" /> if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
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
      out byte result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return byte.TryParse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result);
      result = (byte) 0;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its <see cref="T:System.Byte" /> equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert. The span is interpreted using the <see cref="F:System.Globalization.NumberStyles.Integer" /> style.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. If <paramref name="provider" /> is <see langword="null" />, the thread current culture is used.</param>
    /// <param name="result">When this method returns, contains the 8-bit unsigned integer value equivalent to the number contained in <paramref name="s" /> if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out byte result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return byte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
    }


    #nullable disable
    private static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      NumberFormatInfo info,
      out byte result)
    {
      uint result1;
      if (Number.TryParseUInt32(s, style, info, out result1) != Number.ParsingStatus.OK || result1 > (uint) byte.MaxValue)
      {
        result = (byte) 0;
        return false;
      }
      result = (byte) result1;
      return true;
    }


    #nullable enable
    /// <summary>Converts the value of the current <see cref="T:System.Byte" /> object to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this object, which consists of a sequence of digits that range from 0 to 9 with no leading zeroes.</returns>
    public override string ToString() => Number.UInt32ToDecStr((uint) this);

    /// <summary>Converts the value of the current <see cref="T:System.Byte" /> object to its equivalent string representation using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> includes an unsupported specifier. Supported format specifiers are listed in the Remarks section.</exception>
    /// <returns>The string representation of the current <see cref="T:System.Byte" /> object, formatted as specified by the <paramref name="format" /> parameter.</returns>
    public string ToString(string? format) => Number.FormatUInt32((uint) this, format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of the current <see cref="T:System.Byte" /> object to its equivalent string representation using the specified culture-specific formatting information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this object in the format specified by the <paramref name="provider" /> parameter.</returns>
    public string ToString(IFormatProvider? provider) => Number.UInt32ToDecStr((uint) this);

    /// <summary>Converts the value of the current <see cref="T:System.Byte" /> object to its equivalent string representation using the specified format and culture-specific formatting information.</summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> includes an unsupported specifier. Supported format specifiers are listed in the Remarks section.</exception>
    /// <returns>The string representation of the current <see cref="T:System.Byte" /> object, formatted as specified by the <paramref name="format" /> and <paramref name="provider" /> parameters.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatUInt32((uint) this, format, provider);

    /// <summary>Tries to format the value of the current 8-bit unsigned integer instance into the provided span of characters.</summary>
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

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Byte" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.Byte" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Byte;


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
    /// <returns>The value of the current instance, unchanged.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => this;

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
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Byte), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.Byte" /> value.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies information about the format of the returned value.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidCastException">The requested type conversion is not supported.</exception>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IAdditionOperators<byte, byte, byte>.op_Addition(byte left, byte right) => (byte) ((uint) left + (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IAdditiveIdentity<byte, byte>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBinaryInteger<byte>.LeadingZeroCount(byte value) => (byte) (BitOperations.LeadingZeroCount((uint) value) - 24);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBinaryInteger<byte>.PopCount(byte value) => (byte) BitOperations.PopCount((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBinaryInteger<byte>.RotateLeft(byte value, int rotateAmount) => (byte) ((int) value << (rotateAmount & 7) | (int) value >> (8 - rotateAmount & 7));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBinaryInteger<byte>.RotateRight(byte value, int rotateAmount) => (byte) ((int) value >> (rotateAmount & 7) | (int) value << (8 - rotateAmount & 7));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBinaryInteger<byte>.TrailingZeroCount(byte value) => (byte) (BitOperations.TrailingZeroCount((int) value << 24) - 24);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<byte>.IsPow2(byte value) => BitOperations.IsPow2((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBinaryNumber<byte>.Log2(byte value) => (byte) BitOperations.Log2((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBitwiseOperators<byte, byte, byte>.op_BitwiseAnd(byte left, byte right) => (byte) ((uint) left & (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBitwiseOperators<byte, byte, byte>.op_BitwiseOr(byte left, byte right) => (byte) ((uint) left | (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBitwiseOperators<byte, byte, byte>.op_ExclusiveOr(byte left, byte right) => (byte) ((uint) left ^ (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IBitwiseOperators<byte, byte, byte>.op_OnesComplement(byte value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<byte, byte>.op_LessThan(byte left, byte right) => (int) left < (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<byte, byte>.op_LessThanOrEqual(
      byte left,
      byte right)
    {
      return (int) left <= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<byte, byte>.op_GreaterThan(byte left, byte right) => (int) left > (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<byte, byte>.op_GreaterThanOrEqual(
      byte left,
      byte right)
    {
      return (int) left >= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IDecrementOperators<byte>.op_Decrement(byte value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IDivisionOperators<byte, byte, byte>.op_Division(byte left, byte right) => (byte) ((uint) left / (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<byte, byte>.op_Equality(byte left, byte right) => (int) left == (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<byte, byte>.op_Inequality(byte left, byte right) => (int) left != (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IIncrementOperators<byte>.op_Increment(byte value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IMinMaxValue<byte>.MinValue => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IMinMaxValue<byte>.MaxValue => byte.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IModulusOperators<byte, byte, byte>.op_Modulus(byte left, byte right) => (byte) ((uint) left % (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IMultiplicativeIdentity<byte, byte>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IMultiplyOperators<byte, byte, byte>.op_Multiply(byte left, byte right) => (byte) ((uint) left * (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Abs(byte value) => value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Clamp(byte value, byte min, byte max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    byte INumber<byte>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return checked ((byte) (char) (object) value);
      if (typeof (TOther) == typeof (Decimal))
        return (byte) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((byte) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return checked ((byte) (short) (object) value);
      if (typeof (TOther) == typeof (int))
        return checked ((byte) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((byte) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((byte) (UIntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return checked ((byte) (sbyte) (object) value);
      if (typeof (TOther) == typeof (float))
        return checked ((byte) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return checked ((byte) (ushort) (object) value);
      if (typeof (TOther) == typeof (uint))
        return checked ((byte) (uint) (object) value);
      if (typeof (TOther) == typeof (ulong))
        return checked ((byte) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((byte) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    byte INumber<byte>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (byte) (object) value;
      if (typeof (TOther) == typeof (char))
      {
        char ch = (char) (object) value;
        return ch <= 'ÿ' ? (byte) ch : byte.MaxValue;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 255M)
          return byte.MaxValue;
        return !(num < 0M) ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) byte.MaxValue)
          return byte.MaxValue;
        return num >= 0.0 ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num > (short) byte.MaxValue)
          return byte.MaxValue;
        return num >= (short) 0 ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num > (int) byte.MaxValue)
          return byte.MaxValue;
        return num >= 0 ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) byte.MaxValue)
          return byte.MaxValue;
        return num >= 0L ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num > new IntPtr(255))
          return byte.MaxValue;
        return num >= IntPtr.Zero ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        return num >= (sbyte) 0 ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) byte.MaxValue)
          return byte.MaxValue;
        return (double) num >= 0.0 ? (byte) num : (byte) 0;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        ushort num = (ushort) (object) value;
        return num <= (ushort) byte.MaxValue ? (byte) num : byte.MaxValue;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        return num <= (uint) byte.MaxValue ? (byte) num : byte.MaxValue;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) byte.MaxValue ? (byte) num : byte.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= new UIntPtr(255) ? (byte) num : byte.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    byte INumber<byte>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (byte) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (byte) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (byte) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (byte) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (byte) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (byte) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (byte) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (byte) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (byte) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (byte) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (byte) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (byte) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (byte) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (byte Quotient, byte Remainder) INumber<byte>.DivRem(byte left, byte right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Max(byte x, byte y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Min(byte x, byte y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Parse(string s, NumberStyles style, IFormatProvider provider) => byte.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return byte.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte INumber<byte>.Sign(byte value) => value == (byte) 0 ? (byte) 0 : (byte) 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<byte>.TryCreate<TOther>(TOther value, out byte result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        char ch = (char) (object) value;
        if (ch > 'ÿ')
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) ch;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < 0M || num > 255M)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < 0.0 || num > (double) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num < (short) 0 || num > (short) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < 0 || num > (int) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < 0L || num > (long) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < IntPtr.Zero || num > new IntPtr(255))
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        if (num < (sbyte) 0)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < 0.0 || (double) num > (double) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        ushort num = (ushort) (object) value;
        if (num > (ushort) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        if (num > (uint) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) byte.MaxValue)
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > new UIntPtr(255))
        {
          result = (byte) 0;
          return false;
        }
        result = (byte) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = (byte) 0;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<byte>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out byte result)
    {
      return byte.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<byte>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out byte result)
    {
      return byte.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IParseable<byte>.Parse(string s, IFormatProvider provider) => byte.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<byte>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out byte result) => byte.TryParse(s, NumberStyles.Integer, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IShiftOperators<byte, byte>.op_LeftShift(byte value, int shiftAmount) => (byte) ((uint) value << shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IShiftOperators<byte, byte>.op_RightShift(byte value, int shiftAmount) => (byte) ((uint) value >> shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte ISpanParseable<byte>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => byte.Parse(s, NumberStyles.Integer, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<byte>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out byte result)
    {
      return byte.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte ISubtractionOperators<byte, byte, byte>.op_Subtraction(
      byte left,
      byte right)
    {
      return (byte) ((uint) left - (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IUnaryNegationOperators<byte, byte>.op_UnaryNegation(byte value) => -value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    byte IUnaryPlusOperators<byte, byte>.op_UnaryPlus(byte value) => value;
  }
}
