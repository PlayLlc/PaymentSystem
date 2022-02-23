using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

using BinaryCodec = Play.Emv.Ber.Codecs.BinaryCodec;

namespace Play.Emv.Security.Certificates.Pin;

/// <summary>
///     ICC PIN Encipherment Public Key certified by the issuer
/// </summary>
public record IccPinEnciphermentPublicKeyCertificate : PrimitiveValue, IEqualityComparer<IccPinEnciphermentPublicKeyCertificate>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
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

    public byte[] AsByteArray() => _Value.ToByteArray();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.GetByteCount();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IccPinEnciphermentPublicKeyCertificate Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IccPinEnciphermentPublicKeyCertificate Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(PlayEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IccPinEnciphermentPublicKeyCertificate)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IccPinEnciphermentPublicKeyCertificate(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(PlayEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(PlayEncodingId, _Value, length);

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

    public int GetHashCode(IccPinEnciphermentPublicKeyCertificate obj) => obj.GetHashCode();

    #endregion
}