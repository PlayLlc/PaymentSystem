using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface ISelectApplicationDefinitionFileInformation : ITransceiveData<SelectApplicationDefinitionFileInfoRequest,
    SelectApplicationDefinitionFileInfoResponse>
{ }