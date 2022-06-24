using Play.Messaging.Tests.Data.Channels;

namespace Play.Messaging.Tests.Data.Messages;

public record TestResponseMessage : ResponseMessage
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(TestResponseMessage));
    public static readonly ChannelTypeId ChannelTypeId = TestEndpoint1.ChannelTypeId;

    #endregion

    #region Constructor

    public TestResponseMessage(CorrelationId correlationId) : base(correlationId, MessageTypeId, ChannelTypeId)
    { }

    #endregion
}