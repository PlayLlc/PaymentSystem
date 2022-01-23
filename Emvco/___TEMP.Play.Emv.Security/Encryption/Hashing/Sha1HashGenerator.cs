using System.Security.Cryptography;

using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Encryption.Hashing;

internal class Sha1HashGenerator : IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText)
    {
        return new(SHA1.HashData(clearText));
    }

    public HashAlgorithmIndicator GetHashAlgorithmIndicator()
    {
        return HashAlgorithmIndicator.Sha1;
    }

    #endregion
}