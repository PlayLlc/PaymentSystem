using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services;

public interface IWriteTornTransactions
{
    public bool TryAddAndDisplace(ITlvReaderAndWriter value, out TornRecord? displacedRecord);
    public bool TryDequeue(out TornRecord? result);
    public TornRecord[]? Truncate(KernelDatabase database);
}