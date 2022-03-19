using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Emv.Ber.DataElements;
using Play.Emv.Security.Certificates.Issuer;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates;

internal partial class CertificateFactory
{
    internal static class Issuer
    {
        #region Static Metadata

        private static readonly CompressedNumericCodec _CompressedNumericCodec = new();
        private static readonly NumericCodec _NumericCodec = new();
        private const byte _RecoveredDataHeader = 0x6A;
        private const byte _RecoveredDataTrailer = 0xBC;

        #endregion

        #region Instance Members

        /// <summary>
        ///     Create
        /// </summary>
        /// <param name="caPublicKeyCertificate"></param>
        /// <param name="publicKeyRemainder"></param>
        /// <param name="publicKeyExponent"></param>
        /// <param name="message1"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static DecodedIssuerPublicKeyCertificate Create(
            CaPublicKeyCertificate caPublicKeyCertificate,
            PublicKeyRemainder publicKeyRemainder,
            PublicKeyExponent publicKeyExponent,
            Message1 message1)
        {
            IssuerIdentificationNumber issuerIdentificationNumber = GetIssuerIdentificationNumber(message1);
            CertificateSerialNumber serialNumber = GetCertificateSerialNumber(message1);
            HashAlgorithmIndicator hashAlgorithm = GetHashAlgorithmIndicator(message1);
            PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator = GetPublicKeyAlgorithmIndicator(message1);
            DateRange validityPeriod = new(ShortDateValue.MinValue, GetCertificateExpirationDate(message1));

            PublicKeyModulus publicKeyModulus = GetPublicKeyModulus(caPublicKeyCertificate, message1, publicKeyRemainder);
            PublicKeyInfo publicKeyInfo = new(publicKeyModulus, publicKeyExponent);

            return new DecodedIssuerPublicKeyCertificate(issuerIdentificationNumber, serialNumber, hashAlgorithm,
                                                         publicKeyAlgorithmIndicator, validityPeriod, publicKeyInfo);
        }

        private static ShortDateValue GetCertificateExpirationDate(Message1 message1) =>
            new(_NumericCodec.DecodeToUInt16(message1[new Range(5, 7)]));

        private static CertificateSerialNumber GetCertificateSerialNumber(Message1 message1) => new(message1[new Range(7, 10)]);

        // TODO: The remainder and exponent will be coming from the TLV Database. No need to pass those with the
        // TODO: command. You can pass the ITlvDatabase and query when it's relevant

        /// <exception cref="InvalidOperationException"></exception>
        private static byte[] GetConcatenatedValuesForHash(
            CaPublicKeyCertificate caPublicKeyCertificate,
            Message1 message1,
            PublicKeyRemainder publicKeyRemainder,
            PublicKeyExponent publicKeyExponent)
        {
            byte issuerPublicKeyLength = GetIssuerPublicKeyLength(message1);
            byte issuerExponentLength = GetIssuerPublicKeyExponentLength(message1);

            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(14 + issuerPublicKeyLength + issuerExponentLength);
            Span<byte> buffer = spanOwner.Span;

            message1[2..(2 + 14)].ToArray().AsSpan().CopyTo(buffer);
            GetPublicKeyModulus(caPublicKeyCertificate, message1, publicKeyRemainder).AsByteArray().AsSpan().CopyTo(buffer[17..]);
            publicKeyExponent.Encode().AsSpan().CopyTo(buffer[^publicKeyExponent.GetByteCount()..]);

            return buffer.ToArray();
        }

        private static HashAlgorithmIndicator GetHashAlgorithmIndicator(Message1 message1) => HashAlgorithmIndicator.Get(message1[11]);

        private static IssuerIdentificationNumber GetIssuerIdentificationNumber(Message1 message1) =>
            new(_CompressedNumericCodec.DecodeToUInt16(message1[new Range(1, 5)]));

        private static byte GetIssuerPublicKeyExponentLength(Message1 message1) => message1[14];
        private static byte GetIssuerPublicKeyLength(Message1 message1) => message1[13];

        private static Range GetLeftmostIssuerPublicKeyRange(CaPublicKeyCertificate caPublicKeyCertificate) =>
            new(15, caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() - 36);

        private static PublicKeyAlgorithmIndicator GetPublicKeyAlgorithmIndicator(Message1 message1) =>
            PublicKeyAlgorithmIndicator.Get(message1[12]);

