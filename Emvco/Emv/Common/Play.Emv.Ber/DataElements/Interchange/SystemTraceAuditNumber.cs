using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public record SystemTraceAuditNumber : PlayProprietaryDataElement<uint>
{
    #region Static Metadata

    /// <value>Hex: CB Decimal: 203; Interchange: 11</value>
    public static readonly Tag Tag = 0xCB;

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _MinValue = 1;
    private const int _MaxValue = 999999;
    private const int _ByteLength = 3;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public SystemTraceAuditNumber(uint value) : base(value)
    {
        Check.Primitive.ForMinimumLength(value, _MinValue, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxValue, Tag);
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static SystemTraceAuditNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override SystemTraceAuditNumber Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static SystemTraceAuditNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        Check.Primitive.ForMaximumValue(result, _MaxValue, Tag);

        return new SystemTraceAuditNumber(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion
}