using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.Terminal.Configuration.Transaction;

/// <summary>
///     Indicates the implied position of the decimal point from the right of the transaction amount, with the Transaction
///     Reference Currency Code represented according to ISO 4217
/// </summary>
public record TransactionReferenceCurrencyExponent : PrimitiveValue, IEqualityComparer<TransactionReferenceCurrencyExponent>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    public static readonly Tag Tag = 0x9F3D;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public TransactionReferenceCurrencyExponent(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

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

    public static TransactionReferenceCurrencyExponent Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionReferenceCurrencyExponent Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 1;
        const ushort charLength = 1;

        if (value.Length != byteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(TransactionReferenceCurrencyExponent)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new
                InvalidOperationException($"The {nameof(TransactionReferenceCurrencyExponent)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<byte>)}");

        if (result.CharCount != charLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(TransactionReferenceCurrencyExponent)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new TransactionReferenceCurrencyExponent(result.Value);
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

    public bool Equals(TransactionReferenceCurrencyExponent? x, TransactionReferenceCurrencyExponent? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionReferenceCurrencyExponent obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}