namespace Play.MerchantPortal.Domain.Entities.PointOfSale;

public class DisplayConfiguration
{
    public string MessageHoldTime { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageSet> DisplayMessages { get; set; } = Enumerable.Empty<DisplayMessageSet>();
}

public class DisplayMessageSet
{
    public string LanguageCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public IEnumerable<DisplayMessage> Messages { get; set; } = Enumerable.Empty<DisplayMessage>();
}

public class DisplayMessage
{
    public string MessageIdentifier { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
