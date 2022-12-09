﻿using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Messaging;

namespace Play.Emv.Selection.Contracts;

public record ActivateSelectionRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivateSelectionRequest));
    public static readonly ChannelTypeId ChannelTypeId = SelectionChannel.Id;

    #endregion

    #region Instance Values

    private readonly Transaction _Transaction;

    #endregion

    #region Constructor

    public ActivateSelectionRequest(Transaction transaction) : base(MessageTypeId, ChannelTypeId)
    {
        _Transaction = transaction;
    }

    #endregion

    #region Instance Members

    public AmountAuthorizedNumeric GetAmountAuthorizedNumeric() => _Transaction.GetAmountAuthorizedNumeric();
    public ref readonly Outcome GetOutcome() => ref _Transaction.GetOutcome();
    public StartOutcomes GetStartOutcome() => _Transaction.GetOutcome().GetStartOutcome();
    public Transaction GetTransaction() => _Transaction;
    public TransactionSessionId GetTransactionSessionId() => _Transaction.GetTransactionSessionId();

    #endregion
}