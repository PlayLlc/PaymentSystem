﻿namespace Play.Domain.Events;

internal class DomainEventRouter
{
    #region Instance Values

    private readonly Dictionary<DomainEventTypeId, HashSet<IHandleDomainEvents>> _HandlerMap;

    #endregion

    #region Constructor

    public DomainEventRouter()
    {
        _HandlerMap = new Dictionary<DomainEventTypeId, HashSet<IHandleDomainEvents>>();
    }

    #endregion

    #region Instance Members

    public void Subscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        if (!_HandlerMap.ContainsKey(handler.GetEventTypeId()))
            _HandlerMap.Add(handler.GetEventTypeId(), new HashSet<IHandleDomainEvents>());

        _HandlerMap[handler.GetEventTypeId()].Add(handler);
    }

    public void Unsubscribe(IHandleDomainEvents handler)
    {
        if (!_HandlerMap.ContainsKey(handler.GetEventTypeId()))
            _HandlerMap.Add(handler.GetEventTypeId(), new HashSet<IHandleDomainEvents>());

        _HandlerMap[handler.GetEventTypeId()].Add(handler);
    }

    public void Publish<_T>(_T domainEvent) where _T : DomainEvent
    {
        if (!_HandlerMap.TryGetValue(domainEvent.GetEventTypeId(), out HashSet<IHandleDomainEvents>? handlers))
            return;

        foreach (IHandleDomainEvents handleDomainEvents in handlers!.ToArray())
        {
            DomainEventHandler<_T> handler = (DomainEventHandler<_T>) handleDomainEvents;
            handler.Handle(domainEvent);
        }
    }

    #endregion
}