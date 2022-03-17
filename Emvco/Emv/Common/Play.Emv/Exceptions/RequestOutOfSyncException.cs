using System;
using System.Runtime.CompilerServices;

using Play.Emv.Icc;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Exceptions;

/// <summary>
///     When a channel receives a message but is in an invalid state and cannot process the request
/// </summary>
/// <remarks>This error is logically similar to a <see cref="Level2Error.TerminalDataError" /> /></remarks>
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