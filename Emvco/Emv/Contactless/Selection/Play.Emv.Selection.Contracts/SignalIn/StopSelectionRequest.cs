using Play.Emv.Messaging;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Selection.Contracts;

public record StopSelectionRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(StopSelectionRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Selection;

    #endregion

    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public StopSelectionRequest(TransactionSessionId transactionSessionId) : base(MessageTypeId, ChannelTypeId)
    {
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion
}