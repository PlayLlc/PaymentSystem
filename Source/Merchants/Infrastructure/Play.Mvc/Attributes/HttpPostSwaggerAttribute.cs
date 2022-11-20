using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;

namespace Play.Mvc.Attributes;

/// <summary>
/// This attribute implements <see cref="HttpPostAttribute"/> and automatically beautifies the Open API Name of the resource that implements it 
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HttpPostSwaggerAttribute : HttpPostAttribute
{
    #region Constructor

    public HttpPostSwaggerAttribute([CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "")
    {
        if (!TryCapturingAreasName(callerFilePath, out string? areasName))
        {
            Name = $"{FormatControllerName(Path.GetFileNameWithoutExtension(callerFilePath))}_{memberName}_Post";

            return;
        }

        Name = $"{areasName}_{FormatControllerName(Path.GetFileNameWithoutExtension(callerFilePath))}_{memberName}_Post";
    }

    public HttpPostSwaggerAttribute(string template, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        if (!TryCapturingAreasName(callerFilePath, out string? areasName))
        {
            Name = $"{FormatControllerName(Path.GetFileNameWithoutExtension(callerFilePath))}_{memberName}_Post";

            return;
        }

        Name = $"{areasName}_{FormatControllerName(Path.GetFileNameWithoutExtension(callerFilePath))}_{memberName}_Post";
    }

    public HttpPostSwaggerAttribute(
        string template, string name, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        Name = name;
    }

    #endregion

    #region Instance Members

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

    private static string FormatControllerName(string value)
    {
        return value.EndsWith("Controller") ? value.Substring(0, value.Length - 10) : value;
    }

    #endregion
}