using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;
using Play.Emv.DataElements.Interchange;

namespace Play.Emv.Acquirer.Elavon.DataElements;

/// <summary>
///     The Elavon STAN will be returned to the POS or partner host system in the event of an approval of a sale
///     transaction. This data must be provided to the Elavon host in the event of a reversal of the aforementioned sale
///     transaction. This sub-field must be populated irrespective of whether single or dual message processing is used.
/// </summary>
public record ElavonSystemTraceAuditNumber : DataElement<SystemTraceAuditNumber>
{
    #region Static Metadata

    /// <value>Hex: 0x0002 Decimal: 2</value>
    public static readonly Tag Tag = 0x0002;

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    public ElavonSystemTraceAuditNumber(SystemTraceAuditNumber value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ElavonSystemTraceAuditNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ElavonSystemTraceAuditNumber Decode(ReadOnlySpan<byte> value)
    {
        const byte charLength = 6;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForMaxCharLength(result.Value.GetNumberOfDigits(), charLength, Tag);

        return new ElavonSystemTraceAuditNumber(new SystemTraceAuditNumber(result.Value));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, (uint) _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(ElavonSystemTraceAuditNumber? x, ElavonSystemTraceAuditNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ElavonSystemTraceAuditNumber obj) => obj.GetHashCode();

    #endregion
}