using ___TEMP.Play.Emv.Security.Certificates.Chip;
using ___TEMP.Play.Emv.Security.Certificates.Issuer;
using ___TEMP.Play.Emv.Security.Encryption.Signing;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs.Strings;
using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Globalization.Time;

namespace ___TEMP.Play.Emv.Security.Certificates;

internal partial class CertificateFactory
{
    internal static class Icc
    {
        #region Static Metadata

        private static readonly Numeric _NumericCodec = new();

        #endregion

        #region Instance Members

        /// <summary>
        ///     Create
        /// </summary>
        /// <param name="issuerPublicKeyCertificate"></param>
        /// <param name="publicKeyRemainder"></param>
        /// <param name="publicKeyExponent"></param>
        /// <param name="message1"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static DecodedIccPublicKeyCertificate Create(
            DecodedIssuerPublicKeyCertificate issuerPublicKeyCertificate,
            PublicKeyRemainder publicKeyRemainder,
            PublicKeyExponent publicKeyExponent,
            Message1 message1)
        {
            PrimaryAccountNumber primaryAccountNumber = GetPrimaryAccountNumber(message1);
            CertificateSerialNumber serialNumber = GetCertificateSerialNumber(message1);
            HashAlgorithmIndicator hashAlgorithm = GetHashAlgorithmIndicator(message1);
            PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator = GetPublicKeyAlgorithmIndicator(message1);
            DateRange validityPeriod = new(ShortDateValue.MinValue, GetCertificateExpirationDate(message1));

            PublicKeyModulus publicKeyModulus = GetPublicKeyModulus(issuerPublicKeyCertificate, message1, publicKeyRemainder);
            PublicKeyInfo publicKeyInfo = new(publicKeyModulus, publicKeyExponent);

            return new DecodedIccPublicKeyCertificate(primaryAccountNumber, validityPeriod, serialNumber, hashAlgorithm,
                                                      publicKeyAlgorithmIndicator, publicKeyInfo);
        }

        private static ShortDateValue GetCertificateExpirationDate(Message1 message1)
        {
            return new(_NumericCodec.GetUInt16(message1[new Range(11, 13)]));
        }

        private static CertificateSerialNumber GetCertificateSerialNumber(Message1 message1)
        {
            return new(message1[new Range(13, 16)]);
        }

        // TODO: The remainder and exponent will be coming from the TLV Database. No need to pass those with the
        // TODO: command. You can pass the ITlvDatabase and query when it's relevant

        /// <exception cref="InvalidOperationException"></exception>
        private static byte[] GetConcatenatedValuesForHash(
            DecodedIssuerPublicKeyCertificate issuerPublicKeyCertificate,
            Message1 message1,
            PublicKeyRemainder publicKeyRemainder,
            PublicKeyExponent publicKeyExponent,
            StaticDataToBeAuthenticated staticDataToBeAuthenticated)
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(message1.GetByteCount()
                                                                       + publicKeyRemainder.GetByteCount()
                                                                       + publicKeyExponent.GetByteCount()
                                                                       + staticDataToBeAuthenticated.GetByteCount());

            Span<byte> buffer = spanOwner.Span;

            message1[2..(2 + 14)].ToArray().AsSpan().CopyTo(buffer);

            GetPublicKeyModulus(issuerPublicKeyCertificate, message1, publicKeyRemainder).AsByteArray().AsSpan().CopyTo(buffer[17..]);
            publicKeyExponent.AsByteArray().AsSpan().CopyTo(buffer[^publicKeyExponent.GetByteCount()..]);

