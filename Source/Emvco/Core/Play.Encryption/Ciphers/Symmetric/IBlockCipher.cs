namespace Play.Encryption.Ciphers.Symmetric;

public interface IBlockCipher
{
    #region Instance Members

    public BlockCipherAlgorithm GetAlgorithm();
    public BlockCipherMode GetCipherMode();
    public KeySize GetKeySize();
    public byte[] Encrypt(ReadOnlySpan<byte> message, ReadOnlySpan<byte> key);

    public byte[] Decrypt(ReadOnlySpan<byte> encipherment, ReadOnlySpan<byte> key);

    public void SetInitializationVector(byte[] initializationVector);

    #endregion
}