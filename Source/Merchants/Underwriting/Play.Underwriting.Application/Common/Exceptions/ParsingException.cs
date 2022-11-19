using Play.Core.Exceptions;
using System.Runtime.CompilerServices;

namespace Play.Underwriting.Common.Exceptions;

internal class ParsingException : PlayException
{
    public ParsingException(string message) : base(message)
    {
    }

    public ParsingException(
    string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
    $"{TraceExceptionMessage(typeof(PlayInternalException), fileName, memberName, lineNumber)} {message}")
    { }
}
