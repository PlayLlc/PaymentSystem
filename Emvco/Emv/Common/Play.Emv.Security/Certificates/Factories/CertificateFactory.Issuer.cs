using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Security.Certificates.Factories;

internal partial class CertificateFactory
{
    #region Instance Members

    /// <remarks>
    ///     Book 3 Section 5.3
    /// </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public DecodedIssuerPublicKeyCertificate RecoverIssuerCertificate(ITlvReaderAndWriter tlvDatabase, ICertificateDatabase certificateDatabase)
    {
        try
        {
            IssuerPublicKeyCertificate issuerPublicKey = tlvDatabase.Get<IssuerPublicKeyCertificate>(IssuerPublicKeyCertificate.Tag);
            IssuerPublicKeyExponent issuerExponent = tlvDatabase.Get<IssuerPublicKeyExponent>(IssuerPublicKeyExponent.Tag);
            IssuerPublicKeyRemainder issuerRemainder = tlvDatabase.Get<IssuerPublicKeyRemainder>(IssuerPublicKeyRemainder.Tag);
            CaPublicKeyCertificate caPublicKey = RecoverCertificateAuthorityCertificate(tlvDatabase, certificateDatabase);
            DecodedSignature decodedSignature = _SignatureService.Decrypt(issuerPublicKey.GetEncipherment(), caPublicKey);

            // Step 1
            ValidateKeyLength(caPublicKey, issuerPublicKey);

            // Step 2
            RecoverSignedIssuerPublicKey(caPublicKey, issuerPublicKey);

            // Step 4
            ValidateCertificateFormat(decodedSignature);

            // Step 5
            byte[] concatenatedValues = GetConcatenatedValuesForHash(caPublicKey, decodedSignature, issuerRemainder, issuerExponent);

            // Step 6
            HashAlgorithmIndicator hashAlgorithmIndicator = DecodedIssuerPublicKeyCertificate.GetHashAlgorithmIndicator(decodedSignature.GetMessage1());

            // Step 7
            ValidateHashedResult(hashAlgorithmIndicator, concatenatedValues, decodedSignature);

            // Step 8
            ValidateIssuerIdentifier(tlvDatabase, decodedSignature);

            // Step 9
            ValidateExpiryDate(decodedSignature.GetMessage1());

            // Step 10
            ValidateIssuerCertificate(certificateDatabase, caPublicKey.GetRegisteredApplicationProviderIndicator(), caPublicKey.GetPublicKeySerialNumber());

            // Step 11
            ValidateIssuerPublicKeyAlgorithmIndicator(decodedSignature.GetMessage1());

            return DecodedIssuerPublicKeyCertificate.Create(caPublicKey, issuerRemainder, issuerExponent, decodedSignature, hashAlgorithmIndicator);
        }
        catch (TerminalDataException exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
        catch (CryptographicAuthenticationMethodFailedException)
        {
            // TODO: Logging
            throw;
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
    }

    #region 6.3 Step 1

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private static void ValidateKeyLength(CaPublicKeyCertificate caPublicKeyCertificate, IssuerPublicKeyCertificate issuerPublicKey)
    {
        if (caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() != issuerPublicKey.GetEncipherment().Length)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"Authentication failed because the {nameof(ValidateKeyLength)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
        }
    }

    #endregion

    #region 6.3 Step 2

    private DecodedSignature RecoverSignedIssuerPublicKey(CaPublicKeyCertificate caPublicKey, IssuerPublicKeyCertificate issuerPublicKey) =>
        _SignatureService.Decrypt(issuerPublicKey.GetEncipherment(), caPublicKey);

    #endregion

    #region 6.3 Step 4

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateCertificateFormat(DecodedSignature decodedSignature)
    {
        if (decodedSignature.GetMessage1()[0] != CertificateSources.Issuer)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"Authentication failed because the {nameof(ValidateCertificateFormat)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
        }
    }

    #endregion

