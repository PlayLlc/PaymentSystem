namespace Play.Messaging;

// HACK: The event that this class is handling needs to be strongly typed. Otherwise that could lead to runtime errors, which we don't want
public abstract class EventHandlerBase
{
    #region Instance Members

    public abstract EventTypeId GetEventTypeId();
    public abstract Task Handle(Event @event);

    #endregion
}