using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     When an error occurs in the hardware layer of the Reader's Proximity Coupling Device due to a protocol error
/// </summary>
/// <remarks>This exception is logically similar to the Level 1 'Protocol Error''</remarks>
public class PcdProtocolException : PlayException
{
    #region Constructor

    public PcdProtocolException(
        string parameterName,
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdProtocolException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public PcdProtocolException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdProtocolException), fileName, memberName, lineNumber)} {message}")
    { }

    public PcdProtocolException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdProtocolException), fileName, memberName, lineNumber)}", innerException)
    { }

    public PcdProtocolException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdProtocolException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}