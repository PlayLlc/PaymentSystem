namespace Play.Encryption.Encryption;

public interface IFormatPlainText
{
    #region Instance Members

    public byte[] Format(ReadOnlySpan<byte> plainText);

    #endregion
}