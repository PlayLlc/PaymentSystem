namespace MerchantPortal.Application.DTO;

public record StoreDto
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public long MerchantId { get; set; }
}
