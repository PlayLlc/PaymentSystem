using DeleteMe.Certificates.Issuer;
using DeleteMe.Exceptions;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;

namespace DeleteMe.Certificates.Icc;

public class DecodedIccPublicKeyCertificate : PublicKeyCertificate
{
    #region Static Metadata

    private static readonly NumericCodec _Codec = PlayCodec.NumericCodec;
    private static readonly CertificateSources _CertificateSources = CertificateSources.Icc;

    #endregion

    #region Constructor

    public DecodedIccPublicKeyCertificate(
        DateRange validityPeriod, CertificateSerialNumber certificateSerialNumber, HashAlgorithmIndicator hashAlgorithmIndicator,
        PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator, PublicKeyInfo publicKeyInfo) : base(certificateSerialNumber,
     hashAlgorithmIndicator, publicKeyAlgorithmIndicator, validityPeriod, publicKeyInfo)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Concatenates the values that are required for the recovery hash validation
    /// </summary>
    /// <param name="issuerCertificate"></param>
    /// <param name="publicKeyExponent"></param>
    /// <param name="staticDataToBeAuthenticated"></param>
    /// <param name="publicKeyRemainder"></param>
    /// <returns></returns>
    public static byte[] ConcatenateRecoveryHash(
        DecodedSignature issuerCertificate, IccPublicKeyExponent publicKeyExponent, StaticDataToBeAuthenticated staticDataToBeAuthenticated,
        IccPublicKeyRemainder? publicKeyRemainder = null)
    {
        int leftMostDataElementDigitsByteCount = issuerCertificate.GetMessage1().GetByteCount() - 2;
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate((leftMostDataElementDigitsByteCount
                                                                       + publicKeyExponent.GetValueByteCount()
                                                                       + staticDataToBeAuthenticated.GetByteCount()
                                                                       + publicKeyRemainder?.GetByteCount())
                                                                   ?? 0);

        Span<byte> buffer = spanOwner.Span;
        int offset = 0;
        issuerCertificate.GetMessage1()[1..^1].ToArray().AsSpan().CopyTo(buffer);
        offset += leftMostDataElementDigitsByteCount;

        if (publicKeyRemainder != null)
        {
            publicKeyRemainder!.AsPublicKeyRemainder().AsSpan().CopyTo(buffer[offset..]);
            offset += publicKeyRemainder?.GetByteCount() ?? 0;
        }

        publicKeyExponent.EncodeValue().AsSpan().CopyTo(buffer[offset..]);
        offset += publicKeyExponent.GetValueByteCount();

        staticDataToBeAuthenticated.Encode().CopyTo(buffer[offset..]);

        return buffer.ToArray();
    }

    /// <summary>
    ///     Resolves a <see cref="PublicKeyModulus" /> that is specific to the <see cref="DecodedIccPublicKeyCertificate" />
    ///     format
    /// </summary>
    /// <param name="iccModulusLength"></param>
    /// <param name="issuerPublicKeyCertificate"></param>
    /// <param name="message1"></param>
    /// <param name="publicKeyRemainder"></param>
    /// <returns></returns>
    internal static PublicKeyModulus ResolvePublicKeyModulus(
        byte iccModulusLength, DecodedIssuerPublicKeyCertificate issuerPublicKeyCertificate, Message1 message1,
        PublicKeyRemainder? publicKeyRemainder = null)
    {
        Span<byte> modulusBuffer = stackalloc byte[iccModulusLength];
        message1[1..(message1.GetByteCount() - 42)].CopyTo(modulusBuffer);

        if (!issuerPublicKeyCertificate.IsPublicKeySplit())
            return new PublicKeyModulus(modulusBuffer.ToArray());

        publicKeyRemainder!.AsSpan().CopyTo(modulusBuffer[^publicKeyRemainder.GetByteCount()..]);

        return new PublicKeyModulus(message1[1..(message1.GetByteCount() - 42)]);
    }

    private static ShortDate GetCertificateExpirationDate(Message1 message1) => new(_Codec.DecodeToUInt16(message1[new Range(11, 13)]));
    public CertificateSources GetCertificateFormat() => _CertificateSources;

