using System.Runtime.CompilerServices;

namespace Play.Messaging.Exceptions;

public class InvalidMessageRoutingException : MessagingException
{
    #region Constructor

    public InvalidMessageRoutingException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} {message}")
    { }

    public InvalidMessageRoutingException(
        Message message,
        IMessageChannel messageChannel,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} "
        + "\n\r"
        + $"The message type: [{message.GetType().FullName}] was incorrectly routed to the Message Channel: [{messageChannel.GetType().FullName}. Please check your messaging configurations and restart the application")
    { }

    public InvalidMessageRoutingException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InvalidMessageRoutingException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}