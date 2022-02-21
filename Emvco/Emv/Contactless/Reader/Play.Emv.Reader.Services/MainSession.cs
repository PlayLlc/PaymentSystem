using Play.Emv.DataElements.Emv;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Reader.Services;

internal record MainSession(
    CorrelationId ActSignalCorrelationId,
    Transaction Transaction,
    TagsToRead? TagsToRead,
    KernelSessionId? KernelSessionId)
{
    #region Instance Values

    public readonly MainSessionId SessionId = new();

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => Transaction.GetTransactionSessionId();

    #endregion
}