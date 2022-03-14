using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IReadTornTransactions
{
    public bool TryGet(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber, out TornRecord? result)
}