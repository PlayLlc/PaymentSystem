namespace MerchantPortal.Application.DTO;

public record StoreHeaderDto
{
    public long Id { get; set; }

    public string Name { get; set; }
}

public record StoreDto : StoreHeaderDto
{
    public string Address { get; set; }

    public long MerchantId { get; set; }
}
