using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Domain.Exceptions;

public class NotFoundException : PlayException
{
    #region Constructor

    protected NotFoundException(string message, Exception innerException) : base(message, innerException)
    { }

    protected NotFoundException(string message) : base(message)
    { }

    public NotFoundException(
        Type entityType, string id, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(NotFoundException), fileName, memberName, lineNumber)}. The {entityType.Name} with ID: [{id}")
    { }

    public NotFoundException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NotFoundException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public NotFoundException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NotFoundException), fileName, memberName, lineNumber)} {message}")
    { }

    public NotFoundException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(NotFoundException), fileName, memberName, lineNumber)}", innerException)
    { }

    public NotFoundException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(NotFoundException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}