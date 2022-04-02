using DeleteMe.Exceptions;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;

namespace DeleteMe.Certificates.Issuer._Temp;

internal partial class CertificateFactory
{
    /// <remarks>
    ///     EMV Book 2 Section 6.3
    /// </remarks>
    internal class Issuer
    {
        #region Static Metadata

        private static readonly CompressedNumericCodec _CompressedNumericCodec = new();
        private static readonly NumericCodec _NumericCodec = new();

        #endregion

        #region Instance Values

        private readonly SignatureService _SignatureService;

        #endregion

        #region Instance Members

        #region Helpers

        public Issuer()
        {
            _SignatureService = new SignatureService();
        }

        // TODO: The remainder and exponent will be coming from the TLV Database. No need to pass those with the
        // TODO: command. You can pass the ITlvDatabase and query when it's relevant
        private static byte GetIssuerPublicKeyExponentLength(Message1 message1) => message1[14];

        private static bool IsIssuerPublicKeyAlgorithmIndicatorValid(Message1 message1) =>
            PublicKeyAlgorithmIndicator.Exists((byte) GetPublicKeyAlgorithmIndicator(message1));

        #endregion

        #region Validation Main

        #endregion

        #region 6.3 Step 1

        private static void ValidateKeyLength(CaPublicKeyCertificate caPublicKeyCertificate, IssuerPublicKeyCertificate issuerPublicKey)
        {
            if (caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() != issuerPublicKey.GetEncipherment().Length)
            {
                throw new
                    CryptographicAuthenticationMethodFailedException($"Authentication failed because the {nameof(ValidateKeyLength)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
            }
        }

        #endregion

        #region 6.3 Step 2

        private DecodedSignature RecoverSignedIssuerPublicKey(
            CaPublicKeyCertificate caPublicKey, IssuerPublicKeyCertificate issuerPublicKey) =>
            _SignatureService.Decrypt(issuerPublicKey.GetEncipherment(), caPublicKey);

        #endregion

        #region 6.3 Step 4

        /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
        private void ValidateCertificateFormat(DecodedSignature decodedSignature)
        {
            if (decodedSignature.GetMessage1()[0] != CertificateSources.Issuer)
            {
                throw new
                    CryptographicAuthenticationMethodFailedException($"Authentication failed because the {nameof(ValidateCertificateFormat)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
            }
        }

        #endregion

        #region 6.3 Step 5

        private static byte[] GetConcatenatedValuesForHash(
            CaPublicKeyCertificate caPublicKeyCertificate, DecodedSignature decodedSignature, IssuerPublicKeyRemainder publicKeyRemainder,
            PublicKeyExponent publicKeyExponent)
        {
            byte issuerPublicKeyLength = DecodedIssuerPublicKeyCertificate.GetIssuerPublicKeyLength(decodedSignature.GetMessage1());
            byte issuerExponentLength = GetIssuerPublicKeyExponentLength(decodedSignature.GetMessage1());

            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(14 + issuerPublicKeyLength + issuerExponentLength);
            Span<byte> buffer = spanOwner.Span;

            decodedSignature.GetMessage1()[2..(2 + 14)].ToArray().AsSpan().CopyTo(buffer);
            DecodedIssuerPublicKeyCertificate.GetPublicKeyModulus(caPublicKeyCertificate, decodedSignature, publicKeyRemainder)
                .AsByteArray().AsSpan().CopyTo(buffer[17..]);
            publicKeyExponent.Encode().AsSpan().CopyTo(buffer[^publicKeyExponent.GetByteCount()..]);

            return buffer.ToArray();
        }

        #endregion

        #region 6.3 Step 6

        // This step is encapsulated in the DecodedIssuerPublicKeyCertificate.GetHashAlgorithmIndicator(decodedSignature.GetMessage1());

        #endregion

        #region 6.3 Step 7

        /// <summary>
        ///     This method includes validation from previous states regarding the validity of the deciphered signature from the
        ///     <see cref="IccPublicKeyCertificate" />
        /// </summary>
        /// <remarks>EMV Book 2 Section 6.3 Step 2, 3, 5 - 7 </remarks>
        /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
        private void ValidateHashedResult(
            HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> concatenatedValues, DecodedSignature decodedSignature)
        {
            if (!_SignatureService.IsSignatureValid(hashAlgorithmIndicator, concatenatedValues, decodedSignature))
            {
                throw new
                    CryptographicAuthenticationMethodFailedException($"Authentication failed because the {nameof(ValidateHashedResult)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
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
                    new(_CompressedNumericCodec.DecodeToUInt16(decodedSignature.GetMessage1()[new Range(1, 5)]));

                ApplicationPan pan = database.Get<ApplicationPan>(ApplicationPan.Tag);

                if (!pan.IsIssuerIdentifierMatching(issuerIdentificationNumber))
                {
                    throw new
                        CryptographicAuthenticationMethodFailedException($"Authentication failed because the {nameof(ValidateIssuerIdentifier)} constraint was invalid while trying to recover the signed {nameof(IssuerPublicKeyCertificate)}");
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

        /// <exception cref="Play.Codecs.Exceptions.CodecParsingException"></exception>
        private static void ValidateExpiryDate(Message1 message1)
        {
            DateTime today = DateTime.UtcNow;
            DateTimeUtc expiryDate = DecodedIssuerPublicKeyCertificate.GetCertificateExpirationDate(message1).AsDateTimeUtc();

            var today = ShortDateValue.Today
                <= new DateTime(expiryDate.Year(), expiryDate.Month(), DateTime.DaysInMonth(expiryDate.Year(), expiryDate.Month()));
        }

        #endregion

        #endregion

        /// <remarks>
        ///     Book 3 Section 5.3
        /// </remarks>
        /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
        public DecodedIssuerPublicKeyCertificate Create(
            ITlvReaderAndWriter database, CaPublicKeyCertificate caPublicKey, IssuerPublicKeyCertificate issuerPublicKey,
            IssuerPublicKeyExponent issuerExponent, IssuerPublicKeyRemainder issuerRemainder)
        {
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
            HashAlgorithmIndicator hashAlgorithmIndicator =
                DecodedIssuerPublicKeyCertificate.GetHashAlgorithmIndicator(decodedSignature.GetMessage1());

            // Step 7
            ValidateHashedResult(hashAlgorithmIndicator, concatenatedValues, decodedSignature);

            // Step 8
            ValidateIssuerIdentifier(database, decodedSignature);

            if (!IsValid(_SignatureService, caPublicKey, decodedSignature, issuerExponent.AsPublicKeyExponent(),
                         issuerRemainder.AsPublicKeyRemainder(), issuerPublicKey.GetEncipherment().Length))
                throw new CryptographicAuthenticationMethodFailedException("");

            UpdateDatabase(database, decodedSignature);

            return DecodedIssuerPublicKeyCertificate.Create(caPublicKey, issuerRemainder, issuerExponent, decodedSignature);
        }

        #endregion

        private static bool IsValid(
            ApplicationPan pan, IssuerIdentificationNumber iin, SignatureService signatureService,
            CaPublicKeyCertificate caPublicKeyCertificate, DecodedSignature decodedSignature, PublicKeyExponent publicKeyExponent,
            PublicKeyRemainder publicKeyRemainder, int enciphermentLength)
        {
            Message1 message1 = decodedSignature.GetMessage1();

            // Step 1
            if (caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() != enciphermentLength)
                return false;

            // Step 11 (placed before other steps because we're using this indicator to validate the signature)
            if (!IsIssuerPublicKeyAlgorithmIndicatorValid(message1))
                return false;

            // Step 2, 3, 5, 6, 7
            if (!
                signatureService
                    .IsSignatureValid(DecodedIssuerPublicKeyCertificate.GetHashAlgorithmIndicator(decodedSignature.GetMessage1()),
                                      GetConcatenatedValuesForHash(caPublicKeyCertificate, decodedSignature.GetMessage1(),
                                                                   publicKeyRemainder, publicKeyExponent), decodedSignature))
                return false;

            // Step 4
            if (IsCertificateFormatValid(message1))
                return false;

            // Step 8
            if (ValidateIssuerIdentifier(pan, iin))
                throw new CryptographicAuthenticationMethodFailedException($"");

            // Step 9
            if (!ValidateExpiryDate(message1))
                return false;

            //// Step 10
            //if(_CertificateAuthorityDatabase.IsRevoked(_TlvDatabase.Get(CertificateAuthorityPublicKeyIndex.Tag), GetCertificateSerialNumber)

            return true;
        }
    }
}