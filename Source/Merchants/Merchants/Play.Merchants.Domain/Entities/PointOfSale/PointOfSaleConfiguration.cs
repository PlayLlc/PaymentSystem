using MongoDB.Bson.Serialization.Attributes;

namespace Play.Merchants.Domain.Entities.PointOfSale;

public class PointOfSaleConfiguration
{
    #region Instance Values

    [BsonId]
    public Guid Id { get; set; }

    public long TerminalId { get; set; }

    public long StoreId { get; set; }

    public long MerchantId { get; set; }

    public long CompanyId { get; set; }

    public TerminalConfiguration TerminalConfiguration { get; set; } = default!;

    public IEnumerable<CombinationConfiguration> Combinations { get; set; } = Enumerable.Empty<CombinationConfiguration>();

    public KernelConfiguration KernelConfiguration { get; set; } = default!;

    public DisplayConfiguration DisplayConfiguration { get; set; } = default!;

    public ProximityCouplingDeviceConfiguration ProximityCouplingDeviceConfiguration { get; set; } = default!;

    public CertificateAuthorityConfiguration CertificateAuthorityConfiguration { get; set; } = default!;

    #endregion
}