using Microsoft.EntityFrameworkCore;
using Play.Merchants.Underwriting.Persistence.Persistence;
using Play.Merchants.Underwriting.Scheduled.Extensions;
using Play.Underwriting.DataServices.USTreasury;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Persistence.Sql.Repositories;
using Serilog;

//var builder = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostBuilderContext, configuration) =>
//{
//    configuration.SetBasePath(Directory.GetCurrentDirectory());
//    configuration.AddJsonFile("appsettings.json");
//    configuration.AddEnvironmentVariables(prefix: "ENV_");
//    configuration.AddCommandLine(args);
//});

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
string? connectionString = builder.Configuration.GetConnectionString("Underwriting");

services.AddDbContext<UnderwritingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
services.AddScoped<IUnderwritingRepository, UnderwritingRepository>();
services.AddHttpClient<IUsTreasuryClient, UsTreasuryClient>(client =>
{
    client.BaseAddress = new Uri("https://www.treasury.gov/");
});

services.RegisterSchedulingConfiguration(builder.Configuration);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();
app.Run();



