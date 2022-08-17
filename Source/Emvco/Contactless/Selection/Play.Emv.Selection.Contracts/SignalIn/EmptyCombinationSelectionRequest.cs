using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Selection.Contracts.SignalIn
{
    public record EmptyCombinationSelectionRequest : RequestSignal
    {
        #region Static Metadata

        public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(EmptyCombinationSelectionRequest));
        public static readonly ChannelTypeId ChannelTypeId = SelectionChannel.Id;

        #endregion

        #region Instance Values

        public readonly TransactionSessionId TransactionSessionId;

        #endregion

        #region Constructor

        public EmptyCombinationSelectionRequest(TransactionSessionId transactionSessionId) : base(MessageTypeId, ChannelTypeId)
        {
            TransactionSessionId = transactionSessionId;
        }

        #endregion
    }
}