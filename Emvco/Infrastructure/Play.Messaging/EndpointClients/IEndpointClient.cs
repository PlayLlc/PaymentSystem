using Play.Globalization.Time.Seconds;

namespace Play.Messaging;

public interface IEndpointClient
{
    public void Subscribe();
    public void Unsubscribe();
    public void Send(ResponseMessage message);
    public void Send(ResponseMessage message, Milliseconds timeout);
    public void Send(ResponseMessage message, Milliseconds timeout, Action timeoutHandler);
    public void Send(ResponseMessage message, MessagingConfiguration messagingConfiguration);
    public void Send(RequestMessage message);
    public void Send(RequestMessage message, Milliseconds timeout);
    public void Send(RequestMessage message, Milliseconds timeout, Action timeoutHandler);
    public void Send(RequestMessage message, MessagingConfiguration messagingConfiguration);
    public void Publish(Event @event);
    public void Publish(Event @event, Milliseconds timeout);
    public void Publish(Event @event, Milliseconds timeout, Action timeoutHandler);
    public void Publish(Event @event, MessagingConfiguration timeoutConfiguration);
}