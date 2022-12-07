using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Represents the threshold above which the Kernel considers the variation between Device Estimated Transmission Time
///     For Relay Resistance R-APDU and Terminal Expected Transmission Time For Relay Resistance R-APDU no longer
///     acceptable. The Relay Resistance Transmission Time Mismatch Threshold is a percentage and expressed as an integer.
/// </summary>
public record RelayResistanceTransmissionTimeMismatchThreshold : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8137;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public RelayResistanceTransmissionTimeMismatchThreshold(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static RelayResistanceTransmissionTimeMismatchThreshold Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override RelayResistanceTransmissionTimeMismatchThreshold Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static RelayResistanceTransmissionTimeMismatchThreshold Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new RelayResistanceTransmissionTimeMismatchThreshold(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value);

    #endregion

    #region Operator Overrides

    public static explicit operator RelaySeconds(RelayResistanceTransmissionTimeMismatchThreshold value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount(_Value);

    #endregion
}