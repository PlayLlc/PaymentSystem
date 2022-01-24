using Play.Messaging.Exceptions;

namespace Play.Messaging;

internal class MessageBus
{
    #region Instance Values

    private readonly Dictionary<ChannelTypeId, IMessageChannel> _ChannelMap;

    #endregion

    #region Constructor

    public MessageBus()
    {
        _ChannelMap = new Dictionary<ChannelTypeId, IMessageChannel>();
    }

    #endregion

    #region Instance Members

    public void Subscribe(IMessageChannel messageChannel)
    {
        lock (_ChannelMap)
        {
            if (_ChannelMap.ContainsKey(messageChannel.GetChannelTypeId()))
            {
                throw new
                    MessagingException($"The {nameof(IMessageChannel)}: {messageChannel.GetType().FullName} could not {nameof(Subscribe)} because a {nameof(IMessageChannel)} subscription already exists");
            }

            _ChannelMap.Add(messageChannel.GetChannelTypeId(), messageChannel);
        }
    }

    public void Unsubscribe(ChannelTypeId channelTypeId)
    {
        lock (_ChannelMap)
        {
            if (_ChannelMap.ContainsKey(channelTypeId))
                return;

            _ChannelMap.Remove(channelTypeId);
        }
    }

    public void Send(RequestMessage requestMessage)
    {
        lock (_ChannelMap)
        {
            if (!_ChannelMap.ContainsKey(requestMessage.GetChannelTypeId()))
            {
                throw new
                    InvalidMessageRoutingException($"The message type [{requestMessage.GetType().FullName}] could not be sent because the no message channel has subscribed with the {nameof(ChannelTypeId)}: [{requestMessage.GetChannelTypeId()}]");
            }

            _ChannelMap[requestMessage.GetChannelTypeId()]!.Request(requestMessage);
        }
    }

    public void Send(ResponseMessage responseMessage)
    {
        CorrelationId? correlationId = responseMessage.GetCorrelationId();

        lock (_ChannelMap)
        {
            if (!_ChannelMap.ContainsKey(correlationId.GetChannelTypeId()))
            {
                throw new
                    InvalidMessageRoutingException($"The message type [{responseMessage.GetType().FullName}] could not be sent because the no message channel has subscribed with the {nameof(ChannelTypeId)}: [{correlationId.GetChannelTypeId()}]");
            }

            _ChannelMap[correlationId.GetChannelTypeId()].Handle(responseMessage);
        }
    }

    #endregion
}