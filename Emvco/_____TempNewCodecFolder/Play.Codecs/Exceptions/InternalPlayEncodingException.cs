using System.Runtime.CompilerServices;

namespace Play.Codecs.Exceptions;

public class InternalPlayEncodingException : PlayEncodingException
{
    #region Constructor

    public InternalPlayEncodingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InternalPlayEncodingException), fileName, memberName, lineNumber)} {message}")
    { }

    public InternalPlayEncodingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InternalPlayEncodingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InternalPlayEncodingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InternalPlayEncodingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}