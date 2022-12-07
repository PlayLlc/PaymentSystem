using System.Runtime.CompilerServices;

namespace Play.Messaging.Exceptions;

/// <summary>
///     When there's a message request or callback to a channel that has been incorrectly configured
/// </summary>
/// <remarks>This exception is logically similar to the Level 2 'Terminal Data Error''</remarks>
public class InvalidMessageRoutingException : MessagingException
{
    #region Constructor

    public InvalidMessageRoutingException(
        string message, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) : base(
        $"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} {message}")
    { }

    public InvalidMessageRoutingException(
        Message message, IMessageChannel messageChannel, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} "
        + "\n\r"
        + $"The message type: [{message.GetType().FullName}] was incorrectly routed to the Message Channel: [{messageChannel.GetType().FullName}. Please check your messaging configurations and restart the application")
    { }

    public InvalidMessageRoutingException(
        Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)}", innerException)
    { }

    public InvalidMessageRoutingException(
        string message, Exception innerException, [CallerFilePath] string fileName = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) : base($"{TraceExceptionMessage(typeof(MessagingException), fileName, memberName, lineNumber)} {message}",
        innerException)
    { }

    #endregion
}