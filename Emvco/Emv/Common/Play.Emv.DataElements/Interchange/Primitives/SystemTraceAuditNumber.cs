using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs.Metadata;
using Play.Emv.Ber.Codecs;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Interchange;

public record SystemTraceAuditNumber : InterchangeDataElement<uint>
{
    #region Static Metadata

    /// <value>Hex: CB Decimal: 203; Interchange: 11</value>
    public static readonly Tag Tag = 0xCB;

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _MinValue = 1;
    private const int _MaxValue = 999999;
    private const int _ByteLength = 3;

    #endregion

    #region Constructor

    public SystemTraceAuditNumber(uint value) : base(value)
    {
        Check.Primitive.ForMinimumLength(value, _MinValue, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxValue, Tag);
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static SystemTraceAuditNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static SystemTraceAuditNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value).ToUInt32Result()
            ?? throw new DataElementNullException(BerEncodingId);

        return new SystemTraceAuditNumber(result.Value);
    }

    #endregion
}