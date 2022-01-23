using System;
using System.Runtime.CompilerServices;

namespace Play.Ber.Exceptions;

public class BerFormatException : BerException
{
    #region Constructor

    public BerFormatException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(BerFormatException), fileName, memberName, lineNumber)} {message}")
    { }

    public BerFormatException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(BerFormatException), fileName, memberName, lineNumber)}", innerException)
    { }

    public BerFormatException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(BerFormatException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}