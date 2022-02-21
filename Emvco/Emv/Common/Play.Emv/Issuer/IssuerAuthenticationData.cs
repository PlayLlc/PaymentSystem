using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;

namespace Play.Emv.Issuer;

/// <summary>
///     Data sent to the ICC for online issuer authentication
/// </summary>
public record IssuerAuthenticationData : PrimitiveValue, IEqualityComparer<IssuerAuthenticationData>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x91;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public IssuerAuthenticationData(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerAuthenticationData Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerAuthenticationData Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort minByteLength = 8;
        const ushort maxByteLength = 16;

        if (value.Length is not >= minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerAuthenticationData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<BigInteger> result = codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerAuthenticationData)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerAuthenticationData(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IssuerAuthenticationData? x, IssuerAuthenticationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerAuthenticationData obj) => obj.GetHashCode();

    #endregion
}