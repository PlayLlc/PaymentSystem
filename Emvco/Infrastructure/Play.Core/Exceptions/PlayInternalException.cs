using System;
using System.Runtime.CompilerServices;

namespace Play.Core.Exceptions;

/// <summary>
///     When an internal error occurs anywhere in the codebase that is caused by something incorrectly configured or coded
/// </summary>
/// <remarks>This exception is logically similar to the Level 2 'Terminal Data Error'</remarks>
public class PlayInternalException : PlayException
{
    #region Static Metadata

    public const string MissingAnExpectedValue = "An internal exception occurred because an expected value was missing";

    #endregion

    #region Constructor

    public PlayInternalException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public PlayInternalException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    public PlayInternalException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public PlayInternalException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}