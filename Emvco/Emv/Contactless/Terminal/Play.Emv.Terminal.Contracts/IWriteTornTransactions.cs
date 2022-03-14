
using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Common.Services.TornTransactionRecovery;
    public interface IWriteTornTransactions
{
    public TornRecord? AddAndDisplace(TornRecord value);
    public bool TryDequeue(out TornRecord? result);
    public TornRecord[]? Truncate()
}
