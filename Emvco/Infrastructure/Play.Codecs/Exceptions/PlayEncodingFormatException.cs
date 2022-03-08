using System.Runtime.CompilerServices;

namespace Play.Codecs.Exceptions;

public class CodecParsingFormatException : CodecParsingException
{
    #region Constructor

    public CodecParsingFormatException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CodecParsingFormatException), fileName, memberName, lineNumber)} {message}")
    { }

    public CodecParsingFormatException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CodecParsingFormatException), fileName, memberName, lineNumber)}", innerException)
    { }

    public CodecParsingFormatException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CodecParsingFormatException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}