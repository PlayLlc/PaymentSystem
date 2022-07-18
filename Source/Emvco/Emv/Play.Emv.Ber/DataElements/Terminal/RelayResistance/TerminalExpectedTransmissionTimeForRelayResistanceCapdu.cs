using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Represents the time that the Kernel expects to need for transmitting the EXCHANGE RELAY RESISTANCE DATA command to
///     the Card. The Terminal Expected Transmission Time For Relay Resistance C-APDU is expressed in units of hundreds of
///     microseconds.
/// </summary>
public record TerminalExpectedTransmissionTimeForRelayResistanceCapdu : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8134;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public TerminalExpectedTransmissionTimeForRelayResistanceCapdu(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalExpectedTransmissionTimeForRelayResistanceCapdu Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalExpectedTransmissionTimeForRelayResistanceCapdu Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalExpectedTransmissionTimeForRelayResistanceCapdu Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new TerminalExpectedTransmissionTimeForRelayResistanceCapdu(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator RelaySeconds(TerminalExpectedTransmissionTimeForRelayResistanceCapdu value) => value._Value;

    #endregion
}