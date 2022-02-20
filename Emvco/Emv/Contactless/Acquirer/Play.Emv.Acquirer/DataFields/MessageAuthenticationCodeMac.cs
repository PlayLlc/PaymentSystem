using Play.Ber.InternalFactories;
using Play.Emv.Interchange.Codecs;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record MessageAuthenticationCodeMac : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 64</remarks>
    public static readonly DataFieldId DataFieldId = new(64);

    public static readonly InterchangeEncodingId EncodingId = BinaryDataFieldCodec.Identifier;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public MessageAuthenticationCodeMac(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public override MessageAuthenticationCodeMac Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new MessageAuthenticationCodeMac(result.Value);
    }

    #endregion
}