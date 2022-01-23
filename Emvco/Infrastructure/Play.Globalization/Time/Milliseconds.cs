using System;

namespace Play.Globalization.Time;

/// <summary>
///     This struct represents a <see cref="TimeSpan" /> that is initialized with Milliseconds
/// </summary>
public readonly struct Milliseconds
{
    #region Static Metadata

    public static readonly Milliseconds Zero = new(0);
    private const ushort _NanosecondToMillisecondDivisor = 10000;
    private const ulong _MaxTickValue = long.MaxValue / _NanosecondToMillisecondDivisor;

    #endregion

    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    /// <remarks>
    ///     The <paramref name="value" /> must be 922337203685477 or less in value
    /// </remarks>
    /// <param name="value"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public Milliseconds(ulong value)
    {
        if (value > _MaxTickValue)
            throw new InvalidOperationException($"The maximum value to instantiate a {nameof(Milliseconds)} is {_MaxTickValue}");

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

    #endregion

    #region Instance Members

    public TimeSpan AsTimeSpan() => new((long) _Value * _NanosecondToMillisecondDivisor);

    #endregion

    #region Equality

    public bool Equals(Milliseconds other) => _Value == other._Value;
    public bool Equals(TimeSpan other) => AsTimeSpan() == other;
    public static bool Equals(Milliseconds x, Milliseconds y) => x.Equals(y);
    public bool Equals(ulong other) => _Value == other;
    public override bool Equals(object? obj) => obj is Milliseconds timeValue && Equals(timeValue);

    public override int GetHashCode()
    {
        const int hash = 297581;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Milliseconds left, Milliseconds right) => left.Equals(right);
    public static bool operator ==(Milliseconds left, TimeSpan right) => left.Equals(right);
    public static bool operator ==(TimeSpan left, Milliseconds right) => right.Equals(left);
    public static explicit operator ulong(Milliseconds value) => value._Value;
    public static bool operator >(Milliseconds left, Milliseconds right) => left._Value > right._Value;
    public static bool operator >(Milliseconds left, TimeSpan right) => left.AsTimeSpan() > right;
    public static bool operator >(TimeSpan left, Milliseconds right) => right.AsTimeSpan() > left;
    public static bool operator >=(Milliseconds left, Milliseconds right) => left._Value >= right._Value;
    public static bool operator >=(Milliseconds left, TimeSpan right) => left.AsTimeSpan() >= right;
    public static bool operator >=(TimeSpan left, Milliseconds right) => right.AsTimeSpan() >= left;
    public static implicit operator TimeSpan(Milliseconds value) => value.AsTimeSpan();
    public static implicit operator Milliseconds(int value) => new((uint) value);
    public static bool operator !=(Milliseconds left, Milliseconds right) => !left.Equals(right);
    public static bool operator !=(Milliseconds left, TimeSpan right) => !left.Equals(right);
    public static bool operator !=(TimeSpan left, Milliseconds right) => !right.Equals(left);
    public static bool operator <(Milliseconds left, Milliseconds right) => left._Value < right._Value;
    public static bool operator <(Milliseconds left, TimeSpan right) => left.AsTimeSpan() < right;
    public static bool operator <(TimeSpan left, Milliseconds right) => right.AsTimeSpan() < left;
    public static bool operator <=(Milliseconds left, Milliseconds right) => left._Value <= right._Value;
    public static bool operator <=(Milliseconds left, TimeSpan right) => left.AsTimeSpan() <= right;
    public static bool operator <=(TimeSpan left, Milliseconds right) => right.AsTimeSpan() <= left;

    #endregion
}