using System;

using Play.Ber.Codecs;
using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Certificates.Chip;
using Play.Emv.Security.Cryptograms;
using Play.Emv.Security.Messages.CDA;
using Play.Encryption.Encryption.Hashing;
using Play.Encryption.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Offline.CombinedDataAuthentication;

internal class CombinedDataAuthenticator : IAuthenticateCombinedData
{
    #region Instance Values

    private readonly BerCodec _BerCodec;
    private readonly HashAlgorithmProvider _HashAlgorithmProvider;
    private readonly SignatureService _SignatureService;

    #endregion

    #region Constructor

    public CombinedDataAuthenticator(HashAlgorithmProvider hashAlgorithmProvider, SignatureService signatureService, BerCodec berCodec)
    {
        _HashAlgorithmProvider = hashAlgorithmProvider;
        _SignatureService = signatureService;
        _BerCodec = berCodec;
    }

    #endregion

    #region Instance Members

    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData1Command command) =>
        Authenticate(command.GetIccPublicKeyCertificate(), command.GetUnpredictableNumber(), command.GetGenerateAcCdaResponseMessage(),
                     command.GetTransactionDataHashCodeInput(_BerCodec));

    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData2Command command) =>
        Authenticate(command.GetIccPublicKeyCertificate(), command.GetUnpredictableNumber(), command.GetGenerateAcCdaResponseMessage(),
                     command.GetTransactionDataHashCodeInput(_BerCodec));

    private AuthenticateCombinedDataResponse Authenticate(
        DecodedIccPublicKeyCertificate iccPublicKeyCertificate,
        UnpredictableNumber unpredictableNumber,
        GenerateAcCdaResponseMessage generateAcCdaResponseMessage,
        ReadOnlySpan<byte> transactionDataHashCodeInput)
    {
        if (!IsEncipheredSignatureLengthValid(generateAcCdaResponseMessage.GetSignedDynamicApplicationData(), iccPublicKeyCertificate))
            return CreateCombinedAuthenticationFailedResponse();

        DecodedSignedDynamicApplicationDataCda decodedSignature = RecoverSignedDynamicApplicationData(iccPublicKeyCertificate,
         generateAcCdaResponseMessage.GetSignedDynamicApplicationData());

        if (!IsSignedDataValid(decodedSignature, unpredictableNumber))
            return CreateCombinedAuthenticationFailedResponse();

        if (!IsSignedDataFormatValid(decodedSignature.GetSignedDataFormat()))
            return CreateCombinedAuthenticationFailedResponse();

        if (!IsCryptogramInformationDataValid(decodedSignature, generateAcCdaResponseMessage))
            return CreateCombinedAuthenticationFailedResponse();

        if (!IsTransactionDataHashCodeValid(decodedSignature.GetHashAlgorithmIndicator(), transactionDataHashCodeInput,
                                            decodedSignature.GetIccDynamicData().GetTransactionDataHashCode()))
            return CreateCombinedAuthenticationFailedResponse();

        return new AuthenticateCombinedDataResponse(TerminalVerificationResult.Create(), ErrorIndication.Default,
                                                    decodedSignature.GetIccDynamicData().GetCryptogram(),
                                                    decodedSignature.GetIccDynamicData().GetIccDynamicNumber());
    }

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step
    /// </remarks>
    private static AuthenticateCombinedDataResponse CreateCombinedAuthenticationFailedResponse()
    {
        TerminalVerificationResult terminalVerificationResult = TerminalVerificationResult.Create();
        terminalVerificationResult.SetCombinationDataAuthenticationFailed();

        return new AuthenticateCombinedDataResponse(terminalVerificationResult, ErrorIndication.Default);
    }

    private bool IsCryptogramInformationDataValid(
        DecodedSignedDynamicApplicationDataCda decodedSignedDynamicApplicationDataCda,
        GenerateAcCdaResponseMessage generateAcCdaResponseMessage) =>
        decodedSignedDynamicApplicationDataCda.GetIccDynamicData().GetCryptogramInformationData()
        == generateAcCdaResponseMessage.GetCryptogramInformationData();

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 1
    /// </remarks>
    private bool IsEncipheredSignatureLengthValid(
        SignedDynamicApplicationData signedDynamicApplicationData,
        DecodedIccPublicKeyCertificate iccPublicKeyCertificate)
    {
        if (signedDynamicApplicationData.GetByteCount() != iccPublicKeyCertificate.GetPublicKeyModulus().GetByteCount())
            return false;

        return true;
    }

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 4
    /// </remarks>
    private bool IsSignedDataFormatValid(SignedDataFormat signedDataFormat) => signedDataFormat == SignedDataFormat._5;

    private bool IsSignedDataValid(DecodedSignedDynamicApplicationDataCda decodedSignature, UnpredictableNumber unpredictableNumber) =>
        _SignatureService.IsSignatureValid(decodedSignature.GetHashAlgorithmIndicator(),
                                           ReconstructDynamicDataToBeSigned(decodedSignature, unpredictableNumber), decodedSignature);

    private bool IsTransactionDataHashCodeValid(
        HashAlgorithmIndicator hashAlgorithmIndicator,
        ReadOnlySpan<byte> transactionDataHashCodeInput,
        Hash transactionDataHashCode) =>
        transactionDataHashCode == _HashAlgorithmProvider.Generate(transactionDataHashCodeInput, hashAlgorithmIndicator);

    private byte[] ReconstructDynamicDataToBeSigned(
        DecodedSignedDynamicApplicationDataCda decodedSignedDynamicApplicationData,
        UnpredictableNumber unpredictableNumber) =>
        decodedSignedDynamicApplicationData.GetSignatureHashPlainText(unpredictableNumber);

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 2
    /// </remarks>
    private DecodedSignedDynamicApplicationDataCda RecoverSignedDynamicApplicationData(
        PublicKeyCertificate issuerPublicKeyCertificate,
        SignedDynamicApplicationData encipheredData) =>
        new DecodedSignedDynamicApplicationDataCda(_SignatureService.Decrypt(encipheredData.AsByteArray(), issuerPublicKeyCertificate));

    #endregion

    // This step is done during initialization of a DecodedSignedDynamicApplicationDataDda

    // Transaction Data Hash Code Input is retrieved by the AuthenticateCombinedData1Command or AuthenticateCombinedData2Command objects
}