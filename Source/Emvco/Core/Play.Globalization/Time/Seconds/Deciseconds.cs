using System;

namespace Play.Globalization.Time;

/// <summary>
///     Decisecond is one tenth of a second
/// </summary>
public readonly record struct Deciseconds
{
    #region Static Metadata

    public static readonly Deciseconds Zero = new(0);
    public const int _Precision = 10;

    #endregion

    #region Instance Values

    private readonly long _Value;

    #endregion

    #region Constructor

    public Deciseconds(long value)
    {
        _Value = value;
    }

    public Deciseconds(int value)
    {
        _Value = value;
    }

    public Deciseconds(ushort value)
    {
        _Value = value;
    }

    public Deciseconds(byte value)
    {
        _Value = value;
    }

    public Deciseconds(Seconds value)
    {
        _Value = (int) value * (_Precision / Seconds._Precision);
    }

    public Deciseconds(Microseconds value)
    {
        _Value = (long) value / (Microseconds._Precision / _Precision);
    }

    public Deciseconds(Milliseconds value)
    {
        _Value = (long) value / (Milliseconds._Precision / _Precision);
    }

    public Deciseconds(TimeSpan value)
    {
        _Value = value.Ticks / (Ticks._Precision / _Precision);
    }

    public Deciseconds(Ticks value)
    {
        _Value = (long) value / (Ticks._Precision / _Precision);
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => new(_Value * (Ticks._Precision / _Precision));
    public Ticks AsTicks() => new(this);
    public Seconds AsSeconds() => new(this);
    public Milliseconds AsMilliseconds() => new(this);

    #endregion

    #region Equality

    public bool Equals(Deciseconds other) => _Value == other._Value;
    public bool Equals(TimeSpan other) => AsTimeSpan() == other;
    public static bool Equals(Deciseconds x, Deciseconds y) => x.Equals(y);
    public bool Equals(long other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static Deciseconds operator *(Deciseconds left, Deciseconds right) => new(left._Value * right._Value);
    public static Deciseconds operator /(Deciseconds left, Deciseconds right) => new(left._Value / right._Value);
    public static Deciseconds operator -(Deciseconds left, Deciseconds right) => new(left._Value - right._Value);
    public static Deciseconds operator +(Deciseconds left, Deciseconds right) => new(left._Value + right._Value);
    public static explicit operator long(Deciseconds value) => value._Value;
    public static bool operator >(Deciseconds left, Deciseconds right) => left._Value > right._Value;
    public static bool operator >=(Deciseconds left, Deciseconds right) => left._Value >= right._Value;
    public static explicit operator TimeSpan(Deciseconds value) => value.AsTimeSpan();
    public static explicit operator Deciseconds(TimeSpan value) => new(value);
    public static explicit operator Deciseconds(Seconds value) => new(value);
    public static explicit operator Deciseconds(int value) => new(value);
    public static bool operator <(Deciseconds left, Deciseconds right) => left._Value < right._Value;
    public static bool operator <=(Deciseconds left, Deciseconds right) => left._Value <= right._Value;

    #endregion
}