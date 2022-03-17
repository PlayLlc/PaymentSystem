using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     When an error occurs in the hardware layer of the Reader's Proximity Coupling Device due to a timeout
/// </summary>
/// <remarks>This exception is logically similar to the Level 1 'Timeout Error''</remarks>
public class PcdTimeoutException : PlayException
{
    #region Constructor

    public PcdTimeoutException(
        string parameterName,
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTimeoutException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public PcdTimeoutException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTimeoutException), fileName, memberName, lineNumber)} {message}")
    { }

    public PcdTimeoutException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTimeoutException), fileName, memberName, lineNumber)}", innerException)
    { }

    public PcdTimeoutException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTimeoutException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}