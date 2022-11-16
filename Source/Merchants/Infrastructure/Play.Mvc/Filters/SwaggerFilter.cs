using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Play.Mvc.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Play.Mvc.Filters;

public class SwaggerFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var apiDescription in context.ApiDescriptions)
        {
            var controllerActionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(HideInSwaggerAttribute), true).Any() ||
                controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(HideInSwaggerAttribute), true).Any())
            {
                var key = "/" + apiDescription.RelativePath.TrimEnd('/');

                var operation = (OperationType)Enum.Parse(typeof(OperationType), apiDescription.HttpMethod, true);

                swaggerDoc.Paths[key].Operations.Remove(operation);

                // drop the entire route of there are no operations left
                if (!swaggerDoc.Paths[key].Operations.Any())
                {
                    swaggerDoc.Paths.Remove(key);
                }
            }
        }
    }
}
