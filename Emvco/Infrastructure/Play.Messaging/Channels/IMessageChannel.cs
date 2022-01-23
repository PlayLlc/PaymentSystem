namespace Play.Messaging;

public interface IMessageChannel : IHandleRequests, IHandleResponses
{
    public ChannelIdentifier GetChannelIdentifier();
    public ChannelTypeId GetChannelTypeId();
}