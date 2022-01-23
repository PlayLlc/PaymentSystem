using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;

namespace Play.Ber.Emv.Exceptions;

public class EmvEncodingException : BerException
{
    #region Constructor

    public EmvEncodingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(BerException), fileName, memberName, lineNumber)} {message}")
    { }

    public EmvEncodingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(BerException), fileName, memberName, lineNumber)}",
                                                      innerException)
    { }

    public EmvEncodingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(BerException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}