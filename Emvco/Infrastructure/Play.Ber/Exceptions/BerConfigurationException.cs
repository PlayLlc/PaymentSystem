using System;
using System.Runtime.CompilerServices;

namespace Play.Ber.Exceptions;

public class BerConfigurationException : BerException
{
    #region Constructor

    public BerConfigurationException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerFormatException), fileName, memberName, lineNumber)} {message}")
    { }

    public BerConfigurationException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerFormatException), fileName, memberName, lineNumber)}", innerException)
    { }

    public BerConfigurationException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(BerFormatException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}