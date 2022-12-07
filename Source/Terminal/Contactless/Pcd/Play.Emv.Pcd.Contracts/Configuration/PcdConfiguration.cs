using Play.Emv.Ber.DataElements;

namespace Play.Emv.Pcd.Contracts;

public class PcdConfiguration
{
    #region Instance Values

    public readonly TimeoutValue TimeoutValue;

    #endregion

    #region Constructor

    // Modulation Type - Type A, Type B (ICC)
    public PcdConfiguration(TimeoutValue timeoutValue)
    {
        TimeoutValue = timeoutValue;
    }

    #endregion
}