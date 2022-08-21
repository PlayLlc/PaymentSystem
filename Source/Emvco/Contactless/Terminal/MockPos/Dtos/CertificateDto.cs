namespace MockPos.Configuration;

public class CertificateDto
{
    #region Instance Values

    public string? RegisteredApplicationProviderIndicator { get; set; }
    public string? PublicKeyIndex { get; set; }
    public int HashAlgorithmIndicator { get; set; }
    public int PublicKeyAlgorithmIndicator { get; set; }
    public string? ActivationDate { get; set; }
    public string? ExpirationDate { get; set; }
    public int Exponent { get; set; }
    public string? Modulus { get; set; }
    public string? Checksum { get; set; }

    #endregion
}