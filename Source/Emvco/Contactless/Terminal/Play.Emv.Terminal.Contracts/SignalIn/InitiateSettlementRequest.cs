using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Globalization.Time;
using Play.Messaging;

namespace Play.Emv.Terminal.Contracts.SignalIn
{
    public record InitiateSettlementRequest : RequestSignal
    {
        #region Static Metadata

        public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(QueryTerminalRequest));
        public static readonly ChannelTypeId ChannelTypeId = TerminalChannel.Id;

        #endregion

        #region Instance Values

        public readonly DateTimeUtc DateTimeUtc;

        #endregion

        #region Constructor

        public InitiateSettlementRequest(DateTimeUtc dateTime) : base(MessageTypeId, ChannelTypeId)
        {
            DateTimeUtc = dateTime;
        }

        #endregion
    }
}