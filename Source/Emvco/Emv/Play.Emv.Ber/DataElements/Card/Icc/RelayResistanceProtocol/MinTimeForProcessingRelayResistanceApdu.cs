using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the minimum estimated time the Card requires for processing the EXCHANGE RELAY RESISTANCE DATA command.
///     The Min Time For Processing Relay Resistance APDU is expressed in units of hundreds of microseconds.
/// </summary>
public record MinTimeForProcessingRelayResistanceApdu : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8303;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MinTimeForProcessingRelayResistanceApdu(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public override ushort GetValueByteCount() => _ByteLength;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MinTimeForProcessingRelayResistanceApdu Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MinTimeForProcessingRelayResistanceApdu Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MinTimeForProcessingRelayResistanceApdu Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MinTimeForProcessingRelayResistanceApdu(result);
    }

    public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode((ushort)_Value);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode((ushort)_Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator RelaySeconds(MinTimeForProcessingRelayResistanceApdu value) => value._Value;

    #endregion
}