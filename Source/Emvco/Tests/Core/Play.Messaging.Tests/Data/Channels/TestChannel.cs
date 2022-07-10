namespace Play.Messaging.Tests.Data.Channels;

public readonly record struct TestChannel
{
    #region Static Metadata

    public static readonly ChannelTypeId Id;

    #endregion

    #region Instance Values

    private readonly ChannelTypeId _Value;

    #endregion

    #region Constructor

    static TestChannel()
    {
        Id = new ChannelTypeId(nameof(TestChannel));
    }

    private TestChannel(ChannelTypeId value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ChannelTypeId(TestChannel value) => value._Value;

    #endregion
}