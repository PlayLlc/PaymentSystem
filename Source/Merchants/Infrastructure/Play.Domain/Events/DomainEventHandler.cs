using Microsoft.Extensions.Logging;

namespace Play.Domain.Events;

public abstract class DomainEventHandler
{
    #region Instance Values

    protected readonly ILogger _Logger;

    #endregion

    #region Constructor

    protected DomainEventHandler(ILogger logger)
    {
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    protected void Log(DomainEvent domainEvent)
    {
        _Logger.Log(LogLevel.Information, new EventId(domainEvent.GetEventId(), domainEvent.GetEventType()), domainEvent.Description);
    }

    public void Subscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        DomainEventBus.Subscribe(handler);
    }

    #endregion
}