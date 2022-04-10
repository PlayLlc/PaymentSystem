namespace Play.Messaging;

public interface IHandleResponses
{
    #region Instance Members

    public void Handle(ResponseMessage message);

    #endregion
}