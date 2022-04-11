using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Exceptions;

/// <summary>
///     When an error occurs during a write involving Integrated Data Storage
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.IdsWriterError" /> /></remarks>
public class IntegratedDataStorageWriterException : CodecParsingException
{
    #region Constructor

    public IntegratedDataStorageWriterException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageWriterException), fileName, memberName, lineNumber)} {message}")
    { }

    public IntegratedDataStorageWriterException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageWriterException), fileName, memberName, lineNumber)}", innerException)
    { }

    public IntegratedDataStorageWriterException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageWriterException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}