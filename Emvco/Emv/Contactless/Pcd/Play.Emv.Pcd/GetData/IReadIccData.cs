using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.GetData;

public interface IGetData : ITransceiveData<GetDataRequest, GetDataResponse>
{ }