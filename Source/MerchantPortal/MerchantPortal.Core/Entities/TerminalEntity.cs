namespace MerchantPortal.Core.Entities;

public class TerminalEntity : BaseEntity
{
    public CompanyEntity Company { get; set; }

    public MerchantEntity Merchant { get; set; }

    public StoreEntity Store { get; set; }
}
