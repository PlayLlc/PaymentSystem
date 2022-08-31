namespace MerchantPortal.Application.DTO.PointOfSale;

public class CertificateAuthorityConfigurationDto
{
    public IEnumerable<CertificateConfigurationDto> Certificates { get; set; }
}

public class CertificateConfigurationDto
{
    public string RegisteredApplicationProviderIndicator { get; set; }
    public string PublicKeyIndex { get; set; }
    public byte HashAlgorithmIndicator { get; set; }
    public byte PublicKeyAlgorithmIndicator { get; set; }
    public string ActivationDate { get; set; }
    public string ExpirationDate { get; set; }
    public uint Exponent { get; set; }
    public string Modulus { get; set; }
    public string Checksum { get; set; }
    public bool IsRevoked { get; set; }
    public string CertificateSerialNumber { get; set; }
}
