using Play.Codecs;
using Play.Codecs.Strings;

namespace Play.Interchange.DataFields.ValueObjects;

public class BitmapMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(1);
    public static readonly PlayEncodingId PlayEncodingId = Binary.PlayEncodingId;
    private const byte _ByteLength = 8;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}