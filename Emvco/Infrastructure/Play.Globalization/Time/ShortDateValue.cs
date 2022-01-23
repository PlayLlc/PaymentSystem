﻿using System;

using Play.Core.Extensions;

namespace Play.Globalization.Time;

/// <summary>
///     Supports short date values in the formats YYMMDD or YYMM
/// </summary>
/// <remarks>
///     All internal DateTime values are UTC
/// </remarks>
public readonly struct ShortDateValue
{
    #region Static Metadata

    private static readonly int _MillenniumAndCentury = (byte) (DateTime.Now.Year / 100);
    public static readonly ShortDateValue MinValue = new(1901);
    private const byte _YyMmDdLength = 6;
    private const byte _YyMmLength = 4;

    #endregion

    #region Instance Values

    private readonly DateTime _Value;

    #endregion

    #region Constructor

    /// <summary>
    ///     Constructor for a YYMM Short Date Value
    /// </summary>
    /// <param name="value"></param>
    public ShortDateValue(ushort value)
    {
        if (value.GetNumberOfDigits() != _YyMmLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was not in a correct format. The argument must be either {_YyMmDdLength} or {_YyMmLength} digits in length. Only YyMmDd and YyMm formats are supported");
        }

        _Value = new DateTime(GetYear(value), GetMonth(value), GetDay(value));
        ;
    }

    public ShortDateValue(uint value)
    {
        byte numberOfDigits = value.GetNumberOfDigits();

        if ((numberOfDigits != _YyMmLength) && (numberOfDigits != _YyMmDdLength))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} was not in a correct format. The argument must be either {_YyMmDdLength} or {_YyMmLength} digits in length. Only YyMmDd and YyMm formats are supported");
        }

        _Value = new DateTime(GetYear(value), GetMonth(value), GetDay(value), 0, 0, 0, 0, DateTimeKind.Utc);
    }

    #endregion

    #region Instance Members

    public DateTime AsDateTimeUtc() => _Value;
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

    public bool Equals(ShortDateValue other) => _Value == other._Value;
    public bool Equals(DateTime other) => _Value == other;
    public bool Equals(ShortDateValue x, ShortDateValue y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is ShortDateValue shortDateValueYyMm && Equals(shortDateValueYyMm);
    public int GetHashCode(ShortDateValue obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(27329 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(ShortDateValue x, ShortDateValue y) => x.Equals(y);
    public static bool operator ==(ShortDateValue x, DateTime y) => x.Equals(y);
    public static bool operator ==(DateTime x, ShortDateValue y) => y.Equals(x);
    public static bool operator >(ShortDateValue x, ShortDateValue y) => x._Value > y._Value;
    public static bool operator >(ShortDateValue x, DateTime y) => x._Value > y;
    public static bool operator >(DateTime x, ShortDateValue y) => x > y._Value;
    public static bool operator >=(ShortDateValue x, ShortDateValue y) => x._Value >= y._Value;
    public static bool operator >=(ShortDateValue x, DateTime y) => x._Value >= y;
    public static bool operator >=(DateTime x, ShortDateValue y) => x >= y._Value;
    public static bool operator !=(ShortDateValue x, ShortDateValue y) => !x.Equals(y);
    public static bool operator !=(ShortDateValue x, DateTime y) => !x.Equals(y);
    public static bool operator !=(DateTime x, ShortDateValue y) => !y.Equals(x);
    public static bool operator <(ShortDateValue x, ShortDateValue y) => x._Value < y._Value;
    public static bool operator <(ShortDateValue x, DateTime y) => x._Value < y;
    public static bool operator <(DateTime x, ShortDateValue y) => x < y._Value;
    public static bool operator <=(ShortDateValue x, ShortDateValue y) => x._Value <= y._Value;
    public static bool operator <=(ShortDateValue x, DateTime y) => x._Value <= y;
    public static bool operator <=(DateTime x, ShortDateValue y) => x <= y._Value;

    #endregion
}