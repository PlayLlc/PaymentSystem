using System;

namespace Play.Emv.Security.Encryption.Ciphers;

/// <summary>
///     Allows preprocessing of the plain text message before it is enciphered by a block cipher
/// </summary>
public interface IPreprocessPlainText
{
    #region Instance Members

    public byte[] Preprocess(ReadOnlySpan<byte> plainText);

    #endregion
}