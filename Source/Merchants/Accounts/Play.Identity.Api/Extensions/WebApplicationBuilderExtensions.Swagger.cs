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

            foreach (var xmlFilePath in Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "*.xml"))
            {
                try
                {
                    c.IncludeXmlComments(xmlFilePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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