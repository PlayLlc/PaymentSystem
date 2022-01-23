using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Authentications.Static.Signed;
using Play.Emv.Security.Certificates.Issuer;
using Play.Emv.Security.Contracts;

namespace Play.Emv.Security.Authentications.Static;

public class AuthenticateStaticDataCommand : AuthenticationCommand
{
    #region Constructor

    public AuthenticateStaticDataCommand(
        TagLengthValue[] applicationFileLocatorResult,
        SignedStaticApplicationData signedStaticApplicationData,
        CaPublicKeyCertificate caPublicKeyCertificate,
        IssuerPublicKeyCertificate issuerPublicKeyCertificate,
        IssuerPublicKeyExponent issuerPublicKeyExponent,
        IssuerPublicKeyRemainder issuerPublicKeyRemainder,
        ApplicationInterchangeProfile applicationInterchangeProfile,
        StaticDataAuthenticationTagList staticDataAuthenticationTagList) : base(
        new StaticDataToBeAuthenticated(applicationFileLocatorResult, staticDataAuthenticationTagList, applicationInterchangeProfile),
        signedStaticApplicationData, caPublicKeyCertificate, issuerPublicKeyCertificate, issuerPublicKeyExponent, issuerPublicKeyRemainder)
    { }

    #endregion
}