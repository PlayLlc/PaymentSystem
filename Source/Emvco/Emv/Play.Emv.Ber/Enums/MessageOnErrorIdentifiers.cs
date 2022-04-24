namespace Play.Emv.Ber.Enums;

public record MessageOnErrorIdentifiers : MessageIdentifiers
{
    #region Constructor

    protected MessageOnErrorIdentifiers(byte value) : base(value)
    { }

    #endregion
}