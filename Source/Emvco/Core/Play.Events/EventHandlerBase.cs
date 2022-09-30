using System.Threading.Tasks;

namespace Play.Events;

public abstract class EventHandlerBase
{
    #region Instance Members

    public abstract EventTypeId GetEventTypeId();
    public abstract Task Handle(Event @event);

    #endregion
}