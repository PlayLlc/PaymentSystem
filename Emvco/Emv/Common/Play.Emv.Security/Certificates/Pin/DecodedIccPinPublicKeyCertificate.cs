using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates.Pin;

public class DecodedIccPinPublicKeyCertificate : PublicKeyCertificate
{
    #region Static Metadata

    private static readonly CertificateFormat _CertificateFormat = CertificateFormat.Issuer;

    #endregion

    #region Instance Values

    private readonly IssuerIdentificationNumber _IssuerIdentificationNumber;

    #endregion

    #region Constructor

    public DecodedIccPinPublicKeyCertificate(
        IssuerIdentificationNumber issuerIdentificationNumber,
        CertificateSerialNumber certificateSerialNumber,
        HashAlgorithmIndicator hashAlgorithmIndicator,
        PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator,
        DateRange validityPeriod,
        PublicKeyInfo publicKeyInfo) : base(certificateSerialNumber, hashAlgorithmIndicator, publicKeyAlgorithmIndicator, validityPeriod,
        publicKeyInfo)
    {
        _IssuerIdentificationNumber = issuerIdentificationNumber;
    }

    #endregion

    #region Instance Members

    public CertificateFormat GetCertificateFormat() => _CertificateFormat;
    public IssuerIdentificationNumber GetIssuerIdentificationNumber() => _IssuerIdentificationNumber;

    #endregion
}