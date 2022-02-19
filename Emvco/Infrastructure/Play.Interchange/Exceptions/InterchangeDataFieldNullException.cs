using System.Runtime.CompilerServices;

using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;

namespace Play.Interchange.Exceptions;

public class InterchangeDataFieldNullException : InterchangeFormatException
{
    #region Constructor

    public InterchangeDataFieldNullException(
        InterchangeEncodingId berEncodingId,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldNullException), fileName, memberName, lineNumber)} "
        + $"The Data Element: [{memberName}] could not be initialized because the {nameof(EmvCodec)} with {nameof(BerEncodingId)}: [{berEncodingId}] returned a null value")
    { }

    public InterchangeDataFieldNullException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldNullException), fileName, memberName, lineNumber)} {message}")
    { }

    public InterchangeDataFieldNullException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldNullException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InterchangeDataFieldNullException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldNullException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}