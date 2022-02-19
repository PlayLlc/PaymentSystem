using Play.Emv.Interchange.DataFields;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Services.DataExchange;
using Play.Emv.Transactions;

namespace Play.Emv.Terminal.Services;

internal record TerminalSession(
    SystemTraceAuditNumber SystemTraceAuditNumber,
    Transaction Transaction,
    DataExchangeTerminalService DataExchangeTerminalService)
{
    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => Transaction.GetTransactionSessionId();

    #endregion
}