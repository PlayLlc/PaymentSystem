using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the time the Card expects to need for transmitting the EXCHANGE RELAY RESISTANCE DATA R-APDU. The Device
///     Estimated Transmission Time For Relay Resistance RAPDU is expressed in units of hundreds of microseconds.
/// </summary>
public record DeviceEstimatedTransmissionTimeForRelayResistanceRapdu : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8305;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public DeviceEstimatedTransmissionTimeForRelayResistanceRapdu(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DeviceEstimatedTransmissionTimeForRelayResistanceRapdu Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DeviceEstimatedTransmissionTimeForRelayResistanceRapdu Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DeviceEstimatedTransmissionTimeForRelayResistanceRapdu Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new DeviceEstimatedTransmissionTimeForRelayResistanceRapdu(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode((ushort) _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode((ushort) _Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator RelaySeconds(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount() => PlayCodec.BinaryCodec.GetByteCount((ushort) _Value);

    #endregion
}