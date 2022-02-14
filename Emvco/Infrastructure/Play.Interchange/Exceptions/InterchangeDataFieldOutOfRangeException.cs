using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Exceptions;
using Play.Interchange.Messages.DataFields.ValueObjects;

namespace Play.Interchange.Exceptions;

internal class InterchangeDataFieldOutOfRangeException : PlayException
{
    #region Constructor

    public InterchangeDataFieldOutOfRangeException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)} {message}")
    { }

    public InterchangeDataFieldOutOfRangeException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InterchangeDataFieldOutOfRangeException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(InterchangeDataFieldOutOfRangeException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}