namespace Play.Merchants.Contracts.Messages.PointOfSale;

public record CertificateConfigurationDto
{
    #region Instance Values

    public string RegisteredApplicationProviderIndicator { get; set; } = string.Empty;
    public string PublicKeyIndex { get; set; } = string.Empty;
    public byte HashAlgorithmIndicator { get; set; }
    public byte PublicKeyAlgorithmIndicator { get; set; }
    public string ActivationDate { get; set; } = string.Empty;
    public string ExpirationDate { get; set; } = string.Empty;
    public uint Exponent { get; set; }
    public string Modulus { get; set; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }
    public string CertificateSerialNumber { get; set; } = string.Empty;

    #endregion
}