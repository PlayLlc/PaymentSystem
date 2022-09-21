namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record DisplayMessageSetDto
{
    public string LanguageCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageDto> Messages { get; set; } = Enumerable.Empty<DisplayMessageDto>();
}
