using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Reader.Services;

internal record MainSession(
    CorrelationId ActSignalCorrelationId, Transaction Transaction, TagsToRead? TagsToRead, KernelSessionId? KernelSessionId)
{
    #region Instance Values

    public readonly MainSessionId SessionId = new();

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => Transaction.GetTransactionSessionId();

    #endregion
}