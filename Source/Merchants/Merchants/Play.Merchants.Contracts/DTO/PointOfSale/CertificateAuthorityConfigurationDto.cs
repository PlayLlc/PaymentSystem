namespace Play.Merchants.Contracts.DTO;

public record CertificateAuthorityConfigurationDto
{
    #region Instance Values

    public IEnumerable<CertificateConfigurationDto> Certificates { get; set; } = Enumerable.Empty<CertificateConfigurationDto>();

    #endregion
}