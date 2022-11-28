using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Underwriting.Application.Common.Exceptions;

internal class ParsingException : PlayException
{
    #region Constructor

    public ParsingException(string message) : base(message)
    { }

    public ParsingException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }

    #endregion
}