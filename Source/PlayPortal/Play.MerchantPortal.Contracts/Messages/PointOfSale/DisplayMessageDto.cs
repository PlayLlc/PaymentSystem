namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record DisplayMessageDto
{
    public string MessageIdentifier { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
