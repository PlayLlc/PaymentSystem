using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Contains a binary number that indicates which of the application's certification authority public keys and its
///     associated algorithm is to be used
/// </summary>
public record CaPublicKeyIndex : DataElement<byte>, IEqualityComparer<CaPublicKeyIndex>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly CaPublicKeyIndex Five;
    public static readonly CaPublicKeyIndex Four;
    public static readonly CaPublicKeyIndex One;
    public static readonly CaPublicKeyIndex Six;
    public static readonly CaPublicKeyIndex Three;
    public static readonly CaPublicKeyIndex Two;
    public static readonly Tag Tag = 0x8F;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    static CaPublicKeyIndex()
    {
        const byte one = 1;
        const byte two = 2;
        const byte three = 3;
        const byte four = 4;
        const byte five = 5;
        const byte six = 6;

        One = new CaPublicKeyIndex(one);
        Two = new CaPublicKeyIndex(two);
        Three = new CaPublicKeyIndex(three);
        Four = new CaPublicKeyIndex(four);
        Five = new CaPublicKeyIndex(five);
        Six = new CaPublicKeyIndex(six);
    }

    public CaPublicKeyIndex(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CaPublicKeyIndex Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="DataElementNullException"></exception>
    public static CaPublicKeyIndex Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value) as DecodedResult<byte>
            ?? throw new DataElementNullException(EncodingId);

        return new CaPublicKeyIndex(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(CaPublicKeyIndex? x, CaPublicKeyIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CaPublicKeyIndex obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CaPublicKeyIndex value) => value._Value;
    public static explicit operator CaPublicKeyIndex(byte value) => new(value);

    #endregion
}