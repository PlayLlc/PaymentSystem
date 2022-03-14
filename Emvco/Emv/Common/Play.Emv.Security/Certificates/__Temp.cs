using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Emv.DataElements;
using Play.Emv.Security.Authentications.Static;
using Play.Emv.Security.Certificates.Icc;
using Play.Emv.Security.Certificates.Issuer;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates;

internal class __Temp
{
    #region Static Metadata

    private static readonly NumericCodec _Codec = PlayCodec.NumericCodec;

    #endregion

    #region Instance Values

    private readonly SignatureService _SignatureService = new();

    #endregion

    #region Instance Members

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
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private DecodedIccPublicKeyCertificate Create(
        DecodedIssuerPublicKeyCertificate issuerCertificate,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated,
        IccPublicKeyCertificate encipheredCertificate,
        ApplicationPan applicationPan,
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

        // created shit

        CertificateFormat certificateFormat = CertificateFormat.Get(decodedSignature.GetMessage1()[0]);
        ApplicationPan recoveredPan = ApplicationPan.Decode(decodedSignature.GetMessage1()[1..11]);
        ShortDateValue expirationDate = new(PlayCodec.NumericCodec.DecodeToUInt16(decodedSignature.GetMessage1()[new Range(11, 13)]));
        CertificateSerialNumber serialNumber = new(decodedSignature.GetMessage1()[13..16]);
        HashAlgorithmIndicator hashAlgorithmIndicator = HashAlgorithmIndicator.Get(decodedSignature.GetMessage1()[16]);

        byte iccModulusLength = decodedSignature.GetMessage1()[18];
        byte iccExponentLength = decodedSignature.GetMessage1()[19];
        IccPublicKeyExponent exponent = iccExponentLength > 1
            ? new IccPublicKeyExponent(PublicKeyExponent._65537)
            : new IccPublicKeyExponent(PublicKeyExponent._3);

        // Step 4
        if (certificateFormat != CertificateFormat.Icc)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(DecodedIccPublicKeyCertificate)} could not be created because the {nameof(CertificateFormat)} expected is {CertificateFormat.Icc} but the format provided was: [{certificateFormat}]");
        }

        // Step 5
        byte[] hashSeed = DecodedIccPublicKeyCertificate.ConcatenateRecoveryHash(decodedSignature, exponent, staticDataToBeAuthenticated,
                                                                                 encipheredPublicKeyRemainder);

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

        PublicKeyModulus publicKeyModulus = DecodedIccPublicKeyCertificate.ResolvePublicKeyModulus(iccModulusLength, issuerCertificate,
         decodedSignature.GetMessage1(), encipheredPublicKeyRemainder?.AsPublicKeyRemainder());

        return new DecodedIccPublicKeyCertificate(new DateRange(ShortDateValue.MinValue, expirationDate), serialNumber,
                                                  hashAlgorithmIndicator, publicKeyAlgorithmIndicator!,
                                                  new PublicKeyInfo(publicKeyModulus, exponent.AsPublicKeyExponent(),
                                                                    encipheredPublicKeyRemainder?.AsPublicKeyRemainder()));
    }

    #endregion
}