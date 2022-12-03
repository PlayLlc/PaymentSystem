using System;

namespace Play.Globalization.Time;

/// <summary>
///     This struct represents a <see cref="TimeSpan" /> that is initialized with Milliseconds
///     10000 ticks per millisecond
/// </summary>
public readonly record struct Milliseconds
{
    #region Static Metadata

    public static readonly Milliseconds Zero = new Milliseconds(0);
    public const int Precision = 1000;

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
    public Milliseconds(long value)
    {
        _Value = value;
    }

    public Milliseconds(uint value)
    {
        _Value = value;
    }

    public Milliseconds(ushort value)
    {
        _Value = value;
    }

    public Milliseconds(byte value)
    {
        _Value = value;
    }

    public Milliseconds(Deciseconds value)
    {
        _Value = (long) value * (Precision / Deciseconds.Precision);
    }

    public Milliseconds(Seconds value)
    {
        _Value = (long) value * (Precision / Seconds.Precision);
    }

    public Milliseconds(Microseconds value)
    {
        _Value = (long) value / Precision;
    }

    public Milliseconds(TimeSpan value)
    {
        _Value = value.Milliseconds;
    }

    public Milliseconds(Ticks value)
    {
        _Value = (long) value / (Ticks.Precision / Precision);
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => new TimeSpan(_Value * (Ticks.Precision / Precision));
    public Seconds AsSeconds() => new Seconds(this);
    public Ticks AsTicks() => new Ticks(this);
    public Deciseconds AsDeciSeconds() => new Deciseconds(this);
    public Microseconds AsMicroseconds() => new Microseconds(this);

    #endregion

    #region Equality

    public bool Equals(Milliseconds other) => _Value == other._Value;
    public bool Equals(TimeSpan other) => AsTimeSpan() == other;
    public static bool Equals(Milliseconds x, Milliseconds y) => x.Equals(y);
    public bool Equals(long other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static Milliseconds operator -(Milliseconds left, Milliseconds right) => new Milliseconds(left._Value - right._Value);
    public static Milliseconds operator +(Milliseconds left, Milliseconds right) => new Milliseconds(left._Value + right._Value);
    public static Milliseconds operator *(Milliseconds left, Milliseconds right) => new Milliseconds(left._Value * right._Value);
    public static Milliseconds operator /(Milliseconds left, Milliseconds right) => new Milliseconds(left._Value / right._Value);
    public static bool operator >(long left, Milliseconds right) => left > right._Value;
    public static bool operator <(long left, Milliseconds right) => left < right._Value;
    public static bool operator >=(long left, Milliseconds right) => left >= right._Value;
    public static bool operator <=(long left, Milliseconds right) => left <= right._Value;
    public static bool operator ==(long left, Milliseconds right) => left == right._Value;
    public static bool operator !=(long left, Milliseconds right) => left != right._Value;
    public static bool operator >(Milliseconds left, long right) => left._Value > right;
    public static bool operator <(Milliseconds left, long right) => left._Value < right;
    public static bool operator >=(Milliseconds left, long right) => left._Value >= right;
    public static bool operator <=(Milliseconds left, long right) => left._Value <= right;
    public static bool operator ==(Milliseconds left, long right) => left._Value == right;
    public static bool operator !=(Milliseconds left, long right) => left._Value != right;
    public static bool operator ==(Milliseconds left, TimeSpan right) => left.Equals(right);
    public static bool operator ==(TimeSpan left, Milliseconds right) => right.Equals(left);
    public static explicit operator long(Milliseconds value) => value._Value;
    public static bool operator >(Milliseconds left, Milliseconds right) => left._Value > right._Value;
    public static bool operator >(Milliseconds left, TimeSpan right) => left.AsTimeSpan() > right;
    public static bool operator >(TimeSpan left, Milliseconds right) => right.AsTimeSpan() > left;
    public static bool operator >=(Milliseconds left, Milliseconds right) => left._Value >= right._Value;
    public static bool operator >=(Milliseconds left, TimeSpan right) => left.AsTimeSpan() >= right;
    public static bool operator >=(TimeSpan left, Milliseconds right) => right.AsTimeSpan() >= left;
    public static implicit operator TimeSpan(Milliseconds value) => value.AsTimeSpan();
    public static implicit operator Milliseconds(TimeSpan value) => new Milliseconds(value);
    public static implicit operator Milliseconds(int value) => new Milliseconds((uint) value);
    public static bool operator !=(Milliseconds left, TimeSpan right) => !left.Equals(right);
    public static bool operator !=(TimeSpan left, Milliseconds right) => !right.Equals(left);
    public static bool operator <(Milliseconds left, Milliseconds right) => left._Value < right._Value;
    public static bool operator <(Milliseconds left, TimeSpan right) => left.AsTimeSpan() < right;
    public static bool operator <(TimeSpan left, Milliseconds right) => right.AsTimeSpan() < left;
    public static bool operator <=(Milliseconds left, Milliseconds right) => left._Value <= right._Value;
    public static bool operator <=(Milliseconds left, TimeSpan right) => left.AsTimeSpan() <= right;
    public static bool operator <=(TimeSpan left, Milliseconds right) => right.AsTimeSpan() <= left;
    public static implicit operator Milliseconds(Seconds value) => new Milliseconds(value);
    public static implicit operator Milliseconds(Deciseconds value) => new Milliseconds(value);

    #endregion
}