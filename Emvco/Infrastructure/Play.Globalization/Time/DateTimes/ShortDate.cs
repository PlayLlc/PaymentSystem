using System;

using Play.Core;
using Play.Core.Exceptions;
using Play.Core.Extensions;

namespace Play.Globalization.Time;

// TODO: This is actually super unwieldy to use. Let's add some more constructors
/// <summary>
///     Supports short date values in the formats YYMMDD or YYMM
/// </summary>
/// <remarks>
///     All internal DateTime values are UTC
/// </remarks>
public readonly struct ShortDate
{
    #region Static Metadata

    private static readonly int _MillenniumAndCentury = (byte) (DateTime.Now.Year / 100);
    public static readonly ShortDate Min = new(1901);
    private const byte _YyMmDdLength = 6;
    private const byte _YyMmLength = 4;

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
        if (value.GetNumberOfDigits() != _YyMmLength)
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was not in a correct format. The argument must be either {_YyMmDdLength} or {_YyMmLength} digits in length. Only YyMmDd and YyMm formats are supported"));
        }

        _Value = new DateTimeUtc(new DateTime(GetYear(value), GetMonth(value), GetDay(value)));
        ;
    }

    /// <exception cref="PlayInternalException"></exception>
    public ShortDate(uint value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if ((numberOfDigits != _YyMmLength) && (numberOfDigits != _YyMmDdLength))
        {
            throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was not in a correct format. The argument must be either {_YyMmDdLength} or {_YyMmLength} digits in length. Only YyMmDd and YyMm formats are supported"));
        }

        _Value = new DateTimeUtc(new DateTime(GetYear(value), GetMonth(value), GetDay(value), 0, 0, 0, 0, DateTimeKind.Utc));
    }

    /// <exception cref="PlayInternalException"></exception>
    public ShortDate(DateTimeUtc dateTime)
    {
        _Value = new DateTimeUtc(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, DateTimeKind.Utc));
    }

    /// <exception cref="PlayInternalException"></exception>
    public ShortDate(ReadOnlySpan<Nibble> value)
    {
        byte year;
        byte month;

        if (value.Length == 4)
        {
            year = (byte) (value[0] * 10);
            year += value[1];
            month = (byte) (value[2] * 10);
            month += value[3];

            _Value = new DateTimeUtc(new DateTime(year, month, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        if (value.Length == 6)
        {
            year = (byte) (value[0] * 10);
            year += value[1];
            month = (byte) (value[2] * 10);
            month += value[3];
            byte day = (byte) (value[4] * 10);
            day += value[5];

            _Value = new DateTimeUtc(new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        throw new PlayInternalException(new ArgumentOutOfRangeException(nameof(value),
            $"The {nameof(ShortDate)} could not be initialized because the value was out of range"));
    }

    #endregion

    #region Instance Members

    public Nibble[] AsNibbleArray()
    {
        Nibble[] result = new Nibble[6];

        result[0] = (Nibble) (_Value.Year / 1000);
        result[1] = (Nibble) ((_Value.Year / 100) % 10);

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

        if (_Value.Day <= 9)
        {
            result[4] = 0;
            result[5] = (Nibble) _Value.Day;
        }
        else
        {
            result[4] = (Nibble) (_Value.Day / 10);
            result[5] = (Nibble) (_Value.Day % 10);
        }

        return result;
    }

    public ushort AsYyMm() => (ushort) (((byte) (_Value.Year % 100) * 100) + _Value.Month);
    public uint AsYyMmDd() => (uint) (((_Value.Year % 100) * 10000) + (_Value.Month * 100) + _Value.Day);
    private static byte GetDay(uint value) => value.GetNumberOfDigits() == 6 ? GetDayYyMmDd(value) : GetDayYyMm(value);
    private static byte GetDayYyMm(uint value) => 1;
    private static byte GetDayYyMmDd(uint value) => (byte) (value % 100);
    private static byte GetMonth(uint value) => value.GetNumberOfDigits() == 6 ? GetMonthYyMmDd(value) : GetMonthYyMm(value);
    private static byte GetMonthYyMm(uint value) => (byte) (value % 100);
    private static byte GetMonthYyMmDd(uint value) => (byte) ((value / 100) % 100);
    private static int GetYear(uint value) => value.GetNumberOfDigits() == 6 ? GetYearYyMmDd(value) : GetYearYyMm(value);
    private static byte GetYearYyMm(uint value) => (byte) ((_MillenniumAndCentury * 100) + (value / 100));
    private static byte GetYearYyMmDd(uint value) => (byte) ((_MillenniumAndCentury * 100) + (value / 10000));

    #endregion

    #region Equality

    public bool Equals(ShortDate other) => _Value == other._Value;
    public bool Equals(DateTime other) => _Value == other;
    public bool Equals(ShortDate x, ShortDate y) => x.Equals(y);
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