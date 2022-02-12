using Play.Globalization.Time;

namespace Play.Emv.Timeouts;

public abstract class TimeoutConfigurationFactory
{
    #region Instance Members

    public abstract TimeoutConfiguration Create(Milliseconds timeout);

    #endregion
}