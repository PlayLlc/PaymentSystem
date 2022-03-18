using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the type of financial transaction, represented by the first two digits of the ISO 8583:1987 Processing
///     Code. The actual values to be used for the Transaction Type data element are defined by the relevant payment system
/// </summary>
public record TransactionType : DataElement<byte>, IEqualityComparer<TransactionType>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;

    /// <remarks>Hex: 0x9C</remarks>
    public static readonly Tag Tag = 0x9C;

    private const byte _ByteLength = 1;
    private const byte _CharLength = 2;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public TransactionType(byte value) : base(value)
    {
        Validate(value);
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    /// <summary>
    ///     Validate
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    private void Validate(byte value)
    {
        if (value.GetNumberOfDigits() != _CharLength)
        {
            throw new
                DataElementParsingException($"The Primitive Value {nameof(TransactionType)} could not be initialized because the decoded character length was out of range. The decoded character length was {value.GetNumberOfDigits()} but must be {_CharLength} bytes in length");
        }
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TransactionType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TransactionType Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.NumericCodec.DecodeToByte(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new TransactionType(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(TransactionTypes value) => value == _Value;
    public bool Equals(TransactionType x, TransactionTypes y) => (byte) y == x._Value;
    public bool Equals(TransactionTypes x, TransactionType y) => (byte) x == y._Value;

    public bool Equals(TransactionType? x, TransactionType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionType obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(TransactionType value) => value._Value;
    public static bool operator ==(TransactionType left, TransactionTypes right) => left._Value == (byte) right;
    public static bool operator !=(TransactionType left, TransactionTypes right) => left._Value != (byte) right;
    public static bool operator ==(TransactionTypes left, TransactionType right) => right._Value == (byte) left;
    public static bool operator !=(TransactionTypes left, TransactionType right) => right._Value != (byte) left;

    #endregion
}