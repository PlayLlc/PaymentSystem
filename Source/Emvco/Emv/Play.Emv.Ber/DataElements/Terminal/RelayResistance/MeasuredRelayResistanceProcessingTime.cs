using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Globalization.Time;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the time measured by the Kernel for processing the EXCHANGE RELAY RESISTANCE DATA command. The Measured
///     Relay Resistance Processing Time is expressed in units of hundreds of microseconds.
/// </summary>
public record MeasuredRelayResistanceProcessingTime : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8306;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MeasuredRelayResistanceProcessingTime(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <remarks>Creates the <see cref="MeasuredRelayResistanceProcessingTime" /> according to EMV Book C-2 Section SR1.18 </remarks>
    public static MeasuredRelayResistanceProcessingTime Create(
        Microseconds timeElapsed, TerminalExpectedTransmissionTimeForRelayResistanceCapdu terminalExpectedCapduTransmissionTime,
        TerminalExpectedTransmissionTimeForRelayResistanceRapdu terminalExpectedRapduTransmissionTime,
        DeviceEstimatedTransmissionTimeForRelayResistanceRapdu deviceEstimatedTransmissionTime)
    {
        RelaySeconds timeElapsedInRelaySeconds = new(timeElapsed);

        RelaySeconds fastestExpectedTransmissionTime = terminalExpectedRapduTransmissionTime < (RelaySeconds) deviceEstimatedTransmissionTime
            ? terminalExpectedRapduTransmissionTime
            : deviceEstimatedTransmissionTime;

        RelaySeconds expectedResponseTime = terminalExpectedCapduTransmissionTime - fastestExpectedTransmissionTime;
        RelaySeconds processingTime = timeElapsedInRelaySeconds - expectedResponseTime;

        return new MeasuredRelayResistanceProcessingTime(processingTime < RelaySeconds.Zero ? 0 : (ushort) processingTime);
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MeasuredRelayResistanceProcessingTime Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MeasuredRelayResistanceProcessingTime Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MeasuredRelayResistanceProcessingTime Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MeasuredRelayResistanceProcessingTime(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator RelaySeconds(MeasuredRelayResistanceProcessingTime value) => value._Value;

    #endregion
}