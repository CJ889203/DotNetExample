// Decompiled with JetBrains decompiler
// Type: System.Int32
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
  /// <summary>Represents a 32-bit signed integer.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Int32 : 
    IComparable,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IComparable<int>,
    IEquatable<int>,
    IBinaryInteger<int>,
    IBinaryNumber<int>,
    IBitwiseOperators<int, int, int>,
    INumber<int>,
    IAdditionOperators<int, int, int>,
    IAdditiveIdentity<int, int>,
    IComparisonOperators<int, int>,
    IEqualityOperators<int, int>,
    IDecrementOperators<int>,
    IDivisionOperators<int, int, int>,
    IIncrementOperators<int>,
    IModulusOperators<int, int, int>,
    IMultiplicativeIdentity<int, int>,
    IMultiplyOperators<int, int, int>,
    ISpanParseable<int>,
    IParseable<int>,
    ISubtractionOperators<int, int, int>,
    IUnaryNegationOperators<int, int>,
    IUnaryPlusOperators<int, int>,
    IShiftOperators<int, int>,
    IMinMaxValue<int>,
    ISignedNumber<int>
  {
    private readonly int m_value;
    /// <summary>Represents the largest possible value of an <see cref="T:System.Int32" />. This field is constant.</summary>
    public const int MaxValue = 2147483647;
    /// <summary>Represents the smallest possible value of <see cref="T:System.Int32" />. This field is constant.</summary>
    public const int MinValue = -2147483648;

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not an <see cref="T:System.Int32" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is int num))
        throw new ArgumentException(SR.Arg_MustBeInt32);
      if (this < num)
        return -1;
      return this > num ? 1 : 0;
    }

    /// <summary>Compares this instance to a specified 32-bit signed integer and returns an indication of their relative values.</summary>
    /// <param name="value">An integer to compare.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(int value)
    {
      if (this < value)
        return -1;
      return this > value ? 1 : 0;
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Int32" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is int num && this == num;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.Int32" /> value.</summary>
    /// <param name="obj">An <see cref="T:System.Int32" /> value to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(int obj) => this == obj;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => this;

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance, consisting of a negative sign if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.</returns>
    public override string ToString() => Number.Int32ToDecStr(this);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid or not supported.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => this.ToString(format, (IFormatProvider) null);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatInt32(this, 0, (string) null, provider);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid or not supported.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatInt32(this, -1, format, provider);

    /// <summary>Tries to format the value of the current integer number instance into the provided span of characters.</summary>
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
      return Number.TryFormatInt32(this, -1, format, provider, destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its 32-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="s" />.</returns>
    public static int Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt32((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its 32-bit signed integer equivalent.</summary>
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
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 32-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static int Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt32((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified culture-specific format to its 32-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not of the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static int Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt32((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 32-bit signed integer equivalent.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="s" />.</param>
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
    ///        <paramref name="s" /> represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> includes non-zero, fractional digits.</exception>
    /// <returns>A 32-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static int Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseInt32((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 32-bit signed integer equivalent.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="s" />.</param>
    /// <returns>A 32-bit signed integer equivalent to the number specified in <paramref name="s" />.</returns>
    public static int Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number to its 32-bit signed integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the 32-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not of the correct format, or represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out int result)
    {
      if (s != null)
        return Number.TryParseInt32IntegerStyle((ReadOnlySpan<char>) s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
      result = 0;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 32-bit signed integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert.</param>
    /// <param name="result">When this method returns, contains the 32-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out int result) => Number.TryParseInt32IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;

    /// <summary>Converts the string representation of a number in a specified style and culture-specific format to its 32-bit signed integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 32-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
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
      out int result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      if (s != null)
        return Number.TryParseInt32((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
      result = 0;
      return false;
    }

    /// <summary>Converts the span representation of a number in a specified style and culture-specific format to its 32-bit signed integer equivalent. A return value indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters that represent the number to convert. The span is interpreted using the style specified by <paramref name="style" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the 32-bit signed integer value equivalent of the number contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out int result)
    {
      NumberFormatInfo.ValidateParseStyleInteger(style);
      return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
    }

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Int32" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.Int32" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Int32;


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
    /// <returns>The value of the current instance, unchanged.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => this;

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
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Int32), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert this <see cref="T:System.Int32" /> value.</param>
    /// <param name="provider">An object that provides information about the format of the returned value.</param>
    /// <returns>The value of the current instance, converted to <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IAdditionOperators<int, int, int>.op_Addition(int left, int right) => left + right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IAdditiveIdentity<int, int>.AdditiveIdentity => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBinaryInteger<int>.LeadingZeroCount(int value) => BitOperations.LeadingZeroCount((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBinaryInteger<int>.PopCount(int value) => BitOperations.PopCount((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBinaryInteger<int>.RotateLeft(int value, int rotateAmount) => (int) BitOperations.RotateLeft((uint) value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBinaryInteger<int>.RotateRight(int value, int rotateAmount) => (int) BitOperations.RotateRight((uint) value, rotateAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBinaryInteger<int>.TrailingZeroCount(int value) => BitOperations.TrailingZeroCount(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<int>.IsPow2(int value) => BitOperations.IsPow2(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBinaryNumber<int>.Log2(int value)
    {
      if (value < 0)
        ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
      return BitOperations.Log2((uint) value);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBitwiseOperators<int, int, int>.op_BitwiseAnd(int left, int right) => left & right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBitwiseOperators<int, int, int>.op_BitwiseOr(int left, int right) => left | right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBitwiseOperators<int, int, int>.op_ExclusiveOr(int left, int right) => left ^ right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IBitwiseOperators<int, int, int>.op_OnesComplement(int value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<int, int>.op_LessThan(int left, int right) => left < right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<int, int>.op_LessThanOrEqual(int left, int right) => left <= right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<int, int>.op_GreaterThan(int left, int right) => left > right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<int, int>.op_GreaterThanOrEqual(
      int left,
      int right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IDecrementOperators<int>.op_Decrement(int value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IDivisionOperators<int, int, int>.op_Division(int left, int right) => left / right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<int, int>.op_Equality(int left, int right) => left == right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<int, int>.op_Inequality(int left, int right) => left != right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IIncrementOperators<int>.op_Increment(int value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IMinMaxValue<int>.MinValue => int.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IMinMaxValue<int>.MaxValue => int.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IModulusOperators<int, int, int>.op_Modulus(int left, int right) => left % right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IMultiplicativeIdentity<int, int>.MultiplicativeIdentity => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IMultiplyOperators<int, int, int>.op_Multiply(int left, int right) => left * right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.One => 1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Zero => 0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Abs(int value) => Math.Abs(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Clamp(int value, int min, int max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    internal static int Create<TOther>(TOther value) where TOther : INumber<TOther>
    {
      if (typeof (TOther) == typeof (byte))
        return (int) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (int) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (int) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((int) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return (int) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return checked ((int) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((int) (IntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return (int) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return checked ((int) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (int) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return checked ((int) (uint) (object) value);
      if (typeof (TOther) == typeof (ulong))
        return checked ((int) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((int) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int INumber<int>.Create<TOther>(TOther value) => int.Create<TOther>(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int INumber<int>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (int) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (int) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 2147483647M)
          return int.MaxValue;
        return !(num < -2147483648M) ? (int) num : int.MinValue;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) int.MaxValue)
          return int.MaxValue;
        return num >= (double) int.MinValue ? (int) num : int.MinValue;
      }
      if (typeof (TOther) == typeof (short))
        return (int) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (int) (object) value;
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) int.MaxValue)
          return int.MaxValue;
        return num >= (long) int.MinValue ? (int) num : int.MinValue;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num > new IntPtr(2147483647))
          return int.MaxValue;
        return num >= new IntPtr(-2147483648) ? (int) num : int.MinValue;
      }
      if (typeof (TOther) == typeof (sbyte))
        return (int) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > 2147483648.0)
          return int.MaxValue;
        return (double) num >= (double) int.MinValue ? (int) num : int.MinValue;
      }
      if (typeof (TOther) == typeof (ushort))
        return (int) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        return num <= (uint) int.MaxValue ? (int) num : int.MaxValue;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) int.MaxValue ? (int) num : int.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= new UIntPtr(2147483647) ? (int) num : int.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int INumber<int>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (int) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (int) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (int) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (int) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (int) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (int) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (int) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (int) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (int) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (int) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (int) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (int) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (int) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return 0;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (int Quotient, int Remainder) INumber<int>.DivRem(int left, int right) => Math.DivRem(left, right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Max(int x, int y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Min(int x, int y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Parse(string s, NumberStyles style, IFormatProvider provider) => int.Parse(s, style, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return int.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int INumber<int>.Sign(int value) => Math.Sign(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<int>.TryCreate<TOther>(TOther value, out int result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (int) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (int) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < -2147483648M || num > 2147483647M)
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < (double) int.MinValue || num > (double) int.MaxValue)
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (int) (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        result = (int) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < (long) int.MinValue || num > (long) int.MaxValue)
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < new IntPtr(-2147483648) || num > new IntPtr(2147483647))
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (int) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < (double) int.MinValue || (double) num > 2147483648.0)
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (int) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        if (num > (uint) int.MaxValue)
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) int.MaxValue)
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > new UIntPtr(2147483647))
        {
          result = 0;
          return false;
        }
        result = (int) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = 0;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<int>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out int result)
    {
      return int.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<int>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out int result)
    {
      return int.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IParseable<int>.Parse(string s, IFormatProvider provider) => int.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<int>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out int result) => int.TryParse(s, NumberStyles.Integer, provider, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IShiftOperators<int, int>.op_LeftShift(int value, int shiftAmount) => value << shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IShiftOperators<int, int>.op_RightShift(int value, int shiftAmount) => value >> shiftAmount;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int ISignedNumber<int>.NegativeOne => -1;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int ISpanParseable<int>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => int.Parse(s, NumberStyles.Integer, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<int>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out int result)
    {
      return int.TryParse(s, NumberStyles.Integer, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int ISubtractionOperators<int, int, int>.op_Subtraction(
      int left,
      int right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IUnaryNegationOperators<int, int>.op_UnaryNegation(int value) => -value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    int IUnaryPlusOperators<int, int>.op_UnaryPlus(int value) => value;
  }
}
