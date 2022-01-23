namespace Play.Messaging;

internal class EventBus
{
    #region Instance Values

    private readonly Dictionary<EventTypeId, HashSet<EventHandlerBase>> _HandlerMap;

    #endregion

    #region Constructor

    public EventBus()
    {
        _HandlerMap = new Dictionary<EventTypeId, HashSet<EventHandlerBase>>();
    }

    #endregion

    #region Instance Members

    public void Subscribe(EventHandlerBase handler)
    {
        if (!_HandlerMap.ContainsKey(handler.GetEventTypeId()))
            _HandlerMap.Add(handler.GetEventTypeId(), new HashSet<EventHandlerBase>());

        _HandlerMap[handler.GetEventTypeId()].Add(handler);
    }

    public void Unsubscribe(EventHandlerBase handler)
    {
        if (!_HandlerMap.ContainsKey(handler.GetEventTypeId()))
            _HandlerMap.Add(handler.GetEventTypeId(), new HashSet<EventHandlerBase>());

        _HandlerMap[handler.GetEventTypeId()].Add(handler);
    }

    public async Task Publish(Event @event)
    {
        if (!_HandlerMap.TryGetValue(@event.GetEventTypeId(), out HashSet<EventHandlerBase>? handlers))
            return;

        await Task.Run(() =>
        {
            foreach (EventHandlerBase handler in handlers!.ToArray())
                handler.Handle(@event);
        }).ConfigureAwait(false);
    }

    #endregion
}