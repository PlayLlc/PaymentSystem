using System.Security.Cryptography;

using Play.Encryption.Certificates;

namespace Play.Encryption.Ciphers.Asymmetric;

internal class RsaCodec : IAsymmetricCodec
{
    #region Instance Members

    /// <summary>
    ///     Decrypt
    /// </summary>
    /// <param name="decodedSignature"></param>
    /// <param name="publicKeyInfo"></param>
    /// <returns></returns>
    /// <exception cref="CryptographicException"></exception>
    public byte[] Decrypt(ReadOnlySpan<byte> decodedSignature, PublicKeyInfo publicKeyInfo)
    {
        RSACryptoServiceProvider rsaProvider = GetRsaProvider(publicKeyInfo.GetPublicKeyExponent(), publicKeyInfo.GetPublicKeyModulus());

        return rsaProvider.Decrypt(decodedSignature.ToArray(), RSAEncryptionPadding.Pkcs1);
    }

    /// <exception cref="CryptographicException"></exception>
    private static RSACryptoServiceProvider GetRsaProvider(PublicKeyExponents exponents, PublicKeyModulus modulus)
    {
        RSACryptoServiceProvider? rsaProvider = new();
        RSAParameters rsaConfig = new() {Exponent = exponents.Encode(), Modulus = modulus.AsByteArray()};

        rsaProvider.ImportParameters(rsaConfig);

        return rsaProvider;
    }

    /// <exception cref="CryptographicException"></exception>
    public byte[] Sign(ReadOnlySpan<byte> message, PublicKeyInfo publicKeyInfo)
    {
        RSACryptoServiceProvider rsaProvider = GetRsaProvider(publicKeyInfo.GetPublicKeyExponent(), publicKeyInfo.GetPublicKeyModulus());

        return rsaProvider.SignData(message.ToArray(), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
    }

    #endregion
}