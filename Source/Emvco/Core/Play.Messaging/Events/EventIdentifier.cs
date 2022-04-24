namespace Play.Messaging;

public record EventIdentifier : IEqualityComparer<EventIdentifier>
{
    #region Instance Values

    private readonly InstanceId _InstanceId;
    private readonly EventTypeId _EventTypeId;

    #endregion

    #region Constructor

    public EventIdentifier(EventTypeId eventTypeId)
    {
        _InstanceId = new InstanceId();
        _EventTypeId = eventTypeId;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     The identifier of this event type
    /// </summary>
    /// <returns></returns>
    public EventTypeId GetEventTypeId() => _EventTypeId;

    #endregion

    #region Equality

    public bool Equals(EventIdentifier? x, EventIdentifier? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(EventIdentifier obj) => obj.GetHashCode();

    #endregion
}