using System;

using Play.Codecs;
using Play.Messaging;

namespace Play.Emv.Messaging;

public abstract record ResponseSignal : ResponseMessage
{
    #region Constructor

    protected ResponseSignal(CorrelationId correlationId, MessageTypeId messageTypeId, ChannelTypeId channelTypeId) : base(correlationId,
     messageTypeId, channelTypeId)
    { }

    protected ResponseSignal(RequestMessageEnvelope originalMessage, MessageTypeId messageTypeId, ChannelTypeId channelTypeId) :
        base(GetCorrelationId(originalMessage), messageTypeId, channelTypeId)
    { }

    #endregion

    #region Instance Members

    protected static MessageTypeId GetMessageTypeId(Type type) =>
        new(PlayEncoding.UnsignedInteger.GetUInt64(PlayEncoding.ASCII.GetBytes(type.FullName)));

    public static CorrelationId GetCorrelationId(RequestMessageEnvelope originalMessage) => originalMessage.GetCorrelationId();

    #endregion
}