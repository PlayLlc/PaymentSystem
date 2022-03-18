using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services;

public interface IWriteTornTransactions
{
    public TornRecord? AddAndDisplace(TornRecord value);
    public bool TryDequeue(out TornRecord? result);
    public TornRecord[]? Truncate(KernelDatabase database)
}