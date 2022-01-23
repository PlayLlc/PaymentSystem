using Play.Emv.DataElements;
using Play.Emv.Sessions;
using Play.Emv.Transactions;

namespace Play.Emv.Terminal.Services;

internal record TerminalSession(
    TerminalSessionId TerminalSessionId,
    Transaction Transaction,
    TerminalVerificationResults TerminalVerificationResults)
{
    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => Transaction.GetTransactionSessionId();

    #endregion
}