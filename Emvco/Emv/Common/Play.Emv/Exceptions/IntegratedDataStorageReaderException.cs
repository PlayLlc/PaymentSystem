using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs.Exceptions;

namespace Play.Emv.Exceptions;

public class IntegratedDataStorageReaderException : CodecParsingException
{
    #region Constructor

    public IntegratedDataStorageReaderException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageReaderException), fileName, memberName, lineNumber)} {message}")
    { }

    public IntegratedDataStorageReaderException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageReaderException), fileName, memberName, lineNumber)}", innerException)
    { }

    public IntegratedDataStorageReaderException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(IntegratedDataStorageReaderException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}