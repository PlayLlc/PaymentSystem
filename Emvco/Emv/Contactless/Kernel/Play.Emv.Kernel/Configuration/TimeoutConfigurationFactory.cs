using Play.Emv.Timeouts;
using Play.Globalization.Time;

namespace Play.Emv.Kernel;

public abstract class TimeoutConfigurationFactory
{
    #region Instance Members

    public abstract TimeoutConfiguration GetCancellationPolicy(Milliseconds timeout);

    #endregion
}