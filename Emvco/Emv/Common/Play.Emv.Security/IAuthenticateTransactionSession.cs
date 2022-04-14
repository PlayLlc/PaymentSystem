﻿using Play.Emv.Ber;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security.Exceptions;

namespace Play.Emv.Security;

public interface IAuthenticateTransactionSession
{
    #region Instance Members

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void AuthenticateSda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void AuthenticateDda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void AuthenticateCda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, GenerateApplicationCryptogramResponse rapdu,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    public void AuthenticateCda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, RecoverAcResponse rapdu,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    #endregion
}