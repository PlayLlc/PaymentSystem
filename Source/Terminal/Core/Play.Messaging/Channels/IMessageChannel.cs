namespace Play.Messaging;

public interface IMessageChannel : IHandleRequests, IHandleResponses
{
    #region Instance Members

    public ChannelIdentifier GetChannelIdentifier();
    public ChannelTypeId GetChannelTypeId();

    #endregion
}