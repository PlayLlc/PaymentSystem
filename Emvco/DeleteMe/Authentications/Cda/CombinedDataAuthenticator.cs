using DeleteMe.Authentications.Dda;
using DeleteMe.Certificates;
using DeleteMe.Exceptions;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Pcd.Contracts;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;
using Play.Icc.Messaging.Apdu;

namespace DeleteMe.Authentications.Cda;

internal partial class CombinedDataAuthenticator : IAuthenticateCombinedData
{
    #region Instance Values

    private readonly BerCodec _BerCodec;
    private readonly HashAlgorithmProvider _HashAlgorithmProvider;
    private readonly SignatureService _SignatureService;
    private readonly CertificateFactory _CertificateFactory;

    #endregion

    #region Constructor

    public CombinedDataAuthenticator(CertificateFactory certificateFactory)
    {
        _CertificateFactory = certificateFactory;
    }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="Play.Codecs.Exceptions.CodecParsingException"></exception>
    public void AuthenticateFirstCda(
        GenerateApplicationCryptogramResponse rapdu, ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated)
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
        IccDynamicData iccDynamicData = decodedSignature.GetIccDynamicData();

        // Step 6
        ValidateCryptogramInformationData(database, iccDynamicData);

        // Step 7
        ReadOnlySpan<byte> dataToBeSigned = ReconstructDynamicDataToBeSigned(decodedSignature);

        // Step 8
        Hash calculatedHash = ProduceHashResult(decodedSignature.GetHashAlgorithmIndicator(), dataToBeSigned,
                                                decodedSignature.GetIccDynamicData().GetTransactionDataHashCode());

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

        ValidateTransactionDataHashCode(decodedSignature, transactionDataHashCode);

        try
        { }
        catch (Exception)
        {
            // TODO: Logging

            SetCombinedDataAuthenticationFailed(database);
        }
    }

    #endregion

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
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateEncipheredSignatureLength)} was invalid");
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
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateSignedDataFormat)} was invalid");
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
        var dynamicCid = dynamicData.GetCryptogramInformationData();
        CryptogramInformationData cryptogramInformationData = database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

        if (dynamicCid != cryptogramInformationData)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateCryptogramInformationData)} was invalid");
        }
    }

    #endregion

    #region 6.6.2 Step 7

    private byte[] ReconstructDynamicDataToBeSigned(DecodedSignedDynamicApplicationDataCda decodedSignedDynamicApplicationData) =>
        decodedSignedDynamicApplicationData.GetSignatureHashPlainText();

    #endregion

    private bool IsSignedDynamicDataValid(DecodedSignedDynamicApplicationDataCda decodedSignature) =>
        _SignatureService.IsSignatureValid(decodedSignature.GetHashAlgorithmIndicator(), ReconstructDynamicDataToBeSigned(decodedSignature),
                                           decodedSignature);

    #region 6.6.2 Step 8

    private Hash ProduceHashResultForDynamicData(
        HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> transactionDataHashCodeInput) =>
        _HashAlgorithmProvider.Generate(transactionDataHashCodeInput, hashAlgorithmIndicator);

    #endregion

    #region 6.6.2 Step 9

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateHashResult(Hash recoveredHash, Hash calculatedHash)
    {
        if (recoveredHash != calculatedHash)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateHashResult)} was invalid");
        }
    }

    #endregion

    #region 6.6.2 Step 12

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateTransactionDataHashCode(DecodedSignedDynamicApplicationDataCda decodedSignature, Hash transactionDataHashCode)
    {
        if (decodedSignature.GetIccDynamicData().GetTransactionDataHashCode() != transactionDataHashCode)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(CombinedDataAuthenticator)} failed Dynamic Data Authentication because the constraint {nameof(ValidateTransactionDataHashCode)} was invalid");
        }
    }

    #endregion

    #region 6.2.2 Step 10.a

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
    /// <exception cref="OverflowException"></exception>
    private byte[] ConcatenateTransactionDataHashCode(ITlvReaderAndWriter database, GenerateApplicationCryptogramResponse rapdu)
    {
        ProcessingOptionsDataObjectList pdol = database.Get<ProcessingOptionsDataObjectList>(ProcessingOptionsDataObjectList.Tag);
        CardRiskManagementDataObjectList1 cdol1 = database.Get<CardRiskManagementDataObjectList1>(CardRiskManagementDataObjectList1.Tag);
        PrimitiveValue[] rapduDataObjects = rapdu.GetPrimitiveDataObjects();
        int pdolResultLength = pdol.GetValueByteCountOfCommandTemplate();
        int cdolResultLength = cdol1.GetValueByteCountOfCommandTemplate();
        int rapduResultLength = (int) rapduDataObjects.Sum(a => a.GetTagLengthValueByteCount(EmvCodec.GetBerCodec()));

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(pdolResultLength + cdolResultLength + rapduResultLength);
        Span<byte> buffer = spanOwner.Span;

        pdol.AsCommandTemplate(database).EncodeValue().CopyTo(buffer);
        cdol1.AsCommandTemplate(database).EncodeValue().CopyTo(buffer[pdol.GetValueByteCountOfCommandTemplate()..]);
        rapduDataObjects.SelectMany(a => a.EncodeTagLengthValue(EmvCodec.GetBerCodec())).ToArray().AsSpan()
            .CopyTo(buffer[^rapduResultLength..]);

        return buffer.ToArray();
    }

    #region 6.6.2 Step 11

    private Hash ProduceTransactionDataHashCode(
        HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> transactionDataHashCodeInput) =>
        _HashAlgorithmProvider.Generate(transactionDataHashCodeInput, hashAlgorithmIndicator);

    #endregion

    #endregion

    #region 6.6.2 Step 3

    // Step 3 is covered by SignatureService.IsSignatureValid() in another method

    #endregion

    #region 6.6.2 Step 5

    //private void UpdateRecoveredData(ITlvReaderAndWriter database, DecodedSignedDynamicApplicationDataCda GetIccDynamicData)
    //{
    //    IccDynamicData dynamicData = GetIccDynamicData.GetIccDynamicData();
    //    database.Update(dynamicData.GetIccDynamicNumber());
    //    database.Update(dynamicData.GetCryptogramInformationData());
    //    database.Update(dynamicData.GetCryptogram());
    //}

    #endregion

    // This step is done during initialization of a DecodedSignedDynamicApplicationDataDda

    // Transaction Data Hash Code Input is retrieved by the AuthenticateCombinedData1Command or AuthenticateCombinedData2Command objects
}