namespace Play.MerchantPortal.Domain.Entities.PointOfSale;

public class DisplayMessageSet
{
    public string LanguageCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public IEnumerable<DisplayMessage> Messages { get; set; } = Enumerable.Empty<DisplayMessage>();
}
