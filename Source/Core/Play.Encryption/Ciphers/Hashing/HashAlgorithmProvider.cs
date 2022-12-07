using System.Collections.Immutable;

namespace Play.Encryption.Ciphers.Hashing;

public class HashAlgorithmProvider
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<HashAlgorithmIndicators, IHashGenerator> _HashAlgorithmMap;

    #endregion

    #region Constructor

    public HashAlgorithmProvider()
    {
        _HashAlgorithmMap = CreateHashAlgorithmMap();
    }

    #endregion

    #region Instance Members

    private ImmutableSortedDictionary<HashAlgorithmIndicators, IHashGenerator> CreateHashAlgorithmMap() =>
        new Dictionary<HashAlgorithmIndicators, IHashGenerator>
        {
            {HashAlgorithmIndicators.Sha1, new Sha1HashGenerator()}, {HashAlgorithmIndicators.Sha1, new Sha1HashGenerator()}
        }.ToImmutableSortedDictionary();

    public Hash Generate(ReadOnlySpan<byte> clearText, HashAlgorithmIndicators hashAlgorithmIndicators)
    {
        if (!_HashAlgorithmMap.TryGetValue(hashAlgorithmIndicators, out IHashGenerator? hashGenerator))
        {
            throw new ArgumentOutOfRangeException(nameof(hashAlgorithmIndicators),
                $"There was no {nameof(IHashGenerator)} available for the argument {nameof(hashAlgorithmIndicators)} with value {hashAlgorithmIndicators}");
        }

        return hashGenerator.Generate(clearText);
    }

    #endregion
}