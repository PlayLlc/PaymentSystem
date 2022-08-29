namespace MerchantPortal.Core.Entities;

public class TerminalEntity : BaseEntity
{
    public long StoreId { get; set; }
    public StoreEntity Store { get; set; }
}
