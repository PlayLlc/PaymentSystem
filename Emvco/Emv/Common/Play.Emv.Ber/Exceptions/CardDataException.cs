using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.Ber.Exceptions;

/// <summary>
///     When data from the card is an incorrect value or different than expected
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.CardDataError" /> /></remarks>
public class CardDataException : PlayException
{
    #region Constructor

    public CardDataException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public CardDataException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)} {message}")
    { }

    public CardDataException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)}", innerException)
    { }

    public CardDataException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(CardDataException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}