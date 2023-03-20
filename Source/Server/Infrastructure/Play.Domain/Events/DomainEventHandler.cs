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

        //SubscribeAll();
    }

    #endregion

    #region Instance Members

    /// <exception cref="TargetInvocationException"></exception>

    //public void SubscribeAll()
    //{
    //    string clrName = typeof(IHandleDomainEvents<DomainEvent>).Name;

    //    IEnumerable<dynamic> handlers = GetType()
    //        .GetInterfaces()
    //        .Where(a => a.Name == clrName)
    //        .Select(a => (dynamic) a.GetMethod("GetInterface")!.Invoke(this, new object?[] { })!);

    //    foreach (dynamic handler in handlers)
    //        Subscribe(handler);
    //}
    protected void Log(DomainEvent domainEvent)
    {
        // TODO: Should we be using the Aggregate's ID as the EventId so we have some kind of correlation happening?
        _Logger.Log(LogLevel.Information, new EventId(domainEvent.GetEventId(), domainEvent.GetEventType()), domainEvent.Description);
    }

    protected void Log(DomainEvent domainEvent, LogLevel logLevel)
    {
        // TODO: Should we be using the Aggregate's ID as the EventId so we have some kind of correlation happening?
        _Logger.Log(logLevel, new EventId(domainEvent.GetEventId(), domainEvent.GetEventType()), domainEvent.Description);
    }

    protected void Log(DomainEvent domainEvent, LogLevel logLevel, string message)
    {
        // TODO: Should we be using the Aggregate's ID as the EventId so we have some kind of correlation happening?
        _Logger.Log(logLevel, new EventId(domainEvent.GetEventId(), domainEvent.GetEventType()), $"{message}; \n\n {domainEvent.Description}");
    }

    protected void Subscribe<_Event>(IHandleDomainEvents<_Event> handler) where _Event : DomainEvent
    {
        DomainEventBus.Subscribe(handler);
    }

    #endregion
}