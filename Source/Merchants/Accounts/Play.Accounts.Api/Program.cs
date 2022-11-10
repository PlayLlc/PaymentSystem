using Serilog;

using Play.Accounts.Api.Extensions;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build();

    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

    builder.ConfigureServices();

    WebApplication app = builder.Build();

    app.ConfigurePipeline();
}
catch (Exception ex)
{
    Log.Fatal(ex, $"Unhandled exception starting {nameof(Play.Accounts)}");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}