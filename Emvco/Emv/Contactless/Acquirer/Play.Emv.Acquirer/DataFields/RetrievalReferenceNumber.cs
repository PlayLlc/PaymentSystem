using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record RetrievalReferenceNumber : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 37</remarks>
    public static readonly DataFieldId DataFieldId = new(37);

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const ushort _ByteCount = 12;

    #endregion

    #region Constructor

    public RetrievalReferenceNumber(char[] value) : base(value)
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
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    /// <exception cref="InterchangeException"></exception>
    public override RetrievalReferenceNumber Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new RetrievalReferenceNumber(result.Value);
    }

    #endregion
}