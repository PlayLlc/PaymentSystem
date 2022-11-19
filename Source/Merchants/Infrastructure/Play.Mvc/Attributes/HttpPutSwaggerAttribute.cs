using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Play.Mvc.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class HttpPutSwaggerAttribute : HttpPutAttribute
{
    #region Constructor

    public HttpPutSwaggerAttribute([CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "")
    {
        Name = $"{FormatControllerName(callerFilePath)}{memberName}";
    }

    public HttpPutSwaggerAttribute(string template, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string memberName = "") : base(template)
    {
        Name = $"{FormatControllerName(callerFilePath)}{memberName}";
    }

    public HttpPutSwaggerAttribute(
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