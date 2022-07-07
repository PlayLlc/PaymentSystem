using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Messaging.Tests.Data.Channels;

namespace Play.Messaging.Tests.Data.Messages
{
    public record TestRequestMessage : RequestMessage
    {
        #region Static Metadata

        public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(TestRequestMessage));
        public static readonly ChannelTypeId ChannelTypeId = TestEndpoint1.ChannelTypeId;

        #endregion

        #region Instance Values

        private readonly int _Value;

        #endregion

        #region Constructor

        public TestRequestMessage(int value) : base(ChannelTypeId, MessageTypeId)
        {
            _Value = value;
        }

        #endregion

        #region Instance Members

        public int GetValue() => _Value;

        #endregion
    }
}