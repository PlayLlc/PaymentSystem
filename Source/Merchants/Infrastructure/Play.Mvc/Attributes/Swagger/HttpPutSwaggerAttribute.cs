﻿using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;

namespace Play.Mvc.Attributes;

/// <summary>
///     This attribute implements <see cref="HttpPutAttribute" /> and automatically beautifies the Open API Name of the
///     resource that implements it
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HttpPutSwaggerAttribute : HttpPutAttribute
{
    #region Constructor

    public HttpPutSwaggerAttribute([CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "")
    {
        Name = CreateOpenApiName(callerFilePath, memberName);
    }

    public HttpPutSwaggerAttribute(
        string template, string name = "", [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;
        else
            Name = CreateOpenApiName(callerFilePath, memberName);
    }

    public HttpPutSwaggerAttribute(string name)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;
    }

    #endregion

    #region Instance Members

    private static string FormatActionMethodName(string actionMethod)
    {
        if (actionMethod == "Index")
            return "Update";

        return actionMethod;
    }

    private static string CreateOpenApiName(string callerFilePath, string memberName)
    {
        string controller = Path.GetFileNameWithoutExtension(callerFilePath);
        TryCapturingAreasName(callerFilePath, out string? areasName);

        return areasName is null ? CreateWithoutAreas(controller, memberName) : CreateWithAreas(controller, memberName, areasName!);
    }

    private static string CreateWithAreas(string controller, string actionMethod, string areas) =>
        $"{areas}_{FormatActionMethodName(actionMethod)}_{FormatControllerName(controller, areas)}";

    private static string CreateWithoutAreas(string controller, string actionMethod) =>
        $"{FormatActionMethodName(actionMethod)}_{FormatControllerName(controller)}";

    private static bool TryCapturingAreasName(string value, out string? areasName)
    {
        areasName = null;
        Regex regex = new Regex(@"(?<areas>\\Areas\\)(?<areasName>(.*?))(?<controllers>\\Controllers\\)");
        Match match = regex.Match(value);

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