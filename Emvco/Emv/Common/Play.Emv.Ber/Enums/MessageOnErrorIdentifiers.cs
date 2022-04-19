namespace Play.Emv.Ber;

public record MessageOnErrorIdentifiers : MessageIdentifiers
{
    #region Constructor

    protected MessageOnErrorIdentifiers(byte value) : base(value)
    { }

    #endregion
}