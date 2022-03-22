﻿using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Ber.DataElements;

public record MaxLifetimeOfTornTransactionLogRecords : DataElement<Seconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811C;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MaxLifetimeOfTornTransactionLogRecords(Seconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MaxLifetimeOfTornTransactionLogRecords Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MaxLifetimeOfTornTransactionLogRecords Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static MaxLifetimeOfTornTransactionLogRecords Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MaxLifetimeOfTornTransactionLogRecords(new Seconds(result));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(TornRecord? x, TornRecord? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TornRecord obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator Seconds(MaxLifetimeOfTornTransactionLogRecords value) => new(value._Value);

    #endregion
}