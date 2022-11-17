using Microsoft.OpenApi.Models;

using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Play.Mvc.Swagger;
using Play.Mvc.Filters;

namespace Play.Mvc.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    #region Instance Members

    public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder, params Assembly[] xmlFileAssemblies)
    {
        SwaggerConfiguration swaggerConfiguration = builder.Configuration.GetSection(nameof(SwaggerConfiguration)).Get<SwaggerConfiguration>();

        builder.Services.AddSwaggerGen(c =>
        {
            c.DocumentFilter<SwaggerDocumentFilter>();

            foreach (var file in xmlFileAssemblies)
            {
                string xmlFile = $"{file.GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            }

            foreach (string version in swaggerConfiguration.Versions)
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