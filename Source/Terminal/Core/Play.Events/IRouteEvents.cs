namespace Play.Events;

internal interface IRouteEvents
{
    #region Instance Members

    void Subscribe(EventHandlerBase eventHandler);
    void Unsubscribe(EventHandlerBase eventHandler);
    void Publish(EventEnvelope eventEnvelope);

    #endregion
}