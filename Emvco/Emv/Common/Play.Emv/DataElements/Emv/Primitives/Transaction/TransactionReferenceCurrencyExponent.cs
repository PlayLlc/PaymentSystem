using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the implied position of the decimal point from the right of the transaction amount, with the Transaction
///     Reference Currency Code represented according to ISO 4217
/// </summary>
public record TransactionReferenceCurrencyExponent : DataElement<byte>, IEqualityComparer<TransactionReferenceCurrencyExponent>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F3D;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public TransactionReferenceCurrencyExponent(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TransactionReferenceCurrencyExponent Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataObjectParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TransactionReferenceCurrencyExponent Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort charLength = 1;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = codec.Decode(EncodingId, value).ToByteResult() ?? throw new DataObjectParsingException(EncodingId);

        Check.Primitive.ForCharLength(result.Value, charLength, Tag);

        return new TransactionReferenceCurrencyExponent(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(TransactionReferenceCurrencyExponent? x, TransactionReferenceCurrencyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionReferenceCurrencyExponent obj) => obj.GetHashCode();

    #endregion
}