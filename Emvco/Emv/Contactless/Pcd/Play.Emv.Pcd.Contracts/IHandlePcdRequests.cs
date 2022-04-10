namespace Play.Emv.Pcd.Contracts;

public interface IHandlePcdRequests
{
    #region Instance Members

    public void Request(ActivatePcdRequest message);
    public void Request(QueryPcdRequest message);
    public void Request(StopPcdRequest message);

    #endregion
}