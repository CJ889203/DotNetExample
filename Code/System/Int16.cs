// Decompiled with JetBrains decompiler
// Type: System.Int16
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
  /// <summary>Represents a 16-bit signed integer.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Int16 : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<short>,
    IEquatable<short>,
    IBinaryInteger<short>,
    IBinaryNumber<short>,
    IBitwiseOperators<short, short, short>,
    INumber<short>,
    IAdditionOperators<short, short, short>,
    IAdditiveIdentity<short, short>,
    IComparisonOperators<short, short>,
    IEqualityOperators<short, short>,
    IDecrementOperators<short>,
    IDivisionOperators<short, short, short>,
    IIncrementOperators<short>,
    IModulusOperators<short, short, short>,
    IMultiplicativeIdentity<short, short>,
    IMultiplyOperators<short, short, short>,
    ISpanParseable<short>,
    IParseable<short>,
    ISubtractionOperators<short, short, short>,
    IUnaryNegationOperators<short, short>,
    IUnaryPlusOperators<short, short>,
    IShiftOperators<short, short>,
    IMinMaxValue<short>,
    ISignedNumber<short>
  {
    private readonly short m_value;
    /// <summary>Represents the largest possible value of an <see cref="T:System.Int16" />. This field is constant.</summary>
    public const short MaxValue = 32767;
    /// <summary>Represents the smallest possible value of <see cref="T:System.Int16" />. This field is constant.</summary>
    public const short MinValue = -32768;

    /// <summary>Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the object.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not an <see cref="T:System.Int16" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (value is short num)
        return (int) this - (int) num;
      throw new ArgumentException(SR.Arg_MustBeInt16);
    }

    /// <summary>Compares this instance to a specified 16-bit signed integer and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified 16-bit signed integer.</summary>
    /// <param name="value">An integer to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(short value) => (int) this - (int) value;

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Int16" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is short num && (int) this == (int) num;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.Int16" /> value.</summary>
    /// <param name="obj">An <see cref="T:System.Int16" /> value to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(short obj) => (int) this == (int) obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this;

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance, consisting of a minus sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
    public override string ToString() => Number.Int32ToDecStr((int) this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatInt32((int) this, 0, (string) null, provider);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => this.ToString(format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific formatting information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatInt32((int) this, (int) ushort.MaxValue, format, provider);

    /// <summary>Tries to format the value of the current short number instance into the provided span of characters.</summary>
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
      return Number.TryFormatInt32((int) this, (int) ushort.MaxValue, format, provider, destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its 16-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static short Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return short.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its 16-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
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
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero fractional digits.</exception>
    /// <returns>A 16-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static short Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return short.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its 16-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static short Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return short.Parse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 16-bit signed integer equivalent.</summary>
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
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero fractional digits.</exception>
    /// <returns>A 16-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static short Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return short.Parse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 16-bit signed integer equivalent.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <returns>A 16-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static short Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return short.Parse(s, style, NumberFormatInfo.GetInstance(provider));
    }


    #nullable disable
    private static short Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
    {
      int result;
      Number.ParsingStatus int32 = Number.TryParseInt32(s, style, info, out result);
      if (int32 != Number.ParsingStatus.OK)
        Number.ThrowOverflowOrFormatException(int32, TypeCode.Int16);
      if ((uint) (result - (int) short.MinValue - ((int) (style & NumberStyles.AllowHexSpecifier) << 6)) > (uint) ushort.MaxValue)
        Number.ThrowOverflowException(TypeCode.Int16);
      return (short) result;
    }


    #nullable enable
    /// <summary>Converts the string representation of a number to its 16-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the 16-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out short result)
    {
      if (s != null)
        return short.TryParse((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
      result = (short) 0;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 16-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="result">When this method returns, contains the 16-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out short result) => short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 16-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 16-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
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
      out short result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return short.TryParse((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result);
      result = (short) 0;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 16-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert. The span is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 16-bit signed integer value equivalent to the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out short result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return short.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
    }


    #nullable disable
    private static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      NumberFormatInfo info,
      out short result)
    {
      int result1;
      if (Number.TryParseInt32(s, style, info, out result1) != Number.ParsingStatus.OK || (uint) (result1 - (int) short.MinValue - ((int) (style & NumberStyles.AllowHexSpecifier) << 6)) > (uint) ushort.MaxValue)
      {
        result = (short) 0;
        return false;
      }
      result = (short) result1;
      return true;
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Int16" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.Int16" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Int16;

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
    /// <returns>The value of the current instance, unchanged.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => this;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, unchanged.</returns>
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
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Int16), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.Int16" /> value.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IAdditionOperators<short, short, short>.op_Addition(
      short left,
      short right)
    {
      return (short) ((int) left + (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IAdditiveIdentity<short, short>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBinaryInteger<short>.LeadingZeroCount(short value) => (short) (BitOperations.LeadingZeroCount((uint) (ushort) value) - 16);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBinaryInteger<short>.PopCount(short value) => (short) BitOperations.PopCount((uint) (ushort) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBinaryInteger<short>.RotateLeft(short value, int rotateAmount) => (short) ((int) value << (rotateAmount & 15) | (int) (ushort) value >> (16 - rotateAmount & 15));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBinaryInteger<short>.RotateRight(short value, int rotateAmount) => (short) ((int) (ushort) value >> (rotateAmount & 15) | (int) value << (16 - rotateAmount & 15));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBinaryInteger<short>.TrailingZeroCount(short value) => (short) (byte) (BitOperations.TrailingZeroCount((int) value << 16) - 16);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<short>.IsPow2(short value) => BitOperations.IsPow2((int) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBinaryNumber<short>.Log2(short value)
    {
      if (value < (short) 0)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      return (short) BitOperations.Log2((uint) (ushort) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBitwiseOperators<short, short, short>.op_BitwiseAnd(
      short left,
      short right)
    {
      return (short) ((int) left & (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBitwiseOperators<short, short, short>.op_BitwiseOr(
      short left,
      short right)
    {
      return (short) ((int) left | (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBitwiseOperators<short, short, short>.op_ExclusiveOr(
      short left,
      short right)
    {
      return (short) ((int) left ^ (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IBitwiseOperators<short, short, short>.op_OnesComplement(short value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<short, short>.op_LessThan(short left, short right) => (int) left < (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<short, short>.op_LessThanOrEqual(
      short left,
      short right)
    {
      return (int) left <= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<short, short>.op_GreaterThan(
      short left,
      short right)
    {
      return (int) left > (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<short, short>.op_GreaterThanOrEqual(
      short left,
      short right)
    {
      return (int) left >= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IDecrementOperators<short>.op_Decrement(short value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IDivisionOperators<short, short, short>.op_Division(
      short left,
      short right)
    {
      return (short) ((int) left / (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<short, short>.op_Equality(short left, short right) => (int) left == (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<short, short>.op_Inequality(short left, short right) => (int) left != (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IIncrementOperators<short>.op_Increment(short value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IMinMaxValue<short>.MinValue => short.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IMinMaxValue<short>.MaxValue => short.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IModulusOperators<short, short, short>.op_Modulus(
      short left,
      short right)
    {
      return (short) ((int) left % (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IMultiplicativeIdentity<short, short>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IMultiplyOperators<short, short, short>.op_Multiply(
      short left,
      short right)
    {
      return (short) ((int) left * (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Abs(short value) => Math.Abs(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Clamp(short value, short min, short max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    short INumber<short>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (short) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return checked ((short) (char) (object) value);
      if (typeof (TOther) == typeof (Decimal))
        return (short) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((short) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return checked ((short) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((short) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((short) (IntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return (short) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return checked ((short) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return checked ((short) (ushort) (object) value);
      if (typeof (TOther) == typeof (uint))
        return checked ((short) (uint) (object) value);
      if (typeof (TOther) == typeof (ulong))
        return checked ((short) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((short) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    short INumber<short>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (short) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
      {
        char ch = (char) (object) value;
        return ch <= '翿' ? (short) ch : short.MaxValue;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 32767M)
          return short.MaxValue;
        return !(num < -32768M) ? (short) num : short.MinValue;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) short.MaxValue)
          return short.MaxValue;
        return num >= (double) short.MinValue ? (short) num : short.MinValue;
      }
      if (typeof (TOther) == typeof (short))
        return (short) (object) value;
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num > (int) short.MaxValue)
          return short.MaxValue;
        return num >= (int) short.MinValue ? (short) num : short.MinValue;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) short.MaxValue)
          return short.MaxValue;
        return num >= (long) short.MinValue ? (short) num : short.MinValue;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num > new IntPtr(32767))
          return short.MaxValue;
        return num >= new IntPtr(-32768) ? (short) num : short.MinValue;
      }
      if (typeof (TOther) == typeof (sbyte))
        return (short) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) short.MaxValue)
          return short.MaxValue;
        return (double) num >= (double) short.MinValue ? (short) num : short.MinValue;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        ushort num = (ushort) (object) value;
        return num <= (ushort) short.MaxValue ? (short) num : short.MaxValue;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        return num <= (uint) short.MaxValue ? (short) num : short.MaxValue;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) short.MaxValue ? (short) num : short.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= new UIntPtr(32767) ? (short) num : short.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    short INumber<short>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (short) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (short) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (short) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (short) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (short) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (short) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (short) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (short) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (short) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (short) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (short) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (short) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (short) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (short Quotient, short Remainder) INumber<short>.DivRem(short left, short right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Max(short x, short y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Min(short x, short y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Parse(string s, NumberStyles style, IFormatProvider provider) => short.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return short.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short INumber<short>.Sign(short value) => (short) Math.Sign(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<short>.TryCreate<TOther>(TOther value, out short result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (short) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        char ch = (char) (object) value;
        if (ch > '翿')
        {
          result = (short) 0;
          return false;
        }
        result = (short) ch;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < -32768M || num > 32767M)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < (double) short.MinValue || num > (double) short.MaxValue)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < (int) short.MinValue || num > (int) short.MaxValue)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < (long) short.MinValue || num > (long) short.MaxValue)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < new IntPtr(-32768) || num > new IntPtr(32767))
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (short) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < (double) short.MinValue || (double) num > (double) short.MaxValue)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        ushort num = (ushort) (object) value;
        if (num > (ushort) short.MaxValue)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        if (num > (uint) short.MaxValue)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) short.MaxValue)
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > new UIntPtr(32767))
        {
          result = (short) 0;
          return false;
        }
        result = (short) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = (short) 0;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<short>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out short result)
    {
      return short.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<short>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out short result)
    {
      return short.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IParseable<short>.Parse(string s, IFormatProvider provider) => short.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<short>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out short result) => short.TryParse(s, NumberStyles.Integer, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IShiftOperators<short, short>.op_LeftShift(short value, int shiftAmount) => (short) ((int) value << shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IShiftOperators<short, short>.op_RightShift(short value, int shiftAmount) => (short) ((int) value >> shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short ISignedNumber<short>.NegativeOne => -1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short ISpanParseable<short>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => short.Parse(s, NumberStyles.Integer, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<short>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out short result)
    {
      return short.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short ISubtractionOperators<short, short, short>.op_Subtraction(
      short left,
      short right)
    {
      return (short) ((int) left - (int) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IUnaryNegationOperators<short, short>.op_UnaryNegation(short value) => -value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    short IUnaryPlusOperators<short, short>.op_UnaryPlus(short value) => value;
  }
}
