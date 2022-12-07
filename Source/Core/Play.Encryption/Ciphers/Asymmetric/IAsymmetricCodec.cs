﻿using Play.Encryption.Certificates;

namespace Play.Encryption.Ciphers.Asymmetric;

public interface IAsymmetricCodec
{
    #region Instance Members

    public byte[] Decrypt(ReadOnlySpan<byte> decodedSignature, PublicKeyInfo publicKeyInfo);
    public byte[] Sign(ReadOnlySpan<byte> message, PublicKeyInfo publicKeyInfo);

    #endregion
}