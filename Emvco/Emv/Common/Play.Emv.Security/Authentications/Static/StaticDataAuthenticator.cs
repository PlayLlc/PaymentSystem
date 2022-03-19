using System;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Security.Authentications.Static.Signed;
using Play.Emv.Security.Certificates;
using Play.Emv.Security.Certificates.Issuer;
using Play.Emv.Security.Messages.Static;
using Play.Encryption.Certificates;
using Play.Encryption.Signing;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Security.Authentications.Static;

internal class StaticDataAuthenticator
{
    #region Instance Values

    private readonly CertificateFactory _CertificateFactory;
    private readonly SignedStaticApplicationDataDecoder _SignedStaticApplicationDataDecoder;

    #endregion

    #region Constructor

    public StaticDataAuthenticator(SignatureService signatureService)
    {
        _SignedStaticApplicationDataDecoder = new SignedStaticApplicationDataDecoder(signatureService);
        _CertificateFactory = new CertificateFactory(signatureService);
    }

    #endregion

    #region Instance Members

    private static AuthenticateStaticDataResponse HandleStaticDataAuthenticationFailedResponse()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetStaticDataAuthenticationFailed();

        ErrorIndication.Builder builder = ErrorIndication.GetBuilder();
        builder.Set(Level2Error.TerminalDataError);
        builder.Set(StatusWords.NotAvailable);

        return new AuthenticateStaticDataResponse(terminalVerificationResult, builder.Complete());
    }

    // another option would be to pass a reference value of the outcome to each private
    // TODO: ============================================================================================================
    // TODO: The following will have to be done at a higher level:
    // TODO: ------------------------------------------------------------------------------------------------------------
    // TODO: • If a RID/Index CA Public Key combination is requested but the terminal does not have it, then SDA fails
    // TODO: • If the records read for offline data authentication are not TLV-coded with tag equal to '70' then offline
    // TODO:   data authentication shall be considered to have been performed and to have failed
    // TODO: • Set ‘Offline data authentication was performed’ bit in the TerminalStatusInformation to 1
    // TODO: ============================================================================================================
    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public AuthenticateStaticDataResponse Authenticate(AuthenticateStaticDataCommand command)
    {
        if (command.GetCaPublicKeyCertificate().IsRevoked())
            return HandleStaticDataAuthenticationFailedResponse();

        if (!IsIssuerPublicKeyCertificateLengthValid(command.GetCaPublicKeyCertificate(), command.GetIssuerPublicKeyCertificate()))
            return HandleStaticDataAuthenticationFailedResponse();

        if (!_CertificateFactory.TryCreate(command.GetCaPublicKeyCertificate(), command.GetIssuerPublicKeyCertificate(),
                                           command.GetIssuerPublicKeyExponent(), command.GetIssuerPublicKeyRemainder(),
                                           out DecodedIssuerPublicKeyCertificate? decodedIssuerCertificate))
            return HandleStaticDataAuthenticationFailedResponse();

        if (!IsStaticDataToBeAuthenticatedValid(decodedIssuerCertificate!, command.GetSignedStaticApplicationData(),
                                                command.GetStaticDataToBeAuthenticated().Encode()))
            return HandleStaticDataAuthenticationFailedResponse();

        return new AuthenticateStaticDataResponse(TerminalVerificationResult.Create(), ErrorIndication.Default);
    }

    #endregion

    /// <summary>
    ///     The <see cref="IssuerPublicKeyCertificate" /> is initially encrypted by the <see cref="CaPublicKeyCertificate" />.
    ///     It must be decoded in
    ///     order to retrieve the <see cref="PublicKeyModulus" /> of the Issuer
    /// </summary>
    /// <remarks>
    ///     Book 2 Section 5.3
    /// </remarks>

    #region Validation

    private bool IsIssuerPublicKeyCertificateLengthValid(
        CaPublicKeyCertificate caPublicKeyCertificate,
        IssuerPublicKeyCertificate issuerPublicKeyCertificate) =>
        caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() != issuerPublicKeyCertificate.GetByteCount();

    private bool IsStaticDataToBeAuthenticatedValid(
        DecodedIssuerPublicKeyCertificate decodedCertificateResult,
        SignedStaticApplicationData signedStaticApplicationData,
        ReadOnlySpan<byte> staticDataToBeAuthenticated) =>
        _SignedStaticApplicationDataDecoder.IsValid(decodedCertificateResult!, signedStaticApplicationData,
                                                    staticDataToBeAuthenticated.ToArray());

    #endregion
}