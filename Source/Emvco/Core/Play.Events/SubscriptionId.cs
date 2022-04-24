namespace Play.Events;

public readonly record struct SubscriptionId
{
    #region Instance Values

    private readonly EventHandlerId _EventHandlerId;
    private readonly EventTypeId _EventTypeId;

    #endregion

    #region Constructor

    public SubscriptionId(EventHandlerId eventHandlerId, EventTypeId eventTypeId)
    {
        _EventHandlerId = eventHandlerId;
        _EventTypeId = eventTypeId;
    }

    #endregion

    #region Instance Members

    public EventHandlerId GetEventHandlerId() => _EventHandlerId;
    public EventTypeId GetEventTypeId() => _EventTypeId;

    #endregion
}