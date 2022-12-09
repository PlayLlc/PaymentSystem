﻿using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Lengths;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Secondary amount associated with the transaction representing a cashback amount
/// </summary>
public record AmountOtherNumeric : DataElement<ulong>, IEqualityComparer<AmountOtherNumeric>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F03;
    private const byte _ByteLength = 6;
    private const byte _CharLength = 12;

    #endregion

    #region Instance Values

    private Length _Length;

    #endregion

    #region Constructor

    public AmountOtherNumeric(ulong value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static AmountOtherNumeric Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override AmountOtherNumeric Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static AmountOtherNumeric Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new AmountOtherNumeric(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(AmountOtherNumeric? x, AmountOtherNumeric? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AmountOtherNumeric obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public Money AsMoney(NumericCurrencyCode numericCurrencyCode) => new(_Value, numericCurrencyCode);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => PlayCodec.NumericCodec.GetByteCount(_Value);
    public override ushort GetValueByteCount() => PlayCodec.NumericCodec.GetByteCount(_Value);

    #endregion
}