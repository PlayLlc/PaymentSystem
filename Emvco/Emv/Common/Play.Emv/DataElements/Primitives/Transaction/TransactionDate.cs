using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.DataElements;

/// <summary>
///     Local date that the transaction was authorized
/// </summary>
public record TransactionDate : DataElement<uint>, IEqualityComparer<TransactionDate>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9A;
    private const byte _ByteLength = 3;
    private const byte _CharLength = 6;

    #endregion

    #region Constructor

    public TransactionDate(uint value) : base(value)
    { }

    public TransactionDate(DateTimeUtc dateTime) : base(GetNumeric(dateTime))
    { }

    #endregion

    #region Instance Members

    private static uint GetNumeric(DateTimeUtc value)
    {
        int result = value.Year();
        result *= 100;
        result += value.Month();
        result *= 100;
        result += value.Day();

        return (uint) result;
    }

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TransactionDate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TransactionDate Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new TransactionDate(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(TransactionDate? x, TransactionDate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionDate obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(TransactionDate value) => value._Value;

    #endregion
}