namespace Play.Emv.Ber.Enums;

public record DisplayMessageOnErrorIdentifiers : DisplayMessageIdentifiers
{
    #region Constructor

    protected DisplayMessageOnErrorIdentifiers(byte value) : base(value)
    { }

    #endregion
}