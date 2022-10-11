// Decompiled with JetBrains decompiler
// Type: System.DateOnly
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Represents dates with values ranging from January 1, 0001 Anno Domini (Common Era) through December 31, 9999 A.D. (C.E.) in the Gregorian calendar.</summary>
  public readonly struct DateOnly : 
    IComparable,
    IComparable<DateOnly>,
    IEquatable<DateOnly>,
    ISpanFormattable,
    IFormattable,
    IComparisonOperators<DateOnly, DateOnly>,
    IEqualityOperators<DateOnly, DateOnly>,
    IMinMaxValue<DateOnly>,
    ISpanParseable<DateOnly>,
    IParseable<DateOnly>
  {
    private readonly int _dayNumber;

    private static int DayNumberFromDateTime(DateTime dt) => (int) (dt.Ticks / 864000000000L);

    private DateTime GetEquivalentDateTime() => DateTime.UnsafeCreate((long) this._dayNumber * 864000000000L);

    private DateOnly(int dayNumber) => this._dayNumber = dayNumber;

    /// <summary>Gets the earliest possible date that can be created.</summary>
    /// <returns>A <see cref="T:System.DateOnly" /> representing the earliest possible date that can be created.</returns>
    public static DateOnly MinValue => new DateOnly(0);

    /// <summary>Gets the latest possible date that can be created.</summary>
    /// <returns>A <see cref="T:System.DateOnly" /> representing the latest possible date that can be created.</returns>
    public static DateOnly MaxValue => new DateOnly(3652058);

    /// <summary>Creates a new instance of the <see cref="T:System.DateOnly" /> structure to the specified year, month, and day.</summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    public DateOnly(int year, int month, int day) => this._dayNumber = DateOnly.DayNumberFromDateTime(new DateTime(year, month, day));

    /// <summary>Creates a new instance of the <see cref="T:System.DateOnly" /> structure to the specified year, month, and day for the specified calendar.</summary>
    /// <param name="year">The year (1 through the number of years in calendar).</param>
    /// <param name="month">The month (1 through the number of months in calendar).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="calendar">The calendar that is used to interpret year, month, and day.<paramref name="month" />.</param>
    public DateOnly(int year, int month, int day, Calendar calendar) => this._dayNumber = DateOnly.DayNumberFromDateTime(new DateTime(year, month, day, calendar));

    /// <summary>Creates a new instance of the <see cref="T:System.DateOnly" /> structure to the specified number of days.</summary>
    /// <param name="dayNumber">The number of days since January 1, 0001 in the Proleptic Gregorian calendar.</param>
    /// <returns>A <see cref="T:System.DateOnly" /> structure instance to the specified number of days.</returns>
    public static DateOnly FromDayNumber(int dayNumber)
    {
      if ((uint) dayNumber > 3652058U)
        ThrowHelper.ThrowArgumentOutOfRange_DayNumber(dayNumber);
      return new DateOnly(dayNumber);
    }

    /// <summary>Gets the year component of the date represented by this instance.</summary>
    /// <returns>A number that represents the year component of the date.</returns>
    public int Year => this.GetEquivalentDateTime().Year;

    /// <summary>Gets the month component of the date represented by this instance.</summary>
    /// <returns>A number that represents the month component of the date.</returns>
    public int Month => this.GetEquivalentDateTime().Month;

    /// <summary>Gets the day component of the date represented by this instance.</summary>
    /// <returns>A number representing the day component of the date represented by this instance.</returns>
    public int Day => this.GetEquivalentDateTime().Day;

    /// <summary>Gets the day of the week represented by this instance.</summary>
    /// <returns>A number that represents the day of the week represented by this instance.</returns>
    public DayOfWeek DayOfWeek => this.GetEquivalentDateTime().DayOfWeek;

    /// <summary>Gets the day of the year represented by this instance.</summary>
    /// <returns>A number that represents the day of the year represented by this instance.</returns>
    public int DayOfYear => this.GetEquivalentDateTime().DayOfYear;

    /// <summary>Gets the number of days since January 1, 0001 in the Proleptic Gregorian calendar represented by this instance.</summary>
    /// <returns>The number of days since January 1, 0001 in the Proleptic Gregorian calendar represented by this instance.</returns>
    public int DayNumber => this._dayNumber;

    /// <summary>Adds the specified number of days to the value of this instance.</summary>
    /// <param name="value">The number of days to add. To subtract days, specify a negative number.</param>
    /// <returns>An instance whose value is the sum of the date represented by this instance and the number of days represented by value.</returns>
    public DateOnly AddDays(int value)
    {
      int dayNumber = this._dayNumber + value;
      if ((uint) dayNumber > 3652058U)
        ThrowOutOfRange();
      return new DateOnly(dayNumber);

      static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException("value", SR.ArgumentOutOfRange_AddValue);
    }

    /// <summary>Adds the specified number of months to the value of this instance.</summary>
    /// <param name="value">A number of months. The months parameter can be negative or positive.</param>
    /// <returns>An object whose value is the sum of the date represented by this instance and months.</returns>
    public DateOnly AddMonths(int value) => new DateOnly(DateOnly.DayNumberFromDateTime(this.GetEquivalentDateTime().AddMonths(value)));

    /// <summary>Adds the specified number of years to the value of this instance.</summary>
    /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
    /// <returns>An object whose value is the sum of the date represented by this instance and the number of years represented by value.</returns>
    public DateOnly AddYears(int value) => new DateOnly(DateOnly.DayNumberFromDateTime(this.GetEquivalentDateTime().AddYears(value)));

    /// <summary>Determines whether two specified instances of <see cref="T:System.DateOnly" /> are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left and right represent the same date; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(DateOnly left, DateOnly right) => left._dayNumber == right._dayNumber;

    /// <summary>Determines whether two specified instances of <see cref="T:System.DateOnly" /> are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left and right do not represent the same date; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(DateOnly left, DateOnly right) => left._dayNumber != right._dayNumber;

    /// <summary>Determines whether one specified <see cref="T:System.DateOnly" /> is later than another specified DateTime.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is later than right; otherwise, <see langword="false" />.</returns>
    public static bool operator >(DateOnly left, DateOnly right) => left._dayNumber > right._dayNumber;

    /// <summary>Determines whether one specified DateOnly represents a date that is the same as or later than another specified <see cref="T:System.DateOnly" />.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is the same as or later than right; otherwise, <see langword="false" />.</returns>
    public static bool operator >=(DateOnly left, DateOnly right) => left._dayNumber >= right._dayNumber;

    /// <summary>Determines whether one specified <see cref="T:System.DateOnly" /> is earlier than another specified <see cref="T:System.DateOnly" />.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is earlier than right; otherwise, <see langword="false" />.</returns>
    public static bool operator <(DateOnly left, DateOnly right) => left._dayNumber < right._dayNumber;

    /// <summary>Determines whether one specified <see cref="T:System.DateOnly" /> represents a date that is the same as or earlier than another specified <see cref="T:System.DateOnly" />.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is the same as or earlier than right; otherwise, <see langword="false" />.</returns>
    public static bool operator <=(DateOnly left, DateOnly right) => left._dayNumber <= right._dayNumber;

    /// <summary>Returns a <see cref="T:System.DateTime" /> that is set to the date of this <see cref="T:System.DateOnly" /> instance and the time of specified input time.</summary>
    /// <param name="time">The time of the day.</param>
    /// <returns>The <see cref="T:System.DateTime" /> instance composed of the date of the current <see cref="T:System.DateOnly" /> instance and the time specified by the input time.</returns>
    public DateTime ToDateTime(TimeOnly time) => new DateTime((long) this._dayNumber * 864000000000L + time.Ticks);

    /// <summary>Returns a <see cref="T:System.DateTime" /> instance with the specified input kind that is set to the date of this <see cref="T:System.DateOnly" /> instance and the time of specified input time.</summary>
    /// <param name="time">The time of the day.</param>
    /// <param name="kind">One of the enumeration values that indicates whether ticks specifies a local time, Coordinated Universal Time (UTC), or neither.</param>
    /// <returns>The <see cref="T:System.DateTime" /> instance composed of the date of the current <see cref="T:System.DateOnly" /> instance and the time specified by the input time.</returns>
    public DateTime ToDateTime(TimeOnly time, DateTimeKind kind) => new DateTime((long) this._dayNumber * 864000000000L + time.Ticks, kind);

    /// <summary>Returns a <see cref="T:System.DateOnly" /> instance that is set to the date part of the specified <paramref name="dateTime" />.</summary>
    /// <param name="dateTime">The <see cref="T:System.DateTime" /> instance.</param>
    /// <returns>The <see cref="T:System.DateOnly" /> instance composed of the date part of the specified input time <paramref name="dateTime" /> instance.</returns>
    public static DateOnly FromDateTime(DateTime dateTime) => new DateOnly(DateOnly.DayNumberFromDateTime(dateTime));

    /// <summary>Compares the value of this instance to a specified <see cref="T:System.DateOnly" /> value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.DateTime" /> value.</summary>
    /// <param name="value">The object to compare to the current instance.</param>
    /// <returns>Less than zero if this instance is earlier than value. Greater than zero if this instance is later than value. Zero if this instance is the same as value.</returns>
    public int CompareTo(DateOnly value) => this._dayNumber.CompareTo(value._dayNumber);

    /// <summary>Compares the value of this instance to a specified object that contains a specified <see cref="T:System.DateOnly" /> value, and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.DateOnly" /> value.</summary>
    /// <param name="value">A boxed object to compare, or <see langword="null" />.</param>
    /// <returns>Less than zero if this instance is earlier than value. Greater than zero if this instance is later than value. Zero if this instance is the same as value.</returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (value is DateOnly dateOnly)
        return this.CompareTo(dateOnly);
      throw new ArgumentException(SR.Arg_MustBeDateOnly);
    }

    /// <summary>Returns a value indicating whether the value of this instance is equal to the value of the specified <see cref="T:System.DateOnly" /> instance.</summary>
    /// <param name="value">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the value parameter equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(DateOnly value) => this._dayNumber == value._dayNumber;

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="value">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if value is an instance of DateOnly and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? value) => value is DateOnly dateOnly && this._dayNumber == dateOnly._dayNumber;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => this._dayNumber;

    /// <summary>Converts a memory span that contains string representation of a date to its <see cref="T:System.DateOnly" /> equivalent by using culture-specific format information and a formatting style.</summary>
    /// <param name="s">The memory span that contains the string to parse.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by provider and styles.</returns>
    public static DateOnly Parse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider = null,
      DateTimeStyles style = DateTimeStyles.None)
    {
      DateOnly result1;
      ParseFailureKind result2 = DateOnly.TryParseInternal(s, provider, style, out result1);
      if (result2 != ParseFailureKind.None)
        DateOnly.ThrowOnError(result2, s);
      return result1;
    }

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the characters that represent a date to convert.</param>
    /// <param name="format">A span containing the characters that represent a format specifier that defines the required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by format, provider, and style.</returns>
    public static DateOnly ParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider? provider = null,
      DateTimeStyles style = DateTimeStyles.None)
    {
      DateOnly result;
      ParseFailureKind exactInternal = DateOnly.TryParseExactInternal(s, format, provider, style, out result);
      if (exactInternal != ParseFailureKind.None)
        DateOnly.ThrowOnError(exactInternal, s);
      return result;
    }

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified array of formats.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the characters that represent a date to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by format, provider, and style.</returns>
    public static DateOnly ParseExact(ReadOnlySpan<char> s, string[] formats) => DateOnly.ParseExact(s, formats, (IFormatProvider) null);

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified array of formats, culture-specific format information, and style.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the characters that represent a date to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by format, provider, and style.</returns>
    public static DateOnly ParseExact(
      ReadOnlySpan<char> s,
      string[] formats,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      DateOnly result;
      ParseFailureKind exactInternal = DateOnly.TryParseExactInternal(s, formats, provider, style, out result);
      if (exactInternal != ParseFailureKind.None)
        DateOnly.ThrowOnError(exactInternal, s);
      return result;
    }

    /// <summary>Converts a string that contains string representation of a date to its <see cref="T:System.DateOnly" /> equivalent by using the conventions of the current culture.</summary>
    /// <param name="s">The string that contains the string to parse.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />.</returns>
    public static DateOnly Parse(string s) => DateOnly.Parse(s, (IFormatProvider) null, DateTimeStyles.None);

    /// <summary>Converts a string that contains string representation of a date to its <see cref="T:System.DateOnly" /> equivalent by using culture-specific format information and a formatting style.</summary>
    /// <param name="s">The string that contains the string to parse.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" /> for the parse operation to succeed, and that defines how to interpret the parsed date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by provider and styles.</returns>
    public static DateOnly Parse(string s, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return DateOnly.Parse(s.AsSpan(), provider, style);
    }

    /// <summary>Converts the specified string representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified format.
    /// The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A string containing the characters that represent a date to convert.</param>
    /// <param name="format">A string that represent a format specifier that defines the required format of <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by format.</returns>
    public static DateOnly ParseExact(string s, string format) => DateOnly.ParseExact(s, format, (IFormatProvider) null);

    /// <summary>Converts the specified string representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A string containing the characters that represent a date to convert.</param>
    /// <param name="format">A string containing the characters that represent a format specifier that defines the required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that provides additional information about <paramref name="s" />, about style elements that may be present in <paramref name="s" />, or about the conversion from <paramref name="s" /> to a <see cref="T:System.DateOnly" /> value. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by format, provider, and style.</returns>
    public static DateOnly ParseExact(
      string s,
      string format,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if (format == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
      return DateOnly.ParseExact(s.AsSpan(), format.AsSpan(), provider, style);
    }

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified array of formats.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the characters that represent a date to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by format, provider, and style.</returns>
    public static DateOnly ParseExact(string s, string[] formats) => DateOnly.ParseExact(s, formats, (IFormatProvider) null);

    /// <summary>Converts the specified string representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified array of formats, culture-specific format information, and style.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A string containing the characters that represent a date to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date.</exception>
    /// <returns>An object that is equivalent to the date contained in <paramref name="s" />, as specified by format, provider, and style.</returns>
    public static DateOnly ParseExact(
      string s,
      string[] formats,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return DateOnly.ParseExact(s.AsSpan(), formats, provider, style);
    }

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing the date to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the s parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out DateOnly result) => DateOnly.TryParse(s, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified array of formats, culture-specific format information, and style. And returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the characters that represent a date to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the s parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      return DateOnly.TryParseInternal(s, provider, style, out result) == ParseFailureKind.None;
    }


    #nullable disable
    private static ParseFailureKind TryParseInternal(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != DateTimeStyles.None)
      {
        result = new DateOnly();
        return ParseFailureKind.FormatWithParameter;
      }
      DateTimeResult result1 = new DateTimeResult();
      result1.Init(s);
      if (!DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), style, ref result1))
      {
        result = new DateOnly();
        return ParseFailureKind.FormatWithOriginalDateTime;
      }
      if ((result1.flags & (ParseFlags.HaveHour | ParseFlags.HaveMinute | ParseFlags.HaveSecond | ParseFlags.HaveTime | ParseFlags.TimeZoneUsed | ParseFlags.TimeZoneUtc | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern)) != (ParseFlags) 0)
      {
        result = new DateOnly();
        return ParseFailureKind.WrongParts;
      }
      result = new DateOnly(DateOnly.DayNumberFromDateTime(result1.parsedDate));
      return ParseFailureKind.None;
    }


    #nullable enable
    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified format and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing a date to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> is an empty string, or does not contain a date that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      out DateOnly result)
    {
      return DateOnly.TryParseExact(s, format, (IFormatProvider) null, DateTimeStyles.None, out result);
    }

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" />equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing a date to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> is an empty string, or does not contain a date that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      return DateOnly.TryParseExactInternal(s, format, provider, style, out result) == ParseFailureKind.None;
    }


    #nullable disable
    private static ParseFailureKind TryParseExactInternal(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != DateTimeStyles.None)
      {
        result = new DateOnly();
        return ParseFailureKind.FormatWithParameter;
      }
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'O':
          case 'o':
            format = (ReadOnlySpan<char>) "yyyy'-'MM'-'dd";
            provider = (IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat;
            break;
          case 'R':
          case 'r':
            format = (ReadOnlySpan<char>) "ddd, dd MMM yyyy";
            provider = (IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat;
            break;
        }
      }
      DateTimeResult result1 = new DateTimeResult();
      result1.Init(s);
      if (!DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, ref result1))
      {
        result = new DateOnly();
        return ParseFailureKind.FormatWithOriginalDateTime;
      }
      if ((result1.flags & (ParseFlags.HaveHour | ParseFlags.HaveMinute | ParseFlags.HaveSecond | ParseFlags.HaveTime | ParseFlags.TimeZoneUsed | ParseFlags.TimeZoneUtc | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern)) != (ParseFlags) 0)
      {
        result = new DateOnly();
        return ParseFailureKind.WrongParts;
      }
      result = new DateOnly(DateOnly.DayNumberFromDateTime(result1.parsedDate));
      return ParseFailureKind.None;
    }


    #nullable enable
    /// <summary>Converts the specified char span of a date to its <see cref="T:System.DateOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The span containing the string to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is an empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if<paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)] string?[]? formats, out DateOnly result) => DateOnly.TryParseExact(s, formats, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified char span of a date to its <see cref="T:System.DateOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The span containing the string to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that defines how to interpret the parsed date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      return DateOnly.TryParseExactInternal(s, formats, provider, style, out result) == ParseFailureKind.None;
    }


    #nullable disable
    private static ParseFailureKind TryParseExactInternal(
      ReadOnlySpan<char> s,
      string[] formats,
      IFormatProvider provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != DateTimeStyles.None || formats == null)
      {
        result = new DateOnly();
        return ParseFailureKind.FormatWithParameter;
      }
      DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(provider);
      for (int index = 0; index < formats.Length; ++index)
      {
        DateTimeFormatInfo dtfi = instance;
        string format = formats[index];
        if (string.IsNullOrEmpty(format))
        {
          result = new DateOnly();
          return ParseFailureKind.FormatWithFormatSpecifier;
        }
        if (format.Length == 1)
        {
          switch (format[0])
          {
            case 'O':
            case 'o':
              format = "yyyy'-'MM'-'dd";
              dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
              break;
            case 'R':
            case 'r':
              format = "ddd, dd MMM yyyy";
              dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
              break;
          }
        }
        DateTimeResult result1 = new DateTimeResult();
        result1.Init(s);
        if (DateTimeParse.TryParseExact(s, (ReadOnlySpan<char>) format, dtfi, style, ref result1) && (result1.flags & (ParseFlags.HaveHour | ParseFlags.HaveMinute | ParseFlags.HaveSecond | ParseFlags.HaveTime | ParseFlags.TimeZoneUsed | ParseFlags.TimeZoneUtc | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern)) == (ParseFlags) 0)
        {
          result = new DateOnly(DateOnly.DayNumberFromDateTime(result1.parsedDate));
          return ParseFailureKind.None;
        }
      }
      result = new DateOnly();
      return ParseFailureKind.FormatWithOriginalDateTime;
    }


    #nullable enable
    /// <summary>Converts the specified string representation of a date to its <see cref="T:System.DateOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the characters representing the date to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the s parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out DateOnly result) => DateOnly.TryParse(s, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified string representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified array of formats, culture-specific format information, and style. And returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the characters that represent a date to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the s parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      if (s != null)
        return DateOnly.TryParse(s.AsSpan(), provider, style, out result);
      result = new DateOnly();
      return false;
    }

    /// <summary>Converts the specified string representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified format and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the characters representing a date to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a date that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)] string? format, out DateOnly result) => DateOnly.TryParseExact(s, format, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified span representation of a date to its <see cref="T:System.DateOnly" /> equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing a date to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a date that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? s,
      [NotNullWhen(true)] string? format,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      if (s != null && format != null)
        return DateOnly.TryParseExact(s.AsSpan(), format.AsSpan(), provider, style, out result);
      result = new DateOnly();
      return false;
    }

    /// <summary>Converts the specified string of a date to its <see cref="T:System.DateOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The string containing date to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)] string?[]? formats, out DateOnly result) => DateOnly.TryParseExact(s, formats, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified string of a date to its <see cref="T:System.DateOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The string containing the date to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that defines how to interpret the parsed date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateOnly" /> value equivalent to the date contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? s,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateOnly result)
    {
      if (s != null)
        return DateOnly.TryParseExact(s.AsSpan(), formats, provider, style, out result);
      result = new DateOnly();
      return false;
    }


    #nullable disable
    private static void ThrowOnError(ParseFailureKind result, ReadOnlySpan<char> s)
    {
      switch (result)
      {
        case ParseFailureKind.FormatWithParameter:
          throw new ArgumentException(SR.Argument_InvalidDateStyles, "style");
        case ParseFailureKind.FormatWithOriginalDateTime:
          throw new FormatException(SR.Format(SR.Format_BadDateOnly, (object) s.ToString()));
        case ParseFailureKind.FormatWithFormatSpecifier:
          throw new FormatException(SR.Argument_BadFormatSpecifier);
        default:
          throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, (object) s.ToString(), (object) nameof (DateOnly)));
      }
    }


    #nullable enable
    /// <summary>Converts the value of the current <see cref="T:System.DateOnly" /> object to its equivalent long date string representation.</summary>
    /// <returns>A string that contains the long date string representation of the current <see cref="T:System.DateOnly" /> object.</returns>
    public string ToLongDateString() => this.ToString("D");

    /// <summary>Converts the value of the current <see cref="T:System.DateOnly" /> object to its equivalent short date string representation.</summary>
    /// <returns>A string that contains the short date string representation of the current <see cref="T:System.DateOnly" /> object.</returns>
    public string ToShortDateString() => this.ToString();

    /// <summary>Converts the value of the current <see cref="T:System.DateOnly" /> object to its equivalent string representation using the formatting conventions of the current culture.
    /// The <see cref="T:System.DateOnly" /> object will be formatted in short form.</summary>
    /// <returns>A string that contains the short date string representation of the current <see cref="T:System.DateOnly" /> object.</returns>
    public override string ToString() => this.ToString("d");

    /// <summary>Converts the value of the current <see cref="T:System.DateOnly" /> object to its equivalent string representation using the specified format and the formatting conventions of the current culture.</summary>
    /// <param name="format">A standard or custom date format string.</param>
    /// <returns>A string representation of value of the current <see cref="T:System.DateOnly" /> object as specified by format.</returns>
    public string ToString(string? format) => this.ToString(format, (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.DateOnly" /> object to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of the current <see cref="T:System.DateOnly" /> object as specified by provider.</returns>
    public string ToString(IFormatProvider? provider) => this.ToString("d", provider);

    /// <summary>Converts the value of the current <see cref="T:System.DateOnly" /> object to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="format">A standard or custom date format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of the current <see cref="T:System.DateOnly" /> object as specified by format and provider.</returns>
    public string ToString(string? format, IFormatProvider? provider)
    {
      switch (format)
      {
        case "":
        case null:
          format = "d";
          break;
      }
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'D':
          case 'M':
          case 'Y':
          case 'd':
          case 'm':
          case 'y':
            return DateTimeFormat.Format(this.GetEquivalentDateTime(), format, provider);
          case 'O':
          case 'o':
            return string.Create<DateOnly>(10, this, (SpanAction<char, DateOnly>) ((destination, value) => DateTimeFormat.TryFormatDateOnlyO(value.Year, value.Month, value.Day, destination)));
          case 'R':
          case 'r':
            return string.Create<DateOnly>(16, this, (SpanAction<char, DateOnly>) ((destination, value) => DateTimeFormat.TryFormatDateOnlyR(value.DayOfWeek, value.Year, value.Month, value.Day, destination)));
          default:
            throw new FormatException(SR.Format_InvalidString);
        }
      }
      else
      {
        DateTimeFormat.IsValidCustomDateFormat(format.AsSpan(), true);
        return DateTimeFormat.Format(this.GetEquivalentDateTime(), format, provider);
      }
    }

    /// <summary>Tries to format the value of the current <see cref="T:System.DateOnly" /> instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in destination.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for destination.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for destination.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      if (format.Length == 0)
        format = (ReadOnlySpan<char>) "d";
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'D':
          case 'M':
          case 'Y':
          case 'd':
          case 'm':
          case 'y':
            return DateTimeFormat.TryFormat(this.GetEquivalentDateTime(), destination, out charsWritten, format, provider);
          case 'O':
          case 'o':
            if (!DateTimeFormat.TryFormatDateOnlyO(this.Year, this.Month, this.Day, destination))
            {
              charsWritten = 0;
              return false;
            }
            charsWritten = 10;
            return true;
          case 'R':
          case 'r':
            if (!DateTimeFormat.TryFormatDateOnlyR(this.DayOfWeek, this.Year, this.Month, this.Day, destination))
            {
              charsWritten = 0;
              return false;
            }
            charsWritten = 16;
            return true;
          default:
            throw new FormatException(SR.Argument_BadFormatSpecifier);
        }
      }
      else
      {
        if (!DateTimeFormat.IsValidCustomDateFormat(format, false))
          throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, (object) format.ToString(), (object) nameof (DateOnly)));
        return DateTimeFormat.TryFormat(this.GetEquivalentDateTime(), destination, out charsWritten, format, provider);
      }
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateOnly, DateOnly>.op_LessThan(
      DateOnly left,
      DateOnly right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateOnly, DateOnly>.op_LessThanOrEqual(
      DateOnly left,
      DateOnly right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateOnly, DateOnly>.op_GreaterThan(
      DateOnly left,
      DateOnly right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateOnly, DateOnly>.op_GreaterThanOrEqual(
      DateOnly left,
      DateOnly right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<DateOnly, DateOnly>.op_Equality(
      DateOnly left,
      DateOnly right)
    {
      return left == right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<DateOnly, DateOnly>.op_Inequality(
      DateOnly left,
      DateOnly right)
    {
      return left != right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateOnly IMinMaxValue<DateOnly>.MinValue => DateOnly.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateOnly IMinMaxValue<DateOnly>.MaxValue => DateOnly.MaxValue;


    #nullable disable
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateOnly IParseable<DateOnly>.Parse(
      string s,
      IFormatProvider provider)
    {
      return DateOnly.Parse(s, provider, DateTimeStyles.None);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<DateOnly>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out DateOnly result)
    {
      return DateOnly.TryParse(s, provider, DateTimeStyles.None, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateOnly ISpanParseable<DateOnly>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return DateOnly.Parse(s, provider, DateTimeStyles.None);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<DateOnly>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out DateOnly result)
    {
      return DateOnly.TryParse(s, provider, DateTimeStyles.None, out result);
    }
  }
}
