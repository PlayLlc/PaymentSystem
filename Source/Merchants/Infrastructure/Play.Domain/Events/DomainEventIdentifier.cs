using Play.Globalization.Time;

namespace Play.Domain.Events;

internal record DomainEventIdentifier : IEqualityComparer<DomainEventIdentifier>
{
    #region Instance Values

    public readonly int EventId;
    public readonly DomainEventType DomainEventType;

    #endregion

    #region Constructor

    public DomainEventIdentifier(DomainEventType domainEventType, DateTimeUtc dateTime)
    {
        unchecked
        {
            EventId = (int) dateTime.Ticks;
        }

        DomainEventType = domainEventType;
    }

    #endregion

    #region Equality

    public bool Equals(DomainEventIdentifier? x, DomainEventIdentifier? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DomainEventIdentifier obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}