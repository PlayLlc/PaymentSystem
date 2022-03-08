using System;
using System.Numerics;
using System.Security.Cryptography;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Emv.DataElements.Emv;
using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Globalization.Time;

namespace Play.Emv.Security.Certificates;

public class CaPublicKeyCertificate : PublicKeyCertificate
{
    #region Instance Values

    private readonly CaPublicKeyCertificateIdentifier _Id;
    private readonly bool _IsRevoked;

    #endregion

    #region Constructor

    public CaPublicKeyCertificate(
        CaPublicKeyCertificateIdentifier id,
        bool isRevoked,
        CertificateSerialNumber certificateSerialNumber,
        HashAlgorithmIndicator hashAlgorithmIndicator,
        PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator,
        DateRange validityPeriod,
        PublicKeyInfo publicKeyInfo) : base(certificateSerialNumber, hashAlgorithmIndicator, publicKeyAlgorithmIndicator, validityPeriod,
        publicKeyInfo)
    {
        _Id = id;
        _IsRevoked = isRevoked;
        _IsRevoked = isRevoked ? _IsRevoked : validityPeriod.IsExpired();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     A check value calculated using SHA-1 to verify the integrity of the Certification Authority Public Key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="modulus"></param>
    /// <param name="exponent"></param>
    private static BigInteger CalculateChecksum(CaPublicKeyCertificateIdentifier key, PublicKeyModulus modulus, PublicKeyExponent exponent)
    {
        using SpanOwner<byte> owner = SpanOwner<byte>.Allocate(20);
        Span<byte> buffer = owner.Span;
        key.GetRegisteredApplicationProviderIndicator().AsByteArray().AsSpan().CopyTo(buffer[..5]);
        buffer[5] = (byte) key.GetCaPublicKeyIndex();
        modulus.AsByteArray().AsSpan().CopyTo(buffer[6..(modulus.GetByteCount() + 6)]);
        exponent.AsByteArray().AsSpan().CopyTo(buffer[(modulus.GetByteCount() + 6)..]);

        return new BigInteger(SHA1.HashData(buffer));
    }

    public CaPublicKeyIndex GetCaPublicKeyIndex() => _Id.GetCaPublicKeyIndex();
    public BigInteger GetChecksum() => CalculateChecksum(_Id, GetPublicKeyModulus(), GetPublicKeyExponent());
    public CaPublicKeyCertificateIdentifier GetId() => _Id;

    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() =>
        _Id.GetRegisteredApplicationProviderIndicator();

    public bool IsRevoked() => _IsRevoked ? _IsRevoked : IsExpired();

    #endregion
}