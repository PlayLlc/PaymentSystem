using Play.Emv.Ber.Enums;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Icc;

public abstract class RApduSignal : ApduResponse
{
    #region Instance Values

    private readonly Level1Error _Level1Error;

    #endregion

    #region Constructor

    protected RApduSignal(byte[] value) : base(value)
    {
        _Level1Error = Level1Error.Ok;
    }

    protected RApduSignal(byte[] value, Level1Error level1Error) : base(value)
    {
        _Level1Error = level1Error;
    }

    #endregion

    #region Instance Members

    public abstract bool IsSuccessful();
    public Level1Error GetLevel1Error() => _Level1Error;

    #endregion
}