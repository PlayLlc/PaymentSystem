namespace Play.Messaging;

public class MessageBus : IRouteMessages
{
    #region Instance Values

    private readonly EventRouter _EventBus;
    private readonly MessageRouter _MessageRouter;

    #endregion

    #region Constructor

    public MessageBus()
    {
        _EventBus = new EventRouter();
        _MessageRouter = new MessageRouter();
    }

    #endregion

    #region Instance Members

    public IEndpointClient CreateEndpointClient() => new EndpointClient(this);

    #endregion

    #region Message Subscription

    /// <summary>
    ///     Subscribe
    /// </summary>
    /// <param name="messageChannel"></param>
    /// <exception cref="Exceptions.MessagingException"></exception>
    public void Subscribe(IMessageChannel messageChannel)
    {
        _MessageRouter.Subscribe(messageChannel);
    }

    public void Unsubscribe(ChannelIdentifier channelIdentifier)
    {
        _MessageRouter.Unsubscribe(channelIdentifier.GetChannelTypeId());
    }

    #region Requests

    /// <summary>
    ///     Send
    /// </summary>
    /// <param name="requestMessageEnvelope"></param>
    /// <exception cref="Exceptions.InvalidMessageRoutingException"></exception>
    void IRouteMessages.Send(RequestMessageEnvelope requestMessageEnvelope)
    {
        _MessageRouter.Send(requestMessageEnvelope.GetMessage());
    }

    #endregion

    #region Replies

    /// <summary>
    ///     Send
    /// </summary>
    /// <param name="responseMessageEnvelope"></param>
    /// <exception cref="Exceptions.InvalidMessageRoutingException"></exception>
    void IRouteMessages.Send(ResponseMessageEnvelope responseMessageEnvelope)
    {
        _MessageRouter.Send(responseMessageEnvelope.GetMessage());
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