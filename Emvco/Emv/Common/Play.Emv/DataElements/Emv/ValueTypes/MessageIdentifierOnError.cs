namespace Play.Emv.DataElements;

public record MessageOnErrorIdentifier : MessageIdentifier
{
    #region Constructor

    protected MessageOnErrorIdentifier(byte value) : base(value)
    { }

    #endregion
}