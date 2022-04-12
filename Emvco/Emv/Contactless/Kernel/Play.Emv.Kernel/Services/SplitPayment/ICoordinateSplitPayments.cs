using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Services;

/// <summary>
///     Coordinates transactions that are split into multiple payments and sends the atomized payments as a whole
/// </summary>
public interface ICoordinateSplitPayments
{
    #region Instance Members

    public void CreateSnapshot(SplitPaymentLogItem paymentLogItem);
    public bool TryGetSplitPaymentLogItem(ApplicationPan primaryAccountNumber, uint sequenceNumber, out SplitPaymentLogItem result);
    public bool TryGetSplitPaymentLogItem(ApplicationPan primaryAccountNumber, out SplitPaymentLogItem result);

    #endregion
}