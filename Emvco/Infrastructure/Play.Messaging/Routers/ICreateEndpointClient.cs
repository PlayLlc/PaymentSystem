namespace Play.Messaging;

public interface ICreateEndpointClient
{
    public IEndpointClient CreateEndpointClient(IMessageChannel messageChannel);
}