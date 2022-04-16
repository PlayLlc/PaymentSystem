using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     CA(C-APDU) DataExchangeSignal. Send a C-APDU to the Card and return either an R-APDU or an error indication. The
///     parameter to the DataExchangeSignal
///     is the command to be sent to the Card
/// </summary>
public record QueryPcdRequest : RequestSignal
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ProximityCouplingDeviceChannel.Id;

    #endregion

    #region Instance Values

    protected readonly CApduSignal _CApduSignal;
    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    protected QueryPcdRequest(CApduSignal cApduSignal, MessageTypeId messageTypeId, TransactionSessionId transactionSessionId) : base(
        messageTypeId, ChannelTypeId)
    {
        _CApduSignal = cApduSignal;
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public CApduSignal GetCApduSignal() => _CApduSignal;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion

    #region Serialization

    public byte[] Serialize() => _CApduSignal.Serialize();

    #endregion
}