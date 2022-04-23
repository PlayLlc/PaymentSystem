using Play.Emv.Ber.DataElements;

namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public class TransactionConfiguration
{
    #region Instance Values

    private readonly TransactionCurrencyCode _TransactionCurrencyCode;
    private readonly TransactionReferenceCurrencyCode _TransactionReferenceCurrencyCode;

    //private readonly TransactionReferenceCurrencyConversion _TransactionReferenceCurrencyConversion;
    private readonly TransactionReferenceCurrencyExponent _TransactionReferenceCurrencyExponent;

    #endregion

    #region Constructor

    public TransactionConfiguration(
        TransactionCurrencyCode transactionCurrencyCode, TransactionReferenceCurrencyCode transactionReferenceCurrencyCode,
        TransactionReferenceCurrencyExponent transactionReferenceCurrencyExponent)
    {
        _TransactionCurrencyCode = transactionCurrencyCode;
        _TransactionReferenceCurrencyCode = transactionReferenceCurrencyCode;
        _TransactionReferenceCurrencyExponent = transactionReferenceCurrencyExponent;
    }

    #endregion
}