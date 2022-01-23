using System.Security.Cryptography;

using Play.Emv.DataElements.CertificateAuthority;

namespace ___TEMP.Play.Emv.Security.Encryption.Ciphers.Asymmetric;

internal class RsaCodec : IAsymmetricCodec
{
    #region Instance Members

    public byte[] Decrypt(ReadOnlySpan<byte> decodedSignature, PublicKeyInfo publicKeyInfo)
    {
        RSACryptoServiceProvider rsaProvider = GetRsaProvider(publicKeyInfo.GetPublicKeyExponent(), publicKeyInfo.GetPublicKeyModulus());

        return rsaProvider.Decrypt(decodedSignature.ToArray(), RSAEncryptionPadding.Pkcs1);
    }

    private static RSACryptoServiceProvider GetRsaProvider(PublicKeyExponent exponent, PublicKeyModulus modulus)
    {
        RSACryptoServiceProvider? rsaProvider = new();
        RSAParameters rsaConfig = new() {Exponent = exponent.AsByteArray(), Modulus = modulus.AsByteArray()};

        rsaProvider.ImportParameters(rsaConfig);

        return rsaProvider;
    }

    public byte[] Sign(ReadOnlySpan<byte> message, PublicKeyInfo publicKeyInfo)
    {
        RSACryptoServiceProvider rsaProvider = GetRsaProvider(publicKeyInfo.GetPublicKeyExponent(), publicKeyInfo.GetPublicKeyModulus());

        return rsaProvider.SignData(message.ToArray(), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
    }

    #endregion
}