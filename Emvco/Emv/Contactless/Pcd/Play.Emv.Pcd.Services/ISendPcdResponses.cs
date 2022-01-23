using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.Services;

internal interface ISendPcdResponses
{
    public void Send(ActivatePcdResponse message);
    public void Send(QueryPcdResponse message);
    public void Send(StopPcdAcknowledgedResponse message);
}