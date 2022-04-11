using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security.Certificates;
using Play.Emv.Security.Certificates.Factories;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Encryption.Signing;

namespace Play.Emv.Security.Authentications.Cda;

internal class CombinedDataAuthenticator
{
    #region Instance Values

    private readonly EmvCodec _Codec;
    private readonly HashAlgorithmProvider _HashAlgorithmProvider;
    private readonly SignatureService _SignatureService;
    private readonly CertificateFactory _CertificateFactory;

    #endregion

    #region Constructor

    public CombinedDataAuthenticator(
        HashAlgorithmProvider hashAlgorithmProvider, SignatureService signatureService, CertificateFactory certificateFactory)
    {
        _Codec = EmvCodec.GetBerCodec();
        _HashAlgorithmProvider = hashAlgorithmProvider;
        _SignatureService = signatureService;
        _CertificateFactory = certificateFactory;
    }

    #endregion

    #region Instance Members

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public void AuthenticateFirstGenAc(
        GenerateApplicationCryptogramResponse rapdu, ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated)
    {
        try
        {
            SignedDynamicApplicationData signedDynamicApplicationData =
                database.Get<SignedDynamicApplicationData>(SignedDynamicApplicationData.Tag);

            DecodedIccPublicKeyCertificate decodedIccCertificate =
                _CertificateFactory.RecoverIccCertificate(database, certificateDatabase, staticDataToBeAuthenticated);

            // Step 1
            ValidateEncipheredSignatureLength(signedDynamicApplicationData, decodedIccCertificate);

            // Step 2, Step 
            DecodedSignedDynamicApplicationDataCda decodedSignature =
                RecoverSignedDynamicApplicationData(decodedIccCertificate, signedDynamicApplicationData);

            // Step 3 is covered by SignatureService.IsSignatureValid() below

            // Step 4
            ValidateSignedDataFormat(decodedSignature.GetSignedDataFormat());

            // Step 5 
            IccDynamicData iccDynamicData = RetrieveIccDynamicData(database, decodedSignature);

            // Step 6
            ValidateCryptogramInformationData(database, iccDynamicData);

            // Step 7
            ReadOnlySpan<byte> dataToBeSigned = ReconstructDynamicDataToBeSigned(decodedSignature);

            // Step 8
            Hash calculatedHash = ProduceHashResultForDynamicData(decodedSignature.GetHashAlgorithmIndicator(), dataToBeSigned);

            // Step 9
            ValidateHashResult(decodedSignature.GetHash(), calculatedHash);

            // WARNING =========================================================================================================================================
            // BUG: For now we are only validating for the first GENERATE AC request. We are NOT distinguishing between the first and second when authenticating
            // WARNING =========================================================================================================================================

            // Step 10
            ReadOnlySpan<byte> concatenatedTransactionData = ConcatenateTransactionDataHashCode(database, rapdu);

            // Step 11
            Hash transactionDataHashCode =
                ProduceTransactionDataHashCode(decodedSignature.GetHashAlgorithmIndicator(), concatenatedTransactionData);

            // Step 12
            ValidateTransactionDataHashCode(decodedSignature, transactionDataHashCode);
        }
        catch (TerminalDataException)
        {
            // TODO: Logging 
            SetCombinedDataAuthenticationFailed(database);
        }
        catch (CryptographicAuthenticationMethodFailedException)
        {
            // TODO: Logging 
            SetCombinedDataAuthenticationFailed(database);
        }
        catch (CodecParsingException)
        {
            // TODO: Logging 
            SetCombinedDataAuthenticationFailed(database);
        }

        catch (Exception)
        {
            // TODO: Logging 
            SetCombinedDataAuthenticationFailed(database);
        }
    }

    #region Section 6 CDA Failed

