using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Exceptions;

/// <summary>
///     When the  Amount, Authorized (Numeric) > Reader Contactless Transaction Limit
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.MaxLimitExceeded" /> /></remarks>
public class MaxLimitExceededException : CodecParsingException
{
    #region Constructor

    public MaxLimitExceededException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MaxLimitExceededException), fileName, memberName, lineNumber)} {message}")
    { }

    public MaxLimitExceededException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MaxLimitExceededException), fileName, memberName, lineNumber)}", innerException)
    { }

    public MaxLimitExceededException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(MaxLimitExceededException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}