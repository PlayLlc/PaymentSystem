using Play.Emv.DataElements.CertificateAuthority;

namespace Play.Encryption.Encryption.Ciphers.Asymmetric;

public interface IAsymmetricCodec
{
    #region Instance Members

    public byte[] Decrypt(ReadOnlySpan<byte> decodedSignature, PublicKeyInfo publicKeyInfo);
    public byte[] Sign(ReadOnlySpan<byte> message, PublicKeyInfo publicKeyInfo);

    #endregion
}