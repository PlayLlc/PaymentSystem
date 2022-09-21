namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record DisplayConfigurationDto
{
    public string MessageHoldTime { get; set; } = string.Empty;

    public IEnumerable<DisplayMessageSetDto> DisplayMessages { get; set; } = Enumerable.Empty<DisplayMessageSetDto>();
}
