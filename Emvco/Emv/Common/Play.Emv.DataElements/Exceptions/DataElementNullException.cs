using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Codecs;
using Play.Emv.Ber;

namespace Play.Emv.DataElements.Exceptions;

public class DataElementNullException : DataElementException
{
    #region Constructor

    public DataElementNullException(
        PlayEncodingId playEncodingId,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementException), fileName, memberName, lineNumber)} "
        + $"The Data Element: [{memberName}] could not be initialized because the {nameof(EmvCodec)} with {nameof(PlayEncodingId)}: [{playEncodingId}] returned a null value")
    { }

    public DataElementNullException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementException), fileName, memberName, lineNumber)} {message}")
    { }

    public DataElementNullException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementException), fileName, memberName, lineNumber)}", innerException)
    { }

    public DataElementNullException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}