using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Asymmetric;
using Play.Encryption.Hashing;

namespace Play.Encryption.Signing;

public class SignatureService
{
    #region Instance Values

    private readonly AsymmetricAlgorithmProvider _AsymmetricAlgorithmProvider;
    private readonly HashAlgorithmProvider _HashAlgorithmProvider;

    #endregion

    #region Constructor

    public SignatureService()
    {
        _AsymmetricAlgorithmProvider = new AsymmetricAlgorithmProvider();
        _HashAlgorithmProvider = new HashAlgorithmProvider();
    }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    public DecodedSignature Decrypt(ReadOnlySpan<byte> signature, PublicKeyCertificate publicKeyCertificate)
    {
        byte[] decipheredSignature = _AsymmetricAlgorithmProvider.Decrypt(signature, publicKeyCertificate)
            ?? throw new InvalidOperationException($"The argument {nameof(signature)} returned a null value when deciphered");

        return new DecodedSignature(decipheredSignature);
    }

    private bool IsHashValid(HashAlgorithmIndicator hashAlgorithmIndicator, DecodedSignature decodedSignature, ReadOnlySpan<byte> message)
    {
        using SpanOwner<byte> spanOwner = new();
        Span<byte> decipheredHash = decodedSignature.GetHash();
        Span<byte> generatedHash = stackalloc byte[20];

        // TODO: This should be able to pass in PublicKeyAlgorithmIdentifier so the algorithm type
        // TODO: isn't hardcoded
        _HashAlgorithmProvider.Generate(message, hashAlgorithmIndicator);

        for (int i = 0; i < 20; i++)
        {
            if (decipheredHash[i] != generatedHash[i])
                return false;
        }

        return true;
    }

    private bool IsLeadingByteValid(DecodedSignature decodedSignature) =>
        decodedSignature.GetLeadingByte() == SignatureSpecifications.LeadingByte;

    private bool IsMessage1Valid(DecodedSignature decodedSignature, ReadOnlySpan<byte> message)
    {
        Message1 message1 = decodedSignature.GetMessage1();

        for (int i = 0; i < message1.GetByteCount(); i++)
        {
            if (message1[i] != message[i])
                return false;
        }

        return true;
    }

    public bool IsSignatureValid(HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> message, DecodedSignature signature) =>
        IsLeadingByteValid(signature)
        && IsMessage1Valid(signature, message)
        && IsHashValid(hashAlgorithmIndicator, signature, message)
        && IsTrailingByteValid(signature);

    private bool IsTrailingByteValid(DecodedSignature signature) => signature.GetTrailingByte() == SignatureSpecifications.TrailingByte;

    /// <remarks>
    ///     Book 2 Section A2.1.2 & B2.1 RSA Algorithm
    ///     An RSA algorithm is used to generate a reversible signature
    /// </remarks>
    public byte[] Sign(ReadOnlySpan<byte> message, PublicKeyCertificate publicKeyCertificate)
    {
        using SpanOwner<byte> spanOwner = new();
        Span<byte> leftmostMessage = spanOwner.Span;
        Span<byte> rightmostMessage = spanOwner.Span;
        Span<byte> concatenationBuffer = spanOwner.Span;

        message[..^22].CopyTo(leftmostMessage);
        message[^22..].CopyTo(rightmostMessage);

        concatenationBuffer[0] = SignatureSpecifications.LeadingByte;
        leftmostMessage.CopyTo(concatenationBuffer[1..]);
        _HashAlgorithmProvider.Generate(message, publicKeyCertificate.GetHashAlgorithmIndicator()).AsReadOnlySpan()
            .CopyTo(concatenationBuffer[(1 + leftmostMessage.Length)..]);
        concatenationBuffer[1 + leftmostMessage.Length + SignatureSpecifications.HashLength] = SignatureSpecifications.TrailingByte;

        return _AsymmetricAlgorithmProvider.Sign(concatenationBuffer.ToArray(), publicKeyCertificate);
    }

    #endregion
}