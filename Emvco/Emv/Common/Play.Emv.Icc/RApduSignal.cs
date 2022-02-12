using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public abstract class RApduSignal : ApduResponse
{
    #region Constructor

    protected RApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool Success() => (_StatusWords == StatusWords._9000) && (GetLevel1Error() == Level1Error.Ok);
    public abstract Level1Error GetLevel1Error();

    #endregion
}