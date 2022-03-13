using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Icc.ComputeCryptographicChecksddum;
using Play.Emv.Icc.GenerateApplicationCryptogram;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts.SignalIn.Quereies
{
    public record ExchangeRelayResistanceDataRequest : QueryPcdRequest
    {
        #region Static Metadata

        public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ExchangeRelayResistanceDataRequest));

        #endregion

        #region Constructor

        private ExchangeRelayResistanceDataRequest(TransactionSessionId transactionSessionId, CApduSignal cApduSignal) :
            base(cApduSignal, MessageTypeId, transactionSessionId)
        { }

        #endregion

        #region Instance Members

        public static ExchangeRelayResistanceDataRequest Create(
            TransactionSessionId sessionId,
            TerminalRelayResistanceEntropy terminalRelayResistanceEntropy) =>
            new ExchangeRelayResistanceDataRequest(sessionId,
                                                   ExchangeRelayResistanceDataCApduSignal.Create(terminalRelayResistanceEntropy
                                                       .EncodeValue()));

        #endregion
    }
}