namespace Play.Domain.Events;

public static class DomainEventBus
{
    #region Static Metadata

    private static readonly DomainEventRouter _DomainEventRouter;

    #endregion

    #region Constructor

    static DomainEventBus()
    {
        _DomainEventRouter = new DomainEventRouter();
    }

    #endregion

    #region Events

    public static void Subscribe(IHandleDomainEvents domainEventHandler)
    {
        _DomainEventRouter.Subscribe(domainEventHandler);
    }

    public static void Unsubscribe(IHandleDomainEvents domainEventHandler)
    {
        _DomainEventRouter.Unsubscribe(domainEventHandler);
    }

    public static void Publish<_Event>(_Event domainEvent) where _Event : DomainEvent
    {
        _DomainEventRouter.Publish(domainEvent);
    }

    #endregion
}