namespace Play.Messaging;

internal interface IRouteMessages : ICreateEndpointClient
{
    void Subscribe(IMessageChannel messageChannel);
    void Unsubscribe(ChannelIdentifier channelIdentifier);
    void Send(RequestMessageEnvelope messageEnvelop);
    void Send(ResponseMessageEnvelope messageEnvelop);
    void Subscribe(EventHandlerBase eventHandler);
    void Unsubscribe(EventHandlerBase eventHandler);
    Task Publish(EventEnvelope eventEnvelope);
}