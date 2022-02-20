using Play.Emv.Messaging;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record StopPcdRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(StopPcdRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.ProximityCouplingDevice;

    #endregion

    #region Instance Values

    private readonly StopType _StopType;
    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public StopPcdRequest(StopType stopType, TransactionSessionId transactionSessionId) : base(MessageTypeId, ChannelTypeId)
    {
        _StopType = stopType;
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public StopType GetStopType() => _StopType;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion

    public enum StopType
    {
        /// <summary>
        ///     STOP(Abort) DataExchangeSignal. Remove the field immediately without card removal procedure
        /// </summary>
        Abort,

        /// <summary>
        ///     STOP(CloseSession) DataExchangeSignal. Perform card removal as described in [EMV CL L1] and indicate when the Card
        ///     has been removed.
        ///     CloseSession” starts the removal sequence and returns a Signal L1RSP(Card Removed) when the Card has been removed.
        /// </summary>
        CloseSession,

        /// <summary>
        ///     STOP(CloseSessionCardCheckCommand) DataExchangeSignal. Perform card removal as described in [EMV CL L1] and
        ///     indicate when
        ///     the Card has been
        ///     “CloseSessionCardCheckCommand” includes a request to check for Card presence. If the Card is still present, then it
        ///     causes
        ///     a “Please Remove
        ///     Card” message to be displayed as part of the removal sequence and returns L1RSP(Card Removed) when the Card has
        ///     been removed. If the
        ///     Card has been removed already, then no message is displayed and an L1RSP(Card Removed) is returned immediately.
        ///     removed.
        /// </summary>
        CloseSessionCardCheck
    }
}