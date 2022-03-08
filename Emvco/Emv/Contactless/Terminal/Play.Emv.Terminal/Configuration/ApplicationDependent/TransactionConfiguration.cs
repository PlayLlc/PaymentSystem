using Play.Emv.DataElements.Emv.Primitives.Transaction;

namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public class TransactionConfiguration
{
    #region Instance Values

    private readonly TransactionCurrencyCode _TransactionCurrencyCode;
    private readonly TransactionCurrencyExponent _TransactionCurrencyExponent;
    private readonly TransactionReferenceCurrencyCode _TransactionReferenceCurrencyCode;

    //private readonly TransactionReferenceCurrencyConversion _TransactionReferenceCurrencyConversion;
    private readonly TransactionReferenceCurrencyExponent _TransactionReferenceCurrencyExponent;

    #endregion

    #region Constructor

    public TransactionConfiguration(
        TransactionCurrencyCode transactionCurrencyCode,
        TransactionCurrencyExponent transactionCurrencyExponent,
        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode,
        TransactionReferenceCurrencyExponent transactionReferenceCurrencyExponent)
    {
        _TransactionCurrencyCode = transactionCurrencyCode;
        _TransactionCurrencyExponent = transactionCurrencyExponent;
        _TransactionReferenceCurrencyCode = transactionReferenceCurrencyCode;
        _TransactionReferenceCurrencyExponent = transactionReferenceCurrencyExponent;
    }

    #endregion
}