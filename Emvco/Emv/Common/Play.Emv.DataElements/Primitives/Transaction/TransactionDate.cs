using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Globalization.Time;

namespace Play.Emv.DataElements;

/// <summary>
///     Local date that the transaction was authorized
/// </summary>
public record TransactionDate : DataElement<uint>, IEqualityComparer<TransactionDate>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = Numeric.Identifier;
    public static readonly Tag Tag = 0x9A;
    private const byte _ByteLength = 3;

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

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static TransactionDate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionDate Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 6;

        if (value.Length != _ByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionDate)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteLength} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(TransactionDate)} could not be initialized because the {nameof(Numeric)} returned a null {nameof(DecodedResult<uint>)}");

        if (result.CharCount != charLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(TransactionDate)} could not be initialized because the decoded character length was out of range. The decoded character length was {result.CharCount} but must be {charLength} bytes in length");
        }

        return new TransactionDate(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(GetBerEncodingId(), _Value, _ByteLength);
    public override byte[] EncodeValue(BerCodec berCodec) => EncodeValue();

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
}