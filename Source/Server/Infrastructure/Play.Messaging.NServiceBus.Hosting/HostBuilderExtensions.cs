using Microsoft.Extensions.Hosting;

using NServiceBus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NServiceBus.Logging;

using Play.Logging.Serilog;

using Serilog;

namespace Play.Messaging.NServiceBus.Hosting;

public static class HostBuilderExtensions
{
    #region Instance Members

    public static IHostBuilder ConfigureNServiceBus(this IHostBuilder builder, Assembly hostAssembly)
    {
        ConfigureSerilogForNServiceBus(builder);

        builder.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration(hostAssembly.GetName().Name);
            var connectionString = context.Configuration.GetConnectionString("AzureServiceBus");
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
            endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
            endpointConfiguration.EnableInstallers();

            return endpointConfiguration;
        });

        return builder;
    }

    private static IHostBuilder ConfigureSerilogForNServiceBus(this IHostBuilder builder)
    {
        var configuration = new LoggerConfiguration();
        LogManager.Use<SerilogFactory>();
        builder.ConfigureSerilog();
        configuration.Enrich.WithNsbExceptionDetails();

        builder.UseConsoleLifetime();

        builder.ConfigureLogging((ctx, logging) =>
        {
            logging.AddEventLog();
            logging.AddConsole();
        });

        return builder;
    }

    private static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
    {
        var fatalMessage = "The following critical error was "
                           + $"encountered: {Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. "
                           + $"StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";

        // Useful if running on Windows
        // EventLog.WriteEntry(".NET Runtime", fatalMessage, EventLogEntryType.Error);

        try
        {
            await context.Stop(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            Environment.FailFast(fatalMessage, context.Exception);
        }
    }

    #endregion
}