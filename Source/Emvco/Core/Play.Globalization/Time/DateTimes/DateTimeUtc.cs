using System;

using Play.Core.Exceptions;

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

    #region Equality

    public bool Equals(DateTime dateTime) => dateTime == _Value;
    public int CompareTo(DateTime other) => _Value.CompareTo(other);

    #endregion

    #region Operator Overrides

    public static bool operator <(DateTimeUtc left, DateTimeUtc right) => left._Value < right._Value;
    public static bool operator >(DateTimeUtc left, DateTimeUtc right) => left._Value > right._Value;
    public static bool operator <=(DateTimeUtc left, DateTimeUtc right) => left._Value <= right._Value;
    public static bool operator >=(DateTimeUtc left, DateTimeUtc right) => left._Value >= right._Value;
    public static Ticks operator -(DateTimeUtc left, DateTimeUtc right) => (Ticks) (left._Value - right._Value);
    public static implicit operator DateTime(DateTimeUtc value) => value._Value;

    #endregion

    #region Instance Members

    public string ToString(string format) => _Value.ToString(format);

    #endregion
}