using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.Codecs.Exceptions;

public class EmvEncodingException : PlayException
{
    #region Constructor

    public EmvEncodingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmvEncodingException), fileName, memberName, lineNumber)} {message}")
    { }

    public EmvEncodingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmvEncodingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public EmvEncodingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(EmvEncodingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}