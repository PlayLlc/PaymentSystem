namespace Play.Domain.Events;

public interface IHandleDomainEvents<in _Event> where _Event : DomainEvent
{
    #region Instance Members

    public bool Handle(_Event domainEvent);

    #endregion
}

public abstract class DomainEventHandler<_Event> where _Event : DomainEvent
{
    #region Instance Members

    public void Subscribe(IHandleDomainEvents<_Event> handler)
    {
        DomainEventBus.Subscribe(handler);
    }

    #endregion
}