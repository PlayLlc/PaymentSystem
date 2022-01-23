namespace Play.Encryption.Encryption.Ciphers.Symmetric;

public class DefaultPlainTextPreprocessor : IPreprocessPlainText
{
    #region Instance Members

    public byte[] Preprocess(ReadOnlySpan<byte> plainText)
    {
        return plainText.ToArray();
    }

    #endregion
}