using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.Enums;

namespace DeleteMe.Exceptions;

/// <summary>
///     When a form of authentication fails, such as Combined Data Authentication, Dynamic Data Authentication, or Static
///     Data Authentication
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.TerminalDataError" /> /></remarks>
public class CryptographicAuthenticationMethodFailedException : CodecParsingException
{
    #region Constructor

    public CryptographicAuthenticationMethodFailedException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(CryptographicAuthenticationMethodFailedException), fileName, memberName, lineNumber)} {message}")
    { }

    public CryptographicAuthenticationMethodFailedException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(CryptographicAuthenticationMethodFailedException), fileName, memberName, lineNumber)}",
             innerException)
    { }

    public CryptographicAuthenticationMethodFailedException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(CryptographicAuthenticationMethodFailedException), fileName, memberName, lineNumber)} {message}",
             innerException)
    { }

    #endregion
}