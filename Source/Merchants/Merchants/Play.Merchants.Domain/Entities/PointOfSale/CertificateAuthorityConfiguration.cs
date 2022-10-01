namespace Play.Merchants.Domain.Entities;

public class CertificateAuthorityConfiguration
{
    #region Instance Values

    public IEnumerable<CertificateConfiguration> Certificates { get; set; } = Enumerable.Empty<CertificateConfiguration>();

    #endregion
}