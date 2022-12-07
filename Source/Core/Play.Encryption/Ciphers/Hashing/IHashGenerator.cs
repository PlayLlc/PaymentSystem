namespace Play.Encryption.Ciphers.Hashing;

internal interface IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText);
    public HashAlgorithmIndicators GetHashAlgorithmIndicator();

    #endregion
}