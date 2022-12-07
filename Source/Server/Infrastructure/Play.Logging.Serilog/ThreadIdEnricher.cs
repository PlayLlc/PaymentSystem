using Serilog.Core;
using Serilog.Events;

namespace Play.Logging.Serilog;

internal class ThreadIdEnricher : ILogEventEnricher
{
    #region Instance Members

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadID", Thread.CurrentThread.ManagedThreadId));
    }

    #endregion
}