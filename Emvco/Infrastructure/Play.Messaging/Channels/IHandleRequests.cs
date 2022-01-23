namespace Play.Messaging;

public interface IHandleRequests
{
    public void Request(RequestMessage message);
}