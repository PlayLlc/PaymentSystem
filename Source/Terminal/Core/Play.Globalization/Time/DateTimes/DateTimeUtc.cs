using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

using Play.Core;
using Play.Core.Exceptions;
using Play.Globalization.Currency;
using Play.Globalization.Extensions;

namespace Play.Globalization.Time;

public readonly record struct DateTimeUtc
{
    #region Instance Values

    private readonly DateTime _Value;
    public int Year => _Value.Year;
    public int Month => _Value.Month;
    public int Day => _Value.Day;
    public int Hour => _Value.Hour;
    public int Minute => _Value.Minute;
    public int Second => _Value.Second;
    public long Ticks => _Value.Ticks;
    public static DateTimeUtc Now => new(DateTime.UtcNow);
    public static DateTimeUtc Today => new(DateTime.UtcNow);

    #endregion

    #region Constructor

    /// <exception cref="PlayInternalException"></exception>
    public DateTimeUtc(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
            throw new PlayInternalException(new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format"));

        _Value = value;
    }

    public DateTimeUtc(int year, int month, int day)
    {
        _Value = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
    }

    public DateTimeUtc(int year, Months month, DaysOfTheWeek day)
    {
        _Value = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
    }

    /// <exception cref="PlayInternalException"></exception>
    public DateTimeUtc(long value)
    {
        DateTime dateTimeValue = new(value, DateTimeKind.Utc);

        if (dateTimeValue.Kind != DateTimeKind.Utc)
            throw new PlayInternalException(new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format"));

        _Value = dateTimeValue;
    }

    /// <exception cref="PlayInternalException"></exception>
    public DateTimeUtc(int value)
    {
        DateTime dateTimeValue = new(value, DateTimeKind.Utc);

        if (dateTimeValue.Kind != DateTimeKind.Utc)
            throw new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format");

        _Value = dateTimeValue;
    }

    #endregion

    #region Instance Members

    public ShortDate AsShortDate() => new(this);

    public DateTimeUtc GetLast(DaysOfTheWeek dayOfTheWeek)
    {
        DayOfWeek dayOfWeek = GetDayOfTheWeek();

        if (dayOfWeek == 0)
            return AddDays(-7);

        if (dayOfWeek < dayOfTheWeek)
            return AddDays(-(dayOfWeek - dayOfTheWeek));

        return AddDays(7 - (dayOfTheWeek - dayOfWeek));
    }

    public DateTimeUtc GetNext(DaysOfTheWeek dayOfTheWeek)
    {
        DayOfWeek dayOfWeek = GetDayOfTheWeek();

        if (dayOfWeek == 0)
            return AddDays(7);

        if (dayOfWeek < dayOfTheWeek)
            return AddDays(dayOfWeek - dayOfTheWeek);

        return AddDays(dayOfTheWeek - dayOfWeek);
    }

    public DateTimeUtc GetLast(DaysOfTheMonth dayOfTheMonth)
    {
        int daysInLastMonth = DateTime.DaysInMonth(_Value.Year, _Value.Month - 1);
        int lastMonthDaysTillTarget = daysInLastMonth - dayOfTheMonth;

        DaysOfTheMonth currentDayOfTheMonth = GetDayOfTheMonth();

        if (currentDayOfTheMonth == dayOfTheMonth)
            return AddDays(-(lastMonthDaysTillTarget + dayOfTheMonth));

        if (currentDayOfTheMonth < dayOfTheMonth)
            return AddDays(-((lastMonthDaysTillTarget + currentDayOfTheMonth) - dayOfTheMonth));

        return AddDays(-((lastMonthDaysTillTarget + dayOfTheMonth) - currentDayOfTheMonth));
    }

    public DateTimeUtc GetNext(DaysOfTheMonth dayOfTheMonth)
    {
        int daysInThisMonth = DateTime.DaysInMonth(_Value.Year, _Value.Month);

        DaysOfTheMonth currentDayOfTheMonth = GetDayOfTheMonth();

        if (currentDayOfTheMonth == dayOfTheMonth)
            return AddDays((daysInThisMonth - currentDayOfTheMonth) + dayOfTheMonth);

        if (currentDayOfTheMonth < dayOfTheMonth)
            return AddDays((daysInThisMonth - currentDayOfTheMonth) + (currentDayOfTheMonth - dayOfTheMonth));

        return AddDays((daysInThisMonth - currentDayOfTheMonth) + (dayOfTheMonth - currentDayOfTheMonth));
    }

    public DayOfWeek GetDayOfTheWeek() => _Value.DayOfWeek;

    public DaysOfTheMonth GetDayOfTheMonth()
    {
        DaysOfTheMonth.Empty.TryGet((byte) _Value.Day, out EnumObject<byte>? result);

        return (DaysOfTheMonth) result!;
    }

    public string ToShortDateFormat() => $"{_Value.Year}-{_Value.Month}-{_Value.Day:00}";
    public string ToString(string format) => _Value.ToString(format);
    public DateTimeUtc AddDays(int days) => new(_Value.AddDays(days));
    public TimeSpan Subtract(DateTimeUtc value) => value._Value.Subtract(_Value);

    #endregion

    #region Equality

    public bool Equals(DateTime dateTime) => dateTime == _Value;
    public int CompareTo(DateTime other) => _Value.CompareTo(other);

    #endregion

    #region Operator Overrides

    public static bool operator <(DateTimeUtc left, DateTimeUtc right) => left._Value < right._Value;
    public static bool operator >(DateTimeUtc left, DateTimeUtc right) => left._Value > right._Value;
    public static bool operator <=(DateTimeUtc left, DateTimeUtc right) => left._Value <= right._Value;
    public static bool operator >=(DateTimeUtc left, DateTimeUtc right) => left._Value >= right._Value;
    public static TimeSpan operator -(DateTimeUtc left, DateTimeUtc right) => left._Value - right._Value;
    public static implicit operator DateTime(DateTimeUtc value) => value._Value;

    #endregion
}