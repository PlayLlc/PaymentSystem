using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Play.Mvc.Filters;
using Play.Mvc.Swagger;

namespace Play.Mvc.Extensions;

public static class WebApplicationBuilderExtensions
{
    #region Instance Members

    public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder, params Assembly[] xmlFileAssemblies)
    {
        SwaggerConfiguration swaggerConfiguration = builder.Configuration.GetSection(nameof(SwaggerConfiguration)).Get<SwaggerConfiguration>();

        builder.Services.AddSwaggerGen(c =>
        {
            c.DocumentFilter<SwaggerDocumentFilter>();

            c.OperationFilter<SwaggerTagByAreaOperationFilter>();

            foreach (Assembly file in xmlFileAssemblies)
            {
                string xmlFile = $"{file.GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            }

            foreach (string version in swaggerConfiguration.Versions)
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = swaggerConfiguration.ApplicationTitle,
                    Version = version
                });
        });

        return builder;
    }

    #endregion
}