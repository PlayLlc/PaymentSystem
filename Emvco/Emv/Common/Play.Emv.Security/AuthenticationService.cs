using Play.Emv.Ber;
using Play.Emv.DataElements.Emv;
using Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;
using Play.Emv.Security.Authentications.Offline.DynamicDataAuthentication;
using Play.Emv.Security.Authentications.Static;
using Play.Emv.Security.Messages.CDA;
using Play.Emv.Security.Messages.DDA;
using Play.Emv.Security.Messages.Static;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;

namespace Play.Emv.Security;

public class AuthenticationService : IAuthenticateOfflineData, IResolveAuthenticationType
{
    #region Instance Values

    private readonly StaticDataAuthenticator _StaticDataAuthenticator;
    private readonly DynamicDataAuthenticator _DynamicDataAuthenticator;
    private readonly CombinedDataAuthenticator _CombinedDataAuthenticator;

    #endregion

    #region Constructor

    internal AuthenticationService(
        StaticDataAuthenticator staticDataAuthenticator,
        DynamicDataAuthenticator dynamicDataAuthenticator,
        CombinedDataAuthenticator combinedDataAuthenticator)
    {
        _StaticDataAuthenticator = staticDataAuthenticator;
        _DynamicDataAuthenticator = dynamicDataAuthenticator;
        _CombinedDataAuthenticator = combinedDataAuthenticator;
    }

    #endregion

    #region Instance Members

    // TODO: Going to have to do a little refactoring with the PCD Transceiver. Moving shit to Play.Emv.Card
    public static AuthenticationService Create()
    {
        SignatureService? signatureService = new();

        return new AuthenticationService(new StaticDataAuthenticator(signatureService), new DynamicDataAuthenticator(signatureService),
            new CombinedDataAuthenticator(new HashAlgorithmProvider(), signatureService, EmvCodec.GetBerCodec()));
    }

    /// <remarks>
    ///     Book 3 Section 10.3
    /// </remarks>
    public AuthenticationTypes GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile)
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