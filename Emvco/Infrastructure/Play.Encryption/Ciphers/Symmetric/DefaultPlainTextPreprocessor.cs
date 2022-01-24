namespace Play.Encryption.Ciphers.Symmetric;

public class DefaultPlainTextPreprocessor : IPreprocessPlainText
{
    #region Instance Members

    public byte[] Preprocess(ReadOnlySpan<byte> plainText) => plainText.ToArray();

    #endregion
}