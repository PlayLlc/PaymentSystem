using System;

namespace Play.Globalization.Time.Seconds;

public readonly record struct Microseconds
{
    #region Static Metadata

    public static readonly Microseconds Zero = new(0);
    public const int Precision = 1000000;

    #endregion

    #region Instance Values

    private readonly long _Value;

    #endregion

    #region Constructor

    /// <remarks>
    ///     The <paramref name="value" /> must be 922337203685477 or less in value
    /// </remarks>
    /// <param name="value"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public Microseconds(long value)
    {
        _Value = value;
    }

    public Microseconds(uint value)
    {
        _Value = value;
    }

    public Microseconds(ushort value)
    {
        _Value = value;
    }

    public Microseconds(byte value)
    {
        _Value = value;
    }

    public Microseconds(TimeSpan value)
    {
        _Value = value.Ticks / (Ticks.Precision / Precision);
    }

    public Microseconds(Deciseconds value)
    {
        _Value = (long) value * (Precision / Deciseconds.Precision);
    }

    public Microseconds(Seconds value)
    {
        _Value = (long) value * (Precision / Seconds.Precision);
    }

    public Microseconds(Milliseconds value)
    {
        _Value = (long) value * (Precision / Milliseconds.Precision);
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => new(_Value * (Ticks.Precision / Precision));
    public Seconds AsSeconds() => new(this);

    #endregion

    #region Equality

    public bool Equals(Microseconds other) => _Value == other._Value;
    public bool Equals(TimeSpan other) => AsTimeSpan() == other;
    public static bool Equals(Microseconds x, Microseconds y) => x.Equals(y);
    public bool Equals(long other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static Microseconds operator *(Microseconds left, Microseconds right) => new(left._Value * right._Value);
    public static Microseconds operator /(Microseconds left, Microseconds right) => new(left._Value / right._Value);
    public static Microseconds operator -(Microseconds left, Microseconds right) => new(left._Value - right._Value);
    public static Microseconds operator +(Microseconds left, Microseconds right) => new(left._Value + right._Value);
    public static bool operator >(long left, Microseconds right) => left > right._Value;
    public static bool operator <(long left, Microseconds right) => left < right._Value;
    public static bool operator >=(long left, Microseconds right) => left >= right._Value;
    public static bool operator <=(long left, Microseconds right) => left <= right._Value;
    public static bool operator ==(long left, Microseconds right) => left == right._Value;
    public static bool operator !=(long left, Microseconds right) => left != right._Value;
    public static bool operator >(Microseconds left, long right) => left._Value > right;
    public static bool operator <(Microseconds left, long right) => left._Value < right;
    public static bool operator >=(Microseconds left, long right) => left._Value >= right;
    public static bool operator <=(Microseconds left, long right) => left._Value <= right;
    public static bool operator ==(Microseconds left, long right) => left._Value == right;
    public static bool operator !=(Microseconds left, long right) => left._Value != right;
    public static bool operator ==(Microseconds left, TimeSpan right) => left.Equals(right);
    public static bool operator ==(TimeSpan left, Microseconds right) => right.Equals(left);
    public static explicit operator long(Microseconds value) => value._Value;
    public static bool operator >(Microseconds left, Microseconds right) => left._Value > right._Value;
    public static bool operator >(Microseconds left, TimeSpan right) => left.AsTimeSpan() > right;
    public static bool operator >(TimeSpan left, Microseconds right) => right.AsTimeSpan() > left;
    public static bool operator >=(Microseconds left, Microseconds right) => left._Value >= right._Value;
    public static bool operator >=(Microseconds left, TimeSpan right) => left.AsTimeSpan() >= right;
    public static bool operator >=(TimeSpan left, Microseconds right) => right.AsTimeSpan() >= left;
    public static implicit operator TimeSpan(Microseconds value) => value.AsTimeSpan();
    public static implicit operator Microseconds(TimeSpan value) => new(value);
    public static implicit operator Microseconds(int value) => new((uint) value);
    public static bool operator !=(Microseconds left, TimeSpan right) => !left.Equals(right);
    public static bool operator !=(TimeSpan left, Microseconds right) => !right.Equals(left);
    public static bool operator <(Microseconds left, Microseconds right) => left._Value < right._Value;
    public static bool operator <(Microseconds left, TimeSpan right) => left.AsTimeSpan() < right;
    public static bool operator <(TimeSpan left, Microseconds right) => right.AsTimeSpan() < left;
    public static bool operator <=(Microseconds left, Microseconds right) => left._Value <= right._Value;
    public static bool operator <=(Microseconds left, TimeSpan right) => left.AsTimeSpan() <= right;
    public static bool operator <=(TimeSpan left, Microseconds right) => right.AsTimeSpan() <= left;
    public static implicit operator Microseconds(Seconds value) => new(value);
    public static implicit operator Microseconds(Deciseconds value) => new(value);

    #endregion
}