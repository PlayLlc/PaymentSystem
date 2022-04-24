using System;

namespace Play.Core;

/// <summary>
///     A value that can be used to correlate an Application Session passed between physical or logical boundaries
/// </summary>
/// <example>
///     Example value:
///     0x00 FF FF FF FF FF FFFF
///     |  |  |  |  |  |  |
///     |  |  |  |  |  |  |
///     |  |  |  |  |  |  |
///     YY MM DD HH MM SS Milliseconds
///     Empty
/// </example>
public readonly struct SessionId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public SessionId(DateTime utcNow)
    {
        if (utcNow.Kind != DateTimeKind.Utc)
            throw new ArgumentOutOfRangeException(nameof(utcNow), $"The argument {nameof(utcNow)} was expected to be UTC but was not");

        _Value = GetConstructorValueYear(utcNow)
            | GetConstructorValueMonth(utcNow)
            | GetConstructorValueDay(utcNow)
            | GetConstructorValueHour(utcNow)
            | GetConstructorValueMinute(utcNow)
            | GetConstructorValueSecond(utcNow)
            | GetConstructorValueMillisecond(utcNow);
    }

    #endregion

    #region Instance Members

    private static ulong GetConstructorValueDay(DateTime utcNow) => (ulong) utcNow.Day << (5 * 8);
    private static ulong GetConstructorValueHour(DateTime utcNow) => (ulong) utcNow.Hour << (4 * 8);
    private static ulong GetConstructorValueMillisecond(DateTime utcNow) => (ushort) utcNow.Millisecond;
    private static ulong GetConstructorValueMinute(DateTime utcNow) => (ulong) utcNow.Minute << (3 * 8);
    private static ulong GetConstructorValueMonth(DateTime utcNow) => (ulong) utcNow.Month << (6 * 8);
    private static ulong GetConstructorValueSecond(DateTime utcNow) => (ulong) utcNow.Second << (2 * 8);
    private static ulong GetConstructorValueYear(DateTime utcNow) => (ulong) utcNow.Year << (7 * 8);
    public DateTime GetUtcDateTime() => throw new NotImplementedException();

    #endregion

    #region Operator Overrides

    public static explicit operator ulong(SessionId value) => value._Value;

    #endregion
}