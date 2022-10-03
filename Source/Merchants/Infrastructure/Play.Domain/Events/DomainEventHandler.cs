namespace Play.Domain.Events;

public interface IHandleDomainEvents
{
    #region Instance Members

    public DomainEventTypeId GetEventTypeId();

    #endregion
}

public abstract class DomainEventHandler<_Event> : IHandleDomainEvents where _Event : DomainEvent
{
    #region Instance Members

    public abstract DomainEventTypeId GetEventTypeId();

    public abstract Task Handle(_Event domainEvent);

    #endregion
}