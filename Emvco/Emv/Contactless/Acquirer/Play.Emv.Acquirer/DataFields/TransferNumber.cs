using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Metadata;
using Play.Emv.Acquirer.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record TransferNumber : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 78</remarks>
    public static readonly DataFieldId DataFieldId = new(78);

    public static readonly PlayEncodingId EncodingId = NumericDataFieldCodec.Identifier;
    private const ushort _ByteCount = 5;

    #endregion

    #region Constructor

    public TransferNumber(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override TransferNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new TransferNumber(result.Value);
    }

    #endregion
}