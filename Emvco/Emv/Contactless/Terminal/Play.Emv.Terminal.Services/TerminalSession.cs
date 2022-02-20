using Play.Emv.Acquirer.Contracts;
using Play.Emv.Interchange.DataFields;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Services.DataExchange;
using Play.Emv.Transactions;

namespace Play.Emv.Terminal.Services;

internal class TerminalSession
{
    #region Instance Values

    public readonly SystemTraceAuditNumber SystemTraceAuditNumber;
    public readonly MessageTypeIndicator MessageTypeIndicator;
    public readonly Transaction Transaction;

    // HACK: This is a hack, we should use a state transitioning pattern that throws an exception if the terminal tries to interact with the session in an invalid way
    public FinalOutcome? FinalOutcome;

    #endregion

    #region Constructor

    public TerminalSession(
        SystemTraceAuditNumber systemTraceAuditNumber,
        MessageTypeIndicator messageTypeIndicator,
        Transaction transaction)
    {
        SystemTraceAuditNumber = systemTraceAuditNumber;
        MessageTypeIndicator = messageTypeIndicator;
        Transaction = transaction;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => Transaction.GetTransactionSessionId();
    public void SetFinalOutcome(FinalOutcome finalOutcome) => FinalOutcome = finalOutcome;

    #endregion
}