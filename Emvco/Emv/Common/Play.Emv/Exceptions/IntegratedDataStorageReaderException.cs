using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Emv.Icc;

namespace Play.Emv.Exceptions;
/// <summary>
///     When an error occurs during a read involving Integrated Data Storage
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.IdsReaderError" /> /></remarks>
public class IntegratedDataStorageReaderException : CodecParsingException
{
    #region Constructor

    public IntegratedDataStorageReaderException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(IntegratedDataStorageReaderException), fileName, memberName, lineNumber)} {message}")
    { }

    public IntegratedDataStorageReaderException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(IntegratedDataStorageReaderException), fileName, memberName, lineNumber)}", innerException)
    { }

    public IntegratedDataStorageReaderException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(IntegratedDataStorageReaderException), fileName, memberName, lineNumber)} {message}",
             innerException)
    { }

    #endregion
}