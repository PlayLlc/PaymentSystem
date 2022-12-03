using System;

using Play.Core;
using Play.Core.Exceptions;

namespace Play.Globalization.Time;

// TODO: This is actually super unwieldy to use. Let's add some more constructors
/// <summary>
///     Supports short date values in the format YYMM. The day value will always be the first of the month
/// </summary>
/// <remarks>
///     All internal DateTime values are UTC
/// </remarks>
public readonly struct ShortDate
{
    #region Static Metadata

    private static readonly int _MillenniumAndCentury = (byte) (DateTime.Now.Year / 100) * 100;
    public static readonly ShortDate Min = new(0001);

    #endregion

    #region Instance Values

    private readonly DateTimeUtc _Value;
    public ShortDate Now => new(DateTimeUtc.Now);
    public static ShortDate Today => new(DateTimeUtc.Today);
    public DateTimeUtc AsDateTimeUtc => _Value;

    #endregion

    #region Constructor

    /// <summary>
    ///     Constructor for a YYMM Short Date Value
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="PlayInternalException"></exception>
    public ShortDate(ushort value)
    {
        int month = GetMonth(value);

        if (month > 12)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(ShortDate)} could not be initialized because the value was out of range"));
        }

        int year = GetYear(value);

        if (year > 99)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(ShortDate)} could not be initialized because the value was out of range"));
        }

        _Value = new(new DateTime(year + _MillenniumAndCentury, month, 1, 0, 0, 0, 0, DateTimeKind.Utc));

        if (_Value < Min)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument was out of range. The minimum {nameof(ShortDate)} value is {Min.AsYyMm()}"));
        }
    }

    /// <exception cref="PlayInternalException"></exception>
    public ShortDate(DateTimeUtc dateTime)
    {
        _Value = new(new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc));
    }

    /// <exception cref="PlayInternalException"></exception>
    public ShortDate(ReadOnlySpan<Nibble> value)
    {
        byte month = (byte) (value[2] * 10);
        month += value[3];

        if (month > 12)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(ShortDate)} could not be initialized because the value was out of range"));
        }

        byte year = (byte) (value[0] * 10);
        year += value[1];

        if (year > 99)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The {nameof(ShortDate)} could not be initialized because the value was out of range"));
        }

        _Value = new(new DateTime(year + _MillenniumAndCentury, month, 1, 0, 0, 0, 0, DateTimeKind.Utc));

        if (_Value < Min)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument was out of range. The minimum {nameof(ShortDate)} value is {Min.AsYyMm()}"));
        }
    }

    #endregion

    #region Instance Members

    private static int GetYear(int value) => value / 100;
    private static int GetMonth(int value) => value % 100;

    public Nibble[] AsNibbleArray()
    {
        Nibble[] result = new Nibble[4];

        result[0] = (Nibble) ((_Value.Year % 100) / 10);
        result[1] = (Nibble) (_Value.Year % 100 % 10);

        if (_Value.Month <= 9)
        {
            result[2] = 0;
            result[3] = (Nibble) _Value.Month;
        }
        else
        {
            result[2] = (Nibble) (_Value.Month / 10);
            result[3] = (Nibble) (_Value.Month % 10);
        }

        return result;
    }

    public ushort AsYyMm() => (ushort) (((byte) (_Value.Year % 100) * 100) + _Value.Month);

    #endregion

    #region Equality

    public bool Equals(ShortDate other) => _Value == other._Value;
    public bool Equals(DateTime other) => _Value == other;
    public static bool Equals(ShortDate x, ShortDate y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is ShortDate shortDateValueYyMm && Equals(shortDateValueYyMm);
    public int GetHashCode(ShortDate obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(27329 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator >(ShortDate x, DateTimeUtc y) => x._Value > y;
    public static bool operator <(ShortDate x, DateTimeUtc y) => x._Value < y;
    public static bool operator >(DateTimeUtc x, ShortDate y) => x > y._Value;
    public static bool operator <(DateTimeUtc x, ShortDate y) => x < y._Value;
    public static bool operator ==(ShortDate x, ShortDate y) => x.Equals(y);
    public static bool operator ==(ShortDate x, DateTime y) => x.Equals(y);
    public static bool operator ==(DateTime x, ShortDate y) => y.Equals(x);
    public static bool operator >(ShortDate x, ShortDate y) => x._Value > y._Value;
    public static bool operator >(ShortDate x, DateTime y) => x._Value > y;
    public static bool operator >(DateTime x, ShortDate y) => x > y._Value;
    public static bool operator >=(ShortDate x, ShortDate y) => x._Value >= y._Value;
    public static bool operator >=(ShortDate x, DateTime y) => x._Value >= y;
    public static bool operator >=(DateTime x, ShortDate y) => x >= y._Value;
    public static bool operator !=(ShortDate x, ShortDate y) => !x.Equals(y);
    public static bool operator !=(ShortDate x, DateTime y) => !x.Equals(y);
    public static bool operator !=(DateTime x, ShortDate y) => !y.Equals(x);
    public static bool operator <(ShortDate x, ShortDate y) => x._Value < y._Value;
    public static bool operator <(ShortDate x, DateTime y) => x._Value < y;
    public static bool operator <(DateTime x, ShortDate y) => x < y._Value;
    public static bool operator <=(ShortDate x, ShortDate y) => x._Value <= y._Value;
    public static bool operator <=(ShortDate x, DateTime y) => x._Value <= y;
    public static bool operator <=(DateTime x, ShortDate y) => x <= y._Value;
    public static implicit operator DateTime(ShortDate value) => value._Value;

    #endregion
}