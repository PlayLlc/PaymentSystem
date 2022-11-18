using System.Net;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Restful.Clients;

public class ApiException : PlayException
{
    #region Instance Values

    /// <summary>
    ///     Gets or sets the error code (HTTP status code)
    /// </summary>
    /// <value>The error code (HTTP status code).</value>
    public HttpStatusCode StatusCode { get; set; }

    #endregion

    #region Constructor

    protected ApiException(string message) : base(message)
    { }

    public ApiException(
        HttpStatusCode statusCode, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} $\"An error occurred attempting to access an API causing an HTTP Status Code: [{{statusCode}}]; \\n\\n{message}")
    {
        StatusCode = statusCode;
    }

    public ApiException(
        int statusCode, string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} $\"An error occurred attempting to access an API causing an HTTP Status Code: [{{statusCode}}]; \\n\\n{message}; \\n\\n{innerException.Message}")
    {
        StatusCode = (HttpStatusCode) statusCode;
    }

    public ApiException(
        int statusCode, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        : base(
            $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} $\"An error occurred attempting to access an API causing an HTTP Status Code: [{{statusCode}}]; \\n\\n{message}")
    {
        StatusCode = (HttpStatusCode) statusCode;
    }

    public ApiException(
        int statusCode, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} $\"An error occurred attempting to access an API causing an HTTP Status Code: [{{statusCode}}]; {innerException.Message}; \\n\\n{innerException.Message}")
    {
        StatusCode = (HttpStatusCode) statusCode;
    }

    public ApiException(
        HttpStatusCode statusCode, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} $\"An error occurred attempting to access an API causing an HTTP Status Code: [{{statusCode}}]; {innerException.Message}; \\n\\n{innerException.Message}")
    {
        StatusCode = (HttpStatusCode) statusCode;
    }

    public ApiException(
        HttpStatusCode statusCode, string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} $\"An error occurred attempting to access an API causing an HTTP Status Code: [{{statusCode}}]; \\n\\n{message}; \\n\\n{innerException.Message}")
    {
        StatusCode = statusCode;
    }

    public ApiException(string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        : base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    public ApiException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public ApiException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}