// Decompiled with JetBrains decompiler
// Type: System.DateTime
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Represents an instant in time, typically expressed as a date and time of day.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  [StructLayout(LayoutKind.Auto)]
  public readonly struct DateTime : 
    IComparable,
    ISpanFormattable,
    IFormattable,
    IConvertible,
    IComparable<DateTime>,
    IEquatable<DateTime>,
    ISerializable,
    IAdditionOperators<DateTime, TimeSpan, DateTime>,
    IAdditiveIdentity<DateTime, TimeSpan>,
    IComparisonOperators<DateTime, DateTime>,
    IEqualityOperators<DateTime, DateTime>,
    IMinMaxValue<DateTime>,
    ISpanParseable<DateTime>,
    IParseable<DateTime>,
    ISubtractionOperators<DateTime, TimeSpan, DateTime>,
    ISubtractionOperators<DateTime, DateTime, TimeSpan>
  {

    #nullable disable
    private static readonly uint[] s_daysToMonth365 = new uint[13]
    {
      0U,
      31U,
      59U,
      90U,
      120U,
      151U,
      181U,
      212U,
      243U,
      273U,
      304U,
      334U,
      365U
    };
    private static readonly uint[] s_daysToMonth366 = new uint[13]
    {
      0U,
      31U,
      60U,
      91U,
      121U,
      152U,
      182U,
      213U,
      244U,
      274U,
      305U,
      335U,
      366U
    };
    /// <summary>Represents the smallest possible value of <see cref="T:System.DateTime" />. This field is read-only.</summary>
    public static readonly DateTime MinValue;
    /// <summary>Represents the largest possible value of <see cref="T:System.DateTime" />. This field is read-only.</summary>
    public static readonly DateTime MaxValue = new DateTime(3155378975999999999L, DateTimeKind.Unspecified);
    /// <summary>The value of this constant is equivalent to 00:00:00.0000000 UTC, January 1, 1970, in the Gregorian calendar. <see cref="F:System.DateTime.UnixEpoch" /> defines the point in time when Unix time is equal to 0.</summary>
    public static readonly DateTime UnixEpoch = new DateTime(621355968000000000L, DateTimeKind.Utc);
    private readonly ulong _dateData;
    internal static readonly bool s_systemSupportsLeapSeconds = DateTime.SystemSupportsLeapSeconds();
    private static readonly __FnPtr<void (ulong*)> s_pfnGetSystemTimeAsFileTime = DateTime.GetGetSystemTimeAsFileTimeFnPtr();
    private static DateTime.LeapSecondCache s_leapSecondCache = new DateTime.LeapSecondCache();


    #nullable enable
    private static unsafe ReadOnlySpan<byte> DaysInMonth365 => new ReadOnlySpan<byte>((void*) &\u003CPrivateImplementationDetails\u003E.\u0039D61D7D7A1AA7E8ED5214C2F39E0C55230433C7BA728C92913CA4E1967FAF8EA, 12);

    private static unsafe ReadOnlySpan<byte> DaysInMonth366 => new ReadOnlySpan<byte>((void*) &\u003CPrivateImplementationDetails\u003E.\u00369EADD2D8A0D38E5F581C5F3533EE497009AD4A2B8ECA04B388D4CB5B41ACEA5, 12);

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to a specified number of ticks.</summary>
    /// <param name="ticks">A date and time expressed in the number of 100-nanosecond intervals that have elapsed since January 1, 0001 at 00:00:00.000 in the Gregorian calendar.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="ticks" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    public DateTime(long ticks)
    {
      if ((ulong) ticks > 3155378975999999999UL)
        DateTime.ThrowTicksOutOfRange();
      this._dateData = (ulong) ticks;
    }

    private DateTime(ulong dateData) => this._dateData = dateData;

    internal static DateTime UnsafeCreate(long ticks) => new DateTime((ulong) ticks);

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to a specified number of ticks and to Coordinated Universal Time (UTC) or local time.</summary>
    /// <param name="ticks">A date and time expressed in the number of 100-nanosecond intervals that have elapsed since January 1, 0001 at 00:00:00.000 in the Gregorian calendar.</param>
    /// <param name="kind">One of the enumeration values that indicates whether <paramref name="ticks" /> specifies a local time, Coordinated Universal Time (UTC), or neither.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="ticks" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
    public DateTime(long ticks, DateTimeKind kind)
    {
      if ((ulong) ticks > 3155378975999999999UL)
        DateTime.ThrowTicksOutOfRange();
      switch (kind)
      {
        case DateTimeKind.Unspecified:
        case DateTimeKind.Utc:
        case DateTimeKind.Local:
          this._dateData = (ulong) (ticks | (long) (uint) kind << 62);
          break;
        default:
          DateTime.ThrowInvalidKind();
          goto case DateTimeKind.Unspecified;
      }
    }

    internal DateTime(long ticks, DateTimeKind kind, bool isAmbiguousDst)
    {
      if ((ulong) ticks > 3155378975999999999UL)
        DateTime.ThrowTicksOutOfRange();
      this._dateData = (ulong) (ticks | (isAmbiguousDst ? -4611686018427387904L : long.MinValue));
    }

    private static void ThrowTicksOutOfRange() => throw new ArgumentOutOfRangeException("ticks", SR.ArgumentOutOfRange_DateTimeBadTicks);

    private static void ThrowInvalidKind() => throw new ArgumentException(SR.Argument_InvalidDateTimeKind, "kind");

    private static void ThrowMillisecondOutOfRange() => throw new ArgumentOutOfRangeException("millisecond", SR.Format(SR.ArgumentOutOfRange_Range, (object) 0, (object) 999));

    private static void ThrowDateArithmetic(int param)
    {
      string paramName;
      switch (param)
      {
        case 0:
          paramName = "value";
          break;
        case 1:
          paramName = "t";
          break;
        default:
          paramName = "months";
          break;
      }
      throw new ArgumentOutOfRangeException(paramName, SR.ArgumentOutOfRange_DateArithmetic);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, and day.</summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is less than 1 or greater than 9999.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than 12.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.</exception>
    public DateTime(int year, int month, int day) => this._dateData = DateTime.DateToTicks(year, month, day);

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, and day for the specified calendar.</summary>
    /// <param name="year">The year (1 through the number of years in <paramref name="calendar" />).</param>
    /// <param name="month">The month (1 through the number of months in <paramref name="calendar" />).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="calendar" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.</exception>
    public DateTime(int year, int month, int day, Calendar calendar)
      : this(year, month, day, 0, 0, 0, calendar)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, and second.</summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is less than 1 or greater than 9999.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than 12.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="hour" /> is less than 0 or greater than 23.
    /// 
    /// -or-
    /// 
    /// <paramref name="minute" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="second" /> is less than 0 or greater than 59.</exception>
    public DateTime(int year, int month, int day, int hour, int minute, int second)
    {
      if (second != 60 || !DateTime.s_systemSupportsLeapSeconds)
      {
        this._dateData = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second);
      }
      else
      {
        this = new DateTime(year, month, day, hour, minute, 59);
        this.ValidateLeapSecond();
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, and Coordinated Universal Time (UTC) or local time.</summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <param name="kind">One of the enumeration values that indicates whether <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, <paramref name="hour" />, <paramref name="minute" /> and <paramref name="second" /> specify a local time, Coordinated Universal Time (UTC), or neither.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is less than 1 or greater than 9999.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than 12.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="hour" /> is less than 0 or greater than 23.
    /// 
    /// -or-
    /// 
    /// <paramref name="minute" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="second" /> is less than 0 or greater than 59.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
    public DateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      DateTimeKind kind)
    {
      switch (kind)
      {
        case DateTimeKind.Unspecified:
        case DateTimeKind.Utc:
        case DateTimeKind.Local:
          if (second != 60 || !DateTime.s_systemSupportsLeapSeconds)
          {
            this._dateData = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second) | (ulong) kind << 62;
            break;
          }
          this = new DateTime(year, month, day, hour, minute, 59, kind);
          this.ValidateLeapSecond();
          break;
        default:
          DateTime.ThrowInvalidKind();
          goto case DateTimeKind.Unspecified;
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, and second for the specified calendar.</summary>
    /// <param name="year">The year (1 through the number of years in <paramref name="calendar" />).</param>
    /// <param name="month">The month (1 through the number of months in <paramref name="calendar" />).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="calendar" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="hour" /> is less than 0 or greater than 23
    /// 
    /// -or-
    /// 
    /// <paramref name="minute" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="second" /> is less than 0 or greater than 59.</exception>
    public DateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      Calendar calendar)
    {
      if (calendar == null)
        throw new ArgumentNullException(nameof (calendar));
      if (second != 60 || !DateTime.s_systemSupportsLeapSeconds)
      {
        this._dateData = calendar.ToDateTime(year, month, day, hour, minute, second, 0).UTicks;
      }
      else
      {
        this = new DateTime(year, month, day, hour, minute, 59, calendar);
        this.ValidateLeapSecond();
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, and millisecond.</summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <param name="millisecond">The milliseconds (0 through 999).</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is less than 1 or greater than 9999.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than 12.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="hour" /> is less than 0 or greater than 23.
    /// 
    /// -or-
    /// 
    /// <paramref name="minute" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="second" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="millisecond" /> is less than 0 or greater than 999.</exception>
    public DateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond)
    {
      if ((uint) millisecond >= 1000U)
        DateTime.ThrowMillisecondOutOfRange();
      if (second != 60 || !DateTime.s_systemSupportsLeapSeconds)
      {
        this._dateData = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second) + (ulong) (uint) (millisecond * 10000);
      }
      else
      {
        this = new DateTime(year, month, day, hour, minute, 59, millisecond);
        this.ValidateLeapSecond();
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, millisecond, and Coordinated Universal Time (UTC) or local time.</summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <param name="millisecond">The milliseconds (0 through 999).</param>
    /// <param name="kind">One of the enumeration values that indicates whether <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, <paramref name="hour" />, <paramref name="minute" />, <paramref name="second" />, and <paramref name="millisecond" /> specify a local time, Coordinated Universal Time (UTC), or neither.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is less than 1 or greater than 9999.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than 12.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="hour" /> is less than 0 or greater than 23.
    /// 
    /// -or-
    /// 
    /// <paramref name="minute" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="second" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="millisecond" /> is less than 0 or greater than 999.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
    public DateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      DateTimeKind kind)
    {
      if ((uint) millisecond >= 1000U)
        DateTime.ThrowMillisecondOutOfRange();
      switch (kind)
      {
        case DateTimeKind.Unspecified:
        case DateTimeKind.Utc:
        case DateTimeKind.Local:
          if (second != 60 || !DateTime.s_systemSupportsLeapSeconds)
          {
            this._dateData = DateTime.DateToTicks(year, month, day) + DateTime.TimeToTicks(hour, minute, second) + (ulong) (uint) (millisecond * 10000) | (ulong) kind << 62;
            break;
          }
          this = new DateTime(year, month, day, hour, minute, 59, millisecond, kind);
          this.ValidateLeapSecond();
          break;
        default:
          DateTime.ThrowInvalidKind();
          goto case DateTimeKind.Unspecified;
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, and millisecond for the specified calendar.</summary>
    /// <param name="year">The year (1 through the number of years in <paramref name="calendar" />).</param>
    /// <param name="month">The month (1 through the number of months in <paramref name="calendar" />).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <param name="millisecond">The milliseconds (0 through 999).</param>
    /// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="calendar" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="hour" /> is less than 0 or greater than 23.
    /// 
    /// -or-
    /// 
    /// <paramref name="minute" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="second" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="millisecond" /> is less than 0 or greater than 999.</exception>
    public DateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      Calendar calendar)
    {
      if (calendar == null)
        throw new ArgumentNullException(nameof (calendar));
      if (second != 60 || !DateTime.s_systemSupportsLeapSeconds)
      {
        this._dateData = calendar.ToDateTime(year, month, day, hour, minute, second, millisecond).UTicks;
      }
      else
      {
        this = new DateTime(year, month, day, hour, minute, 59, millisecond, calendar);
        this.ValidateLeapSecond();
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, millisecond, and Coordinated Universal Time (UTC) or local time for the specified calendar.</summary>
    /// <param name="year">The year (1 through the number of years in <paramref name="calendar" />).</param>
    /// <param name="month">The month (1 through the number of months in <paramref name="calendar" />).</param>
    /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
    /// <param name="hour">The hours (0 through 23).</param>
    /// <param name="minute">The minutes (0 through 59).</param>
    /// <param name="second">The seconds (0 through 59).</param>
    /// <param name="millisecond">The milliseconds (0 through 999).</param>
    /// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />.</param>
    /// <param name="kind">One of the enumeration values that indicates whether <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, <paramref name="hour" />, <paramref name="minute" />, <paramref name="second" />, and <paramref name="millisecond" /> specify a local time, Coordinated Universal Time (UTC), or neither.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="calendar" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="hour" /> is less than 0 or greater than 23.
    /// 
    /// -or-
    /// 
    /// <paramref name="minute" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="second" /> is less than 0 or greater than 59.
    /// 
    /// -or-
    /// 
    /// <paramref name="millisecond" /> is less than 0 or greater than 999.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
    public DateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      Calendar calendar,
      DateTimeKind kind)
    {
      if (calendar == null)
        throw new ArgumentNullException(nameof (calendar));
      if ((uint) millisecond >= 1000U)
        DateTime.ThrowMillisecondOutOfRange();
      switch (kind)
      {
        case DateTimeKind.Unspecified:
        case DateTimeKind.Utc:
        case DateTimeKind.Local:
          if (second != 60 || !DateTime.s_systemSupportsLeapSeconds)
          {
            this._dateData = calendar.ToDateTime(year, month, day, hour, minute, second, millisecond).UTicks | (ulong) kind << 62;
            break;
          }
          this = new DateTime(year, month, day, hour, minute, 59, millisecond, calendar, kind);
          this.ValidateLeapSecond();
          break;
        default:
          DateTime.ThrowInvalidKind();
          goto case DateTimeKind.Unspecified;
      }
    }

    private void ValidateLeapSecond()
    {
      if (DateTime.IsValidTimeWithLeapSeconds(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Kind))
        return;
      ThrowHelper.ThrowArgumentOutOfRange_BadHourMinuteSecond();
    }


    #nullable disable
    private DateTime(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
      bool flag = false;
      SerializationInfoEnumerator enumerator = info.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string name = enumerator.Name;
        if (!(name == "ticks"))
        {
          if (name == "dateData")
          {
            this._dateData = Convert.ToUInt64(enumerator.Value, (IFormatProvider) CultureInfo.InvariantCulture);
            goto label_11;
          }
        }
        else
        {
          this._dateData = (ulong) Convert.ToInt64(enumerator.Value, (IFormatProvider) CultureInfo.InvariantCulture);
          flag = true;
        }
      }
      if (!flag)
        throw new SerializationException(SR.Serialization_MissingDateTimeData);
label_11:
      if (this.UTicks > 3155378975999999999UL)
        throw new SerializationException(SR.Serialization_DateTimeTicksOutOfRange);
    }

    private ulong UTicks => this._dateData & 4611686018427387903UL;

    private ulong InternalKind => this._dateData & 13835058055282163712UL;

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the value of the specified <see cref="T:System.TimeSpan" /> to the value of this instance.</summary>
    /// <param name="value">A positive or negative time interval.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the time interval represented by <paramref name="value" />.</returns>
    public DateTime Add(TimeSpan value) => this.AddTicks(value._ticks);

    private DateTime Add(double value, int scale)
    {
      double num = value * (double) scale + (value >= 0.0 ? 0.5 : -0.5);
      if (num <= -315537897600000.0 || num >= 315537897600000.0)
        ThrowOutOfRange();
      return this.AddTicks((long) num * 10000L);

      static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException("value", SR.ArgumentOutOfRange_AddValue);
    }

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of days to the value of this instance.</summary>
    /// <param name="value">A number of whole and fractional days. The <paramref name="value" /> parameter can be negative or positive.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the number of days represented by <paramref name="value" />.</returns>
    public DateTime AddDays(double value) => this.Add(value, 86400000);

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of hours to the value of this instance.</summary>
    /// <param name="value">A number of whole and fractional hours. The <paramref name="value" /> parameter can be negative or positive.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the number of hours represented by <paramref name="value" />.</returns>
    public DateTime AddHours(double value) => this.Add(value, 3600000);

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of milliseconds to the value of this instance.</summary>
    /// <param name="value">A number of whole and fractional milliseconds. The <paramref name="value" /> parameter can be negative or positive. Note that this value is rounded to the nearest integer.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the number of milliseconds represented by <paramref name="value" />.</returns>
    public DateTime AddMilliseconds(double value) => this.Add(value, 1);

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of minutes to the value of this instance.</summary>
    /// <param name="value">A number of whole and fractional minutes. The <paramref name="value" /> parameter can be negative or positive.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the number of minutes represented by <paramref name="value" />.</returns>
    public DateTime AddMinutes(double value) => this.Add(value, 60000);

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of months to the value of this instance.</summary>
    /// <param name="months">A number of months. The <paramref name="months" /> parameter can be negative or positive.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="months" /> is less than -120,000 or greater than 120,000.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and <paramref name="months" />.</returns>
    public DateTime AddMonths(int months)
    {
      if (months < -120000 || months > 120000)
        throw new ArgumentOutOfRangeException(nameof (months), SR.ArgumentOutOfRange_DateTimeBadMonths);
      int year1;
      int month;
      int day;
      this.GetDate(out year1, out month, out day);
      int num1 = year1;
      int num2 = day;
      int num3 = month + months;
      int year2;
      int index;
      if (num3 > 0)
      {
        int num4 = (int) ((uint) (num3 - 1) / 12U);
        year2 = num1 + num4;
        index = num3 - num4 * 12;
      }
      else
      {
        year2 = num1 + (num3 / 12 - 1);
        index = 12 + num3 % 12;
      }
      if (year2 < 1 || year2 > 9999)
        DateTime.ThrowDateArithmetic(2);
      uint[] numArray = DateTime.IsLeapYear(year2) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
      uint num5 = numArray[index - 1];
      int num6 = (int) numArray[index] - (int) num5;
      if (num2 > num6)
        num2 = num6;
      return new DateTime((ulong) (uint) ((int) DateTime.DaysToYear((uint) year2) + (int) num5 + num2 - 1) * 864000000000UL + this.UTicks % 864000000000UL | this.InternalKind);
    }

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of seconds to the value of this instance.</summary>
    /// <param name="value">A number of whole and fractional seconds. The <paramref name="value" /> parameter can be negative or positive.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the number of seconds represented by <paramref name="value" />.</returns>
    public DateTime AddSeconds(double value) => this.Add(value, 1000);

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of ticks to the value of this instance.</summary>
    /// <param name="value">A number of 100-nanosecond ticks. The <paramref name="value" /> parameter can be positive or negative.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the time represented by <paramref name="value" />.</returns>
    public DateTime AddTicks(long value)
    {
      ulong num = (ulong) (this.Ticks + value);
      if (num > 3155378975999999999UL)
        DateTime.ThrowDateArithmetic(0);
      return new DateTime(num | this.InternalKind);
    }

    internal bool TryAddTicks(long value, out DateTime result)
    {
      ulong num = (ulong) (this.Ticks + value);
      if (num > 3155378975999999999UL)
      {
        result = new DateTime();
        return false;
      }
      result = new DateTime(num | this.InternalKind);
      return true;
    }

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of years to the value of this instance.</summary>
    /// <param name="value">A number of years. The <paramref name="value" /> parameter can be negative or positive.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> or the resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the sum of the date and time represented by this instance and the number of years represented by <paramref name="value" />.</returns>
    public DateTime AddYears(int value)
    {
      if (value < -10000 || value > 10000)
        throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_DateTimeBadYears);
      int year1;
      int month;
      int day;
      this.GetDate(out year1, out month, out day);
      int year2 = year1 + value;
      if (year2 < 1 || year2 > 9999)
        DateTime.ThrowDateArithmetic(0);
      uint year3 = DateTime.DaysToYear((uint) year2);
      int index = month - 1;
      int num1 = day - 1;
      uint num2;
      if (DateTime.IsLeapYear(year2))
      {
        num2 = year3 + DateTime.s_daysToMonth366[index];
      }
      else
      {
        if (num1 == 28 && index == 1)
          --num1;
        num2 = year3 + DateTime.s_daysToMonth365[index];
      }
      return new DateTime((ulong) (num2 + (uint) num1) * 864000000000UL + this.UTicks % 864000000000UL | this.InternalKind);
    }

    /// <summary>Compares two instances of <see cref="T:System.DateTime" /> and returns an integer that indicates whether the first instance is earlier than, the same as, or later than the second instance.</summary>
    /// <param name="t1">The first object to compare.</param>
    /// <param name="t2">The second object to compare.</param>
    /// <returns>A signed number indicating the relative values of <paramref name="t1" /> and <paramref name="t2" />.
    /// 
    /// <list type="table"><listheader><term> Value Type</term><description> Condition</description></listheader><item><term> Less than zero</term><description><paramref name="t1" /> is earlier than <paramref name="t2" />.</description></item><item><term> Zero</term><description><paramref name="t1" /> is the same as <paramref name="t2" />.</description></item><item><term> Greater than zero</term><description><paramref name="t1" /> is later than <paramref name="t2" />.</description></item></list></returns>
    public static int Compare(DateTime t1, DateTime t2)
    {
      long ticks1 = t1.Ticks;
      long ticks2 = t2.Ticks;
      if (ticks1 > ticks2)
        return 1;
      return ticks1 < ticks2 ? -1 : 0;
    }


    #nullable enable
    /// <summary>Compares the value of this instance to a specified object that contains a specified <see cref="T:System.DateTime" /> value, and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.DateTime" /> value.</summary>
    /// <param name="value">A boxed object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.DateTime" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is earlier than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is the same as <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is later than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      return value is DateTime t2 ? DateTime.Compare(this, t2) : throw new ArgumentException(SR.Arg_MustBeDateTime);
    }

    /// <summary>Compares the value of this instance to a specified <see cref="T:System.DateTime" /> value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.DateTime" /> value.</summary>
    /// <param name="value">The object to compare to the current instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and the <paramref name="value" /> parameter.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> This instance is earlier than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is the same as <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is later than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(DateTime value) => DateTime.Compare(this, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong DateToTicks(int year, int month, int day)
    {
      if (year < 1 || year > 9999 || month < 1 || month > 12 || day < 1)
        ThrowHelper.ThrowArgumentOutOfRange_BadYearMonthDay();
      uint[] numArray = DateTime.IsLeapYear(year) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
      if ((uint) day > numArray[month] - numArray[month - 1])
        ThrowHelper.ThrowArgumentOutOfRange_BadYearMonthDay();
      return (ulong) (uint) ((int) DateTime.DaysToYear((uint) year) + (int) numArray[month - 1] + day - 1) * 864000000000UL;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint DaysToYear(uint year)
    {
      uint num1 = year - 1U;
      uint num2 = num1 / 100U;
      return num1 * 1461U / 4U - num2 + num2 / 4U;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong TimeToTicks(int hour, int minute, int second)
    {
      if ((uint) hour >= 24U || (uint) minute >= 60U || (uint) second >= 60U)
        ThrowHelper.ThrowArgumentOutOfRange_BadHourMinuteSecond();
      return (ulong) (uint) (hour * 3600 + minute * 60 + second) * 10000000UL;
    }

    internal static ulong TimeToTicks(int hour, int minute, int second, int millisecond)
    {
      ulong ticks = DateTime.TimeToTicks(hour, minute, second);
      if ((uint) millisecond >= 1000U)
        DateTime.ThrowMillisecondOutOfRange();
      return ticks + (ulong) (uint) (millisecond * 10000);
    }

    /// <summary>Returns the number of days in the specified month and year.</summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month (a number ranging from 1 to 12).</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="month" /> is less than 1 or greater than 12.
    /// 
    /// -or-
    /// 
    /// <paramref name="year" /> is less than 1 or greater than 9999.</exception>
    /// <returns>The number of days in <paramref name="month" /> for the specified <paramref name="year" />.
    /// 
    /// For example, if <paramref name="month" /> equals 2 for February, the return value is 28 or 29 depending upon whether <paramref name="year" /> is a leap year.</returns>
    public static int DaysInMonth(int year, int month)
    {
      if (month < 1 || month > 12)
        ThrowHelper.ThrowArgumentOutOfRange_Month(month);
      return (int) (DateTime.IsLeapYear(year) ? DateTime.DaysInMonth366 : DateTime.DaysInMonth365)[month - 1];
    }

    internal static long DoubleDateToTicks(double value)
    {
      if (value >= 2958466.0 || value <= -657435.0)
        throw new ArgumentException(SR.Arg_OleAutDateInvalid);
      long num1 = (long) (value * 86400000.0 + (value >= 0.0 ? 0.5 : -0.5));
      if (num1 < 0L)
        num1 -= num1 % 86400000L * 2L;
      long num2 = num1 + 59926435200000L;
      if (num2 < 0L || num2 >= 315537897600000L)
        throw new ArgumentException(SR.Arg_OleAutDateScale);
      return num2 * 10000L;
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="value">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is an instance of <see cref="T:System.DateTime" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? value) => value is DateTime dateTime && this.Ticks == dateTime.Ticks;

    /// <summary>Returns a value indicating whether the value of this instance is equal to the value of the specified <see cref="T:System.DateTime" /> instance.</summary>
    /// <param name="value">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="value" /> parameter equals the value of this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(DateTime value) => this.Ticks == value.Ticks;

    /// <summary>Returns a value indicating whether two <see cref="T:System.DateTime" /> instances  have the same date and time value.</summary>
    /// <param name="t1">The first object to compare.</param>
    /// <param name="t2">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the two values are equal; otherwise, <see langword="false" />.</returns>
    public static bool Equals(DateTime t1, DateTime t2) => t1.Ticks == t2.Ticks;

    /// <summary>Deserializes a 64-bit binary value and recreates an original serialized <see cref="T:System.DateTime" /> object.</summary>
    /// <param name="dateData">A 64-bit signed integer that encodes the <see cref="P:System.DateTime.Kind" /> property in a 2-bit field and the <see cref="P:System.DateTime.Ticks" /> property in a 62-bit field.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="dateData" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object that is equivalent to the <see cref="T:System.DateTime" /> object that was serialized by the <see cref="M:System.DateTime.ToBinary" /> method.</returns>
    public static DateTime FromBinary(long dateData)
    {
      if ((dateData & long.MinValue) != 0L)
      {
        long ticks1 = dateData & 4611686018427387903L;
        if (ticks1 > 4611685154427387904L)
          ticks1 -= 4611686018427387904L;
        bool isAmbiguousLocalDst = false;
        long num = ticks1 >= 0L ? (ticks1 <= 3155378975999999999L ? TimeZoneInfo.GetUtcOffsetFromUtc(new DateTime(ticks1, DateTimeKind.Utc), TimeZoneInfo.Local, out bool _, out isAmbiguousLocalDst).Ticks : TimeZoneInfo.GetLocalUtcOffset(DateTime.MaxValue, TimeZoneInfoOptions.NoThrowOnInvalidTime).Ticks) : TimeZoneInfo.GetLocalUtcOffset(DateTime.MinValue, TimeZoneInfoOptions.NoThrowOnInvalidTime).Ticks;
        long ticks2 = ticks1 + num;
        if (ticks2 < 0L)
          ticks2 += 864000000000L;
        return (ulong) ticks2 <= 3155378975999999999UL ? new DateTime(ticks2, DateTimeKind.Local, isAmbiguousLocalDst) : throw new ArgumentException(SR.Argument_DateTimeBadBinaryData, nameof (dateData));
      }
      return (ulong) (dateData & 4611686018427387903L) <= 3155378975999999999UL ? new DateTime((ulong) dateData) : throw new ArgumentException(SR.Argument_DateTimeBadBinaryData, nameof (dateData));
    }

    /// <summary>Converts the specified Windows file time to an equivalent local time.</summary>
    /// <param name="fileTime">A Windows file time expressed in ticks.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileTime" /> is less than 0 or represents a time greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object that represents the local time equivalent of the date and time represented by the <paramref name="fileTime" /> parameter.</returns>
    public static DateTime FromFileTime(long fileTime) => DateTime.FromFileTimeUtc(fileTime).ToLocalTime();

    /// <summary>Converts the specified Windows file time to an equivalent UTC time.</summary>
    /// <param name="fileTime">A Windows file time expressed in ticks.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileTime" /> is less than 0 or represents a time greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object that represents the UTC time equivalent of the date and time represented by the <paramref name="fileTime" /> parameter.</returns>
    public static DateTime FromFileTimeUtc(long fileTime)
    {
      if ((ulong) fileTime > 2650467743999999999UL)
        throw new ArgumentOutOfRangeException(nameof (fileTime), SR.ArgumentOutOfRange_FileTimeInvalid);
      return DateTime.s_systemSupportsLeapSeconds ? DateTime.FromFileTimeLeapSecondsAware((ulong) fileTime) : new DateTime((ulong) (fileTime + 504911232000000000L) | 4611686018427387904UL);
    }

    /// <summary>Returns a <see cref="T:System.DateTime" /> equivalent to the specified OLE Automation Date.</summary>
    /// <param name="d">An OLE Automation Date value.</param>
    /// <exception cref="T:System.ArgumentException">The date is not a valid OLE Automation Date value.</exception>
    /// <returns>An object that represents the same date and time as <paramref name="d" />.</returns>
    public static DateTime FromOADate(double d) => new DateTime(DateTime.DoubleDateToTicks(d), DateTimeKind.Unspecified);


    #nullable disable
    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize the current <see cref="T:System.DateTime" /> object.</summary>
    /// <param name="info">The object to populate with data.</param>
    /// <param name="context">The destination for this serialization. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
      info.AddValue("ticks", this.Ticks);
      info.AddValue("dateData", this._dateData);
    }

    /// <summary>Indicates whether this instance of <see cref="T:System.DateTime" /> is within the daylight saving time range for the current time zone.</summary>
    /// <returns>
    /// <see langword="true" /> if the value of the <see cref="P:System.DateTime.Kind" /> property is <see cref="F:System.DateTimeKind.Local" /> or <see cref="F:System.DateTimeKind.Unspecified" /> and the value of this instance of <see cref="T:System.DateTime" /> is within the daylight saving time range for the local time zone; <see langword="false" /> if <see cref="P:System.DateTime.Kind" /> is <see cref="F:System.DateTimeKind.Utc" />.</returns>
    public bool IsDaylightSavingTime() => this.InternalKind != 4611686018427387904UL && TimeZoneInfo.Local.IsDaylightSavingTime(this, TimeZoneInfoOptions.NoThrowOnInvalidTime);

    /// <summary>Creates a new <see cref="T:System.DateTime" /> object that has the same number of ticks as the specified <see cref="T:System.DateTime" />, but is designated as either local time, Coordinated Universal Time (UTC), or neither, as indicated by the specified <see cref="T:System.DateTimeKind" /> value.</summary>
    /// <param name="value">A date and time.</param>
    /// <param name="kind">One of the enumeration values that indicates whether the new object represents local time, UTC, or neither.</param>
    /// <returns>A new object that has the same number of ticks as the object represented by the <paramref name="value" /> parameter and the <see cref="T:System.DateTimeKind" /> value specified by the <paramref name="kind" /> parameter.</returns>
    public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
    {
      switch (kind)
      {
        case DateTimeKind.Unspecified:
        case DateTimeKind.Utc:
        case DateTimeKind.Local:
          return new DateTime(value.UTicks | (ulong) kind << 62);
        default:
          DateTime.ThrowInvalidKind();
          goto case DateTimeKind.Unspecified;
      }
    }

    /// <summary>Serializes the current <see cref="T:System.DateTime" /> object to a 64-bit binary value that subsequently can be used to recreate the <see cref="T:System.DateTime" /> object.</summary>
    /// <returns>A 64-bit signed integer that encodes the <see cref="P:System.DateTime.Kind" /> and <see cref="P:System.DateTime.Ticks" /> properties.</returns>
    public long ToBinary()
    {
      if (((long) this._dateData & long.MinValue) == 0L)
        return (long) this._dateData;
      long num = this.Ticks - TimeZoneInfo.GetLocalUtcOffset(this, TimeZoneInfoOptions.NoThrowOnInvalidTime).Ticks;
      if (num < 0L)
        num = 4611686018427387904L + num;
      return num | long.MinValue;
    }

    /// <summary>Gets the date component of this instance.</summary>
    /// <returns>A new object with the same date as this instance, and the time value set to 12:00:00 midnight (00:00:00).</returns>
    public DateTime Date
    {
      get
      {
        ulong uticks = this.UTicks;
        return new DateTime(uticks - uticks % 864000000000UL | this.InternalKind);
      }
    }

    private int GetDatePart(int part)
    {
      uint num1 = (uint) (this.UTicks / 864000000000UL);
      uint num2 = num1 / 146097U;
      uint num3 = num1 - num2 * 146097U;
      uint num4 = num3 / 36524U;
      if (num4 == 4U)
        num4 = 3U;
      uint num5 = num3 - num4 * 36524U;
      uint num6 = num5 / 1461U;
      uint num7 = num5 - num6 * 1461U;
      uint num8 = num7 / 365U;
      if (num8 == 4U)
        num8 = 3U;
      if (part == 0)
        return (int) num2 * 400 + (int) num4 * 100 + (int) num6 * 4 + (int) num8 + 1;
      uint num9 = num7 - num8 * 365U;
      if (part == 1)
        return (int) num9 + 1;
      uint[] numArray = num8 != 3U || num6 == 24U && num4 != 3U ? DateTime.s_daysToMonth365 : DateTime.s_daysToMonth366;
      uint index = (num9 >> 5) + 1U;
      while (num9 >= numArray[(int) index])
        ++index;
      return part == 2 ? (int) index : (int) num9 - (int) numArray[(int) index - 1] + 1;
    }

    internal void GetDate(out int year, out int month, out int day)
    {
      uint num1 = (uint) (this.UTicks / 864000000000UL);
      uint num2 = num1 / 146097U;
      uint num3 = num1 - num2 * 146097U;
      uint num4 = num3 / 36524U;
      if (num4 == 4U)
        num4 = 3U;
      uint num5 = num3 - num4 * 36524U;
      uint num6 = num5 / 1461U;
      uint num7 = num5 - num6 * 1461U;
      uint num8 = num7 / 365U;
      if (num8 == 4U)
        num8 = 3U;
      year = (int) num2 * 400 + (int) num4 * 100 + (int) num6 * 4 + (int) num8 + 1;
      uint num9 = num7 - num8 * 365U;
      uint[] numArray = num8 != 3U || num6 == 24U && num4 != 3U ? DateTime.s_daysToMonth365 : DateTime.s_daysToMonth366;
      uint index = (num9 >> 5) + 1U;
      while (num9 >= numArray[(int) index])
        ++index;
      month = (int) index;
      day = (int) num9 - (int) numArray[(int) index - 1] + 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void GetTime(out int hour, out int minute, out int second)
    {
      ulong num1 = this.UTicks / 10000000UL;
      ulong num2 = num1 / 60UL;
      second = (int) ((long) num1 - (long) num2 * 60L);
      ulong num3 = num2 / 60UL;
      minute = (int) ((long) num2 - (long) num3 * 60L);
      hour = (int) ((uint) num3 % 24U);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void GetTime(out int hour, out int minute, out int second, out int millisecond)
    {
      ulong num1 = this.UTicks / 10000UL;
      ulong num2 = num1 / 1000UL;
      millisecond = (int) ((long) num1 - (long) num2 * 1000L);
      ulong num3 = num2 / 60UL;
      second = (int) ((long) num2 - (long) num3 * 60L);
      ulong num4 = num3 / 60UL;
      minute = (int) ((long) num3 - (long) num4 * 60L);
      hour = (int) ((uint) num4 % 24U);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void GetTimePrecise(out int hour, out int minute, out int second, out int tick)
    {
      ulong uticks = this.UTicks;
      ulong num1 = uticks / 10000000UL;
      tick = (int) ((long) uticks - (long) num1 * 10000000L);
      ulong num2 = num1 / 60UL;
      second = (int) ((long) num1 - (long) num2 * 60L);
      ulong num3 = num2 / 60UL;
      minute = (int) ((long) num2 - (long) num3 * 60L);
      hour = (int) ((uint) num3 % 24U);
    }

    /// <summary>Gets the day of the month represented by this instance.</summary>
    /// <returns>The day component, expressed as a value between 1 and 31.</returns>
    public int Day => this.GetDatePart(3);

    /// <summary>Gets the day of the week represented by this instance.</summary>
    /// <returns>An enumerated constant that indicates the day of the week of this <see cref="T:System.DateTime" /> value.</returns>
    public DayOfWeek DayOfWeek => (DayOfWeek) (((uint) (this.UTicks / 864000000000UL) + 1U) % 7U);

    /// <summary>Gets the day of the year represented by this instance.</summary>
    /// <returns>The day of the year, expressed as a value between 1 and 366.</returns>
    public int DayOfYear => this.GetDatePart(1);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
      long ticks = this.Ticks;
      return (int) ticks ^ (int) (ticks >> 32);
    }

    /// <summary>Gets the hour component of the date represented by this instance.</summary>
    /// <returns>The hour component, expressed as a value between 0 and 23.</returns>
    public int Hour => (int) ((uint) (this.UTicks / 36000000000UL) % 24U);

    internal bool IsAmbiguousDaylightSavingTime() => this.InternalKind == 13835058055282163712UL;

    /// <summary>Gets a value that indicates whether the time represented by this instance is based on local time, Coordinated Universal Time (UTC), or neither.</summary>
    /// <returns>One of the enumeration values that indicates what the current time represents. The default is <see cref="F:System.DateTimeKind.Unspecified" />.</returns>
    public DateTimeKind Kind
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        DateTimeKind kind;
        switch (this.InternalKind)
        {
          case 0:
            kind = DateTimeKind.Unspecified;
            break;
          case 4611686018427387904:
            kind = DateTimeKind.Utc;
            break;
          default:
            kind = DateTimeKind.Local;
            break;
        }
        return kind;
      }
    }

    /// <summary>Gets the milliseconds component of the date represented by this instance.</summary>
    /// <returns>The milliseconds component, expressed as a value between 0 and 999.</returns>
    public int Millisecond => (int) (this.UTicks / 10000UL % 1000UL);

    /// <summary>Gets the minute component of the date represented by this instance.</summary>
    /// <returns>The minute component, expressed as a value between 0 and 59.</returns>
    public int Minute => (int) (this.UTicks / 600000000UL % 60UL);

    /// <summary>Gets the month component of the date represented by this instance.</summary>
    /// <returns>The month component, expressed as a value between 1 and 12.</returns>
    public int Month => this.GetDatePart(2);

    /// <summary>Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer, expressed as the local time.</summary>
    /// <returns>An object whose value is the current local date and time.</returns>
    public static DateTime Now
    {
      get
      {
        DateTime utcNow = DateTime.UtcNow;
        bool isAmbiguousLocalDst;
        long ticks = TimeZoneInfo.GetDateTimeNowUtcOffsetFromUtc(utcNow, out isAmbiguousLocalDst).Ticks;
        long num = utcNow.Ticks + ticks;
        if ((ulong) num > 3155378975999999999UL)
          return new DateTime(num < 0L ? 9223372036854775808UL : 12378751012854775807UL);
        return !isAmbiguousLocalDst ? new DateTime((ulong) (num | long.MinValue)) : new DateTime((ulong) (num | -4611686018427387904L));
      }
    }

    /// <summary>Gets the seconds component of the date represented by this instance.</summary>
    /// <returns>The seconds component, expressed as a value between 0 and 59.</returns>
    public int Second => (int) (this.UTicks / 10000000UL % 60UL);

    /// <summary>Gets the number of ticks that represent the date and time of this instance.</summary>
    /// <returns>The number of ticks that represent the date and time of this instance. The value is between <see langword="DateTime.MinValue.Ticks" /> and <see langword="DateTime.MaxValue.Ticks" />.</returns>
    public long Ticks => (long) this._dateData & 4611686018427387903L;

    /// <summary>Gets the time of day for this instance.</summary>
    /// <returns>A time interval that represents the fraction of the day that has elapsed since midnight.</returns>
    public TimeSpan TimeOfDay => new TimeSpan((long) (this.UTicks % 864000000000UL));

    /// <summary>Gets the current date.</summary>
    /// <returns>An object that is set to today's date, with the time component set to 00:00:00.</returns>
    public static DateTime Today => DateTime.Now.Date;

    /// <summary>Gets the year component of the date represented by this instance.</summary>
    /// <returns>The year, between 1 and 9999.</returns>
    public int Year => this.GetDatePart(0);

    /// <summary>Returns an indication whether the specified year is a leap year.</summary>
    /// <param name="year">A 4-digit year.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="year" /> is less than 1 or greater than 9999.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="year" /> is a leap year; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(int year)
    {
      if (year < 1 || year > 9999)
        ThrowHelper.ThrowArgumentOutOfRange_Year();
      return (year & 3) == 0 && ((year & 15) == 0 || (uint) year % 25U != 0U);
    }


    #nullable enable
    /// <summary>Converts the string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent by using the conventions of the current thread culture.</summary>
    /// <param name="s">A string that contains a date and time to convert. See The string to parse for more information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date and time.</exception>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />.</returns>
    public static DateTime Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return DateTimeParse.Parse((ReadOnlySpan<char>) s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
    }

    /// <summary>Converts the string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent by using culture-specific format information.</summary>
    /// <param name="s">A string that contains a date and time to convert. See The string to parse for more information.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.  See Parsing and cultural conventions</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date and time.</exception>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" /> as specified by <paramref name="provider" />.</returns>
    public static DateTime Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return DateTimeParse.Parse((ReadOnlySpan<char>) s, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None);
    }

    /// <summary>Converts the string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent by using culture-specific format information and a formatting style.</summary>
    /// <param name="s">A string that contains a date and time to convert. See The string to parse for more information.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.  See Parsing and cultural conventions</param>
    /// <param name="styles">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" /> for the parse operation to succeed, and that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date and time.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values. For example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />.</exception>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="provider" /> and <paramref name="styles" />.</returns>
    public static DateTime Parse(string s, IFormatProvider? provider, DateTimeStyles styles)
    {
      DateTimeFormatInfo.ValidateStyles(styles, true);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return DateTimeParse.Parse((ReadOnlySpan<char>) s, DateTimeFormatInfo.GetInstance(provider), styles);
    }

    /// <summary>Converts a memory span that contains string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent by using culture-specific format information and a formatting style.</summary>
    /// <param name="s">The memory span that contains the string to parse. See The string to parse for more information.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.  See Parsing and cultural conventions</param>
    /// <param name="styles">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" /> for the parse operation to succeed, and that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> does not contain a valid string representation of a date and time.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values. For example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />.</exception>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="provider" /> and <paramref name="styles" />.</returns>
    public static DateTime Parse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider = null,
      DateTimeStyles styles = DateTimeStyles.None)
    {
      DateTimeFormatInfo.ValidateStyles(styles, true);
      return DateTimeParse.Parse(s, DateTimeFormatInfo.GetInstance(provider), styles);
    }

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format and culture-specific format information. The format of the string representation must match the specified format exactly.</summary>
    /// <param name="s">A string that contains a date and time to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref name="s" />. For more information, see the Remarks section.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> or <paramref name="format" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    ///        <paramref name="s" /> or <paramref name="format" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> does not contain a date and time that corresponds to the pattern specified in <paramref name="format" />.
    /// 
    /// -or-
    /// 
    /// The hour component and the AM/PM designator in <paramref name="s" /> do not agree.</exception>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public static DateTime ParseExact(string s, string format, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if (format == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
      return DateTimeParse.ParseExact((ReadOnlySpan<char>) s, (ReadOnlySpan<char>) format, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None);
    }

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A string containing a date and time to convert.</param>
    /// <param name="format">A format specifier that defines the required format of <paramref name="s" />. For more information, see the Remarks section.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that provides additional information about <paramref name="s" />, about style elements that may be present in <paramref name="s" />, or about the conversion from <paramref name="s" /> to a <see cref="T:System.DateTime" /> value. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> or <paramref name="format" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    ///        <paramref name="s" /> or <paramref name="format" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> does not contain a date and time that corresponds to the pattern specified in <paramref name="format" />.
    /// 
    /// -or-
    /// 
    /// The hour component and the AM/PM designator in <paramref name="s" /> do not agree.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values. For example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />.</exception>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="format" />, <paramref name="provider" />, and <paramref name="style" />.</returns>
    public static DateTime ParseExact(
      string s,
      string format,
      IFormatProvider? provider,
      DateTimeStyles style)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      if (format == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.format);
      return DateTimeParse.ParseExact((ReadOnlySpan<char>) s, (ReadOnlySpan<char>) format, DateTimeFormatInfo.GetInstance(provider), style);
    }

    /// <summary>Converts the specified span representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the characters that represent a date and time to convert.</param>
    /// <param name="format">A span containing the characters that represent a format specifier that defines the required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of the enumeration values that provides additional information about <paramref name="s" />, about style elements that may be present in <paramref name="s" />, or about the conversion from <paramref name="s" /> to a <see cref="T:System.DateTime" /> value. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="format" />, <paramref name="provider" />, and <paramref name="style" />.</returns>
    public static DateTime ParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      return DateTimeParse.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style);
    }

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A string that contains a date and time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />. For more information, see the Remarks section.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> or <paramref name="formats" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    ///        <paramref name="s" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// an element of <paramref name="formats" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="s" /> does not contain a date and time that corresponds to any element of <paramref name="formats" />.
    /// 
    /// -or-
    /// 
    /// The hour component and the AM/PM designator in <paramref name="s" /> do not agree.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="style" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values. For example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />.</exception>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="formats" />, <paramref name="provider" />, and <paramref name="style" />.</returns>
    public static DateTime ParseExact(
      string s,
      string[] formats,
      IFormatProvider? provider,
      DateTimeStyles style)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return DateTimeParse.ParseExactMultiple((ReadOnlySpan<char>) s, formats, DateTimeFormatInfo.GetInstance(provider), style);
    }

    /// <summary>Converts the specified span representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
    /// <param name="s">A span containing the characters that represent a date and time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <returns>An object that is equivalent to the date and time contained in <paramref name="s" />, as specified by <paramref name="formats" />, <paramref name="provider" />, and <paramref name="style" />.</returns>
    public static DateTime ParseExact(
      ReadOnlySpan<char> s,
      string[] formats,
      IFormatProvider? provider,
      DateTimeStyles style = DateTimeStyles.None)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      return DateTimeParse.ParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style);
    }

    /// <summary>Returns a new <see cref="T:System.TimeSpan" /> that subtracts the specified date and time from the value of this instance.</summary>
    /// <param name="value">The date and time value to subtract.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The result is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>A time interval that is equal to the date and time represented by this instance minus the date and time represented by <paramref name="value" />.</returns>
    public TimeSpan Subtract(DateTime value) => new TimeSpan(this.Ticks - value.Ticks);

    /// <summary>Returns a new <see cref="T:System.DateTime" /> that subtracts the specified duration from the value of this instance.</summary>
    /// <param name="value">The time interval to subtract.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The result is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object that is equal to the date and time represented by this instance minus the time interval represented by <paramref name="value" />.</returns>
    public DateTime Subtract(TimeSpan value)
    {
      ulong num = (ulong) (this.Ticks - value._ticks);
      if (num > 3155378975999999999UL)
        DateTime.ThrowDateArithmetic(0);
      return new DateTime(num | this.InternalKind);
    }

    private static double TicksToOADate(long value)
    {
      if (value == 0L)
        return 0.0;
      if (value < 864000000000L)
        value += 599264352000000000L;
      if (value < 31241376000000000L)
        throw new OverflowException(SR.Arg_OleAutDateInvalid);
      long num1 = (value - 599264352000000000L) / 10000L;
      if (num1 < 0L)
      {
        long num2 = num1 % 86400000L;
        if (num2 != 0L)
          num1 -= (86400000L + num2) * 2L;
      }
      return (double) num1 / 86400000.0;
    }

    /// <summary>Converts the value of this instance to the equivalent OLE Automation date.</summary>
    /// <exception cref="T:System.OverflowException">The value of this instance cannot be represented as an OLE Automation Date.</exception>
    /// <returns>A double-precision floating-point number that contains an OLE Automation date equivalent to the value of this instance.</returns>
    public double ToOADate() => DateTime.TicksToOADate(this.Ticks);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to a Windows file time.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting file time would represent a date and time before 12:00 midnight January 1, 1601 C.E. UTC.</exception>
    /// <returns>The value of the current <see cref="T:System.DateTime" /> object expressed as a Windows file time.</returns>
    public long ToFileTime() => this.ToUniversalTime().ToFileTimeUtc();

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to a Windows file time.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting file time would represent a date and time before 12:00 midnight January 1, 1601 C.E. UTC.</exception>
    /// <returns>The value of the current <see cref="T:System.DateTime" /> object expressed as a Windows file time.</returns>
    public long ToFileTimeUtc()
    {
      long ticks = ((long) this._dateData & long.MinValue) != 0L ? this.ToUniversalTime().Ticks : this.Ticks;
      if (DateTime.s_systemSupportsLeapSeconds)
        return (long) DateTime.ToFileTimeLeapSecondsAware(ticks);
      long num = ticks - 504911232000000000L;
      return num >= 0L ? num : throw new ArgumentOutOfRangeException((string) null, SR.ArgumentOutOfRange_FileTimeInvalid);
    }

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to local time.</summary>
    /// <returns>An object whose <see cref="P:System.DateTime.Kind" /> property is <see cref="F:System.DateTimeKind.Local" />, and whose value is the local time equivalent to the value of the current <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MaxValue" /> if the converted value is too large to be represented by a <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MinValue" /> if the converted value is too small to be represented as a <see cref="T:System.DateTime" /> object.</returns>
    public DateTime ToLocalTime()
    {
      if (((long) this._dateData & long.MinValue) != 0L)
        return this;
      bool isAmbiguousLocalDst;
      long num = this.Ticks + TimeZoneInfo.GetUtcOffsetFromUtc(this, TimeZoneInfo.Local, out bool _, out isAmbiguousLocalDst).Ticks;
      if ((ulong) num > 3155378975999999999UL)
        return new DateTime(num < 0L ? 9223372036854775808UL : 12378751012854775807UL);
      return !isAmbiguousLocalDst ? new DateTime((ulong) (num | long.MinValue)) : new DateTime((ulong) (num | -4611686018427387904L));
    }

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent long date string representation.</summary>
    /// <returns>A string that contains the long date string representation of the current <see cref="T:System.DateTime" /> object.</returns>
    public string ToLongDateString() => DateTimeFormat.Format(this, "D", (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent long time string representation.</summary>
    /// <returns>A string that contains the long time string representation of the current <see cref="T:System.DateTime" /> object.</returns>
    public string ToLongTimeString() => DateTimeFormat.Format(this, "T", (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent short date string representation.</summary>
    /// <returns>A string that contains the short date string representation of the current <see cref="T:System.DateTime" /> object.</returns>
    public string ToShortDateString() => DateTimeFormat.Format(this, "d", (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent short time string representation.</summary>
    /// <returns>A string that contains the short time string representation of the current <see cref="T:System.DateTime" /> object.</returns>
    public string ToShortTimeString() => DateTimeFormat.Format(this, "t", (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation using the formatting conventions of the current culture.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by the current culture.</exception>
    /// <returns>A string representation of the value of the current <see cref="T:System.DateTime" /> object.</returns>
    public override string ToString() => DateTimeFormat.Format(this, (string) null, (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation using the specified format and the formatting conventions of the current culture.</summary>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <exception cref="T:System.FormatException">The length of <paramref name="format" /> is 1, and it is not one of the format specifier characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="format" /> does not contain a valid custom format pattern.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by the current culture.</exception>
    /// <returns>A string representation of value of the current <see cref="T:System.DateTime" /> object as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => DateTimeFormat.Format(this, format, (IFormatProvider) null);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by <paramref name="provider" />.</exception>
    /// <returns>A string representation of value of the current <see cref="T:System.DateTime" /> object as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => DateTimeFormat.Format(this, (string) null, provider);

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">The length of <paramref name="format" /> is 1, and it is not one of the format specifier characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="format" /> does not contain a valid custom format pattern.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by <paramref name="provider" />.</exception>
    /// <returns>A string representation of value of the current <see cref="T:System.DateTime" /> object as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => DateTimeFormat.Format(this, format, provider);

    /// <summary>Tries to format the value of the current datetime instance into the provided span of characters.</summary>
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
      return DateTimeFormat.TryFormat(this, destination, out charsWritten, format, provider);
    }

    /// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to Coordinated Universal Time (UTC).</summary>
    /// <returns>An object whose <see cref="P:System.DateTime.Kind" /> property is <see cref="F:System.DateTimeKind.Utc" />, and whose value is the UTC equivalent to the value of the current <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MaxValue" /> if the converted value is too large to be represented by a <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MinValue" /> if the converted value is too small to be represented by a <see cref="T:System.DateTime" /> object.</returns>
    public DateTime ToUniversalTime() => TimeZoneInfo.ConvertTimeToUtc(this, TimeZoneInfoOptions.NoThrowOnInvalidTime);

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a date and time to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" />, is an empty string (""), or does not contain a valid string representation of a date and time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out DateTime result)
    {
      if (s != null)
        return DateTimeParse.TryParse((ReadOnlySpan<char>) s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out result);
      result = new DateTime();
      return false;
    }

    /// <summary>Converts the specified char span of a date and time to its <see cref="T:System.DateTime" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a date and time to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" />, is an empty string (""), or does not contain a valid string representation of a date and time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out DateTime result) => DateTimeParse.TryParse(s, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out result);

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified culture-specific format information and formatting style, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a date and time to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="styles">A bitwise combination of enumeration values that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" />, is an empty string (""), or does not contain a valid string representation of a date and time. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="styles" /> is not a valid <see cref="T:System.Globalization.DateTimeStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values (for example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />).</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="provider" /> is a neutral culture and cannot be used in a parsing operation.</exception>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      IFormatProvider? provider,
      DateTimeStyles styles,
      out DateTime result)
    {
      DateTimeFormatInfo.ValidateStyles(styles, true);
      if (s != null)
        return DateTimeParse.TryParse((ReadOnlySpan<char>) s, DateTimeFormatInfo.GetInstance(provider), styles, out result);
      result = new DateTime();
      return false;
    }

    /// <summary>Converts the span representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified culture-specific format information and formatting style, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing the date and time to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="styles">A bitwise combination of enumeration values that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" />, is an empty string (""), or does not contain a valid string representation of a date and time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider? provider,
      DateTimeStyles styles,
      out DateTime result)
    {
      DateTimeFormatInfo.ValidateStyles(styles, true);
      return DateTimeParse.TryParse(s, DateTimeFormatInfo.GetInstance(provider), styles, out result);
    }

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string containing a date and time to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if either the <paramref name="s" /> or <paramref name="format" /> parameter is <see langword="null" />, is an empty string, or does not contain a date and time that correspond to the pattern specified in <paramref name="format" />. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="styles" /> is not a valid <see cref="T:System.Globalization.DateTimeStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values (for example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />).</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? s,
      [NotNullWhen(true)] string? format,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateTime result)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      if (s != null && format != null)
        return DateTimeParse.TryParseExact((ReadOnlySpan<char>) s, (ReadOnlySpan<char>) format, DateTimeFormatInfo.GetInstance(provider), style, out result);
      result = new DateTime();
      return false;
    }

    /// <summary>Converts the specified span representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A span containing the characters representing a date and time to convert.</param>
    /// <param name="format">The required format of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if either the <paramref name="s" /> or <paramref name="format" /> parameter is <see langword="null" />, is an empty string, or does not contain a date and time that correspond to the pattern specified in <paramref name="format" />. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      ReadOnlySpan<char> format,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateTime result)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      return DateTimeParse.TryParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), style, out result);
    }

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">A string that contains a date and time to convert.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> or <paramref name="formats" /> is <see langword="null" />, <paramref name="s" /> or an element of <paramref name="formats" /> is an empty string, or the format of <paramref name="s" /> is not exactly as specified by at least one of the format patterns in <paramref name="formats" />. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="styles" /> is not a valid <see cref="T:System.Globalization.DateTimeStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values (for example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />).</exception>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      [NotNullWhen(true)] string? s,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateTime result)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      if (s != null)
        return DateTimeParse.TryParseExactMultiple((ReadOnlySpan<char>) s, formats, DateTimeFormatInfo.GetInstance(provider), style, out result);
      result = new DateTime();
      return false;
    }

    /// <summary>Converts the specified char span of a date and time to its <see cref="T:System.DateTime" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="s">The span containing the string to parse.</param>
    /// <param name="formats">An array of allowable formats of <paramref name="s" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />.</param>
    /// <param name="style">A bitwise combination of enumeration values that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" />, is <see cref="F:System.String.Empty" />, or does not contain a valid string representation of a date and time. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="s" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> s,
      [NotNullWhen(true)] string?[]? formats,
      IFormatProvider? provider,
      DateTimeStyles style,
      out DateTime result)
    {
      DateTimeFormatInfo.ValidateStyles(style);
      return DateTimeParse.TryParseExactMultiple(s, formats, DateTimeFormatInfo.GetInstance(provider), style, out result);
    }

    /// <summary>Adds a specified time interval to a specified date and time, yielding a new date and time.</summary>
    /// <param name="d">The date and time value to add.</param>
    /// <param name="t">The time interval to add.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object that is the sum of the values of <paramref name="d" /> and <paramref name="t" />.</returns>
    public static DateTime operator +(DateTime d, TimeSpan t)
    {
      ulong num = (ulong) (d.Ticks + t._ticks);
      if (num > 3155378975999999999UL)
        DateTime.ThrowDateArithmetic(1);
      return new DateTime(num | d.InternalKind);
    }

    /// <summary>Subtracts a specified time interval from a specified date and time and returns a new date and time.</summary>
    /// <param name="d">The date and time value to subtract from.</param>
    /// <param name="t">The time interval to subtract.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.</exception>
    /// <returns>An object whose value is the value of <paramref name="d" /> minus the value of <paramref name="t" />.</returns>
    public static DateTime operator -(DateTime d, TimeSpan t)
    {
      ulong num = (ulong) (d.Ticks - t._ticks);
      if (num > 3155378975999999999UL)
        DateTime.ThrowDateArithmetic(1);
      return new DateTime(num | d.InternalKind);
    }

    /// <summary>Subtracts a specified date and time from another specified date and time and returns a time interval.</summary>
    /// <param name="d1">The date and time value to subtract from (the minuend).</param>
    /// <param name="d2">The date and time value to subtract (the subtrahend).</param>
    /// <returns>The time interval between <paramref name="d1" /> and <paramref name="d2" />; that is, <paramref name="d1" /> minus <paramref name="d2" />.</returns>
    public static TimeSpan operator -(DateTime d1, DateTime d2) => new TimeSpan(d1.Ticks - d2.Ticks);

    /// <summary>Determines whether two specified instances of <see cref="T:System.DateTime" /> are equal.</summary>
    /// <param name="d1">The first object to compare.</param>
    /// <param name="d2">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> and <paramref name="d2" /> represent the same date and time; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(DateTime d1, DateTime d2) => d1.Ticks == d2.Ticks;

    /// <summary>Determines whether two specified instances of <see cref="T:System.DateTime" /> are not equal.</summary>
    /// <param name="d1">The first object to compare.</param>
    /// <param name="d2">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> and <paramref name="d2" /> do not represent the same date and time; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(DateTime d1, DateTime d2) => d1.Ticks != d2.Ticks;

    /// <summary>Determines whether one specified <see cref="T:System.DateTime" /> is earlier than another specified <see cref="T:System.DateTime" />.</summary>
    /// <param name="t1">The first object to compare.</param>
    /// <param name="t2">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="t1" /> is earlier than <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <(DateTime t1, DateTime t2) => t1.Ticks < t2.Ticks;

    /// <summary>Determines whether one specified <see cref="T:System.DateTime" /> represents a date and time that is the same as or earlier than another specified <see cref="T:System.DateTime" />.</summary>
    /// <param name="t1">The first object to compare.</param>
    /// <param name="t2">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="t1" /> is the same as or earlier than <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <=(DateTime t1, DateTime t2) => t1.Ticks <= t2.Ticks;

    /// <summary>Determines whether one specified <see cref="T:System.DateTime" /> is later than another specified <see cref="T:System.DateTime" />.</summary>
    /// <param name="t1">The first object to compare.</param>
    /// <param name="t2">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="t1" /> is later than <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >(DateTime t1, DateTime t2) => t1.Ticks > t2.Ticks;

    /// <summary>Determines whether one specified <see cref="T:System.DateTime" /> represents a date and time that is the same as or later than another specified <see cref="T:System.DateTime" />.</summary>
    /// <param name="t1">The first object to compare.</param>
    /// <param name="t2">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="t1" /> is the same as or later than <paramref name="t2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >=(DateTime t1, DateTime t2) => t1.Ticks >= t2.Ticks;

    /// <summary>Converts the value of this instance to all the string representations supported by the standard date and time format specifiers.</summary>
    /// <returns>A string array where each element is the representation of the value of this instance formatted with one of the standard date and time format specifiers.</returns>
    public string[] GetDateTimeFormats() => this.GetDateTimeFormats((IFormatProvider) CultureInfo.CurrentCulture);

    /// <summary>Converts the value of this instance to all the string representations supported by the standard date and time format specifiers and the specified culture-specific formatting information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information about this instance.</param>
    /// <returns>A string array where each element is the representation of the value of this instance formatted with one of the standard date and time format specifiers.</returns>
    public string[] GetDateTimeFormats(IFormatProvider? provider) => DateTimeFormat.GetAllDateTimes(this, DateTimeFormatInfo.GetInstance(provider));

    /// <summary>Converts the value of this instance to all the string representations supported by the specified standard date and time format specifier.</summary>
    /// <param name="format">A standard date and time format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is not a valid standard date and time format specifier character.</exception>
    /// <returns>A string array where each element is the representation of the value of this instance formatted with the <paramref name="format" /> standard date and time format specifier.</returns>
    public string[] GetDateTimeFormats(char format) => this.GetDateTimeFormats(format, (IFormatProvider) CultureInfo.CurrentCulture);

    /// <summary>Converts the value of this instance to all the string representations supported by the specified standard date and time format specifier and culture-specific formatting information.</summary>
    /// <param name="format">A date and time format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about this instance.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is not a valid standard date and time format specifier character.</exception>
    /// <returns>A string array where each element is the representation of the value of this instance formatted with one of the standard date and time format specifiers.</returns>
    public string[] GetDateTimeFormats(char format, IFormatProvider? provider) => DateTimeFormat.GetAllDateTimes(this, format, DateTimeFormatInfo.GetInstance(provider));

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.DateTime" />.</summary>
    /// <returns>The enumerated constant, <see cref="F:System.TypeCode.DateTime" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.DateTime;


    #nullable disable
    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => throw DateTime.InvalidCast("Boolean");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => throw DateTime.InvalidCast("Char");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => throw DateTime.InvalidCast("SByte");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => throw DateTime.InvalidCast("Byte");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => throw DateTime.InvalidCast("Int16");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => throw DateTime.InvalidCast("UInt16");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => throw DateTime.InvalidCast("Int32");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => throw DateTime.InvalidCast("UInt32");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => throw DateTime.InvalidCast("Int64");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => throw DateTime.InvalidCast("UInt64");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => throw DateTime.InvalidCast("Single");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => throw DateTime.InvalidCast("Double");

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>The return value for this member is not used.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => throw DateTime.InvalidCast("Decimal");

    private static Exception InvalidCast(string to) => (Exception) new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (DateTime), (object) to));

    /// <summary>Returns the current <see cref="T:System.DateTime" /> object.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <returns>The current object.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => this;

    /// <summary>Converts the current <see cref="T:System.DateTime" /> object to an object of a specified type.</summary>
    /// <param name="type">The desired type.</param>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DateTime" /> type.</exception>
    /// <returns>An object of the type specified by the <paramref name="type" /> parameter, with a value equivalent to the current <see cref="T:System.DateTime" /> object.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    internal static bool TryCreate(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      out DateTime result)
    {
      result = new DateTime();
      if (year < 1 || year > 9999 || month < 1 || month > 12 || day < 1 || (uint) hour >= 24U || (uint) minute >= 60U || (uint) millisecond >= 1000U)
        return false;
      uint[] numArray = DateTime.IsLeapYear(year) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
      if ((uint) day > numArray[month] - numArray[month - 1])
        return false;
      ulong num = (ulong) (uint) ((int) DateTime.DaysToYear((uint) year) + (int) numArray[month - 1] + day - 1) * 864000000000UL;
      ulong dateData;
      if ((uint) second < 60U)
      {
        dateData = num + (DateTime.TimeToTicks(hour, minute, second) + (ulong) (uint) (millisecond * 10000));
      }
      else
      {
        if (second != 60 || !DateTime.s_systemSupportsLeapSeconds || !DateTime.IsValidTimeWithLeapSeconds(year, month, day, hour, minute, DateTimeKind.Unspecified))
          return false;
        dateData = num + (DateTime.TimeToTicks(hour, minute, 59) + 9990000UL);
      }
      result = new DateTime(dateData);
      return true;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateTime IAdditionOperators<DateTime, TimeSpan, DateTime>.op_Addition(
      DateTime left,
      TimeSpan right)
    {
      return left + right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan IAdditiveIdentity<DateTime, TimeSpan>.AdditiveIdentity => new TimeSpan();

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateTime, DateTime>.op_LessThan(
      DateTime left,
      DateTime right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateTime, DateTime>.op_LessThanOrEqual(
      DateTime left,
      DateTime right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateTime, DateTime>.op_GreaterThan(
      DateTime left,
      DateTime right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<DateTime, DateTime>.op_GreaterThanOrEqual(
      DateTime left,
      DateTime right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<DateTime, DateTime>.op_Equality(
      DateTime left,
      DateTime right)
    {
      return left == right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<DateTime, DateTime>.op_Inequality(
      DateTime left,
      DateTime right)
    {
      return left != right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateTime IMinMaxValue<DateTime>.MinValue => DateTime.MinValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateTime IMinMaxValue<DateTime>.MaxValue => DateTime.MaxValue;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateTime IParseable<DateTime>.Parse(
      string s,
      IFormatProvider provider)
    {
      return DateTime.Parse(s, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<DateTime>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out DateTime result)
    {
      return DateTime.TryParse(s, provider, DateTimeStyles.None, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateTime ISpanParseable<DateTime>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return DateTime.Parse(s, provider, DateTimeStyles.None);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<DateTime>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out DateTime result)
    {
      return DateTime.TryParse(s, provider, DateTimeStyles.None, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    DateTime ISubtractionOperators<DateTime, TimeSpan, DateTime>.op_Subtraction(
      DateTime left,
      TimeSpan right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    TimeSpan ISubtractionOperators<DateTime, DateTime, TimeSpan>.op_Subtraction(
      DateTime left,
      DateTime right)
    {
      return left - right;
    }

    private static unsafe bool SystemSupportsLeapSeconds()
    {
      Interop.NtDll.SYSTEM_LEAP_SECOND_INFORMATION secondInformation;
      return Interop.NtDll.NtQuerySystemInformation(206, (void*) &secondInformation, (uint) sizeof (Interop.NtDll.SYSTEM_LEAP_SECOND_INFORMATION), (uint*) null) == 0U && secondInformation.Enabled != 0;
    }

    /// <summary>Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).</summary>
    /// <returns>An object whose value is the current UTC date and time.</returns>
    public static unsafe DateTime UtcNow
    {
      get
      {
        ulong num1;
        __calli(DateTime.s_pfnGetSystemTimeAsFileTime)(&num1);
        ulong num2 = num1;
        if (!DateTime.s_systemSupportsLeapSeconds)
          return new DateTime(num2 + 5116597250427387904UL);
        DateTime.LeapSecondCache leapSecondCache = DateTime.s_leapSecondCache;
        ulong num3 = num2 - leapSecondCache.OSFileTimeTicksAtStartOfValidityWindow;
        return num3 < 3000000000UL ? new DateTime(leapSecondCache.DotnetDateDataAtStartOfValidityWindow + num3) : DateTime.UpdateLeapSecondCacheAndReturnUtcNow();
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static unsafe bool IsValidTimeWithLeapSeconds(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      DateTimeKind kind)
    {
      Interop.Kernel32.SYSTEMTIME systemtime1;
      systemtime1.Year = (ushort) year;
      systemtime1.Month = (ushort) month;
      systemtime1.DayOfWeek = (ushort) 0;
      systemtime1.Day = (ushort) day;
      systemtime1.Hour = (ushort) hour;
      systemtime1.Minute = (ushort) minute;
      systemtime1.Second = (ushort) 60;
      systemtime1.Milliseconds = (ushort) 0;
      Interop.Kernel32.SYSTEMTIME systemtime2;
      ulong num;
      return kind != DateTimeKind.Utc && Interop.Kernel32.TzSpecificLocalTimeToSystemTime(IntPtr.Zero, &systemtime1, &systemtime2) != Interop.BOOL.FALSE || kind != DateTimeKind.Local && Interop.Kernel32.SystemTimeToFileTime(&systemtime1, &num) != Interop.BOOL.FALSE;
    }

    private static unsafe DateTime FromFileTimeLeapSecondsAware(ulong fileTime)
    {
      Interop.Kernel32.SYSTEMTIME time;
      return Interop.Kernel32.FileTimeToSystemTime(&fileTime, &time) != Interop.BOOL.FALSE ? DateTime.CreateDateTimeFromSystemTime(in time, fileTime % 10000UL) : throw new ArgumentOutOfRangeException(nameof (fileTime), SR.ArgumentOutOfRange_DateTimeBadTicks);
    }

    private static unsafe ulong ToFileTimeLeapSecondsAware(long ticks)
    {
      DateTime dateTime = new DateTime(ticks);
      int year;
      int month;
      int day;
      dateTime.GetDate(out year, out month, out day);
      Interop.Kernel32.SYSTEMTIME systemtime;
      systemtime.Year = (ushort) year;
      systemtime.Month = (ushort) month;
      systemtime.DayOfWeek = (ushort) 0;
      systemtime.Day = (ushort) day;
      int hour;
      int minute;
      int second;
      int tick;
      dateTime.GetTimePrecise(out hour, out minute, out second, out tick);
      systemtime.Hour = (ushort) hour;
      systemtime.Minute = (ushort) minute;
      systemtime.Second = (ushort) second;
      systemtime.Milliseconds = (ushort) 0;
      ulong num;
      if (Interop.Kernel32.SystemTimeToFileTime(&systemtime, &num) == Interop.BOOL.FALSE)
        throw new ArgumentOutOfRangeException((string) null, SR.ArgumentOutOfRange_FileTimeInvalid);
      return num + (ulong) (uint) tick;
    }

    private static DateTime CreateDateTimeFromSystemTime(
      in Interop.Kernel32.SYSTEMTIME time,
      ulong hundredNanoSecond)
    {
      uint year = (uint) time.Year;
      uint[] numArray = DateTime.IsLeapYear((int) year) ? DateTime.s_daysToMonth366 : DateTime.s_daysToMonth365;
      int index = (int) time.Month - 1;
      ulong num1 = (ulong) (uint) ((int) DateTime.DaysToYear(year) + (int) numArray[index] + (int) time.Day - 1) * 864000000000UL + (ulong) time.Hour * 36000000000UL + (ulong) time.Minute * 600000000UL;
      uint second = (uint) time.Second;
      if (second > 59U)
        return new DateTime(num1 + 4611686019027387903UL);
      ulong num2 = (ulong) (uint) ((int) second * 10000000 + (int) time.Milliseconds * 10000) + hundredNanoSecond;
      return new DateTime((ulong) ((long) num1 + (long) num2 | 4611686018427387904L));
    }

    private static unsafe __FnPtr<void (ulong*)> GetGetSystemTimeAsFileTimeFnPtr()
    {
      IntPtr handle = Interop.Kernel32.LoadLibraryEx("kernel32.dll", IntPtr.Zero, 2048);
      IntPtr timeAsFileTimeFnPtr = NativeLibrary.GetExport(handle, "GetSystemTimeAsFileTime");
      IntPtr address;
      if (NativeLibrary.TryGetExport(handle, "GetSystemTimePreciseAsFileTime", out address))
      {
        for (int index = 0; index < 10; ++index)
        {
          long num1;
          // ISSUE: cast to a function pointer type
          // ISSUE: function pointer call
          __calli((__FnPtr<void (long*)>) (IntPtr) (void*) timeAsFileTimeFnPtr)(&num1);
          long num2;
          // ISSUE: cast to a function pointer type
          // ISSUE: function pointer call
          __calli((__FnPtr<void (long*)>) (IntPtr) (void*) address)(&num2);
          if (Math.Abs(num2 - num1) <= 1000000L)
          {
            timeAsFileTimeFnPtr = address;
            break;
          }
        }
      }
      // ISSUE: cast to a function pointer type
      return (__FnPtr<void (ulong*)>) (IntPtr) (void*) timeAsFileTimeFnPtr;
    }

    private static unsafe DateTime UpdateLeapSecondCacheAndReturnUtcNow()
    {
      ulong num1;
      // ISSUE: function pointer call
      __calli(DateTime.s_pfnGetSystemTimeAsFileTime)(&num1);
      ulong hundredNanoSecond = num1 % 10000UL;
      Interop.Kernel32.SYSTEMTIME time1;
      if (Interop.Kernel32.FileTimeToSystemTime(&num1, &time1) == Interop.BOOL.FALSE)
        return LowGranularityNonCachedFallback();
      if (time1.Second >= (ushort) 60)
        return DateTime.CreateDateTimeFromSystemTime(in time1, hundredNanoSecond);
      Interop.Kernel32.SYSTEMTIME systemtime;
      if (Interop.Kernel32.FileTimeToSystemTime(&(num1 + 3000000000UL), &systemtime) == Interop.BOOL.FALSE)
        return LowGranularityNonCachedFallback();
      ulong num2;
      ulong num3;
      if ((int) systemtime.Second == (int) time1.Second)
      {
        num2 = num1;
        num3 = DateTime.CreateDateTimeFromSystemTime(in time1, hundredNanoSecond)._dateData;
      }
      else
      {
        Interop.Kernel32.SYSTEMTIME time2 = time1 with
        {
          Hour = 0,
          Minute = 0,
          Second = 0,
          Milliseconds = 0
        };
        ulong num4;
        if (Interop.Kernel32.SystemTimeToFileTime(&time2, &num4) == Interop.BOOL.FALSE)
          return LowGranularityNonCachedFallback();
        num2 = num4 + 863990000000UL - 3000000000UL;
        if (num1 - num2 >= 3000000000UL)
          return DateTime.CreateDateTimeFromSystemTime(in time1, hundredNanoSecond);
        num3 = DateTime.CreateDateTimeFromSystemTime(in time2, 0UL)._dateData + 863990000000UL - 3000000000UL;
      }
      Volatile.Write<DateTime.LeapSecondCache>(ref DateTime.s_leapSecondCache, new DateTime.LeapSecondCache()
      {
        OSFileTimeTicksAtStartOfValidityWindow = num2,
        DotnetDateDataAtStartOfValidityWindow = num3
      });
      return new DateTime(num3 + num1 - num2);

      [MethodImpl(MethodImplOptions.NoInlining)]
      static unsafe DateTime LowGranularityNonCachedFallback()
      {
        Interop.Kernel32.SYSTEMTIME time;
        Interop.Kernel32.GetSystemTime(&time);
        return DateTime.CreateDateTimeFromSystemTime(in time, 0UL);
      }
    }

    private sealed class LeapSecondCache
    {
      internal ulong OSFileTimeTicksAtStartOfValidityWindow;
      internal ulong DotnetDateDataAtStartOfValidityWindow;
    }
  }
}
