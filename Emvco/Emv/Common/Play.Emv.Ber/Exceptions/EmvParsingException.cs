using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;

namespace Play.Emv.Ber.Exceptions;

public class EmvParsingException : BerParsingException
{
    #region Constructor

    public EmvParsingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(EmvParsingException), fileName, memberName, lineNumber)} {message}")
    { }

    public EmvParsingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(EmvParsingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public EmvParsingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(EmvParsingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}