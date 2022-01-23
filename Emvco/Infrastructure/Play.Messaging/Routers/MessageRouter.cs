﻿namespace Play.Messaging;

public class MessageRouter : IRouteMessages
{
    #region Instance Values

    private readonly EventBus _EventBus;
    private readonly MessageBus _MessageBus;

    #endregion

    #region Constructor

    public MessageRouter()
    {
        _EventBus = new EventBus();
        _MessageBus = new MessageBus();
    }

    #endregion

    #region Instance Members

    void IRouteMessages.Subscribe(IMessageChannel messageChannel)
    {
        _MessageBus.Subscribe(messageChannel);
    }

    void IRouteMessages.Unsubscribe(ChannelIdentifier channelIdentifier)
    {
        _MessageBus.Unsubscribe(channelIdentifier.GetChannelTypeId());
    }

    #region Requests

    void IRouteMessages.Send(RequestMessageEnvelope requestMessageEnvelop)
    {
        _MessageBus.Send(requestMessageEnvelop.GetMessage());
    }

    #endregion

    public IEndpointClient CreateEndpointClient(IMessageChannel messageChannel) => new EndpointClient(this, messageChannel);

    #region Replies

    void IRouteMessages.Send(ResponseMessageEnvelope responseMessageEnvelope)
    {
        _MessageBus.Send(responseMessageEnvelope.GetMessage());
    }

    #endregion

    #endregion

    #region Events

    public void Subscribe(EventHandlerBase eventHandler)
    {
        _EventBus.Subscribe(eventHandler);
    }

    public void Unsubscribe(EventHandlerBase eventHandler)
    {
        _EventBus.Unsubscribe(eventHandler);
    }

    async Task IRouteMessages.Publish(EventEnvelope eventEnvelope)
    {
        await _EventBus.Publish(eventEnvelope.GetEvent()).ConfigureAwait(false);
    }

    #endregion
}