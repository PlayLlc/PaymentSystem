﻿using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security.Authentications;
using Play.Emv.Security.Authentications.Cda;
using Play.Emv.Security.Authentications.Dda;
using Play.Emv.Security.Authentications.Sda;
using Play.Emv.Security.Certificates.Factories;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;

namespace Play.Emv.Security;

public class AuthenticationService : IAuthenticateTransactionSession
{
    #region Instance Values

    private readonly StaticDataAuthenticator _StaticDataAuthenticator;
    private readonly DynamicDataAuthenticator _DynamicDataAuthenticator;
    private readonly CombinedDataAuthenticator _CombinedDataAuthenticator;

    #endregion

    #region Constructor

    internal AuthenticationService(
        StaticDataAuthenticator staticDataAuthenticator, DynamicDataAuthenticator dynamicDataAuthenticator,
        CombinedDataAuthenticator combinedDataAuthenticator)
    {
        _StaticDataAuthenticator = staticDataAuthenticator;
        _DynamicDataAuthenticator = dynamicDataAuthenticator;
        _CombinedDataAuthenticator = combinedDataAuthenticator;
    }

    #endregion

    #region Instance Members

    public static AuthenticationService Create()
    {
        SignatureService? signatureService = new();
        CertificateFactory certificateFactory = new();

        return new AuthenticationService(new StaticDataAuthenticator(signatureService, certificateFactory),
                                         new DynamicDataAuthenticator(signatureService, certificateFactory),
                                         new CombinedDataAuthenticator(new HashAlgorithmProvider(), signatureService, certificateFactory));
    }

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void AuthenticateSda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated)
    {
        _StaticDataAuthenticator.Authenticate(database, certificateDatabase, staticDataToBeAuthenticated);
    }

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void AuthenticateDda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated)
    {
        _DynamicDataAuthenticator.Authenticate(database, certificateDatabase, staticDataToBeAuthenticated);
    }

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void AuthenticateFirstCda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated)
    {
        if (!sessionSecurityRepository.TryGetGenerateApplicationCryptogramResponse(out GenerateApplicationCryptogramResponse?
                                                                                       generateApplicationCryptogramResponse))
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(AuthenticationService)} failed to {nameof(AuthenticateFirstCda)} because a {nameof(GenerateApplicationCryptogramResponse)} could not be found");
        }

        _CombinedDataAuthenticator.AuthenticateFirstGenAc(generateApplicationCryptogramResponse!, database, certificateDatabase,
                                                          sessionSecurityRepository.GetStaticDataToBeAuthenticated());
    }

    #endregion
}