using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Security.Certificates;
using Play.Emv.Security.Certificates.Factories;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Sda;

internal class StaticDataAuthenticator
{
    #region Instance Values

    private readonly CertificateFactory _CertificateFactory;
    private readonly SignedStaticApplicationDataDecoder _SignedStaticApplicationDataDecoder;

    #endregion

    #region Constructor

    public StaticDataAuthenticator(SignatureService signatureService, CertificateFactory certificateFactory)
    {
        _SignedStaticApplicationDataDecoder = new SignedStaticApplicationDataDecoder(signatureService);
        _CertificateFactory = certificateFactory;
    }

    #endregion
                        
    #region Instance Members

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void Authenticate(ITlvReaderAndWriter tlvDatabase, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated)
    {
        try
        {
            DecodedIssuerPublicKeyCertificate recoveredIssuerCertificate = _CertificateFactory.RecoverIssuerCertificate(tlvDatabase, certificateDatabase);

            SignedStaticApplicationData signedStaticApplicationData = tlvDatabase.Get<SignedStaticApplicationData>(SignedStaticApplicationData.Tag);

            ValidateStaticDataToBeAuthenticated(recoveredIssuerCertificate!, signedStaticApplicationData, staticDataToBeAuthenticated.Encode());
        }
        catch (TerminalDataException)
        {
            // TODO: Logging
            SetStaticDataAuthenticationFailedResponse(tlvDatabase);
        }
        catch (CryptographicAuthenticationMethodFailedException)
        {
            // TODO: Logging
            SetStaticDataAuthenticationFailedResponse(tlvDatabase);
        }
        catch (Exception)
        {
            // TODO: Logging
            SetStaticDataAuthenticationFailedResponse(tlvDatabase);
        }
    }

    #endregion

    #region Validation

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private static void SetStaticDataAuthenticationFailedResponse(ITlvReaderAndWriter database)
    {
        try
        {
            TerminalVerificationResults terminalVerificationResults = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

            TerminalVerificationResults.Builder tvrBuilder = TerminalVerificationResults.GetBuilder();
            tvrBuilder.Reset(terminalVerificationResults);
            tvrBuilder.Set(TerminalVerificationResultCodes.StaticDataAuthenticationFailed);

            database.Update(tvrBuilder.Complete());
        }
        catch (TerminalDataException exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
    }

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private void ValidateStaticDataToBeAuthenticated(
        DecodedIssuerPublicKeyCertificate decodedCertificateResult, SignedStaticApplicationData signedStaticApplicationData,
        ReadOnlySpan<byte> staticDataToBeAuthenticated)
    {
        if (!_SignedStaticApplicationDataDecoder.IsValid(decodedCertificateResult!, signedStaticApplicationData, staticDataToBeAuthenticated.ToArray()))
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(StaticDataAuthenticator)} failed Static Data Authentication because the constraint {nameof(ValidateStaticDataToBeAuthenticated)} was invalid");
        }
    }

    #endregion
}