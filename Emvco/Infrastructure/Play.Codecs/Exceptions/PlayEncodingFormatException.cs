using System.Runtime.CompilerServices;

namespace Play.Codecs.Exceptions;

public class PlayEncodingFormatException : PlayEncodingException
{
    #region Constructor

    public PlayEncodingFormatException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayEncodingFormatException), fileName, memberName, lineNumber)} {message}")
    { }

    public PlayEncodingFormatException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayEncodingFormatException), fileName, memberName, lineNumber)}", innerException)
    { }

    public PlayEncodingFormatException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayEncodingFormatException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}