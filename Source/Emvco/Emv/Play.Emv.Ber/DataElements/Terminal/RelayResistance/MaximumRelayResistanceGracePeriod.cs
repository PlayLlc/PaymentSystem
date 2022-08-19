using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The Minimum Relay Resistance Grace Period and Maximum Rela Resistance Grace Period represent how far outside the
///     window defined by the Card that the measured time may be and yet still be considered acceptable. The Maximum Relay
///     Resistance Grace Period is expressed in units of hundreds of microseconds.
/// </summary>
public record MaximumRelayResistanceGracePeriod : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8133;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MaximumRelayResistanceGracePeriod(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaximumRelayResistanceGracePeriod Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MaximumRelayResistanceGracePeriod Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaximumRelayResistanceGracePeriod Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MaximumRelayResistanceGracePeriod(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator RelaySeconds(MaximumRelayResistanceGracePeriod value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}