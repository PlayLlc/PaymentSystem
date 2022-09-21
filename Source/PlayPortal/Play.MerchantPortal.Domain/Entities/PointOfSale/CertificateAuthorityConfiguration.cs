namespace Play.MerchantPortal.Domain.Entities.PointOfSale;

public class CertificateAuthorityConfiguration
{
    public IEnumerable<CertificateConfiguration> Certificates { get; set; } = Enumerable.Empty<CertificateConfiguration>();
}
