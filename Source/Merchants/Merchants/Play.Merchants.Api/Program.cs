using FluentValidation.AspNetCore;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using Microsoft.OpenApi.Models;

using Play.Merchants.Api.Filters;
using Play.Merchants.Application;
using Play.Merchants.Persistence.Mongo;
using Play.Merchants.Persistence.Sql;

using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information("Starting {MerchantPortal} up", "MerchantPortal");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    //Add Logging. (Serilog)
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
            loggerConfiguration.WriteTo.Console();

        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiFilterExceptionAttribute>();
    });

    // Add services to the container.
    builder.Services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true)
        .AddFluentValidationClientsideAdapters()
        .AddFluentValidationRulesToSwagger();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    string connectionString = builder.Configuration.GetConnectionString("sql");

    builder.Services.AddMongoPersistenceServices();
    builder.Services.AddSqlPersistenceServices(connectionString);
    builder.Services.AddApplicationServices();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "Merchant Portal", Version = "v1"});
    });

    WebApplication app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled startup exception");
}
finally
{
    Log.CloseAndFlush();
}