namespace Play.Domain.Events;

internal class DomainEventRouter
{
    #region Instance Values

    private readonly Dictionary<DomainEventTypeId, HashSet<DomainEventHandler>> _HandlerMap;

    #endregion

    #region Constructor

    public DomainEventRouter()
    {
        _HandlerMap = new Dictionary<DomainEventTypeId, HashSet<DomainEventHandler>>();
    }

    #endregion

    #region Instance Members

    public void Subscribe(DomainEventHandler handler)
    {
        if (!_HandlerMap.ContainsKey(handler.GetEventTypeId()))
            _HandlerMap.Add(handler.GetEventTypeId(), new HashSet<DomainEventHandler>());

        _HandlerMap[handler.GetEventTypeId()].Add(handler);
    }

    public void Unsubscribe(DomainEventHandler handler)
    {
        if (!_HandlerMap.ContainsKey(handler.GetEventTypeId()))
            _HandlerMap.Add(handler.GetEventTypeId(), new HashSet<DomainEventHandler>());

        _HandlerMap[handler.GetEventTypeId()].Add(handler);
    }

    public void Publish(DomainEvent domainEvent)
    {
        if (!_HandlerMap.TryGetValue(domainEvent.GetEventTypeId(), out HashSet<DomainEventHandler>? handlers))
            return;

        foreach (DomainEventHandler handler in handlers!.ToArray())
            handler.Handle(domainEvent);
    }

    #endregion
}