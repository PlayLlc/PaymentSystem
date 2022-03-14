using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Security.Certificates.Pin;

/// <summary>
///     ICC PIN Encipherment Public Key Exponent used for PIN encipherment
/// </summary>
public record IccPinEnciphermentPublicKeyExponent : PrimitiveValue, IEqualityComparer<IccPinEnciphermentPublicKeyExponent>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F2E;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public IccPinEnciphermentPublicKeyExponent(uint value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IccPinEnciphermentPublicKeyExponent Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IccPinEnciphermentPublicKeyExponent Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort minByteLength = 1;
        const ushort maxByteLength = 3;

        if (value.Length is not >= minByteLength and <= maxByteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(IccPinEnciphermentPublicKeyExponent)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<uint> result = codec.Decode(EncodingId, value) as DecodedResult<uint>
            ?? throw new
                InvalidOperationException($"The {nameof(IccPinEnciphermentPublicKeyExponent)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new IccPinEnciphermentPublicKeyExponent(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IccPinEnciphermentPublicKeyExponent? x, IccPinEnciphermentPublicKeyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPinEnciphermentPublicKeyExponent obj) => obj.GetHashCode();

    #endregion
}