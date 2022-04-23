using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the card data input capability of the Terminal and Reader.
/// </summary>
public record CardDataInputCapability : DataElement<byte>, IEqualityComparer<CardDataInputCapability>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8117;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public CardDataInputCapability(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static CardDataInputCapability Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override CardDataInputCapability Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static CardDataInputCapability Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new CardDataInputCapability(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(CardDataInputCapability? x, CardDataInputCapability? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CardDataInputCapability obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(CardDataInputCapability value) => value._Value;

    #endregion
}