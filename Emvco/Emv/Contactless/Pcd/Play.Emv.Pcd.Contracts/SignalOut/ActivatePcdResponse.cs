using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     Response from an the ACT signal <see cref="ActivatePcdRequest" />
/// </summary>
public record ActivatePcdResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivatePcdResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.ProximityCouplingDevice;

    #endregion

    #region Instance Values

    private readonly Level1Error _Level1Error;
    private readonly TransactionSessionId _TransactionSessionId;
    private readonly bool _IsCollisionDetected;

    #endregion

    #region Constructor

    public ActivatePcdResponse(
        CorrelationId correlationId,
        bool isCollisionDetected,
        Level1Error level1Error,
        TransactionSessionId transactionSessionId) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _Level1Error = level1Error;
        _TransactionSessionId = transactionSessionId;
        _IsCollisionDetected = isCollisionDetected;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public Level1Error GetLevel1Error() => _Level1Error;
    public bool IsCollisionDetected() => _IsCollisionDetected;

    #endregion
}