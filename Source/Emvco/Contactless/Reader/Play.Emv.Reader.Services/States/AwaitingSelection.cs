using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Reader.Services.States;

public record AwaitingSelection
    (TransactionSessionId TransactionSessionId, CorrelationId CorrelationId, ReaderDatabase ReaderDatabase) : AwaitingTransaction(ReaderDatabase)
{
    #region Instance Values

    public readonly TransactionSessionId TransactionSessionId = TransactionSessionId;
    public readonly CorrelationId CorrelationId = CorrelationId;

    #endregion
}