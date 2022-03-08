using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record SystemTraceAuditNumber : InterchangeDataElement<uint>
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

    public static SystemTraceAuditNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static SystemTraceAuditNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        uint result = PlayCodec.NumericCodec.DecodeToUInt32(value);

        return new SystemTraceAuditNumber(result);
    }

    #endregion
}