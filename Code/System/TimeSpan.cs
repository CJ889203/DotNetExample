// Decompiled with JetBrains decompiler
// Type: System.TimeSpan
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Represents a time interval.</summary>
  [Serializable]
  public readonly struct TimeSpan : 
    IComparable,
    IComparable<TimeSpan>,
    IEquatable<TimeSpan>,
    ISpanFormattable,
    IFormattable,
    IAdditionOperators<TimeSpan, TimeSpan, TimeSpan>,
    IAdditiveIdentity<TimeSpan, TimeSpan>,
    IComparisonOperators<TimeSpan, TimeSpan>,
    IEqualityOperators<TimeSpan, TimeSpan>,
    IDivisionOperators<TimeSpan, double, TimeSpan>,
    IDivisionOperators<TimeSpan, TimeSpan, double>,
    IMinMaxValue<TimeSpan>,
    IMultiplyOperators<TimeSpan, double, TimeSpan>,
    IMultiplicativeIdentity<TimeSpan, double>,
    ISpanParseable<TimeSpan>,
    IParseable<TimeSpan>,
    ISubtractionOperators<TimeSpan, TimeSpan, TimeSpan>,
    IUnaryNegationOperators<TimeSpan, TimeSpan>,
    IUnaryPlusOperators<TimeSpan, TimeSpan>
  {
    /// <summary>Represents the number of ticks in 1 millisecond. This field is constant.</summary>
    public const long TicksPerMillisecond = 10000;
    /// <summary>Represents the number of ticks in 1 second.</summary>
    public const long TicksPerSecond = 10000000;
    /// <summary>Represents the number of ticks in 1 minute. This field is constant.</summary>
    public const long TicksPerMinute = 600000000;
    /// <summary>Represents the number of ticks in 1 hour. This field is constant.</summary>
    public const long TicksPerHour = 36000000000;
    /// <summary>Represents the number of ticks in 1 day. This field is constant.</summary>
    public const long TicksPerDay = 864000000000;
    /// <summary>Represents the zero <see cref="T:System.TimeSpan" /> value. This field is read-only.</summary>
    public static readonly TimeSpan Zero = new TimeSpan(0L);
    /// <summary>Represents the maximum <see cref="T:System.TimeSpan" /> value. This field is read-only.</summary>
    public static readonly TimeSpan MaxValue = new TimeSpan(long.MaxValue);
    /// <summary>Represents the minimum <see cref="T:System.TimeSpan" /> value. This field is read-only.</summary>
    public static readonly TimeSpan MinValue = new TimeSpan(long.MinValue);
    internal readonly long _ticks;

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeSpan" /> structure to the specified number of ticks.</summary>
    /// <param name="ticks">A time period expressed in 100-nanosecond units.</param>
    public TimeSpan(long ticks) => this._ticks = ticks;

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeSpan" /> structure to a specified number of hours, minutes, and seconds.</summary>
    /// <param name="hours">Number of hours.</param>
    /// <param name="minutes">Number of minutes.</param>
    /// <param name="seconds">Number of seconds.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The parameters specify a <see cref="T:System.TimeSpan" /> value less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.</exception>
    public TimeSpan(int hours, int minutes, int seconds) => this._ticks = TimeSpan.TimeToTicks(hours, minutes, seconds);

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeSpan" /> structure to a specified number of days, hours, minutes, and seconds.</summary>
    /// <param name="days">Number of days.</param>
    /// <param name="hours">Number of hours.</param>
    /// <param name="minutes">Number of minutes.</param>
    /// <param name="seconds">Number of seconds.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The parameters specify a <see cref="T:System.TimeSpan" /> value less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.</exception>
    public TimeSpan(int days, int hours, int minutes, int seconds)
      : this(days, hours, minutes, seconds, 0)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.TimeSpan" /> structure to a specified number of days, hours, minutes, seconds, and milliseconds.</summary>
    /// <param name="days">Number of days.</param>
    /// <param name="hours">Number of hours.</param>
    /// <param name="minutes">Number of minutes.</param>
    /// <param name="seconds">Number of seconds.</param>
    /// <param name="milliseconds">Number of milliseconds.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The parameters specify a <see cref="T:System.TimeSpan" /> value less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.</exception>
    public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
    {
      long num = ((long) days * 3600L * 24L + (long) hours * 3600L + (long) minutes * 60L + (long) seconds) * 1000L + (long) milliseconds;
      if (num > 922337203685477L || num < -922337203685477L)
        throw new ArgumentOutOfRangeException((string) null, SR.Overflow_TimeSpanTooLong);
      this._ticks = num * 10000L;
    }

    /// <summary>Gets the number of ticks that represent the value of the current <see cref="T:System.TimeSpan" /> structure.</summary>
    /// <returns>The number of ticks contained in this instance.</returns>
    public long Ticks => this._ticks;

    /// <summary>Gets the days component of the time interval represented by the current <see cref="T:System.TimeSpan" /> structure.</summary>
    /// <returns>The day component of this instance. The return value can be positive or negative.</returns>
    public int Days => (int) (this._ticks / 864000000000L);

    /// <summary>Gets the hours component of the time interval represented by the current <see cref="T:System.TimeSpan" /> structure.</summary>
    /// <returns>The hour component of the current <see cref="T:System.TimeSpan" /> structure. The return value ranges from -23 through 23.</returns>
    public int Hours => (int) (this._ticks / 36000000000L % 24L);

    /// <summary>Gets the milliseconds component of the time interval represented by the current <see cref="T:System.TimeSpan" /> structure.</summary>
    /// <returns>The millisecond component of the current <see cref="T:System.TimeSpan" /> structure. The return value ranges from -999 through 999.</returns>
    public int Milliseconds => (int) (this._ticks / 10000L % 1000L);

    /// <summary>Gets the minutes component of the time interval represented by the current <see cref="T:System.TimeSpan" /> structure.</summary>
    /// <returns>The minute component of the current <see cref="T:System.TimeSpan" /> structure. The return value ranges from -59 through 59.</returns>
    public int Minutes => (int) (this._ticks / 600000000L % 60L);

    /// <summary>Gets the seconds component of the time interval represented by the current <see cref="T:System.TimeSpan" /> structure.</summary>
    /// <returns>The second component of the current <see cref="T:System.TimeSpan" /> structure. The return value ranges from -59 through 59.</returns>
    public int Seconds => (int) (this._ticks / 10000000L % 60L);

    /// <summary>Gets the value of the current <see cref="T:System.TimeSpan" /> structure expressed in whole and fractional days.</summary>
    /// <returns>The total number of days represented by this instance.</returns>
    public double TotalDays => (double) this._ticks / 864000000000.0;

    /// <summary>Gets the value of the current <see cref="T:System.TimeSpan" /> structure expressed in whole and fractional hours.</summary>
    /// <returns>The total number of hours represented by this instance.</returns>
    public double TotalHours => (double) this._ticks / 36000000000.0;

    /// <summary>Gets the value of the current <see cref="T:System.TimeSpan" /> structure expressed in whole and fractional milliseconds.</summary>
    /// <returns>The total number of milliseconds represented by this instance.</returns>
    public double TotalMilliseconds
    {
      get
      {
        double num = (double) this._ticks / 10000.0;
        if (num > 922337203685477.0)
          return 922337203685477.0;
        return num < -922337203685477.0 ? -922337203685477.0 : num;
      }
    }

    /// <summary>Gets the value of the current <see cref="T:System.TimeSpan" /> structure expressed in whole and fractional minutes.</summary>
    /// <returns>The total number of minutes represented by this instance.</returns>
    public double TotalMinutes => (double) this._ticks / 600000000.0;

    /// <summary>Gets the value of the current <see cref="T:System.TimeSpan" /> structure expressed in whole and fractional seconds.</summary>
    /// <returns>The total number of seconds represented by this instance.</returns>
    public double TotalSeconds => (double) this._ticks / 10000000.0;

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object whose value is the sum of the specified <see cref="T:System.TimeSpan" /> object and this instance.</summary>
    /// <param name="ts">The time interval to add.</param>
    /// <exception cref="T:System.OverflowException">The resulting <see cref="T:System.TimeSpan" /> is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.</exception>
    /// <returns>A new object that represents the value of this instance plus the value of <paramref name="ts" />.</returns>
    public TimeSpan Add(TimeSpan ts)
    {
      long ticks = this._ticks + ts._ticks;
      if (this._ticks >> 63 == ts._ticks >> 63 && this._ticks >> 63 != ticks >> 63)
        throw new OverflowException(SR.Overflow_TimeSpanTooLong);
      return new TimeSpan(ticks);
    }

    /// <summary>Compares two <see cref="T:System.TimeSpan" /> values and returns an integer that indicates whether the first value is shorter than, equal to, or longer than the second value.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>One of the following values.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> -1</term><description><paramref name="t1" /> is shorter than <paramref name="t2" />.</description></item><item><term> 0</term><description><paramref name="t1" /> is equal to <paramref name="t2" />.</description></item><item><term> 1</term><description><paramref name="t1" /> is longer than <paramref name="t2" />.</description></item></list></returns>
    public static int Compare(TimeSpan t1, TimeSpan t2)
    {
      if (t1._ticks > t2._ticks)
        return 1;
      return t1._ticks < t2._ticks ? -1 : 0;
    }

    /// <summary>Compares this instance to a specified object and returns an integer that indicates whether this instance is shorter than, equal to, or longer than the specified object.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.TimeSpan" />.</exception>
    /// <returns>One of the following values.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> -1</term><description> This instance is shorter than <paramref name="value" />.</description></item><item><term> 0</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> 1</term><description> This instance is longer than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      long num = value is TimeSpan timeSpan ? timeSpan._ticks : throw new ArgumentException(SR.Arg_MustBeTimeSpan);
      if (this._ticks > num)
        return 1;
      return this._ticks < num ? -1 : 0;
    }

    /// <summary>Compares this instance to a specified <see cref="T:System.TimeSpan" /> object and returns an integer that indicates whether this instance is shorter than, equal to, or longer than the <see cref="T:System.TimeSpan" /> object.</summary>
    /// <param name="value">An object to compare to this instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance is shorter than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> A positive integer</term><description> This instance is longer than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(TimeSpan value)
    {
      long ticks = value._ticks;
      if (this._ticks > ticks)
        return 1;
      return this._ticks < ticks ? -1 : 0;
    }

    /// <summary>Returns a <see cref="T:System.TimeSpan" /> that represents a specified number of days, where the specification is accurate to the nearest millisecond.</summary>
    /// <param name="value">A number of days, accurate to the nearest millisecond.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.PositiveInfinity" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.NegativeInfinity" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />.</exception>
    /// <returns>An object that represents <paramref name="value" />.</returns>
    public static TimeSpan FromDays(double value) => TimeSpan.Interval(value, 864000000000.0);

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object whose value is the absolute value of the current <see cref="T:System.TimeSpan" /> object.</summary>
    /// <exception cref="T:System.OverflowException">The value of this instance is <see cref="F:System.TimeSpan.MinValue" />.</exception>
    /// <returns>A new object whose value is the absolute value of the current <see cref="T:System.TimeSpan" /> object.</returns>
    public TimeSpan Duration()
    {
      if (this.Ticks == TimeSpan.MinValue.Ticks)
        throw new OverflowException(SR.Overflow_Duration);
      return new TimeSpan(this._ticks >= 0L ? this._ticks : -this._ticks);
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="value">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is a <see cref="T:System.TimeSpan" /> object that represents the same time interval as the current <see cref="T:System.TimeSpan" /> structure; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? value) => value is TimeSpan timeSpan && this._ticks == timeSpan._ticks;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.TimeSpan" /> object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> represents the same time interval as this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(TimeSpan obj) => this._ticks == obj._ticks;

    /// <summary>Returns a value that indicates whether two specified instances of <see cref="T:System.TimeSpan" /> are equal.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the values of <paramref name="t1" /> and <paramref name="t2" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool Equals(TimeSpan t1, TimeSpan t2) => t1._ticks == t2._ticks;

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) this._ticks ^ (int) (this._ticks >> 32);

    /// <summary>Returns a <see cref="T:System.TimeSpan" /> that represents a specified number of hours, where the specification is accurate to the nearest millisecond.</summary>
    /// <param name="value">A number of hours accurate to the nearest millisecond.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.PositiveInfinity" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.NegativeInfinity" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />.</exception>
    /// <returns>An object that represents <paramref name="value" />.</returns>
    public static TimeSpan FromHours(double value) => TimeSpan.Interval(value, 36000000000.0);

    private static TimeSpan Interval(double value, double scale) => !double.IsNaN(value) ? TimeSpan.IntervalFromDoubleTicks(value * scale) : throw new ArgumentException(SR.Arg_CannotBeNaN);

    private static TimeSpan IntervalFromDoubleTicks(double ticks)
    {
      if (ticks > (double) long.MaxValue || ticks < (double) long.MinValue || double.IsNaN(ticks))
        throw new OverflowException(SR.Overflow_TimeSpanTooLong);
      return ticks == (double) long.MaxValue ? TimeSpan.MaxValue : new TimeSpan((long) ticks);
    }

    /// <summary>Returns a <see cref="T:System.TimeSpan" /> that represents a specified number of milliseconds.</summary>
    /// <param name="value">A number of milliseconds.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.PositiveInfinity" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.NegativeInfinity" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />.</exception>
    /// <returns>An object that represents <paramref name="value" />.</returns>
    public static TimeSpan FromMilliseconds(double value) => TimeSpan.Interval(value, 10000.0);

    /// <summary>Returns a <see cref="T:System.TimeSpan" /> that represents a specified number of minutes, where the specification is accurate to the nearest millisecond.</summary>
    /// <param name="value">A number of minutes, accurate to the nearest millisecond.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.PositiveInfinity" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.NegativeInfinity" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />.</exception>
    /// <returns>An object that represents <paramref name="value" />.</returns>
    public static TimeSpan FromMinutes(double value) => TimeSpan.Interval(value, 600000000.0);

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object whose value is the negated value of this instance.</summary>
    /// <exception cref="T:System.OverflowException">The negated value of this instance cannot be represented by a <see cref="T:System.TimeSpan" />; that is, the value of this instance is <see cref="F:System.TimeSpan.MinValue" />.</exception>
    /// <returns>A new object with the same numeric value as this instance, but with the opposite sign.</returns>
    public TimeSpan Negate()
    {
      if (this.Ticks == TimeSpan.MinValue.Ticks)
        throw new OverflowException(SR.Overflow_NegateTwosCompNum);
      return new TimeSpan(-this._ticks);
    }

    /// <summary>Returns a <see cref="T:System.TimeSpan" /> that represents a specified number of seconds, where the specification is accurate to the nearest millisecond.</summary>
    /// <param name="value">A number of seconds, accurate to the nearest millisecond.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.PositiveInfinity" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.NegativeInfinity" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />.</exception>
    /// <returns>An object that represents <paramref name="value" />.</returns>
    public static TimeSpan FromSeconds(double value) => TimeSpan.Interval(value, 10000000.0);

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object whose value is the difference between the specified <see cref="T:System.TimeSpan" /> object and this instance.</summary>
    /// <param name="ts">The time interval to be subtracted.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.</exception>
    /// <returns>A new time interval whose value is the result of the value of this instance minus the value of <paramref name="ts" />.</returns>
    public TimeSpan Subtract(TimeSpan ts)
    {
      long ticks = this._ticks - ts._ticks;
      if (this._ticks >> 63 != ts._ticks >> 63 && this._ticks >> 63 != ticks >> 63)
        throw new OverflowException(SR.Overflow_TimeSpanTooLong);
      return new TimeSpan(ticks);
    }

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object which value is the result of multiplication of this instance and the specified <paramref name="factor" />.</summary>
    /// <param name="factor">The value to be multiplied by.</param>
    /// <returns>A new object that represents the value of this instance multiplied by the value of <paramref name="factor" />.</returns>
    public TimeSpan Multiply(double factor) => this * factor;

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object which value is the result of division of this instance and the specified <paramref name="divisor" />.</summary>
    /// <param name="divisor">The value to be divided by.</param>
    /// <returns>A new object that represents the value of this instance divided by the value of <paramref name="divisor" />.</returns>
    public TimeSpan Divide(double divisor) => this / divisor;

    /// <summary>Returns a new <see cref="T:System.Double" /> value which is the result of division of this instance and the specified <paramref name="ts" />.</summary>
    /// <param name="ts">The value to be divided by.</param>
    /// <returns>A new value that represents result of division of this instance by the value of the <paramref name="ts" />.</returns>
    public double Divide(TimeSpan ts) => this / ts;

    /// <summary>Returns a <see cref="T:System.TimeSpan" /> that represents a specified time, where the specification is in units of ticks.</summary>
    /// <param name="value">A number of ticks that represent a time.</param>
    /// <returns>An object that represents <paramref name="value" />.</returns>
    public static TimeSpan FromTicks(long value) => new TimeSpan(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static long TimeToTicks(int hour, int minute, int second)
    {
      long num = (long) hour * 3600L + (long) minute * 60L + (long) second;
      if (num > 922337203685L || num < -922337203685L)
        ThrowHelper.ThrowArgumentOutOfRange_TimeSpanTooLong();
      return num * 10000000L;
    }


    #nullable disable
    private static void ValidateStyles(TimeSpanStyles style, string parameterName)
    {
      if (style != TimeSpanStyles.None && style != TimeSpanStyles.AssumeNegative)
        throw new ArgumentException(SR.Argument_InvalidTimeSpanStyles, parameterName);
    }


    #nullable enable
    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent.</summary>
    /// <param name="s">A string that specifies the time interval to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> has an invalid format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="s" /> represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// At least one of the days, hours, minutes, or seconds components is outside its valid range.</exception>
    /// <returns>A time interval that corresponds to <paramref name="s" />.</returns>
    public static TimeSpan Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
      return TimeSpanParse.Parse((ReadOnlySpan<char>) s, (IFormatProvider) null);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified culture-specific format information.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input" /> has an invalid format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="input" /> represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// At least one of the days, hours, minutes, or seconds components in <paramref name="input" /> is outside its valid range.</exception>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="formatProvider" />.</returns>
    public static TimeSpan Parse(string input, IFormatProvider? formatProvider)
    {
      if (input == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
      return TimeSpanParse.Parse((ReadOnlySpan<char>) input, formatProvider);
    }

    /// <summary>Converts the span representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified culture-specific format information.</summary>
    /// <param name="input">A span containing the characters that represent the time interval to convert.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="formatProvider" />.</returns>
    public static TimeSpan Parse(ReadOnlySpan<char> input, IFormatProvider? formatProvider = null) => TimeSpanParse.Parse(input, formatProvider);

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format and culture-specific format information. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="format">A standard or custom format string that defines the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input" /> has an invalid format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="input" /> represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// At least one of the days, hours, minutes, or seconds components in <paramref name="input" /> is outside its valid range.</exception>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="format" /> and <paramref name="formatProvider" />.</returns>
    public static TimeSpan ParseExact(
      string input,
      string format,
      IFormatProvider? formatProvider)
    {
      if (input == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
      if (format == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
      return TimeSpanParse.ParseExact((ReadOnlySpan<char>) input, (ReadOnlySpan<char>) format, formatProvider, TimeSpanStyles.None);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified array of format strings and culture-specific format information. The format of the string representation must match one of the specified formats exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="formats">An array of standard or custom format strings that defines the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input" /> has an invalid format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="input" /> represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// At least one of the days, hours, minutes, or seconds components in <paramref name="input" /> is outside its valid range.</exception>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="formats" /> and <paramref name="formatProvider" />.</returns>
    public static TimeSpan ParseExact(
      string input,
      string[] formats,
      IFormatProvider? formatProvider)
    {
      if (input == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
      return TimeSpanParse.ParseExactMultiple((ReadOnlySpan<char>) input, formats, formatProvider, TimeSpanStyles.None);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format, culture-specific format information, and styles. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="format">A standard or custom format string that defines the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <param name="styles">A bitwise combination of enumeration values that defines the style elements that may be present in <paramref name="input" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="styles" /> is an invalid <see cref="T:System.Globalization.TimeSpanStyles" /> value.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input" /> has an invalid format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="input" /> represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// At least one of the days, hours, minutes, or seconds components in <paramref name="input" /> is outside its valid range.</exception>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="format" />, <paramref name="formatProvider" />, and <paramref name="styles" />.</returns>
    public static TimeSpan ParseExact(
      string input,
      string format,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      if (input == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
      if (format == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
      return TimeSpanParse.ParseExact((ReadOnlySpan<char>) input, (ReadOnlySpan<char>) format, formatProvider, styles);
    }

    /// <summary>Converts the char span of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format and culture-specific format information. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="input">A span that specifies the time interval to convert.</param>
    /// <param name="format">A standard or custom format string that defines the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <param name="styles">A bitwise combination of enumeration values that defines the style elements that may be present in <paramref name="input" />.</param>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="format" /> and <paramref name="formatProvider" />.</returns>
    public static TimeSpan ParseExact(
      ReadOnlySpan<char> input,
      ReadOnlySpan<char> format,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles = TimeSpanStyles.None)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      return TimeSpanParse.ParseExact(input, format, formatProvider, styles);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified formats, culture-specific format information, and styles. The format of the string representation must match one of the specified formats exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="formats">An array of standard or custom format strings that define the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <param name="styles">A bitwise combination of enumeration values that defines the style elements that may be present in input.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="styles" /> is an invalid <see cref="T:System.Globalization.TimeSpanStyles" /> value.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input" /> has an invalid format.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="input" /> represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// At least one of the days, hours, minutes, or seconds components in <paramref name="input" /> is outside its valid range.</exception>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="formats" />, <paramref name="formatProvider" />, and <paramref name="styles" />.</returns>
    public static TimeSpan ParseExact(
      string input,
      string[] formats,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      if (input == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.input);
      return TimeSpanParse.ParseExactMultiple((ReadOnlySpan<char>) input, formats, formatProvider, styles);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified formats, culture-specific format information, and styles. The format of the string representation must match one of the specified formats exactly.</summary>
    /// <param name="input">A span that specifies the time interval to convert.</param>
    /// <param name="formats">An array of standard or custom format strings that define the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <param name="styles">A bitwise combination of enumeration values that defines the style elements that may be present in input.</param>
    /// <returns>A time interval that corresponds to <paramref name="input" />, as specified by <paramref name="formats" />, <paramref name="formatProvider" />, and <paramref name="styles" />.</returns>
    public static TimeSpan ParseExact(
      ReadOnlySpan<char> input,
      string[] formats,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles = TimeSpanStyles.None)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      return TimeSpanParse.ParseExactMultiple(input, formats, formatProvider, styles);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string that specifies the time interval to convert.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="s" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />. This operation returns <see langword="false" /> if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, has an invalid format, represents a time interval that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />, or has at least one days, hours, minutes, or seconds component outside its valid range.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out TimeSpan result)
    {
      if (s != null)
        return TimeSpanParse.TryParse((ReadOnlySpan<char>) s, (IFormatProvider) null, out result);
      result = new TimeSpan();
      return false;
    }

    /// <summary>Converts the span representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing the time interval to convert.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="s" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />. This operation returns <see langword="false" /> if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, has an invalid format, represents a time interval that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />, or has at least one days, hours, minutes, or seconds component outside its valid range.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out TimeSpan result) => TimeSpanParse.TryParse(s, (IFormatProvider) null, out result);

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified culture-specific formatting information, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />. This operation returns <see langword="false" /> if the <paramref name="input" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, has an invalid format, represents a time interval that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />, or has at least one days, hours, minutes, or seconds component outside its valid range.</returns>
    public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? formatProvider, out TimeSpan result)
    {
      if (input != null)
        return TimeSpanParse.TryParse((ReadOnlySpan<char>) input, formatProvider, out result);
      result = new TimeSpan();
      return false;
    }

    /// <summary>Converts the span representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified culture-specific formatting information, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="input">A span containing the characters representing the time interval to convert.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />. This operation returns <see langword="false" /> if the <paramref name="input" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, has an invalid format, represents a time interval that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />, or has at least one days, hours, minutes, or seconds component outside its valid range.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> input,
      IFormatProvider? formatProvider,
      out TimeSpan result)
    {
      return TimeSpanParse.TryParse(input, formatProvider, out result);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format and culture-specific format information. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="format">A standard or custom format string that defines the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? input,
      [NotNullWhen(true)] string? format,
      IFormatProvider? formatProvider,
      out TimeSpan result)
    {
      if (input != null && format != null)
        return TimeSpanParse.TryParseExact((ReadOnlySpan<char>) input, (ReadOnlySpan<char>) format, formatProvider, TimeSpanStyles.None, out result);
      result = new TimeSpan();
      return false;
    }

    /// <summary>Converts the specified span representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format and culture-specific format information. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="input">A span containing the characters that represent a time interval to convert.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> input,
      ReadOnlySpan<char> format,
      IFormatProvider? formatProvider,
      out TimeSpan result)
    {
      return TimeSpanParse.TryParseExact(input, format, formatProvider, TimeSpanStyles.None, out result);
    }

    /// <summary>Converts the specified string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified formats and culture-specific format information. The format of the string representation must match one of the specified formats exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="formats">An array of standard or custom format strings that define the acceptable formats of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? input,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? formatProvider,
      out TimeSpan result)
    {
      if (input != null)
        return TimeSpanParse.TryParseExactMultiple((ReadOnlySpan<char>) input, formats, formatProvider, TimeSpanStyles.None, out result);
      result = new TimeSpan();
      return false;
    }

    /// <summary>Converts the specified span representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified formats and culture-specific format information. The format of the string representation must match one of the specified formats exactly.</summary>
    /// <param name="input">A span containing the characters that represent a time interval to convert.</param>
    /// <param name="formats">An array of standard or custom format strings that define the acceptable formats of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> input,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? formatProvider,
      out TimeSpan result)
    {
      return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, TimeSpanStyles.None, out result);
    }

    /// <summary>Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format, culture-specific format information and styles. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="format">A standard or custom format string that defines the required format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <param name="styles">One or more enumeration values that indicate the style of <paramref name="input" />.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? input,
      [NotNullWhen(true)] string? format,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles,
      out TimeSpan result)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      if (input != null && format != null)
        return TimeSpanParse.TryParseExact((ReadOnlySpan<char>) input, (ReadOnlySpan<char>) format, formatProvider, styles, out result);
      result = new TimeSpan();
      return false;
    }

    /// <summary>Converts the specified span representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format, culture-specific format information, and styles, and returns a value that indicates whether the conversion succeeded. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="input">A span containing the characters that represent a time interval to convert.</param>
    /// <param name="format">A span containing the charactes that represent a standard or custom format string that defines the acceptable format of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="styles">One or more enumeration values that indicate the style of <paramref name="input" />.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> input,
      ReadOnlySpan<char> format,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles,
      out TimeSpan result)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      return TimeSpanParse.TryParseExact(input, format, formatProvider, styles, out result);
    }

    /// <summary>Converts the specified string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified formats, culture-specific format information and styles. The format of the string representation must match one of the specified formats exactly.</summary>
    /// <param name="input">A string that specifies the time interval to convert.</param>
    /// <param name="formats">An array of standard or custom format strings that define the acceptable formats of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="styles">One or more enumeration values that indicate the style of <paramref name="input" />.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? input,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles,
      out TimeSpan result)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      if (input != null)
        return TimeSpanParse.TryParseExactMultiple((ReadOnlySpan<char>) input, formats, formatProvider, styles, out result);
      result = new TimeSpan();
      return false;
    }

    /// <summary>Converts the specified span representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified formats, culture-specific format information and styles. The format of the string representation must match one of the specified formats exactly.</summary>
    /// <param name="input">A span containing the characters that represent a time interval to convert.</param>
    /// <param name="formats">An array of standard or custom format strings that define the acceptable formats of <paramref name="input" />.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <param name="styles">One or more enumeration values that indicate the style of <paramref name="input" />.</param>
    /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="input" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="input" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> input,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? formatProvider,
      TimeSpanStyles styles,
      out TimeSpan result)
    {
      TimeSpan.ValidateStyles(styles, nameof (styles));
      return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, styles, out result);
    }

    /// <summary>Converts the value of the current <see cref="T:System.TimeSpan" /> object to its equivalent string representation.</summary>
    /// <returns>The string representation of the current <see cref="T:System.TimeSpan" /> value.</returns>
    public override string ToString() => TimeSpanFormat.FormatC(this);

    /// <summary>Converts the value of the current <see cref="T:System.TimeSpan" /> object to its equivalent string representation by using the specified format.</summary>
    /// <param name="format">A standard or custom <see cref="T:System.TimeSpan" /> format string.</param>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is not recognized or is not supported.</exception>
    /// <returns>The string representation of the current <see cref="T:System.TimeSpan" /> value in the format specified by the <paramref name="format" /> parameter.</returns>
    public string ToString(string? format) => TimeSpanFormat.Format(this, format, (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.TimeSpan" /> object to its equivalent string representation by using the specified format and culture-specific formatting information.</summary>
    /// <param name="format">A standard or custom <see cref="T:System.TimeSpan" /> format string.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter is not recognized or is not supported.</exception>
    /// <returns>The string representation of the current <see cref="T:System.TimeSpan" /> value, as specified by <paramref name="format" /> and <paramref name="formatProvider" />.</returns>
    public string ToString(string? format, IFormatProvider? formatProvider) => TimeSpanFormat.Format(this, format, formatProvider);

    /// <summary>Tries to format the value of the current timespan number instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the charactes that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="formatProvider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? formatProvider = null)
    {
      return TimeSpanFormat.TryFormat(this, destination, out charsWritten, format, formatProvider);
    }

    /// <summary>Returns a <see cref="T:System.TimeSpan" /> whose value is the negated value of the specified instance.</summary>
    /// <param name="t">The time interval to be negated.</param>
    /// <exception cref="T:System.OverflowException">The negated value of this instance cannot be represented by a <see cref="T:System.TimeSpan" />; that is, the value of this instance is <see cref="F:System.TimeSpan.MinValue" />.</exception>
    /// <returns>An object that has the same numeric value as this instance, but the opposite sign.</returns>
    public static TimeSpan operator -(TimeSpan t)
    {
      if (t._ticks == TimeSpan.MinValue._ticks)
        throw new OverflowException(SR.Overflow_NegateTwosCompNum);
      return new TimeSpan(-t._ticks);
    }

    /// <summary>Subtracts a specified <see cref="T:System.TimeSpan" /> from another specified <see cref="T:System.TimeSpan" />.</summary>
    /// <param name="t1">The minuend.</param>
    /// <param name="t2">The subtrahend.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.</exception>
    /// <returns>An object whose value is the result of the value of <paramref name="t1" /> minus the value of <paramref name="t2" />.</returns>
    public static TimeSpan operator -(TimeSpan t1, TimeSpan t2) => t1.Subtract(t2);

    /// <summary>Returns the specified instance of <see cref="T:System.TimeSpan" />.</summary>
    /// <param name="t">The time interval to return.</param>
    /// <returns>The time interval specified by <paramref name="t" />.</returns>
    public static TimeSpan operator +(TimeSpan t) => t;

    /// <summary>Adds two specified <see cref="T:System.TimeSpan" /> instances.</summary>
    /// <param name="t1">The first time interval to add.</param>
    /// <param name="t2">The second time interval to add.</param>
    /// <exception cref="T:System.OverflowException">The resulting <see cref="T:System.TimeSpan" /> is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the values of <paramref name="t1" /> and <paramref name="t2" />.</returns>
    public static TimeSpan operator +(TimeSpan t1, TimeSpan t2) => t1.Add(t2);

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object whose value is the result of multiplying the specified <paramref name="timeSpan" /> instance and the specified <paramref name="factor" />.</summary>
    /// <param name="timeSpan">The value to be multiplied.</param>
    /// <param name="factor">The value to be multiplied by.</param>
    /// <returns>A new object that represents the value of the specified <paramref name="timeSpan" /> instance multiplied by the value of the specified <paramref name="factor" />.</returns>
    public static TimeSpan operator *(TimeSpan timeSpan, double factor)
    {
      if (double.IsNaN(factor))
        throw new ArgumentException(SR.Arg_CannotBeNaN, nameof (factor));
      return TimeSpan.IntervalFromDoubleTicks(Math.Round((double) timeSpan.Ticks * factor));
    }

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object whose value is the result of multiplying the specified <paramref name="factor" /> and the specified <paramref name="timeSpan" /> instance.</summary>
    /// <param name="factor">The value to be multiplied by.</param>
    /// <param name="timeSpan">The value to be multiplied.</param>
    /// <returns>A new object that represents the value of the specified <paramref name="factor" /> multiplied by the value of the specified <paramref name="timeSpan" /> instance.</returns>
    public static TimeSpan operator *(double factor, TimeSpan timeSpan) => timeSpan * factor;

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> object which value is the result of division of <paramref name="timeSpan" /> instance and the specified <paramref name="divisor" />.</summary>
    /// <param name="timeSpan">Dividend or the value to be divided.</param>
    /// <param name="divisor">The value to be divided by.</param>
    /// <returns>A new object that represents the value of <paramref name="timeSpan" /> instance divided by the value of <paramref name="divisor" />.</returns>
    public static TimeSpan operator /(TimeSpan timeSpan, double divisor)
    {
      if (double.IsNaN(divisor))
        throw new ArgumentException(SR.Arg_CannotBeNaN, nameof (divisor));
      return TimeSpan.IntervalFromDoubleTicks(Math.Round((double) timeSpan.Ticks / divisor));
    }

    /// <summary>Returns a new <see cref="T:System.Double" /> value which is the result of division of <paramref name="t1" /> instance and the specified <paramref name="t2" />.</summary>
    /// <param name="t1">Divident or the value to be divided.</param>
    /// <param name="t2">The value to be divided by.</param>
    /// <returns>A new value that represents result of division of <paramref name="t1" /> instance by the value of the <paramref name="t2" />.</returns>
    public static double operator /(TimeSpan t1, TimeSpan t2) => (double) t1.Ticks / (double) t2.Ticks;

    /// <summary>Indicates whether two <see cref="T:System.TimeSpan" /> instances are equal.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the values of <paramref name="t1" /> and <paramref name="t2" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(TimeSpan t1, TimeSpan t2) => t1._ticks == t2._ticks;

    /// <summary>Indicates whether two <see cref="T:System.TimeSpan" /> instances are not equal.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the values of <paramref name="t1" /> and <paramref name="t2" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(TimeSpan t1, TimeSpan t2) => t1._ticks != t2._ticks;

    /// <summary>Indicates whether a specified <see cref="T:System.TimeSpan" /> is less than another specified <see cref="T:System.TimeSpan" />.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the value of <paramref name="t1" /> is less than the value of <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <(TimeSpan t1, TimeSpan t2) => t1._ticks < t2._ticks;

    /// <summary>Indicates whether a specified <see cref="T:System.TimeSpan" /> is less than or equal to another specified <see cref="T:System.TimeSpan" />.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the value of <paramref name="t1" /> is less than or equal to the value of <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <=(TimeSpan t1, TimeSpan t2) => t1._ticks <= t2._ticks;

    /// <summary>Indicates whether a specified <see cref="T:System.TimeSpan" /> is greater than another specified <see cref="T:System.TimeSpan" />.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the value of <paramref name="t1" /> is greater than the value of <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >(TimeSpan t1, TimeSpan t2) => t1._ticks > t2._ticks;

    /// <summary>Indicates whether a specified <see cref="T:System.TimeSpan" /> is greater than or equal to another specified <see cref="T:System.TimeSpan" />.</summary>
    /// <param name="t1">The first time interval to compare.</param>
    /// <param name="t2">The second time interval to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the value of <paramref name="t1" /> is greater than or equal to the value of <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >=(TimeSpan t1, TimeSpan t2) => t1._ticks >= t2._ticks;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IAdditionOperators<TimeSpan, TimeSpan, TimeSpan>.op_Addition(
      TimeSpan left,
      TimeSpan right)
    {
      return left + right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IAdditiveIdentity<TimeSpan, TimeSpan>.AdditiveIdentity => new TimeSpan();

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeSpan, TimeSpan>.op_LessThan(
      TimeSpan left,
      TimeSpan right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeSpan, TimeSpan>.op_LessThanOrEqual(
      TimeSpan left,
      TimeSpan right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeSpan, TimeSpan>.op_GreaterThan(
      TimeSpan left,
      TimeSpan right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<TimeSpan, TimeSpan>.op_GreaterThanOrEqual(
      TimeSpan left,
      TimeSpan right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IDivisionOperators<TimeSpan, double, TimeSpan>.op_Division(
      TimeSpan left,
      double right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IDivisionOperators<TimeSpan, TimeSpan, double>.op_Division(
      TimeSpan left,
      TimeSpan right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<TimeSpan, TimeSpan>.op_Equality(
      TimeSpan left,
      TimeSpan right)
    {
      return left == right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<TimeSpan, TimeSpan>.op_Inequality(
      TimeSpan left,
      TimeSpan right)
    {
      return left != right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IMinMaxValue<TimeSpan>.MinValue => TimeSpan.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IMinMaxValue<TimeSpan>.MaxValue => TimeSpan.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    double IMultiplicativeIdentity<TimeSpan, double>.MultiplicativeIdentity => 1.0;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IMultiplyOperators<TimeSpan, double, TimeSpan>.op_Multiply(
      TimeSpan left,
      double right)
    {
      return left * right;
    }


    #nullable disable
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IParseable<TimeSpan>.Parse(
      string s,
      IFormatProvider provider)
    {
      return TimeSpan.Parse(s, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<TimeSpan>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out TimeSpan result)
    {
      return TimeSpan.TryParse(s, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan ISpanParseable<TimeSpan>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return TimeSpan.Parse(s, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<TimeSpan>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out TimeSpan result)
    {
      return TimeSpan.TryParse(s, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan ISubtractionOperators<TimeSpan, TimeSpan, TimeSpan>.op_Subtraction(
      TimeSpan left,
      TimeSpan right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IUnaryNegationOperators<TimeSpan, TimeSpan>.op_UnaryNegation(
      TimeSpan value)
    {
      return -value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IUnaryPlusOperators<TimeSpan, TimeSpan>.op_UnaryPlus(
      TimeSpan value)
    {
      return +value;
    }
  }
}
