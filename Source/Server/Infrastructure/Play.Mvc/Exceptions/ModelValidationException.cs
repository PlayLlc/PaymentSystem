using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Play.Core.Exceptions;

namespace Play.Mvc.Exceptions;

public class ModelValidationException : PlayException
{
    #region Constructor

    protected ModelValidationException(string message, Exception innerException) : base(message, innerException)
    { }

    protected ModelValidationException(string message) : base(message)
    { }

    public ModelValidationException(
        ModelStateDictionary modelState, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(ModelValidationException), fileName, memberName, lineNumber)}. Errors: [{modelState?.Values.SelectMany(v => v.Errors)?.Select(e => e.ErrorMessage) ?? Array.Empty<string>()}]")
    { }

    public ModelValidationException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(ModelValidationException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public ModelValidationException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(ModelValidationException), fileName, memberName, lineNumber)} {message}")
    { }

    public ModelValidationException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(ModelValidationException), fileName, memberName, lineNumber)}", innerException)
    { }

    public ModelValidationException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(ModelValidationException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}