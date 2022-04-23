using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Represents the threshold above which the Kernel considers the variation between Measured Relay Resistance
///     Processing Time and Min Time For Processing Relay Resistance APDU no longer acceptable. The Relay Resistance
///     Accuracy Threshold is expressed in units of hundreds of microseconds.
/// </summary>
public record RelayResistanceAccuracyThreshold : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8136;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public RelayResistanceAccuracyThreshold(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static RelayResistanceAccuracyThreshold Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override RelayResistanceAccuracyThreshold Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static RelayResistanceAccuracyThreshold Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new RelayResistanceAccuracyThreshold(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Operator Overrides

    public static explicit operator RelaySeconds(RelayResistanceAccuracyThreshold value) => value._Value;

    #endregion
}