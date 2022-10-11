// Decompiled with JetBrains decompiler
// Type: System.TimeZone
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Globalization;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Represents a time zone.</summary>
  [Obsolete("System.TimeZone has been deprecated. Investigate the use of System.TimeZoneInfo instead.")]
  public abstract class TimeZone
  {

    #nullable disable
    private static volatile TimeZone currentTimeZone;
    private static object s_InternalSyncObject;


    #nullable enable
    private static object InternalSyncObject
    {
      get
      {
        if (TimeZone.s_InternalSyncObject == null)
        {
          object obj = new object();
          Interlocked.CompareExchange<object>(ref TimeZone.s_InternalSyncObject, obj, (object) null);
        }
        return TimeZone.s_InternalSyncObject;
      }
    }

    /// <summary>Gets the time zone of the current computer.</summary>
    /// <returns>A <see cref="T:System.TimeZone" /> object that represents the current local time zone.</returns>
    public static TimeZone CurrentTimeZone
    {
      get
      {
        TimeZone currentTimeZone = TimeZone.currentTimeZone;
        if (currentTimeZone == null)
        {
          lock (TimeZone.InternalSyncObject)
          {
            if (TimeZone.currentTimeZone == null)
              TimeZone.currentTimeZone = (TimeZone) new CurrentSystemTimeZone();
            currentTimeZone = TimeZone.currentTimeZone;
          }
        }
        return currentTimeZone;
      }
    }

    internal static void ResetTimeZone()
    {
      if (TimeZone.currentTimeZone == null)
        return;
      lock (TimeZone.InternalSyncObject)
        TimeZone.currentTimeZone = (TimeZone) null;
    }

    /// <summary>Gets the standard time zone name.</summary>
    /// <exception cref="T:System.ArgumentNullException">An attempt was made to set this property to <see langword="null" />.</exception>
    /// <returns>The standard time zone name.</returns>
    public abstract string StandardName { get; }

    /// <summary>Gets the daylight saving time zone name.</summary>
    /// <returns>The daylight saving time zone name.</returns>
    public abstract string DaylightName { get; }

    /// <summary>Returns the Coordinated Universal Time (UTC) offset for the specified local time.</summary>
    /// <param name="time">A date and time value.</param>
    /// <returns>The Coordinated Universal Time (UTC) offset from <paramref name="time" />.</returns>
    public abstract TimeSpan GetUtcOffset(DateTime time);

    /// <summary>Returns the Coordinated Universal Time (UTC) that corresponds to a specified time.</summary>
    /// <param name="time">A date and time.</param>
    /// <returns>A <see cref="T:System.DateTime" /> object whose value is the Coordinated Universal Time (UTC) that corresponds to <paramref name="time" />.</returns>
    public virtual DateTime ToUniversalTime(DateTime time)
    {
      if (time.Kind == DateTimeKind.Utc)
        return time;
      long ticks = time.Ticks - this.GetUtcOffset(time).Ticks;
      if (ticks > 3155378975999999999L)
        return new DateTime(3155378975999999999L, DateTimeKind.Utc);
      return ticks < 0L ? new DateTime(0L, DateTimeKind.Utc) : new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>Returns the local time that corresponds to a specified date and time value.</summary>
    /// <param name="time">A Coordinated Universal Time (UTC) time.</param>
    /// <returns>A <see cref="T:System.DateTime" /> object whose value is the local time that corresponds to <paramref name="time" />.</returns>
    public virtual DateTime ToLocalTime(DateTime time)
    {
      if (time.Kind == DateTimeKind.Local)
        return time;
      bool isAmbiguousLocalDst = false;
      long fromUniversalTime = ((CurrentSystemTimeZone) TimeZone.CurrentTimeZone).GetUtcOffsetFromUniversalTime(time, ref isAmbiguousLocalDst);
      return new DateTime(time.Ticks + fromUniversalTime, DateTimeKind.Local, isAmbiguousLocalDst);
    }

    /// <summary>Returns the daylight saving time period for a particular year.</summary>
    /// <param name="year">The year that the daylight saving time period applies to.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="year" /> is less than 1 or greater than 9999.</exception>
    /// <returns>A <see cref="T:System.Globalization.DaylightTime" /> object that contains the start and end date for daylight saving time in <paramref name="year" />.</returns>
    public abstract DaylightTime GetDaylightChanges(int year);

    /// <summary>Returns a value indicating whether the specified date and time is within a daylight saving time period.</summary>
    /// <param name="time">A date and time.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="time" /> is in a daylight saving time period; otherwise, <see langword="false" />.</returns>
    public virtual bool IsDaylightSavingTime(DateTime time) => TimeZone.IsDaylightSavingTime(time, this.GetDaylightChanges(time.Year));

    /// <summary>Returns a value indicating whether the specified date and time is within the specified daylight saving time period.</summary>
    /// <param name="time">A date and time.</param>
    /// <param name="daylightTimes">A daylight saving time period.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="daylightTimes" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="time" /> is in <paramref name="daylightTimes" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDaylightSavingTime(DateTime time, DaylightTime daylightTimes) => TimeZone.CalculateUtcOffset(time, daylightTimes) != TimeSpan.Zero;


    #nullable disable
    internal static TimeSpan CalculateUtcOffset(DateTime time, DaylightTime daylightTimes)
    {
      if (daylightTimes == null || time.Kind == DateTimeKind.Utc)
        return TimeSpan.Zero;
      DateTime dateTime1 = daylightTimes.Start + daylightTimes.Delta;
      DateTime end = daylightTimes.End;
      DateTime dateTime2;
      DateTime dateTime3;
      if (daylightTimes.Delta.Ticks > 0L)
      {
        dateTime2 = end - daylightTimes.Delta;
        dateTime3 = end;
      }
      else
      {
        dateTime2 = dateTime1;
        dateTime3 = dateTime1 - daylightTimes.Delta;
      }
      bool flag = false;
      if (dateTime1 > end)
      {
        if (time >= dateTime1 || time < end)
          flag = true;
      }
      else if (time >= dateTime1 && time < end)
        flag = true;
      if (flag && time >= dateTime2 && time < dateTime3)
        flag = time.IsAmbiguousDaylightSavingTime();
      return flag ? daylightTimes.Delta : TimeSpan.Zero;
    }
  }
}
