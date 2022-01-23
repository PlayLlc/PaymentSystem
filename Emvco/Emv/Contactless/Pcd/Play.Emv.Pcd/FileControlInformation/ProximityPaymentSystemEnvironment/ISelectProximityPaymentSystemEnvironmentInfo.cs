using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface ISelectProximityPaymentSystemEnvironmentInfo : ITransceiveData<SelectProximityPaymentSystemEnvironmentRequest,
    SelectProximityPaymentSystemEnvironmentResponse>
{ }