using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Icc.Exceptions;

/// <summary>
///     Thrown when processing data that does not conform to the ISO 7816 specification
/// </summary>
public class IccProtocolException : PlayException
{
    #region Constructor

    public IccProtocolException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IccProtocolException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public IccProtocolException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IccProtocolException), fileName, memberName, lineNumber)} {message}")
    { }

    public IccProtocolException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IccProtocolException), fileName, memberName, lineNumber)}", innerException)
    { }

    public IccProtocolException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IccProtocolException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}