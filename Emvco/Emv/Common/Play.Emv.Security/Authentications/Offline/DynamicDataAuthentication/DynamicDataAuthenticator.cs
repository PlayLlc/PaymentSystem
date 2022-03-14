using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Security.Cryptograms;
using Play.Emv.Security.Messages.DDA;
using Play.Encryption.Certificates;
using Play.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Offline.DynamicDataAuthentication;

internal class DynamicDataAuthenticator : IAuthenticateDynamicData
{
    #region Instance Values

    private readonly SignatureService _SignatureService;

    #endregion

    #region Constructor

    public DynamicDataAuthenticator(SignatureService signatureService)
    {
        _SignatureService = signatureService;
    }

    #endregion

    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command)
    {
        if (!IsEncipheredSignatureLengthValid(command.GetSignedDynamicApplicationData(), command.GetIssuerPublicKeyCertificate()))
            return CreateDynamicAuthenticationFailed();

        DecodedSignedDynamicApplicationData decodedSignature =
            RecoverSignedDynamicApplicationData(command.GetIssuerPublicKeyCertificate(), command.GetSignedDynamicApplicationData());

        if (!IsSignedDataFormatValid(decodedSignature.GetSignedDataFormat()))
            return CreateDynamicAuthenticationFailed();

        if (!IsSignedDataValid(decodedSignature, command.GetDataObjectListResult()))
            return CreateDynamicAuthenticationFailed();

        return new AuthenticateDynamicDataResponse(TerminalVerificationResult.Create(), ErrorIndication.Default);
    }

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step
    /// </remarks>
    private static AuthenticateDynamicDataResponse CreateDynamicAuthenticationFailed()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetDynamicDataAuthenticationFailed();

        return new AuthenticateDynamicDataResponse(terminalVerificationResult, ErrorIndication.Default);
    }

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 1
    /// </remarks>
    private bool IsEncipheredSignatureLengthValid(
        SignedDynamicApplicationData signedDynamicApplicationData,
        PublicKeyCertificate issuerPublicKeyCertificate)
    {
        if (signedDynamicApplicationData.GetByteCount() != issuerPublicKeyCertificate.GetPublicKeyModulus().GetByteCount())
            return false;

        return true;
    }

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 4
    /// </remarks>
    private bool IsSignedDataFormatValid(SignedDataFormat signedDataFormat) => signedDataFormat == SignedDataFormat._3;

    private bool IsSignedDataValid(
        DecodedSignedDynamicApplicationData decodedSignature,
        DataObjectListResult dynamicDataObjectListResult) =>
        _SignatureService.IsSignatureValid(decodedSignature.GetHashAlgorithmIndicator(),
                                           ReconstructDynamicDataToBeSigned(decodedSignature, dynamicDataObjectListResult),
                                           decodedSignature);

    private byte[] ReconstructDynamicDataToBeSigned(
        DecodedSignedDynamicApplicationData signedData,
        DataObjectListResult dynamicDataObjectListResult) =>
        signedData.GetMessage1().AsSpan().ConcatArrays(dynamicDataObjectListResult.AsByteArray());

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 2
    /// </remarks>
    private DecodedSignedDynamicApplicationData RecoverSignedDynamicApplicationData(
        PublicKeyCertificate issuerPublicKeyCertificate,
        SignedDynamicApplicationData encipheredData) =>
        new(_SignatureService.Decrypt(encipheredData.AsByteArray(), issuerPublicKeyCertificate));

    #endregion
}