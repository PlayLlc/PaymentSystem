namespace Play.MerchantPortal.Contracts.Messages.PointOfSale;

public record CertificateAuthorityConfigurationDto
{
    public IEnumerable<CertificateConfigurationDto> Certificates { get; set; } = Enumerable.Empty<CertificateConfigurationDto>();
}
