namespace Play.MerchantPortal.Domain.Entities;

public class StoreEntity : BaseEntity
{
    #region Instance Values

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public long MerchantId { get; set; }

    public MerchantEntity Merchant { get; set; } = default!;

    public ICollection<TerminalEntity>? Terminals { get; set; }

    #endregion
}