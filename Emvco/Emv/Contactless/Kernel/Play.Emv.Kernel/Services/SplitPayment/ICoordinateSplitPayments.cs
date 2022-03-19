using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Logs;

namespace Play.Emv.Kernel.Services.SplitPayment;

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