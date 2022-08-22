using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The value of NATC(Track1) represents the number of digits of the Application Transaction Counter to be included in
///     the discretionary data field of Track 1 Data.
/// </summary>
public record NumericApplicationTransactionCounterTrack1 : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F64;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public NumericApplicationTransactionCounterTrack1(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static NumericApplicationTransactionCounterTrack1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static NumericApplicationTransactionCounterTrack1 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new NumericApplicationTransactionCounterTrack1(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator byte(NumericApplicationTransactionCounterTrack1 value) => value._Value;

    #endregion

    #region Instance Members

    public int GetSetBitCount() => _Value.GetSetBitCount();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}