using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.Security.Authentications.Static;
using Play.Emv.Security.Authentications.Static.Signed;
using Play.Emv.Security.Certificates;
using Play.Emv.Security.Certificates.Issuer;

namespace Play.Emv.Security.Messages.Static;

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
        _StaticDataToBeAuthenticated = new StaticDataToBeAuthenticated(applicationFileLocatorResult, staticDataAuthenticationTagList,
            applicationInterchangeProfile);
        _SignedStaticApplicationData = signedStaticApplicationData;
        _CaPublicKeyCertificate = caPublicKeyCertificate;
        _IssuerPublicKeyCertificate = issuerPublicKeyCertificate;
        _IssuerPublicKeyExponent = issuerPublicKeyExponent;
        _IssuerPublicKeyRemainder = issuerPublicKeyRemainder;
    }

    #endregion

    #region Instance Members

    internal CaPublicKeyCertificate GetCaPublicKeyCertificate() => _CaPublicKeyCertificate;
    internal IssuerPublicKeyCertificate GetIssuerPublicKeyCertificate() => _IssuerPublicKeyCertificate;
    internal IssuerPublicKeyExponent GetIssuerPublicKeyExponent() => _IssuerPublicKeyExponent;
    internal IssuerPublicKeyRemainder GetIssuerPublicKeyRemainder() => _IssuerPublicKeyRemainder;
    internal SignedStaticApplicationData GetSignedStaticApplicationData() => _SignedStaticApplicationData;
    internal StaticDataToBeAuthenticated GetStaticDataToBeAuthenticated() => _StaticDataToBeAuthenticated;

    #endregion
}