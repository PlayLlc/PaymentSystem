using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Contracts.SignalIn.Quereies;
using Play.Emv.Pcd.Contracts.SignalOut.Queddries;

namespace Play.Emv.Pcd
{
    public interface IExchangeRelayResistanceData : ITransceiveData<ExchangeRelayResistanceDataRequest, ExchangeRelayResistanceDataResponse>
    { }
}