using ___TEMP.Play.Emv.Security.Authentications.Static.Signed;
using ___TEMP.Play.Emv.Security.Certificates.Issuer;
using ___TEMP.Play.Emv.Security.Messaging;

using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Authentications;

public class AuthenticationCommand : SecurityCommand
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

    /// <param name="staticDataToBeAuthenticated"></param>
    /// <param name="signedStaticApplicationData"></param>
    /// <param name="caPublicKeyCertificate"></param>
    /// <param name="issuerPublicKeyCertificate"></param>
    /// <param name="issuerPublicKeyExponent"></param>
    /// <param name="issuerPublicKeyRemainder"></param>
    public AuthenticationCommand(
        StaticDataToBeAuthenticated staticDataToBeAuthenticated,
        SignedStaticApplicationData signedStaticApplicationData,
        CaPublicKeyCertificate caPublicKeyCertificate,
        IssuerPublicKeyCertificate issuerPublicKeyCertificate,
        IssuerPublicKeyExponent issuerPublicKeyExponent,
        IssuerPublicKeyRemainder issuerPublicKeyRemainder)
    {
        _StaticDataToBeAuthenticated = staticDataToBeAuthenticated;
        _SignedStaticApplicationData = signedStaticApplicationData;
        _CaPublicKeyCertificate = caPublicKeyCertificate;
        _IssuerPublicKeyCertificate = issuerPublicKeyCertificate;
        _IssuerPublicKeyExponent = issuerPublicKeyExponent;
        _IssuerPublicKeyRemainder = issuerPublicKeyRemainder;
    }

    #endregion

    #region Instance Members

    public CaPublicKeyCertificate GetCaPublicKeyCertificate()
    {
        return _CaPublicKeyCertificate;
    }

    public IssuerPublicKeyCertificate GetIssuerPublicKeyCertificate()
    {
        return _IssuerPublicKeyCertificate;
    }

    public IssuerPublicKeyExponent GetIssuerPublicKeyExponent()
    {
        return _IssuerPublicKeyExponent;
    }

    public IssuerPublicKeyRemainder GetIssuerPublicKeyRemainder()
    {
        return _IssuerPublicKeyRemainder;
    }

    public SignedStaticApplicationData GetSignedStaticApplicationData()
    {
        return _SignedStaticApplicationData;
    }

    public StaticDataToBeAuthenticated GetStaticDataToBeAuthenticated()
    {
        return _StaticDataToBeAuthenticated;
    }

    #endregion
}