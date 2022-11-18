using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Mvc.Filters
{
    public class SwaggerTagByAreaOperationFilter : IOperationFilter
    {
        #region Instance Members

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var areaName = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AreaAttribute), true)
                    .Cast<AreaAttribute>()
                    .FirstOrDefault();
                if (areaName != null)
                    operation.Tags = new List<OpenApiTag> {new() {Name = areaName.RouteValue}};
                else
                    operation.Tags = new List<OpenApiTag> {new() {Name = controllerActionDescriptor.ControllerName}};
            }
        }

        #endregion
    }
}