using Play.Codecs;
using Play.Codecs.Strings;
using Play.Interchange.Messages.DataFields.ValueObjects;

namespace Play.Interchange.Messages.DataFields._Temp;

public class CardAcceptorTerminalIdentificationMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(41);
    public static readonly PlayEncodingId PlayEncodingId = AlphaNumericSpecial.PlayEncodingId;
    private const byte _ByteLength = 8;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}