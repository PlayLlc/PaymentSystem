using Play.Messaging.Tests.Data.Channels;

namespace Play.Messaging.Tests.Data.Messages;

public record TestChannel2ResponseMessage : ResponseMessage
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(TestChannel2ResponseMessage));
    public static readonly ChannelTypeId ChannelTypeId = TestChannel2.ChannelTypeId;

    #endregion

    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public TestChannel2ResponseMessage(CorrelationId correlationId, int value) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public int GetValue() => _Value;

    #endregion
}