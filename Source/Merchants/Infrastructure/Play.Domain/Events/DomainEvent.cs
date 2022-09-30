using Play.Globalization.Time;

namespace Play.Domain.Events;

public abstract record DomainEvent
{
    #region Instance Values

    public readonly DateTimeUtc DateTimeUtc;

    private readonly DomainEventIdentifier _DomainEventIdentifier;

    #endregion

    #region Constructor

    protected DomainEvent(DomainEventTypeId domainEventTypeId)
    {
        _DomainEventIdentifier = new DomainEventIdentifier(domainEventTypeId);

        DateTimeUtc = new DateTimeUtc();
    }

    #endregion

    #region Instance Members

    public DomainEventTypeId GetEventTypeId()
    {
        return _DomainEventIdentifier.DomainEventTypeId;
    }

    public Guid GetEventId()
    {
        return _DomainEventIdentifier.EventId;
    }

    /// <exception cref="ArgumentNullException"></exception>
    protected static DomainEventTypeId CreateEventTypeId(Type type)
    {
        return new DomainEventTypeId(type!.FullName ?? throw new ArgumentNullException(nameof(type)));
    }

    #endregion
}