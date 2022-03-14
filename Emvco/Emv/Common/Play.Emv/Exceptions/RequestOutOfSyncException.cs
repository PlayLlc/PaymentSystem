using System;
using System.Runtime.CompilerServices;

using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Exceptions;

public class RequestOutOfSyncException : InvalidSignalRequest
{
    #region Constructor

    public RequestOutOfSyncException(
        Message message,
        ChannelType channelType,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(RequestOutOfSyncException), fileName, memberName, lineNumber)} \n\rThe request [{message.GetType().FullName}] is invalid for the current state of the [{ChannelType.GetChannelTypeName(channelType)}] channel")
    { }

    public RequestOutOfSyncException(
        string message,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(RequestOutOfSyncException), fileName, memberName, lineNumber)} {message}")
    { }

    public RequestOutOfSyncException(
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(RequestOutOfSyncException), fileName, memberName, lineNumber)}", innerException)
    { }

    public RequestOutOfSyncException(
        string message,
        Exception innerException,
        [CallerFilePath] string fileName = "",
        [CallerMemberName] string memberName = "",
        [CallerLineNumber] int lineNumber = 0) :
        base($"{TraceExceptionMessage(typeof(RequestOutOfSyncException), fileName, memberName, lineNumber)} {message}", innerException)
    { }

    #endregion
}