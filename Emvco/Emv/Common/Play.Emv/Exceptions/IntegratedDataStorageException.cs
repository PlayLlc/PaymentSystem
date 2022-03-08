using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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