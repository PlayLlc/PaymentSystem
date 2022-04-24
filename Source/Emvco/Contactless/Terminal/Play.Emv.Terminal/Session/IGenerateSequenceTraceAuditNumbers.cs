﻿using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Terminal.Session;

public interface IGenerateSequenceTraceAuditNumbers
{
    #region Instance Members

    public SystemTraceAuditNumber Generate();
    public void Reset(AcquirerResponseSignal settlementAcknowledgement);

    #endregion
}