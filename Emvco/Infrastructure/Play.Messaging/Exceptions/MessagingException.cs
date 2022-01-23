using System.Runtime.CompilerServices;

using Play.Core.Exceptions;

namespace Play.Messaging.Exceptions;

public class MessagingException : PlayException
{
    #region Constructor

    public MessagingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} {message}")
    { }

    public MessagingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public MessagingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}