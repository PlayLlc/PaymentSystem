namespace Play.Messaging;

public class MessageBus : IRouteMessages
{
    #region Instance Values

    private readonly EventRouter _EventBus;
    private readonly MessageRouter _MessageBus;

    #endregion

    #region Constructor

    public MessageBus()
    {
        _EventBus = new EventRouter();
        _MessageBus = new MessageRouter();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Subscribe
    /// </summary>
    /// <param name="messageChannel"></param>
    /// <exception cref="Exceptions.MessagingException"></exception>
    void IRouteMessages.Subscribe(IMessageChannel messageChannel)
    {
        _MessageBus.Subscribe(messageChannel);
    }

    void IRouteMessages.Unsubscribe(ChannelIdentifier channelIdentifier)
    {
        _MessageBus.Unsubscribe(channelIdentifier.GetChannelTypeId());
    }

    #region Requests

    /// <summary>
    ///     Send
    /// </summary>
    /// <param name="requestMessageEnvelope"></param>
    /// <exception cref="Exceptions.InvalidMessageRoutingException"></exception>
    void IRouteMessages.Send(RequestMessageEnvelope requestMessageEnvelope)
    {
        _MessageBus.Send(requestMessageEnvelope.GetMessage());
    }

    #endregion

    public IEndpointClient CreateEndpointClient() => new EndpointClient(this);

    #region Replies

    /// <summary>
    ///     Send
    /// </summary>
    /// <param name="responseMessageEnvelope"></param>
    /// <exception cref="Exceptions.InvalidMessageRoutingException"></exception>
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