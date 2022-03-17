using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;
using Play.Emv.Icc;

namespace Play.Emv.Exceptions;

/// <summary>
///     When an internal error occurs in the terminal as a result of something incorrectly configured or coded in our
///     system
/// </summary>
/// <remarks>
///     This exception is logically similar to the <see cref="Level2Error.TerminalDataError" /> Level 2 'Terminal Data
///     Error'
/// </remarks>
public class TerminalException : PlayInternalException
{
    #region Constructor

    public TerminalException(
        string parameterName,
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public TerminalException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)} {message}")
    { }

    public TerminalException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public TerminalException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}

/// <summary>
///     When data from the terminal is an incorrect value or different than expected
/// </summary>
public class TerminalDataException : PlayInternalException
{
    #region Constructor

    public TerminalDataException(
        string parameterName,
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public TerminalDataException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)} {message}")
    { }

    public TerminalDataException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)}", innerException)
    { }

    public TerminalDataException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(TerminalDataException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}