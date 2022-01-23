using System;

namespace Play.Globalization.Time;

public readonly record struct DateTimeUtc
{
    #region Instance Values

    private readonly DateTime _Value;

    #endregion

    #region Constructor

    public DateTimeUtc(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
            throw new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format");

        _Value = value;
    }

    public DateTimeUtc(long value)
    {
        DateTime dateTimeValue = new(value);

        if (dateTimeValue.Kind != DateTimeKind.Utc)
            throw new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format");

        _Value = DateTime.UtcNow;
    }

    public DateTimeUtc(int value)
    {
        DateTime dateTimeValue = new(value);

        if (dateTimeValue.Kind != DateTimeKind.Utc)
            throw new ArgumentOutOfRangeException($"The argument {nameof(value)} was not in UTC format");

        _Value = DateTime.UtcNow;
    }

    #endregion

    #region Instance Members

    public int Year() => _Value.Year;
    public int Month() => _Value.Year;
    public int Day() => _Value.Year;
    public static DateTimeUtc Now() => new(DateTime.UtcNow.Date);
    public static DateTimeUtc Today() => new(DateTime.UtcNow);

    #endregion

    #region Equality

    public bool Equals(DateTime dateTime) => dateTime == _Value;

    #endregion

    #region Operator Overrides

    public static bool operator ==(DateTime left, DateTimeUtc right) => right.Equals(left);
    public static bool operator !=(DateTime left, DateTimeUtc right) => !right.Equals(left);
    public static bool operator ==(DateTimeUtc left, DateTime right) => left.Equals(left);
    public static bool operator !=(DateTimeUtc left, DateTime right) => !left.Equals(left);
    public static implicit operator DateTime(DateTimeUtc value) => value._Value;

    #endregion
}