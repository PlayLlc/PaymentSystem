using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     When an error occurred during transmission such as a card being removed too quickly
/// </summary>
/// <remarks>This exception is logically similar to the Level 1 'Transmission Error''</remarks>
public class PcdTransmissionException : PlayException
{
    #region Constructor

    public PcdTransmissionException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTransmissionException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public PcdTransmissionException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTransmissionException), fileName, memberName, lineNumber)} {message}")
    { }

    public PcdTransmissionException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTransmissionException), fileName, memberName, lineNumber)}", innerException)
    { }

    public PcdTransmissionException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(PcdTransmissionException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}