using System;

namespace Play.Globalization.Time;

/// <summary>
///     Decisecond is one tenth of a second
/// </summary>
public readonly record struct Deciseconds
{
    #region Static Metadata

    public static readonly Deciseconds Zero = new(0);
    public const int Precision = 10;

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
        _Value = (int) value * (Precision / Seconds.Precision);
    }

    public Deciseconds(Microseconds value)
    {
        _Value = (long) value / (Microseconds.Precision / Precision);
    }

    public Deciseconds(Milliseconds value)
    {
        _Value = (long) value / (Milliseconds.Precision / Precision);
    }

    public Deciseconds(TimeSpan value)
    {
        _Value = value.Ticks / (Ticks.Precision / Precision);
        ;
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => new(_Value * (Ticks.Precision / Precision));
    public Ticks AsTicks() => new(this);
    public Seconds AsSeconds() => new(this);

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
    public static bool operator ==(Deciseconds left, TimeSpan right) => left.Equals(right);
    public static bool operator ==(TimeSpan left, Deciseconds right) => right.Equals(left);
    public static explicit operator long(Deciseconds value) => value._Value;
    public static bool operator >(Deciseconds left, Deciseconds right) => left._Value > right._Value;
    public static bool operator >(Deciseconds left, TimeSpan right) => left.AsTimeSpan() > right;
    public static bool operator >(TimeSpan left, Deciseconds right) => right.AsTimeSpan() > left;
    public static bool operator >=(Deciseconds left, Deciseconds right) => left._Value >= right._Value;
    public static bool operator >=(Deciseconds left, TimeSpan right) => left.AsTimeSpan() >= right;
    public static bool operator >=(TimeSpan left, Deciseconds right) => right.AsTimeSpan() >= left;
    public static implicit operator TimeSpan(Deciseconds value) => value.AsTimeSpan();
    public static implicit operator Deciseconds(TimeSpan value) => new(value);
    public static implicit operator Deciseconds(int value) => new(value);
    public static bool operator !=(Deciseconds left, TimeSpan right) => !left.Equals(right);
    public static bool operator !=(TimeSpan left, Deciseconds right) => !right.Equals(left);
    public static bool operator <(Deciseconds left, Deciseconds right) => left._Value < right._Value;
    public static bool operator <(Deciseconds left, TimeSpan right) => left.AsTimeSpan() < right;
    public static bool operator <(TimeSpan left, Deciseconds right) => right.AsTimeSpan() < left;
    public static bool operator <=(Deciseconds left, Deciseconds right) => left._Value <= right._Value;
    public static bool operator <=(Deciseconds left, TimeSpan right) => left.AsTimeSpan() <= right;
    public static bool operator <=(TimeSpan left, Deciseconds right) => right.AsTimeSpan() <= left;
    public static implicit operator Deciseconds(Seconds value) => new(value);

    #endregion
}