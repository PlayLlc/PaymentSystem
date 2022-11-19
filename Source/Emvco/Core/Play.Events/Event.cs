namespace Play.Events;

public abstract record Event
{
    #region Instance Values

    private readonly EventIdentifier _MessageIdentifier;

    #endregion

    #region Constructor

    protected Event(EventTypeId eventTypeId)
    {
        _MessageIdentifier = new EventIdentifier(eventTypeId);
    }

    #endregion

    #region Instance Members

    public EventTypeId GetEventTypeId() => _MessageIdentifier.GetEventTypeId();

    #endregion
}