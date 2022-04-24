namespace Play.Emv.Ber.ValueTypes;

public readonly record struct CryptogramType
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public CryptogramType(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CryptogramType value) => value._Value;

    #endregion
}