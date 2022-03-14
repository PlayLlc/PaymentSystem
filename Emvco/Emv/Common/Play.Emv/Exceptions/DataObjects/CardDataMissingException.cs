using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Emv.Exceptions;

/// <summary>
///     When a Template or Data Object List is missing a required Data Element
/// </summary>
public class CardDataMissingException : PlayException
{
    #region Constructor

    public CardDataMissingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(CardDataMissingException), fileName, memberName, lineNumber)} {message}")
    { }

    public CardDataMissingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(CardDataMissingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public CardDataMissingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(CardDataMissingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}