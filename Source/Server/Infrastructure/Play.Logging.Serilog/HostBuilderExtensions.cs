using Microsoft.Extensions.Hosting;

using Serilog;

namespace Play.Logging.Serilog;

public static class HostBuilderExtensions
{
    #region Instance Members

    public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((ctx, lc) =>
        {
            lc.Enrich.FromLogContext();
            lc.Enrich.With(new MachineNameEnricher());
            lc.Enrich.With(new ProcessIdEnricher());
            lc.Enrich.With(new ThreadIdEnricher());
            lc.MinimumLevel.Information()
                .WriteTo.Console(
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}");

            lc.MinimumLevel.Debug()
                .WriteTo.File($"Logging_{DateTime.UtcNow}_",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10, fileSizeLimitBytes: 2147483648, rollOnFileSizeLimit: true)
                .CreateLogger();

            lc.WriteTo.Console(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}").CreateLogger();
            lc.MinimumLevel.Debug().WriteTo.Console().CreateLogger();

            lc.ReadFrom.Configuration(ctx.Configuration);
        });

        return builder;
    }

    #endregion
}