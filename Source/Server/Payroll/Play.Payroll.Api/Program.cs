using Play.Logging.Serilog;
using Play.Loyalty.Api.Extensions;
using Play.Mvc.Extensions;
using Play.Mvc.Filters;
using Play.Mvc.Swagger;
using Play.Payroll.Contracts.Dtos;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
SwaggerConfiguration swaggerConfiguration = builder.Configuration.GetSection(nameof(SwaggerConfiguration)).Get<SwaggerConfiguration>();

builder.Host.ConfigureSerilogForMvc();
builder.ConfigureEntityFramework();
builder.ConfigureServices();
builder.ConfigureSwagger(typeof(Program).Assembly, typeof(EmployerDto).Assembly);

//await builder.SeedDb().ConfigureAwait(false);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/{swaggerConfiguration.Versions.Max()}/swagger.json", swaggerConfiguration.ApplicationTitle);
    });
}

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller}/{action}/{id?}");
app.Run();