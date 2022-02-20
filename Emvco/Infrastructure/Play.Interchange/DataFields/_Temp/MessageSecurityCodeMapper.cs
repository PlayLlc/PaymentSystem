using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields._Temp;

public class MessageSecurityCodeMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    /// <remarks>DecimalValue: 96</remarks>
    public static readonly DataFieldId DataFieldId = new(96);

    public static readonly PlayEncodingId PlayEncodingId = Binary.PlayEncodingId;
    private const ushort _ByteLength = 64;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}