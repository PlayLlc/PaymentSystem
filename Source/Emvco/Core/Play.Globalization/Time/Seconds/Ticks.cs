using System;

namespace Play.Globalization.Time;

/// <summary>
///     A single tick represents one hundred nanoseconds or one ten-millionth of a second. There are 10,000 ticks in a
///     millisecond and 10 million ticks in a second.
/// </summary>
public readonly record struct Ticks
{
    #region Static Metadata

    public static readonly Ticks Zero = new(0);
    public const int Precision = 10000000;

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
    public Ticks(long value)
    {
        _Value = value;
    }

    public Ticks(uint value)
    {
        _Value = value;
    }

    public Ticks(ushort value)
    {
        _Value = value;
    }

    public Ticks(byte value)
    {
        _Value = value;
    }

    public Ticks(Milliseconds value)
    {
        _Value = (long) value * (Precision / Milliseconds.Precision);
    }

    public Ticks(Microseconds value)
    {
        _Value = (long)value * (Precision / Microseconds.Precision);
    }

    public Ticks(Deciseconds value)
    {
        _Value = (long) value * (Precision / Deciseconds.Precision);
    }

    public Ticks(Seconds value)
    {
        _Value = (long) value * (Precision / Seconds.Precision);
    }

    public Ticks(TimeSpan value)
    {
        _Value = (uint) value.Ticks;
    }

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => new(_Value * (Precision - Milliseconds.Precision));
    public Seconds AsSeconds() => new(this);
    public Deciseconds AsDeciSeconds() => new(this);
    public Milliseconds AsMilliSeconds() => new(this);
    public Microseconds AsMicroSeconds() => new(this);

    #endregion

    #region Equality

    public bool Equals(Ticks other) => _Value == other._Value;
    public bool Equals(TimeSpan other) => AsTimeSpan() == other;
    public static bool Equals(Ticks x, Ticks y) => x.Equals(y);
    public bool Equals(long other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static Ticks operator *(Ticks left, Ticks right) => new(left._Value * right._Value);
    public static Ticks operator /(Ticks left, Ticks right) => new(left._Value / right._Value);
    public static Ticks operator -(Ticks left, Ticks right) => new(left._Value - right._Value);
    public static Ticks operator +(Ticks left, Ticks right) => new(left._Value + right._Value);
    public static bool operator >(long left, Ticks right) => left > right._Value;
    public static bool operator <(long left, Ticks right) => left < right._Value;
    public static bool operator >=(long left, Ticks right) => left >= right._Value;
    public static bool operator <=(long left, Ticks right) => left <= right._Value;
    public static bool operator ==(long left, Ticks right) => left == right._Value;
    public static bool operator !=(long left, Ticks right) => left != right._Value;
    public static bool operator >(Ticks left, long right) => left._Value > right;
    public static bool operator <(Ticks left, long right) => left._Value < right;
    public static bool operator >=(Ticks left, long right) => left._Value >= right;
    public static bool operator <=(Ticks left, long right) => left._Value <= right;
    public static bool operator ==(Ticks left, long right) => left._Value == right;
    public static bool operator !=(Ticks left, long right) => left._Value != right;
    public static bool operator ==(Ticks left, TimeSpan right) => left.Equals(right);
    public static bool operator ==(TimeSpan left, Ticks right) => right.Equals(left);
    public static explicit operator long(Ticks value) => value._Value;
    public static bool operator >(Ticks left, Ticks right) => left._Value > right._Value;
    public static bool operator >(Ticks left, TimeSpan right) => left.AsTimeSpan() > right;
    public static bool operator >(TimeSpan left, Ticks right) => right.AsTimeSpan() > left;
    public static bool operator >=(Ticks left, Ticks right) => left._Value >= right._Value;
    public static bool operator >=(Ticks left, TimeSpan right) => left.AsTimeSpan() >= right;
    public static bool operator >=(TimeSpan left, Ticks right) => right.AsTimeSpan() >= left;
    public static explicit operator TimeSpan(Ticks value) => value.AsTimeSpan();
    public static explicit operator Ticks(TimeSpan value) => new(value);
    public static explicit operator Ticks(int value) => new((uint) value);
    public static bool operator !=(Ticks left, TimeSpan right) => !left.Equals(right);
    public static bool operator !=(TimeSpan left, Ticks right) => !right.Equals(left);
    public static bool operator <(Ticks left, Ticks right) => left._Value < right._Value;
    public static bool operator <(Ticks left, TimeSpan right) => left.AsTimeSpan() < right;
    public static bool operator <(TimeSpan left, Ticks right) => right.AsTimeSpan() < left;
    public static bool operator <=(Ticks left, Ticks right) => left._Value <= right._Value;
    public static bool operator <=(Ticks left, TimeSpan right) => left.AsTimeSpan() <= right;
    public static bool operator <=(TimeSpan left, Ticks right) => right.AsTimeSpan() <= left;
    public static explicit operator Ticks(Seconds value) => new(value);
    public static explicit operator Ticks(Deciseconds value) => new(value);

    #endregion
}