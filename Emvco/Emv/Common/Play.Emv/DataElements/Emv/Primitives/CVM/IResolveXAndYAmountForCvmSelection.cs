using Play.Globalization.Currency;

namespace Play.Emv.DataElements;

public interface IResolveXAndYAmountForCvmSelection
{
    public Money GetXAmount(ApplicationCurrencyCode currencyCode);
    public Money GetYAmount(ApplicationCurrencyCode currencyCode);
}