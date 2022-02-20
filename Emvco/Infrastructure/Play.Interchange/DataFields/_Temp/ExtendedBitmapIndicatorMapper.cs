using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class ExtendedBitmapIndicatorMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 65</remarks>
    public static readonly DataFieldId DataFieldId = new(65);

    public static readonly PlayEncodingId PlayEncodingId = Binary.PlayEncodingId;
    private const ushort _ByteLength = 1;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}