using Play.Encryption.Ciphers.Symmetric;

namespace Play.Encryption;

public class Iso7816PaddingFormatter : IFormatPlainText
{
    #region Instance Values

    private readonly BlockSize _BlockSize;

    #endregion

    #region Constructor

    public Iso7816PaddingFormatter(BlockSize blockSize)
    {
        _BlockSize = blockSize;
    }

    #endregion

    #region Instance Members

    public byte[] Format(ReadOnlySpan<byte> plainText)
    {
        if (IsPaddingValid(plainText))
            return plainText.ToArray();

        int numberOfBlocks = (plainText.Length / _BlockSize) + 1;
        Span<byte> buffer = stackalloc byte[numberOfBlocks * _BlockSize];

        plainText.CopyTo(buffer);
        Span<byte> filler = buffer[^(buffer.Length - plainText.Length)..];

        for (int i = 0; i < filler.Length; i++)
            filler[i] = 0x80;

        return buffer.ToArray();
    }

    private bool IsPaddingValid(ReadOnlySpan<byte> value) => (value.Length % _BlockSize) == 0;

    #endregion
}