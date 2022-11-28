using Microsoft.EntityFrameworkCore;

using Play.Mvc.Filters;
using Play.Underwriting.Application;
using Play.Underwriting.Application.DataServices.USTreasury;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Persistence.Sql.Persistence;
using Play.Underwriting.Persistence.Sql.Repositories;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Configure Services
IServiceCollection services = builder.Services;
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

builder.Services.RegisterSchedulingConfiguration(builder.Configuration);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

if (builder.Environment.IsDevelopment())
    builder.Services.AddSwaggerGen();

//Configure pipeline.
WebApplication app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/error");
    }

    app.UseHttpsRedirection();

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.MapControllers();
    app.Run();
}