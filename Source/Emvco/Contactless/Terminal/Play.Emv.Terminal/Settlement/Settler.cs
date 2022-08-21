using System;

using Play.Ber.DataObjects;
using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.Terminal.StateMachine;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Settlement
{
    public class Settler : ISettleTransactions
    {
        #region Instance Members

        public AcquirerRequestSignal CreateSettlementRequest(AcquirerMessageFactory messageFactory, DateTimeUtc settlementRequestTimeUtc) =>
            new SettlementRequestSignal(Array.Empty<TagLengthValue>());

        #endregion
    }
}