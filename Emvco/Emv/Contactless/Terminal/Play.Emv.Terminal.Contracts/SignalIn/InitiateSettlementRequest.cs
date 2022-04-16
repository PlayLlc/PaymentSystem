using Play.Emv.Messaging;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Terminal.Contracts.SignalIn;

public record InitiateSettlementRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(InitiateSettlementRequest));
    public static readonly ChannelTypeId ChannelTypeId = TerminalChannel.Id;

    #endregion

    #region Instance Values

    public readonly DateTimeUtc SettlementRequestDateTimeUtc;

    #endregion

    #region Constructor

    public InitiateSettlementRequest(DateTimeUtc settlementRequestDateTimeUtcUtc) : base(MessageTypeId, ChannelTypeId)
    {
        SettlementRequestDateTimeUtc = settlementRequestDateTimeUtcUtc;
    }

    #endregion
}