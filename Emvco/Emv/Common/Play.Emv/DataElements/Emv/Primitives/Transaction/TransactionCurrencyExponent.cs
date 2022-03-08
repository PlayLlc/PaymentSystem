using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.Transaction;

/// <summary>
///     Indicates the implied position of the decimal point from the right of the transaction amount represented according
///     to ISO 4217
/// </summary>
public record TransactionCurrencyExponent : DataElement<byte>, IEqualityComparer<TransactionCurrencyExponent>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5F36;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public TransactionCurrencyExponent(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TransactionCurrencyExponent Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="DataElementNullException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TransactionCurrencyExponent Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 1;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value).ToByteResult() ?? throw new DataElementNullException(EncodingId);

        Check.Primitive.ForCharLength(result.CharCount, charLength, Tag);

        return new TransactionCurrencyExponent(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(TransactionCurrencyExponent? x, TransactionCurrencyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionCurrencyExponent obj) => obj.GetHashCode();

    #endregion
}