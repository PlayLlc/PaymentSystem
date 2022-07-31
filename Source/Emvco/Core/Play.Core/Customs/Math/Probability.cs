namespace Play.Core;

/// <summary>
///     Represents a percentage in the range of 0 to 100. In other words, this class is a proper fraction
/// </summary>
public readonly struct Probability
{
    #region Static Metadata

    private const int _MaxValue = 100;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    /// <param name="value">
    ///     Must be a value between 0 and 100
    /// </param>
    public Probability(byte value)
    {
        if (value > _MaxValue)
        {
            _Value = _MaxValue;

            return;
        }

        _Value = value;
    }

    #endregion

    #region Equality

    public bool Equals(Probability other) => _Value == other._Value;
    public bool Equals(Probability x, Probability y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is Probability percentage && Equals(percentage);
    public int GetHashCode(Probability other) => other.GetHashCode();
    public override int GetHashCode() => 56179 * _Value.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(Probability left, Probability right) => left._Value == right._Value;
    public static explicit operator byte(Probability value) => value._Value;
    public static explicit operator decimal(Probability value) => value._Value / 100M;
    public static bool operator >(Probability left, Probability right) => left._Value > right._Value;
    public static bool operator >=(Probability left, Probability right) => left._Value >= right._Value;
    public static bool operator !=(Probability left, Probability right) => !(left == right);
    public static bool operator <(Probability left, Probability right) => left._Value < right._Value;
    public static bool operator <=(Probability left, Probability right) => left._Value <= right._Value;

    #endregion
}