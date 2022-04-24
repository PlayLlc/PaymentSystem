using System.Runtime.CompilerServices;

namespace Play.Emv.Codecs.Exceptions;

public class InternalEmvEncodingException : EmvEncodingException
{
    #region Constructor

    public InternalEmvEncodingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InternalEmvEncodingException), fileName, memberName, lineNumber)} {message}")
    { }

    public InternalEmvEncodingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InternalEmvEncodingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InternalEmvEncodingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InternalEmvEncodingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}