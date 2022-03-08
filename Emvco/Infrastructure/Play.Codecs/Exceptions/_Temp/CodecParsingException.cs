using System.Runtime.CompilerServices;

namespace Play.Codecs.Exceptions._Tempz;

public class CodecParsingException : Exceptions.CodecParsingException
{
    #region Constructor

    public CodecParsingException(
        PlayCodec codec,
        Type encodedType,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CodecParsingException), fileName, memberName, lineNumber)} "
        + $"The {codec.GetType().Name} does not have the capability to {memberName} the type: [{encodedType.Name}]")
    { }

    public CodecParsingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CodecParsingException), fileName, memberName, lineNumber)} {message}")
    { }

    public CodecParsingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CodecParsingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public CodecParsingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CodecParsingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}