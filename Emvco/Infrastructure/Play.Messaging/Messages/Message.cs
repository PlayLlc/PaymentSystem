﻿namespace Play.Messaging;

public abstract record Message
{
    #region Instance Values

    protected readonly MessageIdentifier _MessageIdentifier;

    #endregion

    #region Constructor

    protected Message(MessageTypeId messageTypeId, ChannelTypeId channelTypeId)
    {
        _MessageIdentifier = new MessageIdentifier(messageTypeId, channelTypeId);
    }

    #endregion

    #region Instance Members

    public MessageIdentifier GetMessageIdentifier() => _MessageIdentifier;
    public MessageTypeId GetMessageTypeId() => _MessageIdentifier.GetMessageTypeId();
    public ChannelTypeId GetChannelTypeId() => _MessageIdentifier.GetChannelTypeId();

    #endregion
}