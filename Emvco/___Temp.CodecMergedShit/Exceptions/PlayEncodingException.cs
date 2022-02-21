using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace ___Temp.CodecMergedShit.Exceptions;

public class PlayEncodingException : PlayException
{
    #region Static Metadata

    public const string ByteWasOutOfRangeOfHexadecimalCharacter = "The Byte value was out of range of a hexadecimal character";
    public const string ByteArrayContainsInvalidValue = "The Byte Array contains a value that is out of range of this encoding type.";
    public const string ByteArrayLengthTooBig = "The length supplied for the Byte Array was too large.";
    public const string ByteArrayWasEmpty = "The byte array supplied contained no elements in the array";

    public const string CharacterArrayContainsInvalidValue =
        "The Character Array contains a value that is out of range of this encoding type.";

    public const string CharacterArrayValuesCouldNotBeRecognized = "The Character Array did not contain any recognizable values.";
    public const string CharArrayWasEmpty = "The char array supplied contained no elements in the array";

    #endregion

    #region Constructor

    public PlayEncodingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayEncodingException), fileName, memberName, lineNumber)} {message}")
    { }

    public PlayEncodingException(
        string message,
        string value,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayEncodingException), fileName, memberName, lineNumber)} {message}. The offending value was: {value}")
    { }

    public PlayEncodingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayEncodingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public PlayEncodingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayEncodingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}