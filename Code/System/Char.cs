// Decompiled with JetBrains decompiler
// Type: System.Char
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;


#nullable enable
namespace System
{
  /// <summary>Represents a character as a UTF-16 code unit.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Char : 
    IComparable,
    IComparable<char>,
    IEquatable<char>,
    IConvertible,
    ISpanFormattable,
    IFormattable,
    IBinaryInteger<char>,
    IBinaryNumber<char>,
    IBitwiseOperators<char, char, char>,
    INumber<char>,
    IAdditionOperators<char, char, char>,
    IAdditiveIdentity<char, char>,
    IComparisonOperators<char, char>,
    IEqualityOperators<char, char>,
    IDecrementOperators<char>,
    IDivisionOperators<char, char, char>,
    IIncrementOperators<char>,
    IModulusOperators<char, char, char>,
    IMultiplicativeIdentity<char, char>,
    IMultiplyOperators<char, char, char>,
    ISpanParseable<char>,
    IParseable<char>,
    ISubtractionOperators<char, char, char>,
    IUnaryNegationOperators<char, char>,
    IUnaryPlusOperators<char, char>,
    IShiftOperators<char, char>,
    IMinMaxValue<char>,
    IUnsignedNumber<char>
  {
    private readonly char m_value;
    /// <summary>Represents the largest possible value of a <see cref="T:System.Char" />. This field is constant.</summary>
    public const char MaxValue = '\uFFFF';
    /// <summary>Represents the smallest possible value of a <see cref="T:System.Char" />. This field is constant.</summary>
    public const char MinValue = '\0';

    private static unsafe ReadOnlySpan<byte> Latin1CharInfo => new ReadOnlySpan<byte>((void*) &\u003CPrivateImplementationDetails\u003E.\u0031D715D2A2ED1CDD8C368F519DF4B8B9748F65E031AEA80652432FBBA5C35DFE6, 256);

    private static bool IsLatin1(char c) => (uint) c < (uint) char.Latin1CharInfo.Length;

    /// <summary>Returns <see langword="true" /> if <paramref name="c" /> is an ASCII character ([ U+0000..U+007F ]).</summary>
    /// <param name="c">The character to analyze.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is an ASCII character; <see langword="false" /> otherwise.</returns>
    public static bool IsAscii(char c) => c <= '\u007F';

    private static UnicodeCategory GetLatin1UnicodeCategory(char c) => (UnicodeCategory) ((int) char.Latin1CharInfo[(int) c] & 31);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this | (int) this << 16;

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Char" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is char ch && (int) this == (int) ch;

    /// <summary>Returns a value that indicates whether this instance is equal to the specified <see cref="T:System.Char" /> object.</summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="obj" /> parameter equals the value of this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(char obj) => (int) this == (int) obj;

    /// <summary>Compares this instance to a specified object and indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified <see cref="T:System.Object" />.</summary>
    /// <param name="value">An object to compare this instance to, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.Char" /> object.</exception>
    /// <returns>A signed number indicating the position of this instance in the sort order in relation to the <paramref name="value" /> parameter.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance has the same position in the sort order as <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="value" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is char ch))
        throw new ArgumentException(SR.Arg_MustBeChar);
      return (int) this - (int) ch;
    }

    /// <summary>Compares this instance to a specified <see cref="T:System.Char" /> object and indicates whether this instance precedes, follows, or appears in the same position in the sort order as the specified <see cref="T:System.Char" /> object.</summary>
    /// <param name="value">A <see cref="T:System.Char" /> object to compare.</param>
    /// <returns>A signed number indicating the position of this instance in the sort order in relation to the <paramref name="value" /> parameter.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance has the same position in the sort order as <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(char value) => (int) this - (int) value;

    /// <summary>Converts the value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance.</returns>
    public override string ToString() => char.ToString(this);

    /// <summary>Converts the value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">(Reserved) An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => char.ToString(this);

    /// <summary>Converts the specified Unicode character to its equivalent string representation.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <returns>The string representation of the value of <paramref name="c" />.</returns>
    public static string ToString(char c) => string.CreateFromChar(c);


