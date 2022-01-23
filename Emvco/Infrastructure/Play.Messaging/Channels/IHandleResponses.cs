namespace Play.Messaging;

public interface IHandleResponses
{
    public void Handle(ResponseMessage message);
}