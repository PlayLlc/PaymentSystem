namespace Play.Icc.Messaging.Apdu;

public readonly struct StatusWord
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public StatusWord(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Equality

    public bool Equals(StatusWord other) => _Value == other._Value;
    public bool Equals(StatusWord x, StatusWord y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is StatusWord statusWord && Equals(statusWord);
    public int GetHashCode(StatusWord other) => other.GetHashCode();
    public override int GetHashCode() => unchecked(4801 * _Value);

    #endregion

    #region Operator Overrides

    public static bool operator ==(StatusWord left, StatusWord right) => left.Equals(right);
    public static implicit operator byte(StatusWord value) => value._Value;
    public static implicit operator StatusWord(byte value) => new(value);
    public static bool operator !=(StatusWord left, StatusWord right) => !left.Equals(right);

    #endregion
}