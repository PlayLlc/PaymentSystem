using Play.Identity.Api.Extensions;
using Play.Identity.Api.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.ConfigureIdentityServer();
builder.ConfigureApplicationServices();
await builder.SeedDefaultIdentityData().ConfigureAwait(false);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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