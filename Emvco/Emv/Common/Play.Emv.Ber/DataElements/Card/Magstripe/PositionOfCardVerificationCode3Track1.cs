using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     PCVC3(Track1) indicates to the Kernel the positions in the discretionary data field of the Track 1 Data where the
///     CVC3 (Track1) digits must be copied.
/// </summary>
public record PositionOfCardVerificationCode3Track1 : DataElement<ulong>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F62;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public PositionOfCardVerificationCode3Track1(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PositionOfCardVerificationCode3Track1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PositionOfCardVerificationCode3Track1 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PositionOfCardVerificationCode3Track1 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new PositionOfCardVerificationCode3Track1(result);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion
}