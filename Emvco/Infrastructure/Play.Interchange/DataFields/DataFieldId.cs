using Play.Interchange.Exceptions;

namespace Play.Interchange.DataFields;

public readonly record struct DataFieldId
{
    #region Static Metadata

    private const byte _MaxValue = 128;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    /// <summary>
    /// </summary>
    /// <param name="value">Max value of 128</param>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    public DataFieldId(byte value)
    {
        if (value > _MaxValue)
            throw new InterchangeDataFieldOutOfRangeException($"The {nameof(DataFieldId)} must not exceed the value: [{_MaxValue}]");

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