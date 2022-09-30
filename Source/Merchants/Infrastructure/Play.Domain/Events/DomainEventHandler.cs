namespace Play.Domain.Events;

public abstract class DomainEventHandler
{
    #region Instance Members

    public abstract DomainEventTypeId GetEventTypeId();
    public abstract void Handle(DomainEvent domainEvent);

    #endregion
}