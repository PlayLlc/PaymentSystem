namespace Play.Messaging;

internal interface IRouteMessages : ICreateEndpointClient
{
    #region Instance Members

    void Subscribe(IMessageChannel messageChannel);
    void Unsubscribe(ChannelIdentifier channelIdentifier);
    void Send(RequestMessageEnvelope messageEnvelop);
    void Send(ResponseMessageEnvelope messageEnvelop);
    void Subscribe(EventHandlerBase eventHandler);
    void Unsubscribe(EventHandlerBase eventHandler);
    Task Publish(EventEnvelope eventEnvelope);

    #endregion
}