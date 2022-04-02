using DeleteMe.Authentications;
using DeleteMe.Authentications.Cda;
using DeleteMe.Authentications.Dda;
using DeleteMe.Certificates;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;

namespace Play.Emv.Security;

public interface ISecureTransactionSession
{
    public void Authenticate( //
        ITlvReaderAndWriter tlvDatabase, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData1Command command);
    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData2Command command);
}

public class SecurityService : IAuthenticateOfflineData, IResolveAuthenticationType
{
    #region Instance Values

    private readonly StaticDataAuthenticator _StaticDataAuthenticator;
    private readonly DynamicDataAuthenticator _DynamicDataAuthenticator;
    private readonly CombinedDataAuthenticator _CombinedDataAuthenticator;

    #endregion

    #region Constructor

    internal SecurityService(
        StaticDataAuthenticator staticDataAuthenticator, DynamicDataAuthenticator dynamicDataAuthenticator,
        CombinedDataAuthenticator combinedDataAuthenticator)
    {
        _StaticDataAuthenticator = staticDataAuthenticator;
        _DynamicDataAuthenticator = dynamicDataAuthenticator;
        _CombinedDataAuthenticator = combinedDataAuthenticator;
    }

    #endregion

    #region Instance Members

    // TODO: Going to have to do a little refactoring with the PCD Transceiver. Moving shit to Play.Emv.Card
    public static SecurityService Create()
    {
        SignatureService? signatureService = new();

        return new SecurityService(new StaticDataAuthenticator(signatureService), new DynamicDataAuthenticator(signatureService),
                                   new CombinedDataAuthenticator(new HashAlgorithmProvider(), signatureService, EmvCodec.GetBerCodec()));
    }

    /// <remarks>
    ///     Book 3 Section 10.3
    /// </remarks>
    public AuthenticationTypes GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities, ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (applicationInterchangeProfile.IsStaticDataAuthenticationSupported()
            && terminalCapabilities.IsStaticDataAuthenticationSupported())
            return AuthenticationTypes.CombinedDataAuthentication;

        if (applicationInterchangeProfile.IsDynamicDataAuthenticationSupported()
            && terminalCapabilities.IsDynamicDataAuthenticationSupported())
            return AuthenticationTypes.DynamicDataAuthentication;

        if (applicationInterchangeProfile.IsCombinedDataAuthenticationSupported()
            && terminalCapabilities.IsCombinedDataAuthenticationSupported())
            return AuthenticationTypes.CombinedDataAuthentication;

        return AuthenticationTypes.None;
    }

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command) =>
        _DynamicDataAuthenticator.Authenticate(command);

    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData1Command command) =>
        _CombinedDataAuthenticator.Authenticate(command);

    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData2Command command) =>
        _CombinedDataAuthenticator.Authenticate(command);

    public AuthenticateStaticDataResponse Authenticate(AuthenticateStaticDataCommand command) =>
        _StaticDataAuthenticator.Authenticate(command);

    #endregion
}