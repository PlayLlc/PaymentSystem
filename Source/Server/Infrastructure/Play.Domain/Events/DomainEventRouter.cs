﻿namespace Play.Domain.Events;

internal class DomainEventRouter
{
    #region Instance Values

    private readonly Dictionary<string, HashSet<dynamic>> _HandlerMap;

    #endregion

    #region Constructor

    public DomainEventRouter()
    {
        _HandlerMap = new Dictionary<string, HashSet<dynamic>>();
    }

    #endregion

    #region Instance Members

    public void Subscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        string fullName = typeof(_Event).FullName!;

        if (!_HandlerMap.ContainsKey(fullName))
            _HandlerMap.Add(fullName, new HashSet<dynamic>());
        _HandlerMap[fullName].Add(handler);
    }

    public void Unsubscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        string fullName = typeof(_Event).FullName!;
        if (!_HandlerMap.ContainsKey(fullName))
            _HandlerMap.Add(fullName, new HashSet<dynamic>());

        _HandlerMap[fullName].Remove((dynamic) handler);
    }

    public async Task Publish(dynamic domainEvent) //where _Event : DomainEvent
    {
        string fullName = domainEvent.GetType().FullName!;

        if (!_HandlerMap.TryGetValue(fullName, out HashSet<dynamic>? handlers))
            return;

        foreach (dynamic handler in handlers!.ToArray())
            await handler.Handle(domainEvent).ConfigureAwait(false);
    }

    #endregion
}