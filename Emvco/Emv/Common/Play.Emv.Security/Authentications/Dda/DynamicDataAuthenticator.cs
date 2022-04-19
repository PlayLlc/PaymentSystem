using Play.Ber.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Security.Certificates;
using Play.Emv.Security.Certificates.Factories;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Dda;

internal class DynamicDataAuthenticator
{
    #region Instance Values

    private readonly SignatureService _SignatureService;
    private readonly CertificateFactory _CertificateFactory;

    #endregion

    #region Constructor

    public DynamicDataAuthenticator(SignatureService signatureService, CertificateFactory certificateFactory)
    {
        _SignatureService = signatureService;
        _CertificateFactory = certificateFactory;
    }

    #endregion

    #region Instance Members

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void Authenticate(ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated)
    {
        try
        {
            SignedDynamicApplicationData signedDynamicApplicationData = database.Get<SignedDynamicApplicationData>(SignedDynamicApplicationData.Tag);

            DecodedIccPublicKeyCertificate decodedIccCertificate =
                _CertificateFactory.RecoverIccCertificate(database, certificateDatabase, staticDataToBeAuthenticated);

            // Step 1
            ValidateEncipheredSignatureLength(signedDynamicApplicationData, decodedIccCertificate);

            // Step 2
            DecodedSignedDynamicApplicationData decodedSignature = RecoverSignedDynamicApplicationData(decodedIccCertificate,
                database.Get<SignedDynamicApplicationData>(SignedDynamicApplicationData.Tag));

            // Step 3 is accomplished by Step 6 - 7 below

            // Step 4
            ValidateSignedDataFormat(decodedSignature.GetSignedDataFormat());

            DataObjectListResult ddolResult = database.Get<DynamicDataAuthenticationDataObjectList>(DynamicDataAuthenticationDataObjectList.Tag)
                .AsDataObjectListResult(database);

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
            TerminalVerificationResults terminalVerificationResults = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

            TerminalVerificationResults.Builder builder = TerminalVerificationResults.GetBuilder();
            builder.Reset(terminalVerificationResults);
            builder.Set(TerminalVerificationResultCodes.DynamicDataAuthenticationFailed);

            database.Update(builder.Complete());
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
    private void ValidateEncipheredSignatureLength(SignedDynamicApplicationData signedDynamicApplicationData, PublicKeyCertificate issuerPublicKeyCertificate)
    {
        if (signedDynamicApplicationData.GetByteCount() != issuerPublicKeyCertificate.GetPublicKeyModulus().GetByteCount())
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DynamicDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateEncipheredSignatureLength)} was invalid");
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

    #region 6.5.2 Step 4

    /// <remarks>
    ///     Book 2 Section 6.5.2 Step 4
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateSignedDataFormat(SignedDataFormat signedDataFormat)
    {
        if (signedDataFormat != SignedDataFormat._3)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DynamicDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateSignedDataFormat)} was invalid");
        }
    }

    #endregion

    #region 6.5.2 Step 5

    /// <exception cref="BerParsingException"></exception>
    private byte[] ReconstructDynamicDataToBeSigned(DecodedSignedDynamicApplicationData signedData, DataObjectListResult dynamicDataObjectListResult) =>
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
        DecodedSignedDynamicApplicationData decodedSignature, ReadOnlySpan<byte> concatenatedValue, DataObjectListResult dynamicDataObjectListResult)
    {
        if (!_SignatureService.IsSignatureValid(decodedSignature.GetHashAlgorithmIndicator(),
            ReconstructDynamicDataToBeSigned(decodedSignature, dynamicDataObjectListResult), decodedSignature))
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DynamicDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateSignedData)} was invalid");
        }
    }

    #endregion

    #endregion

    #region 6.5.2 Step 3

    // Step 3 is accomplished by Step 6-7 SignatureService.IsSignatureValid()

    #endregion
}