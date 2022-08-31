namespace MerchantPortal.Application.DTO.PointOfSale;

public class DisplayConfigurationDto
{
    public string MessageHoldTime { get; set; }

    public IEnumerable<DisplayMessageSet> DisplayMessages { get; set; }
}

public class DisplayMessageSet
{
    public string LanguageCode { get; set; }

    public string CountryCode { get; set; }

    public IEnumerable<DisplayMessage> Messages { get; set; }
}

public class DisplayMessage
{
    public string MessageIdentifier { get; set; }
    public string Message { get; set; }
}
