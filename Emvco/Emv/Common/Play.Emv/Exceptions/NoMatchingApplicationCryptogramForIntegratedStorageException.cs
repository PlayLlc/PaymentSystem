using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs.Exceptions;

namespace Play.Emv.Exceptions;

public class NoMatchingApplicationCryptogramForIntegratedStorageException : CodecParsingException
{
    #region Constructor

    public NoMatchingApplicationCryptogramForIntegratedStorageException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NoMatchingApplicationCryptogramForIntegratedStorageException), fileName, memberName, lineNumber)} {message}")
    { }

    public NoMatchingApplicationCryptogramForIntegratedStorageException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NoMatchingApplicationCryptogramForIntegratedStorageException), fileName, memberName, lineNumber)}",
        innerException)
    { }

    public NoMatchingApplicationCryptogramForIntegratedStorageException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(NoMatchingApplicationCryptogramForIntegratedStorageException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}