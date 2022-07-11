using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

public record MaxTimeForProcessingRelayResistanceApdu : DataElement<RelaySeconds>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8304;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public MaxTimeForProcessingRelayResistanceApdu(RelaySeconds value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaxTimeForProcessingRelayResistanceApdu Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MaxTimeForProcessingRelayResistanceApdu Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MaxTimeForProcessingRelayResistanceApdu Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new MaxTimeForProcessingRelayResistanceApdu(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator RelaySeconds(MaxTimeForProcessingRelayResistanceApdu value) => value._Value;

    #endregion
}