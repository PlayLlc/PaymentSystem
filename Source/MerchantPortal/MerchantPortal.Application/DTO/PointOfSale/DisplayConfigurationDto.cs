namespace MerchantPortal.Application.DTO.PointOfSale;

public class DisplayConfigurationDto
{
    public string MessageHoldTime { get; set; }

    public IEnumerable<DisplayMessageSetDto> DisplayMessages { get; set; }
}

public class DisplayMessageSetDto
{
    public string LanguageCode { get; set; }

    public string CountryCode { get; set; }

    public IEnumerable<DisplayMessageDto> Messages { get; set; }
}

public class DisplayMessageDto
{
    public string MessageIdentifier { get; set; }
    public string Message { get; set; }
}
