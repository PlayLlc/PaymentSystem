namespace MerchantPortal.Core.Entities;

public class StoreEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public long MerchantId { get; set; }

    public MerchantEntity Merchant { get; set; } = default!;

    public ICollection<TerminalEntity>? Terminals { get; set; }
}
