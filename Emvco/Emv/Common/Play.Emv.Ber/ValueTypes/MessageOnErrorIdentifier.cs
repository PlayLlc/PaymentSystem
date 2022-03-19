namespace Play.Emv.Ber;

public record MessageOnErrorIdentifier : MessageIdentifier
{
    #region Constructor

    protected MessageOnErrorIdentifier(byte value) : base(value)
    { }

    #endregion
}