using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

public interface IResolveXAndYAmountForCvmSelection
{
    #region Instance Members

    public Money GetXAmount(NumericCurrencyCode currencyCode);
    public Money GetYAmount(NumericCurrencyCode currencyCode);

    #endregion
}