using Play.Emv.Configuration;
using Play.Emv.Sessions;
using Play.Emv.Terminal.___Temp;
using Play.Emv.Transactions;

namespace Play.Emv.Terminal.Services;

internal record TerminalSession(
    Transaction Transaction,
    TerminalConfiguration TerminalConfiguration,
    DataExchangeTerminalService DataExchangeTerminalService)
{
    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => Transaction.GetTransactionSessionId();

    #endregion
}