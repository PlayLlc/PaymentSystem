using System;

using Play.Codecs;
using Play.Messaging;

namespace Play.Emv.Messaging;

public abstract record RequestSignal : RequestMessage
{
    #region Constructor

    protected RequestSignal(MessageTypeId messageTypeId, ChannelTypeId channelTypeId) : base(channelTypeId, messageTypeId)
    { }

    #endregion

    #region Instance Members

    protected static MessageTypeId CreateMessageTypeId(Type type) =>
        new(PlayCodec.UnsignedIntegerCodec.DecodeToUInt64(PlayCodec.AsciiCodec.Encode(type.FullName)));

    #endregion
}