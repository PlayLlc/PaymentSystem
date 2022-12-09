using Play.Domain.Aggregates;
using Play.Domain.Entities;
using Play.Globalization.Time;

namespace Play.Domain.Events;

// TODO: Figure out how to correlate domain events with the Aggregate instance
public abstract record DomainEvent
{
    #region Instance Values

    /// <summary>
    ///     The aggregate that is initiating the event
    /// </summary>
    public readonly IAggregate Source;

    /// <summary>
    ///     The entity that the event is being applied to
    /// </summary>
    public readonly IEntity? Target;

    public readonly DateTimeUtc DateTimeUtc;
    public readonly string Description;
    public readonly DomainEventIdentifier DomainEventIdentifier;
    public DomainEventType DomainEventType = CreateEventTypeId(typeof(DomainEvent));

    #endregion

    #region Constructor

    protected DomainEvent(IAggregate source, IEntity? target, string description)
    {
        Source = source;
        Target = target;
        DomainEventIdentifier = new DomainEventIdentifier(DomainEventType, DateTimeUtc);
        DateTimeUtc = new DateTimeUtc();
        Description = description;
    }

    #endregion

    #region Instance Members

    public DomainEventType GetEventType() => DomainEventIdentifier.DomainEventType;
    public int GetEventId() => DomainEventIdentifier.EventId;
    protected static DomainEventType CreateEventTypeId(Type type) => new(type);

    #endregion
}