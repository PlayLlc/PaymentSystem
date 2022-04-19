using System;
using System.Runtime.CompilerServices;

using Play.Codecs.Exceptions;
using Play.Emv.Ber;

namespace Play.Emv.Exceptions;

/// <summary>
///     When an error occurs because of data from received from Integrated Data Storage
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.IdsDataError" /> /></remarks>
public class IntegratedDataStorageDataException : CodecParsingException
{
    #region Constructor

    public IntegratedDataStorageDataException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageDataException), fileName, memberName, lineNumber)} {message}")
    { }

    public IntegratedDataStorageDataException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(IntegratedDataStorageDataException), fileName, memberName, lineNumber)}", innerException)
    { }

    public IntegratedDataStorageDataException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageDataException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}