using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The value of NATC(Track2) represents the number of digits of the Application Transaction Counter to be included in
///     the discretionary data field of Track 2 Data.
/// </summary>
public record NumericApplicationTransactionCounterTrack2 : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F67;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public NumericApplicationTransactionCounterTrack2(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int GetSetBitCount() => _Value.GetSetBitCount();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static NumericApplicationTransactionCounterTrack2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override NumericApplicationTransactionCounterTrack2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static NumericApplicationTransactionCounterTrack2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new NumericApplicationTransactionCounterTrack2(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator byte(NumericApplicationTransactionCounterTrack2 value) => value._Value;

    #endregion
}