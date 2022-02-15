using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;

namespace Play.Ber.Exceptions;

public class BerException : EncodingException
{
    #region Constructor

    public BerException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerException), fileName, memberName, lineNumber)} {message}")
    { }

    public BerException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(BerException), fileName, memberName, lineNumber)}",
        innerException)
    { }

    public BerException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}