    /// <summary>
    /// </summary>
    /// <param name="issuerCertificate"></param>
    /// <param name="staticDataToBeAuthenticated"></param>
    /// <param name="encipheredCertificate"></param>
    /// <param name="applicationPan"></param>
    /// <param name="encipheredPublicKeyRemainder"></param>
    /// <returns></returns>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <remarks>EMV Book 2 Section 6.4</remarks>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static DecodedIccPublicKeyCertificate Create(
        DecodedIssuerPublicKeyCertificate issuerCertificate, StaticDataToBeAuthenticated staticDataToBeAuthenticated,
        IccPublicKeyCertificate encipheredCertificate, ApplicationPan applicationPan,
        IccPublicKeyRemainder? encipheredPublicKeyRemainder = null)
    {
        // Step 1
        if (issuerCertificate.GetPublicKeyModulus().GetByteCount() != encipheredCertificate.GetValueByteCount())
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the length of the Issuer Public Key Modulus is different than {nameof(IccPublicKeyCertificate)}");
        }

        // Step 2
        DecodedSignature decodedSignature = _SignatureService.Decrypt(encipheredCertificate.EncodeValue(), issuerCertificate);

        CertificateSources certificateSources;
        ApplicationPan recoveredPan;
        ShortDate expirationDate;
        CertificateSerialNumber serialNumber;
        HashAlgorithmIndicator hashAlgorithmIndicator;
        byte iccModulusLength;
        IccPublicKeyExponent exponent;

        try
        {
            certificateSources = CertificateSources.Get(decodedSignature.GetMessage1()[0]);
            recoveredPan = ApplicationPan.Decode(decodedSignature.GetMessage1()[1..11]);
            expirationDate = new ShortDate(PlayCodec.NumericCodec.DecodeToUInt16(decodedSignature.GetMessage1()[new Range(11, 13)]));
            serialNumber = new CertificateSerialNumber(decodedSignature.GetMessage1()[13..16]);
            hashAlgorithmIndicator = HashAlgorithmIndicator.Get(decodedSignature.GetMessage1()[16]);

            iccModulusLength = decodedSignature.GetMessage1()[18];
            byte iccExponentLength = decodedSignature.GetMessage1()[19];
            exponent = iccExponentLength > 1
                ? new IccPublicKeyExponent(PublicKeyExponent._65537)
                : new IccPublicKeyExponent(PublicKeyExponent._3);
        }
        catch (CodecParsingException exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
        catch (Exception exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }

        // Step 4
        if (certificateSources != CertificateSources.Icc)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(CertificateSources)} expected is {CertificateSources.Icc} but the format provided was: [{certificateSources}]");
        }

        // Step 5
        byte[] hashSeed = ConcatenateRecoveryHash(decodedSignature, exponent, staticDataToBeAuthenticated, encipheredPublicKeyRemainder);

        // Step 3, 4, 6, 7, 
        if (!_SignatureService.IsSignatureValid(hashAlgorithmIndicator, hashSeed, decodedSignature))
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the hash validation failed");
        }

        // Step 8
        if (recoveredPan != applicationPan)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(ApplicationPan)} is different than the value recovered from {nameof(IccPublicKeyCertificate)}");
        }

        // Step 9
        if (DateTimeUtc.Today() > expirationDate)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(IccPublicKeyCertificate)} has expired");
        }

        // Step 10
        if (!PublicKeyAlgorithmIndicator.TryGet(decodedSignature.GetMessage1()[17],
                                                out PublicKeyAlgorithmIndicator? publicKeyAlgorithmIndicator))
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(PublicKeyAlgorithmIndicator)} value: [{decodedSignature.GetMessage1()[17]}] could not be recognized");
        }

        PublicKeyModulus publicKeyModulus = ResolvePublicKeyModulus(iccModulusLength, issuerCertificate, decodedSignature.GetMessage1(),
                                                                    encipheredPublicKeyRemainder?.AsPublicKeyRemainder());

        return new DecodedIccPublicKeyCertificate(new DateRange(ShortDate.Min, expirationDate), serialNumber, hashAlgorithmIndicator,
                                                  publicKeyAlgorithmIndicator!,
                                                  new PublicKeyInfo(publicKeyModulus, exponent.AsPublicKeyExponent(),
                                                                    encipheredPublicKeyRemainder?.AsPublicKeyRemainder()));
    }

    #endregion
}