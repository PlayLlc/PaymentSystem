﻿using Play.Ber.Emv;
using Play.Emv.DataElements;
using Play.Emv.Security.Authentications.DynamicDataAuthentication;
using Play.Emv.Security.Authentications.Static;
using Play.Emv.Security.Encryption;
using Play.Emv.Security.Encryption.Signing;

namespace Play.Emv.Security.Authentications;

public class AuthenticationService
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
    public static AuthenticationType GetAuthenticationMethod(
        TerminalCapabilities terminalCapabilities,
        ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        if (applicationInterchangeProfile.IsStaticDataAuthenticationSupported()
            && terminalCapabilities.IsStaticDataAuthenticationSupported())
            return AuthenticationType.CombinedDataAuthentication;

        if (applicationInterchangeProfile.IsDynamicDataAuthenticationSupported()
            && terminalCapabilities.IsDynamicDataAuthenticationSupported())
            return AuthenticationType.DynamicDataAuthentication;

        if (applicationInterchangeProfile.IsCombinedDataAuthenticationSupported()
            && terminalCapabilities.IsCombinedDataAuthenticationSupported())
            return AuthenticationType.CombinedDataAuthentication;

        return AuthenticationType.None;
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