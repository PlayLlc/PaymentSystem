namespace Play.Messaging;

public interface ICreateEndpointClient
{
    #region Instance Members

    public IEndpointClient CreateEndpointClient();

    #endregion
}