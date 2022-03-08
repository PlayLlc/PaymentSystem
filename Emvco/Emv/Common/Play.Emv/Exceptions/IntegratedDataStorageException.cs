using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;

namespace Play.Emv.Exceptions;

public class IntegratedDataStorageException : CodecParsingException
{
    #region Constructor

    public IntegratedDataStorageException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageException), fileName, memberName, lineNumber)} {message}")
    { }

    public IntegratedDataStorageException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageException), fileName, memberName, lineNumber)}", innerException)
    { }

    public IntegratedDataStorageException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}