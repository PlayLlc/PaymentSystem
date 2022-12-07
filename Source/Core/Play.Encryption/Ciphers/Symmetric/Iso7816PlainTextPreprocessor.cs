namespace Play.Encryption.Ciphers.Symmetric;

public class Iso7816PlainTextPreprocessor : IPreprocessPlainText
{
    #region Instance Values

    private readonly IFormatPlainText _Formatter;

    #endregion

    #region Constructor

    public Iso7816PlainTextPreprocessor(BlockSize blockSize)
    {
        _Formatter = new Iso7816PaddingFormatter(blockSize);
    }

    #endregion

    #region Instance Members

    public byte[] Preprocess(ReadOnlySpan<byte> plainText) => _Formatter.Format(plainText);

    #endregion
}