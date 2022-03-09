using System.Threading.Tasks;

namespace Play.Emv.Pcd.Contracts;

public interface IHandlePcdRequests
{
    public void Request(ActivatePcdRequest message);
    public void Request(QueryPcdRequest message);
    public void Request(StopPcdRequest message);
}