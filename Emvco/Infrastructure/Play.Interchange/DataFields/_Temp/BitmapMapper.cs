using Play.Codecs;
using Play.Codecs.Strings;

namespace Play.Interchange.Messages.DataFields._Temp;

public class BitmapMapper : FixedLengthDataFieldMapper
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = new(1);
    public static readonly PlayEncodingId PlayEncodingId = Binary.PlayEncodingId;
    private const byte _ByteLength = 64;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId;

    #endregion
}

// public class Track2DataMapper : VariableLengthDataFieldMapper { public static readonly DataFieldId DataFieldId = new(35); SHIIIIIIIIIIIIIIIIIIIIIIIIITprivate const ushort _MaxByteLength = 37; private const byte _LeadingOctetLength = 1; public override DataFieldId GetDataFieldId() => DataFieldId; public override PlayEncodingId GetPlayEncodingId() => PlayEncodingId; protected override ushort GetMaxByteLength() => _MaxByteLength; protected override ushort GetLeadingOctetLength() => _LeadingOctetLength; }