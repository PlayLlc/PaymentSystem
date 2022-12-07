using Play.Messaging.Tests.Data.Channels;

namespace Play.Messaging.Tests.Data.Messages;

public record TestChannel2RequestMessage : RequestMessage
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(TestChannel2RequestMessage));
    public static readonly ChannelTypeId ChannelTypeId = TestChannel2Id.Id;

    #endregion

    #region Instance Values

    private readonly int _Value;

    #endregion

    #region Constructor

    public TestChannel2RequestMessage(int value) : base(ChannelTypeId, MessageTypeId)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public int GetValue() => _Value;

    #endregion
}