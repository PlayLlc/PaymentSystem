using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Encryption.Hashing;

internal interface IHashAlgorithmProvider
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText, HashAlgorithmIndicator hashAlgorithmIndicator);

    #endregion
}