    /// <remarks>
    ///     Book 2 Section 6
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private static void SetCombinedDataAuthenticationFailed(ITlvReaderAndWriter database)
    {
        try
        {
            TerminalVerificationResults terminalVerificationResults =
                database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);
            TerminalVerificationResults.Builder tvrBuilder = TerminalVerificationResults.GetBuilder();
            tvrBuilder.Reset(terminalVerificationResults);
            tvrBuilder.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);
            database.Update(tvrBuilder.Complete());
        }
        catch (TerminalDataException exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
    }

    #endregion

    #region 6.6.2 Step 1

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 1
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateEncipheredSignatureLength(
        SignedDynamicApplicationData signedDynamicApplicationData, DecodedIccPublicKeyCertificate iccPublicKeyCertificate)
    {
        if (signedDynamicApplicationData.GetByteCount() != iccPublicKeyCertificate.GetPublicKeyModulus().GetByteCount())
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateEncipheredSignatureLength)} was invalid");
        }
    }

    #endregion

    #region 6.6.2 Step 2

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 2
    /// </remarks>
    private DecodedSignedDynamicApplicationDataCda RecoverSignedDynamicApplicationData(
        PublicKeyCertificate issuerPublicKeyCertificate, SignedDynamicApplicationData encipheredData) =>
        new(_SignatureService.Decrypt(encipheredData.AsByteArray(), issuerPublicKeyCertificate));

    #endregion

    #region 6.6.2 Step 4

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 4
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateSignedDataFormat(SignedDataFormat signedDataFormat)
    {
        if (signedDataFormat != SignedDataFormat._5)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateSignedDataFormat)} was invalid");
        }
    }

    #endregion

    #region 6.6.2 Step 6

    /// <remarks>
    ///     Book 2 Section 6.6.2 Step 6
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private void ValidateCryptogramInformationData(ITlvReaderAndWriter database, IccDynamicData dynamicData)
    {
        CryptogramInformationData? dynamicCid = dynamicData.GetCryptogramInformationData();
        CryptogramInformationData cryptogramInformationData = database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

        if (dynamicCid != cryptogramInformationData)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateCryptogramInformationData)} was invalid");
        }
    }

    #endregion

    #region 6.6.2 Step 7

    /// <remarks>EMV Book 2 Section 6.6.2 Step 7</remarks>
    private byte[] ReconstructDynamicDataToBeSigned(DecodedSignedDynamicApplicationDataCda decodedSignedDynamicApplicationData) =>
        decodedSignedDynamicApplicationData.GetSignatureHashPlainText();

    #endregion

    #region 6.6.2 Step 8

    /// <remarks>EMV Book 2 Section 6.6.2 Step 8</remarks>
    private Hash ProduceHashResultForDynamicData(
        HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> transactionDataHashCodeInput) =>
        _HashAlgorithmProvider.Generate(transactionDataHashCodeInput, hashAlgorithmIndicator);

    #endregion

    #region 6.6.2 Step 9

    /// <remarks>EMV Book 2 Section 6.6.2 Step 9</remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateHashResult(Hash recoveredHash, Hash calculatedHash)
    {
        if (recoveredHash != calculatedHash)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateHashResult)} was invalid");
        }
    }

    #endregion

    #region 6.6.2 Step 12

    /// <remarks>EMV Book 2 Section 6.6.2 Step 12</remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateTransactionDataHashCode(DecodedSignedDynamicApplicationDataCda decodedSignature, Hash transactionDataHashCode)
    {
        if (decodedSignature.GetIccDynamicData().GetTransactionDataHashCode() != transactionDataHashCode)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateTransactionDataHashCode)} was invalid");
        }
    }

    #endregion

    #region 6.6.2 Step 5

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public IccDynamicData RetrieveIccDynamicData(ITlvReaderAndWriter database, DecodedSignedDynamicApplicationDataCda decodedSignature)
    {
        IccDynamicData iccDynamicData = decodedSignature.GetIccDynamicData();

        database.Update(iccDynamicData.GetCryptogram());
        database.Update(iccDynamicData.GetIccDynamicNumber());

        if (iccDynamicData.TryGetAdditionalData(out PrimitiveValue[] additionalData))
            database.Update(additionalData);

        return iccDynamicData;
    }

    #endregion

    #endregion

    #region 6.2.2 Step 10.a

    /// <remarks>EMV Book 2 Section 6.6.2 Step 10.a</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    private byte[] ConcatenateTransactionDataHashCode(ITlvReaderAndWriter database, GenerateApplicationCryptogramResponse rapdu)
    {
        ProcessingOptionsDataObjectList pdol = database.Get<ProcessingOptionsDataObjectList>(ProcessingOptionsDataObjectList.Tag);
        CardRiskManagementDataObjectList1 cdol1 = database.Get<CardRiskManagementDataObjectList1>(CardRiskManagementDataObjectList1.Tag);
        PrimitiveValue[] rapduDataObjects = rapdu.GetPrimitiveDataObjects(database);
        int pdolResultLength = pdol.GetValueByteCountOfCommandTemplate();
        int cdolResultLength = cdol1.GetValueByteCountOfCommandTemplate();
        int rapduResultLength = (int) rapduDataObjects.Sum(a => a.GetTagLengthValueByteCount(_Codec));

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(pdolResultLength + cdolResultLength + rapduResultLength);
        Span<byte> buffer = spanOwner.Span;

        pdol.AsCommandTemplate(database).EncodeValue().CopyTo(buffer);
        cdol1.AsCommandTemplate(database).EncodeValue().CopyTo(buffer[pdol.GetValueByteCountOfCommandTemplate()..]);
        rapduDataObjects.SelectMany(a => a.EncodeTagLengthValue(_Codec)).ToArray().AsSpan().CopyTo(buffer[^rapduResultLength..]);

        return buffer.ToArray();
    }

    #region 6.6.2 Step 11

    /// <remarks>EMV Book 2 Section 6.6.2 Step 11</remarks>
    private Hash ProduceTransactionDataHashCode(
        HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> transactionDataHashCodeInput) =>
        _HashAlgorithmProvider.Generate(transactionDataHashCodeInput, hashAlgorithmIndicator);

    #endregion

    #endregion

    #region 6.6.2 Step 3

    // Step 3 is covered by SignatureService.IsSignatureValid() in another method

    #endregion
}