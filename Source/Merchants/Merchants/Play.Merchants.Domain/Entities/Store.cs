using Play.Domain;

namespace Play.Merchants.Domain.Entities;

public class Store : BaseEntity
{
    #region Instance Values

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public long MerchantId { get; set; }

    public Merchant Merchant { get; set; } = default!;

    public ICollection<Terminal>? Terminals { get; set; }

    #endregion
}