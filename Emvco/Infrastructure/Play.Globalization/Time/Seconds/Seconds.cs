using System;

namespace Play.Globalization.Time.Seconds;

/// <summary>
///     This struct represents a <see cref="TimeSpan" /> that is initialized with Milliseconds
/// </summary>
public readonly record struct Seconds
{
    #region Static Metadata

    public static readonly Seconds Zero = new(0);
    public const byte Precision = 1;

    #endregion

    #region Instance Values

    private readonly long _Value;

    #endregion

    #region Constructor

    public Seconds(Deciseconds value)
    {
        _Value = (long) value / (Deciseconds.Precision / Precision);
    }

    public Seconds(Milliseconds value)
    {
        _Value = (long) value / (Milliseconds.Precision / Precision);
    }

    public Seconds(Microseconds value)
    {
        _Value = (long) value / (Microseconds.Precision / Precision);
    }

    public Seconds(Ticks value)
    {
        _Value = (long) value / (Ticks.Precision / Precision);
    }

    public Seconds(byte value)
    {
        _Value = value;
    }

    public Seconds(int value)
    {
        _Value = value;
    }

    public Seconds(long value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => GetTimeSpan(_Value);
    private static TimeSpan GetTimeSpan(long value) => new(GetDays(value), GetHours(value), GetMinutes(value), GetSeconds(value));
    private static int GetDays(long value) => GetHours(value) / 24;
    private static int GetHours(long value) => GetMinutes(value) / 60;
    private static int GetMinutes(long value) => checked((int) value / 60);
    private static int GetSeconds(long value) => checked((int) value % 60);

    #endregion

    #region Operator Overrides

    public static explicit operator long(Seconds value) => value._Value;
    public static explicit operator Deciseconds(Seconds value) => new(value);
    public static explicit operator Milliseconds(Seconds value) => new(value);
    public static explicit operator Microseconds(Seconds value) => new(value);
    public static implicit operator TimeSpan(Seconds value) => value.AsTimeSpan();
    public static implicit operator Seconds(Milliseconds value) => new(value);
    public static Microseconds operator *(Seconds left, Seconds right) => new(left._Value * right._Value);
    public static Microseconds operator /(Seconds left, Seconds right) => new(left._Value / right._Value);
    public static Microseconds operator -(Seconds left, Seconds right) => new(left._Value - right._Value);
    public static Microseconds operator +(Seconds left, Seconds right) => new(left._Value + right._Value);
    public static bool operator <(Seconds left, Seconds right) => left._Value < right._Value;
    public static bool operator >(Seconds left, Seconds right) => left._Value > right._Value;
    public static bool operator <=(Seconds left, Seconds right) => left._Value <= right._Value;
    public static bool operator >=(Seconds left, Seconds right) => left._Value >= right._Value;

    #endregion
}