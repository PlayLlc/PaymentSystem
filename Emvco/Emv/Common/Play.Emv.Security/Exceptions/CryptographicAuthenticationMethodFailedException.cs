using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs.Exceptions;

namespace Play.Emv.Security.Exceptions;

/// <summary>
///     When a form of authentication fails, such as Combined Data Authentication, Dynamic Data Authentication, or Static
///     Data Authentication
/// </summary>
public class CryptographicAuthenticationMethodFailedException : CodecParsingException
{
    #region Constructor

    public CryptographicAuthenticationMethodFailedException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CryptographicAuthenticationMethodFailedException), fileName, memberName, lineNumber)} {message}")
    { }

    public CryptographicAuthenticationMethodFailedException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CryptographicAuthenticationMethodFailedException), fileName, memberName, lineNumber)}",
        innerException)
    { }

    public CryptographicAuthenticationMethodFailedException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(CryptographicAuthenticationMethodFailedException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}