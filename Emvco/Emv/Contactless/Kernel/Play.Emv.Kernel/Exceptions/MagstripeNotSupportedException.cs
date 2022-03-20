using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Kernel.Exceptions;

/// <summary>
///     When the kernel and the card do not have a matching operating mode
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.MagstripeNotSupported" /></remarks>
public class MagstripeNotSupportedException : PlayException
{
    #region Constructor

    public MagstripeNotSupportedException(
        string parameterName,
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public MagstripeNotSupportedException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)} {message}")
    { }

    public MagstripeNotSupportedException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)}", innerException)
    { }

    public MagstripeNotSupportedException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MagstripeNotSupportedException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}