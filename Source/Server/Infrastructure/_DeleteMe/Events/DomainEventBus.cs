namespace Play.Domain.Events;

internal static class DomainEventBus
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

    public static void Subscribe<_Event>(IHandleDomainEvents<_Event> domainEventHandler) where _Event : DomainEvent
    {
        _DomainEventRouter.Subscribe(domainEventHandler);
    }

    public static void Publish<_Event>(_Event domainEvent) where _Event : DomainEvent
    {
        _DomainEventRouter.Publish(domainEvent);
    }

    #endregion
}