﻿using System;

namespace Play.Emv.Security.Encryption;

public interface IFormatPlainText
{
    #region Instance Members

    public byte[] Format(ReadOnlySpan<byte> plainText);

    #endregion
}