using Play.Emv.DataElements;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Emv.Transactions;
using Play.Messaging;

namespace Play.Emv.Selection.Contracts;

public record ActivateSelectionRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivateSelectionRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Selection;

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
    public StartOutcome GetStartOutcome() => _Transaction.GetOutcome().GetStartOutcome();
    public Transaction GetTransaction() => _Transaction;
    public TransactionSessionId GetTransactionSessionId() => _Transaction.GetTransactionSessionId();

    #endregion
}