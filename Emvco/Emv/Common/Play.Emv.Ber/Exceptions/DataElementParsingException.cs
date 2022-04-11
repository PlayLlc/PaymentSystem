using System.Runtime.CompilerServices;

using Play.Ber.Exceptions;
using Play.Codecs;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Ber.Exceptions;

/// <summary>
///     When a Template or Data Object List is missing a required Data Element
/// </summary>
/// <remarks>
///     This error is logically similar to a <see cref="Level2Error.ParsingError" />
/// </remarks>
public class DataElementParsingException : BerParsingException
{
    #region Constructor

    public DataElementParsingException(
        PlayEncodingId playEncodingId, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementParsingException), fileName, memberName, lineNumber)} "
        + $"The Data Element: [{memberName}] could not be initialized because the {nameof(EmvCodec)} with {nameof(PlayEncodingId)}: [{playEncodingId}] returned a null value")
    { }

    public DataElementParsingException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementParsingException), fileName, memberName, lineNumber)} {message}")
    { }

    public DataElementParsingException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementParsingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public DataElementParsingException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(DataElementParsingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}