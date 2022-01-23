using ___TEMP.Play.Emv.Security.Authentications.Static.Signed;
using ___TEMP.Play.Emv.Security.Certificates.Issuer;

using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Authentications.Static;

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
        StaticDataAuthenticationTagList staticDataAuthenticationTagList) :
        base(new StaticDataToBeAuthenticated(applicationFileLocatorResult, staticDataAuthenticationTagList, applicationInterchangeProfile),
             signedStaticApplicationData, caPublicKeyCertificate, issuerPublicKeyCertificate, issuerPublicKeyExponent,
             issuerPublicKeyRemainder)
    { }

    #endregion
}