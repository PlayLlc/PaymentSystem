﻿using Play.Emv.DataElements;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Emv.Transactions;

namespace Play.Emv.Selection.Services;

internal record SelectionSession
{
    #region Instance Values

    private readonly SelectionSessionId _SelectionSessionId;
    private readonly Transaction _Transaction;

    #endregion

    #region Constructor

    public SelectionSession(Transaction transaction)
    {
        _SelectionSessionId = new SelectionSessionId();
        _Transaction = transaction;
    }

    #endregion

    #region Instance Members

    public TransactionType GetTransactionType() => _Transaction.GetTransactionType();
    public Outcome GetOutcome() => _Transaction.GetOutcome();
    public Transaction GetTransaction() => _Transaction;
    public TransactionSessionId GetTransactionSessionId() => _Transaction.GetTransactionSessionId();

    #endregion
}