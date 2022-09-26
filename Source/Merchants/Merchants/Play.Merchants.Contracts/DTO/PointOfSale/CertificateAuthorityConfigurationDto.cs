namespace Play.Merchants.Contracts.DTO.PointOfSale;

public record CertificateAuthorityConfigurationDto
{
    #region Instance Values

    public IEnumerable<CertificateConfigurationDto> Certificates { get; set; } = Enumerable.Empty<CertificateConfigurationDto>();

    #endregion
}