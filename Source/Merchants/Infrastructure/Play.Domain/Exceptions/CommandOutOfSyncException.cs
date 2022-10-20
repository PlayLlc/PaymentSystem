using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Domain.Exceptions;

public class CommandOutOfSyncException : PlayException
{
    #region Constructor

    protected CommandOutOfSyncException(string message, Exception innerException) : base(message, innerException)
    { }

    protected CommandOutOfSyncException(string message) : base(message)
    { }

    public CommandOutOfSyncException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public CommandOutOfSyncException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    public CommandOutOfSyncException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public CommandOutOfSyncException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}