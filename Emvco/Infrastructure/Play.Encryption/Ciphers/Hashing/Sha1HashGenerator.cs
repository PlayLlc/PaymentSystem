using System.Security.Cryptography;

namespace Play.Encryption.Hashing;

internal class Sha1HashGenerator : IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText) => new(SHA1.HashData(clearText));
    public HashAlgorithmIndicator GetHashAlgorithmIndicator() => HashAlgorithmIndicator.Sha1;

    #endregion
}