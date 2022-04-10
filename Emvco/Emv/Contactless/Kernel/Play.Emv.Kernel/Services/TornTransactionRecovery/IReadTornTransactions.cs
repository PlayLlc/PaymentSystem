using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IReadTornTransactions
{
    #region Instance Members

    public bool TryGet(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber, out TornRecord? result);

    #endregion
}