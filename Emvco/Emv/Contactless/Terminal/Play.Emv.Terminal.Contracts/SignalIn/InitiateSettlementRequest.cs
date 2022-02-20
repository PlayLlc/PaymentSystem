using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Messaging;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Terminal.Contracts.SignalIn;

public record InitiateSettlementRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(InitiateSettlementRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Terminal;

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