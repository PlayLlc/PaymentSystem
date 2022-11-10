using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements;

public record MaxLifetimeOfTornTransactionLogRecords : DataElement<Seconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811C;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MaxLifetimeOfTornTransactionLogRecords(Seconds value) : base(value)
    {
        Check.Primitive.ForExactLength(PlayCodec.BinaryCodec.Encode((long)value), _ByteLength, Tag);
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaxLifetimeOfTornTransactionLogRecords Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MaxLifetimeOfTornTransactionLogRecords Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaxLifetimeOfTornTransactionLogRecords Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MaxLifetimeOfTornTransactionLogRecords(new Seconds(result));
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode((ushort)_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode((ushort)_Value, length);

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

    public static implicit operator Seconds(MaxLifetimeOfTornTransactionLogRecords value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount((ushort)_Value);

    #endregion
}