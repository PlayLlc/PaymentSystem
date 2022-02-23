using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class PrimaryAccountNumberPanCodec : VariableLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 2</remarks>
    public static readonly DataFieldId DataFieldId = new(2);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _MaxByteLength = 10;
    private const byte _LeadingOctetLength = 1;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}