using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Certificates.Chip;
using Play.Emv.Security.Certificates.Issuer;
using Play.Emv.Security.Contracts;
using Play.Emv.Security.Encryption.Signing;

namespace Play.Emv.Security.Certificates;

internal partial class CertificateFactory
{
    #region Instance Values

    protected readonly ISignatureService _SignatureService;

    #endregion

    #region Constructor

    public CertificateFactory(ISignatureService signatureService)
    {
        _SignatureService = signatureService;
    }

    #endregion

    #region Instance Members

    public bool TryCreate(
        CaPublicKeyCertificate publicKeyCertificate,
        IssuerPublicKeyCertificate encipheredCertificate,
        IssuerPublicKeyExponent encipheredPublicKeyExponent,
        IssuerPublicKeyRemainder enciphermentPublicKeyRemainder,
        out DecodedIssuerPublicKeyCertificate? result) =>
        Issuer.TryCreate(_SignatureService, publicKeyCertificate, encipheredCertificate, encipheredPublicKeyExponent,
            enciphermentPublicKeyRemainder, out result);

    public bool TryCreate(
        StaticDataToBeAuthenticated staticDataToBeAuthenticated,
        PrimaryAccountNumber primaryAccountNumber,
        DecodedIssuerPublicKeyCertificate publicKeyCertificate,
        IccPublicKeyCertificate encipheredCertificate,
        IccPublicKeyExponent encipheredPublicKeyExponent,
        IccPublicKeyRemainder enciphermentPublicKeyRemainder,
        out DecodedIccPublicKeyCertificate? result) =>
        Icc.TryCreate(_SignatureService, staticDataToBeAuthenticated, primaryAccountNumber, publicKeyCertificate, encipheredCertificate,
            encipheredPublicKeyExponent, enciphermentPublicKeyRemainder, out result);

    #endregion
}