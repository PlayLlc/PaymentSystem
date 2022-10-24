namespace Play.MerchantPortal.Contracts.DTO;

public record StoreDto
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public long MerchantId { get; set; }
}
