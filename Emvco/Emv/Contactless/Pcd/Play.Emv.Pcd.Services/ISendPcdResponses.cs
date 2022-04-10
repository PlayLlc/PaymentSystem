using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.Services;

internal interface ISendPcdResponses
{
    #region Instance Members

    public void Send(ActivatePcdResponse message);
    public void Send(QueryPcdResponse message);
    public void Send(StopPcdAcknowledgedResponse message);

    #endregion
}