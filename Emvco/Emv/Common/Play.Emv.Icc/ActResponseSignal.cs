using Play.Emv.Ber;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Icc;

public class ActResponseSignal
{
    #region Instance Values

    private readonly bool _IsCollisionDetected;
    private readonly Level1Error _Level1Error;

    #endregion

    #region Constructor

    public ActResponseSignal(bool isCollisionDetected, Level1Error level1Error)
    {
        _IsCollisionDetected = isCollisionDetected;
        _Level1Error = level1Error;
    }

    #endregion

    #region Instance Members

    public Level1Error GetLevel1Error() => _Level1Error;
    public bool IsCollisionDetected() => _IsCollisionDetected;

    #endregion
}