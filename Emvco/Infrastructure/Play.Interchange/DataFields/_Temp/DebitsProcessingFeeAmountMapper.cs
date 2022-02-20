using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class DebitsProcessingFeeAmountMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 84</remarks>
    public static readonly DataFieldId DataFieldId = new(84);

    public static readonly PlayEncodingId PlayEncodingId = Numeric.PlayEncodingId;
    private const ushort _ByteLength = 6;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}