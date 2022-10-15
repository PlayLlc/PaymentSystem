namespace Play.Domain.Events;

public interface IHandleDomainEvents<_Event> where _Event : DomainEvent
{
    #region Instance Members

    public abstract Task Handle(_Event domainEvent);

    #endregion
}

public abstract class DomainEventHandler<_Event> : IHandleDomainEvents where _Event : DomainEvent
{
    #region Instance Members

    public abstract DomainEventTypeId GetEventTypeId();

    public void Subscribe(IHandleDomainEvents<_Event> handler)
    {
        DomainEventBus.Subscribe((IHandleDomainEvents < _Event) > handler);
    }

    #endregion
}