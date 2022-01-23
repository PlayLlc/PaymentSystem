using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IGetProcessingOptions : ITransceiveData<GetProcessingOptionsCommand, GetProcessingOptionsResponse>
{ }