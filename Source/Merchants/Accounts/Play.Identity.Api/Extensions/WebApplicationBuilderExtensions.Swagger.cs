using Microsoft.OpenApi.Models;

using System.Reflection;

using Play.Mvc.Swagger;

namespace Play.Identity.Api.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    internal static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
    {
        SwaggerConfiguration swaggerConfiguration = builder.Configuration.GetSection(nameof(SwaggerConfiguration)).Get<SwaggerConfiguration>();

        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            foreach (var version in swaggerConfiguration.Versions)
                c.SwaggerDoc(version, new OpenApiInfo()
                {
                    Title = swaggerConfiguration.ApplicationTitle,
                    Version = version
                });
        });

        return builder;
    }

    #endregion
}