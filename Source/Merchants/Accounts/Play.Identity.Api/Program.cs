using ES.PurchaseAdjustment.Configuration;

using Microsoft.OpenApi.Models;

using Play.Identity.Api.Extensions;
using Play.Identity.Api.Filters;

using Serilog;

using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
SwaggerConfiguration swaggerConfiguration = builder.Configuration.GetSection(nameof(SwaggerConfiguration)).Get<SwaggerConfiguration>();

builder.ConfigureAutoMapper();
builder.ConfigureEntityFramework();
builder.ConfigureIdentityServer();
builder.ConfigureServices();
builder.ConfigureSwagger();
await builder.SeedDb().ConfigureAwait(false);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});

builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/{swaggerConfiguration.Versions.Max()}/swagger.json", swaggerConfiguration.ApplicationTitle);
    });
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute("areas", "{area:exists}/{controller=User}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();