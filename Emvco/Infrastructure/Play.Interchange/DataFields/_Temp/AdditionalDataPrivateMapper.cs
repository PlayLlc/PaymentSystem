using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class AdditionalDataPrivateMapper : VariableLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 48</remarks>
    public static readonly DataFieldId DataFieldId = new(48);

    public static readonly PlayEncodingId PlayEncodingId = AlphaNumeric.PlayEncodingId;
    private const ushort _MaxByteLength = 999;
    private const byte _LeadingOctetLength = 2;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}