using System;

using Play.Emv.Timeouts;
using Play.Globalization.Time;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Kernel;

public class KernelTimeoutConfigurationDatabase : TimeoutConfigurationFactory
{
    #region Instance Values

    private readonly Action _TimeoutHandler;

    #endregion

    #region Constructor

    public KernelTimeoutConfigurationDatabase(Action timeoutHandler)
    {
        _TimeoutHandler = timeoutHandler;
    }

    #endregion

    #region Instance Members

    public override TimeoutConfiguration GetCancellationPolicy(Milliseconds timeout) => new(timeout, _TimeoutHandler);

    #endregion
}