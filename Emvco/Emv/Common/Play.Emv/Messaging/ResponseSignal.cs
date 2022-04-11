using Play.Messaging;

namespace Play.Emv.Messaging;

public abstract record ResponseSignal : ResponseMessage
{
    #region Constructor

    protected ResponseSignal(CorrelationId correlationId, MessageTypeId messageTypeId, ChannelTypeId channelTypeId) : base(correlationId,
        messageTypeId, channelTypeId)
    { }

    #endregion
}