    #nullable disable
    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    bool ISpanFormattable.TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format,
      IFormatProvider provider)
    {
      if (!destination.IsEmpty)
      {
        destination[0] = this;
        charsWritten = 1;
        return true;
      }
      charsWritten = 0;
      return false;
    }

    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use.
    /// -or-
    /// A <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
    /// <param name="formatProvider">The provider to use to format the value.
    /// -or-
    /// A <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
    /// <returns>The value of the current instance in the specified format.</returns>
    string IFormattable.ToString(string format, IFormatProvider formatProvider) => char.ToString(this);


    #nullable enable
    /// <summary>Converts the value of the specified string to its equivalent Unicode character.</summary>
    /// <param name="s">A string that contains a single character, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The length of <paramref name="s" /> is not 1.</exception>
    /// <returns>A Unicode character equivalent to the sole character in <paramref name="s" />.</returns>
    public static char Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return s.Length == 1 ? s[0] : throw new FormatException(SR.Format_NeedSingleChar);
    }

    /// <summary>Converts the value of the specified string to its equivalent Unicode character. A return code indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A string that contains a single character, or <see langword="null" />.</param>
    /// <param name="result">When this method returns, contains a Unicode character equivalent to the sole character in <paramref name="s" />, if the conversion succeeded, or an undefined value if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or the length of <paramref name="s" /> is not 1. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out char result)
    {
      result = char.MinValue;
      if (s == null || s.Length != 1)
        return false;
      result = s[0];
      return true;
    }

    /// <summary>Indicates whether the specified Unicode character is categorized as a decimal digit.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a decimal digit; otherwise, <see langword="false" />.</returns>
    public static bool IsDigit(char c) => char.IsLatin1(c) ? char.IsInRange(c, '0', '9') : CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber;

    internal static bool IsInRange(char c, char min, char max) => (uint) c - (uint) min <= (uint) max - (uint) min;

    private static bool IsInRange(UnicodeCategory c, UnicodeCategory min, UnicodeCategory max) => (uint) (c - min) <= (uint) (max - min);

    internal static bool CheckLetter(UnicodeCategory uc) => char.IsInRange(uc, UnicodeCategory.UppercaseLetter, UnicodeCategory.OtherLetter);

    /// <summary>Indicates whether the specified Unicode character is categorized as a Unicode letter.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a letter; otherwise, <see langword="false" />.</returns>
    public static bool IsLetter(char c) => char.IsAscii(c) ? ((uint) char.Latin1CharInfo[(int) c] & 96U) > 0U : char.CheckLetter(CharUnicodeInfo.GetUnicodeCategory(c));

    private static bool IsWhiteSpaceLatin1(char c) => ((uint) char.Latin1CharInfo[(int) c] & 128U) > 0U;

    /// <summary>Indicates whether the specified Unicode character is categorized as white space.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is white space; otherwise, <see langword="false" />.</returns>
    public static bool IsWhiteSpace(char c) => char.IsLatin1(c) ? char.IsWhiteSpaceLatin1(c) : CharUnicodeInfo.GetIsWhiteSpace(c);

    /// <summary>Indicates whether the specified Unicode character is categorized as an uppercase letter.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is an uppercase letter; otherwise, <see langword="false" />.</returns>
    public static bool IsUpper(char c) => char.IsLatin1(c) ? ((uint) char.Latin1CharInfo[(int) c] & 64U) > 0U : CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.UppercaseLetter;

    /// <summary>Indicates whether the specified Unicode character is categorized as a lowercase letter.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a lowercase letter; otherwise, <see langword="false" />.</returns>
    public static bool IsLower(char c) => char.IsLatin1(c) ? ((uint) char.Latin1CharInfo[(int) c] & 32U) > 0U : CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter;

    internal static bool CheckPunctuation(UnicodeCategory uc) => char.IsInRange(uc, UnicodeCategory.ConnectorPunctuation, UnicodeCategory.OtherPunctuation);

    /// <summary>Indicates whether the specified Unicode character is categorized as a punctuation mark.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a punctuation mark; otherwise, <see langword="false" />.</returns>
    public static bool IsPunctuation(char c) => char.IsLatin1(c) ? char.CheckPunctuation(char.GetLatin1UnicodeCategory(c)) : char.CheckPunctuation(CharUnicodeInfo.GetUnicodeCategory(c));

    internal static bool CheckLetterOrDigit(UnicodeCategory uc) => char.CheckLetter(uc) || uc == UnicodeCategory.DecimalDigitNumber;

    /// <summary>Indicates whether the specified Unicode character is categorized as a letter or a decimal digit.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a letter or a decimal digit; otherwise, <see langword="false" />.</returns>
    public static bool IsLetterOrDigit(char c) => char.IsLatin1(c) ? char.CheckLetterOrDigit(char.GetLatin1UnicodeCategory(c)) : char.CheckLetterOrDigit(CharUnicodeInfo.GetUnicodeCategory(c));

    /// <summary>Converts the value of a specified Unicode character to its uppercase equivalent using specified culture-specific formatting information.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="culture" /> is <see langword="null" />.</exception>
    /// <returns>The uppercase equivalent of <paramref name="c" />, modified according to <paramref name="culture" />, or the unchanged value of <paramref name="c" /> if <paramref name="c" /> is already uppercase, has no uppercase equivalent, or is not alphabetic.</returns>
    public static char ToUpper(char c, CultureInfo culture)
    {
      if (culture == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.culture);
      return culture.TextInfo.ToUpper(c);
    }

    /// <summary>Converts the value of a Unicode character to its uppercase equivalent.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <returns>The uppercase equivalent of <paramref name="c" />, or the unchanged value of <paramref name="c" /> if <paramref name="c" /> is already uppercase, has no uppercase equivalent, or is not alphabetic.</returns>
    public static char ToUpper(char c) => CultureInfo.CurrentCulture.TextInfo.ToUpper(c);

    /// <summary>Converts the value of a Unicode character to its uppercase equivalent using the casing rules of the invariant culture.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <returns>The uppercase equivalent of the <paramref name="c" /> parameter, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already uppercase or not alphabetic.</returns>
    public static char ToUpperInvariant(char c) => TextInfo.ToUpperInvariant(c);

    /// <summary>Converts the value of a specified Unicode character to its lowercase equivalent using specified culture-specific formatting information.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="culture" /> is <see langword="null" />.</exception>
    /// <returns>The lowercase equivalent of <paramref name="c" />, modified according to <paramref name="culture" />, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already lowercase or not alphabetic.</returns>
    public static char ToLower(char c, CultureInfo culture)
    {
      if (culture == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.culture);
      return culture.TextInfo.ToLower(c);
    }

    /// <summary>Converts the value of a Unicode character to its lowercase equivalent.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <returns>The lowercase equivalent of <paramref name="c" />, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already lowercase or not alphabetic.</returns>
    public static char ToLower(char c) => CultureInfo.CurrentCulture.TextInfo.ToLower(c);

    /// <summary>Converts the value of a Unicode character to its lowercase equivalent using the casing rules of the invariant culture.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <returns>The lowercase equivalent of the <paramref name="c" /> parameter, or the unchanged value of <paramref name="c" />, if <paramref name="c" /> is already lowercase or not alphabetic.</returns>
    public static char ToLowerInvariant(char c) => TextInfo.ToLowerInvariant(c);

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Char" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.Char" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Char;


    #nullable disable
    /// <summary>Note This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Char), (object) "Boolean"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToChar(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current <see cref="T:System.Char" /> object unchanged.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => this;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> object. (Specify <see langword="null" /> because the <paramref name="provider" /> parameter is ignored.)</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> object. (Specify <see langword="null" /> because the <paramref name="provider" /> parameter is ignored.)</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> object. (Specify <see langword="null" /> because the <paramref name="provider" /> parameter is ignored.)</param>
    /// <returns>The converted value of the current <see cref="T:System.Char" /> object.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(this);

    /// <summary>Note This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>No value is returned.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Char), (object) "Single"));

    /// <summary>Note This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>No value is returned.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Char), (object) "Double"));

    /// <summary>Note This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>No value is returned.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Char), (object) "Decimal"));

    /// <summary>Note This conversion is not supported. Attempting to do so throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>No value is returned.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Char), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">A <see cref="T:System.Type" /> object.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> object.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidCastException">The value of the current <see cref="T:System.Char" /> object cannot be converted to the type specified by the <paramref name="type" /> parameter.</exception>
    /// <returns>An object of the specified type.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    /// <summary>Indicates whether the specified Unicode character is categorized as a control character.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a control character; otherwise, <see langword="false" />.</returns>
    public static bool IsControl(char c) => (uint) ((int) c + 1 & -129) <= 32U;


    #nullable enable
    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a control character.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a control character; otherwise, <see langword="false" />.</returns>
    public static bool IsControl(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return char.IsControl(s[index]);
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a decimal digit.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a decimal digit; otherwise, <see langword="false" />.</returns>
    public static bool IsDigit(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char c = s[index];
      return char.IsLatin1(c) ? char.IsInRange(c, '0', '9') : CharUnicodeInfo.GetUnicodeCategoryInternal(s, index) == UnicodeCategory.DecimalDigitNumber;
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a Unicode letter.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a letter; otherwise, <see langword="false" />.</returns>
    public static bool IsLetter(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char ch = s[index];
      return char.IsAscii(ch) ? ((uint) char.Latin1CharInfo[(int) ch] & 96U) > 0U : char.CheckLetter(CharUnicodeInfo.GetUnicodeCategoryInternal(s, index));
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a letter or a decimal digit.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a letter or a decimal digit; otherwise, <see langword="false" />.</returns>
    public static bool IsLetterOrDigit(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char c = s[index];
      return char.IsLatin1(c) ? char.CheckLetterOrDigit(char.GetLatin1UnicodeCategory(c)) : char.CheckLetterOrDigit(CharUnicodeInfo.GetUnicodeCategoryInternal(s, index));
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a lowercase letter.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a lowercase letter; otherwise, <see langword="false" />.</returns>
    public static bool IsLower(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char ch = s[index];
      return char.IsLatin1(ch) ? ((uint) char.Latin1CharInfo[(int) ch] & 32U) > 0U : CharUnicodeInfo.GetUnicodeCategoryInternal(s, index) == UnicodeCategory.LowercaseLetter;
    }

    internal static bool CheckNumber(UnicodeCategory uc) => char.IsInRange(uc, UnicodeCategory.DecimalDigitNumber, UnicodeCategory.OtherNumber);

    /// <summary>Indicates whether the specified Unicode character is categorized as a number.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a number; otherwise, <see langword="false" />.</returns>
    public static bool IsNumber(char c)
    {
      if (!char.IsLatin1(c))
        return char.CheckNumber(CharUnicodeInfo.GetUnicodeCategory(c));
      return char.IsAscii(c) ? char.IsInRange(c, '0', '9') : char.CheckNumber(char.GetLatin1UnicodeCategory(c));
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a number.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a number; otherwise, <see langword="false" />.</returns>
    public static bool IsNumber(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char c = s[index];
      if (!char.IsLatin1(c))
        return char.CheckNumber(CharUnicodeInfo.GetUnicodeCategoryInternal(s, index));
      return char.IsAscii(c) ? char.IsInRange(c, '0', '9') : char.CheckNumber(char.GetLatin1UnicodeCategory(c));
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a punctuation mark.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a punctuation mark; otherwise, <see langword="false" />.</returns>
    public static bool IsPunctuation(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char c = s[index];
      return char.IsLatin1(c) ? char.CheckPunctuation(char.GetLatin1UnicodeCategory(c)) : char.CheckPunctuation(CharUnicodeInfo.GetUnicodeCategoryInternal(s, index));
    }

    internal static bool CheckSeparator(UnicodeCategory uc) => char.IsInRange(uc, UnicodeCategory.SpaceSeparator, UnicodeCategory.ParagraphSeparator);

    private static bool IsSeparatorLatin1(char c) => c == ' ' || c == ' ';

    /// <summary>Indicates whether the specified Unicode character is categorized as a separator character.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a separator character; otherwise, <see langword="false" />.</returns>
    public static bool IsSeparator(char c) => char.IsLatin1(c) ? char.IsSeparatorLatin1(c) : char.CheckSeparator(CharUnicodeInfo.GetUnicodeCategory(c));

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a separator character.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a separator character; otherwise, <see langword="false" />.</returns>
    public static bool IsSeparator(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char c = s[index];
      return char.IsLatin1(c) ? char.IsSeparatorLatin1(c) : char.CheckSeparator(CharUnicodeInfo.GetUnicodeCategoryInternal(s, index));
    }

    /// <summary>Indicates whether the specified character has a surrogate code unit.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is either a high surrogate or a low surrogate; otherwise, <see langword="false" />.</returns>
    public static bool IsSurrogate(char c) => char.IsInRange(c, '\uD800', '\uDFFF');

    /// <summary>Indicates whether the character at the specified position in a specified string has a surrogate code unit.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a either a high surrogate or a low surrogate; otherwise, <see langword="false" />.</returns>
    public static bool IsSurrogate(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return char.IsSurrogate(s[index]);
    }

    internal static bool CheckSymbol(UnicodeCategory uc) => char.IsInRange(uc, UnicodeCategory.MathSymbol, UnicodeCategory.OtherSymbol);

    /// <summary>Indicates whether the specified Unicode character is categorized as a symbol character.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="c" /> is a symbol character; otherwise, <see langword="false" />.</returns>
    public static bool IsSymbol(char c) => char.IsLatin1(c) ? char.CheckSymbol(char.GetLatin1UnicodeCategory(c)) : char.CheckSymbol(CharUnicodeInfo.GetUnicodeCategory(c));

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as a symbol character.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is a symbol character; otherwise, <see langword="false" />.</returns>
    public static bool IsSymbol(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char c = s[index];
      return char.IsLatin1(c) ? char.CheckSymbol(char.GetLatin1UnicodeCategory(c)) : char.CheckSymbol(CharUnicodeInfo.GetUnicodeCategoryInternal(s, index));
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as an uppercase letter.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is an uppercase letter; otherwise, <see langword="false" />.</returns>
    public static bool IsUpper(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      char ch = s[index];
      return char.IsLatin1(ch) ? ((uint) char.Latin1CharInfo[(int) ch] & 64U) > 0U : CharUnicodeInfo.GetUnicodeCategoryInternal(s, index) == UnicodeCategory.UppercaseLetter;
    }

    /// <summary>Indicates whether the character at the specified position in a specified string is categorized as white space.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the character at position <paramref name="index" /> in <paramref name="s" /> is white space; otherwise, <see langword="false" />.</returns>
    public static bool IsWhiteSpace(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return char.IsWhiteSpace(s[index]);
    }

    /// <summary>Categorizes a specified Unicode character into a group identified by one of the <see cref="T:System.Globalization.UnicodeCategory" /> values.</summary>
    /// <param name="c">The Unicode character to categorize.</param>
    /// <returns>A <see cref="T:System.Globalization.UnicodeCategory" /> value that identifies the group that contains <paramref name="c" />.</returns>
    public static UnicodeCategory GetUnicodeCategory(char c) => char.IsLatin1(c) ? char.GetLatin1UnicodeCategory(c) : CharUnicodeInfo.GetUnicodeCategory((int) c);

    /// <summary>Categorizes the character at the specified position in a specified string into a group identified by one of the <see cref="T:System.Globalization.UnicodeCategory" /> values.</summary>
    /// <param name="s">A <see cref="T:System.String" />.</param>
    /// <param name="index">The character position in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>A <see cref="T:System.Globalization.UnicodeCategory" /> enumerated constant that identifies the group that contains the character at position <paramref name="index" /> in <paramref name="s" />.</returns>
    public static UnicodeCategory GetUnicodeCategory(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return char.IsLatin1(s[index]) ? char.GetLatin1UnicodeCategory(s[index]) : CharUnicodeInfo.GetUnicodeCategoryInternal(s, index);
    }

    /// <summary>Converts the specified numeric Unicode character to a double-precision floating point number.</summary>
    /// <param name="c">The Unicode character to convert.</param>
    /// <returns>The numeric value of <paramref name="c" /> if that character represents a number; otherwise, -1.0.</returns>
    public static double GetNumericValue(char c) => CharUnicodeInfo.GetNumericValue(c);

    /// <summary>Converts the numeric Unicode character at the specified position in a specified string to a double-precision floating point number.</summary>
    /// <param name="s">A <see cref="T:System.String" />.</param>
    /// <param name="index">The character position in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than zero or greater than the last position in <paramref name="s" />.</exception>
    /// <returns>The numeric value of the character at position <paramref name="index" /> in <paramref name="s" /> if that character represents a number; otherwise, -1.</returns>
    public static double GetNumericValue(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return CharUnicodeInfo.GetNumericValueInternal(s, index);
    }

    /// <summary>Indicates whether the specified <see cref="T:System.Char" /> object is a high surrogate.</summary>
    /// <param name="c">The Unicode character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if the numeric value of the <paramref name="c" /> parameter ranges from U+D800 through U+DBFF; otherwise, <see langword="false" />.</returns>
    public static bool IsHighSurrogate(char c) => char.IsInRange(c, '\uD800', '\uDBFF');

    /// <summary>Indicates whether the <see cref="T:System.Char" /> object at the specified position in a string is a high surrogate.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a position within <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the numeric value of the specified character in the <paramref name="s" /> parameter ranges from U+D800 through U+DBFF; otherwise, <see langword="false" />.</returns>
    public static bool IsHighSurrogate(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return char.IsHighSurrogate(s[index]);
    }

    /// <summary>Indicates whether the specified <see cref="T:System.Char" /> object is a low surrogate.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>
    /// <see langword="true" /> if the numeric value of the <paramref name="c" /> parameter ranges from U+DC00 through U+DFFF; otherwise, <see langword="false" />.</returns>
    public static bool IsLowSurrogate(char c) => char.IsInRange(c, '\uDC00', '\uDFFF');

    /// <summary>Indicates whether the <see cref="T:System.Char" /> object at the specified position in a string is a low surrogate.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The position of the character to evaluate in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a position within <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the numeric value of the specified character in the <paramref name="s" /> parameter ranges from U+DC00 through U+DFFF; otherwise, <see langword="false" />.</returns>
    public static bool IsLowSurrogate(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return char.IsLowSurrogate(s[index]);
    }

    /// <summary>Indicates whether two adjacent <see cref="T:System.Char" /> objects at a specified position in a string form a surrogate pair.</summary>
    /// <param name="s">A string.</param>
    /// <param name="index">The starting position of the pair of characters to evaluate within <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a position within <paramref name="s" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter includes adjacent characters at positions <paramref name="index" /> and <paramref name="index" /> + 1, and the numeric value of the character at position <paramref name="index" /> ranges from U+D800 through U+DBFF, and the numeric value of the character at position <paramref name="index" />+1 ranges from U+DC00 through U+DFFF; otherwise, <see langword="false" />.</returns>
    public static bool IsSurrogatePair(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if ((uint) index >= (uint) s.Length)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
      return index + 1 < s.Length && char.IsSurrogatePair(s[index], s[index + 1]);
    }

    /// <summary>Indicates whether the two specified <see cref="T:System.Char" /> objects form a surrogate pair.</summary>
    /// <param name="highSurrogate">The character to evaluate as the high surrogate of a surrogate pair.</param>
    /// <param name="lowSurrogate">The character to evaluate as the low surrogate of a surrogate pair.</param>
    /// <returns>
    /// <see langword="true" /> if the numeric value of the <paramref name="highSurrogate" /> parameter ranges from U+D800 through U+DBFF, and the numeric value of the <paramref name="lowSurrogate" /> parameter ranges from U+DC00 through U+DFFF; otherwise, <see langword="false" />.</returns>
    public static bool IsSurrogatePair(char highSurrogate, char lowSurrogate) => ((uint) highSurrogate - 55296U | (uint) lowSurrogate - 56320U) <= 1023U;

    /// <summary>Converts the specified Unicode code point into a UTF-16 encoded string.</summary>
    /// <param name="utf32">A 21-bit Unicode code point.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="utf32" /> is not a valid 21-bit Unicode code point ranging from U+0 through U+10FFFF, excluding the surrogate pair range from U+D800 through U+DFFF.</exception>
    /// <returns>A string consisting of one <see cref="T:System.Char" /> object or a surrogate pair of <see cref="T:System.Char" /> objects equivalent to the code point specified by the <paramref name="utf32" /> parameter.</returns>
    public static string ConvertFromUtf32(int utf32) => UnicodeUtility.IsValidUnicodeScalar((uint) utf32) ? Rune.UnsafeCreate((uint) utf32).ToString() : throw new ArgumentOutOfRangeException(nameof (utf32), SR.ArgumentOutOfRange_InvalidUTF32);

    /// <summary>Converts the value of a UTF-16 encoded surrogate pair into a Unicode code point.</summary>
    /// <param name="highSurrogate">A high surrogate code unit (that is, a code unit ranging from U+D800 through U+DBFF).</param>
    /// <param name="lowSurrogate">A low surrogate code unit (that is, a code unit ranging from U+DC00 through U+DFFF).</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="highSurrogate" /> is not in the range U+D800 through U+DBFF, or <paramref name="lowSurrogate" /> is not in the range U+DC00 through U+DFFF.</exception>
    /// <returns>The 21-bit Unicode code point represented by the <paramref name="highSurrogate" /> and <paramref name="lowSurrogate" /> parameters.</returns>
    public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
    {
      uint highSurrogateOffset = (uint) highSurrogate - 55296U;
      uint num = (uint) lowSurrogate - 56320U;
      if ((highSurrogateOffset | num) > 1023U)
        char.ConvertToUtf32_ThrowInvalidArgs(highSurrogateOffset);
      return ((int) highSurrogateOffset << 10) + ((int) lowSurrogate - 56320) + 65536;
    }

    [StackTraceHidden]
    private static void ConvertToUtf32_ThrowInvalidArgs(uint highSurrogateOffset)
    {
      if (highSurrogateOffset > 1023U)
        throw new ArgumentOutOfRangeException("highSurrogate", SR.ArgumentOutOfRange_InvalidHighSurrogate);
      throw new ArgumentOutOfRangeException("lowSurrogate", SR.ArgumentOutOfRange_InvalidLowSurrogate);
    }

    /// <summary>Converts the value of a UTF-16 encoded character or surrogate pair at a specified position in a string into a Unicode code point.</summary>
    /// <param name="s">A string that contains a character or surrogate pair.</param>
    /// <param name="index">The index position of the character or surrogate pair in <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a position within <paramref name="s" />.</exception>
    /// <exception cref="T:System.ArgumentException">The specified index position contains a surrogate pair, and either the first character in the pair is not a valid high surrogate or the second character in the pair is not a valid low surrogate.</exception>
    /// <returns>The 21-bit Unicode code point represented by the character or surrogate pair at the position in the <paramref name="s" /> parameter specified by the <paramref name="index" /> parameter.</returns>
    public static int ConvertToUtf32(string s, int index)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if (index < 0 || index >= s.Length)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_Index);
      int num1 = (int) s[index] - 55296;
      if (num1 < 0 || num1 > 2047)
        return (int) s[index];
      if (num1 > 1023)
        throw new ArgumentException(SR.Format(SR.Argument_InvalidLowSurrogate, (object) index), nameof (s));
      if (index >= s.Length - 1)
        throw new ArgumentException(SR.Format(SR.Argument_InvalidHighSurrogate, (object) index), nameof (s));
      int num2 = (int) s[index + 1] - 56320;
      if (num2 >= 0 && num2 <= 1023)
        return num1 * 1024 + num2 + 65536;
      throw new ArgumentException(SR.Format(SR.Argument_InvalidHighSurrogate, (object) index), nameof (s));
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IAdditionOperators<char, char, char>.op_Addition(char left, char right) => (char) ((uint) left + (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IAdditiveIdentity<char, char>.AdditiveIdentity => char.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBinaryInteger<char>.LeadingZeroCount(char value) => (char) (BitOperations.LeadingZeroCount((uint) value) - 16);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBinaryInteger<char>.PopCount(char value) => (char) BitOperations.PopCount((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBinaryInteger<char>.RotateLeft(char value, int rotateAmount) => (char) ((int) value << (rotateAmount & 15) | (int) value >> (16 - rotateAmount & 15));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBinaryInteger<char>.RotateRight(char value, int rotateAmount) => (char) ((int) value >> (rotateAmount & 15) | (int) value << (16 - rotateAmount & 15));

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBinaryInteger<char>.TrailingZeroCount(char value) => (char) (BitOperations.TrailingZeroCount((int) value << 16) - 16);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IBinaryNumber<char>.IsPow2(char value) => BitOperations.IsPow2((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBinaryNumber<char>.Log2(char value) => (char) BitOperations.Log2((uint) value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBitwiseOperators<char, char, char>.op_BitwiseAnd(char left, char right) => (char) ((uint) left & (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBitwiseOperators<char, char, char>.op_BitwiseOr(char left, char right) => (char) ((uint) left | (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBitwiseOperators<char, char, char>.op_ExclusiveOr(char left, char right) => (char) ((uint) left ^ (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IBitwiseOperators<char, char, char>.op_OnesComplement(char value) => ~value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<char, char>.op_LessThan(char left, char right) => (int) left < (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<char, char>.op_LessThanOrEqual(
      char left,
      char right)
    {
      return (int) left <= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<char, char>.op_GreaterThan(char left, char right) => (int) left > (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<char, char>.op_GreaterThanOrEqual(
      char left,
      char right)
    {
      return (int) left >= (int) right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IDecrementOperators<char>.op_Decrement(char value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IDivisionOperators<char, char, char>.op_Division(char left, char right) => (char) ((uint) left / (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<char, char>.op_Equality(char left, char right) => (int) left == (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<char, char>.op_Inequality(char left, char right) => (int) left != (int) right;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IIncrementOperators<char>.op_Increment(char value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IMinMaxValue<char>.MinValue => char.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IMinMaxValue<char>.MaxValue => char.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IModulusOperators<char, char, char>.op_Modulus(char left, char right) => (char) ((uint) left % (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IMultiplicativeIdentity<char, char>.MultiplicativeIdentity => '\u0001';

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IMultiplyOperators<char, char, char>.op_Multiply(char left, char right) => (char) ((uint) left * (uint) right);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.One => '\u0001';

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Zero => char.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Abs(char value) => value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Clamp(char value, char min, char max) => (char) Math.Clamp((ushort) value, (ushort) min, (ushort) max);


    #nullable disable
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    char INumber<char>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (char) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (char) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return checked ((char) (double) (object) value);
      if (typeof (TOther) == typeof (short))
        return checked ((char) (short) (object) value);
      if (typeof (TOther) == typeof (int))
        return checked ((char) (int) (object) value);
      if (typeof (TOther) == typeof (long))
        return checked ((char) (long) (object) value);
      if (typeof (TOther) == typeof (IntPtr))
        return checked ((char) (UIntPtr) (object) value);
      if (typeof (TOther) == typeof (sbyte))
        return checked ((char) (sbyte) (object) value);
      if (typeof (TOther) == typeof (float))
        return checked ((char) (float) (object) value);
      if (typeof (TOther) == typeof (ushort))
        return (char) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return checked ((char) (uint) (object) value);
      if (typeof (TOther) == typeof (ulong))
        return checked ((char) (ulong) (object) value);
      if (typeof (TOther) == typeof (UIntPtr))
        return checked ((char) (UIntPtr) (object) value);
      ThrowHelper.ThrowNotSupportedException();
      return char.MinValue;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    char INumber<char>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (char) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num > 65535M)
          return char.MaxValue;
        return !(num < 0M) ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num > (double) ushort.MaxValue)
          return char.MaxValue;
        return num >= 0.0 ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        return num >= (short) 0 ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num > (int) ushort.MaxValue)
          return char.MaxValue;
        return num >= 0 ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num > (long) ushort.MaxValue)
          return char.MaxValue;
        return num >= 0L ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num > new IntPtr(65535))
          return char.MaxValue;
        return num >= IntPtr.Zero ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        return num >= (sbyte) 0 ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num > (double) ushort.MaxValue)
          return char.MaxValue;
        return (double) num >= 0.0 ? (char) num : char.MinValue;
      }
      if (typeof (TOther) == typeof (ushort))
        return (char) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        return num <= (uint) ushort.MaxValue ? (char) num : char.MaxValue;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        return num <= (ulong) ushort.MaxValue ? (char) num : char.MaxValue;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        return num <= new UIntPtr(65535) ? (char) num : char.MaxValue;
      }
      ThrowHelper.ThrowNotSupportedException();
      return char.MinValue;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    char INumber<char>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (char) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (char) (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (char) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (char) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (char) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (char) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (char) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (char) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (char) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (char) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (char) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (char) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (char) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return char.MinValue;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (char Quotient, char Remainder) INumber<char>.DivRem(char left, char right)
    {
      (ushort Quotient, ushort Remainder) tuple = Math.DivRem((ushort) left, (ushort) right);
      return ((char) tuple.Quotient, (char) tuple.Remainder);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Max(char x, char y) => (char) Math.Max((ushort) x, (ushort) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Min(char x, char y) => (char) Math.Min((ushort) x, (ushort) y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Parse(string s, NumberStyles style, IFormatProvider provider) => char.Parse(s);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      if (s.Length != 1)
        throw new FormatException(SR.Format_NeedSingleChar);
      return s[0];
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char INumber<char>.Sign(char value) => value == char.MinValue ? char.MinValue : '\u0001';

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<char>.TryCreate<TOther>(TOther value, out char result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (char) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        Decimal num = (Decimal) (object) value;
        if (num < 0M || num > 65535M)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        double num = (double) (object) value;
        if (num < 0.0 || num > (double) ushort.MaxValue)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        short num = (short) (object) value;
        if (num < (short) 0)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        int num = (int) (object) value;
        if (num < 0 || num > (int) ushort.MaxValue)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        long num = (long) (object) value;
        if (num < 0L || num > (long) ushort.MaxValue)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        IntPtr num = (IntPtr) (object) value;
        if (num < IntPtr.Zero || num > new IntPtr(65535))
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        sbyte num = (sbyte) (object) value;
        if (num < (sbyte) 0)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        float num = (float) (object) value;
        if ((double) num < 0.0 || (double) num > (double) ushort.MaxValue)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        ushort num = (ushort) (object) value;
        if (num > ushort.MaxValue)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        uint num = (uint) (object) value;
        if (num > (uint) ushort.MaxValue)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        ulong num = (ulong) (object) value;
        if (num > (ulong) ushort.MaxValue)
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        UIntPtr num = (UIntPtr) (object) value;
        if (num > new UIntPtr(65535))
        {
          result = char.MinValue;
          return false;
        }
        result = (char) num;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = char.MinValue;
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<char>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out char result)
    {
      return char.TryParse(s, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<char>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out char result)
    {
      if (s.Length != 1)
      {
        result = char.MinValue;
        return false;
      }
      result = s[0];
      return true;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IParseable<char>.Parse(string s, IFormatProvider provider) => char.Parse(s);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<char>.TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out char result) => char.TryParse(s, out result);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IShiftOperators<char, char>.op_LeftShift(char value, int shiftAmount) => (char) ((uint) value << shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IShiftOperators<char, char>.op_RightShift(char value, int shiftAmount) => (char) ((uint) value >> shiftAmount);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char ISpanParseable<char>.Parse(ReadOnlySpan<char> s, IFormatProvider provider)
    {
      if (s.Length != 1)
        throw new FormatException(SR.Format_NeedSingleChar);
      return s[0];
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<char>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out char result)
    {
      if (s.Length != 1)
      {
        result = char.MinValue;
        return false;
      }
      result = s[0];
      return true;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char ISubtractionOperators<char, char, char>.op_Subtraction(
      char left,
      char right)
    {
      return (char) ((uint) left - (uint) right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IUnaryNegationOperators<char, char>.op_UnaryNegation(char value) => -value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    char IUnaryPlusOperators<char, char>.op_UnaryPlus(char value) => value;
  }
}
