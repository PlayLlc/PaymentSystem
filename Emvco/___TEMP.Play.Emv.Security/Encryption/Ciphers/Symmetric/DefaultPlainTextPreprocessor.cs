using System;

namespace Play.Emv.Security.Encryption.Ciphers;

public class DefaultPlainTextPreprocessor : IPreprocessPlainText
{
    #region Instance Members

    public byte[] Preprocess(ReadOnlySpan<byte> plainText) => plainText.ToArray();

    #endregion
}