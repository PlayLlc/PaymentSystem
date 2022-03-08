using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public abstract class RApduSignal : ApduResponse
{
    #region Constructor

    protected RApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public abstract bool IsSuccessful();
    public abstract Level1Error GetLevel1Error();
    public StatusWords GetStatusWords() => _StatusWords;

    #endregion
}