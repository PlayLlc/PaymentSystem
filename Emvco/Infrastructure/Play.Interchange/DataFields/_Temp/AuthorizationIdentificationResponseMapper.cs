using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class AuthorizationIdentificationResponseMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 38</remarks>
    public static readonly DataFieldId DataFieldId = new(38);

    public static readonly PlayEncodingId PlayEncodingId = AlphaNumeric.PlayEncodingId;
    private const ushort _ByteLength = 6;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}