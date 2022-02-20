using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class PersonalIdentificationNumberDataMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 52</remarks>
    public static readonly DataFieldId DataFieldId = new(52);

    public static readonly PlayEncodingId PlayEncodingId = Binary.PlayEncodingId;
    private const ushort _ByteLength = 8;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}