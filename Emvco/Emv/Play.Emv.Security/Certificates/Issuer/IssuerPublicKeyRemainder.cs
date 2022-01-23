using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements.CertificateAuthority;

namespace Play.Emv.Security.Certificates.Issuer;

/// <summary>
///     Remaining digits of the Issuer Public Key Modulus
/// </summary>
public record IssuerPublicKeyRemainder : PrimitiveValue, IEqualityComparer<IssuerPublicKeyRemainder>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x92;

    #endregion

    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public IssuerPublicKeyRemainder(byte[] value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray()
    {
        return _Value;
    }

    public PublicKeyRemainder AsPublicKeyRemainder()
    {
        return new PublicKeyRemainder(_Value);
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    public int GetByteCount()
    {
        return _Value.Length;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return codec.GetByteCount(GetBerEncodingId(), _Value);
    }

    public bool IsEmpty()
    {
        return _Value.Length == 0;
    }

    #endregion

    #region Serialization

    public static IssuerPublicKeyRemainder Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerPublicKeyRemainder Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<byte[]> result = codec.Decode(BerEncodingId, value) as DecodedResult<byte[]>
            ?? throw new
                InvalidOperationException($"The {nameof(IssuerPublicKeyRemainder)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<byte[]>)}");

        return new IssuerPublicKeyRemainder(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec)
    {
        return codec.EncodeValue(BerEncodingId, _Value);
    }

    public override byte[] EncodeValue(BerCodec codec, int length)
    {
        return codec.EncodeValue(BerEncodingId, _Value, length);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerPublicKeyRemainder? x, IssuerPublicKeyRemainder? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerPublicKeyRemainder obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}