using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the maximum number of records that can be stored in the Torn Transaction Log.
/// </summary>
public record MaxNumberOfTornTransactionLogRecords : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811D;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public MaxNumberOfTornTransactionLogRecords(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaxNumberOfTornTransactionLogRecords Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MaxNumberOfTornTransactionLogRecords Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaxNumberOfTornTransactionLogRecords Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new MaxNumberOfTornTransactionLogRecords(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(TornRecord? x, TornRecord? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TornRecord obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(MaxNumberOfTornTransactionLogRecords value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}