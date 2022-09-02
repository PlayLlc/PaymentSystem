namespace MerchantPortal.WebApi.Models;

public class StoreRequest
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public long MerchantId { get; set; }
}
