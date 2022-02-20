using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class TransactionDescriptionMapper : VariableLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 104</remarks>
    public static readonly DataFieldId DataFieldId = new(104);

    public static readonly PlayEncodingId PlayEncodingId = AlphaNumericSpecial.PlayEncodingId;
    private const ushort _MaxByteLength = 100;
    private const byte _LeadingOctetLength = 2;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}