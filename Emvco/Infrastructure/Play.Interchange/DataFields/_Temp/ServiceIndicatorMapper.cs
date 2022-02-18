using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields.ValueObjects;

namespace Play.Interchange.Messages.DataFields._Temp;

public class ServiceIndicatorMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(94);
    public static readonly PlayEncodingId PlayEncodingId = AlphaNumeric.PlayEncodingId;
    private const byte _ByteLength = 7;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}