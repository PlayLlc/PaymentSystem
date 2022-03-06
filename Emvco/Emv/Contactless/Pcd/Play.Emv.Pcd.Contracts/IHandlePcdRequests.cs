using System.Threading.Tasks;

using Play.Emv.Pcd.GetData;

namespace Play.Emv.Pcd.Contracts;

public interface IHandleBlockingPcdRequests
{
    public Task<GetDataBatchResponse> Transceive(GetDataBatchRequest message);
    public Task<ReadApplicationDataResponse> Transceive(ReadApplicationDataRequest message);
}

public interface IHandlePcdRequests : IHandleBlockingPcdRequests
{
    public void Request(ActivatePcdRequest message);
    public void Request(QueryPcdRequest message);
    public void Request(StopPcdRequest message);
}