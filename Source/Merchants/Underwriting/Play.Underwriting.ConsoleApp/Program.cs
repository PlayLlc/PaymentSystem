using Microsoft.EntityFrameworkCore;
using Play.Merchants.Underwriting.Persistence.Persistence;
using Play.Merchants.Underwriting.Scheduled.Extensions;
using Play.Underwriting.DataServices.USTreasury;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Persistence.Sql.Repositories;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
string? connectionString = builder.Configuration.GetConnectionString("Underwriting");

services.AddControllers();
services.AddDbContext<UnderwritingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

services.AddScoped<IImportIndividualsRepository, ImportIndividualsRepository>();
services.AddHttpClient<IUsTreasuryClient, UsTreasuryClient>(client =>
{
    client.BaseAddress = new Uri("https://www.treasury.gov/");
});

//services.RegisterSchedulingConfiguration(builder.Configuration);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();
    //app.UseAuthentication();
    //app.UseAuthorization();
    app.MapControllers();
    app.Run();
}