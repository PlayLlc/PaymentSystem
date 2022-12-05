using Play.Encryption.Ciphers.Hashing;
using Play.Globalization.Time;

namespace Play.Encryption.Certificates;

public class PublicKeyCertificate
{
    #region Instance Values

    /// <summary>
    ///     The serial number is a value provided by the Certificate Authority that is unique for all other certificates
    ///     the Certificate Authority has created
    /// </summary>
    protected readonly CertificateSerialNumber _CertificateSerialNumber;

    /// <summary>
    ///     The symmetric algorithm used to create the hash in the signature
    /// </summary>
    protected readonly HashAlgorithmIndicators _HashAlgorithmIndicators;

    /// <summary>
    ///     The Algorithm used to create the signature for this certificate
    /// </summary>
    protected readonly PublicKeyAlgorithmIndicators _PublicKeyAlgorithmIndicators;

    /// <summary>
    ///     Information about the Public Key this certificate is for
    /// </summary>
    protected readonly PublicKeyInfo _PublicKeyInfo;

    /// <summary>
    ///     The date range that this certificate is valid
    /// </summary>
    protected readonly DateRange _ValidityPeriod;

    #endregion

    #region Constructor

    public PublicKeyCertificate(
        CertificateSerialNumber certificateSerialNumber, HashAlgorithmIndicators hashAlgorithmIndicators,
        PublicKeyAlgorithmIndicators publicKeyAlgorithmIndicators, DateRange validityPeriod, PublicKeyInfo publicKeyInfo)
    {
        _CertificateSerialNumber = certificateSerialNumber;
        _HashAlgorithmIndicators = hashAlgorithmIndicators;
        _PublicKeyAlgorithmIndicators = publicKeyAlgorithmIndicators;
        _ValidityPeriod = validityPeriod;
        _PublicKeyInfo = publicKeyInfo;
    }

    #endregion

    #region Instance Members

    public bool IsPublicKeySplit() => _PublicKeyInfo.IsPublicKeySplit();
    public HashAlgorithmIndicators GetHashAlgorithmIndicator() => _HashAlgorithmIndicators;
    public PublicKeyAlgorithmIndicators GetPublicKeyAlgorithmIndicator() => _PublicKeyAlgorithmIndicators;
    public PublicKeyExponents GetPublicKeyExponent() => _PublicKeyInfo.GetPublicKeyExponent();
    public PublicKeyInfo GetPublicKeyInfo() => _PublicKeyInfo;
    public PublicKeyModulus GetPublicKeyModulus() => _PublicKeyInfo.GetPublicKeyModulus();
    public CertificateSerialNumber GetPublicKeySerialNumber() => _CertificateSerialNumber;
    public ShortDate GetExpirationDate() => _ValidityPeriod.GetExpirationDate().AsShortDate();
    public ShortDate GetActivationDate() => _ValidityPeriod.GetActivationDate().AsShortDate();
    public bool IsExpired() => _ValidityPeriod.IsExpired();

    #endregion
}