using System.Collections.Immutable;

using Play.Encryption.Certificates;

namespace Play.Encryption.Ciphers.Asymmetric;

internal class AsymmetricAlgorithmProvider
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<PublicKeyAlgorithmIndicator, IAsymmetricCodec> _AsymmetricAlgorithmMap;

    #endregion

    #region Constructor

    public AsymmetricAlgorithmProvider()
    {
        _AsymmetricAlgorithmMap = CreateAsymmetricAlgorithmMap();
    }

    #endregion

    #region Instance Members

    private ImmutableSortedDictionary<PublicKeyAlgorithmIndicator, IAsymmetricCodec> CreateAsymmetricAlgorithmMap() =>
        new Dictionary<PublicKeyAlgorithmIndicator, IAsymmetricCodec> {{PublicKeyAlgorithmIndicator.Rsa, new RsaCodec()}}
            .ToImmutableSortedDictionary();

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public byte[] Decrypt(ReadOnlySpan<byte> cipherText, PublicKeyCertificate publicKeyCertificate)
    {
        if (!_AsymmetricAlgorithmMap.TryGetValue(publicKeyCertificate.GetPublicKeyAlgorithmIndicator(),
                                                 out IAsymmetricCodec? asymmetricCodec))
        {
            throw new ArgumentOutOfRangeException(nameof(publicKeyCertificate),
                                                  $"There was no {nameof(IAsymmetricCodec)} available for the argument {nameof(publicKeyCertificate)} with the {nameof(PublicKeyAlgorithmIndicator)} value of {publicKeyCertificate.GetPublicKeyAlgorithmIndicator()}");
        }

        return asymmetricCodec.Sign(cipherText, publicKeyCertificate.GetPublicKeyInfo());
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public byte[] Sign(ReadOnlySpan<byte> clearText, PublicKeyCertificate publicKeyCertificate)
    {
        if (!_AsymmetricAlgorithmMap.TryGetValue(publicKeyCertificate.GetPublicKeyAlgorithmIndicator(),
                                                 out IAsymmetricCodec? asymmetricCodec))
        {
            throw new ArgumentOutOfRangeException(nameof(publicKeyCertificate),
                                                  $"There was no {nameof(IAsymmetricCodec)} available for the argument {nameof(publicKeyCertificate)} with the {nameof(PublicKeyAlgorithmIndicator)} value of {publicKeyCertificate.GetPublicKeyAlgorithmIndicator()}");
        }

        return asymmetricCodec.Sign(clearText, publicKeyCertificate.GetPublicKeyInfo());
    }

    #endregion
}