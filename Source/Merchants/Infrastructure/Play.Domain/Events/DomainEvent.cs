using Play.Globalization.Time;

namespace Play.Domain.Events;

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

    public DomainEventType GetEventType()
    {
        return _DomainEventIdentifier.DomainEventType;
    }

    public int GetEventId()
    {
        return _DomainEventIdentifier.EventId;
    }

    protected static DomainEventType CreateEventTypeId(Type type)
    {
        return new DomainEventType(type);
    }

    #endregion
}