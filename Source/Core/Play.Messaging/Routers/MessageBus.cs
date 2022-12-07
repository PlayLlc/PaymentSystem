namespace Play.Messaging;

public class MessageBus : IRouteMessages
{
    #region Instance Values

    private readonly EventRouter _EventRouter;
    private readonly MessageRouter _MessageRouter;
    private readonly IEndpointClient _EndpointClient;

    #endregion

    #region Constructor

    public MessageBus()
    {
        _EventRouter = new EventRouter();
        _MessageRouter = new MessageRouter();
        _EndpointClient = new EndpointClient(this);
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
        lock (_MessageRouter)
        {
            _MessageRouter.Subscribe(messageChannel);
        }
    }

    void IRouteMessages.Unsubscribe(ChannelIdentifier channelIdentifier)
    {
        lock (_MessageRouter)
        {
            _MessageRouter.Unsubscribe(channelIdentifier.GetChannelTypeId());
        }
    }

    #region Requests

    /// <summary>
    ///     Send
    /// </summary>
    /// <param name="requestMessageEnvelope"></param>
    /// <exception cref="Exceptions.InvalidMessageRoutingException"></exception>
    void IRouteMessages.Send(RequestMessageEnvelope requestMessageEnvelope)
    {
        lock (_MessageRouter)
        {
            _MessageRouter.Send(requestMessageEnvelope.GetMessage());
        }
    }

    #endregion

    public IEndpointClient GetEndpointClient() => _EndpointClient;

    #region Replies

    /// <summary>
    ///     Send
    /// </summary>
    /// <param name="responseMessageEnvelope"></param>
    /// <exception cref="Exceptions.InvalidMessageRoutingException"></exception>
    void IRouteMessages.Send(ResponseMessageEnvelope responseMessageEnvelope)
    {
        lock (_MessageRouter)
        {
            _MessageRouter.Send(responseMessageEnvelope.GetMessage());
        }
    }

    #endregion

    #endregion

    #region Events

    public void Subscribe(EventHandlerBase eventHandler)
    {
        lock (_EventRouter)
        {
            _EventRouter.Subscribe(eventHandler);
        }
    }

    public void Unsubscribe(EventHandlerBase eventHandler)
    {
        lock (_EventRouter)
        {
            _EventRouter.Unsubscribe(eventHandler);
        }
    }

    void IRouteMessages.Publish(EventEnvelope eventEnvelope)
    {
        lock (_EventRouter)
        {
            _EventRouter.Publish(eventEnvelope.GetEvent()).ConfigureAwait(false);
        }
    }

    #endregion
}