using Play.Codecs;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class PrimaryAccountNumberExtendedCodec : VariableLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 34</remarks>
    public static readonly DataFieldId DataFieldId = new(34);

    public static readonly PlayEncodingId EncodingId = NumericSpecialCodec.EncodingId;
    private const ushort _MaxByteLength = 28;
    private const byte _LeadingOctetLength = 1;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}