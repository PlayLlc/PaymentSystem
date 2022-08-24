namespace MerchantPortal.Core.Entities;

public class TerminalEntity : BaseEntity
{
    public MerchantEntity Merchant { get; set; }

    public StoreEntity Store { get; set; }
}
