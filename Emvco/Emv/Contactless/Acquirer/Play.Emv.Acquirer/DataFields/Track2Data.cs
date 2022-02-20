using Play.Interchange.Codecs;
using Play.Interchange.DataFields;

namespace Play.Emv.Acquirer.DataFields;

public record Track2Data : VariableDataField<byte[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 35</remarks>
    public static readonly DataFieldId DataFieldId = new(35);

    private const ushort _MaxByteCount = 37;
    private const byte _LeadingOctetByteCount = 1;

    #endregion

    #region Constructor

    public Track2Data(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => throw new NotImplementedException();
    protected override ushort GetMaxByteCount() => _MaxByteCount;
    protected override ushort GetLeadingOctetByteCount() => _LeadingOctetByteCount;

    #endregion

    #region Serialization

    public override Track2Data Decode(ReadOnlyMemory<byte> value) => throw new NotImplementedException();

    #endregion
}