using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Icc.Exceptions;

/// <summary>
///     Thrown when the RAPDU returned contains a status word that is considered an error or a failure
/// </summary>
public class StatusBytesException : PlayException
{
    #region Constructor

    public StatusBytesException(
        string parameterName,
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public StatusBytesException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    public StatusBytesException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public StatusBytesException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}