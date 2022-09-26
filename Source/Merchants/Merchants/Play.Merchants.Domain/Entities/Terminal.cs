using Play.Domain;

namespace Play.Merchants.Domain.Entities;

public class Terminal : BaseEntity
{
    #region Instance Values

    public long StoreId { get; set; }
    public Store Store { get; set; } = default!;

    #endregion
}