using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;

namespace Play.Emv.Ber.Exceptions;

public class EmvEncodingException : BerParsingException
{
    #region Constructor

    public EmvEncodingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerParsingException), fileName, memberName, lineNumber)} {message}")
    { }

    public EmvEncodingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(BerParsingException), fileName, memberName, lineNumber)}",
        innerException)
    { }

    public EmvEncodingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerParsingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}