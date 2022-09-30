namespace Play.Domain.Events;

internal record DomainEventIdentifier : IEqualityComparer<DomainEventIdentifier>
{
    #region Instance Values

    public readonly Guid EventId;
    public readonly DomainEventTypeId DomainEventTypeId;

    #endregion

    #region Constructor

    public DomainEventIdentifier(DomainEventTypeId domainEventTypeId)
    {
        EventId = Guid.NewGuid();
        DomainEventTypeId = domainEventTypeId;
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