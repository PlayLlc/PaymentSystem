using Play.Emv.Ber.DataElements;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates.Pin;

public class DecodedIccPinPublicKeyCertificate : PublicKeyCertificate
{
    #region Static Metadata

    private static readonly CertificateSources _CertificateSources = CertificateSources.Issuer;

    #endregion

    #region Instance Values

    private readonly IssuerIdentificationNumber _IssuerIdentificationNumber;

    #endregion

    #region Constructor

    public DecodedIccPinPublicKeyCertificate(
        IssuerIdentificationNumber issuerIdentificationNumber, CertificateSerialNumber certificateSerialNumber,
        HashAlgorithmIndicator hashAlgorithmIndicator, PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator, DateRange validityPeriod,
        PublicKeyInfo publicKeyInfo) : base(certificateSerialNumber, hashAlgorithmIndicator, publicKeyAlgorithmIndicator, validityPeriod,
                                            publicKeyInfo)
    {
        _IssuerIdentificationNumber = issuerIdentificationNumber;
    }

    #endregion

    #region Instance Members

    public CertificateSources GetCertificateFormat() => _CertificateSources;
    public IssuerIdentificationNumber GetIssuerIdentificationNumber() => _IssuerIdentificationNumber;

    #endregion
}