namespace Play.Messaging;

public interface IMessageBus
{
    #region Instance Members

    public void Subscribe(IMessageChannel messageChannel);
    public void Unsubscribe(ChannelIdentifier channelIdentifier);
    public void Subscribe(EventHandlerBase eventHandler);
    public void Unsubscribe(EventHandlerBase eventHandler);
    public IEndpointClient CreateEndpointClient();

    #endregion
}

internal interface IRouteMessages : IMessageBus
{
    #region Instance Members

    void Send(RequestMessageEnvelope messageEnvelop);
    void Send(ResponseMessageEnvelope messageEnvelop);
    Task Publish(EventEnvelope eventEnvelope);

    #endregion
}