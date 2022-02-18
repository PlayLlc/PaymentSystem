using System.Runtime.CompilerServices;

using Play.Interchange.DataFields;

namespace Play.Emv.Interchange.Exceptions;

internal class InterchangeDataFieldOutOfRangeException : InterchangeFormatException
{
    #region Constructor

    public InterchangeDataFieldOutOfRangeException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)} {message}")
    { }

    public InterchangeDataFieldOutOfRangeException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InterchangeDataFieldOutOfRangeException(
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