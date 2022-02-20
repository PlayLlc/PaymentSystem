using Play.Emv.Messaging;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     ACT DataExchangeSignal. Generate an Answer to Reset, start polling for an PICC or HCE
/// </summary>
public record ActivatePcdRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ActivatePcdRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.ProximityCouplingDevice;

    #endregion

    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public ActivatePcdRequest(TransactionSessionId transactionSessionId) : base(MessageTypeId, ChannelTypeId)
    {
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion
}