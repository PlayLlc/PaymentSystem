using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class CardAcceptorNameLocationCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 43</remarks>
    public static readonly DataFieldId DataFieldId = new(43);

    public static readonly PlayEncodingId EncodingId = AlphaNumericSpecialCodec.EncodingId;
    private const ushort _ByteLength = 40;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}