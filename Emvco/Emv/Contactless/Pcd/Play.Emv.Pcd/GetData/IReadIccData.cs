using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.GetData;

public interface IReadIccData : ITransceiveData<GetDataRequest, GetDataResponse>
{ }