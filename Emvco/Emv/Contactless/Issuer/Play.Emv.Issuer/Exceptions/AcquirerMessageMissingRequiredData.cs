using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;
using Play.Emv.Issuer.Exceptions;

namespace Play.Emv.Issuer.Exceptions;

internal class AcquirerMessageMissingRequiredData : AcquirerInterfaceException
{
    #region Constructor

    public AcquirerMessageMissingRequiredData(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(AcquirerMessageMissingRequiredData), fileName, memberName, lineNumber)} {message}")
    { }

    public AcquirerMessageMissingRequiredData(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(AcquirerMessageMissingRequiredData), fileName, memberName, lineNumber)}", innerException)
    { }

    public AcquirerMessageMissingRequiredData(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(AcquirerMessageMissingRequiredData), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}