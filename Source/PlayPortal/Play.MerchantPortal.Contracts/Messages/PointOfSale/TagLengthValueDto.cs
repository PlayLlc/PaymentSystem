namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record TagLengthValueDto
{
    public string Name { get; set; } = string.Empty;

    public string Tag { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}
