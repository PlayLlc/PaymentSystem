namespace Play.Domain.Events;

public interface IHandleDomainEvents
{
    #region Instance Members

    public DomainEventTypeId GetEventTypeId();

    #endregion
}