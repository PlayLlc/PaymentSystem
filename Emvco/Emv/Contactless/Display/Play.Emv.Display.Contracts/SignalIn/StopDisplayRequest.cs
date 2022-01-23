﻿using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Display.Contracts.SignalIn;

/// <summary>
///     The STOP Signal clears the display immediately and flushes all pending messages
/// </summary>
public record StopDisplayRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(DisplayMessageRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Display;

    #endregion

    #region Constructor

    public StopDisplayRequest() : base(MessageTypeId, ChannelTypeId)
    { }

    #endregion
}