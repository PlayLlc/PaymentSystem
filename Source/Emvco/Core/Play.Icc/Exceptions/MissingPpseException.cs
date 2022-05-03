using System;
using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Icc.Exceptions;

public class MissingPpseException : PlayException
{
    #region Constructor

    public MissingPpseException(
        string parameterName, string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MissingPpseException), fileName, memberName, lineNumber)}. Parameter {parameterName} experienced an issue. {message}")
    { }

    public MissingPpseException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MissingPpseException), fileName, memberName, lineNumber)} {message}")
    { }

    public MissingPpseException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MissingPpseException), fileName, memberName, lineNumber)}", innerException)
    { }

    public MissingPpseException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(MissingPpseException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}