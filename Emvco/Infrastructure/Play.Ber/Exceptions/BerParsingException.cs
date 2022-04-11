using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;

namespace Play.Ber.Exceptions;

/// <summary>
///     When there's a problem encoding or decoding a primitive or constructed TLV object due to a format error
/// </summary>
/// <remarks>This exception is logically similar to the Level 2 'Card Parsing error''</remarks>
public class BerParsingException : CodecParsingException
{
    #region Static Metadata

    public static string ArgumentDidNotConformToTheFormatExpected = "The argument did not conform to the format that was expected";

    public static string ValueExperiencedAnOverflowExceptionCastingToNarrowerType =
        "Value was too large to cast into a narrower numeric type";

    #endregion

    #region Constructor

    public BerParsingException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerParsingException), fileName, memberName, lineNumber)} {message}")
    { }

    public BerParsingException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerParsingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public BerParsingException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerParsingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}