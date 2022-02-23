using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class PersonalIdentificationNumberDataCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 52</remarks>
    public static readonly DataFieldId DataFieldId = new(52);

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const ushort _ByteLength = 8;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}