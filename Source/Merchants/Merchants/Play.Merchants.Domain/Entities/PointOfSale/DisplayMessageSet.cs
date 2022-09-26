namespace Play.Merchants.Domain.Entities.PointOfSale;

public class DisplayMessageSet
{
    #region Instance Values

    public string LanguageCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public IEnumerable<DisplayMessage> Messages { get; set; } = Enumerable.Empty<DisplayMessage>();

    #endregion
}