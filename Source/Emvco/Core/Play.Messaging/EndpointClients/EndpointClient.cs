using Play.Globalization.Time.Seconds;

namespace Play.Messaging;

public class EndpointClient : IEndpointClient
{
    #region Instance Values

    private readonly IMessageChannel _MessageChannel;
    private readonly IRouteMessages _MessageRouter;
    private readonly TimeoutConfiguration? _TimeoutConfiguration;

    #endregion

    #region Constructor

    internal EndpointClient(IRouteMessages messageRouter, IMessageChannel messageChannel)
    {
        _MessageRouter = messageRouter;
        _MessageChannel = messageChannel;
    }

    internal EndpointClient(IRouteMessages messageRouter, IMessageChannel messageChannel, TimeoutConfiguration? timeoutConfiguration)
    {
        _MessageRouter = messageRouter;
        _MessageChannel = messageChannel;
        _TimeoutConfiguration = timeoutConfiguration;
    }

    #endregion

    #region Subscription

    public void Subscribe()
    {
        _MessageRouter.Subscribe(_MessageChannel);
    }

    public void Unsubscribe()
    {
        _MessageRouter.Unsubscribe(_MessageChannel.GetChannelIdentifier());
    }

    #endregion

    #region Response Messages

    public void Send(ResponseMessage message)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateResponseEnvelope(message, new MessagingConfiguration(_TimeoutConfiguration)));
    }

    public void Send(ResponseMessage message, Milliseconds timeout)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateResponseEnvelope(message, new MessagingConfiguration(new TimeoutConfiguration(timeout))));
    }

    public void Send(ResponseMessage message, Milliseconds timeout, Action timeoutHandler)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateResponseEnvelope(message,
            new MessagingConfiguration(new TimeoutConfiguration(timeout, timeoutHandler))));
    }

    public void Send(ResponseMessage message, MessagingConfiguration messagingConfiguration)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateResponseEnvelope(message, messagingConfiguration));
    }

    #endregion

    #region Request Messages

    public void Send(RequestMessage message)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateRequestEnvelope(message, new MessagingConfiguration(_TimeoutConfiguration)));
    }

    public void Send(RequestMessage message, Milliseconds timeout)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateRequestEnvelope(message, new MessagingConfiguration(new TimeoutConfiguration(timeout))));
    }

    public void Send(RequestMessage message, Milliseconds timeout, Action timeoutHandler)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateRequestEnvelope(message,
            new MessagingConfiguration(new TimeoutConfiguration(timeout, timeoutHandler))));
    }

    public void Send(RequestMessage message, MessagingConfiguration messagingConfiguration)
    {
        _MessageRouter.Send(EnvelopeFactory.CreateRequestEnvelope(message, messagingConfiguration));
    }

    #endregion

    #region Events

    public void Publish(Event @event)
    {
        _MessageRouter.Publish(new EventEnvelope(new EventHeader(new MessagingConfiguration(_TimeoutConfiguration)), @event));
    }

    public void Publish(Event @event, Milliseconds timeout)
    {
        _MessageRouter.Publish(new EventEnvelope(new EventHeader(new MessagingConfiguration(new TimeoutConfiguration(timeout))), @event));
    }

    public void Publish(Event @event, Milliseconds timeout, Action timeoutHandler)
    {
        _MessageRouter.Publish(
            new EventEnvelope(new EventHeader(new MessagingConfiguration(new TimeoutConfiguration(timeout, timeoutHandler))), @event));
    }

    public void Publish(Event @event, MessagingConfiguration timeoutConfiguration)
    {
        _MessageRouter.Publish(new EventEnvelope(new EventHeader(timeoutConfiguration), @event));
    }

    #endregion
}