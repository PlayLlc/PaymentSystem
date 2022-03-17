using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalOut;

public record StopReaderAcknowledgedResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(StopReaderAcknowledgedResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Reader;

    #endregion

    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public StopReaderAcknowledgedResponse(TransactionSessionId transactionSessionId, CorrelationId correlationId) : base(correlationId,
     MessageTypeId, ChannelTypeId)
    {
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion
}