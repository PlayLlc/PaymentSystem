namespace Play.Domain.Events;

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
        _HandlerMap[fullName].Add((dynamic) handler);
    }

    public void Unsubscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        string fullName = typeof(_Event).FullName!;
        if (!_HandlerMap.ContainsKey(fullName))
            _HandlerMap.Add(fullName, new HashSet<dynamic>());

        _HandlerMap[fullName].Remove((dynamic) handler);
    }

    public void Publish<_Event>(_Event domainEvent) where _Event : DomainEvent
    {
        string fullName = typeof(_Event).FullName!;

        if (!_HandlerMap.TryGetValue(fullName, out HashSet<dynamic>? handlers))
            return;

        foreach (IHandleDomainEvents<DomainEvent> handler in handlers!.ToArray())
            handler.Handle(domainEvent);
    }

    #endregion
}