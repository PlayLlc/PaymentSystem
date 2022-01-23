using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.Security.Certificates.Chip.Pin;

/// <summary>
///     ICC PIN Encipherment Public Key certified by the issuer
/// </summary>
public record IccPinEnciphermentPublicKeyCertificate : PrimitiveValue, IEqualityComparer<IccPinEnciphermentPublicKeyCertificate>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F2D;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public IccPinEnciphermentPublicKeyCertificate(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray()
    {
        return _Value.ToByteArray();
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    public int GetByteCount()
    {
        return _Value.GetByteCount();
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return codec.GetByteCount(GetBerEncodingId(), _Value);
    }

    #endregion

    #region Serialization

    public static IccPinEnciphermentPublicKeyCertificate Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IccPinEnciphermentPublicKeyCertificate Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new
                InvalidOperationException($"The {nameof(IccPinEnciphermentPublicKeyCertificate)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IccPinEnciphermentPublicKeyCertificate(result.Value);
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

    public bool Equals(IccPinEnciphermentPublicKeyCertificate? x, IccPinEnciphermentPublicKeyCertificate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPinEnciphermentPublicKeyCertificate obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}