using System.Security.Cryptography;

namespace Play.Encryption.Ciphers.Hashing;

internal class Sha1HashGenerator : IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText) => new(SHA1.HashData(clearText));
    public HashAlgorithmIndicators GetHashAlgorithmIndicator() => HashAlgorithmIndicators.Sha1;

    #endregion
}