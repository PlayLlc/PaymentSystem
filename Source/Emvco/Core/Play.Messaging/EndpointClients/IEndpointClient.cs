using Play.Globalization.Time;

namespace Play.Messaging;

public interface IEndpointClient
{
    #region Instance Members

    public void Subscribe(IMessageChannel messageChannel);
    public void Unsubscribe(IMessageChannel messageChannel);
    public void Send(ResponseMessage message);
    public void Send(RequestMessage message);
    public void Publish(Event @event);

    #endregion
}