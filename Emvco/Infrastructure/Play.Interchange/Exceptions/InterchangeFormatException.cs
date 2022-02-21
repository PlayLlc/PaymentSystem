using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Interchange.Exceptions;

public class InterchangeFormatException : PlayException
{
    #region Constructor

    public InterchangeFormatException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)} {message}")
    { }

    public InterchangeFormatException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InterchangeFormatException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}