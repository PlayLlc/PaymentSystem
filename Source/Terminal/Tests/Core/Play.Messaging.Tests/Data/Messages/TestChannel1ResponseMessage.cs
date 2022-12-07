using Play.Messaging.Tests.Data.Channels;

namespace Play.Messaging.Tests.Data.Messages;

public record TestChannel1ResponseMessage : ResponseMessage
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(TestChannel1ResponseMessage));
    public static readonly ChannelTypeId ChannelTypeId = TestChannel1.ChannelTypeId;

    #endregion

    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public TestChannel1ResponseMessage(CorrelationId correlationId, int value) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public int GetValue() => _Value;

    #endregion
}