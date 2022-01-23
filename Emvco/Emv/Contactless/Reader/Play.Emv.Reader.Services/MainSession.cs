﻿using Play.Emv.DataElements;
using Play.Emv.Sessions;
using Play.Emv.Transactions;
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