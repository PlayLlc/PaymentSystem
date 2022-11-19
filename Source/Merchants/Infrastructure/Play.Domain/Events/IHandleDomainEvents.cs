namespace Play.Domain.Events;

public interface IHandleDomainEvents<in _Event> where _Event : DomainEvent
{
    #region Instance Members

    public Task Handle(_Event domainEvent);

    public IHandleDomainEvents<_Event> GetInterface()
    {
        return this;
    }

    #endregion
}