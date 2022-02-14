using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;

namespace Play.Emv.Pos.Configuration;

/// <summary>
///     Indicates the implied position of the decimal point from the right of the transaction amount represented according
///     to ISO 4217
/// </summary>
public record TransactionCurrencyExponent : PrimitiveValue, IEqualityComparer<TransactionCurrencyExponent>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = Numeric.Identifier;
    public static readonly Tag Tag = 0x5F36;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public TransactionCurrencyExponent(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TransactionCurrencyExponent Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionCurrencyExponent Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 1;
        const ushort charLength = 1;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionCurrencyExponent)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(TransactionCurrencyExponent)} could not be initialized because the {nameof(Numeric)} returned a null {nameof(DecodedResult<byte>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionCurrencyExponent)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new TransactionCurrencyExponent(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

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