using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class CreditsProcessingFeeAmountCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 82</remarks>
    public static readonly DataFieldId DataFieldId = new(82);

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const ushort _ByteLength = 6;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}