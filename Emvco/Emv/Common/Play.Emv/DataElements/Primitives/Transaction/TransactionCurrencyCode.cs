using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the currency code of the transaction according to ISO 4217
/// </summary>
public record TransactionCurrencyCode : DataElement<NumericCurrencyCode>, IEqualityComparer<TransactionCurrencyCode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5F2A;
    private const byte _ByteLength = 2;
    private const byte _CharLength = 3;

    #endregion

    #region Constructor

    public TransactionCurrencyCode(NumericCurrencyCode value) : base(value)
    { }

    public TransactionCurrencyCode(CultureProfile cultureProfile) : base(cultureProfile.GetNumericCurrencyCode())
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TransactionCurrencyCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TransactionCurrencyCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new TransactionCurrencyCode(new NumericCurrencyCode(result));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, (ushort) _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(TransactionCurrencyCode? x, TransactionCurrencyCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionCurrencyCode obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator NumericCurrencyCode(TransactionCurrencyCode value) => value._Value;

    #endregion
}