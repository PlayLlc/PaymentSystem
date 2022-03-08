using System;
using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Emv.Ber;

namespace Play.Emv.Exceptions;

/// <summary>
/// When there's a problem encoding or decoding a Data Element or Template due to a format error
/// </summary>
public class DataObjectParsingException : BerParsingException
{
    #region Constructor

    public DataObjectParsingException(
        PlayEncodingId playEncodingId,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataObjectParsingException), fileName, memberName, lineNumber)} "
        + $"The Data Element: [{memberName}] could not be initialized because the {nameof(EmvCodec)} with {nameof(PlayEncodingId)}: [{playEncodingId}] returned a null value")
    { }

    public DataObjectParsingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataObjectParsingException), fileName, memberName, lineNumber)} {message}")
    { }

    public DataObjectParsingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataObjectParsingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public DataObjectParsingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataObjectParsingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}