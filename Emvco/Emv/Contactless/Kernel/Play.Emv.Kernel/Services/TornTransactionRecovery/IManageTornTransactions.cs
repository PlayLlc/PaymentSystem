using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes.DataStorage;
using Play.Emv.Kernel.DataExchange;

namespace Play.Emv.Kernel.Services;

public interface IManageTornTransactions
{
    #region Instance Members

    public void Add(TornRecord tornRecord, ITlvReaderAndWriter database);
    public bool TryGet(TornEntry tornEntry, out TornRecord? result);
    public void Remove(IWriteToDek dataExchangeKernel, TornEntry tornEntry);

    #endregion
}