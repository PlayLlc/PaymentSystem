using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.Security.Checksum;

/// <summary>
///     Description: The CVC3 (Track1) is a 2-byte cryptogram returned by the Card in the response to the COMPUTE
///     CRYPTOGRAPHIC CHECKSUM command.
/// </summary>
public record CardholderVerificationCode3Track1 : PrimitiveValue, IEqualityComparer<CardholderVerificationCode3Track1>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F60;

    #endregion

    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public CardholderVerificationCode3Track1(ushort value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static bool EqualsStatic(CardholderVerificationCode3Track1? x, CardholderVerificationCode3Track1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
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

    public static CardholderVerificationCode3Track1 Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CardholderVerificationCode3Track1 Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 2;

        if (value.Length != byteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(CardholderVerificationCode3Track1)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ushort> result = codec.Decode(BerEncodingId, value) as DecodedResult<ushort>
            ?? throw new
                InvalidOperationException($"The {nameof(CardholderVerificationCode3Track1)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new CardholderVerificationCode3Track1(result.Value);
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

    public bool Equals(CardholderVerificationCode3Track1? x, CardholderVerificationCode3Track1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CardholderVerificationCode3Track1 obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}