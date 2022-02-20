using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.Interchange.Exceptions;

internal class InterchangeException : PlayException
{
    #region Constructor

    public InterchangeException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)} {message}")
    { }

    public InterchangeException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InterchangeException(
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