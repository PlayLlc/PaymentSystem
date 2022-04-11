using System.Runtime.CompilerServices;

using Play.Core.Exceptions;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Ber.Exceptions;

/// <summary>
///     When an internal error occurs in the terminal as a result of something incorrectly configured or coded in our
///     system
/// </summary>
/// <remarks>
///     This exception is logically similar to the <see cref="Level2Error.TerminalDataError" /> Level 2 'Terminal Data
///     Error'
/// </remarks>
public class TerminalException : PlayInternalException
{
    #region Constructor

    public TerminalException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public TerminalException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)} {message}")
    { }

    public TerminalException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)}", innerException)
    { }

    public TerminalException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(TerminalException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}