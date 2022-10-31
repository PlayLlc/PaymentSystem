using System.Reflection;

using Microsoft.Extensions.Logging;

namespace Play.Domain.Events;

public abstract class DomainEventHandler
{
    #region Instance Values

    protected readonly ILogger _Logger;

    #endregion

    #region Constructor

    /// <exception cref="TargetInvocationException"></exception>
    protected DomainEventHandler(ILogger logger)
    {
        _Logger = logger;
        SubscribeAll();
    }

    #endregion

    #region Instance Members

    /// <exception cref="TargetInvocationException"></exception>
    public void SubscribeAll()
    {
        var clrName = typeof(IHandleDomainEvents<DomainEvent>).Name;

        IEnumerable<dynamic> handlers = GetType()
            .GetInterfaces()
            .Where(a => a.Name == clrName)
            .Select(a => (dynamic) a.GetMethod("GetInterface")!.Invoke(this, new object?[] { })!);

        foreach (var handler in handlers)
            Subscribe(handler);
    }

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