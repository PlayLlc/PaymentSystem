using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes.DataStorage;
using Play.Globalization.Currency;

namespace Play.Emv.Kernel.Services._TempLogShit;

/// <summary>
///     Maintains a log of transactions to aid in Split Payment scenarios. The <see cref="ICoordinateSplitPayments" /> will
///     use this object
///     to coordinate the individual payments with the state of the transaction session. The only value stored in this log
///     is a snapshot of
///     the previous successful transaction session
/// </summary>
public class SplitPaymentLogItem : Record
{
    #region Instance Values

    /// <summary>
    ///     A subtotal of the split payments successfully processed for this transaction session
    /// </summary>
    private readonly Money _Subtotal;

    #endregion

    #region Constructor

    protected SplitPaymentLogItem(Record value, Money subtotal) : base(value.GetKey(), value.GetValues())
    {
        _Subtotal = subtotal;
    }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    public SplitPaymentLogItem CreateNewSnapshot(ITlvReaderAndWriter database)
    {
        AmountAuthorizedNumeric amountAuthorizedNumeric = database.Get<AmountAuthorizedNumeric>(AmountAuthorizedNumeric.Tag);

        // BUG: This isn't correct. We need to make sure we're resolving the correct currency code when the application and terminal prefer different currency types and when the terminal supports multiple currencies. Look at the logic again and fix this scenarios
        ApplicationCurrencyCode currency = database.Get<ApplicationCurrencyCode>(ApplicationCurrencyCode.Tag);
        Money amount = amountAuthorizedNumeric.AsMoney(currency);
        Record newRecord = Create(database);

        return new SplitPaymentLogItem(newRecord, _Subtotal.Add(amount));
    }

    public Money GetSubtotal() => _Subtotal;

    #endregion
}