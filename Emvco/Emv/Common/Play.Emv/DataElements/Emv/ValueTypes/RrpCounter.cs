namespace Play.Emv.DataElements.Emv.ValueTypes;

/// <summary>
///     The underlying value for Action Codes such as TerminalActionCodeDefault, IssuerActionCodeDefault, etc
/// </summary>
public readonly record struct RrpCounter
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public RrpCounter(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator byte(RrpCounter value) => value._Value;

    #endregion
}