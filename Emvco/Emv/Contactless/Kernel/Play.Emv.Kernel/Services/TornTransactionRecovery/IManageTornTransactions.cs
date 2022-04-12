using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IManageTornTransactions
{
    #region Instance Members

    public void Add(TornRecord tornRecord, ITlvReaderAndWriter database);
    public bool TryGet(TornEntry tornEntry, out TornRecord? result);

    #endregion
}