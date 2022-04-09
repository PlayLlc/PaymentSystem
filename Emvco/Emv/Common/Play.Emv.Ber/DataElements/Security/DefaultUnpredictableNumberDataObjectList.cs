﻿using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The <see cref="DefaultUnpredictableNumberDataObjectList" /> is the UDOL to be used for constructing the value field
///     of the COMPUTE CRYPTOGRAPHIC CHECKSUM command if the UDOL in the Card is not present. The Default UDOL must contain
///     as its only entry the tag and length of the Unpredictable Number (Numeric) and has the value: '9F6A04'.
/// </summary>
public record DefaultUnpredictableNumberDataObjectList : DataObjectList, IEqualityComparer<DefaultUnpredictableNumberDataObjectList>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811A;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public DefaultUnpredictableNumberDataObjectList(BigInteger value) : base(value)
    {
        if (!_Codec.IsTagPresent(UnpredictableNumber.Tag, value.ToByteArray()))
        {
            throw new
                CardDataMissingException($"The {nameof(DefaultUnpredictableNumberDataObjectList)} must contain a tag for {nameof(UnpredictableNumber)}");
        }
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DefaultUnpredictableNumberDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DefaultUnpredictableNumberDataObjectList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DefaultUnpredictableNumberDataObjectList Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new DefaultUnpredictableNumberDataObjectList(new BigInteger(value));
    }

    #endregion

    #region Equality

    public bool Equals(DefaultUnpredictableNumberDataObjectList? x, DefaultUnpredictableNumberDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DefaultUnpredictableNumberDataObjectList obj) => obj.GetHashCode();

    #endregion
}