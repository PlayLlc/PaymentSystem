using System;
using System.Runtime.CompilerServices;

namespace Play.Core.Exceptions;

/// <summary>
///     When an internal error occurs anywhere in the codebase that is caused by something incorrectly configured or coded
/// </summary>
/// <remarks>This exception is logically similar to the Level 2 'Terminal Data Error'</remarks>
public class PlayInternalException : PlayException
{
    #region Constructor

    protected PlayInternalException(string message, Exception innerException) : base(message, innerException)
    { }

    protected PlayInternalException(string message) : base(message)
    { }

    public PlayInternalException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public PlayInternalException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    public PlayInternalException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public PlayInternalException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}