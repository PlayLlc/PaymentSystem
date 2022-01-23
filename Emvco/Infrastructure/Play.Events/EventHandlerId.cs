namespace Play.Events;

/// <summary>
///     Uniquely identifies an event type
/// </summary>
public readonly record struct EventHandlerId
{
    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public EventHandlerId(ushort eventHandlerId)
    {
        _Value = eventHandlerId;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator ushort(EventHandlerId value) => value._Value;

    #endregion
}