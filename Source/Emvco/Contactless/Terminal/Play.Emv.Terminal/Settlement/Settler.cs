using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalIn;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.StateMachine;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Settlement
{
    public class Settler : ISettleTransactions
    {
        public AcquirerRequestSignal CreateSettlementRequest(AcquirerMessageFactory messageFactory, DateTimeUtc settlementRequestTimeUtc) =>
            new SettlementRequestSignal(Array.Empty<TagLengthValue>());
    }
}