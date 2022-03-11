using Play.Emv.DataElements;
using Play.Emv.Security.Authentications.Static;
using Play.Emv.Security.Certificates.Icc;
using Play.Emv.Security.Certificates.Issuer;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Signing;

using PrimaryAccountNumber = Play.Emv.DataElements.PrimaryAccountNumber;

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

    // HACK: This needs to throw a CryptographicAuthenticationMethodFailedException when validation does not pass
    public bool TryCreate(
        CaPublicKeyCertificate publicKeyCertificate,
        IssuerPublicKeyCertificate encipheredCertificate,
        IssuerPublicKeyExponent encipheredPublicKeyExponent,
        IssuerPublicKeyRemainder enciphermentPublicKeyRemainder,
        out DecodedIssuerPublicKeyCertificate? result) =>
        Issuer.TryCreate(_SignatureService, publicKeyCertificate, encipheredCertificate, encipheredPublicKeyExponent,
            enciphermentPublicKeyRemainder, out result);

    /// <summary>
    /// </summary>
    /// <param name="issuerCertificate"></param>
    /// <param name="staticDataToBeAuthenticated"></param>
    /// <param name="encipheredCertificate"></param>
    /// <param name="applicationPan"></param>
    /// <param name="encipheredPublicKeyRemainder"></param>
    /// <returns></returns>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public static DecodedIccPublicKeyCertificate TryCreate(
        DecodedIssuerPublicKeyCertificate issuerCertificate,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated,
        IccPublicKeyCertificate encipheredCertificate,
        ApplicationPan applicationPan,
        IccPublicKeyRemainder? encipheredPublicKeyRemainder = null) =>
        Icc.TryCreate(issuerCertificate, staticDataToBeAuthenticated, encipheredCertificate, applicationPan, encipheredPublicKeyRemainder);

    #endregion
}