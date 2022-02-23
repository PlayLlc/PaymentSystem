using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic.Fields;

internal class ServiceRestrictionCodeCodec : FixedLengthCodec
{
    #region Static Metadata

    /// <remarks>DecimalValue: 40</remarks>
    public static readonly DataFieldId DataFieldId = new(40);

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const ushort _ByteLength = 3;

    #endregion

    #region Instance Members

    protected override ushort GetByteLength() => _ByteLength;
    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion
}