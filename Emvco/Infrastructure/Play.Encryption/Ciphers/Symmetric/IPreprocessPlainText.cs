namespace Play.Encryption.Encryption.Ciphers.Symmetric;

/// <summary>
///     Allows preprocessing of the plain text message before it is enciphered by a block cipher
/// </summary>
public interface IPreprocessPlainText
{
    #region Instance Members

    public byte[] Preprocess(ReadOnlySpan<byte> plainText);

    #endregion
}