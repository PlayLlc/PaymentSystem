using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Exceptions;

/// <remarks>This error is logically similar to a <see cref="Level2Error.IdsNoMatchingAc" /> /></remarks>
public class NoMatchingApplicationCryptogramForIntegratedStorageException : CodecParsingException
{
    #region Constructor

    public NoMatchingApplicationCryptogramForIntegratedStorageException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NoMatchingApplicationCryptogramForIntegratedStorageException), fileName, memberName, lineNumber)} {message}")
    { }

    public NoMatchingApplicationCryptogramForIntegratedStorageException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(NoMatchingApplicationCryptogramForIntegratedStorageException), fileName, memberName, lineNumber)}", innerException)
    { }

    public NoMatchingApplicationCryptogramForIntegratedStorageException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NoMatchingApplicationCryptogramForIntegratedStorageException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}