namespace MerchantPortal.Core.Entities;

public class StoreEntity : BaseEntity
{
    public string Name { get; set; }

    public string Address { get; set; }

    public MerchantEntity Merchant { get; set; }

    public ICollection<TerminalEntity> Terminals { get; set; }
}
