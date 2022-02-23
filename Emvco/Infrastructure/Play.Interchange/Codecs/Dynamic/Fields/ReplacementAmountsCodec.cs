using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class ReplacementAmountsCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 95</remarks>
    public static readonly DataFieldId DataFieldId = new(95);

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const ushort _ByteLength = 42;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}