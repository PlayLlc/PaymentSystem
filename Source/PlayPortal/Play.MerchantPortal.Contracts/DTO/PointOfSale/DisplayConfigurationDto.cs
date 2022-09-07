namespace Play.MerchantPortal.Contracts.DTO.PointOfSale;

public class DisplayConfigurationDto
{
    public string MessageHoldTime { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageSetDto> DisplayMessages { get; set; } = Enumerable.Empty<DisplayMessageSetDto>();
}

public class DisplayMessageSetDto
{
    public string LanguageCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageDto> Messages { get; set; } = Enumerable.Empty<DisplayMessageDto>();
}

public class DisplayMessageDto
{
    public string MessageIdentifier { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
