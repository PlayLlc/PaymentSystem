using System.Security.Cryptography;

using Play.Emv.DataElements.CertificateAuthority;

namespace Play.Encryption.Encryption.Hashing;

internal class Sha1HashGenerator : IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText)
    {
        return new Hash(SHA1.HashData(clearText));
    }

    public HashAlgorithmIndicator GetHashAlgorithmIndicator()
    {
        return HashAlgorithmIndicator.Sha1;
    }

    #endregion
}