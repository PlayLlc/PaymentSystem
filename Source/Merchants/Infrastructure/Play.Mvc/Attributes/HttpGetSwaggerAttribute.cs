﻿using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

namespace Play.Mvc.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class HttpGetSwaggerAttribute : HttpGetAttribute
{
    #region Constructor

    public HttpGetSwaggerAttribute([CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "")
    {
        Name = $"{FormatControllerName(callerFilePath)}{memberName}";
    }

    public HttpGetSwaggerAttribute(string template, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        Name = $"{FormatControllerName(callerFilePath)}{memberName}";
    }

    public HttpGetSwaggerAttribute(
        string template, string name, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        Name = name;
    }

    #endregion

    #region Instance Members

    private static string FormatControllerName(string value)
    {
        return value.EndsWith("Controller") ? value.Substring(0, value.Length - 10) : value;
    }

    #endregion
}