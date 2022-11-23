using Play.Loyalty.Api.Extensions;
using Play.Loyalty.Contracts.Dtos;
using Play.Mvc.Extensions;
using Play.Mvc.Swagger;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
SwaggerConfiguration swaggerConfiguration = builder.Configuration.GetSection(nameof(SwaggerConfiguration)).Get<SwaggerConfiguration>();

builder.ConfigureEntityFramework();
builder.ConfigureServices();
builder.ConfigureSwagger(typeof(Program).Assembly, typeof(LoyaltyProgramDto).Assembly);
await builder.SeedDb().ConfigureAwait(false);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();