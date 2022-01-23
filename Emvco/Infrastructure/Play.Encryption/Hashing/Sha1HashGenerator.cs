using System.Security.Cryptography;

using Play.Emv.DataElements.CertificateAuthority;

namespace Play.Encryption.Encryption.Hashing;

internal class Sha1HashGenerator : IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText) => new Hash(SHA1.HashData(clearText));
    public HashAlgorithmIndicator GetHashAlgorithmIndicator() => HashAlgorithmIndicator.Sha1;

    #endregion
}