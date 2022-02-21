using System;

using Play.Globalization.Currency;
using Play.Globalization.Time;

using PrimaryAccountNumber = Play.Emv.DataElements.Interchange.PrimaryAccountNumber;

namespace Play.Emv.Terminal.SplitPayment;

/// <summary>
///     Maintains a log of transactions to aid in Split Payment scenarios. The <see cref="ICoordinateSplitPayments" /> will
///     use this object
///     to coordinate the individual payments with the state of the transaction session. The only value stored in this log
///     is a snapshot of
///     the previous successful transaction session
/// </summary>
public class SplitPaymentLogItem : PaymentLogItem
{
    #region Instance Values

    /// <summary>
    ///     A subtotal of the split payments successfully processed for this transaction session
    /// </summary>
    private readonly Money _Subtotal;

    #endregion

    #region Constructor

    public SplitPaymentLogItem(
        Money amountAuthorizedNumeric,
        PrimaryAccountNumber primaryAccountNumber,
        uint sequenceNumber,
        ShortDateValue transactionDate) : base(primaryAccountNumber, sequenceNumber, transactionDate)
    {
        _Subtotal = amountAuthorizedNumeric;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     CreateNewSnapshot
    /// </summary>
    /// <param name="amountAuthorizedNumeric"></param>
    /// <param name="primaryAccountNumber"></param>
    /// <param name="sequenceNumber"></param>
    /// <param name="transactionDate"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public SplitPaymentLogItem CreateNewSnapshot(
        Money amountAuthorizedNumeric,
        PrimaryAccountNumber primaryAccountNumber,
        uint sequenceNumber,
        ShortDateValue transactionDate)
    {
        if (!_Subtotal.IsCurrencyEqual(amountAuthorizedNumeric))
        {
            throw new ArgumentOutOfRangeException(nameof(amountAuthorizedNumeric),
                $"The argument {nameof(amountAuthorizedNumeric)} is not in the same currency");
        }

        if (primaryAccountNumber != _PrimaryAccountNumber)
            throw new ArgumentOutOfRangeException(nameof(primaryAccountNumber));

        return new SplitPaymentLogItem(_Subtotal.Add(amountAuthorizedNumeric), primaryAccountNumber, sequenceNumber, transactionDate);
    }

    public Money GetSubtotal() => _Subtotal;

    #endregion
}