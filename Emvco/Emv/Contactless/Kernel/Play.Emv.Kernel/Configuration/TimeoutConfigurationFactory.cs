﻿using Play.Emv.Timeouts;
using Play.Globalization.Time;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Kernel;

public abstract class TimeoutConfigurationFactory
{
    #region Instance Members

    public abstract TimeoutConfiguration GetCancellationPolicy(Milliseconds timeout);

    #endregion
}