using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Domain.EventSourcing.Aggregates;

public abstract class EventSourcedAggregate<_TId> : Aggregate<_TId> where _TId : IEquatable<_TId>
{
    #region Static Metadata

    private const long _NewAggregateVersion = -1;

    #endregion

    #region Instance Values

    private readonly ICollection<DomainEvent> _UncommittedEvents = new LinkedList<DomainEvent>();
    private long _Version = _NewAggregateVersion;

    #endregion

    #region Instance Members

    protected abstract void Apply(dynamic @event);

    protected void ApplyEvent(DomainEvent domainEvent, long version)
    {
        if (!_UncommittedEvents.Any(x => x.GetEventId() == domainEvent.GetEventId()))
        {
            this.Apply((dynamic) domainEvent);
            _Version = version;
        }
    }

    protected IEnumerable<DomainEvent> GetUncommittedEvents() => _UncommittedEvents;

    private void ClearUncommittedEvents()
    {
        _UncommittedEvents.Clear();
    }

    #endregion
}