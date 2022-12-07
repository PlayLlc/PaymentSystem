namespace Play.Messaging;

public interface IHandleRequests
{
    #region Instance Members

    public void Request(RequestMessage message);

    #endregion
}