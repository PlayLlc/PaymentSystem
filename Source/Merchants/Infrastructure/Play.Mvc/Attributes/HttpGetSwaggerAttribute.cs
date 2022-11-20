using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;

namespace Play.Mvc.Attributes;

/// <summary>
///     This attribute implements <see cref="HttpGetAttribute" /> and automatically beautifies the Open API Name of the
///     resource that implements it
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HttpGetSwaggerAttribute : HttpGetAttribute
{
    #region Constructor

    public HttpGetSwaggerAttribute([CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "")
    {
        Name = CreateOpenApiName(callerFilePath, memberName);
    }

    public HttpGetSwaggerAttribute(string template, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        Name = CreateOpenApiName(callerFilePath, memberName);
    }

    public HttpGetSwaggerAttribute(
        string template, string name, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        Name = name;
    }

    #endregion

    #region Instance Members

    private static string CreateOpenApiName(string callerFilePath, string memberName)
    {
        var controller = Path.GetFileNameWithoutExtension(callerFilePath);
        TryCapturingAreasName(callerFilePath, out string? areasName);

        return areasName is null ? CreateWithoutAreas(controller, memberName) : CreateWithAreas(controller, memberName, areasName!);
    }

    private static string CreateWithAreas(string controller, string actionMethod, string areas)
    {
        return $"{areas}_{actionMethod}_{FormatControllerName(controller, areas)}";
    }

    private static string CreateWithoutAreas(string controller, string actionMethod)
    {
        return $"{actionMethod}_{FormatControllerName(controller)}";
    }

    private static bool TryCapturingAreasName(string value, out string? areasName)
    {
        areasName = null;
        Regex regex = new Regex(@"(?<areas>\\Areas\\)(?<areasName>(.*?))(?<controllers>\\Controllers\\)");
        var match = regex.Match(value);

        if (!match.Success)
            return false;

        areasName = match.Groups["areasName"].Value;

        return true;
    }

    private static string FormatControllerName(string value, string? areas = null)
    {
        static string ReplaceControllerSubstring(string value)
        {
            return value.EndsWith("Controller") ? value.Substring(0, value.Length - 10) : value;
        }

        if (areas is null)
            return ReplaceControllerSubstring(value);

        return value == "HomeController" ? areas : ReplaceControllerSubstring(value);
    }

    #endregion
}