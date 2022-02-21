using Play.Emv.DataElements.Emv;
using Play.Emv.Security.Authentications.Static;
using Play.Emv.Security.Certificates.Icc;
using Play.Emv.Security.Certificates.Issuer;
using Play.Encryption.Signing;

using PrimaryAccountNumber = Play.Emv.DataElements.Interchange.PrimaryAccountNumber;

namespace Play.Emv.Security.Certificates;

internal partial class CertificateFactory
{
    #region Instance Values

    protected readonly SignatureService _SignatureService;

    #endregion

    #region Constructor

    public CertificateFactory(SignatureService signatureService)
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