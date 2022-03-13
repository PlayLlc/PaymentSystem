using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements;

/// <summary>
///     Authorized amount of the transaction (excluding adjustments)
/// </summary>
public record AmountAuthorizedNumeric : DataElement<ulong>, IEqualityComparer<AmountAuthorizedNumeric>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F02;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public AmountAuthorizedNumeric(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Money AsMoney(CultureProfile cultureProfile) => new(_Value, cultureProfile);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static AmountAuthorizedNumeric Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static AmountAuthorizedNumeric Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        return new AmountAuthorizedNumeric(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(AmountAuthorizedNumeric? x, AmountAuthorizedNumeric? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AmountAuthorizedNumeric obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(AmountAuthorizedNumeric left, ulong right) => left._Value == right;
    public static bool operator ==(ulong left, AmountAuthorizedNumeric right) => left == right._Value;
    public static explicit operator BigInteger(AmountAuthorizedNumeric value) => value._Value;
    public static implicit operator ulong(AmountAuthorizedNumeric value) => value._Value;
    public static bool operator !=(AmountAuthorizedNumeric left, ulong right) => !(left == right);
    public static bool operator !=(ulong left, AmountAuthorizedNumeric right) => !(left == right);

    #endregion
}