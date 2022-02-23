using System.Runtime.CompilerServices;

namespace Play.Emv.Ber.Exceptions;

public class EmvEncodingFormatException : EmvEncodingException
{
    #region Constructor

    public EmvEncodingFormatException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmvEncodingFormatException), fileName, memberName, lineNumber)} {message}")
    { }

    public EmvEncodingFormatException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmvEncodingFormatException), fileName, memberName, lineNumber)}", innerException)
    { }

    public EmvEncodingFormatException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmvEncodingFormatException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}