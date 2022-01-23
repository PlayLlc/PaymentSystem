﻿using ___TEMP.Play.Emv.Security.Cryptograms;
using ___TEMP.Play.Emv.Security.Encryption.Signing;

using Play.Ber.Emv.DataObjects;
using Play.Core.Extensions;
using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Authentications.Dynamic.DynamicDataAuthentication;

internal class DynamicDataAuthenticator : IAuthenticateDynamicData
{
    #region Instance Values

    private readonly ISignatureService _SignatureService;

    #endregion

    #region Constructor

    public DynamicDataAuthenticator(ISignatureService signatureService)
    {
        _SignatureService = signatureService;
    }

    #endregion

    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command)
    {
        if (!IsEncipheredSignatureLengthValid(command.GetSignedDynamicApplicationData(), command.GetIssuerPublicKeyCertificate()))
            return CreateDynamicAuthenticationFailed();

        DecodedSignedDynamicApplicationDataDda decodedSignature =
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
    private bool IsSignedDataFormatValid(SignedDataFormat signedDataFormat)
    {
        return signedDataFormat == SignedDataFormat._3;
    }

    private bool IsSignedDataValid(
        DecodedSignedDynamicApplicationDataDda decodedSignature,
        DataObjectListResult dynamicDataObjectListResult)
    {
        return _SignatureService.IsSignatureValid(decodedSignature.GetHashAlgorithmIndicator(),
                                                  ReconstructDynamicDataToBeSigned(decodedSignature, dynamicDataObjectListResult),
                                                  decodedSignature);
    }

    private byte[] ReconstructDynamicDataToBeSigned(
        DecodedSignedDynamicApplicationDataDda signedDataDda,
        DataObjectListResult dynamicDataObjectListResult)
    {
        return signedDataDda.GetMessage1().AsSpan().ConcatArrays(dynamicDataObjectListResult.AsByteArray());
    }

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 2
    /// </remarks>
    private DecodedSignedDynamicApplicationDataDda RecoverSignedDynamicApplicationData(
        PublicKeyCertificate issuerPublicKeyCertificate,
        SignedDynamicApplicationData encipheredData)
    {
        return new(_SignatureService.Decrypt(encipheredData.AsByteArray(), issuerPublicKeyCertificate));
    }

    #endregion
}