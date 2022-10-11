// Decompiled with JetBrains decompiler
// Type: System.TimeOnly
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
  /// <summary>Represents a time of day, as would be read from a clock, within the range 00:00:00 to 23:59:59.9999999.</summary>
  public readonly struct TimeOnly : 
    IComparable,
    IComparable<TimeOnly>,
    IEquatable<TimeOnly>,
    ISpanFormattable,
    IFormattable,
    IComparisonOperators<TimeOnly, TimeOnly>,
    IEqualityOperators<TimeOnly, TimeOnly>,
    IMinMaxValue<TimeOnly>,
    ISpanParseable<TimeOnly>,
    IParseable<TimeOnly>,
    ISubtractionOperators<TimeOnly, TimeOnly, TimeSpan>
  {
    private readonly long _ticks;

    /// <summary>Represents the smallest possible value of <see cref="T:System.TimeOnly" />.</summary>
    /// <returns>A <see cref="T:System.TimeOnly" /> that represents its smallest possible value.</returns>
    public static TimeOnly MinValue => new TimeOnly(0UL);

    /// <summary>Represents the largest possible value of <see cref="T:System.TimeOnly" />.</summary>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance representing its largest possible value.</returns>
    public static TimeOnly MaxValue => new TimeOnly(863999999999UL);

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeOnly" /> structure to the specified hour and the minute.</summary>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    public TimeOnly(int hour, int minute)
      : this(DateTime.TimeToTicks(hour, minute, 0, 0))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeOnly" /> structure to the specified hour, minute, and second.</summary>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    public TimeOnly(int hour, int minute, int second)
      : this(DateTime.TimeToTicks(hour, minute, second, 0))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeOnly" /> structure to the specified hour, minute, second, and millisecond.</summary>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <param name="millisecond">The millisecond (0 through 999).</param>
    public TimeOnly(int hour, int minute, int second, int millisecond)
      : this(DateTime.TimeToTicks(hour, minute, second, millisecond))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeOnly" /> structure using a specified number of ticks.</summary>
    /// <param name="ticks">A time of day expressed in the number of 100-nanosecond units since 00:00:00.0000000.</param>
    public TimeOnly(long ticks) => this._ticks = (ulong) ticks <= 863999999999UL ? ticks : throw new ArgumentOutOfRangeException(nameof (ticks), SR.ArgumentOutOfRange_TimeOnlyBadTicks);

    internal TimeOnly(ulong ticks) => this._ticks = (long) ticks;

    /// <summary>Gets the hour component of the time represented by this instance.</summary>
    public int Hour => new TimeSpan(this._ticks).Hours;

    /// <summary>Gets the minute component of the time represented by this instance.</summary>
    /// <returns>A number representing the minute component of this <see cref="T:System.TimeOnly" />.</returns>
    public int Minute => new TimeSpan(this._ticks).Minutes;

    /// <summary>Gets the seconds component of the time represented by this instance.</summary>
    /// <returns>A number representing the seconds component of this instance.</returns>
    public int Second => new TimeSpan(this._ticks).Seconds;

    /// <summary>Gets the millisecond component of the time represented by this instance.</summary>
    /// <returns>A number representing the millisecond component of this <see cref="T:System.TimeOnly" />.</returns>
    public int Millisecond => new TimeSpan(this._ticks).Milliseconds;

    /// <summary>Gets the number of ticks that represent the time of this instance.</summary>
    /// <returns>A number representing the number of ticks of this instance.</returns>
    public long Ticks => this._ticks;

    private TimeOnly AddTicks(long ticks) => new TimeOnly((this._ticks + 864000000000L + ticks % 864000000000L) % 864000000000L);


    #nullable disable
    private TimeOnly AddTicks(long ticks, out int wrappedDays)
    {
      wrappedDays = (int) (ticks / 864000000000L);
      long ticks1 = this._ticks + ticks % 864000000000L;
      if (ticks1 < 0L)
      {
        --wrappedDays;
        ticks1 += 864000000000L;
      }
      else if (ticks1 >= 864000000000L)
      {
        ++wrappedDays;
        ticks1 -= 864000000000L;
      }
      return new TimeOnly(ticks1);
    }

    /// <summary>Returns a new <see cref="T:System.TimeOnly" /> that adds the value of the specified time span to the value of this instance.</summary>
    /// <param name="value">A positive or negative time interval.</param>
    /// <returns>An object whose value is the sum of the time represented by this instance and the time interval represented by value.</returns>
    public TimeOnly Add(TimeSpan value) => this.AddTicks(value.Ticks);


    #nullable enable
    /// <summary>Returns a new <see cref="T:System.TimeOnly" /> that adds the value of the specified time span to the value of this instance.
    /// If the result wraps past the end of the day, this method will return the number of excess days as an out parameter.</summary>
    /// <param name="value">A positive or negative time interval.</param>
    /// <param name="wrappedDays">When this method returns, contains the number of excess days if any that resulted from wrapping during this addition operation.</param>
    /// <returns>An object whose value is the sum of the time represented by this instance and the time interval represented by value.</returns>
    public TimeOnly Add(TimeSpan value, out int wrappedDays) => this.AddTicks(value.Ticks, out wrappedDays);

    /// <summary>Returns a new <see cref="T:System.TimeOnly" /> that adds the specified number of hours to the value of this instance.</summary>
    /// <param name="value">A number of whole and fractional hours. The value parameter can be negative or positive.</param>
    /// <returns>An object whose value is the sum of the time represented by this instance and the number of hours represented by value.</returns>
    public TimeOnly AddHours(double value) => this.AddTicks((long) (value * 36000000000.0));

    /// <summary>Returns a new <see cref="T:System.TimeOnly" /> that adds the specified number of hours to the value of this instance.
    /// If the result wraps past the end of the day, this method will return the number of excess days as an out parameter.</summary>
    /// <param name="value">A number of whole and fractional hours. The value parameter can be negative or positive.</param>
    /// <param name="wrappedDays">When this method returns, contains the number of excess days if any that resulted from wrapping during this addition operation.</param>
    /// <returns>An object whose value is the sum of the time represented by this instance and the number of hours represented by value.</returns>
    public TimeOnly AddHours(double value, out int wrappedDays) => this.AddTicks((long) (value * 36000000000.0), out wrappedDays);

    /// <summary>Returns a new <see cref="T:System.TimeOnly" /> that adds the specified number of minutes to the value of this instance.</summary>
    /// <param name="value">A number of whole and fractional minutes. The value parameter can be negative or positive.</param>
    /// <returns>An object whose value is the sum of the time represented by this instance and the number of minutes represented by value.</returns>
    public TimeOnly AddMinutes(double value) => this.AddTicks((long) (value * 600000000.0));

    /// <summary>Returns a new <see cref="T:System.TimeOnly" /> that adds the specified number of minutes to the value of this instance.
    /// If the result wraps past the end of the day, this method will return the number of excess days as an out parameter.</summary>
    /// <param name="value">A number of whole and fractional minutes. The value parameter can be negative or positive.</param>
    /// <param name="wrappedDays">When this method returns, contains the number of excess days if any that resulted from wrapping during this addition operation.</param>
    /// <returns>An object whose value is the sum of the time represented by this instance and the number of minutes represented by value.</returns>
    public TimeOnly AddMinutes(double value, out int wrappedDays) => this.AddTicks((long) (value * 600000000.0), out wrappedDays);

    /// <summary>Determines if a time falls within the range provided.
    /// Supports both "normal" ranges such as 10:00-12:00, and ranges that span midnight such as 23:00-01:00.</summary>
    /// <param name="start">The starting time of day, inclusive.</param>
    /// <param name="end">The ending time of day, exclusive.</param>
    /// <returns>
    /// <see langword="true" />, if the time falls within the range, <see langword="false" /> otherwise.</returns>
    public bool IsBetween(TimeOnly start, TimeOnly end)
    {
      long ticks1 = start._ticks;
      long ticks2 = end._ticks;
      return ticks1 > ticks2 ? ticks1 <= this._ticks || ticks2 > this._ticks : ticks1 <= this._ticks && ticks2 > this._ticks;
    }

    /// <summary>Determines whether two specified instances of <see cref="T:System.TimeOnly" />are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left and right represent the same time; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(TimeOnly left, TimeOnly right) => left._ticks == right._ticks;

    /// <summary>Determines whether two specified instances of <see cref="T:System.TimeOnly" /> are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left and right do not represent the same time; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(TimeOnly left, TimeOnly right) => left._ticks != right._ticks;

    /// <summary>Determines whether one specified <see cref="T:System.TimeOnly" /> is later than another specified <see cref="T:System.TimeOnly" />.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is later than right; otherwise, <see langword="false" />.</returns>
    public static bool operator >(TimeOnly left, TimeOnly right) => left._ticks > right._ticks;

    /// <summary>Determines whether one specified <see cref="T:System.TimeOnly" /> represents a time that is the same as or later than another specified <see cref="T:System.TimeOnly" />.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is the same as or later than right; otherwise, <see langword="false" />.</returns>
    public static bool operator >=(TimeOnly left, TimeOnly right) => left._ticks >= right._ticks;

    /// <summary>Determines whether one specified <see cref="T:System.TimeOnly" /> is earlier than another specified <see cref="T:System.TimeOnly" />.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is earlier than right; otherwise, <see langword="false" />.</returns>
    public static bool operator <(TimeOnly left, TimeOnly right) => left._ticks < right._ticks;

    /// <summary>Determines whether one specified <see cref="T:System.TimeOnly" /> represents a time that is the same as or earlier than another specified <see cref="T:System.TimeOnly" />.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if left is the same as or earlier than right; otherwise, <see langword="false" />.</returns>
    public static bool operator <=(TimeOnly left, TimeOnly right) => left._ticks <= right._ticks;

    /// <summary>Gives the elapsed time between two points on a circular clock, which will always be a positive value.</summary>
    /// <param name="t1">The first <see cref="T:System.TimeOnly" /> instance.</param>
    /// <param name="t2">The second <see cref="T:System.TimeOnly" /> instance..</param>
    /// <returns>The elapsed time between <paramref name="t1" /> and <paramref name="t2" />.</returns>
    public static TimeSpan operator -(TimeOnly t1, TimeOnly t2) => new TimeSpan((t1._ticks - t2._ticks + 864000000000L) % 864000000000L);

    /// <summary>Constructs a <see cref="T:System.TimeOnly" /> object from a time span representing the time elapsed since midnight.</summary>
    /// <param name="timeSpan">The time interval measured since midnight. This value has to be positive and not exceeding the time of the day.</param>
    /// <returns>A <see cref="T:System.TimeOnly" /> object representing the time elapsed since midnight using the specified time span value.</returns>
    public static TimeOnly FromTimeSpan(TimeSpan timeSpan) => new TimeOnly(timeSpan._ticks);

    /// <summary>Constructs a <see cref="T:System.TimeOnly" /> object from a <see cref="T:System.DateTime" /> representing the time of the day in this <see cref="T:System.DateTime" /> object.</summary>
    /// <param name="dateTime">The <see cref="T:System.DateTime" /> object to extract the time of the day from.</param>
    /// <returns>A <see cref="T:System.TimeOnly" /> object representing time of the day specified in the <see cref="T:System.DateTime" /> object.</returns>
    public static TimeOnly FromDateTime(DateTime dateTime) => new TimeOnly(dateTime.TimeOfDay.Ticks);

    /// <summary>Convert the current <see cref="T:System.TimeOnly" /> instance to a <see cref="T:System.TimeSpan" /> object.</summary>
    /// <returns>A <see cref="T:System.TimeSpan" /> object spanning to the time specified in the current <see cref="T:System.TimeOnly" /> instance.</returns>
    public TimeSpan ToTimeSpan() => new TimeSpan(this._ticks);

    internal DateTime ToDateTime() => new DateTime(this._ticks);

    /// <summary>Compares the value of this instance to a specified <see cref="T:System.TimeOnly" /> value and indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.TimeOnly" /> value.</summary>
    /// <param name="value">The object to compare to the current instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and the value parameter.
    /// 
    /// - Less than zero if this instance is earlier than value.
    /// 
    /// - Zero if this instance is the same as value.
    /// 
    /// - Greater than zero if this instance is later than value.</returns>
    public int CompareTo(TimeOnly value) => this._ticks.CompareTo(value._ticks);

    /// <summary>Compares the value of this instance to a specified object that contains a specified <see cref="T:System.TimeOnly" /> value, and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.TimeOnly" /> value.</summary>
    /// <param name="value">A boxed object to compare, or <see langword="null" />.</param>
    /// <returns>A signed number indicating the relative values of this instance and the value parameter.
    /// Less than zero if this instance is earlier than value.
    /// Zero if this instance is the same as value.
    /// Greater than zero if this instance is later than value.</returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (value is TimeOnly timeOnly)
        return this.CompareTo(timeOnly);
      throw new ArgumentException(SR.Arg_MustBeTimeOnly);
    }

    /// <summary>Returns a value indicating whether the value of this instance is equal to the value of the specified <see cref="T:System.TimeOnly" /> instance.</summary>
    /// <param name="value">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the value parameter equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(TimeOnly value) => this._ticks == value._ticks;

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="value">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if value is an instance of <see cref="T:System.TimeOnly" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? value) => value is TimeOnly timeOnly && this._ticks == timeOnly._ticks;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
      long ticks = this._ticks;
      return (int) ticks ^ (int) (ticks >> 32);
    }

    /// <summary>Converts a memory span that contains string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent by using culture-specific format information and a formatting style.</summary>
    /// <param name="s">The memory span that contains the time to parse.</param>
    /// <param name="provider">The culture-specific format information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly Parse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider = null,
      DateTimeStyles style = DateTimeStyles.None)
    {
      TimeOnly result1;
      ParseFailureKind result2 = TimeOnly.TryParseInternal(s, provider, style, out result1);
      if (result2 != ParseFailureKind.None)
        TimeOnly.ThrowOnError(result2, s);
      return result1;
    }

    /// <summary>Converts the specified span representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the time to convert.</param>
    /// <param name="format">The format specifier that defines the required format of <paramref name="s" />.</param>
    /// <param name="provider">The culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly ParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider? provider = null,
      DateTimeStyles style = DateTimeStyles.None)
    {
      TimeOnly result;
      ParseFailureKind exactInternal = TimeOnly.TryParseExactInternal(s, format, provider, style, out result);
      if (exactInternal != ParseFailureKind.None)
        TimeOnly.ThrowOnError(exactInternal, s);
      return result;
    }

    /// <summary>Converts the specified span to its <see cref="T:System.TimeOnly" /> equivalent using the specified array of formats.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly ParseExact(ReadOnlySpan<char> s, string[] formats) => TimeOnly.ParseExact(s, formats, (IFormatProvider) null);

    /// <summary>Converts the specified span representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified array of formats, culture-specific format information, and style.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">The culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly ParseExact(
      ReadOnlySpan<char> s,
      string[] formats,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      TimeOnly result;
      ParseFailureKind exactInternal = TimeOnly.TryParseExactInternal(s, formats, provider, style, out result);
      if (exactInternal != ParseFailureKind.None)
        TimeOnly.ThrowOnError(exactInternal, s);
      return result;
    }

    /// <summary>Converts the string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent by using the conventions of the current culture.</summary>
    /// <param name="s">The string to parse.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly Parse(string s) => TimeOnly.Parse(s, (IFormatProvider) null, DateTimeStyles.None);

    /// <summary>Converts the string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent by using culture-specific format information and a formatting style.</summary>
    /// <param name="s">The string containing the time to parse.</param>
    /// <param name="provider">The culture-specific format information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that indicates the style elements that can be present in s for the parse operation to succeed, and that defines how to interpret the parsed date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A<see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly Parse(string s, IFormatProvider? provider, DateTimeStyles style = DateTimeStyles.None)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return TimeOnly.Parse(s.AsSpan(), provider, style);
    }

    /// <summary>Converts the specified string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified format.
    /// The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A string containing a time to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly ParseExact(string s, string format) => TimeOnly.ParseExact(s, format, (IFormatProvider) null);

    /// <summary>Converts the specified string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A string containing the time to convert.</param>
    /// <param name="format">The format specifier that defines the required format of <paramref name="s" />.</param>
    /// <param name="provider">The culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that provides additional information about <paramref name="s" />, about style elements that may be present in <paramref name="s" />, or about the conversion from <paramref name="s" /> to a <see cref="T:System.TimeOnly" /> value. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly ParseExact(
      string s,
      string format,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if (format == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
      return TimeOnly.ParseExact(s.AsSpan(), format.AsSpan(), provider, style);
    }

    /// <summary>Converts the specified span to a <see cref="T:System.TimeOnly" /> equivalent using the specified array of formats.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly ParseExact(string s, string[] formats) => TimeOnly.ParseExact(s, formats, (IFormatProvider) null);

    /// <summary>Converts the specified string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified array of formats, culture-specific format information, and style.
    /// The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A string containing the time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">The culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a time.</exception>
    /// <returns>A <see cref="T:System.TimeOnly" /> instance.</returns>
    public static TimeOnly ParseExact(
      string s,
      string[] formats,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return TimeOnly.ParseExact(s.AsSpan(), formats, provider, style);
    }

    /// <summary>Converts the specified span representation of a time to its TimeOnly equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing the time to convert.</param>
    /// <param name="result">When this method returns, contains the TimeOnly value equivalent to the time contained in s, if the conversion succeeded, or MinValue if the conversion failed. The conversion fails if the s parameter is empty string, or does not contain a valid string representation of a time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the s parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out TimeOnly result) => TimeOnly.TryParse(s, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified span representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified array of formats, culture-specific format information and style, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the characters that represent a time to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a date. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if<paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      return TimeOnly.TryParseInternal(s, provider, style, out result) == ParseFailureKind.None;
    }


    #nullable disable
    private static ParseFailureKind TryParseInternal(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != DateTimeStyles.None)
      {
        result = new TimeOnly();
        return ParseFailureKind.FormatWithParameter;
      }
      DateTimeResult result1 = new DateTimeResult();
      result1.Init(s);
      if (!DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), style, ref result1))
      {
        result = new TimeOnly();
        return ParseFailureKind.FormatWithOriginalDateTime;
      }
      if ((result1.flags & (ParseFlags.HaveYear | ParseFlags.HaveMonth | ParseFlags.HaveDay | ParseFlags.HaveDate | ParseFlags.TimeZoneUsed | ParseFlags.TimeZoneUtc | ParseFlags.ParsedMonthName | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern)) != (ParseFlags) 0)
      {
        result = new TimeOnly();
        return ParseFailureKind.WrongParts;
      }
      result = new TimeOnly(result1.parsedDate.TimeOfDay.Ticks);
      return ParseFailureKind.None;
    }


    #nullable enable
    /// <summary>Converts the specified span representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified format and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the time to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a time that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      out TimeOnly result)
    {
      return TimeOnly.TryParseExact(s, format, (IFormatProvider) null, DateTimeStyles.None, out result);
    }

    /// <summary>Converts the specified span representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the time to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a time that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider? provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      return TimeOnly.TryParseExactInternal(s, format, provider, style, out result) == ParseFailureKind.None;
    }


    #nullable disable
    private static ParseFailureKind TryParseExactInternal(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != DateTimeStyles.None)
      {
        result = new TimeOnly();
        return ParseFailureKind.FormatWithParameter;
      }
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'O':
          case 'o':
            format = (ReadOnlySpan<char>) "HH':'mm':'ss'.'fffffff";
            provider = (IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat;
            break;
          case 'R':
          case 'r':
            format = (ReadOnlySpan<char>) "HH':'mm':'ss";
            provider = (IFormatProvider) CultureInfo.InvariantCulture.DateTimeFormat;
            break;
        }
      }
      DateTimeResult result1 = new DateTimeResult();
      result1.Init(s);
      if (!DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, ref result1))
      {
        result = new TimeOnly();
        return ParseFailureKind.FormatWithOriginalDateTime;
      }
      if ((result1.flags & (ParseFlags.HaveYear | ParseFlags.HaveMonth | ParseFlags.HaveDay | ParseFlags.HaveDate | ParseFlags.TimeZoneUsed | ParseFlags.TimeZoneUtc | ParseFlags.ParsedMonthName | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern)) != (ParseFlags) 0)
      {
        result = new TimeOnly();
        return ParseFailureKind.WrongParts;
      }
      result = new TimeOnly(result1.parsedDate.TimeOfDay.Ticks);
      return ParseFailureKind.None;
    }


    #nullable enable
    /// <summary>Converts the specified char span of a time to its <see cref="T:System.TimeOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The span containing the time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is an empty string, or does not contain a valid string representation of a time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(ReadOnlySpan<char> s, [NotNullWhen(true)] string?[]? formats, out TimeOnly result) => TimeOnly.TryParseExact(s, formats, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified char span of a time to its <see cref="T:System.TimeOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The span containing the time to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that defines how to interpret the parsed time. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      return TimeOnly.TryParseExactInternal(s, formats, provider, style, out result) == ParseFailureKind.None;
    }


    #nullable disable
    private static ParseFailureKind TryParseExactInternal(
      ReadOnlySpan<char> s,
      string[] formats,
      IFormatProvider provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      if ((style & ~DateTimeStyles.AllowWhiteSpaces) != DateTimeStyles.None || formats == null)
      {
        result = new TimeOnly();
        return ParseFailureKind.FormatWithParameter;
      }
      DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(provider);
      for (int index = 0; index < formats.Length; ++index)
      {
        DateTimeFormatInfo dtfi = instance;
        string format = formats[index];
        if (string.IsNullOrEmpty(format))
        {
          result = new TimeOnly();
          return ParseFailureKind.FormatWithFormatSpecifier;
        }
        if (format.Length == 1)
        {
          switch (format[0])
          {
            case 'O':
            case 'o':
              format = "HH':'mm':'ss'.'fffffff";
              dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
              break;
            case 'R':
            case 'r':
              format = "HH':'mm':'ss";
              dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
              break;
          }
        }
        DateTimeResult result1 = new DateTimeResult();
        result1.Init(s);
        if (DateTimeParse.TryParseExact(s, (ReadOnlySpan<char>) format, dtfi, style, ref result1) && (result1.flags & (ParseFlags.HaveYear | ParseFlags.HaveMonth | ParseFlags.HaveDay | ParseFlags.HaveDate | ParseFlags.TimeZoneUsed | ParseFlags.TimeZoneUtc | ParseFlags.ParsedMonthName | ParseFlags.CaptureOffset | ParseFlags.UtcSortPattern)) == (ParseFlags) 0)
        {
          result = new TimeOnly(result1.parsedDate.TimeOfDay.Ticks);
          return ParseFailureKind.None;
        }
      }
      result = new TimeOnly();
      return ParseFailureKind.FormatWithOriginalDateTime;
    }


    #nullable enable
    /// <summary>Converts the specified string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the time to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out TimeOnly result) => TimeOnly.TryParse(s, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified array of formats, culture-specific format information and style, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the time to convert.</param>
    /// <param name="provider">The culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      IFormatProvider? provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      if (s != null)
        return TimeOnly.TryParse(s.AsSpan(), provider, style, out result);
      result = new TimeOnly();
      return false;
    }

    /// <summary>Converts the specified string representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified format and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing the time to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a time that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)] string? format, out TimeOnly result) => TimeOnly.TryParseExact(s, format, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified span representation of a time to its <see cref="T:System.TimeOnly" /> equivalent using the specified format, culture-specific format information, and style.
    /// The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing a time to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a time that correspond to the pattern specified in format. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? s,
      [NotNullWhen(true)] string? format,
      IFormatProvider? provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      if (s != null && format != null)
        return TimeOnly.TryParseExact(s.AsSpan(), format.AsSpan(), provider, style, out result);
      result = new TimeOnly();
      return false;
    }

    /// <summary>Converts the specified string of a time to its <see cref="T:System.TimeOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The string containing the time to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact([NotNullWhen(true)] string? s, [NotNullWhen(true)] string?[]? formats, out TimeOnly result) => TimeOnly.TryParseExact(s, formats, (IFormatProvider) null, DateTimeStyles.None, out result);

    /// <summary>Converts the specified string of a time to its <see cref="T:System.TimeOnly" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The string containing the time to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that defines how to interpret the parsed date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.TimeOnly" /> value equivalent to the time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.TimeOnly.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> is an empty string, or does not contain a valid string representation of a time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? s,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? provider,
      DateTimeStyles style,
      out TimeOnly result)
    {
      if (s != null)
        return TimeOnly.TryParseExact(s.AsSpan(), formats, provider, style, out result);
      result = new TimeOnly();
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
          throw new FormatException(SR.Format(SR.Format_BadTimeOnly, (object) s.ToString()));
        case ParseFailureKind.FormatWithFormatSpecifier:
          throw new FormatException(SR.Argument_BadFormatSpecifier);
        default:
          throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, (object) s.ToString(), (object) nameof (TimeOnly)));
      }
    }


    #nullable enable
    /// <summary>Converts the value of the current <see cref="T:System.TimeOnly" /> instance to its equivalent long date string representation.</summary>
    /// <returns>The long time string representation of the current instance.</returns>
    public string ToLongTimeString() => this.ToString("T");

    /// <summary>Converts the current <see cref="T:System.TimeOnly" /> instance to its equivalent short time string representation.</summary>
    /// <returns>The short time string representation of the current instance.</returns>
    public string ToShortTimeString() => this.ToString();

    /// <summary>Converts the current <see cref="T:System.TimeOnly" /> instance to its equivalent short time string representation using the formatting conventions of the current culture.</summary>
    /// <returns>The short time string representation of the current instance.</returns>
    public override string ToString() => this.ToString("t");

    /// <summary>Converts the current <see cref="T:System.TimeOnly" /> instance to its equivalent string representation using the specified format and the formatting conventions of the current culture.</summary>
    /// <param name="format">A standard or custom time format string.</param>
    /// <returns>A string representation of the current instance with the specified format and the formatting conventions of the current culture.</returns>
    public string ToString(string? format) => this.ToString(format, (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.TimeOnly" /> instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">The culture-specific formatting information.</param>
    /// <returns>A string representation of the current instance as specified by the provider.</returns>
    public string ToString(IFormatProvider? provider) => this.ToString("t", provider);

    /// <summary>Converts the value of the current <see cref="T:System.TimeOnly" /> instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="format">A standard or custom time format string.</param>
    /// <param name="provider">The culture-specific formatting information.</param>
    /// <returns>A string representation of value of the current instance.</returns>
    public string ToString(string? format, IFormatProvider? provider)
    {
      switch (format)
      {
        case "":
        case null:
          format = "t";
          break;
      }
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'O':
          case 'o':
            return string.Create<TimeOnly>(16, this, (SpanAction<char, TimeOnly>) ((destination, value) => DateTimeFormat.TryFormatTimeOnlyO(value.Hour, value.Minute, value.Second, value._ticks % 10000000L, destination)));
          case 'R':
          case 'r':
            return string.Create<TimeOnly>(8, this, (SpanAction<char, TimeOnly>) ((destination, value) => DateTimeFormat.TryFormatTimeOnlyR(value.Hour, value.Minute, value.Second, destination)));
          case 'T':
          case 't':
            return DateTimeFormat.Format(this.ToDateTime(), format, provider);
          default:
            throw new FormatException(SR.Format_InvalidString);
        }
      }
      else
      {
        DateTimeFormat.IsValidCustomTimeFormat(format.AsSpan(), true);
        return DateTimeFormat.Format(this.ToDateTime(), format, provider);
      }
    }

    /// <summary>Tries to format the value of the current TimeOnly instance into the provided span of characters.</summary>
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
        format = (ReadOnlySpan<char>) "t";
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'O':
          case 'o':
            if (!DateTimeFormat.TryFormatTimeOnlyO(this.Hour, this.Minute, this.Second, this._ticks % 10000000L, destination))
            {
              charsWritten = 0;
              return false;
            }
            charsWritten = 16;
            return true;
          case 'R':
          case 'r':
            if (!DateTimeFormat.TryFormatTimeOnlyR(this.Hour, this.Minute, this.Second, destination))
            {
              charsWritten = 0;
              return false;
            }
            charsWritten = 8;
            return true;
          case 'T':
          case 't':
            return DateTimeFormat.TryFormat(this.ToDateTime(), destination, out charsWritten, format, provider);
          default:
            throw new FormatException(SR.Argument_BadFormatSpecifier);
        }
      }
      else
      {
        if (!DateTimeFormat.IsValidCustomTimeFormat(format, false))
          throw new FormatException(SR.Format(SR.Format_DateTimeOnlyContainsNoneDateParts, (object) format.ToString(), (object) nameof (TimeOnly)));
        return DateTimeFormat.TryFormat(this.ToDateTime(), destination, out charsWritten, format, provider);
      }
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeOnly, TimeOnly>.op_LessThan(
      TimeOnly left,
      TimeOnly right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeOnly, TimeOnly>.op_LessThanOrEqual(
      TimeOnly left,
      TimeOnly right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeOnly, TimeOnly>.op_GreaterThan(
      TimeOnly left,
      TimeOnly right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeOnly, TimeOnly>.op_GreaterThanOrEqual(
      TimeOnly left,
      TimeOnly right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<TimeOnly, TimeOnly>.op_Equality(
      TimeOnly left,
      TimeOnly right)
    {
      return left == right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<TimeOnly, TimeOnly>.op_Inequality(
      TimeOnly left,
      TimeOnly right)
    {
      return left != right;
    }


    #nullable disable
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeOnly IParseable<TimeOnly>.Parse(
      string s,
      IFormatProvider provider)
    {
      return TimeOnly.Parse(s, provider, DateTimeStyles.None);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<TimeOnly>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out TimeOnly result)
    {
      return TimeOnly.TryParse(s, provider, DateTimeStyles.None, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeOnly ISpanParseable<TimeOnly>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return TimeOnly.Parse(s, provider, DateTimeStyles.None);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<TimeOnly>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out TimeOnly result)
    {
      return TimeOnly.TryParse(s, provider, DateTimeStyles.None, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan ISubtractionOperators<TimeOnly, TimeOnly, TimeSpan>.op_Subtraction(
      TimeOnly left,
      TimeOnly right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeOnly IMinMaxValue<TimeOnly>.MinValue => TimeOnly.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeOnly IMinMaxValue<TimeOnly>.MaxValue => TimeOnly.MaxValue;
  }
}
