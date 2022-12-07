using Play.Codecs;
using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Security;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Globalization.Time;
using Play.Icc.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace MockPos.Dtos;

public class CertificateDto
{
    #region Instance Values

    public string? RegisteredApplicationProviderIndicator { get; set; }
    public string? PublicKeyIndex { get; set; }
    public byte HashAlgorithmIndicator { get; set; }
    public byte PublicKeyAlgorithmIndicator { get; set; }
    public string? ActivationDate { get; set; }
    public string? ExpirationDate { get; set; }
    public uint Exponent { get; set; }
    public string? Modulus { get; set; }
    public string? Checksum { get; set; }
    public bool IsRevoked { get; set; }
    public string? CertificateSerialNumber { get; set; }

    #endregion

    #region Serialization

    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="OverflowException"></exception>
    public CaPublicKeyCertificate Decode()
    {
        RegisteredApplicationProviderIndicator rid = new(PlayCodec.HexadecimalCodec.DecodeToUIn64(RegisteredApplicationProviderIndicator));
        PublicKeyModulus modulus = new(PlayCodec.HexadecimalCodec.Encode(Modulus));
        CertificateSerialNumber serialNumber = new(PlayCodec.HexadecimalCodec.Encode(CertificateSerialNumber));
        ushort activationDate = ushort.Parse(ActivationDate!);
        ushort expirationDate = ushort.Parse(ExpirationDate!);
        DateRange validityPeriod = new(new ShortDate(activationDate), new ShortDate(expirationDate));

        PublicKeyExponents.Empty.TryGet(Exponent, out EnumObject<uint>? publicKeyExponentResult);
        HashAlgorithmIndicators.Empty.TryGet(HashAlgorithmIndicator, out EnumObject<byte>? hashAlgorithmIndicatorResult);
        PublicKeyAlgorithmIndicators.Empty.TryGet(PublicKeyAlgorithmIndicator, out EnumObject<byte>? publicKeyAlgorithmIndicatorResult);

        PublicKeyExponents publicKeyExponent = (PublicKeyExponents) (publicKeyExponentResult ?? PublicKeyExponents._3);
        HashAlgorithmIndicators hashAlgorithmIndicator = (HashAlgorithmIndicators) (hashAlgorithmIndicatorResult ?? HashAlgorithmIndicators.Sha1);
        PublicKeyAlgorithmIndicators publicKeyAlgorithmIndicators =
            (PublicKeyAlgorithmIndicators) (publicKeyAlgorithmIndicatorResult ?? PublicKeyAlgorithmIndicators.Rsa);

        CaPublicKeyIndex index = new(PlayCodec.HexadecimalCodec.DecodeToByte(PublicKeyIndex));
        PublicKeyInfo publicKeyInfo = new(modulus, publicKeyExponent);
        CaPublicKeyCertificateIdentifier identifier = new(index, rid);

        return new CaPublicKeyCertificate(identifier, IsRevoked, serialNumber, hashAlgorithmIndicator, publicKeyAlgorithmIndicators, validityPeriod,
            publicKeyInfo);
    }

    #endregion
}