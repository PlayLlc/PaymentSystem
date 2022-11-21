using Microsoft.AspNetCore.Mvc.Filters;

namespace Play.Mvc.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class SwaggerIgnoreAttribute : ActionFilterAttribute
{ }