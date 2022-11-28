using Play.Globalization.Time;

namespace Play.Domain.Events;

// TODO: Figure out how to correlate domain events with the Aggregate instance
public abstract record DomainEvent
{
    #region Instance Values

    public readonly DateTimeUtc DateTimeUtc;
    public readonly string Description;
    public DomainEventType DomainEventType = CreateEventTypeId(typeof(DomainEvent));
    private readonly DomainEventIdentifier _DomainEventIdentifier;

    #endregion

    #region Constructor

    protected DomainEvent(string description)
    {
        DateTimeUtc = new DateTimeUtc();
        _DomainEventIdentifier = new DomainEventIdentifier(DomainEventType, DateTimeUtc);
        Description = description;
    }

    #endregion

    #region Instance Members

    public DomainEventType GetEventType() => _DomainEventIdentifier.DomainEventType;

    public int GetEventId() => _DomainEventIdentifier.EventId;

    protected static DomainEventType CreateEventTypeId(Type type) => new DomainEventType(type);

    #endregion
}