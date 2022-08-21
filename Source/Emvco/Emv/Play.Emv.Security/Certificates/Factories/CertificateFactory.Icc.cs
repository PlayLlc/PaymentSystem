using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates.Factories;

internal partial class CertificateFactory
{
    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public DecodedIccPublicKeyCertificate RecoverIccCertificate(
        ITlvReaderAndWriter tlvDatabase, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated)
    {
        tlvDatabase.TryGet(IccPublicKeyRemainder.Tag, out IccPublicKeyRemainder? iccPublicKeyRemainder);
        IccPublicKeyCertificate iccPublicKeyCertificate = tlvDatabase.Get<IccPublicKeyCertificate>(IccPublicKeyCertificate.Tag);
        ApplicationPan applicationPan = tlvDatabase.Get<ApplicationPan>(ApplicationPan.Tag);

        DecodedIssuerPublicKeyCertificate issuerCertificate = RecoverIssuerCertificate(tlvDatabase, certificateDatabase);

        // Step 1
        if (issuerCertificate.GetPublicKeyModulus().GetByteCount() != iccPublicKeyCertificate.GetValueByteCount())
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the length of the Issuer Public Key Modulus is different than {nameof(IccPublicKeyCertificate)}");
        }

        // Step 2
        DecodedSignature decodedSignature = _SignatureService.Decrypt(iccPublicKeyCertificate.EncodeValue(), issuerCertificate);

        CertificateSources certificateSources;
        ApplicationPan recoveredPan;
        ShortDate expirationDate;
        CertificateSerialNumber serialNumber;
        HashAlgorithmIndicators hashAlgorithmIndicators;
        byte iccModulusLength;
        IccPublicKeyExponent exponent;

        // Resolving decoded signature values
        try
        {
            certificateSources = CertificateSources.Get(decodedSignature.GetMessage1()[0]);
            recoveredPan = ApplicationPan.Decode(decodedSignature.GetMessage1()[1..11]);
            expirationDate = new ShortDate(PlayCodec.NumericCodec.DecodeToUInt16(decodedSignature.GetMessage1()[new Range(11, 13)]));
            serialNumber = new CertificateSerialNumber(decodedSignature.GetMessage1()[13..16]);
            HashAlgorithmIndicators.Empty.TryGet(decodedSignature.GetMessage1()[16], out EnumObject<byte>? hashAlgorithmIndicatorResult);
            hashAlgorithmIndicators = (HashAlgorithmIndicators) hashAlgorithmIndicatorResult!;

            iccModulusLength = decodedSignature.GetMessage1()[18];
            byte iccExponentLength = decodedSignature.GetMessage1()[19];
            exponent = iccExponentLength > 1 ? new IccPublicKeyExponent(PublicKeyExponents._65537) : new IccPublicKeyExponent(PublicKeyExponents._3);
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
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(CertificateSources)} expected is {CertificateSources.Icc} but the format provided was: [{certificateSources}]");
        }

        // Step 5
        byte[] hashSeed = DecodedIccPublicKeyCertificate.ConcatenateRecoveryHash(decodedSignature, exponent,
            staticDataToBeAuthenticated, iccPublicKeyRemainder);

        // Step 3, 4, 6, 7, 
        if (!_SignatureService.IsSignatureValid(hashAlgorithmIndicators, hashSeed, decodedSignature))
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the hash validation failed");
        }

        // Step 8
        if (recoveredPan != applicationPan)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(ApplicationPan)} is different than the value recovered from {nameof(IccPublicKeyCertificate)}");
        }

        // Step 9
        if (DateTimeUtc.Today > expirationDate)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(IccPublicKeyCertificate)} has expired");
        }

        // Step 10
        if (!PublicKeyAlgorithmIndicators.Empty.TryGet(decodedSignature.GetMessage1()[17], out EnumObject<byte>? publicKeyAlgorithmIndicator))
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(PublicKeyAlgorithmIndicators)} value: [{decodedSignature.GetMessage1()[17]}] could not be recognized");
        }

        PublicKeyModulus publicKeyModulus = DecodedIccPublicKeyCertificate.ResolvePublicKeyModulus(iccModulusLength, issuerCertificate,
            decodedSignature.GetMessage1(), iccPublicKeyRemainder?.AsPublicKeyRemainder());

        return new DecodedIccPublicKeyCertificate(new DateRange(ShortDate.Min, expirationDate), serialNumber, hashAlgorithmIndicators,
            (PublicKeyAlgorithmIndicators) publicKeyAlgorithmIndicator!,
            new PublicKeyInfo(publicKeyModulus, exponent.AsPublicKeyExponent(), iccPublicKeyRemainder?.AsPublicKeyRemainder()));
    }

    #endregion
}