using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class RetrievalReferenceNumberCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 37</remarks>
    public static readonly DataFieldId DataFieldId = new(37);

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const ushort _ByteLength = 12;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}