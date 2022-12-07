namespace Play.Messaging;

public class EndpointClient : IEndpointClient
{
    #region Instance Values

    private readonly IRouteMessages _MessageRouter;

    #endregion

    #region Constructor

    internal EndpointClient(IRouteMessages messageRouter)
    {
        _MessageRouter = messageRouter;
    }

    #endregion

    #region Instance Members

    #region Events

    public void Publish(Event @event)
    {
        _MessageRouter.Publish(new EventEnvelope(new EventHeader(), @event));
    }

    #endregion

    #endregion

    #region Subscription

    public void Subscribe(IMessageChannel messageChannel)
    {
        _MessageRouter.Subscribe(messageChannel);
    }

    public void Unsubscribe(IMessageChannel messageChannel)
    {
        _MessageRouter.Unsubscribe(messageChannel.GetChannelIdentifier());
    }

    #endregion

    #region Messages

    public void Send(ResponseMessage message)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateResponseEnvelope(message));
    }

    public void Send(RequestMessage message)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateRequestEnvelope(message));
    }

    #endregion
}