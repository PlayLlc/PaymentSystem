using System;
using System.Numerics;

using Play.Globalization.Timed;

namespace Play.Globalization.Time;

/// <summary>
///     This struct represents a <see cref="TimeSpan" /> that is initialized with Milliseconds
/// </summary>
public readonly record struct Seconds
{
    #region Static Metadata

    public static readonly Seconds Zero = new(0);
    private const int _SecondsToDecisecondsDivisor = 10;
    private const int _SecondsToMillisecondsDivisor = 1000;

    #endregion

    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public Seconds(Deciseconds value)
    {
        _Value = checked((int) value / _SecondsToDecisecondsDivisor);
    }

    public Seconds(Milliseconds value)
    {
        _Value = checked((int) value / _SecondsToMillisecondsDivisor);
    }

    public Seconds(byte value)
    {
        _Value = value;
    }

    public Seconds(int value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => GetTimeSpan(_Value);
    private static TimeSpan GetTimeSpan(int value) => new(GetDays(value), GetHours(value), GetMinutes(value), GetSeconds(value));
    private static int GetDays(int value) => GetHours(value) / 24;
    private static int GetHours(int value) => GetMinutes(value) / 60;
    private static int GetMinutes(int value) => value / 60;
    private static int GetSeconds(int value) => value % 60;

    #endregion

    #region Operator Overrides

    public static explicit operator int(Seconds value) => value._Value;
    public static implicit operator TimeSpan(Seconds value) => value.AsTimeSpan();
    public static implicit operator Seconds(Milliseconds value) => new(value);
    public static Milliseconds operator -(Seconds left, Seconds right) => new(left._Value - right._Value);
    public static Milliseconds operator +(Seconds left, Seconds right) => new(left._Value + right._Value);
    public static bool operator <(Seconds left, Seconds right) => left._Value < right._Value;
    public static bool operator >(Seconds left, Seconds right) => left._Value > right._Value;
    public static bool operator <=(Seconds left, Seconds right) => left._Value <= right._Value;
    public static bool operator >=(Seconds left, Seconds right) => left._Value >= right._Value;

    #endregion
}