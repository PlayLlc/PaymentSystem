using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Emv.Ber.DataElements;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Encryption.Signing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates;

internal class DecodedIccPublicKeyCertificate : PublicKeyCertificate
{
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

    #endregion
}