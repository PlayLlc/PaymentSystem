﻿using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.StateMachine;

public interface ISettlementReconciliationService
{
    #region Instance Members

    public AcquirerRequestSignal CreateSettlementRequest(AcquirerMessageFactory messageFactory, DateTimeUtc settlementRequestTimeUtc);

    #endregion
}