using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields.ValueObjects;

namespace Play.Interchange.Messages.DataFields._Temp;

public class AccountIdentification1Mapper : VariableLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(102);
    public static readonly PlayEncodingId PlayEncodingId = AlphaNumericSpecial.PlayEncodingId;
    private const ushort _MaxByteLength = 28;
    private const byte _LeadingOctetLength = 1;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}