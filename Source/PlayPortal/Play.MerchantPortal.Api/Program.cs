using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Play.MerchantPortal.Api.AuthorizationHandlers;
using Play.MerchantPortal.Api.Filters;
using Play.MerchantPortal.Api.Mapping;
using Play.MerchantPortal.Api.Requirements;
using Play.MerchantPortal.Application;
using Play.MerchantPortal.Infrastructure.Persistence;

using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

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

    builder.Services
        .AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true)
        .AddFluentValidationClientsideAdapters()
        .AddFluentValidationRulesToSwagger();

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = "https://localhost:7191";

            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateAudience = false,
                //ValidateIssuerSigningKey = true,
                //ValidateIssuer = true,
                //ValidIssuer = "http://localhost:5000/",
                //IssuerSigningKey = new X509SecurityKey(new X509Certificate2(certLocation)), //we will probably use RSA to sign our access tokens.
            };
        });

    builder.Services.AddSingleton<IAuthorizationHandler, ApiScopeAuthorizationHandler>();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("MerchantPortalApiScope", policy =>
        {
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);

            policy.RequireAuthenticatedUser();

            policy.Requirements.Add(new ApiScopeRequirement());
        });
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    var connectionString = builder.Configuration.GetConnectionString("sql");

    builder.Services.AddPersistenceServices(connectionString);
    builder.Services.AddApplicationServices();
    builder.Services.AddAutoMapper(typeof(ProfileModelMapper));

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Merchant Portal", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
           {
             new OpenApiSecurityScheme
             {
               Reference = new OpenApiReference
               {
                 Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
               }
              },
              new string[] { }
            }
        });
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

    app.UseAuthentication();
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