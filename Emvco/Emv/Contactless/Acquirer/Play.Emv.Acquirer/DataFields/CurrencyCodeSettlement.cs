using Play.Codecs;
using Play.Emv.Acquirer.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Emv.Acquirer.DataFields;

public record CurrencyCodeSettlement : FixedDataField<char[]>
{
    #region Static Metadata

    /// <remarks>DecimalValue: 50</remarks>
    public static readonly DataFieldId DataFieldId = new(50);

    public static readonly PlayEncodingId EncodingId = AlphabeticCodec.EncodingId;
    private const ushort _ByteCount = 2;

    #endregion

    #region Constructor

    public CurrencyCodeSettlement(char[] value) : base(value)
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
    /// <exception cref="InterchangeDataFieldNullException"></exception>
    public override CurrencyCodeSettlement Decode(ReadOnlyMemory<byte> value)
    {
        Check.DataField.ForExactLength(value, _ByteCount, DataFieldId);
        DecodedResult<char[]> result = _Codec.Decode(EncodingId, value.Span) as DecodedResult<char[]>
            ?? throw new InterchangeDataFieldNullException(EncodingId);

        return new CurrencyCodeSettlement(result.Value);
    }

    #endregion
}