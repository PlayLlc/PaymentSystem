using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IGenerateApplicationCryptogram : ITransceiveData<GenerateApplicationCryptogramRequest, GenerateApplicationCryptogramResponse>
{ }