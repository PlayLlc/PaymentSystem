using Play.Identity.Api.Extensions;
using Play.Identity.Api.Filters;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureAutoMapper();
builder.ConfigureEntityFramework();
builder.ConfigureIdentityServer();
builder.ConfigureServices();

//await builder.SeedDb().ConfigureAwait(false);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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