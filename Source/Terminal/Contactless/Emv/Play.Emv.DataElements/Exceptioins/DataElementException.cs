using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.DataElements;

internal class DataElementException : PlayException
{
    #region Constructor

    public DataElementException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementException), fileName, memberName, lineNumber)} {message}")
    { }

    public DataElementException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementException), fileName, memberName, lineNumber)}", innerException)
    { }

    public DataElementException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}