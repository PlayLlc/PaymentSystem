namespace Play.Merchants.Contracts.Messages.PointOfSale;

public record DisplayMessageSetDto
{
    #region Instance Values

    public string LanguageCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageDto> Messages { get; set; } = Enumerable.Empty<DisplayMessageDto>();

    #endregion
}