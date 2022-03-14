using System.Runtime.CompilerServices;

namespace Play.Messaging.Exceptions;

public class UnhandledRequestException : MessagingException
{
    #region Constructor

    public UnhandledRequestException(
        RequestMessage requestMessage,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} \n\rThe request type {requestMessage.GetType().FullName} could not be processed because a handler hasn't been implemented yet.")
    { }

    public UnhandledRequestException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public UnhandledRequestException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}