namespace Play.Acquirer.Contracts;

public interface IHandleAcquirerRequests
{
    public Dictionary<MessageTypeIndicator, IssuerMessageFactory> GetMessageFactories();
    public void Request(IssuerMessageRequest message);
}