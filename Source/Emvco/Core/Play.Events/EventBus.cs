namespace Play.Events;

public class EventBus : IRouteEvents
{
    #region Instance Values

    private readonly EventRouter _EventRouter;

    #endregion

    #region Constructor

    public EventBus()
    {
        _EventRouter = new EventRouter();
    }

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

    void IRouteEvents.Publish(EventEnvelope eventEnvelope)
    {
        lock (_EventRouter)
        {
            _EventRouter.Publish(eventEnvelope.GetEvent()).ConfigureAwait(false);
        }
    }

    #endregion
}