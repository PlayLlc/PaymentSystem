using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;

namespace Play.Emv.Exceptions;

public class IntegratedDataStorageWriterException : CodecParsingException
{
    #region Constructor

    public IntegratedDataStorageWriterException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageWriterException), fileName, memberName, lineNumber)} {message}")
    { }

    public IntegratedDataStorageWriterException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageWriterException), fileName, memberName, lineNumber)}", innerException)
    { }

    public IntegratedDataStorageWriterException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageWriterException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}