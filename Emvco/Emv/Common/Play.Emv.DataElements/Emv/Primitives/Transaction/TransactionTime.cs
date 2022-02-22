using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Globalization.Time;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Local date that the transaction was authorized
/// </summary>
public record TransactionTime : DataElement<uint>, IEqualityComparer<TransactionTime>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F21;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public TransactionTime(uint value) : base(value)
    { }

    public TransactionTime(DateTimeUtc dateTime) : base(GetNumeric(dateTime))
    { }

    #endregion

    #region Instance Members

    private static uint GetNumeric(DateTimeUtc value)
    {
        int result = value.Hour();
        result *= 100;
        result += value.Minute();
        result *= 100;
        result += value.Second();

        return (uint) result;
    }

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static TransactionTime Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionTime Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 6;

        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionTime)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(PlayEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(TransactionTime)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<uint>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionTime)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new TransactionTime(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(GetEncodingId(), _Value, _ByteLength);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(TransactionTime? x, TransactionTime? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionTime obj) => obj.GetHashCode();

    #endregion
}