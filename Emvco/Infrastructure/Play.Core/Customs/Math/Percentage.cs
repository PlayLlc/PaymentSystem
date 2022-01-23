namespace Play.Core.Math;

public readonly struct Percentage
{
    #region Static Metadata

    private const byte _MaxValue = 100;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    /// <param name="value">
    ///     Must be a value between 0 and 99
    /// </param>
    public Percentage(byte value)
    {
        if (value > _MaxValue)
            _Value = _MaxValue;

        _Value = value;
    }

    #endregion

    #region Equality

    public bool Equals(Percentage other) => _Value == other._Value;
    public bool Equals(Percentage x, Percentage y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is Percentage percentage && Equals(percentage);
    public int GetHashCode(Percentage other) => other.GetHashCode();
    public override int GetHashCode() => 56179 * _Value.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(Percentage left, Percentage right) => left._Value == right._Value;
    public static explicit operator byte(Percentage value) => value._Value;
    public static explicit operator decimal(Percentage value) => value._Value / 100M;
    public static bool operator >(Percentage left, Percentage right) => left._Value > right._Value;
    public static bool operator >=(Percentage left, Percentage right) => left._Value >= right._Value;
    public static bool operator !=(Percentage left, Percentage right) => !(left == right);
    public static bool operator <(Percentage left, Percentage right) => left._Value < right._Value;
    public static bool operator <=(Percentage left, Percentage right) => left._Value <= right._Value;

    #endregion
}