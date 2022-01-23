using Play.Emv.DataElements.CertificateAuthority;

namespace Play.Encryption.Encryption.Hashing;

internal interface IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText);
    public HashAlgorithmIndicator GetHashAlgorithmIndicator();

    #endregion
}