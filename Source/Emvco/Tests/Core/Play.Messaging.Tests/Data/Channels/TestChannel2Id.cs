namespace Play.Messaging.Tests.Data.Channels;

public readonly record struct TestChannel2Id
{
    #region Static Metadata

    public static readonly ChannelTypeId Id;

    #endregion

    #region Instance Values

    private readonly ChannelTypeId _Value;

    #endregion

    #region Constructor

    static TestChannel2Id()
    {
        Id = new ChannelTypeId(nameof(TestChannel2Id));
    }

    private TestChannel2Id(ChannelTypeId value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static explicit operator ChannelTypeId(TestChannel2Id value) => value._Value;

    #endregion
}