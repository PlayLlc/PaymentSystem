using Play.Infrastructure.Domain.Entities;

namespace Play.MerchantPortal.Domain.Entities;

public class Company : BaseEntity
{
    #region Instance Values

    public string Name { get; set; } = string.Empty;

    #endregion
}