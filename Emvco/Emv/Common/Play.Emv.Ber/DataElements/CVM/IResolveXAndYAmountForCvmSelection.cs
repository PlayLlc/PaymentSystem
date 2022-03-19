using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

public interface IResolveXAndYAmountForCvmSelection
{
    public Money GetXAmount(NumericCurrencyCode currencyCode);
    public Money GetYAmount(NumericCurrencyCode currencyCode);
}