namespace Play.Domain.Events;

internal class DomainEventRouter
{
    #region Instance Values

    private readonly Dictionary<string, HashSet<IHandleDomainEvents<DomainEvent>>> _HandlerMap;

    #endregion

    #region Constructor

    public DomainEventRouter()
    {
        _HandlerMap = new Dictionary<string, HashSet<IHandleDomainEvents<DomainEvent>>>();
    }

    #endregion

    #region Instance Members

    public void Subscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        string fullName = typeof(_Event).FullName!;

        if (!_HandlerMap.ContainsKey(fullName))
            _HandlerMap.Add(fullName, new HashSet<IHandleDomainEvents<DomainEvent>>());
        _HandlerMap[fullName].Add((dynamic) handler);
    }

    public void Unsubscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        string fullName = typeof(_Event).FullName!;
        if (!_HandlerMap.ContainsKey(fullName))
            _HandlerMap.Add(fullName, new HashSet<IHandleDomainEvents<DomainEvent>>());

        _HandlerMap[fullName].Remove((dynamic) handler);
    }

    public void Publish<_Event>(_Event domainEvent) where _Event : DomainEvent
    {
        string fullName = typeof(_Event).FullName!;

        if (!_HandlerMap.TryGetValue(fullName, out HashSet<IHandleDomainEvents<DomainEvent>>? handlers))
            return;

        foreach (IHandleDomainEvents<DomainEvent> handler in handlers!.ToArray())
            handler.Handle(domainEvent);
    }

    #endregion
}