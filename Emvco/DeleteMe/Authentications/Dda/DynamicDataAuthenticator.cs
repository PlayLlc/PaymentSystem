﻿using DeleteMe.Certificates;
using DeleteMe.Exceptions;

using Play.Ber.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;
using Play.Icc.Messaging.Apdu;

namespace DeleteMe.Authentications.Dda;

internal class DynamicDataAuthenticator : IAuthenticateDynamicData
{
    #region Instance Values

    private readonly SignatureService _SignatureService;
    private readonly CertificateFactory _CertificateFactory;

    #endregion

    #region Instance Members

    public DynamicDataAuthenticator(SignatureService signatureService, CertificateFactory certificateFactory)
    {
        _SignatureService = signatureService;
        _CertificateFactory = certificateFactory;
    }

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void Authenticate(ITlvReaderAndWriter database)
    {
        try
        {
            SignedDynamicApplicationData signedDynamicApplicationData =
                database.Get<SignedDynamicApplicationData>(SignedDynamicApplicationData.Tag);

            DecodedIccPublicKeyCertificate decodedIccCertificate = _CertificateFactory.RecoverIccCertificate();

            // Step 1
            ValidateEncipheredSignatureLength(signedDynamicApplicationData, decodedIccCertificate);

            // Step 2
            DecodedSignedDynamicApplicationData decodedSignature =
                RecoverSignedDynamicApplicationData(decodedIccCertificate,
                                                    database.Get<SignedDynamicApplicationData>(SignedDynamicApplicationData.Tag));

            // Step 3 is accomplished by Step 6 - 7 below

            // Step 4
            ValidateSignedDataFormat(decodedSignature.GetSignedDataFormat());

            DataObjectListResult ddolResult = database
                .Get<DynamicDataAuthenticationDataObjectList>(DynamicDataAuthenticationDataObjectList.Tag).AsDataObjectListResult(database);

            // Step 5 
            ReadOnlySpan<byte> concatenatedSeed = ReconstructDynamicDataToBeSigned(decodedSignature, ddolResult);

            // Step 6 - 7
            ValidateSignedData(decodedSignature, concatenatedSeed, ddolResult);
        }

        catch (BerParsingException)
        {
            // TODO: Logging

            SetDynamicAuthenticationFailed(database);
        }
        catch (Exception)
        {
            SetDynamicAuthenticationFailed(database);
        }
    }

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private static void SetDynamicAuthenticationFailed(ITlvReaderAndWriter database)
    {
        try
        {
            TerminalVerificationResults terminalVerificationResults =
                database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
            ErrorIndication errorIndication = database.Get<ErrorIndication>(ErrorIndication.Tag);

            TerminalVerificationResults.Builder tvrBuilder = TerminalVerificationResults.GetBuilder();
            tvrBuilder.Reset(terminalVerificationResults);
            tvrBuilder.Set(TerminalVerificationResultCodes.DynamicDataAuthenticationFailed);
            ErrorIndication.Builder errorIndicationBuilder = ErrorIndication.GetBuilder();
            errorIndicationBuilder.Reset(errorIndication);

            // HACK: Check these values
            errorIndicationBuilder.Set(Level2Error.TerminalDataError);
            errorIndicationBuilder.Set(StatusWords.NotAvailable);

            database.Update(tvrBuilder.Complete());
            database.Update(errorIndicationBuilder.Complete());
        }
        catch (TerminalDataException exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
    }

    #region 6.5.2 Step 1

    /// <remarks>
    ///     Book 2 Section 6.5.2 Step 1
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateEncipheredSignatureLength(
        SignedDynamicApplicationData signedDynamicApplicationData, PublicKeyCertificate issuerPublicKeyCertificate)
    {
        if (signedDynamicApplicationData.GetByteCount() != issuerPublicKeyCertificate.GetPublicKeyModulus().GetByteCount())
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DynamicDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateEncipheredSignatureLength)} was invalid");
        }
    }

    #endregion

    #region 6.5.2 Step 2

    /// <remarks>
    ///     Book 2 Section 6.5.2 Step 2
    /// </remarks>
    private DecodedSignedDynamicApplicationData RecoverSignedDynamicApplicationData(
        PublicKeyCertificate issuerPublicKeyCertificate, SignedDynamicApplicationData encipheredData) =>
        new(_SignatureService.Decrypt(encipheredData.AsByteArray(), issuerPublicKeyCertificate));

    #endregion

    #region 6.5.2 Step 3

    // Step 3 is accomplished by Step 6-7 SignatureService.IsSignatureValid()

    #endregion

    #region 6.5.2 Step 4

    /// <remarks>
    ///     Book 2 Section 6.5.2 Step 4
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateSignedDataFormat(SignedDataFormat signedDataFormat)
    {
        if (signedDataFormat != SignedDataFormat._3)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DynamicDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateSignedDataFormat)} was invalid");
        }
    }

    #endregion

    #region 6.5.2 Step 5

    /// <exception cref="BerParsingException"></exception>
    private byte[] ReconstructDynamicDataToBeSigned(
        DecodedSignedDynamicApplicationData signedData, DataObjectListResult dynamicDataObjectListResult) =>
        signedData.GetMessage1().AsSpan().ConcatArrays(dynamicDataObjectListResult.AsByteArray());

    #endregion

    #region 6.5.2 Step 6 - 7

    /// <summary>
    ///     This method includes validation from previous steps regarding the validity of the deciphered signature
    /// </summary>
    /// <remarks>
    ///     Book 2 Section 6.5.2 Step 2, 3, 4, 6
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private void ValidateSignedData(
        DecodedSignedDynamicApplicationData decodedSignature, ReadOnlySpan<byte> concatenatedValue,
        DataObjectListResult dynamicDataObjectListResult)
    {
        if (!_SignatureService.IsSignatureValid(decodedSignature.GetHashAlgorithmIndicator(),
                                                ReconstructDynamicDataToBeSigned(decodedSignature, dynamicDataObjectListResult),
                                                decodedSignature))
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DynamicDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateSignedData)} was invalid");
        }
    }

    #endregion

    #endregion
}