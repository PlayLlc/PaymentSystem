using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
/// </summary>
public record StopPcdAcknowledgedResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(StopPcdAcknowledgedResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.ProximityCouplingDevice;

    #endregion

    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;
    private readonly Level1Error _Level1Error;

    #endregion

    #region Constructor

    public StopPcdAcknowledgedResponse(CorrelationId correlationId, TransactionSessionId transactionSessionId, Level1Error level1Error) :
        base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _TransactionSessionId = transactionSessionId;
        _Level1Error = level1Error;
    }

    #endregion

    #region Instance Members

    public bool IsTransactionActive(TransactionSessionId currentTransactionSession) => _TransactionSessionId == currentTransactionSession;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public bool Success() => _Level1Error == Level1Error.Ok;

    #endregion
}