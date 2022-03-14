
using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Common.Services.TornTransactionRecovery;
public interface IReadTornTransactions
{
    public bool TryGet(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber, out TornRecord? result)
}
