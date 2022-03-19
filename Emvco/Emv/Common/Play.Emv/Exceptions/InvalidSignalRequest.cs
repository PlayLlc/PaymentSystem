using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;
using Play.Emv.Ber.Enums;
using Play.Emv.Icc;

namespace Play.Emv.Exceptions;

/// <summary>
///     When an internal error occurs when sending a signal to a terminal or reader process. These error occur because
///     something is incorrectly configured or coded in our code base
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.TerminalDataError" /> /></remarks>
public class InvalidSignalRequest : PlayInternalException
{
    #region Constructor

    public InvalidSignalRequest(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(InvalidSignalRequest), fileName, memberName, lineNumber)} {message}")
    { }

    public InvalidSignalRequest(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(InvalidSignalRequest), fileName, memberName, lineNumber)}", innerException)
    { }

    public InvalidSignalRequest(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(InvalidSignalRequest), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}