using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts
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
            new(sessionId, ExchangeRelayResistanceDataCApduSignal.Create(terminalRelayResistanceEntropy.EncodeValue()));

        #endregion
    }
}