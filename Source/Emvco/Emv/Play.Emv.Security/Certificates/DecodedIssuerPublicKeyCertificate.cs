﻿using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Security.Exceptions;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates;

internal class DecodedIssuerPublicKeyCertificate : PublicKeyCertificate
{
    #region Static Metadata

    private static readonly CertificateSources _CertificateSources = CertificateSources.Issuer;

    #endregion

    #region Constructor

    public DecodedIssuerPublicKeyCertificate(
        CertificateSerialNumber certificateSerialNumber, HashAlgorithmIndicator hashAlgorithmIndicator, PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator,
        DateRange validityPeriod, PublicKeyInfo publicKeyInfo) : base(certificateSerialNumber, hashAlgorithmIndicator, publicKeyAlgorithmIndicator,
        validityPeriod, publicKeyInfo)
    { }

    #endregion

    #region Instance Members

    internal CertificateSources GetCertificateFormat() => _CertificateSources;

    /// <exception cref="CodecParsingException"></exception>
    internal static DecodedIssuerPublicKeyCertificate Create(
        CaPublicKeyCertificate caPublicKeyCertificate, IssuerPublicKeyRemainder issuerRemainder, IssuerPublicKeyExponent issuerExponent,
        DecodedSignature decodedSignature, HashAlgorithmIndicator hashAlgorithm)
    {
        CertificateSerialNumber serialNumber = GetCertificateSerialNumber(decodedSignature.GetMessage1());
        PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator = GetPublicKeyAlgorithmIndicator(decodedSignature.GetMessage1());
        DateRange validityPeriod = new(ShortDate.Min, GetCertificateExpirationDate(decodedSignature.GetMessage1()));

        PublicKeyModulus publicKeyModulus = GetPublicKeyModulus(caPublicKeyCertificate, decodedSignature, issuerRemainder);
        PublicKeyInfo publicKeyInfo = new(publicKeyModulus, issuerExponent.AsPublicKeyExponent());

        return new DecodedIssuerPublicKeyCertificate(serialNumber, hashAlgorithm, publicKeyAlgorithmIndicator, validityPeriod, publicKeyInfo);
    }

    internal static HashAlgorithmIndicator GetHashAlgorithmIndicator(Message1 message1) => HashAlgorithmIndicator.Get(message1[11]);
    private static CertificateSerialNumber GetCertificateSerialNumber(Message1 message1) => new(message1[new Range(7, 10)]);

    internal static PublicKeyAlgorithmIndicator GetPublicKeyAlgorithmIndicator(Message1 message1)
    {
        if (!PublicKeyAlgorithmIndicator.Empty.TryGet(message1[12], out EnumObject<byte>? result))
        {
            throw new TerminalDataException(
                $"The {nameof(DecodedIssuerPublicKeyCertificate)} could not execute: [{nameof(GetPublicKeyAlgorithmIndicator)}] because the {nameof(PublicKeyAlgorithmIndicator)} could not be resolved");
        }

        return (PublicKeyAlgorithmIndicator) result!;
    }

    /// <exception cref="CodecParsingException"></exception>
    internal static ShortDate GetCertificateExpirationDate(Message1 message1) => new(PlayCodec.NumericCodec.DecodeToUInt16(message1[new Range(5, 7)]));

    private static bool IsIssuerPublicKeySplit(CaPublicKeyCertificate caPublicKeyCertificate, Message1 message1) =>
        GetIssuerPublicKeyLength(message1) > caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount();

    internal static byte GetIssuerPublicKeyLength(Message1 message1) => message1[13];
    internal static byte GetIssuerPublicKeyExponentLength(Message1 message1) => message1[14];

    private static Range GetLeftmostIssuerPublicKeyRange(CaPublicKeyCertificate caPublicKeyCertificate) =>
        new(15, caPublicKeyCertificate.GetPublicKeyModulus().GetByteCount() - 36);

    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    internal static PublicKeyModulus GetPublicKeyModulus(
        CaPublicKeyCertificate caPublicKeyCertificate, DecodedSignature decodedSignature, IssuerPublicKeyRemainder publicKeyRemainder)
    {
        PublicKeyRemainder remainder = publicKeyRemainder;

        if (IsIssuerPublicKeySplit(caPublicKeyCertificate, decodedSignature.GetMessage1()))
        {
            if (remainder.IsEmpty())
                throw new CryptographicAuthenticationMethodFailedException($"A {nameof(PublicKeyRemainder)} was expected but could not be retrieved");

            Span<byte> modulusBuffer = stackalloc byte[GetIssuerPublicKeyLength(decodedSignature.GetMessage1())];
            decodedSignature.GetMessage1()[GetLeftmostIssuerPublicKeyRange(caPublicKeyCertificate)].CopyTo(modulusBuffer);
            remainder.AsSpan().CopyTo(modulusBuffer[^remainder.GetByteCount()..]);

            return new PublicKeyModulus(modulusBuffer.ToArray());
        }

        return new PublicKeyModulus(decodedSignature.GetMessage1()[GetLeftmostIssuerPublicKeyRange(caPublicKeyCertificate)]);
    }

    #endregion
}