            return buffer.ToArray();
        }

        private static HashAlgorithmIndicator GetHashAlgorithmIndicator(Message1 message1)
        {
            return HashAlgorithmIndicator.Get(message1[16]);
        }

        private static byte GetIccPublicKeyLength(Message1 message1)
        {
            return message1[18];
        }

        private static Range GetLeftmostIssuerPublicKeyRange(DecodedIssuerPublicKeyCertificate issuerPublicKeyCertificate)
        {
            return new(20, issuerPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() - 42);
        }

        private static PrimaryAccountNumber GetPrimaryAccountNumber(Message1 message1)
        {
            return new(message1[new Range(1, 11)]);
        }

        private static PublicKeyAlgorithmIndicator GetPublicKeyAlgorithmIndicator(Message1 message1)
        {
            return PublicKeyAlgorithmIndicator.Get(message1[17]);
        }

        /// <exception cref="InvalidOperationException"></exception>
        private static PublicKeyModulus GetPublicKeyModulus(
            DecodedIssuerPublicKeyCertificate issuerPublicKeyCertificate,
            Message1 message1,
            PublicKeyRemainder publicKeyRemainder)
        {
            if (IsIssuerPublicKeySplit(issuerPublicKeyCertificate, message1))
            {
                if (publicKeyRemainder.IsEmpty())
                    throw new InvalidOperationException($"A {nameof(PublicKeyRemainder)} was expected but could not be retrieved");

                Span<byte> modulusBuffer = stackalloc byte[GetIccPublicKeyLength(message1)];
                message1[GetLeftmostIssuerPublicKeyRange(issuerPublicKeyCertificate)].CopyTo(modulusBuffer);
                publicKeyRemainder.AsSpan().CopyTo(modulusBuffer[^publicKeyRemainder.GetByteCount()..]);

                return new PublicKeyModulus(modulusBuffer.ToArray());
            }

            return new PublicKeyModulus(message1[GetLeftmostIssuerPublicKeyRange(issuerPublicKeyCertificate)]);
        }

        private static bool IsCertificateFormatValid(Message1 message1)
        {
            return message1[0] == CertificateFormat.Icc;
        }

        private static bool IsExpiryDateValid(Message1 message1)
        {
            DateTime today = DateTime.UtcNow;
            DateTime expiryDate = GetCertificateExpirationDate(message1).AsDateTimeUtc();

            return new DateTime(today.Year, today.Month, today.Day)
                <= new DateTime(expiryDate.Year, expiryDate.Month, DateTime.DaysInMonth(expiryDate.Year, expiryDate.Month));
        }

        private static bool IsIssuerPublicKeySplit(DecodedIssuerPublicKeyCertificate issuerPublicKeyCertificate, Message1 message1)
        {
            return GetIccPublicKeyLength(message1) > issuerPublicKeyCertificate.GetPublicKeyModulus().GetByteCount();
        }

        /// <summary>
        ///     IsValid
        /// </summary>
        /// <param name="signatureService"></param>
        /// <param name="issuerPublicKeyCertificate"></param>
        /// <param name="decodedSignature"></param>
        /// <param name="encipheredCertificate"></param>
        /// <param name="publicKeyExponent"></param>
        /// <param name="publicKeyRemainder"></param>
        /// <param name="staticDataToBeAuthenticated"></param>
        /// <param name="primaryAccountNumber"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static bool IsValid(
            ISignatureService signatureService,
            DecodedIssuerPublicKeyCertificate issuerPublicKeyCertificate,
            DecodedSignature decodedSignature,
            IccPublicKeyCertificate encipheredCertificate,
            PublicKeyExponent publicKeyExponent,
            PublicKeyRemainder publicKeyRemainder,
            StaticDataToBeAuthenticated staticDataToBeAuthenticated,
            PrimaryAccountNumber primaryAccountNumber)
        {
            // Step 1
            if (issuerPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() != encipheredCertificate.GetByteCount())
                return false;

            // Step 10 (placed before other steps because we're using this indicator to validate the signature)
            if (GetHashAlgorithmIndicator(decodedSignature.GetMessage1()) != HashAlgorithmIndicator.NotAvailable)
                return false;

            // Step 2.b, 3, 5, 6, 7, 
            if (!signatureService.IsSignatureValid(GetHashAlgorithmIndicator(decodedSignature.GetMessage1()),
                                                   GetConcatenatedValuesForHash(issuerPublicKeyCertificate, decodedSignature.GetMessage1(),
                                                                                publicKeyRemainder, publicKeyExponent,
                                                                                staticDataToBeAuthenticated), decodedSignature))
                return false;

            // Step 4
            if (IsCertificateFormatValid(decodedSignature.GetMessage1()))
                return false;

            // Step 5.b
            if (!staticDataToBeAuthenticated.IsValid())
                return false;

            // Step 8
            if (primaryAccountNumber != GetPrimaryAccountNumber(decodedSignature.GetMessage1()))
                return false;

            // Step 9
            if (IsExpiryDateValid(decodedSignature.GetMessage1()))
                return false;

            return true;
        }

        /// <remarks>
        ///     Book 3 Section 5.3
        /// </remarks>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool TryCreate(
            ISignatureService signatureService,
            StaticDataToBeAuthenticated staticDataToBeAuthenticated,
            PrimaryAccountNumber primaryAccountNumber,
            DecodedIssuerPublicKeyCertificate publicKeyCertificate,
            IccPublicKeyCertificate encipheredCertificate,
            IccPublicKeyExponent encipheredPublicKeyExponent,
            IccPublicKeyRemainder enciphermentPublicKeyRemainder,
            out DecodedIccPublicKeyCertificate? result)
        {
            try
            {
                ReadOnlySpan<byte> encipherment = encipheredCertificate.GetEncipherment();
                DecodedSignature decodedSignature = signatureService.Decrypt(encipherment, publicKeyCertificate);

                if (!IsValid(signatureService, publicKeyCertificate, decodedSignature, encipheredCertificate,
                             encipheredPublicKeyExponent.AsPublicKeyExponent(), enciphermentPublicKeyRemainder.AsPublicKeyRemainder(),
                             staticDataToBeAuthenticated, primaryAccountNumber))
                {
                    result = null;

                    return false;
                }

                result = Create(publicKeyCertificate, enciphermentPublicKeyRemainder.AsPublicKeyRemainder(),
                                encipheredPublicKeyExponent.AsPublicKeyExponent(), decodedSignature.GetMessage1());

                return true;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // TODO: LOG
                result = null;

                return false;
            }
            catch (Exception e)
            {
                // TODO: LOG
                result = null;

                return false;
            }
        }

        #endregion
    }
}