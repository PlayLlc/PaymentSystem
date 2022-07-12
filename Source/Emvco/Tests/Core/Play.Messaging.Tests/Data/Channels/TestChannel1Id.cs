namespace Play.Messaging.Tests.Data.Channels;

public readonly record struct TestChannel1Id
{
    #region Static Metadata

    public static readonly ChannelTypeId Id;

    #endregion

    #region Instance Values

    private readonly ChannelTypeId _Value;

    #endregion

    #region Constructor

    static TestChannel1Id()
    {
        Id = new ChannelTypeId(nameof(TestChannel1Id));
    }

    private TestChannel1Id(ChannelTypeId value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ChannelTypeId(TestChannel1Id value) => value._Value;

    #endregion
}