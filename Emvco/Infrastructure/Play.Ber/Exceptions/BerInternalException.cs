using System;
using System.Runtime.CompilerServices;

namespace Play.Ber.Exceptions;

public class BerInternalException : BerException
{
    #region Constructor

    public BerInternalException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    public BerInternalException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerInternalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public BerInternalException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerInternalException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}