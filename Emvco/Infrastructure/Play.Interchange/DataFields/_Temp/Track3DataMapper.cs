using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class Track3DataMapper : VariableLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 36</remarks>
    public static readonly DataFieldId DataFieldId = new(36);

    public static readonly PlayEncodingId PlayEncodingId = Numeric.PlayEncodingId;
    private const ushort _MaxByteLength = 52;
    private const byte _LeadingOctetLength = 2;

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;
    protected override ushort GetMaxByteLength() => _MaxByteLength;
    protected override ushort GetLeadingOctetLength() => _LeadingOctetLength;

    #endregion
}