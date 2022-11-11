using Microsoft.EntityFrameworkCore;
using Play.Merchants.Underwriting.Persistence.Persistence;
using Play.Merchants.Underwriting.Scheduled.Extensions;
using Play.Mvc.Filters;
using Play.Underwriting.DataServices.USTreasury;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Persistence.Sql.Repositories;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
string? connectionString = builder.Configuration.GetConnectionString("Underwriting");

builder.Services.AddControllers();
builder.Services.AddDbContext<UnderwritingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IImportIndividualsRepository, ImportIndividualsRepository>();
builder.Services.AddHttpClient<IUsTreasuryClient, UsTreasuryClient>(client =>
{
    client.BaseAddress = new Uri("https://www.treasury.gov/");
});

//builder.Services.RegisterSchedulingConfiguration(builder.Configuration);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();
    app.UseHsts();
    app.MapControllers();
    app.Run();
}