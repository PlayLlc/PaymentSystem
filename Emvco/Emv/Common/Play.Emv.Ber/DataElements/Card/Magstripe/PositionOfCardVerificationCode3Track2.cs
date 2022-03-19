using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     PCVC3(Track2) indicates to the Kernel the positions in the discretionary data field of the Track 2 Data where the
///     CVC3 (Track2) digits must be copied.
/// </summary>
public record PositionOfCardVerificationCode3Track2 : DataElement<ulong>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F65;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public PositionOfCardVerificationCode3Track2(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PositionOfCardVerificationCode3Track2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PositionOfCardVerificationCode3Track2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new PositionOfCardVerificationCode3Track2(result);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion
}