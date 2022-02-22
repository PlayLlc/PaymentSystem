using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Encryption.Certificates;

using BinaryCodec = Play.Emv.Ber.Codecs.BinaryCodec;
using PlayEncodingId = Play.Ber.Codecs.PlayEncodingId;

namespace Play.Emv.Security.Certificates.Issuer;

/// <summary>
///     Issuer public key exponent used for the verification of the Signed Static Application Data and the ICC Public Key
///     Certificate
/// </summary>
public record IssuerPublicKeyExponent : PrimitiveValue, IEqualityComparer<IssuerPublicKeyExponent>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F32;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    public IssuerPublicKeyExponent(uint value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => PlayEncoding.UnsignedInteger.GetBytes(_Value);
    public PublicKeyExponent AsPublicKeyExponent() => PublicKeyExponent.Get(_Value);
    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public int GetByteCount() => _Value.GetMostSignificantBit();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerPublicKeyExponent Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerPublicKeyExponent Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort minByteLength = 1;
        const ushort maxByteLength = 3;

        if (value.Length is not >= minByteLength and <= maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerPublicKeyExponent)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be in the range of {minByteLength}-{maxByteLength}");
        }

        DecodedResult<uint> result = codec.Decode(PlayEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerPublicKeyExponent)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new IssuerPublicKeyExponent(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(PlayEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(PlayEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IssuerPublicKeyExponent? x, IssuerPublicKeyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerPublicKeyExponent obj) => obj.GetHashCode();

    #endregion
}