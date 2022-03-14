using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;

namespace Play.Emv.Exceptions;

public class InvalidSignalRequest : CodecParsingException
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