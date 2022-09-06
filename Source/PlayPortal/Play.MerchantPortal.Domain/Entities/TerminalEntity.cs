namespace Play.MerchantPortal.Domain.Entities;

public class TerminalEntity : BaseEntity
{
    #region Instance Values

    public long StoreId { get; set; }
    public StoreEntity Store { get; set; } = default!;

    #endregion
}