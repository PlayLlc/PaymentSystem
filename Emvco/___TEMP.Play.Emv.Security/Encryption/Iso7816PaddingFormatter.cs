using System;

using Play.Emv.Security.Encryption.Ciphers;

namespace Play.Emv.Security.Encryption;

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

        Span<byte> buffer = stackalloc byte[plainText.Length + (plainText.Length % _BlockSize)];

        plainText.CopyTo(buffer);
        buffer[plainText.Length + 1] = 0x80;

        return buffer.ToArray();
    }

    private bool IsPaddingValid(ReadOnlySpan<byte> value) => (value.Length % _BlockSize) == 0;

    #endregion
}