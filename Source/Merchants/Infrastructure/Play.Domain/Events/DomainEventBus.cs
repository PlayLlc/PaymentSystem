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

    public static void Subscribe(DomainEventHandler domainEventHandler)
    {
        _DomainEventRouter.Subscribe(domainEventHandler);
    }

    public static void Unsubscribe(DomainEventHandler domainEventHandler)
    {
        _DomainEventRouter.Unsubscribe(domainEventHandler);
    }

    public static void Publish(DomainEvent domainEvent)
    {
        _DomainEventRouter.Publish(domainEvent);
    }

    #endregion
}