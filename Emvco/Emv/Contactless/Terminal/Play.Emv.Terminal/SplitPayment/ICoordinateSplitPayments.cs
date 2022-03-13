using PrimaryAccountNumber = Play.Emv.DataElements.TrackData.PrimaryAccountNumber;

namespace Play.Emv.Terminal.SplitPayment;

/// <summary>
///     Coordinates transactions that are split into multiple payments and sends the atomized payments as a whole
/// </summary>
public interface ICoordinateSplitPayments
{
    #region Instance Members

    public void CreateSnapshot(PaymentLogItem paymentLogItem);
    public bool TryGetSplitPaymentLogItem(PrimaryAccountNumber primaryAccountNumber, uint sequenceNumber, out SplitPaymentLogItem result);
    public bool TryGetSplitPaymentLogItem(PrimaryAccountNumber primaryAccountNumber, out SplitPaymentLogItem result);

    #endregion
}