    #region 6.3 Step 5

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private static byte[] GetConcatenatedValuesForHash(
        CaPublicKeyCertificate caPublicKeyCertificate, DecodedSignature decodedSignature, IssuerPublicKeyRemainder publicKeyRemainder,
        PublicKeyExponent publicKeyExponent)
    {
        byte issuerPublicKeyLength = DecodedIssuerPublicKeyCertificate.GetIssuerPublicKeyLength(decodedSignature.GetMessage1());
        byte issuerExponentLength = DecodedIssuerPublicKeyCertificate.GetIssuerPublicKeyExponentLength(decodedSignature.GetMessage1());

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(14 + issuerPublicKeyLength + issuerExponentLength);
        Span<byte> buffer = spanOwner.Span;

        decodedSignature.GetMessage1()[2..(2 + 14)].ToArray().AsSpan().CopyTo(buffer);
        DecodedIssuerPublicKeyCertificate.GetPublicKeyModulus(caPublicKeyCertificate, decodedSignature, publicKeyRemainder).AsByteArray().AsSpan()
            .CopyTo(buffer[17..]);
        publicKeyExponent.Encode().AsSpan().CopyTo(buffer[^publicKeyExponent.GetByteCount()..]);

        return buffer.ToArray();
    }

    #endregion

    #region 6.3 Step 6

    internal HashAlgorithmIndicator GetHashAlgorithmIndicator(Message1 message1) => HashAlgorithmIndicator.Get(message1[11]);

    #endregion

    #region 6.3 Step 7

    /// <summary>
    ///     This method includes validation from previous states regarding the validity of the deciphered signature from the
    ///     <see cref="IccPublicKeyCertificate" />
    /// </summary>
    /// <remarks>EMV Book 2 Section 6.3 Step 2, 3, 5 - 7 </remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateHashedResult(HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> concatenatedValues, DecodedSignature decodedSignature)
    {
        if (!_SignatureService.IsSignatureValid(hashAlgorithmIndicator, concatenatedValues, decodedSignature))
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"Authentication failed because the {nameof(ValidateHashedResult)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
        }
    }

    #endregion

    #region 6.3 Step 8

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private void ValidateIssuerIdentifier(ITlvReaderAndWriter database, DecodedSignature decodedSignature)
    {
        try
        {
            IssuerIdentificationNumber issuerIdentificationNumber =
                new(PlayCodec.CompressedNumericCodec.DecodeToUInt16(decodedSignature.GetMessage1()[new Range(1, 5)]));

            ApplicationPan pan = database.Get<ApplicationPan>(ApplicationPan.Tag);

            if (!pan.IsIssuerIdentifierMatching(issuerIdentificationNumber))
            {
                throw new CryptographicAuthenticationMethodFailedException(
                    $"Authentication failed because the {nameof(ValidateIssuerIdentifier)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
            }

            database.Update(issuerIdentificationNumber);
        }
        catch (TerminalDataException exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
        catch (CryptographicAuthenticationMethodFailedException)
        {
            // TODO: Logging
            throw;
        }
        catch (Exception exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
    }

    #endregion

    #region 6.3 Step 9

    /// <exception cref="CodecParsingException"></exception>
    private static void ValidateExpiryDate(Message1 message1)
    {
        ShortDate expiryDate = DecodedIssuerPublicKeyCertificate.GetCertificateExpirationDate(message1);

        ShortDate today = ShortDate.Today;

        if (expiryDate < today)
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"Authentication failed because the {nameof(ValidateExpiryDate)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
        }
    }

    #endregion

    #region 6.3 Step 10

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    private static void ValidateIssuerCertificate(
        ICertificateDatabase certificateDatabase, RegisteredApplicationProviderIndicator rid, CertificateSerialNumber serialNumber)
    {
        if (certificateDatabase.IsRevoked(rid, serialNumber))
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"Authentication failed because the {nameof(ValidateIssuerCertificate)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
        }
    }

    #endregion

    #region 6.3 Step 11

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    private static void ValidateIssuerPublicKeyAlgorithmIndicator(Message1 message1)
    {
        if (!PublicKeyAlgorithmIndicator.Exists((byte) DecodedIssuerPublicKeyCertificate.GetPublicKeyAlgorithmIndicator(message1)))
        {
            throw new CryptographicAuthenticationMethodFailedException(
                $"Authentication failed because the {nameof(ValidateIssuerPublicKeyAlgorithmIndicator)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
        }
    }

    #endregion

    #endregion

    #region Recovery Steps

    #endregion

    #region 6.3 Step 3

    // Step 3 is handled by Step 7. The deciphered signature validation is encapsulated in  the SignatureService.IsSignatureValid()

    #endregion

    #region Helper Methods

    #endregion
}