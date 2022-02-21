using Play.Codecs;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class TransactionDescriptionCodec : VariableLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 104</remarks>
    public static readonly DataFieldId DataFieldId = new(104);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const ushort _MaxByteLength = 100;
    private const byte _LeadingOctetLength = 2;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}