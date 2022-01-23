using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface ISendPoiInformation : ITransceiveData<SendPoiInformationCommand, SendPoiInformationResponse>
{ }