namespace Play.Events;

/// <summary>
///     Uniquely identifies an event type
/// </summary>
public readonly record struct EventTypeId
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public EventTypeId(ushort eventTypeIdentifier)
    {
        _Value = eventTypeIdentifier;
    }

    #endregion
}