using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record MessageAuthenticationCodeMac : FixedDataField<ulong>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 64</remarks>
    public static readonly DataFieldId DataFieldId = new(64);

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const ushort _ByteCount = 8;

    #endregion

    #region Constructor

    public MessageAuthenticationCodeMac(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    /// <summary>
    /// Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="System.Exception"></exception>
    public override MessageAuthenticationCodeMac Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value.Span).ToUInt64Result()
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new MessageAuthenticationCodeMac(result.Value);
    }

    #endregion
}