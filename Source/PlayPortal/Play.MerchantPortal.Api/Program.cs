using MerchantPortal.Infrastructure.Persistence;
using MerchantPortal.WebApi.Filters;
using MerchantPortal.WebApi.Mapping;
using Play.MerchantPortal.Application;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting {MerchantPortal} up", "MerchantPortal");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //Add Logging. (Serilog)
    //Currently default sink is console. Will change to other provider probably.
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

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("sql");

    builder.Services.AddPersistenceServices(connectionString);
    builder.Services.AddApplicationServices();

    builder.Services.AddAutoMapper(typeof(ProfileModelMapper));

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
