using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Codecs;
using Play.Core.Exceptions;
using Play.Emv.Ber;

namespace Play.Emv.Issuer.Exceptions;

internal class AcquirerInterfaceException : PlayException
{
    #region Constructor

    public AcquirerInterfaceException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(AcquirerInterfaceException), fileName, memberName, lineNumber)} {message}")
    { }

    public AcquirerInterfaceException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(AcquirerInterfaceException), fileName, memberName, lineNumber)}", innerException)
    { }

    public AcquirerInterfaceException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(AcquirerInterfaceException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}