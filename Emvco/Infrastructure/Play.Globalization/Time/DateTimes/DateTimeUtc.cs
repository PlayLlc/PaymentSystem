using System;

using Play.Core.Exceptions;
using Play.Globalization.Time.Seconds;

namespace Play.Globalization.Time;

public readonly record struct DateTimeUtc
{
    #region Instance Values

    private readonly DateTime _Value;
    public int Year => _Value.Year;
    public int Month => _Value.Year;
    public int Day => _Value.Year;
    public int Hour => _Value.Hour;
    public int Minute => _Value.Minute;
    public int Second => _Value.Second;
    public static DateTimeUtc Now => new(DateTime.UtcNow.Date);
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

    /// <exception cref="PlayInternalException"></exception>
    public DateTimeUtc(long value)
    {
        DateTime dateTimeValue = new(value);

        if (dateTimeValue.Kind != DateTimeKind.Utc)
            throw new PlayInternalException(new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format"));

        _Value = DateTime.UtcNow;
    }

    /// <exception cref="PlayInternalException"></exception>
    public DateTimeUtc(int value)
    {
        DateTime dateTimeValue = new(value);

        if (dateTimeValue.Kind != DateTimeKind.Utc)
            throw new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format");

        _Value = DateTime.UtcNow;
    }

    #endregion

    #region Equality

    public bool Equals(DateTime dateTime) => dateTime == _Value;
    public int CompareTo(DateTimeUtc? other) => _Value.CompareTo(other);

    #endregion

    #region Operator Overrides

    public static bool operator <(DateTimeUtc left, DateTimeUtc right) => left._Value < right._Value;
    public static bool operator >(DateTimeUtc left, DateTimeUtc right) => left._Value > right._Value;
    public static bool operator <=(DateTimeUtc left, DateTimeUtc right) => left._Value <= right._Value;
    public static bool operator >=(DateTimeUtc left, DateTimeUtc right) => left._Value >= right._Value;
    public static Ticks operator -(DateTimeUtc left, DateTimeUtc right) => left._Value - right._Value;
    public static implicit operator DateTime(DateTimeUtc value) => value._Value;

    #endregion
}