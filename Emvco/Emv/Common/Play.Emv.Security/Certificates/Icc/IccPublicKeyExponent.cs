using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Encryption.Certificates; 

namespace Play.Emv.Security.Certificates.Icc;

/// <summary>
///     ICC Public Key Exponent used for the verification of the Signed Dynamic Application Data
/// </summary>
public record IccPublicKeyExponent : PrimitiveValue, IEqualityComparer<IccPublicKeyExponent>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F47;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public IccPublicKeyExponent(uint value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public PublicKeyExponent AsPublicKeyExponent() => PublicKeyExponent.Get(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(EncodingId, _Value);

    #endregion

    #region Serialization

    public static IccPublicKeyExponent Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IccPublicKeyExponent Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort minByteLength = 1;
        const ushort maxByteLength = 3;

        if (value.Length is < minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IccPublicKeyExponent)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<uint> result = codec.Decode(EncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(IccPublicKeyExponent)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new IccPublicKeyExponent(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IccPublicKeyExponent? x, IccPublicKeyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IccPublicKeyExponent obj) => obj.GetHashCode();

    #endregion
}