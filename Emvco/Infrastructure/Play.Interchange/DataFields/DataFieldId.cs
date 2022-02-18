namespace Play.Interchange.DataFields;

public readonly record struct DataFieldId
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    internal DataFieldId(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public int CompareTo(DataFieldId other) => _Value.CompareTo(other._Value);
    public int CompareTo(byte other) => _Value.CompareTo(other);

    #endregion

    #region Operator Overrides

    public static bool operator ==(DataFieldId left, byte right) => left._Value == right;
    public static bool operator !=(DataFieldId left, byte right) => left._Value != right;
    public static bool operator ==(byte left, DataFieldId right) => right._Value == left;
    public static bool operator !=(byte left, DataFieldId right) => right._Value != left;
    public static implicit operator byte(DataFieldId tag) => tag._Value;
    public static implicit operator DataFieldId(byte tag) => new(tag);

    #endregion
}