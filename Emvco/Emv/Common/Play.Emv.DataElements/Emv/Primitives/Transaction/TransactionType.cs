using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the type of financial transaction, represented by the first two digits of the ISO 8583:1987 Processing
///     Code. The actual values to be used for the Transaction Type data element are defined by the relevant payment system
/// </summary>
public record TransactionType : DataElement<byte>, IEqualityComparer<TransactionType>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = NumericCodec.EncodingId;

    /// <summary>
    ///     Also known as 'Cash Withdrawal'
    /// </summary>
    public static readonly TransactionType CashAdvance = new(0x01);

    public static readonly TransactionType Purchase = new(0x00);
    public static readonly TransactionType PurchaseWithCashback = new(0x09);
    public static readonly TransactionType Refund = new(0x20);
    public static readonly Tag Tag = 0x9C;
    private const byte _ByteLength = 1;
    private const byte _CharLength = 2;

    #endregion

    #region Constructor

    public TransactionType(byte value) : base(value)
    {
        Validate(value);
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    private void Validate(byte value)
    {
        if (value.GetNumberOfDigits() != _CharLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionType)} could not be initialized because the decoded character length was out of range. The decoded character length was {value.GetNumberOfDigits()} but must be {_CharLength} bytes in length");
        }
    }

    #endregion

    #region Serialization

    public static TransactionType Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionType Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionType)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<byte> result = codec.Decode(PlayEncodingId, value) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(TransactionType)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new TransactionType(result.Value);
    }

    #endregion

    #region Equality

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

    #endregion
}