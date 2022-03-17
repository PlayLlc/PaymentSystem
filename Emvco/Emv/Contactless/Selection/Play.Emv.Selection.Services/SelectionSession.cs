﻿using Play.Emv.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Messaging;

namespace Play.Emv.Selection.Services;

internal record SelectionSession
{
    #region Instance Values

    private readonly CorrelationId _CorrelationId;
    private readonly SelectionSessionId _SelectionSessionId;
    private readonly Transaction _Transaction;

    #endregion

    #region Constructor

    public SelectionSession(Transaction transaction, CorrelationId correlationId)
    {
        _SelectionSessionId = new SelectionSessionId();
        _Transaction = transaction;
        _CorrelationId = correlationId;
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _CorrelationId;
    public TransactionType GetTransactionType() => _Transaction.GetTransactionType();
    public Outcome GetOutcome() => _Transaction.GetOutcome();
    public Transaction GetTransaction() => _Transaction;
    public TransactionSessionId GetTransactionSessionId() => _Transaction.GetTransactionSessionId();

    #endregion
}