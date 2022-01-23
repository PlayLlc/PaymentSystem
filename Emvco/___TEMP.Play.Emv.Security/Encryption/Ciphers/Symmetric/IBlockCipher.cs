namespace ___TEMP.Play.Emv.Security.Encryption.Ciphers.Symmetric;

public interface IBlockCipher
{
    #region Instance Members

    public BlockCipherAlgorithm GetAlgorithm();
    public BlockCipherMode GetCipherMode();
    public KeySize GetKeySize();
    public byte[] Sign(ReadOnlySpan<byte> message, ReadOnlySpan<byte> key);

    #endregion
}