        /// <exception cref="InvalidOperationException"></exception>
        private static PublicKeyModulus GetPublicKeyModulus(
            CaPublicKeyCertificate caPublicKeyCertificate,
            Message1 message1,
            PublicKeyRemainder publicKeyRemainder)
        {
            if (IsIssuerPublicKeySplit(caPublicKeyCertificate, message1))
            {
                if (publicKeyRemainder.IsEmpty())
                    throw new InvalidOperationException($"A {nameof(PublicKeyRemainder)} was expected but could not be retrieved");

                Span<byte> modulusBuffer = stackalloc byte[GetIssuerPublicKeyLength(message1)];
                message1[GetLeftmostIssuerPublicKeyRange(caPublicKeyCertificate)].CopyTo(modulusBuffer);
                publicKeyRemainder.AsSpan().CopyTo(modulusBuffer[^publicKeyRemainder.GetByteCount()..]);

                return new PublicKeyModulus(modulusBuffer.ToArray());
            }

            return new PublicKeyModulus(message1[GetLeftmostIssuerPublicKeyRange(caPublicKeyCertificate)]);
        }

        private static bool IsCertificateFormatValid(Message1 message1) => message1[0] == CertificateFormat.Issuer;

        private static bool IsExpiryDateValid(Message1 message1)
        {
            DateTime today = DateTime.UtcNow;
            DateTime expiryDate = GetCertificateExpirationDate(message1).AsDateTimeUtc();

            return new DateTime(today.Year, today.Month, today.Day)
                <= new DateTime(expiryDate.Year, expiryDate.Month, DateTime.DaysInMonth(expiryDate.Year, expiryDate.Month));
        }

        private static bool IsIssuerPublicKeyAlgorithmIndicatorValid(Message1 message1) =>
            PublicKeyAlgorithmIndicator.Exists((byte) GetPublicKeyAlgorithmIndicator(message1));

        private static bool IsIssuerPublicKeySplit(CaPublicKeyCertificate caPublicKeyCertificate, Message1 message1) =>
            GetIssuerPublicKeyLength(message1) > caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount();

        /// <summary>
        ///     IsValid
        /// </summary>
        /// <param name="signatureService"></param>
        /// <param name="caPublicKeyCertificate"></param>
        /// <param name="decodedSignature"></param>
        /// <param name="publicKeyExponent"></param>
        /// <param name="publicKeyRemainder"></param>
        /// <param name="enciphermentLength"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static bool IsValid(
            SignatureService signatureService,
            CaPublicKeyCertificate caPublicKeyCertificate,
            DecodedSignature decodedSignature,
            PublicKeyExponent publicKeyExponent,
            PublicKeyRemainder publicKeyRemainder,
            int enciphermentLength)
        {
            Message1 message1 = decodedSignature.GetMessage1();

            // Step 1
            if (caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() != enciphermentLength)
                return false;

            // Step 11 (placed before other steps because we're using this indicator to validate the signature)
            if (!IsIssuerPublicKeyAlgorithmIndicatorValid(message1))
                return false;

            // Step 2, 3, 5, 6, 7
            if (!signatureService.IsSignatureValid(GetHashAlgorithmIndicator(decodedSignature.GetMessage1()),
                                                   GetConcatenatedValuesForHash(caPublicKeyCertificate, decodedSignature.GetMessage1(),
                                                                                publicKeyRemainder, publicKeyExponent), decodedSignature))
                return false;

            // Step 4
            if (IsCertificateFormatValid(message1))
                return false;

            // Step 8
            // verify IssuerIdentificationNumber == IIN

            // Step 9
            if (!IsExpiryDateValid(message1))
                return false;

            //// Step 10
            //if(_CertificateAuthorityDatabase.IsRevoked(_TlvDatabase.Get(CertificateAuthorityPublicKeyIndex.Tag), GetCertificateSerialNumber)

            return true;
        }

        /// <remarks>
        ///     Book 3 Section 5.3
        /// </remarks>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool TryCreate(
            SignatureService signatureService,
            CaPublicKeyCertificate publicKeyCertificate,
            IssuerPublicKeyCertificate encipheredCertificate,
            IssuerPublicKeyExponent encipheredPublicKeyExponent,
            IssuerPublicKeyRemainder enciphermentPublicKeyRemainder,
            out DecodedIssuerPublicKeyCertificate? result)
        {
            try
            {
                ReadOnlySpan<byte> encipherment = encipheredCertificate.GetEncipherment();
                DecodedSignature decodedSignature = signatureService.Decrypt(encipherment, publicKeyCertificate);

                if (!IsValid(signatureService, publicKeyCertificate, decodedSignature, encipheredPublicKeyExponent.AsPublicKeyExponent(),
                             enciphermentPublicKeyRemainder.AsPublicKeyRemainder(), encipherment.Length))
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
                // LOG
                result = null;

                return false;
            }
            catch (Exception e)
            {
                // LOG
                result = null;

                return false;
            }
        }

        #endregion
    }
}