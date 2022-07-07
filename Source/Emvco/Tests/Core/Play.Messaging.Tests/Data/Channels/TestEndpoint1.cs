using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Messaging.Exceptions;
using Play.Messaging.Tests.Data.Messages;

namespace Play.Messaging.Tests.Data.Channels
{
    public class TestEndpoint1 : IMessageChannel
    {
        #region Static Metadata

        public static readonly ChannelTypeId ChannelTypeId = TestChannel.Id;

        #endregion

        #region Instance Values

        public readonly ChannelIdentifier ChannelIdentifier;
        private readonly IEndpointClient _EndpointClient;

        #endregion

        #region Instance Values

        private int _Value;

        #endregion

        #region Constructor

        public TestEndpoint1(ICreateEndpointClient messageRouter)
        {
            _EndpointClient = messageRouter.CreateEndpointClient(this);
            ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
        }

        #endregion

        #region Instance Members

        public ChannelTypeId GetChannelTypeId() => ChannelTypeId;
        public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;

        #region Requests

        public void Request(RequestMessage message)
        {
            if (message is TestRequestMessage testRequestMessage)
                Request(testRequestMessage);
            else
                throw new InvalidMessageRoutingException(message, this);
        }

        public void Request(TestRequestMessage message)
        {
            _Value = message.GetValue();
        }

        #endregion

        #region Callbacks

        public void Handle(ResponseMessage message)
        {
            throw new NotImplementedException();
        }

        #endregion

        public int GetIndex() => _Value;

        #endregion
    }
}