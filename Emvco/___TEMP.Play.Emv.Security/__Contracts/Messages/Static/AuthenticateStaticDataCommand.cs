using ___TEMP.Play.Emv.Security.Authentications.Static.Signed;
using ___TEMP.Play.Emv.Security.Certificates.Issuer;

using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Authentications.Static;

public class AuthenticateStaticDataCommand
{
    #region Instance Values

    private readonly CaPublicKeyCertificate _CaPublicKeyCertificate;
    private readonly IssuerPublicKeyCertificate _IssuerPublicKeyCertificate;
    private readonly IssuerPublicKeyExponent _IssuerPublicKeyExponent;
    private readonly IssuerPublicKeyRemainder _IssuerPublicKeyRemainder;
    private readonly SignedStaticApplicationData _SignedStaticApplicationData;
    private readonly StaticDataToBeAuthenticated _StaticDataToBeAuthenticated;

    #endregion

    #region Constructor

    public AuthenticateStaticDataCommand(
        TagLengthValue[] applicationFileLocatorResult,
        SignedStaticApplicationData signedStaticApplicationData,
        CaPublicKeyCertificate caPublicKeyCertificate,
        IssuerPublicKeyCertificate issuerPublicKeyCertificate,
        IssuerPublicKeyExponent issuerPublicKeyExponent,
        IssuerPublicKeyRemainder issuerPublicKeyRemainder,
        ApplicationInterchangeProfile applicationInterchangeProfile,
        StaticDataAuthenticationTagList staticDataAuthenticationTagList)
    {
        _StaticDataToBeAuthenticated =
            new StaticDataToBeAuthenticated(applicationFileLocatorResult, staticDataAuthenticationTagList, applicationInterchangeProfile);
        _SignedStaticApplicationData = signedStaticApplicationData;
        _CaPublicKeyCertificate = caPublicKeyCertificate;
        _IssuerPublicKeyCertificate = issuerPublicKeyCertificate;
        _IssuerPublicKeyExponent = issuerPublicKeyExponent;
        _IssuerPublicKeyRemainder = issuerPublicKeyRemainder;
    }

    #endregion

    #region Instance Members

    internal CaPublicKeyCertificate GetCaPublicKeyCertificate()
    {
        return _CaPublicKeyCertificate;
    }

    internal IssuerPublicKeyCertificate GetIssuerPublicKeyCertificate()
    {
        return _IssuerPublicKeyCertificate;
    }

    internal IssuerPublicKeyExponent GetIssuerPublicKeyExponent()
    {
        return _IssuerPublicKeyExponent;
    }

    internal IssuerPublicKeyRemainder GetIssuerPublicKeyRemainder()
    {
        return _IssuerPublicKeyRemainder;
    }

    internal SignedStaticApplicationData GetSignedStaticApplicationData()
    {
        return _SignedStaticApplicationData;
    }

    internal StaticDataToBeAuthenticated GetStaticDataToBeAuthenticated()
    {
        return _StaticDataToBeAuthenticated;
    }

    #endregion
}