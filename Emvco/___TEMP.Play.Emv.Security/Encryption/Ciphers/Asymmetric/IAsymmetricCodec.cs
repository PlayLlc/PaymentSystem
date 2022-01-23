using System;

using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Contracts;

namespace Play.Emv.Security.Encryption.Ciphers;

public interface IAsymmetricCodec
{
    #region Instance Members

    public byte[] Decrypt(ReadOnlySpan<byte> decodedSignature, PublicKeyInfo publicKeyInfo);
    public byte[] Sign(ReadOnlySpan<byte> message, PublicKeyInfo publicKeyInfo);

    #endregion
}