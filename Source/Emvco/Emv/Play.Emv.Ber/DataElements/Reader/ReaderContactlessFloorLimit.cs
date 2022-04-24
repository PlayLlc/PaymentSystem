﻿using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Currency;

namespace Play.Emv.Ber.DataElements;

public record ReaderContactlessFloorLimit : DataElement<ulong>, IEqualityComparer<ReaderContactlessFloorLimit>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8123;
    private const byte _ByteLength = 6;
    private const byte _CharLength = 12;

    #endregion

    #region Constructor

    public ReaderContactlessFloorLimit(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public Money AsMoney(NumericCurrencyCode numericCurrencyCode) => new(_Value, numericCurrencyCode);

    #endregion

    #region Serialization

    public static ReaderContactlessFloorLimit Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override ReaderContactlessFloorLimit Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static ReaderContactlessFloorLimit Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        Check.Primitive.ForMaxCharLength(result, _CharLength, Tag);

        return new ReaderContactlessFloorLimit(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(ReaderContactlessFloorLimit? x, ReaderContactlessFloorLimit? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ReaderContactlessFloorLimit obj) => obj.GetHashCode();

    #endregion
}