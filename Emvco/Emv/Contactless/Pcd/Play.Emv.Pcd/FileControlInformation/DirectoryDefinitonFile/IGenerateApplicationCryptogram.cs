using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Contracts.SignalIn.Queriesdd;
using Play.Emv.Pcd.Contracts.SignalOut.Queries;

namespace Play.Emv.Pcd;

public interface
    IGenerateApplicationCryptogram : ITransceiveData<GenerateApplicationCryptogramCommand, GenerateApplicationCryptogramResponse>
{ }