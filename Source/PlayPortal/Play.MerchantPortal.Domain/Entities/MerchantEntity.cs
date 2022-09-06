namespace Play.MerchantPortal.Domain.Entities;

public class MerchantEntity : BaseEntity
{
    #region Instance Values

    public string AcquirerId { get; set; } = string.Empty;

    public string MerchantIdentifier { get; set; } = string.Empty;

    public short MerchantCategoryCode { get; set; }

    public string Name { get; set; } = string.Empty;

    public string StreetAddress { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string ZipCode { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public long CompanyId { get; set; }
    public CompanyEntity Company { get; set; } = default!;

    public ICollection<StoreEntity>? Stores { get; set; }

    #endregion
}