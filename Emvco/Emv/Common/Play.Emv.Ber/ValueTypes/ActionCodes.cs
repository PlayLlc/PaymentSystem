namespace Play.Emv.Ber;

/// <summary>
///     The underlying value for Action Codes such as TerminalActionCodeDefault, IssuerActionCodeDefault, etc
/// </summary>
public readonly struct ActionCodes
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public ActionCodes(ulong value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ulong(ActionCodes value) => value._Value;

    #endregion
}