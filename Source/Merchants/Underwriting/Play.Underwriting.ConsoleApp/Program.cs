using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Play.Merchants.Underwriting.Persistence.Persistence;
using Play.Merchants.Underwriting.Scheduled.Extensions;
using Play.Underwriting.DataServices.USTreasury;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Jobs;
using Play.Underwriting.Persistence.Sql.Repositories;
using Quartz.Impl;
using Serilog;

var builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostBuilderContext, configuration) =>
{
    configuration.SetBasePath(Directory.GetCurrentDirectory());
    configuration.AddJsonFile("appsettings.json");
    configuration.AddEnvironmentVariables(prefix: "ENV_");
    configuration.AddCommandLine(args);
});

builder.ConfigureServices((hostContext, services) =>
{
    string? connectionString = hostContext.Configuration.GetConnectionString("Underwriting");

    services.AddDbContext<UnderwritingDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });
    services.AddScoped<IUnderwritingRepository, UnderwritingRepository>();
    services.AddHttpClient<IUsTreasuryClient, UsTreasuryClient>(client =>
    {
        client.BaseAddress = new Uri("https://www.treasury.gov/");
    });
    
    services.RegisterSchedulingConfiguration(hostContext.Configuration);
});

builder.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

await app.RunAsync();
Console.ReadKey();



