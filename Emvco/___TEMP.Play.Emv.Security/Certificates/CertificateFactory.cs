using ___TEMP.Play.Emv.Security.Certificates.Chip;
using ___TEMP.Play.Emv.Security.Certificates.Issuer;
using ___TEMP.Play.Emv.Security.Encryption.Signing;

using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Certificates;

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
        out DecodedIssuerPublicKeyCertificate? result)
    {
        return Issuer.TryCreate(_SignatureService, publicKeyCertificate, encipheredCertificate, encipheredPublicKeyExponent,
                                enciphermentPublicKeyRemainder, out result);
    }

    public bool TryCreate(
        StaticDataToBeAuthenticated staticDataToBeAuthenticated,
        PrimaryAccountNumber primaryAccountNumber,
        DecodedIssuerPublicKeyCertificate publicKeyCertificate,
        IccPublicKeyCertificate encipheredCertificate,
        IccPublicKeyExponent encipheredPublicKeyExponent,
        IccPublicKeyRemainder enciphermentPublicKeyRemainder,
        out DecodedIccPublicKeyCertificate? result)
    {
        return Icc.TryCreate(_SignatureService, staticDataToBeAuthenticated, primaryAccountNumber, publicKeyCertificate,
                             encipheredCertificate, encipheredPublicKeyExponent, enciphermentPublicKeyRemainder, out result);
    }

    #endregion
}